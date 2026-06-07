using System.ComponentModel.DataAnnotations;

namespace Nora.Shop.WebAPI.Models
{
    public class CreateOrderRequest
    {
        [Required]
        [StringLength(250)]
        public string Address { get; set; } = string.Empty;
    }

    public class UpdateOrderStatusRequest
    {
        [Required]
        [StringLength(40)]
        public string Status { get; set; } = string.Empty;
    }
}
