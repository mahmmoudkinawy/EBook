namespace EBook.DataAccess.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    public UnitOfWork(DataContext context)
    {
        _context = context;
        CategoryRepository = new CategoryRepository(_context); //Initial value for CategoryRepository
    }

    public ICategoryRepository CategoryRepository { get; }

    public void Save() => _context.SaveChanges();
}
