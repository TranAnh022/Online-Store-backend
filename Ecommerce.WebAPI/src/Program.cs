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
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.WebAPI.src.Authorization;
using Swashbuckle.AspNetCore.Filters;


var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

// add automapper dependency injection
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(
  options =>
  {
      options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
      {
          Description = "Bearer token authentication",
          Name = "Authorization",
          In = ParameterLocation.Header,
          Scheme = "Bearer"
      }
      );
      // swagger would add the token to the request header of routes with [Authorize] attribute
      options.OperationFilter<SecurityRequirementsOperationFilter>();
  }
);




// adding db context into your app
var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
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
                    builder.WithOrigins("http://localhost:3000", "http://localhost:3001", "https://online-store-demo.netlify.app") // Add specific origins here
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
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

// Resource based auth handlers
builder.Services.AddSingleton<IAuthorizationHandler, AdminOrOwnerOrderHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AdminOrOwnerReviewHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AdminOrOwnerAccountHandler>();

// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

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

