 using CampusTradingPlatform.Models;
using Common;
using Microsoft.AspNetCore.Mvc;
using CTP.IService;
using Entity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using CampusTradingPlatform.Attributes;
using EntityFrameworkCore;
using CTP.Service;

namespace CampusTradingPlatform.Controllers
{
    public class SellerController : Controller
    {
        private readonly IProductService productService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SellerController(IProductService productService, IWebHostEnvironment _webHostEnvironment)
        {
            this.productService = productService;
            this._webHostEnvironment = _webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            LoginInfo lg = SessionHelper.GET<LoginInfo>(HttpContext, "user");
            var products = await productService.GetProductsByUserId(lg.UserId);
            var soldProducts = await productService.GetSoldProductsByUserId(lg.UserId);
            ViewBag.Products = products;
            ViewBag.SoldProducts = soldProducts;
            return View(lg);
        }

        [HttpGet]
        public IActionResult AddProduct() 
        {
            return View();
        }
        [HttpPost]
        [UnitOfWork(new Type[] { typeof(MyDbContext) })]
        public async Task<IActionResult> AddProduct(ProductInfo product,IFormFile mainImage,IFormFile[] detailImages )
        {
            
            
                try
                {
                    // 处理主图上传
                    if (mainImage != null)
                    {
                        product.PhotoUrl = await SaveImage(mainImage);
                    }

                    // 处理多图上传
                    if (detailImages != null && detailImages.Length > 0)
                    {
                        var imageUrls = new List<string>();
                        foreach (var image in detailImages)
                        {
                            imageUrls.Add(await SaveImage(image));
                        }
                        product.ImagesUrl = imageUrls.ToArray();
                    }

                    // 设置用户ID和状态
                    product.UserId = SessionHelper.GET<LoginInfo>(HttpContext, "user").UserId;
                    product.Status = "上架";

                    // 添加商品到数据库并保存
                    await productService.AddEntity(product);
                    

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // 添加日志记录
                    ModelState.AddModelError("", "保存商品时发生错误：" + ex.Message);
                }
            
            return View("AddProduct", product);
        }
        [HttpGet]
        public async Task<IActionResult> EditProduct(int productId)
        {
            var product = await productService.LoadEntities(p => p.Id == productId).FirstOrDefaultAsync();
            return View(product);
        }

        [HttpPost]
        [UnitOfWork(new Type[] { typeof(MyDbContext) })]
        public async Task<IActionResult> EditProduct(ProductInfo product, IFormFile mainImage, IFormFile[] detailImages)
        {
           
                try
                {
                // 处理图片更新
                //if (mainImage != null)
                //{
                //    product.PhotoUrl = await SaveImage(mainImage);
                //}
                //if (detailImages != null && detailImages.Length > 0)
                //{
                //    var imageUrls = new List<string>();
                //    foreach (var image in detailImages)
                //    {
                //        imageUrls.Add(await SaveImage(image));
                //    }
                //    product.ImagesUrl = imageUrls.ToArray();
                //}

                // 更新修改时间
                ProductInfo productnew = new ProductInfo();
                productnew= (ProductInfo)productService.LoadEntities(a => a.Id == product.Id);
                productnew.Price = product.Price;
                productnew.EditDate = DateTime.Now;

                    await productService.UpdateEntity(productnew);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "更新失败：" + ex.Message);
                }
            
            return View(product);
        }
        private async Task<string> SaveImage(IFormFile image)
        {
            var uploadsPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

            // 如果目录不存在，则创建
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            return $"/uploads/{fileName}";
        }

        public async Task<IActionResult> RemoveProduct(int productId)
        {
            var product = await productService.LoadEntities(p => p.Id == productId).FirstOrDefaultAsync(); 
            if (product != null)
            {
                product.Status = "下架";
                await productService.UpdateEntity(product);
            }
            return RedirectToAction("GetProducts");
        }

        public async Task<IActionResult> GetSoldProducts()
        {
            LoginInfo lg = SessionHelper.GET<LoginInfo>(HttpContext, "user");
            var soldProducts = await productService.GetSoldProductsByUserId(lg.UserId);
            return View(soldProducts);
        }
        [UnitOfWork(new Type[] { typeof(MyDbContext) })]
        public async Task<IActionResult> ShelveProduct(int productId)
        {
            var product = await productService.LoadEntities(p => p.Id == productId).FirstOrDefaultAsync();
            if (product != null)
            {
                product.Status = "上架";
                await productService.UpdateEntity(product);
            }
            return RedirectToAction("Index");
        }
        [UnitOfWork(new Type[] { typeof(MyDbContext) })]
        public async Task<IActionResult> UnshelveProduct(int productId)
        {
            var product = await productService.LoadEntities(p => p.Id == productId).FirstOrDefaultAsync();
            if (product != null)
            {
                product.Status = "下架";
                await productService.UpdateEntity(product);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DetailProduct(int productId)
        {
            var product = await productService.LoadEntities(p => p.Id == productId).FirstOrDefaultAsync();
            return View(product);
        }
        // 新增方法：根据状态获取商品列表
        public async Task<IActionResult> GetProductsByStatus(string status)
        {
            LoginInfo lg = SessionHelper.GET<LoginInfo>(HttpContext, "user");
            IEnumerable<ProductInfo> products;
            if (status == "已售出")
            {
                products = await productService.GetSoldProductsByUserId(lg.UserId);
            }
            else
            {
                var allProducts = await productService.GetProductsByUserId(lg.UserId);
                products = allProducts.Where(p => p.Status == status); // 确保此处正确筛选
            }
            return PartialView("_ProductListPartial", products);
        }
    }
}