using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CPK.Api.SecondaryAdapters.Dto;
using CPK.FilesModule.SecondaryPorts;
using CPK.ProductCategoriesModule.Dto;
using CPK.ProductCategoriesModule.Entities;
using CPK.ProductCategoriesModule.SecondaryPorts;
using CPK.ProductsModule.Dto;
using CPK.ProductsModule.Entities;
using CPK.ProductsModule.SecondaryPorts;
using CPK.SharedModule;
using CPK.SharedModule.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CPK.Api.SecondaryAdapters.Repositories
{
    internal sealed class ProductCategoryRepository : IProductCategoriesRepository
    {
        private readonly CpkContext _context;
        private readonly ICategoryFilesRepository _categoryFiles;

        public ProductCategoryRepository(CpkContext context, ICategoryFilesRepository categoryFiles)
        {
            _context = context;
            _categoryFiles = categoryFiles;
        }

        public async Task<List<ConcurrencyToken<ProductCategory>>> Get(ProductCategoriesFilter productCategoriesFilter)
        {
            var query = Filter(productCategoriesFilter);
            query = Order(productCategoriesFilter, query);
            var productCategories = await query
                .Skip((int) productCategoriesFilter.PageFilter.Skip)
                .Take((int) productCategoriesFilter.PageFilter.Take)
                .ToListAsync();
            return productCategories.Select(p => p.ToProductCategory()).ToList();
        }

        public async Task Add(ProductCategory productCategory)
        {
            await CheckForDoublesOrThrow(productCategory);

            var p = new ProductCategoryDto(productCategory, Guid.NewGuid().ToString());
            await _context.ProductCategories.AddAsync(p);
        }

        private async Task CheckForDoublesOrThrow(ProductCategory productCategory)
        {
            if (await _context.ProductCategories.AnyAsync(entity =>
                entity.Id != productCategory.Id.Value && entity.Title == productCategory.Title.Value))
            {
                throw new ApiException(ApiExceptionCode.EntityAlreadyExists, null,
                    "Категория с таким наименованием уже существует!");
            }
        }

        public async Task Update(ConcurrencyToken<ProductCategory> productCategory)
        {
            await CheckForDoublesOrThrow(productCategory.Entity);
            var original = await _context.ProductCategories.SingleAsync(p => p.Id == productCategory.Entity.Id.Value);
            original.ShortDescription = productCategory.Entity.ShortDescription.Value;
            original.Title = productCategory.Entity.Title.Value;

            var originalImageId = original.ImageId;

            original.ImageId = productCategory.Entity.Image.Value;
            _context.UpdateWithToken<ProductCategory, ProductCategoryDto, Guid>(productCategory, original);

            if (productCategory.Entity.Image.Value != originalImageId && originalImageId != null)
            {
                await _categoryFiles.Remove(originalImageId.Value, original.Id);
            }
        }

        public async Task Remove(ConcurrencyToken<Id> id)
        {
            var original = await _context.ProductCategories
                .SingleAsync(p => p.Id == id.Entity.Value);
            _context.DeleteWithToken<Id, ProductCategoryDto, Guid>(id, original);
            if (original.ImageId != null)
            {
                await _categoryFiles.Remove(original.ImageId.Value, original.Id);
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

        private IOrderedQueryable<ProductCategoryDto> Order(ProductCategoriesFilter filter,
            IQueryable<ProductCategoryDto> query)
        {
            return filter.OrderBy switch
            {
                ProductCategoryOrderBy.Id => filter.Descending
                    ? query.OrderByDescending(x => x.Id)
                    : query.OrderBy(x => x.Id),
                ProductCategoryOrderBy.Title => filter.Descending
                    ? query.OrderByDescending(x => x.Title)
                    : query.OrderBy(x => x.Title),
                _ => throw new NotImplementedException()
            };
        }
    }
}