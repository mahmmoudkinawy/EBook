namespace EBook.DataAccess;
public class DataContext : IdentityDbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<CoverType> CoverTypes { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
}

