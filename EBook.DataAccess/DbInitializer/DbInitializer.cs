namespace EBook.DataAccess.DbInitializer;
public class DbInitializer : IDbInitializer
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly DataContext _context;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        DataContext context,
        ILogger<DbInitializer> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        //Apply pending migrations
        try
        {
            if (_context.Database.GetPendingMigrations().Count() > 0)
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        //Create roles if not created

        if (!await _roleManager.RoleExistsAsync(Constants.RoleAdmin))
        {
            await _roleManager.CreateAsync(new IdentityRole(Constants.RoleUserIndividual));
            await _roleManager.CreateAsync(new IdentityRole(Constants.RoleUserCompany));
            await _roleManager.CreateAsync(new IdentityRole(Constants.RoleAdmin));
            await _roleManager.CreateAsync(new IdentityRole(Constants.RoleEmployee));

            //Create the admin user
            await _userManager.CreateAsync(new AppUser
            {
                UserName = "admin@test.com",
                Email = "admin@test.com",
                City = "Shebin - Elkom",
                Name = "admin",
                PhoneNumber = "01208534244",
                StreetAddress = "Nader",
                State = "Menofia",
                PostalCode = "4321"
            }, "Pa$$w0rd");

            var user = _context.AppUsers.FirstOrDefault(u => u.Email.Equals("admin@test.com"));

            await _userManager.AddToRoleAsync(user, Constants.RoleAdmin);
        }

        return;
    }
}
