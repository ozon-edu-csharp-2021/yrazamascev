using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchApi.Domain.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IChangeTracker _changeTracker;

        public QueryExecutor(IChangeTracker changeTracker) => _changeTracker = changeTracker;

        public async Task<T> Execute<T>(T entity, Func<Task> method) where T : Entity
        {
            await method();
            _changeTracker.Track(entity);

            return entity;
        }

        public async Task<T> Execute<T>(Func<Task<T>> method) where T : Entity
        {
            T result = await method();
            _changeTracker.Track(result);

            return result;
        }

        public async Task<IEnumerable<T>> Execute<T>(Func<Task<IEnumerable<T>>> method) where T : Entity
        {
            List<T> result = (await method()).ToList();
            foreach (T entity in result)
            {
                _changeTracker.Track(entity);
            }

            return result;
        }
    }
}