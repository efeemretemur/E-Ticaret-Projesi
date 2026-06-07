using Nora.Shop.Core.Entities;
using Nora.Shop.Core.Interfaces;

namespace Nora.Shop.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public List<Order> GetAllOrders()
        {
            return _orderRepository.GetAllOrdersWithItems();
        }

        public List<Order> GetUserOrders(string userId)
        {
            return _orderRepository.GetUserOrders(userId);
        }

        public Order GetOrderById(int id)
        {
            return _orderRepository.GetOrderWithItems(id)!;
        }

        public bool CreateOrder(string userId, string address)
        {
            var cartItems = _cartRepository.GetCartItems(userId);

            if (cartItems.Count == 0)
            {
                return false;
            }

            if (cartItems.Any(cart => cart.Product is null || cart.Quantity > cart.Product.Stock))
            {
                return false;
            }

            var order = new Order
            {
                UserId = userId,
                Address = address,
                OrderDate = DateTime.Now,
                Status = "Beklemede",
                TotalPrice = cartItems.Sum(cart => cart.Quantity * (cart.Product?.Price ?? 0)),
                OrderItems = cartItems.Select(cart => new OrderItem
                {
                    ProductId = cart.ProductId,
                    Quantity = cart.Quantity,
                    UnitPrice = cart.Product?.Price ?? 0
                }).ToList()
            };

            _orderRepository.Add(order);

            foreach (var item in cartItems)
            {
                if (item.Product is not null)
                {
                    item.Product.Stock -= item.Quantity;
                    _productRepository.Update(item.Product);
                }
            }

            _cartRepository.ClearCart(userId);
            return true;
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            var order = _orderRepository.GetById(orderId);
            if (order is null)
            {
                return;
            }

            order.Status = status;
            _orderRepository.Update(order);
        }
    }
}
