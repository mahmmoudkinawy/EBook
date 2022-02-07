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
}

