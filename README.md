# Nora Shop

Nora Shop, ASP.NET Core MVC ve ASP.NET Core Web API olarak iki ayrı sunum projesi bulunan, katmanlı mimariye sahip bir e-ticaret uygulamasıdır.

## Çözüm Yapısı

- `E-Ticaret Projesi`: MVC kullanıcı arayüzü ve admin paneli
- `Nora.Shop.WebAPI`: REST API projesi
- `Nora.Shop.Business`: iş kuralları ve servis katmanı
- `Nora.Shop.Core`: entity sınıfları ve repository arayüzleri
- `Nora.Shop.Data.Accsess`: EF Core DbContext, migration, seed ve repository uygulamaları

## Kullanılan Teknolojiler

- ASP.NET Core MVC
- ASP.NET Core Web API
- ASP.NET Core Identity
- Entity Framework Core
- SQL Server
- Generic Repository Pattern
- Entity bazlı repository yapısı
- Dependency Injection
- Bootstrap

## MVC Özellikleri

- Kullanıcı kayıt, giriş ve çıkış işlemleri
- Ürün listeleme, arama, kategori filtresi ve detay sayfası
- Sepete ürün ekleme ve sepetten ürün silme
- Sipariş oluşturma ve sipariş geçmişi görüntüleme
- Admin panelinde ürün, kategori ve sipariş yönetimi
- Admin rolü ile yetkilendirilmiş yönetim ekranları

## Web API Controllerları

Varsayılan `WeatherForecast` controllerı kaldırılmıştır. API projesinde e-ticaret alanına ait controllerlar kullanılır.

- `AccountController`: kayıt, giriş, çıkış ve profil bilgisi
- `ProductsController`: ürün CRUD işlemleri
- `CategoriesController`: kategori CRUD işlemleri
- `CartsController`: kullanıcı sepeti listeleme, ekleme, silme ve temizleme
- `OrdersController`: sipariş listeleme, kullanıcı siparişleri, sipariş oluşturma ve durum güncelleme

## API Endpointleri

### Account

- `POST /api/account/register`
- `POST /api/account/login`
- `POST /api/account/logout`
- `GET /api/account/profile/{email}`

### Products

- `GET /api/products`
- `GET /api/products/{id}`
- `POST /api/products`
- `PUT /api/products/{id}`
- `DELETE /api/products/{id}`

### Categories

- `GET /api/categories`
- `GET /api/categories/{id}`
- `POST /api/categories`
- `PUT /api/categories/{id}`
- `DELETE /api/categories/{id}`

### Cart

- `GET /api/users/{userId}/cart`
- `POST /api/users/{userId}/cart`
- `DELETE /api/users/{userId}/cart/{cartId}`
- `DELETE /api/users/{userId}/cart`

### Orders

- `GET /api/orders`
- `GET /api/orders/{id}`
- `GET /api/orders/user/{userId}`
- `POST /api/orders/user/{userId}`
- `PATCH /api/orders/{id}/status`

## Repository Yapısı

Core katmanında genel repository arayüzü ve entity bazlı repository arayüzleri bulunur.

- `IRepository<T>`
- `IProductRepository`
- `ICategoryRepository`
- `ICartRepository`
- `IOrderRepository`

DataAccess katmanında bu arayüzlerin uygulamaları bulunur.

- `GenericRepository<T>`
- `ProductRepository`
- `CategoryRepository`
- `CartRepository`
- `OrderRepository`

Business servisleri doğrudan `DbContext` ile çalışmaz; repository arayüzleri üzerinden veri erişimi yapar.

## Veritabanı ve Seed

- Veritabanı işlemleri Entity Framework Core ile yönetilir.
- Identity tabloları aynı DbContext içinde tutulur.
- Örnek kategori ve ürünler uygulama açılışında seed edilir.
- Admin hesabı güvenlik nedeniyle public dosyada sabit şifreyle tutulmaz; `User Secrets` veya ortam değişkeni ile tanımlanır.

## Demo Admin Kurulumu

```powershell
dotnet user-secrets set "SeedAdmin:Email" "admin@demo.local" --project ".\E-Ticaret Projesi\E-Ticaret Projesi.csproj"
dotnet user-secrets set "SeedAdmin:Password" "GucluBirSifre123!" --project ".\E-Ticaret Projesi\E-Ticaret Projesi.csproj"
dotnet user-secrets set "SeedAdmin:FullName" "Demo Admin" --project ".\E-Ticaret Projesi\E-Ticaret Projesi.csproj"
```

## Derleme

```powershell
dotnet build "Nora.Shop.WebAPI\Nora.Shop.WebAPI.csproj" /m:1 /p:UseSharedCompilation=false
dotnet build "E-Ticaret Projesi\E-Ticaret Projesi.csproj" /m:1 /p:UseSharedCompilation=false
```

`/m:1` parametresi, bazı Windows ortamlarında restore sırasında oluşabilen paralel MSBuild erişim hatalarını önlemek için kullanılır.
