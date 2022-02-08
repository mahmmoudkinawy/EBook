namespace EBook.Web.Areas.Admin.Controllers;
public class ProductController : BaseAdminController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPhotoService _photoService;

    public ProductController(IUnitOfWork unitOfWork, IPhotoService photoService)
    {
        _unitOfWork = unitOfWork;
        _photoService = photoService;
    }

    //GET the View only
    public IActionResult Index() => View();

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
            //Create new Product Model
            return View(productViewModel);
        else
            //Get the product with the ID from the query
            productViewModel.Product = _unitOfWork.ProductRepository.GetFirstOrDefault(p => p.Id == id);

        return View(productViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductViewModel productViewModel, IFormFile file)
    {
        if (ModelState.IsValid)
        {
            var result = _photoService.AddPhoto(file);
            productViewModel.Product.ImageUrl = result.SecureUrl.AbsoluteUri;

            if (productViewModel.Product.Id == 0)
            {
                _unitOfWork.ProductRepository.Add(productViewModel.Product);
                TempData["success"] = "Product Created Successfully";
            }
            else
            {
                _unitOfWork.ProductRepository.Update(productViewModel.Product);
                TempData["success"] = "Product Updated Successfully";
            }

            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        return View(productViewModel);
    }

    #region API calls

    [HttpGet]
    public IActionResult GetAll()
        => Json(new { data = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category,CoverType") });

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var product = _unitOfWork.ProductRepository.GetFirstOrDefault(p => p.Id == id);

        if (product == null) return Json(new { success = false, message = "Error while deleting!" });

        _unitOfWork.ProductRepository.Remove(product);
        _unitOfWork.Save();

        return Json(new { success = true, message = "Deleted Successfully" });
    }

    #endregion
}
