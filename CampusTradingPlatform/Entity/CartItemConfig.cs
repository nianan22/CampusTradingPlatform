using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CartItemConfig : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");
            builder.HasOne(ci => ci.Cart).WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId);
            builder.HasOne(ci => ci.Product).WithMany()
                .HasForeignKey(ci => ci.ProductId);
        }
    }
}
