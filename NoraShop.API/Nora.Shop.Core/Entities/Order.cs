using System.ComponentModel.DataAnnotations;

namespace Nora.Shop.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = "Sipariş durumu zorunludur.")]
        public string Status { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adres zorunludur.")]
        [StringLength(250, ErrorMessage = "Adres en fazla 250 karakter olabilir.")]
        public string Address { get; set; } = string.Empty;

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
