namespace EBook.DataAccess.Interfaces;
public interface IProductRepository : IRepository<Product>
{
    void Update(Product product);
}
