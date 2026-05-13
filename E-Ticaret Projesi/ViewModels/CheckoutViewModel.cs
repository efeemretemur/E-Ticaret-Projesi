using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Nora.Shop.Core.Entities;

namespace E_Ticaret_Projesi.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Teslimat adresi zorunludur.")]
        [StringLength(250, ErrorMessage = "Adres en fazla 250 karakter olabilir.")]
        [Display(Name = "Teslimat Adresi")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kart üzerindeki isim zorunludur.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Kart üzerindeki isim 3 ile 100 karakter arasında olmalı.")]
        [Display(Name = "Kart Üzerindeki İsim")]
        public string CardHolderName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kart numarası zorunludur.")]
        [RegularExpression(@"^[0-9 ]{16,19}$", ErrorMessage = "Kart numarasını 16 haneli olacak şekilde giriniz.")]
        [Display(Name = "Kart Numarası")]
        public string CardNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Son kullanma tarihi zorunludur.")]
        [RegularExpression(@"^(0[1-9]|1[0-2])/[0-9]{2}$", ErrorMessage = "Son kullanma tarihini AA/YY formatında giriniz.")]
        [Display(Name = "Son Kullanma Tarihi")]
        public string ExpiryDate { get; set; } = string.Empty;

        [Required(ErrorMessage = "CVV zorunludur.")]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "CVV 3 veya 4 haneli olmalıdır.")]
        [Display(Name = "CVV")]
        public string Cvv { get; set; } = string.Empty;

        [ValidateNever]
        public List<Cart> CartItems { get; set; } = new();

        [ValidateNever]
        public decimal TotalAmount => CartItems.Sum(x => (x.Product?.Price ?? 0) * x.Quantity);
    }
}
