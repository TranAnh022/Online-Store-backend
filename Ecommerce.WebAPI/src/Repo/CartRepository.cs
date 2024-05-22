using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Entities.CartAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.src.DTO;
using Ecommerce.WebAPI.src.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Ecommerce.WebAPI.src.Repo
{
    public class CartRepository : BaseRepo<Cart, QueryOptions>, ICartRepository
    {
        private readonly DbSet<CartItem> _cartItems;

        public CartRepository(AppDbContext context) : base(context)
        {
            _cartItems = context.CartsItem;
        }

        public async Task<Cart> AddCartItemToCart(Guid productId, int quantity, Guid userId)
        {
            // Load the cart associated with the user, including its cart items and products
            var cart = await _databaseContext.Carts
                                             .Include(c => c.CartItems!)
                                                .ThenInclude(ci => ci.Product)
                                                    .ThenInclude(p => p.Images)
                                             .FirstOrDefaultAsync(c => c.UserId == userId);

            // If the cart doesn't exist, create a new one
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _databaseContext.Carts.AddAsync(cart);
            }

            // Find the product by its ID
            var product = await _databaseContext.Products.FindAsync(productId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            // Check if the item already exists in the cart
            var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (existingCartItem != null)
            {
                // Update the quantity of the existing cart item
                existingCartItem.Quantity += quantity;
            }
            else
            {
                // Add the new item to the cart
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    Product = product
                };
                cart.CartItems.Add(cartItem);
            }

            // Save changes to the database
            await _databaseContext.SaveChangesAsync();

            return await _data
                .Include(c => c.CartItems!)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Category)
                .Include(c => c.CartItems!)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId) ?? throw new InvalidOperationException("Cart not found");
        }

        public async Task<Cart> RemoveCartItem(Guid productId, int quantity, Guid userId)
        {
            var cart = await _data.Include(c => c.CartItems).ThenInclude(ci => ci.Product).ThenInclude(p => p.Images).FirstOrDefaultAsync(x => x.UserId == userId);
            if (cart == null) throw new Exception("Cart not found");

            var product = await _databaseContext.Products.FindAsync(productId);
            if (product == null) throw new Exception("Product not found");

            cart.RemoveItem(product, quantity);

            await _databaseContext.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> GetCartByUserIdAsync(Guid userId)
        {
            return await _data
                .Include(c => c.CartItems!)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Category)
                .Include(c => c.CartItems!)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId) ?? throw new InvalidOperationException("Cart not found");
        }

        public async Task<IEnumerable<Cart>> ListAsync(QueryOptions options)
        {
            var query = _data.Include(c => c.CartItems)
                           .ThenInclude(ci => ci.Product)
                           .AsQueryable();
            if (options.Page.HasValue && options.PageSize.HasValue)
            {
                query = query.Skip((options.Page.Value - 1) * options.PageSize.Value)
                             .Take(options.PageSize.Value);
            }
            return await query.ToListAsync();
        }


    }
}