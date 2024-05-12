namespace BasicBoilerplate.Interfaces
{
    public interface IGeneralService<TEntity> where TEntity : class
    {
        TEntity GetEntity(string id);
        string CreateEntity(TEntity entity);
        void UpdateEntity(TEntity entity);
        void DisableEntity(string id);
        List<TEntity> GetAll();

    }
}
