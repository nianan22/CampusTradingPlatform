using System.Diagnostics;
using CampusTradingPlatform.Attributes;
using CampusTradingPlatform.Models;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace CampusTradingPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment _webHostEnvironment)
        {
            _logger = logger;
            this._webHostEnvironment = _webHostEnvironment;
        }
        [NoCheckSession]
        public IActionResult Index()
        {
            string username = CookieHelper.Get(HttpContext, "username");
            string password = CookieHelper.Get(HttpContext, "password");
            string userid = CookieHelper.Get(HttpContext, "userid");
            try
            {
                LoginInfo lg = SessionHelper.GET<LoginInfo>(HttpContext, "user");
                if (lg != null)
                {
                    return View(lg);
                }
            }
            catch
            {

            }
     
            LoginInfo loginInfo = new LoginInfo();
            if (username != null || password != null)
            {
                loginInfo.username = username;
                loginInfo.password = password;
                loginInfo.UserId = Convert.ToInt32(userid);
                SessionHelper.Set<LoginInfo>(HttpContext, "user", loginInfo);
            }
            return View(loginInfo);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
