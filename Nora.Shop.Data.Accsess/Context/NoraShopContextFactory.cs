using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Text.Json;

namespace Nora.Shop.DataAccess.Context
{
    public class NoraShopContextFactory : IDesignTimeDbContextFactory<NoraShopContext>
    {
        public NoraShopContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NoraShopContext>();
            var connectionString = ResolveConnectionString();

            optionsBuilder.UseSqlServer(
                connectionString);

            return new NoraShopContext(optionsBuilder.Options);
        }

        private static string ResolveConnectionString()
        {
            var candidates = new[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), "E-Ticaret Projesi", "appsettings.json"),
                Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"),
                Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "E-Ticaret Projesi", "appsettings.json"))
            };

            foreach (var path in candidates.Distinct())
            {
                if (!File.Exists(path))
                {
                    continue;
                }

                using var document = JsonDocument.Parse(File.ReadAllText(path));
                if (document.RootElement.TryGetProperty("ConnectionStrings", out var connectionStrings) &&
                    connectionStrings.TryGetProperty("DefaultConnection", out var defaultConnection) &&
                    !string.IsNullOrWhiteSpace(defaultConnection.GetString()))
                {
                    return defaultConnection.GetString()!;
                }
            }

            return "Server=(local);Database=NoraShopDB;Integrated Security=True;TrustServerCertificate=True;Encrypt=False";
        }
    }
}
