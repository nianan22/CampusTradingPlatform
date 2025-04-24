using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class UserInfo : BaseEntity<int>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string? UserPassword { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string? UserEmail { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string? UserPhone { get; set; }

        /// <summary>
        /// 性别：0表示男，1表示女
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 用户头像，存储头像路径
        /// </summary>
        public string? PhotoUrl { get; set; }
        public List<ProductInfo> Products { get; set; } // 添加: 用户拥有的商品集合
        public virtual Cart Cart { get; set; }
    }
}