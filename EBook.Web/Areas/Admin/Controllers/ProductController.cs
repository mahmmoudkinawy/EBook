namespace EBook.Web.Areas.Admin.Controllers;
public class ProductController : BaseAdminController
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public IActionResult Index() => View(_unitOfWork.ProductRepository.GetAll());

    public IActionResult Upsert(int? id)
    {
        if (id == null || id == 0)
        {
            //Create new Product Model
            ViewBag.CategoryList = _unitOfWork.CategoryList();
            ViewData["CoverTypeList"] = _unitOfWork.CoverTypeList();
            return View(new Product());
        }
        else
        {

        }

        return View();
    }
}
