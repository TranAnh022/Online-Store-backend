﻿// <auto-generated />
using System;
using Ecommerce.Core.src.ValueObjects;
using Ecommerce.WebAPI.src.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.WebAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240515103852_Add_Seed_Data")]
    partial class Add_Seed_Data
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "order_status", new[] { "pending", "completed", "shipped", "cancelled", "processing" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "user_role", new[] { "admin", "user" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ecommerce.Core.src.Entities.CartAggregate.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("cart_pkey");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_carts_user_id");

                    b.ToTable("carts", null, t =>
                        {
                            t.HasCheckConstraint("valid_update_time", "updated_at >= created_at");
                        });
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.CartAggregate.CartItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CartId")
                        .HasColumnType("uuid")
                        .HasColumnName("cart_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("cart_item_pkey");

                    b.HasIndex("CartId")
                        .HasDatabaseName("ix_cart_items_cart_id");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_cart_items_product_id");

                    b.ToTable("cart_items", null, t =>
                        {
                            t.HasCheckConstraint("quantity_check", "quantity > 0");
                        });
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("categories_pkey");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("categories_name_key");

                    b.ToTable("categories", null, t =>
                        {
                            t.HasCheckConstraint("categories_image_check", "image LIKE 'http%' OR image = ''");
                        });

                    b.HasData(
                        new
                        {
                            Id = new Guid("15ab95f1-3589-4f21-9ec1-65018f83412d"),
                            Image = "https://images.unsplash.com/photo-1526738549149-8e07eca6c147?q=80&w=1925&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                            Name = "Electronics"
                        },
                        new
                        {
                            Id = new Guid("af6d1a1b-f69a-4d36-9f33-0e971ed9c34c"),
                            Image = "https://plus.unsplash.com/premium_photo-1682435561654-20d84cef00eb?q=80&w=1918&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                            Name = "Shoes"
                        },
                        new
                        {
                            Id = new Guid("b22247c5-1b6d-4c99-95ce-7a35836949ab"),
                            Image = "https://images.unsplash.com/photo-1556912173-3bb406ef7e77?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                            Name = "Home Goods"
                        },
                        new
                        {
                            Id = new Guid("3044bebd-3597-482c-8a86-f8f7c58c2d98"),
                            Image = "https://images.unsplash.com/photo-1500995617113-cf789362a3e1?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                            Name = "Toys"
                        });
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.OrderAggregate.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<OrderStatus>("Status")
                        .HasColumnType("order_status")
                        .HasColumnName("status");

                    b.Property<decimal>("TotalPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("total_price");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("orders_pkey");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_orders_user_id");

                    b.ToTable("orders", null, t =>
                        {
                            t.HasCheckConstraint("total_price_check", "total_price > 0");

                            t.HasCheckConstraint("updated_check", "updated_at >= created_at");
                        });
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.OrderAggregate.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("order_id");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("price");

                    b.Property<Guid>("ProductSnapshotId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_snapshot_id");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("order_item_pkey");

                    b.HasIndex("OrderId")
                        .HasDatabaseName("ix_order_items_order_id");

                    b.HasIndex("ProductSnapshotId")
                        .HasDatabaseName("ix_order_items_product_snapshot_id");

                    b.ToTable("order_items", null, t =>
                        {
                            t.HasCheckConstraint("price_check", "price > 0");

                            t.HasCheckConstraint("quantity_check", "quantity > 0");
                        });
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("Inventory")
                        .HasColumnType("integer")
                        .HasColumnName("inventory");

                    b.Property<decimal?>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("price");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("product_pkey");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_products_category_id");

                    b.HasIndex("Title")
                        .IsUnique()
                        .HasDatabaseName("title_unique");

                    b.ToTable("products", null, t =>
                        {
                            t.HasCheckConstraint("products_price_check", "price > 0");
                        });

                    b.HasData(
                        new
                        {
                            Id = new Guid("51bf416a-e30b-4e11-acd3-eb99e33f5c9d"),
                            CategoryId = new Guid("15ab95f1-3589-4f21-9ec1-65018f83412d"),
                            Description = "The Smartphone X10 is packed with cutting-edge features, including a high-resolution camera and a powerful processor, making it perfect for staying connected and productive on the go.",
                            Inventory = 100,
                            Price = 499.99m,
                            Title = "Smartphone X10"
                        },
                        new
                        {
                            Id = new Guid("1e7d5cdd-d145-4ebf-98d9-bf88d1983c81"),
                            CategoryId = new Guid("15ab95f1-3589-4f21-9ec1-65018f83412d"),
                            Description = "The Laptop ProBook 2022 offers superior performance and reliability, featuring a sleek design, long-lasting battery, and advanced security features for your peace of mind.",
                            Inventory = 120,
                            Price = 899.99m,
                            Title = "Laptop ProBook 2022"
                        },
                        new
                        {
                            Id = new Guid("e4bb0c50-a10f-497e-b04f-566024ce8e3b"),
                            CategoryId = new Guid("af6d1a1b-f69a-4d36-9f33-0e971ed9c34c"),
                            Description = "Designed for serious runners, the Running Shoes Elite provide exceptional comfort, support, and durability, allowing you to achieve your personal best with every stride.",
                            Inventory = 80,
                            Price = 129.99m,
                            Title = "Running Shoes Elite"
                        },
                        new
                        {
                            Id = new Guid("d23aa941-3376-473b-83f7-9f278dd9cbe6"),
                            CategoryId = new Guid("af6d1a1b-f69a-4d36-9f33-0e971ed9c34c"),
                            Description = "Conquer any trail with confidence in the Hiking Boots Adventure. These rugged boots offer superior traction, waterproof protection, and unmatched durability for your outdoor adventures.",
                            Inventory = 90,
                            Price = 149.99m,
                            Title = "Hiking Boots Adventure"
                        },
                        new
                        {
                            Id = new Guid("4b0d0afc-312a-485d-96bc-2ffb5ffc413b"),
                            CategoryId = new Guid("b22247c5-1b6d-4c99-95ce-7a35836949ab"),
                            Description = "Transform your home into a smart sanctuary with the Smart Home Hub Plus. Control your lights, appliances, and security cameras with ease, and enjoy the convenience of voice commands and automated routines.",
                            Inventory = 110,
                            Price = 199.99m,
                            Title = "Smart Home Hub Plus"
                        },
                        new
                        {
                            Id = new Guid("1cb9c534-5083-4961-a64e-6642cb93508e"),
                            CategoryId = new Guid("b22247c5-1b6d-4c99-95ce-7a35836949ab"),
                            Description = "Say goodbye to manual vacuuming with the Robot Vacuum Pro. This intelligent robot cleaner navigates your home effortlessly, removing dirt, dust, and pet hair with precision, leaving your floors spotless.",
                            Inventory = 95,
                            Price = 299.99m,
                            Title = "Robot Vacuum Pro"
                        },
                        new
                        {
                            Id = new Guid("053945d1-c000-427f-b70c-26efbbea8e92"),
                            CategoryId = new Guid("3044bebd-3597-482c-8a86-f8f7c58c2d98"),
                            Description = "Gather your friends and family for hours of fun with the Board Game Bonanza. Featuring a variety of classic and modern games, this collection is sure to entertain players of all ages.",
                            Inventory = 105,
                            Price = 39.99m,
                            Title = "Board Game Bonanza"
                        },
                        new
                        {
                            Id = new Guid("172039b2-b025-4a5e-af38-252a96e49d26"),
                            CategoryId = new Guid("3044bebd-3597-482c-8a86-f8f7c58c2d98"),
                            Description = "Create unforgettable memories with the Outdoor Playset Deluxe. This premium playset includes a slide, swings, and a climbing wall, providing endless entertainment for kids in the backyard.",
                            Inventory = 85,
                            Price = 499.99m,
                            Title = "Outdoor Playset Deluxe"
                        });
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.ProductImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("url");

                    b.HasKey("Id")
                        .HasName("product_images_pkey");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_product_images_product_id");

                    b.ToTable("product_images", null, t =>
                        {
                            t.HasCheckConstraint("url_check", "url LIKE 'http%' OR url = ''");
                        });

                    b.HasData(
                        new
                        {
                            Id = new Guid("789dfc1c-d8e8-4453-b735-951973e542d4"),
                            ProductId = new Guid("51bf416a-e30b-4e11-acd3-eb99e33f5c9d"),
                            Url = "https://images.unsplash.com/photo-1598327105666-5b89351aff97?q=80&w=2118&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                        },
                        new
                        {
                            Id = new Guid("8600e3eb-cd68-4378-861f-15cf7611453f"),
                            ProductId = new Guid("1e7d5cdd-d145-4ebf-98d9-bf88d1983c81"),
                            Url = "https://images.unsplash.com/photo-1559163499-413811fb2344?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                        },
                        new
                        {
                            Id = new Guid("e9c89a2a-f4c2-44a8-9860-963c58535a9a"),
                            ProductId = new Guid("e4bb0c50-a10f-497e-b04f-566024ce8e3b"),
                            Url = "https://images.unsplash.com/photo-1562183241-b937e95585b6?q=80&w=1965&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                        },
                        new
                        {
                            Id = new Guid("9f23015b-3b69-4794-a5ee-12ebcc9cae48"),
                            ProductId = new Guid("d23aa941-3376-473b-83f7-9f278dd9cbe6"),
                            Url = "https://images.unsplash.com/photo-1575987116913-e96e7d490b8a?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                        },
                        new
                        {
                            Id = new Guid("d758aa87-05fd-422a-beb1-71eb403fdfa4"),
                            ProductId = new Guid("4b0d0afc-312a-485d-96bc-2ffb5ffc413b"),
                            Url = "https://images.unsplash.com/photo-1558089687-f282ffcbc126?q=80&w=2071&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                        },
                        new
                        {
                            Id = new Guid("de3633ae-45be-4f34-8e3a-a525cc1ee238"),
                            ProductId = new Guid("1cb9c534-5083-4961-a64e-6642cb93508e"),
                            Url = "https://images.unsplash.com/photo-1603618090554-f7a5079ffb54?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                        },
                        new
                        {
                            Id = new Guid("f0e2f8ce-68de-4704-84d6-e1312e54ef4c"),
                            ProductId = new Guid("053945d1-c000-427f-b70c-26efbbea8e92"),
                            Url = "https://images.unsplash.com/photo-1610890716171-6b1bb98ffd09?q=80&w=1931&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                        },
                        new
                        {
                            Id = new Guid("4b420343-6bed-4848-a0b1-c24e1efa009e"),
                            ProductId = new Guid("172039b2-b025-4a5e-af38-252a96e49d26"),
                            Url = "https://images.unsplash.com/photo-1587408163744-8482f78bc883?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                        },
                        new
                        {
                            Id = new Guid("b5bac06c-14cf-446e-899f-6a4cd22b4d4d"),
                            ProductId = new Guid("1cb9c534-5083-4961-a64e-6642cb93508e"),
                            Url = "https://images.unsplash.com/photo-1603618090561-412154b4bd1b?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1yZWxhdGVkfDF8fHxlbnwwfHx8fHw%3D"
                        },
                        new
                        {
                            Id = new Guid("d01eb6aa-0f4f-4175-847a-0cd76e8b1af0"),
                            ProductId = new Guid("51bf416a-e30b-4e11-acd3-eb99e33f5c9d"),
                            Url = "https://images.unsplash.com/photo-1598327106026-d9521da673d1?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1yZWxhdGVkfDF8fHxlbnwwfHx8fHw%3D"
                        });
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Context")
                        .HasColumnType("text")
                        .HasColumnName("context");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<int>("Rating")
                        .HasColumnType("integer")
                        .HasColumnName("rating");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("review_pkey");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_reviews_product_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_reviews_user_id");

                    b.ToTable("reviews", null, t =>
                        {
                            t.HasCheckConstraint("rating_check", "rating >= 1 AND rating <= 5");
                        });
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Avatar")
                        .HasColumnType("text")
                        .HasColumnName("avatar");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password");

                    b.Property<UserRole>("Role")
                        .HasColumnType("user_role")
                        .HasColumnName("role");

                    b.HasKey("Id")
                        .HasName("users_pkey");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("users_email_key");

                    b.ToTable("users", null, t =>
                        {
                            t.HasCheckConstraint("users_avatar_check", "avatar LIKE 'http%' OR avatar = ''");
                        });

                    b.HasData(
                        new
                        {
                            Id = new Guid("8f479818-eec3-41ce-a605-f8d27c3315c7"),
                            Avatar = "https://static.vecteezy.com/system/resources/thumbnails/006/487/917/small_2x/man-avatar-icon-free-vector.jpg",
                            Email = "admin@mail.com",
                            Name = "Admin",
                            Password = "AQAAAAIAAYagAAAAEHCMYJvY4mdbnzK5hSGJYGnysq0LPz8hnn7OYmkCAXrsnGbD91kWc+JKykH/nhSR+Q==",
                            Role = UserRole.Admin
                        },
                        new
                        {
                            Id = new Guid("03dc9c48-1ed6-4f49-9c61-9cf67f2ac34b"),
                            Avatar = "https://static.vecteezy.com/system/resources/thumbnails/006/487/917/small_2x/man-avatar-icon-free-vector.jpg",
                            Email = "john@mail.com",
                            Name = "John",
                            Password = "AQAAAAIAAYagAAAAEHJcr9WQaOHRM8dXE8uNHSDmuq1RdCBYj9mMq3dDEQ+btftdHAX0oidjDoAMI8Qjyg==",
                            Role = UserRole.User
                        });
                });

            modelBuilder.Entity("Ecommerce.Core.src.ValueObjects.ProductSnapshot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<string>("Title")
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_product_snapshots");

                    b.ToTable("product_snapshots", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.CartAggregate.Cart", b =>
                {
                    b.HasOne("Ecommerce.Core.src.Entities.User", "User")
                        .WithOne("Cart")
                        .HasForeignKey("Ecommerce.Core.src.Entities.CartAggregate.Cart", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_carts_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.CartAggregate.CartItem", b =>
                {
                    b.HasOne("Ecommerce.Core.src.Entities.CartAggregate.Cart", null)
                        .WithMany("CartItems")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_cart_items_carts_cart_id");

                    b.HasOne("Ecommerce.Core.src.Entities.Product", "Product")
                        .WithMany("CartItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_cart_items_products_product_id");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.OrderAggregate.Order", b =>
                {
                    b.HasOne("Ecommerce.Core.src.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_orders_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.OrderAggregate.OrderItem", b =>
                {
                    b.HasOne("Ecommerce.Core.src.Entities.OrderAggregate.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_order_items_orders_order_id");

                    b.HasOne("Ecommerce.Core.src.ValueObjects.ProductSnapshot", "ProductSnapshot")
                        .WithMany()
                        .HasForeignKey("ProductSnapshotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_order_items_product_snapshots_product_snapshot_id");

                    b.Navigation("Order");

                    b.Navigation("ProductSnapshot");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.Product", b =>
                {
                    b.HasOne("Ecommerce.Core.src.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_products_categories_category_id");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.ProductImage", b =>
                {
                    b.HasOne("Ecommerce.Core.src.Entities.Product", "Product")
                        .WithMany("Images")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_product_images_products_product_id");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.Review", b =>
                {
                    b.HasOne("Ecommerce.Core.src.Entities.Product", "Product")
                        .WithMany("Reviews")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_reviews_products_product_id");

                    b.HasOne("Ecommerce.Core.src.Entities.User", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_reviews_users_user_id");

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.CartAggregate.Cart", b =>
                {
                    b.Navigation("CartItems");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.OrderAggregate.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.Product", b =>
                {
                    b.Navigation("CartItems");

                    b.Navigation("Images");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entities.User", b =>
                {
                    b.Navigation("Cart")
                        .IsRequired();

                    b.Navigation("Orders");

                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}