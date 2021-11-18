using OzonEdu.MerchApi.Domain.Models;

using System.Collections.Generic;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure.Interfaces
{
    public interface IChangeTracker
    {
        IEnumerable<Entity> TrackedEntities { get; }

        public void Track(Entity entity);
    }
}