namespace EBook.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class CoverTypeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CoverTypeController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public IActionResult Index() => View(_unitOfWork.CoverTypeRepository.GetAll());

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CoverType coverType)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverTypeRepository.Add(coverType);
            _unitOfWork.Save();
            TempData["success"] = "Cover Type Created Successfully";
            return RedirectToAction("Index");
        }
        return View(coverType);
    }

    public IActionResult Edit(int? id)
    {
        if (id == 0 || id == null) return NotFound();

        var coverType = _unitOfWork.CoverTypeRepository.GetFirstOrDefault(c => c.Id == id);

        if (coverType == null) return NotFound();

        return View(coverType);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CoverType coverType)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverTypeRepository.Update(coverType);
            _unitOfWork.Save();
            TempData["success"] = "Cover Type Updated Successfully";
            return RedirectToAction("Index");
        }
        return View(coverType);
    }

    public IActionResult Delete(int id)
    {
        if (id == 0) return NotFound();

        var coverType = _unitOfWork.CoverTypeRepository.GetFirstOrDefault(c => c.Id == id);

        if (coverType == null) return NotFound();

        return View(coverType);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int id)
    {
        if (id == 0) return NotFound();

        var coverType = _unitOfWork.CoverTypeRepository.GetFirstOrDefault(c => c.Id == id);

        if (coverType == null) return NotFound();

        _unitOfWork.CoverTypeRepository.Remove(coverType);
        _unitOfWork.Save();
        TempData["success"] = "Cover Type Deleted Successfully";

        return RedirectToAction("Index");
    }
}

