namespace EBook.Web.Areas.Admin.Controllers;

[Authorize]
public class OrderController : BaseAdminController
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public IActionResult Index() => View();

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll(string status)
    {
        IEnumerable<OrderHeader> orderHeaders = null;

        if (User.IsInRole(Constants.RoleAdmin) || User.IsInRole(Constants.RoleEmployee))
            orderHeaders = _unitOfWork.OrderHeaderRepository.GetAll(includeProperties: "AppUser");
        else
            orderHeaders = _unitOfWork.OrderHeaderRepository.GetAll(u => u.AppUserId == User.GetUserNameIdentifier(),
                includeProperties: "AppUser");

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
