using CPK.Api.SecondaryAdapters.Dto;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CPK.Api.SecondaryAdapters.Configs
{
    internal sealed class NewsConfig : IEntityTypeConfiguration<NewsDto>
    {
        public void Configure(EntityTypeBuilder<NewsDto> entity)
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Title).IsUnique();

            entity.HasOne(x => x.Image)
                .WithMany()
                .HasForeignKey(x => x.ImageId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Created).HasDefaultValueSql("NOW()");
        }
    }
}