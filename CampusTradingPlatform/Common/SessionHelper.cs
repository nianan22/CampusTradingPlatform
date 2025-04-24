using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SessionHelper
    {
        public static void Set<T>(HttpContext context, string key, T obj)
        {
            context.Session.SetString(key, JsonConvert.SerializeObject(obj));
        }
        public static T GET<T>(HttpContext context, string key)
        {
            return JsonConvert.DeserializeObject<T>(context.Session.GetString(key));
        }
    }
}
