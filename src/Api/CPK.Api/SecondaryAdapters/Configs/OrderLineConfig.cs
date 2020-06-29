using CPK.Api.SecondaryAdapters.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CPK.Api.SecondaryAdapters.Configs
{
    internal sealed class OrderLineConfig : IEntityTypeConfiguration<OrderLineDto>
    {
        public void Configure(EntityTypeBuilder<OrderLineDto> builder)
        {
            builder.HasKey(b => new { b.ProductId, b.OrderId });
            builder.HasOne(b => b.Order)
                .WithMany(b => b.Lines)
                .HasForeignKey(b => b.OrderId);
            builder.HasOne(b => b.Product)
                .WithMany(b => b.Orders)
                .HasForeignKey(b => b.ProductId);
        }
    }
}