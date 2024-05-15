using System;
using Ecommerce.Core.src.ValueObjects;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ecommerce.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Add_Seed_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("105aaf3b-ff8b-42e5-8944-66347a9a5adf"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("1f469451-ca58-40a6-b3d4-41c9edd5ebd4"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("274200d3-84ca-4dff-bfb1-36cc4565e6df"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("765830f5-da2f-4453-a9d6-4820b566a505"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("4b56e3bb-0581-483c-a887-cbba3669e5fe"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("d35af8c1-72fe-4d5d-a0ce-bc5db32810cc"));

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "image", "name" },
                values: new object[,]
                {
                    { new Guid("15ab95f1-3589-4f21-9ec1-65018f83412d"), "https://images.unsplash.com/photo-1526738549149-8e07eca6c147?q=80&w=1925&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Electronics" },
                    { new Guid("3044bebd-3597-482c-8a86-f8f7c58c2d98"), "https://images.unsplash.com/photo-1500995617113-cf789362a3e1?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Toys" },
                    { new Guid("af6d1a1b-f69a-4d36-9f33-0e971ed9c34c"), "https://plus.unsplash.com/premium_photo-1682435561654-20d84cef00eb?q=80&w=1918&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Shoes" },
                    { new Guid("b22247c5-1b6d-4c99-95ce-7a35836949ab"), "https://images.unsplash.com/photo-1556912173-3bb406ef7e77?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Home Goods" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar", "email", "name", "password", "role" },
                values: new object[,]
                {
                    { new Guid("03dc9c48-1ed6-4f49-9c61-9cf67f2ac34b"), "https://static.vecteezy.com/system/resources/thumbnails/006/487/917/small_2x/man-avatar-icon-free-vector.jpg", "john@mail.com", "John", "AQAAAAIAAYagAAAAEHJcr9WQaOHRM8dXE8uNHSDmuq1RdCBYj9mMq3dDEQ+btftdHAX0oidjDoAMI8Qjyg==", UserRole.User },
                    { new Guid("8f479818-eec3-41ce-a605-f8d27c3315c7"), "https://static.vecteezy.com/system/resources/thumbnails/006/487/917/small_2x/man-avatar-icon-free-vector.jpg", "admin@mail.com", "Admin", "AQAAAAIAAYagAAAAEHCMYJvY4mdbnzK5hSGJYGnysq0LPz8hnn7OYmkCAXrsnGbD91kWc+JKykH/nhSR+Q==", UserRole.Admin }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "category_id", "description", "inventory", "price", "title" },
                values: new object[,]
                {
                    { new Guid("053945d1-c000-427f-b70c-26efbbea8e92"), new Guid("3044bebd-3597-482c-8a86-f8f7c58c2d98"), "Gather your friends and family for hours of fun with the Board Game Bonanza. Featuring a variety of classic and modern games, this collection is sure to entertain players of all ages.", 105, 39.99m, "Board Game Bonanza" },
                    { new Guid("172039b2-b025-4a5e-af38-252a96e49d26"), new Guid("3044bebd-3597-482c-8a86-f8f7c58c2d98"), "Create unforgettable memories with the Outdoor Playset Deluxe. This premium playset includes a slide, swings, and a climbing wall, providing endless entertainment for kids in the backyard.", 85, 499.99m, "Outdoor Playset Deluxe" },
                    { new Guid("1cb9c534-5083-4961-a64e-6642cb93508e"), new Guid("b22247c5-1b6d-4c99-95ce-7a35836949ab"), "Say goodbye to manual vacuuming with the Robot Vacuum Pro. This intelligent robot cleaner navigates your home effortlessly, removing dirt, dust, and pet hair with precision, leaving your floors spotless.", 95, 299.99m, "Robot Vacuum Pro" },
                    { new Guid("1e7d5cdd-d145-4ebf-98d9-bf88d1983c81"), new Guid("15ab95f1-3589-4f21-9ec1-65018f83412d"), "The Laptop ProBook 2022 offers superior performance and reliability, featuring a sleek design, long-lasting battery, and advanced security features for your peace of mind.", 120, 899.99m, "Laptop ProBook 2022" },
                    { new Guid("4b0d0afc-312a-485d-96bc-2ffb5ffc413b"), new Guid("b22247c5-1b6d-4c99-95ce-7a35836949ab"), "Transform your home into a smart sanctuary with the Smart Home Hub Plus. Control your lights, appliances, and security cameras with ease, and enjoy the convenience of voice commands and automated routines.", 110, 199.99m, "Smart Home Hub Plus" },
                    { new Guid("51bf416a-e30b-4e11-acd3-eb99e33f5c9d"), new Guid("15ab95f1-3589-4f21-9ec1-65018f83412d"), "The Smartphone X10 is packed with cutting-edge features, including a high-resolution camera and a powerful processor, making it perfect for staying connected and productive on the go.", 100, 499.99m, "Smartphone X10" },
                    { new Guid("d23aa941-3376-473b-83f7-9f278dd9cbe6"), new Guid("af6d1a1b-f69a-4d36-9f33-0e971ed9c34c"), "Conquer any trail with confidence in the Hiking Boots Adventure. These rugged boots offer superior traction, waterproof protection, and unmatched durability for your outdoor adventures.", 90, 149.99m, "Hiking Boots Adventure" },
                    { new Guid("e4bb0c50-a10f-497e-b04f-566024ce8e3b"), new Guid("af6d1a1b-f69a-4d36-9f33-0e971ed9c34c"), "Designed for serious runners, the Running Shoes Elite provide exceptional comfort, support, and durability, allowing you to achieve your personal best with every stride.", 80, 129.99m, "Running Shoes Elite" }
                });

            migrationBuilder.InsertData(
                table: "product_images",
                columns: new[] { "id", "product_id", "url" },
                values: new object[,]
                {
                    { new Guid("4b420343-6bed-4848-a0b1-c24e1efa009e"), new Guid("172039b2-b025-4a5e-af38-252a96e49d26"), "https://images.unsplash.com/photo-1587408163744-8482f78bc883?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
                    { new Guid("789dfc1c-d8e8-4453-b735-951973e542d4"), new Guid("51bf416a-e30b-4e11-acd3-eb99e33f5c9d"), "https://images.unsplash.com/photo-1598327105666-5b89351aff97?q=80&w=2118&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
                    { new Guid("8600e3eb-cd68-4378-861f-15cf7611453f"), new Guid("1e7d5cdd-d145-4ebf-98d9-bf88d1983c81"), "https://images.unsplash.com/photo-1559163499-413811fb2344?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
                    { new Guid("9f23015b-3b69-4794-a5ee-12ebcc9cae48"), new Guid("d23aa941-3376-473b-83f7-9f278dd9cbe6"), "https://images.unsplash.com/photo-1575987116913-e96e7d490b8a?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
                    { new Guid("b5bac06c-14cf-446e-899f-6a4cd22b4d4d"), new Guid("1cb9c534-5083-4961-a64e-6642cb93508e"), "https://images.unsplash.com/photo-1603618090561-412154b4bd1b?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1yZWxhdGVkfDF8fHxlbnwwfHx8fHw%3D" },
                    { new Guid("d01eb6aa-0f4f-4175-847a-0cd76e8b1af0"), new Guid("51bf416a-e30b-4e11-acd3-eb99e33f5c9d"), "https://images.unsplash.com/photo-1598327106026-d9521da673d1?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1yZWxhdGVkfDF8fHxlbnwwfHx8fHw%3D" },
                    { new Guid("d758aa87-05fd-422a-beb1-71eb403fdfa4"), new Guid("4b0d0afc-312a-485d-96bc-2ffb5ffc413b"), "https://images.unsplash.com/photo-1558089687-f282ffcbc126?q=80&w=2071&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
                    { new Guid("de3633ae-45be-4f34-8e3a-a525cc1ee238"), new Guid("1cb9c534-5083-4961-a64e-6642cb93508e"), "https://images.unsplash.com/photo-1603618090554-f7a5079ffb54?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
                    { new Guid("e9c89a2a-f4c2-44a8-9860-963c58535a9a"), new Guid("e4bb0c50-a10f-497e-b04f-566024ce8e3b"), "https://images.unsplash.com/photo-1562183241-b937e95585b6?q=80&w=1965&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
                    { new Guid("f0e2f8ce-68de-4704-84d6-e1312e54ef4c"), new Guid("053945d1-c000-427f-b70c-26efbbea8e92"), "https://images.unsplash.com/photo-1610890716171-6b1bb98ffd09?q=80&w=1931&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "product_images",
                keyColumn: "id",
                keyValue: new Guid("4b420343-6bed-4848-a0b1-c24e1efa009e"));

            migrationBuilder.DeleteData(
                table: "product_images",
                keyColumn: "id",
                keyValue: new Guid("789dfc1c-d8e8-4453-b735-951973e542d4"));

            migrationBuilder.DeleteData(
                table: "product_images",
                keyColumn: "id",
                keyValue: new Guid("8600e3eb-cd68-4378-861f-15cf7611453f"));

            migrationBuilder.DeleteData(
                table: "product_images",
                keyColumn: "id",
                keyValue: new Guid("9f23015b-3b69-4794-a5ee-12ebcc9cae48"));

            migrationBuilder.DeleteData(
                table: "product_images",
                keyColumn: "id",
                keyValue: new Guid("b5bac06c-14cf-446e-899f-6a4cd22b4d4d"));

            migrationBuilder.DeleteData(
                table: "product_images",
                keyColumn: "id",
                keyValue: new Guid("d01eb6aa-0f4f-4175-847a-0cd76e8b1af0"));

            migrationBuilder.DeleteData(
                table: "product_images",
                keyColumn: "id",
                keyValue: new Guid("d758aa87-05fd-422a-beb1-71eb403fdfa4"));

            migrationBuilder.DeleteData(
                table: "product_images",
                keyColumn: "id",
                keyValue: new Guid("de3633ae-45be-4f34-8e3a-a525cc1ee238"));

            migrationBuilder.DeleteData(
                table: "product_images",
                keyColumn: "id",
                keyValue: new Guid("e9c89a2a-f4c2-44a8-9860-963c58535a9a"));

            migrationBuilder.DeleteData(
                table: "product_images",
                keyColumn: "id",
                keyValue: new Guid("f0e2f8ce-68de-4704-84d6-e1312e54ef4c"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("03dc9c48-1ed6-4f49-9c61-9cf67f2ac34b"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("8f479818-eec3-41ce-a605-f8d27c3315c7"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("053945d1-c000-427f-b70c-26efbbea8e92"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("172039b2-b025-4a5e-af38-252a96e49d26"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("1cb9c534-5083-4961-a64e-6642cb93508e"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("1e7d5cdd-d145-4ebf-98d9-bf88d1983c81"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("4b0d0afc-312a-485d-96bc-2ffb5ffc413b"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("51bf416a-e30b-4e11-acd3-eb99e33f5c9d"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("d23aa941-3376-473b-83f7-9f278dd9cbe6"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("e4bb0c50-a10f-497e-b04f-566024ce8e3b"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("15ab95f1-3589-4f21-9ec1-65018f83412d"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("3044bebd-3597-482c-8a86-f8f7c58c2d98"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("af6d1a1b-f69a-4d36-9f33-0e971ed9c34c"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("b22247c5-1b6d-4c99-95ce-7a35836949ab"));

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "image", "name" },
                values: new object[,]
                {
                    { new Guid("105aaf3b-ff8b-42e5-8944-66347a9a5adf"), "https://images.unsplash.com/photo-1526738549149-8e07eca6c147?q=80&w=1925&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Electronics" },
                    { new Guid("1f469451-ca58-40a6-b3d4-41c9edd5ebd4"), "https://images.unsplash.com/photo-1556912173-3bb406ef7e77?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Home Goods" },
                    { new Guid("274200d3-84ca-4dff-bfb1-36cc4565e6df"), "https://plus.unsplash.com/premium_photo-1682435561654-20d84cef00eb?q=80&w=1918&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Shoes" },
                    { new Guid("765830f5-da2f-4453-a9d6-4820b566a505"), "https://images.unsplash.com/photo-1500995617113-cf789362a3e1?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8N3x8dG95c3xlbnwwfHwwfHx8MA%3D%3D", "Toys" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar", "email", "name", "password", "role" },
                values: new object[,]
                {
                    { new Guid("4b56e3bb-0581-483c-a887-cbba3669e5fe"), "https://static.vecteezy.com/system/resources/thumbnails/006/487/917/small_2x/man-avatar-icon-free-vector.jpg", "john@mail.com", "John", "john@123", UserRole.User },
                    { new Guid("d35af8c1-72fe-4d5d-a0ce-bc5db32810cc"), "https://static.vecteezy.com/system/resources/thumbnails/006/487/917/small_2x/man-avatar-icon-free-vector.jpg", "admin@mail.com", "Admin", "admin@123", UserRole.Admin }
                });
        }
    }
}
