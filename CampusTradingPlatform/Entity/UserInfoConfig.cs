using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class UserInfoConfig : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable("T_Users");
            builder.Property(u => u.UserName).HasMaxLength(20).IsRequired();
            builder.Property(u => u.UserPassword).HasMaxLength(20).IsRequired();
            builder.Property(u => u.UserEmail).HasMaxLength(100).IsRequired();
            builder.Property(u => u.UserPhone).HasMaxLength(16).IsRequired();
            builder.Property(u => u.Gender).HasDefaultValue(0);
            builder.Property(u => u.PhotoUrl).HasMaxLength(100).IsRequired();
            builder.Property(u => u.IsDeleted).HasDefaultValue(false);
            builder.HasMany(u => u.Products) // 添加: 用户与商品的关系
                   .WithOne()
                   .HasForeignKey(p => p.UserId); // 假设在 ProductInfo 中添加 UserId 属性

        }
    }
}