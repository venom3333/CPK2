using System;
using CPK.Api.SecondaryAdapters.Configs;
using CPK.Api.SecondaryAdapters.Dto;
using CPK.SharedModule;
using Microsoft.EntityFrameworkCore;

namespace CPK.Api.SecondaryAdapters
{
    public sealed class CpkContext : DbContext
    {
        public CpkContext(DbContextOptions<CpkContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BasketConfig).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ProductDto> Products { get; set; }
        public DbSet<ProductCategoryDto> ProductCategories { get; set; }
        
        public DbSet<BasketDto> Baskets { get; set; }
        public DbSet<BasketLineDto> BasketLines { get; set; }
        public DbSet<OrderDto> Orders { get; set; }
        public DbSet<OrderLineDto> OrderLines { get; set; }
        public DbSet<CategoryFileDto> CategoryFiles { get; set; }

        public void DeleteWithToken<TEntity, TDto, T>(ConcurrencyToken<TEntity> entity, TDto dto)
            where TDto : EntityDto<T> where T : IEquatable<T>
        {
            PrepareWithToken<TEntity, TDto, T>(entity, dto, EntityState.Deleted);
        }

        public void UpdateWithToken<TEntity, TDto, T>(ConcurrencyToken<TEntity> entity, TDto dto)
            where TDto : EntityDto<T> where T : IEquatable<T>
        {
            PrepareWithToken<TEntity, TDto, T>(entity, dto, EntityState.Modified);
        }

        private void PrepareWithToken<TEntity, TDto, T>(ConcurrencyToken<TEntity> entity, TDto dto, EntityState state)
            where TDto : EntityDto<T> where T : IEquatable<T>
        {
            dto.ConcurrencyToken = entity.Token;
            this.Entry(dto).Property(x => x.ConcurrencyToken).CurrentValue = Guid.NewGuid().ToString("D");
            this.Entry(dto).Property(x => x.ConcurrencyToken).OriginalValue = entity.Token;
            this.Entry(dto).State = state;
        }
    }
}
