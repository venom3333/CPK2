﻿using System;
using System.Collections.Generic;
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
            _context.UpdateWithToken<ProductCategory, ProductCategoryDto, Guid>(productCategory, original);
        }

        public async Task Remove(ConcurrencyToken<Id> id)
        {
            var original = await _context.Products.SingleAsync(p => p.Id == id.Entity.Value);
            _context.DeleteWithToken<Id, ProductDto, Guid>(id, original);
        }

        public async Task<ConcurrencyToken<ProductCategory>> Get(Id id)
        {
            var productCategory = await _context.ProductCategories.FindAsync(id.Value);
            if (productCategory == null)
                return default;
            return productCategory.ToProductCategory();
        }

        public Task<int> Count(ProductCategoriesFilter productsFilter) => Filter(productsFilter).CountAsync();

        private IQueryable<ProductCategoryDto> Filter(ProductCategoriesFilter productsFilter)
        {
            var query = string.IsNullOrWhiteSpace(productsFilter.Title)
                ? _context.ProductCategories
                : _context.ProductCategories.Where(x => x.Title.Contains(productsFilter.Title));
            return query;
        }

        private IOrderedQueryable<ProductCategoryDto> Order(ProductCategoriesFilter productsFilter, IQueryable<ProductCategoryDto> query)
        {
            return productsFilter.OrderBy switch
            {
                ProductCategoryOrderBy.Id => productsFilter.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
                ProductCategoryOrderBy.Title => productsFilter.Descending ? query.OrderByDescending(x => x.Title) : query.OrderBy(x => x.Title),
                _ => throw new NotImplementedException()
            };
        }
    }
}