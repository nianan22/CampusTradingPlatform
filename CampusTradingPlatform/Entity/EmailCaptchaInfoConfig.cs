using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class EmailCaptchaInfoConfig : IEntityTypeConfiguration<EmailCaptchaInfo>
    {
        public void Configure(EntityTypeBuilder<EmailCaptchaInfo> builder)
        {
            builder.ToTable("T_EmailCaptcha");
            builder.Property(a=>a.Email).IsRequired().HasMaxLength(50);
            builder.Property(a=>a.Captcha).IsRequired().HasMaxLength(10);
            builder.Property(u => u.IsDeleted).HasDefaultValue(false);
        }
    }
}
