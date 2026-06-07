using Nora.Shop.Core.Entities;

namespace Nora.Shop.Core.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        List<Order> GetAllOrdersWithItems();
        List<Order> GetUserOrders(string userId);
        Order? GetOrderWithItems(int id);
    }
}
