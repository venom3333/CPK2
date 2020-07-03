using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using CPK.Api.SecondaryAdapters.Dto;
using CPK.ProductCategoriesModule.Dto;
using CPK.ProductCategoriesModule.Entities;
using CPK.ProductCategoriesModule.SecondaryPorts;
using CPK.ProductsModule.Dto;
using CPK.ProductsModule.Entities;
using CPK.ProductsModule.SecondaryPorts;
using CPK.SharedModule;
using CPK.SharedModule.Entities;

using Microsoft.EntityFrameworkCore;

namespace CPK.Api.SecondaryAdapters.Repositories
{
    internal sealed class ProductCategoryRepository : IProductCategoriesRepository
    {
        private readonly CpkContext _context;

        public ProductCategoryRepository(CpkContext context)
        {
            _context = context;
        }

        public async Task<List<ConcurrencyToken<ProductCategory>>> Get(ProductCategoriesFilter productCategoriesFilter)
        {
            var query = Filter(productCategoriesFilter);
            query = Order(productCategoriesFilter, query);
            var productCategories = await query
                .Skip((int)productCategoriesFilter.PageFilter.Skip)
                .Take((int)productCategoriesFilter.PageFilter.Take)
                .ToListAsync();
            return productCategories.Select(p => p.ToProductCategory()).ToList();
        }

        public void Add(ProductCategory productCategory)
        {
            var p = new ProductCategoryDto(productCategory, Guid.NewGuid().ToString());
            _context.ProductCategories.Add(p);
        }

        public async Task Update(ConcurrencyToken<ProductCategory> productCategory)
        {
            var original = await _context.ProductCategories.SingleAsync(p => p.Id == productCategory.Entity.Id.Value);
            original.ShortDescription = productCategory.Entity.ShortDescription.Value;
            original.Title = productCategory.Entity.Title.Value;
            original.ImageId = productCategory.Entity.Image.Value;
            _context.UpdateWithToken<ProductCategory, ProductCategoryDto, Guid>(productCategory, original);
        }

        public async Task Remove(ConcurrencyToken<Id> id)
        {
            var original = await _context.ProductCategories.Include(c => c.Image).SingleAsync(p => p.Id == id.Entity.Value);
            if (original.Image != null)
            {
                TryDeleteImage(original.Image);
            }
            _context.DeleteWithToken<Id, ProductCategoryDto, Guid>(id, original);
        }

        private void TryDeleteImage(FileDto originalImage)
        {
            if (File.Exists(originalImage.Path))
            {
                File.Delete(originalImage.Path);
            }
        }

        public async Task<ConcurrencyToken<ProductCategory>> Get(Id id)
        {
            var productCategory = await _context.ProductCategories.FindAsync(id.Value);
            if (productCategory == null)
                return default;
            return productCategory.ToProductCategory();
        }

        public Task<int> Count(ProductCategoriesFilter filter) => Filter(filter).CountAsync();

        private IQueryable<ProductCategoryDto> Filter(ProductCategoriesFilter filter)
        {
            var query = _context.ProductCategories.AsQueryable();
            if (filter.Id != default)
            {
                query = query.Where(x => x.Id == filter.Id);
            }
            else
            {
                query = string.IsNullOrWhiteSpace(filter.Title)
                    ? query
                    : query.Where(x => x.Title.Contains(filter.Title));
            }
            return query;
        }

        private IOrderedQueryable<ProductCategoryDto> Order(ProductCategoriesFilter filter, IQueryable<ProductCategoryDto> query)
        {
            return filter.OrderBy switch
            {
                ProductCategoryOrderBy.Id => filter.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
                ProductCategoryOrderBy.Title => filter.Descending ? query.OrderByDescending(x => x.Title) : query.OrderBy(x => x.Title),
                _ => throw new NotImplementedException()
            };
        }
    }
}
