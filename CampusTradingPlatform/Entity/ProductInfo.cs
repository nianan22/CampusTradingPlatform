using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ProductInfo : BaseEntity<int>
    {
        public string? Name { get; set; }
        public double Price { get; set; } // 添加价格字段
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? PhotoUrl { get; set; }
        public string[]? ImagesUrl { get; set; }
        public int? Count { get; set; }
        public int UserId { get; set; } // 用户ID关联
        public UserInfo User { get; set; } // 添加: 用户实体关联
        public string Status { get; set; } // 添加: 商品状态
    }
}