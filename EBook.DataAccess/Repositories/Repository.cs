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

    public IEnumerable<T> GetAll() => _dbSet.ToList();

    public T GetFirstOrDefault(Expression<Func<T, bool>> filter) => _dbSet.Where(filter).FirstOrDefault();

    public void Remove(T entity) => _dbSet.Remove(entity);

    public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);
}