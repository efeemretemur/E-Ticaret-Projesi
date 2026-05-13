using Microsoft.EntityFrameworkCore;
using Nora.Shop.Core.Entities;
using Nora.Shop.DataAccess.Context;

namespace Nora.Shop.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly NoraShopContext _context;

        public OrderService(NoraShopContext context)
        {
            _context = context;
        }

        public List<Order> GetAllOrders()
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public List<Order> GetUserOrders(string userId)
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public Order GetOrderById(int id)
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.Id == id)!;
        }

        public bool CreateOrder(string userId, string address)
        {
            var cartItems = _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToList();

            if (cartItems.Count == 0)
            {
                return false;
            }

            if (cartItems.Any(c => c.Product is null || c.Quantity > c.Product.Stock))
            {
                return false;
            }

            var order = new Order
            {
                UserId = userId,
                Address = address,
                OrderDate = DateTime.Now,
                Status = "Beklemede",
                TotalPrice = cartItems.Sum(c => c.Quantity * (c.Product?.Price ?? 0)),
                OrderItems = cartItems.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    UnitPrice = c.Product?.Price ?? 0
                }).ToList()
            };

            _context.Orders.Add(order);

            foreach (var item in cartItems)
            {
                if (item.Product is not null)
                {
                    item.Product.Stock -= item.Quantity;
                }
            }

            _context.Carts.RemoveRange(cartItems);
            _context.SaveChanges();
            return true;
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order is null)
            {
                return;
            }

            order.Status = status;
            _context.SaveChanges();
        }
    }
}
