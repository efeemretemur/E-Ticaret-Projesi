using Microsoft.EntityFrameworkCore;
using Nora.Shop.Core.Entities;
using Nora.Shop.DataAccess.Context;

namespace Nora.Shop.DataAccess.Seed
{
    public static class NoraShopCatalogSeed
    {
        public static async Task EnsureSeedDataAsync(NoraShopContext context)
        {
            await context.Database.MigrateAsync();

            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new() { Name = "Oturma", Description = "Salon ve yaşam alanları için dengeli oturma çözümleri." },
                    new() { Name = "Çalışma", Description = "Ev ofis ve profesyonel kullanım için sade çalışma ürünleri." },
                    new() { Name = "Aydınlatma", Description = "Yumuşak ışık veren modern aydınlatma ürünleri." },
                    new() { Name = "Depolama", Description = "Düzeni destekleyen raf ve konsol seçenekleri." }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            var categoryMap = await context.Categories
                .ToDictionaryAsync(category => category.Name, category => category.Id);

            if (categoryMap.Count == 0)
            {
                return;
            }

            int ResolveCategoryId(string categoryName, int fallbackIndex)
            {
                if (categoryMap.TryGetValue(categoryName, out var categoryId))
                {
                    return categoryId;
                }

                return categoryMap.Values.ElementAt(fallbackIndex % categoryMap.Count);
            }

            var catalogItems = new List<Product>
            {
                new()
                {
                    Name = "Luna Berjer",
                    Description = "Mat dokulu kumaşı ve yalın formu ile sessiz köşeler için tasarlandı.",
                    Price = 12499,
                    Stock = 8,
                    CategoryId = ResolveCategoryId("Oturma", 0),
                    ImageUrl = "/images/products/luna-berjer.png"
                },
                new()
                {
                    Name = "Atlas Çalışma Masası",
                    Description = "Geniş üst yüzeyi ve çizgisel gövdesiyle günlük çalışma düzenini sadeleştirir.",
                    Price = 18990,
                    Stock = 5,
                    CategoryId = ResolveCategoryId("Çalışma", 1),
                    ImageUrl = "/images/products/atlas-masa.png"
                },
                new()
                {
                    Name = "Mira Lambader",
                    Description = "Yumuşak ışık dağılımı ile okuma alanlarında sakin bir atmosfer kurar.",
                    Price = 5990,
                    Stock = 11,
                    CategoryId = ResolveCategoryId("Aydınlatma", 2),
                    ImageUrl = "/images/products/mira-lambader.png"
                },
                new()
                {
                    Name = "Vera Konsol",
                    Description = "Dar alanlarda düzeni koruyan, dengeli oranlara sahip modern konsol.",
                    Price = 15490,
                    Stock = 4,
                    CategoryId = ResolveCategoryId("Depolama", 3),
                    ImageUrl = "/images/products/vera-konsol.png"
                },
                new()
                {
                    Name = "Mono Kanepe",
                    Description = "Düşük profilli yapısı ve geniş oturumu ile yaşam alanını sakinleştirir.",
                    Price = 27990,
                    Stock = 3,
                    CategoryId = ResolveCategoryId("Oturma", 0),
                    ImageUrl = "/images/products/mono-kanepe.png"
                },
                new()
                {
                    Name = "Frame Raf Ünitesi",
                    Description = "Kitap, obje ve dosya yerleşimi için yalın, düzenli bir taşıyıcı çözüm.",
                    Price = 9490,
                    Stock = 6,
                    CategoryId = ResolveCategoryId("Depolama", 3),
                    ImageUrl = "/images/products/frame-raf.png"
                }
            };

            var imageUpdates = new Dictionary<string, string>
            {
                ["/images/products/luna-berjer.svg"] = "/images/products/luna-berjer.png",
                ["/images/products/atlas-masa.svg"] = "/images/products/atlas-masa.png",
                ["/images/products/mira-lambader.svg"] = "/images/products/mira-lambader.png",
                ["/images/products/vera-konsol.svg"] = "/images/products/vera-konsol.png",
                ["/images/products/mono-kanepe.svg"] = "/images/products/mono-kanepe.png",
                ["/images/products/frame-raf.svg"] = "/images/products/frame-raf.png"
            };

            var existingProducts = await context.Products.ToListAsync();
            var hasChanges = false;

            foreach (var existingProduct in existingProducts)
            {
                if (!string.IsNullOrWhiteSpace(existingProduct.ImageUrl) &&
                    imageUpdates.TryGetValue(existingProduct.ImageUrl, out var newImageUrl))
                {
                    existingProduct.ImageUrl = newImageUrl;
                    hasChanges = true;
                    continue;
                }

                var matchingCatalogItem = catalogItems.FirstOrDefault(item => item.Name == existingProduct.Name);
                if (matchingCatalogItem is not null && existingProduct.ImageUrl != matchingCatalogItem.ImageUrl)
                {
                    existingProduct.ImageUrl = matchingCatalogItem.ImageUrl;
                    hasChanges = true;
                }
            }

            if (existingProducts.Count == 0)
            {
                await context.Products.AddRangeAsync(catalogItems);
                hasChanges = true;
            }

            if (hasChanges)
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
