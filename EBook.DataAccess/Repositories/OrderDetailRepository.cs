namespace EBook.DataAccess.Repositories;
public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
{
    private readonly DataContext _context;
    public OrderDetailRepository(DataContext context) : base(context) => _context = context;
    public void Update(OrderDetail orderDetail) => _context.OrderDetails.Update(orderDetail);
}

