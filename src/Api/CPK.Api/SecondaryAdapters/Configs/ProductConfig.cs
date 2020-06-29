using CPK.Api.SecondaryAdapters.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CPK.Api.SecondaryAdapters.Configs
{
    internal sealed class ProductConfig : IEntityTypeConfiguration<ProductDto>
    {
        public void Configure(EntityTypeBuilder<ProductDto> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasAlternateKey(b => b.Title);
        }
    }
}