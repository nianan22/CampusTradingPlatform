using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entity;

public class ProductInfoConfig : IEntityTypeConfiguration<ProductInfo>
{
    public void Configure(EntityTypeBuilder<ProductInfo> builder)
    {
        builder.ToTable("Products");

        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Category).IsRequired().HasMaxLength(100);
        builder.Property(p => p.PhotoUrl).IsRequired().HasMaxLength(100);
        builder.Property(p => p.ImagesUrl).IsRequired();
        builder.Property(p => p.Count).IsRequired();
        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p=>p.Status).IsRequired().HasMaxLength(20);
        builder.HasOne(p => p.User).WithMany().HasForeignKey(p => p.UserId);
        builder.Property(p=>p.Price).IsRequired();
    }
}