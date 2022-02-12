namespace EBook.DataAccess.Repositories;
public class AppUserRepository : Repository<AppUser>, IAppUserRepository
{
    private readonly DataContext _context;
    public AppUserRepository(DataContext context) : base(context) => _context = context;
}

