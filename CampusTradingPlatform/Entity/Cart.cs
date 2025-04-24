using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Cart : BaseEntity<int>
    {
        public int UserId { get; set; }
        public virtual UserInfo User { get; set; }  // 添加virtual关键字
        public virtual List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal GetTotalPrice()
        {
            return (decimal)Items.Sum(i => i.Product.Price * i.Quantity);
        }
    }
}
