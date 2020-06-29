using CPK.Api.SecondaryAdapters.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CPK.Api.SecondaryAdapters.Configs
{
    internal sealed class BasketConfig : IEntityTypeConfiguration<BasketDto>
    {
        public void Configure(EntityTypeBuilder<BasketDto> builder)
        {
            builder.HasKey(b => b.Id);
        }
    }

    internal sealed class FileConfig : IEntityTypeConfiguration<FileDto>
    {
        public void Configure(EntityTypeBuilder<FileDto> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasIndex(f => new { f.Hash, f.Size }).IsUnique();
        }
    }
}