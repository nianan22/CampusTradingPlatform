using CTP.IRepository;
using Entity;
using EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.Repository
{
    public class ChatRoomRepository : BaseRepository<ChatRoom>, IChatRoomRepository
    {
        private readonly MyDbContext _context;
        public ChatRoomRepository(MyDbContext context) : base(context) { 
            this._context = context;
        }

        public async Task<ChatRoom> GetOrCreateChatRoomAsync(int buyerId, int sellerId, int productId)
        {
            var existingRoom = await _context.ChatRooms
                .FirstOrDefaultAsync(cr => cr.BuyerId == buyerId && cr.SellerId == sellerId && cr.ProductId == productId);
            return existingRoom ?? await CreateNewChatRoom(buyerId, sellerId, productId);
        }

        private async Task<ChatRoom> CreateNewChatRoom(int buyerId, int sellerId, int productId)
        {
            var newRoom = new ChatRoom { BuyerId = buyerId, SellerId = sellerId, ProductId = productId };
            await _context.ChatRooms.AddAsync(newRoom);
            await _context.SaveChangesAsync();
            return newRoom;
        }

        public async Task<List<ChatRoom>> GetUserChatRoomsAsync(int userId)
        {
            return await _context.ChatRooms
                .Where(cr => cr.BuyerId == userId || cr.SellerId == userId)
                .Include(cr => cr.Messages)
                .OrderByDescending(cr => cr.Messages.Max(m => m.SentAt))
                .ToListAsync();
        }
    }
}
