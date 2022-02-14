namespace EBook.Web.Areas.Customer.Controllers;

[Authorize]
public class CartController : BaseCustomerController
{
    private readonly IUnitOfWork _unitOfWork;

    [BindProperty]
    public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

    public CartController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public IActionResult Index()
    {
        var user = User.GetUserNameIdentifier();

        var shoppingCart = new ShoppingCartViewModel
        {
            ShoppingCartList = _unitOfWork.ShoppingCartRepository.
                    GetAll(u => u.AppUserId == user, includeProperties: "Product"),
            OrderHeader = new()
        };

        foreach (var cart in shoppingCart.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
            shoppingCart.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }
        return View(shoppingCart);
    }

    public IActionResult Summary()
    {
        var user = User.GetUserNameIdentifier();

        ShoppingCartViewModel = new ShoppingCartViewModel
        {
            ShoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(u => u.AppUserId == user, includeProperties: "Product"),
            OrderHeader = new()
        };

        ShoppingCartViewModel.OrderHeader.AppUser = _unitOfWork.AppUserRepository.GetFirstOrDefault(u => u.Id == user);

        ShoppingCartViewModel.OrderHeader.Name = ShoppingCartViewModel.OrderHeader.AppUser.Name;
        ShoppingCartViewModel.OrderHeader.PhoneNumber = ShoppingCartViewModel.OrderHeader.AppUser.PhoneNumber;
        ShoppingCartViewModel.OrderHeader.StreetAddress = ShoppingCartViewModel.OrderHeader.AppUser.StreetAddress;
        ShoppingCartViewModel.OrderHeader.City = ShoppingCartViewModel.OrderHeader.AppUser.City;
        ShoppingCartViewModel.OrderHeader.State = ShoppingCartViewModel.OrderHeader.AppUser.State;
        ShoppingCartViewModel.OrderHeader.PostalCode = ShoppingCartViewModel.OrderHeader.AppUser.PostalCode;

        foreach (var cart in ShoppingCartViewModel.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
            ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }

        return View(ShoppingCartViewModel);
    }

    [HttpPost, ActionName("Summary")]
    [ValidateAntiForgeryToken]
    public IActionResult SummaryPost()
    {
        var user = User.GetUserNameIdentifier();

        ShoppingCartViewModel.ShoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(u => u.AppUserId == user,
                                    includeProperties: "Product");

        ShoppingCartViewModel.OrderHeader.OrderStatus = Constants.StatusPending;
        ShoppingCartViewModel.OrderHeader.PaymentStatus = Constants.PaymentStatusPending;
        ShoppingCartViewModel.OrderHeader.OrderDate = DateTime.Now;
        ShoppingCartViewModel.OrderHeader.AppUserId = user;

        foreach (var cart in ShoppingCartViewModel.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
            ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }

        //Save order header
        _unitOfWork.OrderHeaderRepository.Add(ShoppingCartViewModel.OrderHeader);
        _unitOfWork.Save();

        foreach (var cart in ShoppingCartViewModel.ShoppingCartList)
        {
            var orderDetail = new OrderDetail
            {
                ProductId = cart.ProductId,
                Count = cart.Count,
                Price = cart.Price,
                OrderId = ShoppingCartViewModel.OrderHeader.Id
            };

            //Save order details
            _unitOfWork.OrderDetailRepository.Add(orderDetail);
            _unitOfWork.Save();
        }

        _unitOfWork.ShoppingCartRepository.RemoveRange(ShoppingCartViewModel.ShoppingCartList);
        _unitOfWork.Save();

        //TempData["success"] = "Order Placed Successfully";

        return RedirectToAction("Index", "Cart");
    }

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

    private static double GetPriceBasedOnQuantity(int quantity, double price,
        double price50, double price100)
    {
        if (quantity <= 50)
            return price;
        else
            return quantity <= 100 ? price50 : price100;
    }
}
