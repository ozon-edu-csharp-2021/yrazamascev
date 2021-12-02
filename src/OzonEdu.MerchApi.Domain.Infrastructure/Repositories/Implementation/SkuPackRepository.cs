using Dapper;

using Microsoft.Extensions.Options;

using Npgsql;

using OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate;
using OzonEdu.MerchApi.Domain.Infrastructure.Configuration;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure.Interfaces;

using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Implementation
{
    public class SkuPackRepository : ISkuPackRepository
    {
        private const int TIMEOUT = 5;
        private readonly DatabaseConnectionOptions _options;
        private readonly IQueryExecutor _queryExecutor;

        public SkuPackRepository(IOptions<DatabaseConnectionOptions> options, IQueryExecutor queryExecutor)
        {
            _options = options.Value;
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

            using NpgsqlConnection connection = new(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

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