using Domain.Image;
using Domain.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class Products2Config : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
       
        builder.HasOne(p => p.Image)
               .WithOne(i => i.Product)
               .HasForeignKey<Image>(i => i.ProductId)
               .IsRequired();



    }
}
