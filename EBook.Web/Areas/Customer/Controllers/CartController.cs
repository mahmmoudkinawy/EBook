namespace EBook.Web.Areas.Customer.Controllers;

[Authorize]
public class CartController : BaseCustomerController
{
    private readonly IUnitOfWork _unitOfWork;

    public CartController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public IActionResult Index()
    {
        var user = User.GetUserNameIdentifier();

        var shoppingCart = new ShoppingCartViewModel
        {
            ShoppingCartList = _unitOfWork.ShoppingCartRepository.
                    GetAll(u => u.AppUserId == user, includeProperties: "Product")
        };

        foreach (var cart in shoppingCart.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
            shoppingCart.CartTotal += (cart.Price * cart.Count);
        }
        return View(shoppingCart);
    }

    public IActionResult Summary() => View();

    public IActionResult Plus(int cartId)
    {
        var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(c => c.Id == cartId);
        _unitOfWork.ShoppingCartRepository.IncrementCount(cart, 1);
        _unitOfWork.Save();
        //TempData["success"] = "Incremented Successfully";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Minus(int cartId)
    {
        var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(c => c.Id == cartId);

        if (cart.Count <= 1)
            _unitOfWork.ShoppingCartRepository.Remove(cart);

        _unitOfWork.ShoppingCartRepository.DecrementCount(cart, 1);

        _unitOfWork.Save();
        //TempData["success"] = "Incremented Successfully";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Remove(int cartId)
    {
        var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(c => c.Id == cartId);
        _unitOfWork.ShoppingCartRepository.Remove(cart);
        _unitOfWork.Save();
        //TempData["success"] = "Incremented Successfully";
        return RedirectToAction(nameof(Index));
    }

    private static double GetPriceBasedOnQuantity(int quantity, double price, double price50, double price100)
    {
        if (quantity <= 50)
            return price;
        else
            return quantity <= 100 ? price50 : price100;
    }
}
