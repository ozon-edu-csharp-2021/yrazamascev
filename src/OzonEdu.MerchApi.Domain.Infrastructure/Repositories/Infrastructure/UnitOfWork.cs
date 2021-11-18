using MediatR;

using Npgsql;

using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure.Exceptions;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IChangeTracker _changeTracker;
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory = null;
        private readonly IPublisher _publisher;
        private NpgsqlTransaction _npgsqlTransaction;

        public UnitOfWork(
            IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory,
            IPublisher publisher,
            IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _publisher = publisher;
            _changeTracker = changeTracker;
        }

        void IDisposable.Dispose()
        {
            _npgsqlTransaction?.Dispose();
            _dbConnectionFactory?.Dispose();
        }

        public async Task SaveChanges(CancellationToken token)
        {
            if (_npgsqlTransaction is null)
            {
                throw new NoActiveTransactionStartedException();
            }

            Queue<INotification> domainEvents = new Queue<INotification>(
                _changeTracker.TrackedEntities
                    .SelectMany(x =>
                    {
                        List<INotification> events = x.DomainEvents.ToList();
                        x.ClearDomainEvents();

                        return events;
                    }));

            while (domainEvents.TryDequeue(out INotification notification))
            {
                await _publisher.Publish(notification, token);
            }

            await _npgsqlTransaction.CommitAsync(token);
        }

        public async ValueTask StartTransaction(CancellationToken token)
        {
            if (_npgsqlTransaction is not null)
            {
                return;
            }

            NpgsqlConnection connection = await _dbConnectionFactory.CreateConnection(token);
            _npgsqlTransaction = await connection.BeginTransactionAsync(token);
        }
    }
}