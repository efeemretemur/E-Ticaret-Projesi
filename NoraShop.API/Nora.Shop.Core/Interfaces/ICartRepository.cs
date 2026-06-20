using Nora.Shop.Core.Entities;

namespace Nora.Shop.Core.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        List<Cart> GetCartItems(string userId);
        Cart? GetCartItem(string userId, int productId);
        void ClearCart(string userId);
    }
}
