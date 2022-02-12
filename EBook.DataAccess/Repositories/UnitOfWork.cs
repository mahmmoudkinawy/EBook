namespace EBook.DataAccess.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    public UnitOfWork(DataContext context)
    {
        _context = context;
        CategoryRepository = new CategoryRepository(_context); //Initial value for CategoryRepository
        CoverTypeRepository = new CoverTypeRepository(_context);
        ProductRepository = new ProductRepository(_context);
        CompanyRepository = new CompanyRepository(_context);
        AppUserRepository = new AppUserRepository(_context);
        ShoppingCartRepository = new ShoppingCartRepository(_context);
    }

    public ICategoryRepository CategoryRepository { get; }
    public ICoverTypeRepository CoverTypeRepository { get; }
    public IProductRepository ProductRepository { get; }
    public ICompanyRepository CompanyRepository { get; }
    public IAppUserRepository AppUserRepository { get; }
    public IShoppingCartRepository ShoppingCartRepository { get; }

    public void Save() => _context.SaveChanges();
}
