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

        return View(shoppingCart);
    }
}
