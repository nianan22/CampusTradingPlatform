
using CTP.IRepository;
using CTP.IService;
using Entity;
using Entity.Email;
using EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace CTP.Service
{
    public class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {
        private readonly IUserInfoRepository userInfoRepository;
        private readonly IEmailRepository emailRepository;
        private readonly MyDbContext myDbContext;
        public UserInfoService(IUserInfoRepository userInfoRepository, IEmailRepository emailRepository, MyDbContext myDbContext,IBaseRepository<UserInfo> repository):base(repository)
        {
            base.repository = userInfoRepository;
            this.userInfoRepository = userInfoRepository;
            this.emailRepository = emailRepository;
            this.myDbContext = myDbContext;
        }

        public async Task<string> RegeistEmail(string email)
        {
            Random rnd = new Random();
            var user = await  userInfoRepository.LoadEntities(a=>a.UserEmail==email).FirstOrDefaultAsync();
            if (user != null)
            {
                return null ;
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("邮箱地址不能为空", nameof(email));
            }
            
            //验证码生成
            string str = "";
            for(int i=0;i<4;i++)
            {
                int randomNumber = rnd.Next(10);
                str += randomNumber;
            }
            var emailcap = await emailRepository.LoadEntities(a => a.Email == email).FirstOrDefaultAsync();
            if (emailcap == null)
            {
                EmailCaptchaInfo captchaInfo = new EmailCaptchaInfo();
                captchaInfo.Email = email;
                captchaInfo.Captcha = str;
                captchaInfo.CreateDate = DateTime.Now;
                await emailRepository.AddEntity(captchaInfo);
                myDbContext.SaveChanges();
            }
            else
            {
                emailcap.Captcha = str;
                emailcap.CreateDate = DateTime.Now;
                await emailRepository.UpdateEntity(emailcap);
                myDbContext.SaveChanges();
            }
            // 构建邮件内容
            MailBox mailBox = new MailBox()
            {
                Body = $"你的校园交易平台验证码是：{str}         5分钟内有效",
                IsHtml = false,
                Subject = "注册验证码",
                To = new List<string>() { email }
            };
            // 将邮件的内容入队
            Common.MailQueueProvider.Enqueue(mailBox);
            return str;
        }
    }
}
