using CPK.Api.SecondaryAdapters.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CPK.Api.SecondaryAdapters.Configs
{
    internal sealed class OrderConfig : IEntityTypeConfiguration<OrderDto>
    {
        public void Configure(EntityTypeBuilder<OrderDto> builder)
        {
            builder.HasKey(dto => dto.Id);
        }
    }
}