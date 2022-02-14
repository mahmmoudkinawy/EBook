namespace EBook.DataAccess.Interfaces;
public interface IOrderDetailRepository : IRepository<OrderDetail>
{
    void Update(OrderDetail orderDetail);
}
