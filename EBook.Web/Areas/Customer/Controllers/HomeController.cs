namespace EBook.Web.Areas.Customer.Controllers;
public class HomeController : BaseCustomerController
{
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public IActionResult Index()
        => View(_unitOfWork.ProductRepository.GetAll(includeProperties: "Category,CoverType"));

    public IActionResult Details(int id)
    {
        var shoppingCart = new ShoppingCart
        {
            Product = _unitOfWork.ProductRepository.GetFirstOrDefault(p => p.Id == id,
            includeProperties: "Category,CoverType"),
            Count = 1
        };

        return View(shoppingCart);
    }
}

