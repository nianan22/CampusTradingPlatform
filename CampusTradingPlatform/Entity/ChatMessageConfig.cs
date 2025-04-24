using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ChatMessageConfig : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.ToTable("ChatMessage");
            builder.Property(a=>a.SenderId).IsRequired();
            builder.Property(a => a.ChatRoomId).IsRequired();
            builder.Property(a => a.Content).IsRequired();
            builder.HasOne<ChatRoom>()
       .WithMany(c => c.Messages)
       .HasForeignKey(m => m.ChatRoomId);

        }
    }
}
