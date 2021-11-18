using Npgsql;

using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Implementation
{
    public class MerchOrderRepository : IMerchOrderRepository
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;

        public MerchOrderRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory, IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
        }

        public IUnitOfWork UnitOfWork { get; }

        public Task<MerchOrder> Create(MerchOrder itemToCreate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<MerchOrder>> FindByEmployeeId(long employeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<MerchOrder> FindIssuedMerch(long employeeId, int merchPackId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<MerchOrder> Update(MerchOrder itemToUpdate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}