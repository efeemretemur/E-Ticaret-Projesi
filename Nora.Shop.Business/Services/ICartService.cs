using Nora.Shop.Core.Entities;

namespace Nora.Shop.Business.Services
{
    public interface ICartService
    {
        List<Cart> GetCartItems(string userId);
        void AddToCart(string userId, int productId, int quantity);
        void RemoveFromCart(int cartId);
        void ClearCart(string userId);
    }
}