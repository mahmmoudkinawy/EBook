namespace EBook.DataAccess.Interfaces;
public interface IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }
    ICoverTypeRepository CoverTypeRepository { get; }
    IProductRepository ProductRepository { get; }
    ICompanyRepository CompanyRepository { get; }
    void Save();
}

