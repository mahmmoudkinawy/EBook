using EBook.Models.ViewModels;

namespace EBook.Web.Areas.Admin.Controllers;
public class ProductController : BaseAdminController
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public IActionResult Index() => View(_unitOfWork.ProductRepository.GetAll());

    public IActionResult Upsert(int? id)
    {
        var productViewModel = new ProductViewModel
        {
            Product = new Product(),
            CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }),
            CoverTypeList = _unitOfWork.CoverTypeRepository.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }),
        };

        if (id == null || id == 0)
        {
            //Create new Product Model
            return View(productViewModel);
        }
        else
        {

        }

        return View(productViewModel);
    }
}
