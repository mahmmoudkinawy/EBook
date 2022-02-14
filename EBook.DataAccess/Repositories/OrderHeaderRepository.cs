namespace EBook.DataAccess.Repositories;
public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
{
    private readonly DataContext _context;
    public OrderHeaderRepository(DataContext context) : base(context) => _context = context;

    public void Update(OrderHeader orderHeader) => _context.OrderHeaders.Update(orderHeader);

    public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
    {
        var orderHeaderFromDb = _context.OrderHeaders.Find(id);
        if (orderHeaderFromDb != null)
        {
            orderHeaderFromDb.OrderStatus = orderStatus;
            if (paymentStatus != null)
                orderHeaderFromDb.PaymentStatus = paymentStatus;
        }
    }

    public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
    {
        var orderHeaderFromDb = _context.OrderHeaders.Find(id);
        orderHeaderFromDb.SessionId = sessionId;
        orderHeaderFromDb.PaymentIntentId = paymentIntentId;
    }
}

