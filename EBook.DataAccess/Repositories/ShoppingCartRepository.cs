namespace EBook.DataAccess.Repositories;
public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
{
    public ShoppingCartRepository(DataContext context) : base(context) { }

    public int DecrementCount(ShoppingCart cart, int count)
    {
        cart.Count -= count;
        return cart.Count;
    }

    public int IncrementCount(ShoppingCart cart, int count)
    {
        cart.Count += count;
        return cart.Count;
    }
}

