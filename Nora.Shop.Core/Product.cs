using System.ComponentModel.DataAnnotations;

namespace Nora.Shop.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ürün adı 2 ile 100 karakter arasında olmalı.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string Description { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "9999999", ErrorMessage = "Fiyat 0'dan büyük olmalı.")]
        public decimal Price { get; set; }

        [Range(0, 100000, ErrorMessage = "Stok negatif olamaz.")]
        public int Stock { get; set; }

        [Url(ErrorMessage = "Resim alanı geçerli bir URL olmalı.")]
        public string? ImageUrl { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Kategori seçmelisiniz.")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
