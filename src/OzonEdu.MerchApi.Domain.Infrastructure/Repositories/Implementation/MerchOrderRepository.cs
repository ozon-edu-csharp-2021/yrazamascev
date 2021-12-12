using CSharpCourse.Core.Lib.Enums;

using Dapper;

using Microsoft.Extensions.Options;

using Npgsql;

using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;
using OzonEdu.MerchApi.Domain.Infrastructure.Configuration;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Extension;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Helpers;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchApi.Infrastructure.Extensions;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using static Dapper.SqlMapper;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Implementation
{
    public class MerchOrderRepository : IMerchOrderRepository
    {
        private const int TIMEOUT = 5;
        private readonly DatabaseConnectionOptions _options;
        private readonly IQueryExecutor _queryExecutor;

        public MerchOrderRepository(IOptions<DatabaseConnectionOptions> options, IQueryExecutor queryExecutor)
        {
            _options = options.Value;
            _queryExecutor = queryExecutor;
        }

        public async Task<MerchOrder> Create(MerchOrder itemToCreate, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO merch_order (
                    merch_pack_type_id
                    ,merch_order_status_id
                    ,merch_request_type_id
                    ,in_work_at
                    ,done_at
                    ,employee_email
                )
                VALUES (
                    @PackType
                    @Status
                    @RequestType
                    @InWorkAt
                    @DoneAt
                    @EmployeeEmail
                )
                RETURNING merch_order.id ;";

            var parameters = new
            {
                PackType = itemToCreate.PackType.Id,
                Status = itemToCreate.Status.Id,
                RequestType = itemToCreate.RequestType.Id,
                InWorkAt = itemToCreate.InWorkAt.Value,
                DoneAt = itemToCreate.DoneAt.Value,
                EmployeeEmail = itemToCreate.EmployeeEmail
            };

            using NpgsqlConnection connection = new(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            CommandDefinition commandDefinition = new(
                sql,
                parameters: parameters,
                commandTimeout: TIMEOUT,
                cancellationToken: cancellationToken);

            return await _queryExecutor.Execute(itemToCreate, async () =>
            {
                long id = await connection.QuerySingleAsync<long>(commandDefinition);
                itemToCreate.SetId(id);
            });
        }

        public async Task<MerchOrder> Update(MerchOrder itemToUpdate, CancellationToken cancellationToken)
        {
            using NpgsqlConnection connection = new(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            return await _queryExecutor.Execute(itemToUpdate, async () =>
                await connection.ExecuteAsync(
                    new CommandDefinition(
                        commandText: @"
                            UPDATE merch_order
                            SET merch_order_status_id = @StatusId, done_at = @DoneAt
                            WHERE id = @MerchOrderId",
                        parameters: new
                        {
                            StatusId = itemToUpdate.Status.Id,
                            DoneAt = itemToUpdate.DoneAt?.Value,
                            MerchOrderId = itemToUpdate.Id,
                        },
                        commandTimeout: TIMEOUT,
                        cancellationToken: cancellationToken)));
        }

        public async Task<IReadOnlyCollection<MerchOrder>> FindByEmployeeId(string employeeEmail, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT merch_order.id
                    ,merch_pack_type.id as pack_type_id
                    ,merch_pack_type.name as pack_type_name
                    ,merch_order_status.id as status_id
                    ,merch_order_status.name as status_name
                    ,merch_request_type.id as request_type_id
                    ,merch_request_type.name as request_type_name
                    ,merch_order.in_work_at
                    ,merch_order.done_at
                    ,merch_order.employee_email
                FROM merch_order
                JOIN merch_pack_type ON merch_pack_type.id = merch_order.merch_pack_type_id
                JOIN merch_order_status ON merch_order_status.id = merch_order.merch_order_status_id
                JOIN merch_request_type ON merch_request_type.id = merch_order.merch_request_type_id
                WHERE merch_order.employee_email = @EmployeeEmail ;
                SELECT sku_pack.id
                    ,sku_pack.merch_order_id
                    ,sku_pack.sku_id
                    ,sku_pack.quantity
                FROM sku_pack
                WHERE sku_pack.merch_order_id IN (
                        SELECT id
                        FROM merch_order
                        WHERE employee_email = @EmployeeEmail) ;";

            var parameters = new
            {
                EmployeeEmail = employeeEmail,
            };

            return await FindMerch(sql, parameters, cancellationToken);
        }

        public async Task<IReadOnlyCollection<MerchOrder>> FindIssuedMerch(string employeeEmail, MerchType merchType, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT merch_order.id
                    ,merch_pack_type.id as pack_type_id
                    ,merch_pack_type.name as pack_type_name
                    ,merch_order_status.id as status_id
                    ,merch_order_status.name as status_name
                    ,merch_request_type.id as request_type_id
                    ,merch_request_type.name as request_type_name
                    ,merch_order.in_work_at
                    ,merch_order.done_at
                    ,merch_order.employee_email
                FROM merch_order
                JOIN merch_pack_type ON merch_pack_type.id = merch_order.merch_pack_type_id
                JOIN merch_order_status ON merch_order_status.id = merch_order.merch_order_status_id
                JOIN merch_request_type ON merch_request_type.id = merch_order.merch_request_type_id
                WHERE merch_order.Employee_id = @EmployeeEmail AND merch_pack_type.Id = @MerchPackTypeId ;
                SELECT sku_pack.id
                    ,sku_pack.merch_order_id
                    ,sku_pack.sku_id
                    ,sku_pack.quantity
                FROM sku_pack
                WHERE sku_pack.merch_order_id IN (
                        SELECT id
                        FROM merch_order
                        WHERE employee_email = @EmployeeEmail AND merch_pack_type_id = @MerchPackTypeId) ;";

            var parameters = new
            {
                EmployeeEmail = employeeEmail,
                MerchPackTypeId = (int)merchType,
            };

            return await FindMerch(sql, parameters, cancellationToken);
        }

        public async Task<IReadOnlyCollection<MerchOrder>> FindInWork(IReadOnlyCollection<long> skus, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT merch_order.id
                    ,merch_pack_type.id as pack_type_id
                    ,merch_pack_type.name as pack_type_name
                    ,merch_order_status.id as status_id
                    ,merch_order_status.name as status_name
                    ,merch_request_type.id as request_type_id
                    ,merch_request_type.name as request_type_name
                    ,merch_order.in_work_at
                    ,merch_order.done_at
                    ,merch_order.employee_email
                FROM merch_order
                JOIN merch_pack_type ON merch_pack_type.id = merch_order.merch_pack_type_id
                JOIN merch_order_status ON merch_order_status.id = merch_order.merch_order_status_id
                JOIN merch_request_type ON merch_request_type.id = merch_order.merch_request_type_id
                WHERE merch_order_status_id = @StatusId
                        AND merch_order.id IN ( SELECT DISTINCT merch_order_id
                                                FROM sku_pack
                                                WHERE sku_id = ANY(@SkuIds) ) ;
                SELECT sku_pack.id
                    ,sku_pack.merch_order_id
                    ,sku_pack.sku_id
                    ,sku_pack.quantity
                FROM sku_pack
                WHERE sku_pack.merch_order_id IN (  SELECT DISTINCT merch_order_id
                                                    FROM sku_pack
                                                    WHERE sku_id = ANY(@SkuIds) )
                        AND sku_pack.merch_order_id IN (    SELECT id
                                                            FROM merch_order
                                                            WHERE merch_order_status_id = @StatusId ) ;";

            var parameters = new
            {
                StatusId = MerchOrderStatus.InWork.Id,
                SkuIds = skus.ToArray(),
            };

            return await FindMerch(sql, parameters, cancellationToken);
        }

        private async Task<IReadOnlyCollection<MerchOrder>> FindMerch(string sql, object parameters, CancellationToken cancellationToken)
        {
            using NpgsqlConnection connection = new(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            CommandDefinition commandDefinition = new(
                sql,
                parameters: parameters,
                commandTimeout: TIMEOUT,
                cancellationToken: cancellationToken);

            IEnumerable<MerchOrder> result = await _queryExecutor.Execute(async () =>
            {
                using GridReader reader = await connection.QueryMultipleAsync(commandDefinition);

                IEnumerable<Models.MerchOrder> merchOrderModels = reader
                    .Map<Models.MerchOrder, Models.SkuPack, long>
                    (
                        merchOrder => merchOrder.Id,
                        skuPack => skuPack.MerchOrderId,
                        (merchOrder, skuPacks) => merchOrder.SkuPackCollection = skuPacks
                    );

                return merchOrderModels
                    .Map(model => ModelsMapper.MerchOrderModelToEntity(model));
            });

            return result.ToList();
        }
    }
}