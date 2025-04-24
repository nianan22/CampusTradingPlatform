using Microsoft.AspNetCore.Mvc;
using Common;
using CTP.IService;
using Entity;
using System.Threading.Tasks;
using CampusTradingPlatform.Models;
using CTP.Repository;
using CampusTradingPlatform.Attributes;
using EntityFrameworkCore;
using CTP.Service;
using Microsoft.EntityFrameworkCore;

namespace CampusTradingPlatform.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        public CartController(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var user = SessionHelper.GET<LoginInfo>(HttpContext, "user");

            // 确保购物车存在
            var cart = await _cartService.GetOrCreateCart(user.UserId);

            return View(cart ?? new Cart { Items = new List<CartItem>() });
        }

        [HttpPost]
        [UnitOfWork(new Type[] { typeof(MyDbContext) })]
        public async Task<IActionResult> Add(int productId)
        {
            var user = SessionHelper.GET<LoginInfo>(HttpContext, "user");

            try
            {
                await _cartService.AddToCart(user.UserId, productId);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        [UnitOfWork(new Type[] { typeof(MyDbContext) })]
        public async Task<IActionResult> Remove(int itemId) // 保持参数名一致
        {
            var user = SessionHelper.GET<LoginInfo>(HttpContext, "user");
            await _cartService.RemoveItem(user.UserId, itemId);
            return RedirectToAction("Index");
        }

        public async Task<JsonResult> Count()
        {
            var user = SessionHelper.GET<LoginInfo>(HttpContext, "user");
            var count = await _cartService.GetCartItemCount(user.UserId);
            return Json(new { count });
        }
        [HttpPost]
        public async Task<JsonResult> Update(int id, int quantity)
        {
            try
            {
                await _cartService.UpdateQuantity(id, quantity);
                var user = SessionHelper.GET<LoginInfo>(HttpContext, "user");
                var cart = await _cartService.GetUserCart(user.UserId);

                return Json(new
                {
                    success = true,
                    total = cart.GetTotalPrice(),
                    newQuantity = quantity // 返回最新数量
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}

