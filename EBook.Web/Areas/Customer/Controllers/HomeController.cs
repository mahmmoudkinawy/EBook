namespace EBook.Web.Areas.Customer.Controllers;
public class HomeController : BaseCustomerController
{
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public IActionResult Index()
        => View(_unitOfWork.ProductRepository.GetAll(includeProperties: "Category,CoverType"));
}

