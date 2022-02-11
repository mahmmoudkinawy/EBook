namespace EBook.DataAccess.Interfaces;
public interface ICompanyRepository : IRepository<Company>
{
    void Update(Company company);
}
