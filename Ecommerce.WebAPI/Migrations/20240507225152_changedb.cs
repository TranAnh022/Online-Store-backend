using System;
using Ecommerce.Core.src.ValueObjects;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ecommerce.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class changedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("37bbcefb-90f4-435d-aa3f-30ebea501f2d"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("786824af-d340-49a0-95a8-ef11107e6e2f"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("9b436a43-6f13-4835-8bf8-e74a091bb1f9"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("a8339233-ec1e-4d9a-a3bc-8a0841b72b11"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("2e991189-c021-476f-a76e-e54f4dca7554"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("4b998fe2-be3a-4e23-bfcd-ad5a5c942665"));

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "image", "name" },
                values: new object[,]
                {
                    { new Guid("30a9b806-efc9-427c-a049-c3cd5f9a0b81"), "https://images.unsplash.com/photo-1500995617113-cf789362a3e1?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8N3x8dG95c3xlbnwwfHwwfHx8MA%3D%3D", "Toys" },
                    { new Guid("9d5745e9-3102-4ec3-b1d8-8fd87b7484c1"), "https://images.unsplash.com/photo-1526738549149-8e07eca6c147?q=80&w=1925&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Electronics" },
                    { new Guid("b5dcb138-9cd9-43a5-8ce8-fba2f5b4e6ae"), "https://plus.unsplash.com/premium_photo-1682435561654-20d84cef00eb?q=80&w=1918&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Shoes" },
                    { new Guid("caa2b08c-3394-43ed-bb98-bb90a6a2180f"), "https://images.unsplash.com/photo-1556912173-3bb406ef7e77?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Home Goods" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar", "email", "name", "password", "role" },
                values: new object[,]
                {
                    { new Guid("4ad9dc45-a4ed-4443-8fcf-c4b34ced8be5"), "https://static.vecteezy.com/system/resources/thumbnails/006/487/917/small_2x/man-avatar-icon-free-vector.jpg", "admin@mail.com", "Admin", "admin@123", UserRole.Admin },
                    { new Guid("50136968-7b26-4fdd-8e3b-2ab557637ec9"), "https://static.vecteezy.com/system/resources/thumbnails/006/487/917/small_2x/man-avatar-icon-free-vector.jpg", "john@mail.com", "John", "john@123", UserRole.User }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("30a9b806-efc9-427c-a049-c3cd5f9a0b81"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("9d5745e9-3102-4ec3-b1d8-8fd87b7484c1"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("b5dcb138-9cd9-43a5-8ce8-fba2f5b4e6ae"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("caa2b08c-3394-43ed-bb98-bb90a6a2180f"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("4ad9dc45-a4ed-4443-8fcf-c4b34ced8be5"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("50136968-7b26-4fdd-8e3b-2ab557637ec9"));

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "image", "name" },
                values: new object[,]
                {
                    { new Guid("37bbcefb-90f4-435d-aa3f-30ebea501f2d"), "https://images.unsplash.com/photo-1526738549149-8e07eca6c147?q=80&w=1925&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Electronics" },
                    { new Guid("786824af-d340-49a0-95a8-ef11107e6e2f"), "https://images.unsplash.com/photo-1500995617113-cf789362a3e1?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8N3x8dG95c3xlbnwwfHwwfHx8MA%3D%3D", "Toys" },
                    { new Guid("9b436a43-6f13-4835-8bf8-e74a091bb1f9"), "https://images.unsplash.com/photo-1556912173-3bb406ef7e77?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Home Goods" },
                    { new Guid("a8339233-ec1e-4d9a-a3bc-8a0841b72b11"), "https://plus.unsplash.com/premium_photo-1682435561654-20d84cef00eb?q=80&w=1918&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Shoes" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar", "email", "name", "password", "role" },
                values: new object[,]
                {
                    { new Guid("2e991189-c021-476f-a76e-e54f4dca7554"), "https://static.vecteezy.com/system/resources/thumbnails/006/487/917/small_2x/man-avatar-icon-free-vector.jpg", "admin@mail.com", "Admin", "admin@123", UserRole.Admin },
                    { new Guid("4b998fe2-be3a-4e23-bfcd-ad5a5c942665"), "https://static.vecteezy.com/system/resources/thumbnails/006/487/917/small_2x/man-avatar-icon-free-vector.jpg", "john@mail.com", "John", "john@123", UserRole.User }
                });
        }
    }
}
