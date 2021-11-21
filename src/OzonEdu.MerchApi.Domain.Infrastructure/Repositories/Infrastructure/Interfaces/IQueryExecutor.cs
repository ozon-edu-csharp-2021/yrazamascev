using OzonEdu.MerchApi.Domain.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure.Interfaces
{
    public interface IQueryExecutor
    {
        Task<T> Execute<T>(T entity, Func<Task> method) where T : Entity;

        Task<T> Execute<T>(Func<Task<T>> method) where T : Entity;

        Task<IEnumerable<T>> Execute<T>(Func<Task<IEnumerable<T>>> method) where T : Entity;
    }
}