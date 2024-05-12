using System.Linq.Expressions;

namespace BasicBoilerplate.Interfaces
{
    public interface IRepositoryService<TEntity> where TEntity : class
    {
        void Delete(TEntity entityToDelete);
        void Delete(int id);
        TEntity Get(int id);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
    }
}
