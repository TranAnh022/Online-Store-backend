using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.ValueObjects;
using Ecommerce.WebAPI.src.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.WebAPI.src.Repo;
using Ecommerce.Service.src.ServiceAbstract;
using Ecommerce.Service.src.Service;
using Ecommerce.WebAPI.src.Service;
using Ecommerce.Service.src.Shared;
using Ecommerce.WebAPI.src.Middleware;
using Ecommerce.WebAPI.src.Repository;
using Microsoft.OpenApi.Models;
using Ecommerce.Core.src.Entities.CartAggregate;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.OrderAggregate;
using Walmad.WebAPI.src.Authorization;
using CloudinaryDotNet;


var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

// add automapper dependency injection
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put Bearer + your token in the box below",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});




// adding db context into your app
var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("Localhost"));
dataSourceBuilder.MapEnum<UserRole>();
dataSourceBuilder.MapEnum<OrderStatus>();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<AppDbContext>
(
    options =>
    options.UseNpgsql(dataSource)
    .UseSnakeCaseNamingConvention()
    .AddInterceptors(new TimeStampInteceptor())
);

// CORS
builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
        });

//Cloudinary

var cloudinarySettings = builder.Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
builder.Services.AddSingleton(cloudinarySettings);
builder.Services.AddSingleton(new Cloudinary(new Account(
    cloudinarySettings.CloudName,
    cloudinarySettings.ApiKey,
    cloudinarySettings.ApiSecret)));


// service registration -> automatically create all instances of dependencies
// User
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<IUserRepository, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();
// Auth
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
// Category
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
// Review
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();
// Product
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepo>();
// ProductImage
builder.Services.AddScoped<IProductImageRepository, ProductImageRepo>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
// Cart and CartItems
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<IBaseRepository<CartItem, QueryOptions>, CartItemRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
//Order and OrderItems

builder.Services.AddScoped<IOrderRepository, OrderRepo>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IBaseRepository<OrderItem, QueryOptions>, OrderItemRepository>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();

//Cloudinary
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
// Add authentication instructions
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Secrets:JwtKey"]!)),
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Secrets:Issuer"]
        };
    }
);


// config authorization
builder.Services.AddAuthorization(policy =>
{
    policy.AddPolicy("AdminOrOwnerOrder", policy => policy.Requirements.Add(new AdminOrOwnerOrderRequirement()));
    policy.AddPolicy("AdminOrOwnerReview", policy => policy.Requirements.Add(new AdminOrOwnerReviewRequirement()));
    policy.AddPolicy("AdminOrOwnerAccount", policy => policy.Requirements.Add(new AdminOrOwnerAccountRequirement()));
});

var app = builder.Build();


app.UseCors("AllowAllOrigins");
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseExceptionHandler("/Error");
app.UseDeveloperExceptionPage();
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

