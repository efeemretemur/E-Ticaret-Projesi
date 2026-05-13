# Nora Shop

Nora Shop, ASP.NET Core tabanlı bir e-ticaret bitirme projesidir. Projede kullanıcı üyelik sistemi, ürün kataloğu, sepet ve sipariş akışı, admin paneli ve Web API katmanı birlikte yer almaktadır.

## Proje Özeti

Bu proje, yazılım uzmanlığı bitirme projesi kapsamında geliştirilmiş çok katmanlı bir e-ticaret uygulamasıdır. Amaç; hem kullanıcı tarafında çalışan bir mağaza deneyimi sunmak hem de admin paneli üzerinden ürün, kategori ve sipariş yönetimini gerçekleştirmektir.

## Kullanılan Teknolojiler

- ASP.NET Core MVC
- ASP.NET Core Identity
- Entity Framework Core
- SQL Server
- Web API
- Generic Repository Pattern
- Katmanlı Mimari
- Bootstrap

## Proje Özellikleri

### Kullanıcı Tarafı

- Üye kayıt olma
- Giriş yapma ve çıkış yapma
- Ürün listeleme
- Kategoriye göre filtreleme
- Ürün arama
- Ürün detay sayfası
- Sepete ürün ekleme
- Sepetten ürün kaldırma
- Ödeme formu ile sipariş oluşturma
- Sipariş geçmişini görüntüleme

### Admin Paneli

- Ürün ekleme, güncelleme, silme
- Kategori ekleme, güncelleme, silme
- Siparişleri listeleme
- Sipariş durumunu güncelleme

### Web API

- Ürünler için CRUD işlemleri
- Kategoriler için CRUD işlemleri

## Katmanlı Mimari

Projede katmanlı mimari kullanılmıştır.

- `Nora.Shop.Core`
  - Entity sınıfları
  - Interface tanımları

- `Nora.Shop.Data.Accsess`
  - EF Core DbContext
  - Migration dosyaları
  - Repository katmanı
  - Seed işlemleri

- `Nora.Shop.Business`
  - Servisler
  - İş kuralları

- `E-Ticaret Projesi`
  - MVC arayüzü
  - Kullanıcı ekranları
  - Admin paneli

- `Nora.Shop.WebAPI`
  - API katmanı

## Veritabanı ve Seed Yapısı

- Veritabanı işlemleri `Entity Framework Core` ile yönetilmektedir.
- Uygulama açılışında migration kontrolü yapılır.
- Gerekli roller oluşturulur.
- Admin kullanıcısı yoksa otomatik eklenir.
- Örnek kategori ve ürünler otomatik olarak sisteme yüklenir.

## Admin Giriş Bilgileri

- E-posta: `admin@norashop.com`
- Şifre: `Admin123!`

## API Endpointleri

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

## Kurulum Adımları

1. Projeyi bilgisayarınıza indirin.
2. `appsettings.json` içindeki `DefaultConnection` bilgisini kontrol edin.
3. Veritabanı bağlantınızın aktif olduğundan emin olun.
4. MVC projesini veya Web API projesini çalıştırın.
5. Uygulama ilk açılışta gerekli verileri otomatik olarak oluşturacaktır.

## Projeyi Çalıştırma

### MVC Uygulaması

Başlangıç projesi olarak `E-Ticaret Projesi` seçilip çalıştırılabilir.

### Web API

Başlangıç projesi olarak `Nora.Shop.WebAPI` seçilip çalıştırılabilir.

## Projede Uygulanan Yapılar

- Generic Repository Pattern
- Katmanlı Mimari
- Dependency Injection
- Identity ile üyelik sistemi
- EF Core migration yapısı
- Validation destekli form yapıları

## Notlar

- Sipariş oluşturulunca stoktan düşüm yapılır.
- Kategoriye bağlı ürün varsa kategori silme işlemi engellenir.
- Admin paneli yalnızca `Admin` rolündeki kullanıcılar tarafından kullanılabilir.
- API tarafında da ürün ve kategori işlemleri ayrı controller yapıları ile sunulmaktadır.

## Teslim İçeriği

Bu proje bitirme projesi teslimi için aşağıdaki maddeleri desteklemektedir:

- Frontend + Backend
- Admin Paneli
- Generic Repository Pattern
- Üyelik Sistemi
- E-ticaret modülleri
- SOLID ve katmanlı yapı yaklaşımı
- EF Core kullanımı
- Web API katmanı

## Geliştirici

- Temur
