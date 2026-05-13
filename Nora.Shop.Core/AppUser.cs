using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Nora.Shop.Core.Entities
{
    public class AppUser : IdentityUser
    {
        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Ad Soyad 3 ile 100 karakter arasında olmalı.")]
        public string FullName { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Adres en fazla 250 karakter olabilir.")]
        public string? Address { get; set; }
    }
}
