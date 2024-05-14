using System;
using Ecommerce.Core.src.ValueObjects;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ecommerce.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("0f477590-a9b1-42c1-907a-0fed525ef403"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("abbc429f-0561-41bd-8310-0f9bb9d579dd"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("be9a45e0-c022-4f0d-9766-39c27a342f9e"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("eaf433aa-0b4e-48df-b851-b2ac3bb271df"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("29a47cf3-38be-4451-978c-eaf4a203ef69"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("a809cd16-e6c8-4283-b5b1-591cb937577e"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { new Guid("0f477590-a9b1-42c1-907a-0fed525ef403"), "https://plus.unsplash.com/premium_photo-1682435561654-20d84cef00eb?q=80&w=1918&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Shoes" },
                    { new Guid("abbc429f-0561-41bd-8310-0f9bb9d579dd"), "https://images.unsplash.com/photo-1526738549149-8e07eca6c147?q=80&w=1925&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Electronics" },
                    { new Guid("be9a45e0-c022-4f0d-9766-39c27a342f9e"), "https://images.unsplash.com/photo-1500995617113-cf789362a3e1?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8N3x8dG95c3xlbnwwfHwwfHx8MA%3D%3D", "Toys" },
                    { new Guid("eaf433aa-0b4e-48df-b851-b2ac3bb271df"), "https://images.unsplash.com/photo-1556912173-3bb406ef7e77?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Home Goods" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar", "email", "name", "password", "role" },
                values: new object[,]
                {
                    { new Guid("29a47cf3-38be-4451-978c-eaf4a203ef69"), "https://static.vecteezy.com/system/resources/thumbnails/006/487/917/small_2x/man-avatar-icon-free-vector.jpg", "admin@mail.com", "Admin", "admin@123", UserRole.Admin },
                    { new Guid("a809cd16-e6c8-4283-b5b1-591cb937577e"), "https://static.vecteezy.com/system/resources/thumbnails/006/487/917/small_2x/man-avatar-icon-free-vector.jpg", "john@mail.com", "John", "john@123", UserRole.User }
                });
        }
    }
}
