using Entity.Email;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class MailQueueProvider
    {
        public static readonly ConcurrentQueue<MailBox> _mailQueue = new ConcurrentQueue<MailBox>();

        public static void Enqueue(MailBox mailBox)
        {
            Console.WriteLine($"邮件已入队: {mailBox.Subject} -> {string.Join(", ", mailBox.To)}");
            _mailQueue.Enqueue(mailBox);
        }
        public static bool TryDequeue(out MailBox mailBox)
        {
            return _mailQueue.TryDequeue(out mailBox!);
        }
    }
}
