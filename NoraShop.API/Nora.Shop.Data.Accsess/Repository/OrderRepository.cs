using System.Collections.Generic;
using System.Linq;
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
            // Tüm siparişleri altındaki sipariş kalemleriyle (OrderItems) birlikte getirir
            return _context.Orders.Include(o => o.OrderItems).ToList();
        }

        public List<Order> GetUserOrders(string userId)
        {
            // Belirli bir kullanıcıya ait siparişleri listeler
            return _context.Orders.Where(o => o.UserId == userId).ToList();
        }

        public Order? GetOrderWithItems(int id)
        {
            // Tek bir siparişi kalemiyle birlikte detaylı getirir
            return _context.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == id);
        }
    }
}