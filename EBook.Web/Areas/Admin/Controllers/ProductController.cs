using Microsoft.AspNetCore.Mvc.Rendering;

namespace EBook.Web.Areas.Admin.Controllers;
public class ProductController : BaseAdminController
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public IActionResult Index() => View(_unitOfWork.ProductRepository.GetAll());

    public IActionResult Upsert(int? id)
    {
        var categoryList = _unitOfWork.CategoryRepository.GetAll()
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    });


        if (id == null || id == 0)
        {
            //Create new Product Model
            ViewBag.categoryList = categoryList;
            return View(new Product());
        }
        else
        {

        }

        return View();
    }
}
