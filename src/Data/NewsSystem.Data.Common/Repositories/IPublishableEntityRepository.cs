

using NewsSystem.Data.Common.Models;
using System.Linq;
using System.Threading.Tasks;

namespace NewsSystem.Data.Common.Repositories
{
    public interface IPublishableEntityRepository<TEntity> : IRepository<TEntity>, IDeletableEntityRepository<TEntity>
        where TEntity : class, IDeletableEntity, IPublishableEntity
    {
        IQueryable<TEntity> AllPublished();

        IQueryable<TEntity> AllNotPublished();

        Task<TEntity> GetByIdWithNotPublishedAsync(params object[] id);

        void Publish(TEntity entity);

        void Unpublish(TEntity entity);
    }
}
