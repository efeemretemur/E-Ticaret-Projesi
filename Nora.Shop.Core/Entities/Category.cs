using System.ComponentModel.DataAnnotations;

namespace Nora.Shop.Core.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Kategori adı 2 ile 60 karakter arasında olmalı.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Açıklama en fazla 250 karakter olabilir.")]
        public string Description { get; set; } = string.Empty;

        public List<Product> Products { get; set; } = new();
    }
}
