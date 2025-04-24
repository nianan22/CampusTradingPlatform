using CampusTradingPlatform.Models;
using CTP.IRepository;
using CTP.IService;
using Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using CampusTradingPlatform.Attributes;
using EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace CampusTradingPlatform.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserInfoService userInfoService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IEmailRepository emailRepository;
        private readonly MyDbContext myDbContext;
        private readonly ICartRepository cartRepository;

        public LoginController(ICartRepository cartRepository,IUserInfoService userInfoService, IWebHostEnvironment webHostEnvironment, IEmailRepository emailRepository, MyDbContext myDbContext)
        {
            this.userInfoService = userInfoService;
            this.webHostEnvironment = webHostEnvironment;
            this.emailRepository = emailRepository;
            this.myDbContext = myDbContext;
            this.cartRepository = cartRepository;
        }

        [NoCheckSession]
        public IActionResult Index()
        {
            try
            {
                LoginInfo lg = SessionHelper.GET<LoginInfo>(HttpContext, "user");
                if (lg != null)
                {
                    
                    return Redirect("/login/alreadylogin");
                }
            }
            catch
            {

            }
            return View();
        }
        [NoCheckSession]
        public IActionResult Regeist()
        {
            return View();
        }
        [NoCheckSession]
        public async Task<IActionResult> EmailPost([FromQuery] string email)
        {
            var b = await userInfoService.RegeistEmail(email);
            if (b == null)
            {
                return Json(new { code = 404, data = "已存在用户" });
            }
            else
            {
                return Json(new { code = 200, data = b });
            }
        }
        [NoCheckSession]
        public IActionResult AlreadyLogin()
        {
            ViewBag.Message = "已经登录";
            return View();
        }
        [NoCheckSession]
        public async Task<IActionResult> RegeistUser([FromForm] RegeistInfo regeistInfo)
        {
            if (regeistInfo == null)
            {
                return Json(new { code = 400, message = "请求数据为空" });
            }

            // 获取用户输入的验证码和邮箱
            string userInputCaptcha = regeistInfo.vercode;
            string userEmail = regeistInfo.email;

            // 从数据库中获取最近一次的验证码信息
            var emailCaptchaInfo = await emailRepository.LoadEntities(a => a.Email == userEmail).FirstOrDefaultAsync();

            if (emailCaptchaInfo == null)
            {
                return Json(new { code = 400, message = "未找到验证码信息" });
            }

            // 计算当前时间和验证码创建时间的时间差
            TimeSpan timeDifference = DateTime.Now - emailCaptchaInfo.CreateDate;

            // 检查时间差是否超过5分钟
            if (timeDifference.TotalMinutes > 5)
            {
                return Json(new { code = 400, message = "验证码已过期，请重试" });
            }

            // 验证验证码是否正确
            if (emailCaptchaInfo.Captcha != userInputCaptcha)
            {
                return Json(new { code = 400, message = "验证码错误" });
            }

            // 处理 agreement 字段
            regeistInfo.agreement = Request.Form["agreement"].ToString() == "on";

            // 创建新的用户信息
            UserInfo newUser = new UserInfo
            {
                UserName = regeistInfo.nickname,
                UserPassword = regeistInfo.password,
                UserEmail = regeistInfo.email,
                UserPhone = regeistInfo.cellphone,
                Gender = regeistInfo.gender,
                PhotoUrl = "", // 默认头像路径，可以根据实际情况设置
                CreateDate = DateTime.Now,
                EditDate = DateTime.Now,
                IsDeleted = false
            };
            newUser.Cart = new Cart
            {
                UserId = newUser.Id,
                CreateDate = DateTime.Now,
                EditDate = DateTime.Now
            };

            // 将新用户添加到数据库
            
            await userInfoService.AddEntity(newUser);
            await myDbContext.SaveChangesAsync();

            return Json(new { code = 200, message = "注册成功" });
        }

        // 添加登录方法
        [NoCheckSession]
        public async Task<IActionResult> LoginUser([FromForm] LoginInfo loginInfo)
        {
            var userCode = Request.Form["captcha"]; // 接收验证码
            if (HttpContext.Session.GetString("imgCode") == "")
            {
                return Json(new { code = 400, message = "验证码为空" });
            }
            // 在验证码的比较中需要忽略大小写
            if (!string.Equals(HttpContext.Session.GetString("imgCode"), userCode, StringComparison.CurrentCultureIgnoreCase))
            {
                return Json(new { code = 400, message = "验证码错误" });
            }
            if (loginInfo == null)
            {
                return Json(new { code = 400, message = "请求数据为空" });
            }

            // 从数据库中获取用户信息
            var user = await myDbContext.Users.FirstOrDefaultAsync(u => u.UserName == loginInfo.username && u.UserPassword == loginInfo.password);

            if (user == null)
            {
                return Json(new { code = 400, message = "用户名或密码错误" });
            }
            var remember = Request.Form["remember"];
            if(remember=="on")
            {
                CookieHelper.Set(HttpContext, "username", loginInfo.username, 7);
                CookieHelper.Set(HttpContext, "password", loginInfo.password, 7);
                loginInfo.UserId = user.Id;
                CookieHelper.Set(HttpContext, "userid",Convert.ToString( loginInfo.UserId), 7);
            }
            SessionHelper.Set<LoginInfo>(HttpContext, "user",loginInfo);
            // 登录成功
            return Json(new { code = 200, message = "登录成功" });
        }
        [NoCheckSession]
        public IActionResult LoginOut()
        {
            CookieHelper.Remove(HttpContext, "username");
            CookieHelper.Remove(HttpContext, "password");
            HttpContext.Session.Remove("user");
            return RedirectToAction("index", "Login");
        }
        [NoCheckSession]
        public IActionResult CreateImageCode()
        {

            string code = CreateRandomCode(5);
            HttpContext.Session.SetString("imgCode", code);

            return File(CreateValidateGraphic(code), "image/Jpeg");
        }
        [NoCheckSession]
        private string CreateRandomCode(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(35);
                if (temp == t)
                {
                    return CreateRandomCode(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }
        /// <summary>
        /// 创建验证码的图片
        /// </summary>
         [NoCheckSession]
        public byte[] CreateValidateGraphic(string validateCode)
        {
            using (Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 16.0), 27))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    try
                    {
                        //生成随机生成器
                        Random random = new Random();
                        //清空图片背景色
                        g.Clear(Color.White);
                        //画图片的干扰线
                        for (int i = 0; i < 25; i++)
                        {
                            int x1 = random.Next(image.Width);
                            int x2 = random.Next(image.Width);
                            int y1 = random.Next(image.Height);
                            int y2 = random.Next(image.Height);
                            g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                        }
                        Font font = new Font("Arial", 13, (FontStyle.Bold | FontStyle.Italic));
                        // 线性渐变画刷
                        LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                         Color.Blue, Color.DarkRed, 1.2f, true);
                        g.DrawString(validateCode, font, brush, 3, 2);
                        //画图片的前景干扰点
                        for (int i = 0; i < 100; i++)
                        {
                            int x = random.Next(image.Width);
                            int y = random.Next(image.Height);
                            image.SetPixel(x, y, Color.FromArgb(random.Next()));
                        }
                        //画图片的边框线
                        g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                        //保存图片数据
                        MemoryStream stream = new MemoryStream();
                        image.Save(stream, ImageFormat.Jpeg);
                        //输出图片流
                        return stream.ToArray();
                    }
                    finally
                    {
                        g.Dispose();
                        image.Dispose();
                    }
                }

            }

        }
    }
}