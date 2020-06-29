using CPK.Api.SecondaryAdapters.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CPK.Api.SecondaryAdapters.Configs
{
    internal sealed class BasketLineConfig : IEntityTypeConfiguration<BasketLineDto>
    {
        public void Configure(EntityTypeBuilder<BasketLineDto> builder)
        {
            builder.HasKey(b => new { b.ProductId, b.BasketId });
            builder.HasOne(b => b.Basket)
                .WithMany(b => b.Lines)
                .HasForeignKey(b => b.BasketId);
            builder.HasOne(b => b.Product)
                .WithMany(b => b.Baskets)
                .HasForeignKey(b => b.ProductId);
        }
    }
}