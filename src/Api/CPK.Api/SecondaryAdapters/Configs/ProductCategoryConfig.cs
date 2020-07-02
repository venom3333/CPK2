using CPK.Api.SecondaryAdapters.Dto;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CPK.Api.SecondaryAdapters.Configs
{
    internal sealed class ProductCategoryConfig : IEntityTypeConfiguration<ProductCategoryDto>
    {
        public void Configure(EntityTypeBuilder<ProductCategoryDto> entity)
        {
            entity.HasKey(e => e.Id);

            entity.HasAlternateKey(e => e.Title);

            entity.HasMany(e => e.Products)
                .WithOne(p => p.Category);

            entity.HasOne(x => x.Image)
                .WithMany()
                .HasForeignKey(x => x.ImageId);
        }
    }
}