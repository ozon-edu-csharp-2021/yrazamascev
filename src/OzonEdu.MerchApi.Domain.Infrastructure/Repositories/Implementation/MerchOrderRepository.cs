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
                INSERT INTO MerchOrder (
                    MerchPackType_id
                    ,MerchOrderStatus_id
                    ,MerchRequestType_id
                    ,InWorkAt
                    ,ReserveAt
                    ,DoneAt
                    ,Employee_id
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
                SELECT
                    MerchOrder.Id

                    ,MerchPackType.Id
                    ,MerchPackType.Name

                    ,MerchOrderStatus.Id
                    ,MerchOrderStatus.Name

                    ,MerchRequestType.Id
                    ,MerchRequestType.Name

                    ,MerchOrder.InWorkAt
                    ,MerchOrder.ReserveAt
                    ,MerchOrder.DoneAt
                    ,MerchOrder.Employee_id

                FROM MerchOrder
                JOIN MerchPackType ON MerchPackType.Id = MerchOrder.MerchPackType_id
                JOIN MerchOrderStatus ON MerchOrderStatus.Id = MerchOrder.MerchOrderStatus_id
                JOIN MerchRequestType ON MerchRequestType.Id = MerchOrder.MerchRequestType_id
                WHERE MerchOrder.Employee_id = @EmployeeId

                SELECT
                    SkuPack.Id
                    ,SkuPack.MerchOrder_id
                    ,SkuPack.Sku_id
                    ,SkuPack.Quantity

                FROM SkuPack
                WHERE SkuPack.MerchOrder_id IN (
                        SELECT Id
                        FROM MerchOrder
                        WHERE Employee_id = @EmployeeId);";

            var parameters = new
            {
                EmployeeId = employeeId,
            };

            return await FindMerch(sql, parameters, cancellationToken);
        }

        public async Task<List<MerchOrder>> FindIssuedMerch(long employeeId, int merchPackId, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT
                    MerchOrder.Id

                    ,MerchPackType.Id
                    ,MerchPackType.Name

                    ,MerchOrderStatus.Id
                    ,MerchOrderStatus.Name

                    ,MerchRequestType.Id
                    ,MerchRequestType.Name

                    ,MerchOrder.InWorkAt
                    ,MerchOrder.ReserveAt
                    ,MerchOrder.DoneAt
                    ,MerchOrder.Employee_id

                FROM MerchOrder
                JOIN MerchPackType ON MerchPackType.Id = MerchOrder.MerchPackType_id
                JOIN MerchOrderStatus ON MerchOrderStatus.Id = MerchOrder.MerchOrderStatus_id
                JOIN MerchRequestType ON MerchRequestType.Id = MerchOrder.MerchRequestType_id
                WHERE MerchOrder.Employee_id = @EmployeeId AND MerchPackType.Id = @MerchPackTypeId

                SELECT
                    SkuPack.Id
                    ,SkuPack.MerchOrder_id
                    ,SkuPack.Sku_id
                    ,SkuPack.Quantity

                FROM SkuPack
                WHERE SkuPack.MerchOrder_id IN (
                        SELECT Id
                        FROM MerchOrder
                        WHERE Employee_id = @EmployeeId AND MerchPackType_id = @MerchPackTypeId);";

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