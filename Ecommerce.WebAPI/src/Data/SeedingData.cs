// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Ecommerce.Core.src.Entities;
// using Ecommerce.Core.src.ValueObjects;
// using Microsoft.AspNetCore.Identity;

// namespace Ecommerce.WebAPI.src.Data
// {
//     public class SeedingData
//     {
//         private static readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();


//         private static Category category1 = new Category("Electronics", "https://images.unsplash.com/photo-1526738549149-8e07eca6c147?q=80&w=1925&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
//         private static Category category2 = new Category("Shoes", "https://plus.unsplash.com/premium_photo-1682435561654-20d84cef00eb?q=80&w=1918&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
//         private static Category category3 = new Category("Home Goods", "https://images.unsplash.com/photo-1556912173-3bb406ef7e77?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
//         private static Category category4 = new Category("Toys", "https://images.unsplash.com/photo-1500995617113-cf789362a3e1?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");

//         public static List<Category> GetCategoriesSeed()
//         {
//             return new List<Category>
//             {
//                 category1, category2, category3, category4
//             };
//         }

//         private static Product product1 = new Product("Smartphone X10", 499.99m, "The Smartphone X10 is packed with cutting-edge features, including a high-resolution camera and a powerful processor, making it perfect for staying connected and productive on the go.", category1.Id, 100);
//         private static Product product2 = new Product("Laptop ProBook 2022", 899.99m, "The Laptop ProBook 2022 offers superior performance and reliability, featuring a sleek design, long-lasting battery, and advanced security features for your peace of mind.", category1.Id, 120);
//         private static Product product3 = new Product("Running Shoes Elite", 129.99m, "Designed for serious runners, the Running Shoes Elite provide exceptional comfort, support, and durability, allowing you to achieve your personal best with every stride.", category2.Id, 80);
//         private static Product product4 = new Product("Hiking Boots Adventure", 149.99m, "Conquer any trail with confidence in the Hiking Boots Adventure. These rugged boots offer superior traction, waterproof protection, and unmatched durability for your outdoor adventures.", category2.Id, 90);
//         private static Product product5 = new Product("Smart Home Hub Plus", 199.99m, "Transform your home into a smart sanctuary with the Smart Home Hub Plus. Control your lights, appliances, and security cameras with ease, and enjoy the convenience of voice commands and automated routines.", category3.Id, 110);
//         private static Product product6 = new Product("Robot Vacuum Pro", 299.99m, "Say goodbye to manual vacuuming with the Robot Vacuum Pro. This intelligent robot cleaner navigates your home effortlessly, removing dirt, dust, and pet hair with precision, leaving your floors spotless.", category3.Id, 95);
//         private static Product product7 = new Product("Board Game Bonanza", 39.99m, "Gather your friends and family for hours of fun with the Board Game Bonanza. Featuring a variety of classic and modern games, this collection is sure to entertain players of all ages.", category4.Id, 105);
//         private static Product product8 = new Product("Outdoor Playset Deluxe", 499.99m, "Create unforgettable memories with the Outdoor Playset Deluxe. This premium playset includes a slide, swings, and a climbing wall, providing endless entertainment for kids in the backyard.", category4.Id, 85);

//         public static List<Product> GetProductsSeed()
//         {
//             return new List<Product>
//             {
//                 product1, product2, product3, product4, product5, product6, product7, product8
//             };
//         }

//         private static ProductImage image1 = new ProductImage(product1.Id, "https://images.unsplash.com/photo-1598327105666-5b89351aff97?q=80&w=2118&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
//         private static ProductImage image2 = new ProductImage(product2.Id, "https://images.unsplash.com/photo-1559163499-413811fb2344?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
//         private static ProductImage image3 = new ProductImage(product3.Id, "https://images.unsplash.com/photo-1562183241-b937e95585b6?q=80&w=1965&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
//         private static ProductImage image4 = new ProductImage(product4.Id, "https://images.unsplash.com/photo-1575987116913-e96e7d490b8a?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
//         private static ProductImage image5 = new ProductImage(product5.Id, "https://images.unsplash.com/photo-1558089687-f282ffcbc126?q=80&w=2071&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
//         private static ProductImage image6 = new ProductImage(product6.Id, "https://images.unsplash.com/photo-1603618090554-f7a5079ffb54?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
//         private static ProductImage image7 = new ProductImage(product7.Id, "https://images.unsplash.com/photo-1610890716171-6b1bb98ffd09?q=80&w=1931&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
//         private static ProductImage image8 = new ProductImage(product8.Id, "https://images.unsplash.com/photo-1587408163744-8482f78bc883?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
//         private static ProductImage image9 = new ProductImage(product6.Id, "https://images.unsplash.com/photo-1603618090561-412154b4bd1b?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1yZWxhdGVkfDF8fHxlbnwwfHx8fHw%3D");
//         private static ProductImage image10 = new ProductImage(product1.Id, "https://images.unsplash.com/photo-1598327106026-d9521da673d1?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1yZWxhdGVkfDF8fHxlbnwwfHx8fHw%3D");


//         public static List<ProductImage> GetProductImagesSeed()
//         {
//             return new List<ProductImage>
//             {
//                 image1, image2, image3, image4, image5, image6, image7, image8, image9,image10
//             };
//         }


//         public static List<User> GetUsersSeed()
//         {
//             var users = new List<User>
//         {
//             new User("Admin", "admin@mail.com", "admin@123", "https://static.vecteezy.com/system/resources/thumbnails/006/487/917/small_2x/man-avatar-icon-free-vector.jpg", UserRole.Admin),
//             new User("John", "john@mail.com", "john@123", "https://static.vecteezy.com/system/resources/thumbnails/006/487/917/small_2x/man-avatar-icon-free-vector.jpg", UserRole.User)
//         };

//             foreach (var user in users)
//             {
//                 user.Password = _passwordHasher.HashPassword(user, user.Password);
//             }

//             return users;
//         }
//     }
// }