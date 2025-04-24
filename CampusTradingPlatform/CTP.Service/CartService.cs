using CTP.IRepository;
using CTP.IService;
using Entity;
using EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.Service
{
    public class CartService : BaseService<Cart>, ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IProductService _productService;
        private readonly MyDbContext _context;

        public CartService(ICartRepository cartRepo,IProductService productService, MyDbContext _context, IBaseRepository<Cart> repository) :base(repository)
        {
            _cartRepo = cartRepo;
            _productService = productService;
            this._context = _context;
        }

        public async Task AddToCart(int userId, int productId)
        {
            var user = await _context.Users.Include(u => u.Cart).ThenInclude(c => c.Items).FirstOrDefaultAsync(u => u.Id == userId);

            if (user.Cart == null)
            {
                user.Cart = new Cart { UserId = userId };
                await _context.AddAsync(user.Cart);
            }

            var item = user.Cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
            {
                user.Cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = 1,
                    CreateDate = DateTime.Now
                });
            }
            else
            {
                item.Quantity++;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetUserCart(int userId)
        {
            return await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product).FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task RemoveItem(int userId, int itemId)
        {
            // 添加权限验证
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            var targetItem = cart?.Items.FirstOrDefault(i => i.Id == itemId);
            if (targetItem == null) return;

            _context.Remove(targetItem);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateQuantity(int itemId, int quantity)
        {
            if (quantity < 1) throw new ArgumentException("数量不能小于1");

            var item = await _context.CartItems
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.Id == itemId);

            if (item == null) throw new KeyNotFoundException("购物车项不存在");

            // 添加库存检查
            if (quantity > item.Product.Count)
                throw new InvalidOperationException($"库存不足，最多可购买{item.Product.Count}件");

            item.Quantity = quantity;
            item.EditDate = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCartItemCount(int userId)
        {
            var cart = await GetOrCreateCart(userId);
            return cart.Items.Sum(i => i.Quantity);
        }

        public async Task<Cart> GetOrCreateCart(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            return cart ?? new Cart
            {
                UserId = userId,
                Items = new List<CartItem>()
            };
        }

    }
}
