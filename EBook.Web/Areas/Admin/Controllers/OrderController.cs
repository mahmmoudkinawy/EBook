namespace EBook.Web.Areas.Admin.Controllers;

[Authorize]
public class OrderController : BaseAdminController
{
    private readonly IUnitOfWork _unitOfWork;

    [BindProperty]
    public OrderViewModel OrderViewModel { get; set; }

    public OrderController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public IActionResult Index() => View();

    public IActionResult Details(int orderId)
    {
        OrderViewModel = new()
        {
            OrderDetails = _unitOfWork.OrderDetailRepository.GetAll(u => u.OrderId == orderId,
                includeProperties: "Product"),
            OrderHeader = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault(u => u.Id == orderId,
                includeProperties: "AppUser")
        };

        return View(OrderViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateOrderitem()
    {
        var orderHeaderFromDb = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault(u => u.Id == OrderViewModel.OrderHeader.Id);

        orderHeaderFromDb.Name = OrderViewModel.OrderHeader.Name;
        orderHeaderFromDb.PhoneNumber = OrderViewModel.OrderHeader.PhoneNumber;
        orderHeaderFromDb.StreetAddress = OrderViewModel.OrderHeader.StreetAddress;
        orderHeaderFromDb.City = OrderViewModel.OrderHeader.City;
        orderHeaderFromDb.State = OrderViewModel.OrderHeader.State;
        orderHeaderFromDb.PostalCode = OrderViewModel.OrderHeader.PostalCode;

        if (OrderViewModel.OrderHeader.Carrier != null)
            orderHeaderFromDb.Carrier = OrderViewModel.OrderHeader.Carrier;

        if (OrderViewModel.OrderHeader.TrackingNumber != null)
            orderHeaderFromDb.TrackingNumber = OrderViewModel.OrderHeader.TrackingNumber;


        _unitOfWork.OrderHeaderRepository.Update(orderHeaderFromDb);
        _unitOfWork.Save();

        TempData["success"] = "Order Details Updated Successfully";

        return RedirectToAction("Details", "Order", new { orderId = orderHeaderFromDb.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult StartProcessing()
    {
        _unitOfWork.OrderHeaderRepository.UpdateStatus(OrderViewModel.OrderHeader.Id,
            Constants.StatusInProcess);
        _unitOfWork.Save();

        TempData["success"] = "Order Status Updated Successfully";

        return RedirectToAction("Details", "Order", new { orderId = OrderViewModel.OrderHeader.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ShipOrder()
    {
        var orderHeaderFromDb = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault(u => u.Id == OrderViewModel.OrderHeader.Id);
        orderHeaderFromDb.TrackingNumber = OrderViewModel.OrderHeader.TrackingNumber;
        orderHeaderFromDb.Carrier = OrderViewModel.OrderHeader.Carrier;
        orderHeaderFromDb.OrderStatus = OrderViewModel.OrderHeader.OrderStatus;
        orderHeaderFromDb.ShippingDate = DateTime.Now;

        _unitOfWork.OrderHeaderRepository.Update(orderHeaderFromDb);
        _unitOfWork.Save();

        TempData["success"] = "Order Shipped Successfully";

        return RedirectToAction("Details", "Order", new { orderId = OrderViewModel.OrderHeader.Id });
    }

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
