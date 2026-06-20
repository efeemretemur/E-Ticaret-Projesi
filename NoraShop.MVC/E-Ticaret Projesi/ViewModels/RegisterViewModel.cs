using System.ComponentModel.DataAnnotations;

namespace E_Ticaret_Projesi.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Ad Soyad 3 ile 100 karakter arasında olmalı.")]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalı.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Şifre Tekrarı")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Adres en fazla 250 karakter olabilir.")]
        [Display(Name = "Adres")]
        public string? Address { get; set; }
    }
}
