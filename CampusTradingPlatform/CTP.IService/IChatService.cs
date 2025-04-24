using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.IService
{
    public interface IChatService: IBaseService<ChatRoom>
    {
        Task<ChatRoom> StartChatAsync(int buyerId, int sellerId, int productId);
        Task SendMessageAsync(int roomId, int senderId, string content);
        Task<List<ChatRoom>> GetUserChatRoomsAsync(int userId);
    }
}
