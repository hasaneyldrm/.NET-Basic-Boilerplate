using BasicBoilerplate.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BasicBoilerplate.Services
{
    public class GeneralService<TEntity> : IRepositoryService<TEntity> where TEntity : class
    {
        internal AppDbContext Context;
        internal DbSet<TEntity> DbSet;

        public GeneralService(AppDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public IHttpContextAccessor Accessor { get; set; }


        public virtual TEntity Get(int id)
        {
            return DbSet.Find(id);
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null) query = query.Where(filter);

            if (includeProperties != null)
                foreach (var includeProperty in includeProperties.Split
                    (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);


            if (orderBy != null)
                return orderBy(query).ToList();
            return query.ToList();
        }

        public virtual void Insert(TEntity entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            var entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached) DbSet.Attach(entityToDelete);
            DbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            Context.Entry(entityToUpdate).State = EntityState.Modified;
            DbSet.Update(entityToUpdate);
            Context.SaveChanges();
        }
    }
}
