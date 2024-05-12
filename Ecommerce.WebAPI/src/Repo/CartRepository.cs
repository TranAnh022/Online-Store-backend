using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.CartAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.WebAPI.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repo
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Cart> _carts;
        private readonly DbSet<CartItem> _cartItems;

        public CartRepository(AppDbContext context)
        {
            _context = context;
            _carts = context.Carts;
            _cartItems = context.CartsItem;
        }

        public async Task<Cart> AddAsync(Cart entity)
        {
            _carts.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Cart> CreateCartForUser(Guid userId)
        {
            // Check if the user already has a cart
            var existingCart = await _carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (existingCart != null)
            {
                // Return existing cart if it already exists
                return existingCart;
            }

            // Create a new cart if none exists
            var newCart = new Cart { UserId = userId };
            _carts.Add(newCart);
            await _context.SaveChangesAsync();
            return newCart;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var cart = await _carts.FindAsync(id);
            if (cart == null) return false;
            _carts.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCartItemAsync(Guid cartItemId)
        {
            var item = await _cartItems.FindAsync(cartItemId);
            if (item != null)
            {
                _cartItems.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Cart> GetByIdAsync(Guid id)
        {
            return await _carts
                .Include(c => c.CartItems!)
                .ThenInclude(ci => ci.Product)
                .SingleOrDefaultAsync(c => c.Id == id) ?? throw new InvalidOperationException("Cart not found");
        }

        public async Task<Cart> GetCartByUserIdAsync(Guid userId)
        {
            return await _carts
                 .Include(c => c.CartItems!)
                 .FirstOrDefaultAsync(c => c.UserId == userId) ?? throw new InvalidOperationException("Cart not found"); ;
        }

        public async Task<IEnumerable<Cart>> ListAsync(QueryOptions options)
        {
            var query = _carts.AsQueryable();

            query = query.Include(c => c.CartItems!);

            if (!string.IsNullOrWhiteSpace(options.Search))
            {
                query = query.Where(c => c.User!.Email.Contains(options.Search) || c.User.Name.Contains(options.Search));
            }

            if (!string.IsNullOrWhiteSpace(options.SortBy))
            {
                query = options.SortOrder == "asc" ? query.OrderBy(c => c.CreatedAt) : query.OrderByDescending(c => c.UpdatedAt);
            }

            if (options.Page.HasValue && options.PageSize.HasValue)
            {
                query = query.Skip((options.Page.Value - 1) * options.PageSize.Value).Take(options.PageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Cart> UpdateAsync(Cart entity)
        {
            _carts.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<CartItem> UpdateCartItemAsync(Guid cartId, CartItem cartItem)
        {
            var cart = await _carts
               .Include(c => c.CartItems)
               .FirstOrDefaultAsync(c => c.Id == cartId);
            // Check if the cart was found and if it contains the item to update
            var existingItem = _cartItems.FirstOrDefault(ci => ci.Id == cartItem.Id);
            if (existingItem != null)
            {
                // Update the quantity of the existing item
                existingItem.SetQuantity(cartItem.Quantity);
                _context.Update(existingItem);
                await _context.SaveChangesAsync();
                return existingItem;
            }
            // Return null if no cart or cart item was found to update
            return null!;
        }
        public async Task EnsureCartsForAllUsers()
        {
            var usersWithoutCarts = await _context.Users
                .Include(u => u.Cart)
                .Where(u => u.Cart == null)
                .ToListAsync();

            foreach (var user in usersWithoutCarts)
            {
                _context.Carts.Add(new Cart { UserId = user.Id });
            }

            await _context.SaveChangesAsync();
        }
    }
}