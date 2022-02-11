namespace EBook.Web.Areas.Admin.Controllers;
public class CompanyController : BaseAdminController
{
    private readonly IUnitOfWork _unitOfWork;

    public CompanyController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    //GET the View only
    public IActionResult Index() => View();

    //GET the view based on the ID
    public IActionResult Upsert(int? id)
    {
        var company = new Company();

        if (id == null || id == 0)
            return View(company);
        else
            company = _unitOfWork.CompanyRepository.GetFirstOrDefault(p => p.Id == id);

        return View(company);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(Company company)
    {
        if (ModelState.IsValid)
        {
            if (company.Id == 0)
            {
                _unitOfWork.CompanyRepository.Add(company);
                TempData["success"] = "Company Created Successfully";
            }
            else
            {
                _unitOfWork.CompanyRepository.Update(company);
                TempData["success"] = "Company Updated Successfully";
            }

            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        return View(company);
    }

    #region API calls

    [HttpGet]
    public IActionResult GetAll()
        => Json(new { data = _unitOfWork.CompanyRepository.GetAll() });

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var company = _unitOfWork.CompanyRepository.GetFirstOrDefault(p => p.Id == id);

        if (company == null) return Json(new { success = false, message = "Error while deleting!" });

        _unitOfWork.CompanyRepository.Remove(company);
        _unitOfWork.Save();

        return Json(new { success = true, message = "Deleted Successfully" });
    }

    #endregion
}
