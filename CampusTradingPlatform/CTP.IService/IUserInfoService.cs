using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.IService
{
    public interface IUserInfoService : IBaseService<UserInfo>
    {
        Task<string> RegeistEmail(string email);
    }
}
