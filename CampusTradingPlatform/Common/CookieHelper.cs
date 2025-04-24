using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CookieHelper
    {

        /// <summary>
        /// 获取Cookie信息
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string Get(HttpContext context, string key)
        {
            return context.Request.Cookies[key]!;
        }
        /// <summary>
        /// 设置cookie的信息
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireTime">过期时间</param>
        public static void Set(HttpContext context, string key, string value, int? expireTime)
        {
            CookieOptions options = new CookieOptions();
            if (expireTime.HasValue)
            {
                options.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            }

            context.Response.Cookies.Append(key, value, options);
        }
        /// <summary>
        /// 删除cookie数据
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(HttpContext context, string key)
        {
            context.Response.Cookies.Delete(key);
        }
    }
}
