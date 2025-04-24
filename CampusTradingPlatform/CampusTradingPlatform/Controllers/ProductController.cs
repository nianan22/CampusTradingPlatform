using Microsoft.AspNetCore.Mvc;
using CTP.IService;
using Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CampusTradingPlatform.Models;
using Common;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using EntityFrameworkCore;

namespace CampusTradingPlatform.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IUserInfoService userInfoService;
        private readonly MyDbContext myDbContext;

        public ProductController(IProductService productService, IUserInfoService userInfoService, MyDbContext myDbContext)
        {
            this.productService = productService;
            this.userInfoService = userInfoService;
            this.myDbContext = myDbContext;
        }
        public async Task<IActionResult> DetailProduct([FromQuery]int productId)
        {
            var product = await productService.LoadEntities(p => p.Id == productId)
                .Include(p => p.User)  // 加载卖家信息
                .FirstOrDefaultAsync();

            if (product == null) return NotFound();
            return View("Detail", product);
        }
        public async Task<IActionResult> Index(string search = "", string category = "")
        {
            var query = productService.LoadEntities(p => p.Status == "上架");  // 获取所有商品

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            var products = await query.ToListAsync();
            ViewBag.Categories = new List<string> { "服装", "食品", "书籍", "电子产品", "交通工具", "其他" };
            return View(products);
        }

        public async Task<IActionResult> GetProducts()
        {
            LoginInfo lg = SessionHelper.GET<LoginInfo>(HttpContext, "user");
            var products = await productService.GetProductsByUserId(lg.UserId);
            return View(products);
        }


        public async Task<IActionResult> GetSoldProducts()
        {
            LoginInfo lg = SessionHelper.GET<LoginInfo>(HttpContext, "user");
            var soldProducts = await productService.GetSoldProductsByUserId(lg.UserId);
            return View(soldProducts);
        }

        public async Task<IActionResult> ShelveProduct(int productId)
        {
            LoginInfo lg = SessionHelper.GET<LoginInfo>(HttpContext, "user");
            var product = await productService.LoadEntities(p => p.Id == productId).FirstOrDefaultAsync();
            if (product == null || product.UserId != lg.UserId)
            {
                return RedirectToAction("Index"); // 或返回错误提示
            }
            if (product != null)
            {
                product.Status = "下架";
                await productService.UpdateEntity(product);
            }
            return RedirectToAction("GetProducts");
        }

        public async Task<IActionResult> UnshelveProduct(int productId)
        {
            LoginInfo lg = SessionHelper.GET<LoginInfo>(HttpContext, "user");
            var product = await productService.LoadEntities(p => p.Id == productId).FirstOrDefaultAsync();
            if (product == null || product.UserId != lg.UserId)
            {
                return RedirectToAction("Index"); // 或返回错误提示
            }
            if (product != null)
            {
                product.Status = "上架";
                await productService.UpdateEntity(product);
            }
            return RedirectToAction("GetProducts");
        }
    }
}