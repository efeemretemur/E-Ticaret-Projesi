using Microsoft.EntityFrameworkCore;
using Nora.Shop.Core.Entities;
using Nora.Shop.Core.Interfaces;
using Nora.Shop.DataAccess.Context;

namespace Nora.Shop.Business.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<Cart> _cartRepository;
        private readonly NoraShopContext _context;

        public CartService(IRepository<Cart> cartRepository, NoraShopContext context)
        {
            _cartRepository = cartRepository;
            _context = context;
        }

        public List<Cart> GetCartItems(string userId)
        {
            return _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToList();
        }

        public void AddToCart(string userId, int productId, int quantity)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product is null || product.Stock <= 0)
            {
                return;
            }

            var quantityToAdd = Math.Max(1, Math.Min(quantity, product.Stock));
            var existingCartItem = _context.Carts
                .FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity = Math.Min(existingCartItem.Quantity + quantityToAdd, product.Stock);
                _cartRepository.Update(existingCartItem);
                return;
            }

            var cart = new Cart
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantityToAdd
            };

            _cartRepository.Add(cart);
        }

        public void RemoveFromCart(int cartId)
        {
            _cartRepository.Delete(cartId);
        }

        public void ClearCart(string userId)
        {
            var items = GetCartItems(userId);
            foreach (var item in items)
            {
                _cartRepository.Delete(item.Id);
            }
        }
    }
}
