using Common;
using Entity.Email;
using MimeKit;
using System.Diagnostics;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace CampusTradingPlatform.Email
{
    public class MailQueueManager
    {
        // 发送邮件客户端
        private readonly SmtpClient _client;
        private Thread? _thread = null;

        public MailQueueManager()
        {
            _client = new SmtpClient();
        }

        /// <summary>
        /// 启动队列
        /// </summary>
        public void Run()
        {

            _thread = new Thread(StartSendMail)
            {
                Name = "PmpEmailQueue",
                IsBackground = true,
            };
            Console.WriteLine("线程即将启动");
            _thread.Start();
            Console.WriteLine("线程已经启动，线程Id是：{0}", _thread.ManagedThreadId);
        }

        /// <summary>
        /// 开始发送邮件
        /// </summary>
        private void StartSendMail()
        {
            var sw = new Stopwatch();
            try
            {
                while (true)
                {
                    if (MailQueueProvider._mailQueue.Count <= 0)
                    {
                        Console.WriteLine("队列是空，开始睡眠");
                        Thread.Sleep(5000);
                        continue;
                    }
                    if (MailQueueProvider.TryDequeue(out MailBox box))
                    {

                        Console.WriteLine($"开始发送邮件 标题：{box.Subject}, 收件人：{string.Join(", ", box.To)}");
                        sw.Restart();
                        SendMail(box);
                        sw.Stop();
                        Console.WriteLine($"发送邮件结束 标题：{box.Subject}, 收件人：{string.Join(", ", box.To)}, 耗时：{sw.Elapsed.TotalSeconds} 秒");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString(), "循环中出错,线程即将结束");

            }
        }

        /// <summary>
        /// 开始构建具体的邮件内容
        /// </summary>
        /// <param name="box"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private void SendMail(MailBox box)
        {
            if (box == null)
            {
                throw new ArgumentNullException(nameof(box));
            }

            try
            {
                MimeMessage message = ConvertToMimeMessage(box);
                SendMail(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString(), "发送邮件发生异常主题：{0},收件人：{1}", box.Subject, box.To!.First());
            }

        }

        /// <summary>
        /// 邮件具体的内容转换成MimeMessage
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private MimeMessage ConvertToMimeMessage(MailBox box)
        {

            if (box == null)
            {
                throw new ArgumentNullException(nameof(box));
            }

            var message = new MimeMessage();
            //MailboxAddress(发送者名称，发送者账号)
            message.From.Add(new MailboxAddress("校园交易平台系统", "1078322198@qq.com"));

            var validRecipients = box.To.Where(f => !string.IsNullOrWhiteSpace(f)).ToList();
            if (!validRecipients.Any())
            {
                throw new InvalidOperationException("没有有效的收件人邮箱地址");
            }
            //下面构建的是接收者账号
            var mailboxAddressList = new List<MailboxAddress>();
            box.To!.ToList().ForEach(f =>
            {

                mailboxAddressList.Add(new MailboxAddress("用户", f));
            });
            message.To.AddRange(mailboxAddressList);

            message.Subject = box.Subject;// 邮件主题

            // 下面构建邮件内容
            var builder = new BodyBuilder();
            if (box.IsHtml)
            {
                builder.HtmlBody = box.Body;
            }
            else
            {
                builder.TextBody = box.Body;
            }
            message.Body = builder.ToMessageBody();
            return message;
        }

        /// <summary>
        /// 邮件的具体发送
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private void SendMail(MimeMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            try
            {
                Console.WriteLine("开始连接到 SMTP 服务器");
                _client.Connect("smtp.qq.com", 587, SecureSocketOptions.StartTls); // 使用 STARTTLS
                Console.WriteLine("连接到 SMTP 服务器成功");

                if (!_client.IsAuthenticated)
                {
                    Console.WriteLine("开始认证");
                    _client.Authenticate("1078322198@qq.com", "vtczmykjilgmbacg");
                    Console.WriteLine("认证成功");
                }

                Console.WriteLine("开始发送邮件");
                _client.Send(message);
                Console.WriteLine("邮件发送成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"邮件发送失败: {ex.Message}");
            }
            finally
            {
                _client.Disconnect(true); // 断开连接时建议设置 true 以确保资源释放
                Console.WriteLine("断开与 SMTP 服务器的连接");
            }
        }

    }



}


