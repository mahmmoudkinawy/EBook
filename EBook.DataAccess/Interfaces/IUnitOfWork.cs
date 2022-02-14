namespace EBook.DataAccess.Interfaces;
public interface IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }
    ICoverTypeRepository CoverTypeRepository { get; }
    IProductRepository ProductRepository { get; }
    ICompanyRepository CompanyRepository { get; }
    IAppUserRepository AppUserRepository { get; }
    IShoppingCartRepository ShoppingCartRepository { get; }
    IOrderHeaderRepository OrderHeaderRepository { get; }
    IOrderDetailRepository OrderDetailRepository { get; }
    void Save();
}

