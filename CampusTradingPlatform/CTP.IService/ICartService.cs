using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.IService
{
    public interface ICartService: IBaseService<Cart>
    {
        Task AddToCart(int userId, int productId);
        Task<Cart> GetUserCart(int userId);
        Task RemoveItem(int userId, int itemId);
        Task<int> GetCartItemCount(int userId);
        Task UpdateQuantity(int itemId, int newQuantity);
        Task<Cart> GetOrCreateCart(int userId);
    }
}
