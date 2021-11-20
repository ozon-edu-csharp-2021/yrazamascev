using Dapper;

using Npgsql;

using OzonEdu.MerchApi.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Extension;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Helpers;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure.Interfaces;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using static Dapper.SqlMapper;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Implementation
{
    public class MerchPackRepository : IMerchPackRepository
    {
        private const int TIMEOUT = 5;
        private readonly IChangeTracker _changeTracker;
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;

        public MerchPackRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory, IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
        }

        public async Task<MerchPack> FindByType(MerchPackType merchPackType, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT merch_pack.id
                    ,merch_pack.merch_pack_type_id
                FROM merch_pack
                JOIN merch_pack_type ON merch_pack_type.id = merch_pack.merch_pack_type_id
                WHERE merch_pack_type.id = @MerchPackTypeId ;
                SELECT item_pack.id
                    ,item_pack.merch_pack_id
                    ,item_pack.stock_item_id
                    ,item_pack.quantity
                FROM item_pack
                WHERE item_pack.merch_pack_id IN (
                        SELECT id
                        FROM merch_pack
                        WHERE merch_pack_type_id = @MerchPackTypeId) ;";

            var parameters = new
            {
                MerchPackTypeId = merchPackType.Id,
            };

            NpgsqlConnection connection = await _dbConnectionFactory.CreateConnection(cancellationToken);

            CommandDefinition commandDefinition = new(
                sql,
                parameters: parameters,
                commandTimeout: TIMEOUT,
                cancellationToken: cancellationToken);

            GridReader reader = await connection.QueryMultipleAsync(commandDefinition);

            Models.MerchPack merchPackModel = reader
                .Map<Models.MerchPack, Models.ItemPack, long>
                (
                    merchPack => merchPack.Id,
                    itemPack => itemPack.MerchPackId,
                    (merchPack, itemPacks) => merchPack.ItemPackCollection = itemPacks
                ).FirstOrDefault();

            MerchPack merchPack = ModelsMapper.MerchPackModelToEntity(merchPackModel);

            if (merchPack is not null)
            {
                _changeTracker.Track(merchPack);
            }

            return merchPack;
        }
    }
}