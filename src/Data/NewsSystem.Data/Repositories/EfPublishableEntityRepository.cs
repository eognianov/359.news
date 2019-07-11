using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsSystem.Data.Common.Models;
using NewsSystem.Data.Common.Repositories;

namespace NewsSystem.Data.Repositories
{
    public class EfPublishableEntityRepository<TEntity>:EfDeletableEntityRepository<TEntity>, IPublishableEntityRepository<TEntity> where TEntity : class, IDeletableEntity, IPublishableEntity
    {
        public EfPublishableEntityRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<TEntity> AllPublished()
        {
            return base.All().Where(x => x.isPublished);
        }

        public IQueryable<TEntity> AllNotPublished()
        {
            return base.All().Where(x => !x.isPublished);
        }

        public Task<TEntity> GetByIdWithNotPublishedAsync(params object[] id)
        {
            var byIdPredicate = EfExpressionHelper.BuildByIdPredicate<TEntity>(this.Context, id);

            return this.AllWithDeleted().FirstOrDefaultAsync(byIdPredicate);
        }

        public void Publish(TEntity entity)
        {
            entity.isPublished = true;
            entity.PublishedOn = DateTime.UtcNow;

            this.Update(entity);
        }

        public void Unpublish(TEntity entity)
        {
            entity.isPublished = false;
            entity.PublishedOn = null;

            this.Update(entity);
        }
    }
}
