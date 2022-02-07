namespace EBook.Web.Areas.Admin.Controllers;
public class ProductController : BaseAdminController
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public IActionResult Index() => View(_unitOfWork.ProductRepository.GetAll());
}
