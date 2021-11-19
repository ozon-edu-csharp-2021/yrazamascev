using Dapper;

using Npgsql;

using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure.Interfaces;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Implementation
{
    public class SkuPackRepository : ISkuPackRepository
    {
        private const int TIMEOUT = 5;
        private readonly IChangeTracker _changeTracker;
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;

        public SkuPackRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory, IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
        }

        public async Task<List<SkuPack>> Create(MerchOrder merchOrder, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO SkuPack (
                    MerchOrder_id
                    ,Sku_id
                    ,Quantity
                )
                OUTPUT INSERTED.Id
                VALUES (
                    @MerchOrder_id
                    @Sku_id
                    @Quantity
                );";

            foreach (SkuPack skuPack in merchOrder.SkuPackCollection)
            {
                var parameters = new
                {
                    MerchOrderId = merchOrder.Id,
                    SkuId = skuPack.Sku.Value,
                    Quantity = skuPack.Quantity.Value,
                };

                CommandDefinition commandDefinition = new(
                    sql,
                    parameters: parameters,
                    commandTimeout: TIMEOUT,
                    cancellationToken: cancellationToken);

                NpgsqlConnection connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
                long id = await connection.QuerySingleAsync<long>(commandDefinition);
                skuPack.SetId(id);

                _changeTracker.Track(skuPack);
            }

            return merchOrder.SkuPackCollection.ToList();
        }
    }
}