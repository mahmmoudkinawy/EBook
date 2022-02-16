namespace EBook.Web.Areas.Customer.Controllers;

[Authorize]
public class CartController : BaseCustomerController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;

    [BindProperty]
    public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

    public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
    {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
    }

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

        ShoppingCartViewModel.OrderHeader.OrderDate = DateTime.Now;
        ShoppingCartViewModel.OrderHeader.AppUserId = user;

        foreach (var cart in ShoppingCartViewModel.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
            ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }

        var appUser = _unitOfWork.AppUserRepository.GetFirstOrDefault(u => u.Id == user);
        if (appUser.CompanyId.GetValueOrDefault() == 0)
        {
            ShoppingCartViewModel.OrderHeader.OrderStatus = Constants.StatusPending;
            ShoppingCartViewModel.OrderHeader.PaymentStatus = Constants.PaymentStatusPending;
        }
        else
        {
            ShoppingCartViewModel.OrderHeader.OrderStatus = Constants.StatusApproved;
            ShoppingCartViewModel.OrderHeader.PaymentStatus = Constants.PaymentStatusDelayedPayment;
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

        if (appUser.CompanyId.GetValueOrDefault() == 0)
        {
            //Stripe ready code
            var domain = "https://localhost:44364/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartViewModel.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index"
            };

            foreach (var item in ShoppingCartViewModel.ShoppingCartList)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)item.Price * 100, //20.00 => 2000
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        },

                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            _unitOfWork.OrderHeaderRepository.UpdateStripePaymentId(ShoppingCartViewModel.OrderHeader.Id,
                session.Id, session.PaymentIntentId);
            _unitOfWork.Save();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        else
        {
            return RedirectToAction("OrderConfirmation", "Cart",
                new
                {
                    id = ShoppingCartViewModel.OrderHeader.Id
                }
                );
        }
    }

    public IActionResult OrderConfirmation(int id)
    {
        var orderHeader = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault(o => o.Id == id,
            includeProperties: "AppUser");

        if (orderHeader.PaymentStatus != Constants.PaymentStatusDelayedPayment)
        {
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            //Check the stripe status, so no one can copy the link and redirected
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeaderRepository.UpdateStatus(id, Constants.StatusApproved,
                    Constants.PaymentStatusApproved);
                _unitOfWork.Save();
            }
        }

        _emailSender.SendEmailAsync(orderHeader.AppUser.Email, "New Order - Kinawy's EBook Store",
            "<p>Hello Khloud This is message from Kinawy's Application</p>");
        var shoppingCarts = _unitOfWork.ShoppingCartRepository
            .GetAll(u => u.AppUserId == orderHeader.AppUserId).ToList();
        _unitOfWork.ShoppingCartRepository.RemoveRange(shoppingCarts);
        _unitOfWork.Save();

        return View(id);
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
