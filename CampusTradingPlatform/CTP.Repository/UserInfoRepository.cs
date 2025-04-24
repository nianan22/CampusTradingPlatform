using CTP.IRepository;
using Entity;
using EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.Repository
{
    public class UserInfoRepository: BaseRepository<UserInfo>, IUserInfoRepository
    {
        public UserInfoRepository(MyDbContext myDbContext) : base(myDbContext)
        {
        }
    }
}
