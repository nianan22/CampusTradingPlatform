using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.IRepository
{
    public interface IChatRoomRepository : IBaseRepository<ChatRoom>
    {
        Task<ChatRoom> GetOrCreateChatRoomAsync(int buyerId, int sellerId, int productId);
        Task<List<ChatRoom>> GetUserChatRoomsAsync(int userId);
    }
}
