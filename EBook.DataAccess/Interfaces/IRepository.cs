namespace EBook.DataAccess.Interfaces;
public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null);
    T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
    void Add(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}