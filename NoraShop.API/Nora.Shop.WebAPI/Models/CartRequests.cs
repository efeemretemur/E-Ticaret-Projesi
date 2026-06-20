using System.ComponentModel.DataAnnotations;

namespace Nora.Shop.WebAPI.Models
{
    public class AddToCartRequest
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; } = 1;
    }
}
