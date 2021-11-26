using Dapper;

using Npgsql;

using OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure.Interfaces;

using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Implementation
{
    public class SkuPackRepository : ISkuPackRepository
    {
        private const int TIMEOUT = 5;
        private readonly IQueryExecutor _queryExecutor;
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;

        public SkuPackRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory, IQueryExecutor queryExecutor)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _queryExecutor = queryExecutor;
        }

        public async Task<SkuPack> Create(SkuPack skuPack, long merchOrderId, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO sku_pack (
                    merch_order_id
                    ,sku_id
                    ,quantity
                )
                OUTPUT INSERTED.Id
                VALUES (
                    @MerchOrder_id
                    @Sku_id
                    @Quantity
                );";

            var parameters = new
            {
                MerchOrderId = merchOrderId,
                SkuId = skuPack.Sku.Value,
                Quantity = skuPack.Quantity.Value,
            };

            using NpgsqlConnection connection = await _dbConnectionFactory.CreateConnection(cancellationToken);

            CommandDefinition commandDefinition = new(
                sql,
                parameters: parameters,
                commandTimeout: TIMEOUT,
                cancellationToken: cancellationToken);

            return await _queryExecutor.Execute(skuPack, async () =>
            {
                long id = await connection.QuerySingleAsync<long>(commandDefinition);
                skuPack.SetId(id);
            });
        }
    }
}