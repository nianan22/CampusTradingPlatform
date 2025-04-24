using CampusTradingPlatform.Attributes;
using CampusTradingPlatform.Models;
using Common;
using CTP.IService;
using EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusTradingPlatform.Controllers
{

    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IUserInfoService _userService;
        private readonly IProductService _productService;

        public ChatController(IChatService chatService, IUserInfoService userService, IProductService productService)
        {
            _chatService = chatService;
            _userService = userService;
            _productService = productService;
        }

        // 立即购买跳转聊天室
        public async Task<IActionResult> Start(int productId)
        {
            var buyer = SessionHelper.GET<LoginInfo>(HttpContext, "user");
            if (buyer == null) return RedirectToAction("Login", "Account");

            var product = await _productService.LoadEntities(p => p.Id == productId).FirstOrDefaultAsync();
            if (product == null) return NotFound();

            var chatRoom = await _chatService.StartChatAsync(buyer.UserId, product.UserId, productId);
            return RedirectToAction("ChatRoom", new { roomId = chatRoom.Id });
        }

        // 聊天室视图
        public async Task<IActionResult> ChatRoom(int roomId)
        {
            var user = SessionHelper.GET<LoginInfo>(HttpContext, "user");
            if (user == null) return RedirectToAction("Login", "Account");

            var chatRoom = await _chatService.StartChatAsync(0, 0, 0); // 实际需从数据库获取
            var messages = chatRoom.Messages;
            return View(messages);
        }

        // 发送消息
        [HttpPost]
        [UnitOfWork(new Type[] { typeof(MyDbContext) })]
        public async Task<IActionResult> SendMessage(int roomId, string message)
        {
            var user = SessionHelper.GET<LoginInfo>(HttpContext, "user");
            if (user == null) return Unauthorized();

            await _chatService.SendMessageAsync(roomId, user.UserId, message);
            return Ok();
        }

        // 消息列表
        public async Task<IActionResult> MessageList()
        {
            var user = SessionHelper.GET<LoginInfo>(HttpContext, "user");
            if (user == null) return RedirectToAction("Login", "Account");

            var chatRooms = await _chatService.GetUserChatRoomsAsync(user.UserId);
            return View(chatRooms);
        }
    }

}
