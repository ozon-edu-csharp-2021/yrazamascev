using Dapper;

using Npgsql;

using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;
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
        private readonly IChangeTracker _changeTracker;
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;

        public MerchOrderRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory, IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
        }

        public async Task<MerchOrder> Create(MerchOrder itemToCreate, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO merch_order (
                    merch_pack_type_id
                    ,merch_order_status_id
                    ,merch_request_type_id
                    ,in_work_at
                    ,reserve_at
                    ,done_at
                    ,employee_id
                )
                OUTPUT INSERTED.Id
                VALUES (
                    @PackType
                    @Status
                    @RequestType
                    @InWorkAt
                    @ReserveAt
                    @DoneAt
                    @EmployeeId
                );";

            var parameters = new
            {
                PackType = itemToCreate.PackType.Id,
                Status = itemToCreate.Status.Id,
                RequestType = itemToCreate.RequestType.Id,
                InWorkAt = itemToCreate.InWorkAt.Value,
                ReserveAt = itemToCreate.ReserveAt.Value,
                DoneAt = itemToCreate.DoneAt.Value,
                itemToCreate.EmployeeId
            };

            CommandDefinition commandDefinition = new(
                sql,
                parameters: parameters,
                commandTimeout: TIMEOUT,
                cancellationToken: cancellationToken);

            NpgsqlConnection connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            long id = await connection.QuerySingleAsync<long>(commandDefinition);
            itemToCreate.SetId(id);

            _changeTracker.Track(itemToCreate);

            return itemToCreate;
        }

        public async Task<List<MerchOrder>> FindByEmployeeId(long employeeId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT merch_order.id
                    ,merch_pack_type.id
                    ,merch_pack_type.name
                    ,merch_order_status.id
                    ,merch_order_status.name
                    ,merch_request_type.id
                    ,merch_request_type.name
                    ,merch_order.in_work_at
                    ,merch_order.reserve_at
                    ,merch_order.done_at
                    ,merch_order.employee_id
                FROM merch_order
                JOIN merch_pack_type ON merch_pack_type.id = merch_order.merch_pack_type_id
                JOIN merch_order_status ON merch_order_status.id = merch_order.merch_order_status_id
                JOIN merch_request_type ON merch_request_type.id = merch_order.merch_request_type_id
                WHERE merch_order.employee_id = @EmployeeId ;
                SELECT sku_pack.id
                    ,sku_pack.merch_order_id
                    ,sku_pack.sku_id
                    ,sku_pack.quantity
                FROM sku_pack
                WHERE sku_pack.merch_order_id IN (
                        SELECT id
                        FROM merch_order
                        WHERE employee_id = @EmployeeId) ;";

            var parameters = new
            {
                EmployeeId = employeeId,
            };

            return await FindMerch(sql, parameters, cancellationToken);
        }

        public async Task<List<MerchOrder>> FindIssuedMerch(long employeeId, int merchPackId, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT merch_order.id
                    ,merch_pack_type.id
                    ,merch_pack_type.name
                    ,merch_order_status.id
                    ,merch_order_status.name
                    ,merch_request_type.id
                    ,merch_request_type.name
                    ,merch_order.in_work_at
                    ,merch_order.reserve_at
                    ,merch_order.done_at
                    ,merch_order.employee_id
                FROM merch_order
                JOIN merch_pack_type ON merch_pack_type.id = merch_order.merch_pack_type_id
                JOIN merch_order_status ON merch_order_status.id = merch_order.merch_order_status_id
                JOIN merch_request_type ON merch_request_type.id = merch_order.merch_request_type_id
                WHERE merch_order.Employee_id = @EmployeeId AND merch_pack_type.Id = @MerchPackTypeId ;
                SELECT sku_pack.id
                    ,sku_pack.merch_order_id
                    ,sku_pack.sku_id
                    ,sku_pack.quantity
                FROM sku_pack
                WHERE sku_pack.merch_order_id IN (
                        SELECT id
                        FROM merch_order
                        WHERE employee_id = @EmployeeId AND merch_pack_type_id = @MerchPackTypeId) ;";

            var parameters = new
            {
                EmployeeId = employeeId,
                MerchPackTypeId = merchPackId,
            };

            return await FindMerch(sql, parameters, cancellationToken);
        }

        private async Task<List<MerchOrder>> FindMerch(string sql, object parameters, CancellationToken cancellationToken = default)
        {
            NpgsqlConnection connection = await _dbConnectionFactory.CreateConnection(cancellationToken);

            CommandDefinition commandDefinition = new(
                sql,
                parameters: parameters,
                commandTimeout: TIMEOUT,
                cancellationToken: cancellationToken);

            GridReader reader = await connection.QueryMultipleAsync(commandDefinition);

            IEnumerable<Models.MerchOrder> merchOrderModels = reader
                .Map<Models.MerchOrder, Models.SkuPack, long>
                (
                    merchOrder => merchOrder.Id,
                    skuPack => skuPack.MerchOrderId,
                    (merchOrder, skuPacks) => merchOrder.SkuPackCollection = skuPacks
                );

            List<MerchOrder> merchOrders = merchOrderModels
                .Map(model => ModelsMapper.MerchOrderModelToEntity(model))
                .ToList();

            if (merchOrders.Count > 0)
            {
                _changeTracker.Track(merchOrders.First());
            }

            return merchOrders;
        }
    }
}