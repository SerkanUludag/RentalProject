using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext>: IEntityRepository<TEntity>
        where TEntity: class, IEntity, new()
        where TContext:  DbContext, new()
    {
        public void Add(TEntity entity)
        {
            using (TContext rc = new TContext())
            {
                var addedEntity = rc.Entry(entity);
                addedEntity.State = EntityState.Added;
                rc.SaveChanges();
            }
        }

        public void Delete(TEntity entity)
        {
            using (TContext rc = new TContext())
            {
                var deletedEntity = rc.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                rc.SaveChanges();
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext rc = new TContext())
            {
                return filter == null ? rc.Set<TEntity>().ToList() : rc.Set<TEntity>().Where(filter).ToList();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext rc = new TContext())
            {
                return rc.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        public void Update(TEntity entity)
        {
            using (TContext rc = new TContext())
            {
                var updatedEntity = rc.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                rc.SaveChanges();
            }
        }
    }
}
