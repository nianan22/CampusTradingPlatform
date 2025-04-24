using CTP.IRepository;
using CTP.IService;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.Service
{
    public class ChatService : BaseService<ChatRoom>, IChatService
    {
        private readonly IChatRoomRepository _chatRoomRepo;
        private readonly IBaseRepository<ChatMessage> _messageRepo;

        public ChatService(IChatRoomRepository chatRoomRepo, IBaseRepository<ChatMessage> messageRepo, IBaseRepository<ChatRoom> repository):base(repository)
        {
            _chatRoomRepo = chatRoomRepo;
            _messageRepo = messageRepo;
        }

        public async Task<ChatRoom> StartChatAsync(int buyerId, int sellerId, int productId)
        {
            return await _chatRoomRepo.GetOrCreateChatRoomAsync(buyerId, sellerId, productId);
        }

        public async Task SendMessageAsync(int roomId, int senderId, string content)
        {
            var message = new ChatMessage
            {
                ChatRoomId = roomId,
                SenderId = senderId,
                Content = content,
                SentAt = DateTime.Now
            };
            await _messageRepo.AddEntity(message);
        }

        public async Task<List<ChatRoom>> GetUserChatRoomsAsync(int userId)
        {
            return await _chatRoomRepo.GetUserChatRoomsAsync(userId);
        }
    }
}
