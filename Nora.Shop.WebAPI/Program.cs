using Microsoft.EntityFrameworkCore;
using Nora.Shop.Business.Services;
using Nora.Shop.Core.Interfaces;
using Nora.Shop.DataAccess.Context;
using Nora.Shop.DataAccess.Repository;
using Nora.Shop.DataAccess.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NoraShopContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<NoraShopContext>();
    await NoraShopCatalogSeed.EnsureSeedDataAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
