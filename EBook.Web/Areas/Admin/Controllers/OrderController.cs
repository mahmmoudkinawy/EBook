namespace EBook.Web.Areas.Admin.Controllers;
public class OrderController : BaseAdminController
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public IActionResult Index() => View();

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll(string status)
    {
        var orderHeaders = _unitOfWork.OrderHeaderRepository.GetAll(includeProperties: "AppUser");

        orderHeaders = status switch
        {
            "pending" => orderHeaders.Where(p => p.PaymentStatus == Constants.PaymentStatusDelayedPayment),
            "inprocess" => orderHeaders.Where(p => p.OrderStatus == Constants.StatusInProcess),
            "completed" => orderHeaders.Where(p => p.OrderStatus == Constants.StatusShipped),
            "approved" => orderHeaders.Where(p => p.OrderStatus == Constants.StatusApproved),
            _ => orderHeaders
        };

        return Json(new
        {
            data = orderHeaders
        });
    }

    #endregion
}
