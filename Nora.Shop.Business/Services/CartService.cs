using Nora.Shop.Core.Entities;
using Nora.Shop.Core.Interfaces;

namespace Nora.Shop.Business.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public List<Cart> GetCartItems(string userId)
        {
            return _cartRepository.GetCartItems(userId);
        }

        public void AddToCart(string userId, int productId, int quantity)
        {
            var product = _productRepository.GetById(productId);
            if (product is null || product.Stock <= 0)
            {
                return;
            }

            var quantityToAdd = Math.Max(1, Math.Min(quantity, product.Stock));
            var existingCartItem = _cartRepository.GetCartItem(userId, productId);

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
            _cartRepository.ClearCart(userId);
        }
    }
}
