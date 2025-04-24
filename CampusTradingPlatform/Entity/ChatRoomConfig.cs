using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ChatRoomConfig : IEntityTypeConfiguration<ChatRoom>
    {
        public void Configure(EntityTypeBuilder<ChatRoom> builder)
        {
            builder.ToTable("ChatRoom");
            builder.Property(a => a.ProductId).IsRequired();
            builder.Property(a => a.SellerId).IsRequired();
            builder.Property(a => a.BuyerId).IsRequired();
        }
    }
}
