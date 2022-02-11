namespace EBook.DataAccess.Repositories;
public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    private readonly DataContext _context;
    public CompanyRepository(DataContext context) : base(context) => _context = context;
    public void Update(Company company) => _context.Companies.Update(company);
}