namespace EBook.Web.Controllers;
public class CategoryController : BaseController
{
    private readonly ICategoryRepository _categoryRepository;
    public CategoryController(ICategoryRepository categoryRepository) => _categoryRepository = categoryRepository;

    //GET all data
    public IActionResult Index() => View(_categoryRepository.GetAll());

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
            _categoryRepository.Add(category);
            _categoryRepository.Save();
            TempData["success"] = "Created Category successfully";
            return RedirectToAction("Index");
        }
        return View(category);
    }

    //GET edit view
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0) return NotFound();

        var category = _categoryRepository.GetFirstOrDefault(c => c.Id == id);

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
            _categoryRepository.Update(category);
            _categoryRepository.Save();
            TempData["success"] = "Updated Category successfully";
            return RedirectToAction("Index");
        }
        return View(category);
    }

    //GET delete VIEW
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0) return NotFound();

        var category = _categoryRepository.GetFirstOrDefault(c => c.Id == id);

        if (category == null) return NotFound();

        return View(category);
    }

    //POST deleted data
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        if (id == null || id == 0) return NotFound();

        var category = _categoryRepository.GetFirstOrDefault(c => c.Id == id);

        if (category == null) return NotFound();

        _categoryRepository.Remove(category);
        _categoryRepository.Save();
        TempData["success"] = "Deleted Category successfully";

        return RedirectToAction("Index");
    }

}

