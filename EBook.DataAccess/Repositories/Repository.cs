using System.Linq;

namespace EBook.DataAccess.Repositories;
public class Repository<T> : IRepository<T> where T : class
{
    private readonly DataContext _context;
    private readonly DbSet<T> _dbSet;
    public Repository(DataContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public void Add(T entity) => _dbSet.Add(entity);

    //includeProperties => "Category,CoverType"
    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null)
    {
        //var query = _dbSet; //That's not correct
        IQueryable<T> query = _dbSet; //Untill now we didn't go to the database

        if (filter != null)
            query = query.Where(filter);

        if (includeProperties != null)
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProp);

        return query.ToList();
    }

    public T GetFirstOrDefault(Expression<Func<T, bool>> filter,
        string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;

        query = query.Where(filter);

        if (includeProperties != null)
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProp);

        return query.FirstOrDefault();
    }

    public void Remove(T entity) => _dbSet.Remove(entity);

    public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);
}