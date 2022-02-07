namespace EBook.DataAccess.Repositories;
public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly DataContext _context;
    public ProductRepository(DataContext context) : base(context) => _context = context;
    public void Update(Product product)
    {
        var productFromDb = _context.Products.FirstOrDefault(p => p.Id == product.Id);
        if (productFromDb != null)
        {
            productFromDb.Title = product.Title;
            productFromDb.ISBN = product.ISBN;
            productFromDb.Price = product.Price;
            productFromDb.Price50 = product.Price50;
            productFromDb.ListPrice = product.ListPrice;
            productFromDb.Price100 = product.Price100;
            productFromDb.Description = product.Description;
            productFromDb.CategoryId = product.CategoryId;
            productFromDb.Author = product.Author;
            productFromDb.CoverTypeId = product.CoverTypeId;

            if (product.ImageUrl != null)
                productFromDb.ImageUrl = product.ImageUrl;
        }
    }
}

