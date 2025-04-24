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
    public class EmailRepository : BaseRepository<EmailCaptchaInfo>, IEmailRepository
    {
        public EmailRepository(MyDbContext myDbContext) : base(myDbContext)
        {
        }
    }
}
