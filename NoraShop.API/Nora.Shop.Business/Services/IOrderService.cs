using Nora.Shop.Core.Entities;

namespace Nora.Shop.Business.Services
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();
        List<Order> GetUserOrders(string userId);
        Order GetOrderById(int id);
        bool CreateOrder(string userId, string address);
        void UpdateOrderStatus(int orderId, string status);
    }
}
