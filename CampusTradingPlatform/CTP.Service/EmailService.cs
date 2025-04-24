
using CTP.IRepository;
using CTP.IService;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.Service
{
    public class EmailService : BaseService<EmailCaptchaInfo>, IEmailService
    {
        public EmailService(IBaseRepository<EmailCaptchaInfo> repository) : base(repository)
        {
        }
    }
}
