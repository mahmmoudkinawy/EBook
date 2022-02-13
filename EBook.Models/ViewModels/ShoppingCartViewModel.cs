namespace EBook.Models.ViewModels;
public class ShoppingCartViewModel
{
    public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
    public double CartTotal { get; set; }
}

