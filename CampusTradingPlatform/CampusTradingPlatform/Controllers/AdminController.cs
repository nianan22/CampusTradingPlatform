using CampusTradingPlatform.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CampusTradingPlatform.Controllers
{
    public class AdminController : Controller
    {
        [NoCheckSession]
        public IActionResult Index()
        {
            return View();
        }
    }
}
