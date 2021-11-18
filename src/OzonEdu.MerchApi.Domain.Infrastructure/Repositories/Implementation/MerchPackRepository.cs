using Npgsql;

using OzonEdu.MerchApi.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Implementation
{
    public class MerchPackRepository : IMerchPackRepository
    {
        private readonly IChangeTracker _changeTracker;
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        public IUnitOfWork UnitOfWork { get; }

        public MerchPackRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory, IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
        }

        public Task<MerchPack> Create(MerchPack itemToCreate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<MerchPack> FindByType(MerchPackType merchPackType, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<MerchPack> Update(MerchPack itemToUpdate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}