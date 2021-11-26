using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchApi.Domain.Models;

using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure
{
    public class ChangeTracker : IChangeTracker
    {
        private readonly ConcurrentBag<Entity> _usedEntitiesBackingField;

        public IEnumerable<Entity> TrackedEntities => _usedEntitiesBackingField.ToArray();

        public ChangeTracker() => _usedEntitiesBackingField = new ConcurrentBag<Entity>();

        public void Track(Entity entity)
        {
            _usedEntitiesBackingField.Add(entity);
        }
    }
}