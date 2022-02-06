using System.Linq.Expressions;

namespace EBook.DataAccess.Interfaces;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    T GetFirstOrDefault(Expression<Func<T, bool>> filter); //FirstOrDefault(x => x.Id == id)
    void Add(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}

