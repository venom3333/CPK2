using CPK.Api.SecondaryAdapters.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CPK.Api.SecondaryAdapters.Configs
{
    internal sealed class ProductCategoryConfig : IEntityTypeConfiguration<ProductCategoryDto>
    {
        public void Configure(EntityTypeBuilder<ProductCategoryDto> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasAlternateKey(b => b.Title);
            builder.HasMany(b => b.Products)
                .WithOne(p => p.Category);
        }
    }
}