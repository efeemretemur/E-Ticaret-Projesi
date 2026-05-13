using Nora.Shop.Core.Entities;

namespace E_Ticaret_Projesi.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<Product> FeaturedProducts { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
    }
}
