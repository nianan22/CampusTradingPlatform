using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class EmailCaptchaInfo :BaseEntity<int>
    {
        public string Captcha {  get; set; }
        public string Email {  get; set; }
    }
}
