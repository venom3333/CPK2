using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CPK.Api.SecondaryAdapters.Dto;
using CPK.ProductsModule.Dto;
using CPK.ProductsModule.Entities;
using CPK.ProductsModule.SecondaryPorts;
using CPK.SharedModule;
using CPK.SharedModule.Entities;

using Microsoft.EntityFrameworkCore;

namespace CPK.Api.SecondaryAdapters.Repositories
{
    internal sealed class ProductRepository : IProductsRepository
    {
        private readonly CpkContext _context;

        public ProductRepository(CpkContext context)
        {
            _context = context;
        }

        public async Task<List<ConcurrencyToken<Product>>> Get(ProductsFilter productsFilter)
        {
            var query = Filter(productsFilter);
            query = Order(productsFilter, query);
            var products = await query
                .Skip((int)productsFilter.PageFilter.Skip)
                .Take((int)productsFilter.PageFilter.Take)
                .ToListAsync();
            return products.Select(p => p.ToProduct()).ToList();
        }

        public void Add(Product product)
        {
            var p = new ProductDto(product, Guid.NewGuid().ToString());
            _context.Products.Add(p);
        }

        public async Task Update(ConcurrencyToken<Product> product)
        {
            var original = await _context.Products.SingleAsync(p => p.Id == product.Entity.Id.Value);
            original.Price = product.Entity.Price.Value;
            original.Title = product.Entity.Title.Value;
            _context.UpdateWithToken<Product, ProductDto, Guid>(product, original);
        }

        public async Task Remove(ConcurrencyToken<Id> id)
        {
            var original = await _context.Products.SingleAsync(p => p.Id == id.Entity.Value);
            _context.DeleteWithToken<Id, ProductDto, Guid>(id, original);
        }

        public async Task<ConcurrencyToken<Product>> Get(Id id)
        {
            var product = await _context.Products.FindAsync(id.Value);
            if (product == null)
                return default;
            return product.ToProduct();
        }

        public Task<int> Count(ProductsFilter productsFilter) => Filter(productsFilter).CountAsync();

        private IQueryable<ProductDto> Filter(ProductsFilter productsFilter)
        {
            var query = string.IsNullOrWhiteSpace(productsFilter.Title)
                ? _context.Products
                : _context.Products.Where(x => x.Title.Contains(productsFilter.Title));
            return query
                .Where(x => x.Price >= productsFilter.MinPrice &&
                            x.Price <= productsFilter.MaxPrice);
        }

        private IOrderedQueryable<ProductDto> Order(ProductsFilter productsFilter, IQueryable<ProductDto> query)
        {
            return productsFilter.OrderBy switch
            {
                ProductOrderBy.Id => productsFilter.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
                ProductOrderBy.Name => productsFilter.Descending ? query.OrderByDescending(x => x.Title) : query.OrderBy(x => x.Title),
                ProductOrderBy.Price => productsFilter.Descending ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price),
                _ => throw new NotImplementedException()
            };
        }
    }
}
