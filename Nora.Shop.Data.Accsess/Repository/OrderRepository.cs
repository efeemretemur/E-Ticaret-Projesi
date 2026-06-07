using Microsoft.EntityFrameworkCore;
using Nora.Shop.Core.Entities;
using Nora.Shop.Core.Interfaces;
using Nora.Shop.DataAccess.Context;

namespace Nora.Shop.DataAccess.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly NoraShopContext _context;

        public OrderRepository(NoraShopContext context) : base(context)
        {
            _context = context;
        }

        public List<Order> GetAllOrdersWithItems()
        {
            return _context.Orders
                .Include(order => order.OrderItems)
                .ThenInclude(orderItem => orderItem.Product)
                .OrderByDescending(order => order.OrderDate)
                .ToList();
        }

        public List<Order> GetUserOrders(string userId)
        {
            return _context.Orders
                .Include(order => order.OrderItems)
                .ThenInclude(orderItem => orderItem.Product)
                .Where(order => order.UserId == userId)
                .OrderByDescending(order => order.OrderDate)
                .ToList();
        }

        public Order? GetOrderWithItems(int id)
        {
            return _context.Orders
                .Include(order => order.OrderItems)
                .ThenInclude(orderItem => orderItem.Product)
                .FirstOrDefault(order => order.Id == id);
        }
    }
}
