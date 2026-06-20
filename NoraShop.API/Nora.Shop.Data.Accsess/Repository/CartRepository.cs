using System.Collections.Generic;
using System.Linq;
using Nora.Shop.Core.Entities;
using Nora.Shop.Core.Interfaces;
using Nora.Shop.DataAccess.Context;

namespace Nora.Shop.DataAccess.Repository
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private readonly NoraShopContext _context;

        public CartRepository(NoraShopContext context) : base(context)
        {
            _context = context;
        }

        public List<Cart> GetCartItems(string userId)
        {
            return _context.Carts.Where(c => c.UserId == userId).ToList();
        }

        public Cart? GetCartItem(string userId, int productId)
        {
            return _context.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
        }

        public void ClearCart(string userId)
        {
            var items = _context.Carts.Where(c => c.UserId == userId).ToList();
            if (items.Any())
            {
                _context.Carts.RemoveRange(items);
                _context.SaveChanges();
            }
        }
    }
}