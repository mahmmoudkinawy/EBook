namespace EBook.Web.Controllers;
public class CategoryController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    //GET all data
    public IActionResult Index() => View(_unitOfWork.CategoryRepository.GetAll());

    //GET the View
    public IActionResult Create() => View();

    //POST the model
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category category)
    {
        if (category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "Name cannot be same as Display Order");
        }
        if (ModelState.IsValid)
        {
            _unitOfWork.CategoryRepository.Add(category);
            _unitOfWork.Save();
            TempData["success"] = "Created Category successfully";
            return RedirectToAction("Index");
        }
        return View(category);
    }

    //GET edit view
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0) return NotFound();

        var category = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == id);

        if (category == null) return NotFound();

        return View(category);
    }

    //POST data
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category category)
    {
        if (category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
        }
        if (ModelState.IsValid)
        {
            _unitOfWork.CategoryRepository.Update(category);
            _unitOfWork.Save();
            TempData["success"] = "Updated Category successfully";
            return RedirectToAction("Index");
        }
        return View(category);
    }

    //GET delete VIEW
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0) return NotFound();

        var category = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == id);

        if (category == null) return NotFound();

        return View(category);
    }

    //POST deleted data
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        if (id == null || id == 0) return NotFound();

        var category = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == id);

        if (category == null) return NotFound();

        _unitOfWork.CategoryRepository.Remove(category);
        _unitOfWork.Save();
        TempData["success"] = "Deleted Category successfully";

        return RedirectToAction("Index");
    }

}

