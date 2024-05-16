using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Entities.CartAggregate;
using Ecommerce.Core.src.Entities.OrderAggregate;
using Ecommerce.Core.src.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartsItem { get; set; } = null!;
        public DbSet<ProductImage> ProductImages { get; set; } = null!;
        public DbSet<ProductSnapshot> ProductSnapshots { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config) : base(options)
        {
            _config = config;
            this.ChangeTracker.LazyLoadingEnabled = true;
        }
        static AppDbContext()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(u => u.Id).HasName("users_pkey");
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(u => u.Email).IsUnique().HasDatabaseName("users_email_key");
                entity.Property(u => u.Name).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Password).IsRequired().HasMaxLength(255);
                entity.HasCheckConstraint("users_avatar_check", "avatar LIKE 'http%' OR avatar = ''");
                entity.HasData(SeedingData.GetUsersSeed());
            });
            // Configuring Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");
                entity.HasKey(o => o.Id).HasName("orders_pkey");
                entity.ToTable("orders").HasCheckConstraint("total_price_check", "total_price > 0");
                entity.HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.SetNull);
                entity.Property(o => o.CreatedAt).HasDefaultValueSql("now()");
                entity.Property(o => o.UpdatedAt).HasDefaultValueSql("now()");
                entity.HasCheckConstraint("updated_check", "updated_at >= created_at");
            });

            // Configuring Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");
                entity.HasKey(c => c.Id).HasName("categories_pkey");
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(c => c.Name).IsUnique().HasDatabaseName("categories_name_key");
                entity.Property(c => c.Image).IsRequired();
                entity.HasCheckConstraint("categories_image_check", "image LIKE 'http%' OR image = ''");
                entity.HasData(SeedingData.GetCategoriesSeed());
            });

            // Configuring Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");
                entity.HasKey(p => p.Id).HasName("product_pkey");
                entity.Property(p => p.Title).IsRequired().HasMaxLength(255);
                entity.HasIndex(p => p.Title).IsUnique().HasDatabaseName("title_unique");
                entity.Property(p => p.Price).HasPrecision(18, 2);
                entity.HasCheckConstraint("products_price_check", "price > 0");
                entity.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.SetNull);
                entity.HasData(SeedingData.GetProductsSeed());
            });
            // Configuring ProductImage entity
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.ToTable("product_images");
                entity.HasKey(pi => pi.Id).HasName("product_images_pkey");
                entity.HasOne(pi => pi.Product).WithMany(p => p.Images).HasForeignKey(pi => pi.ProductId).OnDelete(DeleteBehavior.Cascade);
                entity.Property(pi => pi.Url).IsRequired();
                entity.HasCheckConstraint("url_check", "url LIKE 'http%' OR url = ''");
                entity.HasData(SeedingData.GetProductImagesSeed());
            });

            // Configuring Cart entity
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("carts");
                entity.HasKey(c => c.Id).HasName("cart_pkey");
                entity.HasOne(c => c.User).WithOne(u => u.Cart).HasForeignKey<Cart>(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.Property(c => c.CreatedAt).HasDefaultValueSql("now()");
                entity.Property(c => c.UpdatedAt).HasDefaultValueSql("now()");
                entity.HasCheckConstraint("valid_update_time", "updated_at >= created_at");
                // Configure the navigation property to use a specific backing field
                var navigation = entity.Metadata.FindNavigation(nameof(Cart.CartItems));
                navigation!.SetField("_items");
                navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            });

            // Configuring CartItem entity
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.ToTable("cart_items");
                entity.HasKey(ci => ci.Id).HasName("cart_item_pkey");
                entity.HasOne(ci => ci.Product).WithMany(p => p.CartItems).HasForeignKey(ci => ci.ProductId).OnDelete(DeleteBehavior.SetNull);
                entity.HasCheckConstraint("quantity_check", "quantity > 0");
            });

            // Configuring OrderItem entity
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("order_items");
                entity.HasKey(oi => oi.Id).HasName("order_item_pkey");
                entity.HasOne(oi => oi.Order).WithMany(o => o.OrderItems).HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);
                entity.HasCheckConstraint("quantity_check", "quantity > 0");
                entity.Property(oi => oi.Price).HasPrecision(18, 2);
                entity.HasCheckConstraint("price_check", "price > 0");
            });

            // Configuring Review entity
            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("reviews");
                entity.HasKey(r => r.Id).HasName("review_pkey");
                entity.HasOne(r => r.User).WithMany(u => u.Reviews).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(r => r.Product).WithMany(p => p.Reviews).HasForeignKey(r => r.ProductId).OnDelete(DeleteBehavior.SetNull);
                entity.HasCheckConstraint("rating_check", "rating >= 1 AND rating <= 5");
            });


            modelBuilder.HasPostgresEnum<UserRole>();
            modelBuilder.HasPostgresEnum<OrderStatus>();

            base.OnModelCreating(modelBuilder);
        }
    }

}