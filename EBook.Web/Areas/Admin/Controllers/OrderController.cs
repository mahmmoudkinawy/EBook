namespace EBook.Web.Areas.Admin.Controllers;
public class OrderController : BaseAdminController
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public IActionResult Index() => View();

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var orderHeaders = _unitOfWork.OrderHeaderRepository.GetAll(includeProperties: "AppUser");

        return Json(new
        {
            data = orderHeaders
        });
    }

    #endregion
}
