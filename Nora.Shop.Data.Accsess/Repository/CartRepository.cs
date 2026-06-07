using Microsoft.EntityFrameworkCore;
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
            return _context.Carts
                .Include(cart => cart.Product)
                .Where(cart => cart.UserId == userId)
                .ToList();
        }

        public Cart? GetCartItem(string userId, int productId)
        {
            return _context.Carts
                .FirstOrDefault(cart => cart.UserId == userId && cart.ProductId == productId);
        }

        public void ClearCart(string userId)
        {
            var items = _context.Carts.Where(cart => cart.UserId == userId);
            _context.Carts.RemoveRange(items);
            _context.SaveChanges();
        }
    }
}
