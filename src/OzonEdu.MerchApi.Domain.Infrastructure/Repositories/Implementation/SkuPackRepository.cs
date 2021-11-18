using Npgsql;

using OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Implementation
{
    public class SkuPackRepository : ISkuPackRepository
    {
        private readonly IChangeTracker _changeTracker;
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        public IUnitOfWork UnitOfWork { get; }

        public SkuPackRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory, IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
        }

        public Task<SkuPack> Create(SkuPack itemToCreate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<SkuPack> Update(SkuPack itemToUpdate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}