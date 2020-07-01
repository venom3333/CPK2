using System;
using System.Threading.Tasks;

using CPK.ProductsModule.Dto;
using CPK.ProductsModule.Entities;
using CPK.ProductsModule.PrimaryPorts;
using CPK.ProductsModule.SecondaryPorts;
using CPK.SharedModule;
using CPK.SharedModule.Entities;

namespace CPK.ProductsModule.PrimaryAdapters
{

    public sealed class ProductsService : IProductsService
    {
        private readonly IProductsUow _uow;
        private readonly IProductsRepository _repository;

        public ProductsService(IProductsUow uow, IProductsRepository repository)
        {
            _uow = uow;
            _repository = repository;
        }

        public async Task<int> Add(Product request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            _repository.Add(request);
            return await _uow.SaveAsync();
        }

        public async Task<PageResult<ConcurrencyToken<Product>>> Get(ProductsFilter request)
        {
            if (request == default)
                throw new ArgumentOutOfRangeException(nameof(request));
            var products = await _repository.Get(request);
            var total = await _repository.Count(request);
            return new PageResult<ConcurrencyToken<Product>>(request.PageFilter, products, (uint)total);
        }

        public async Task<int> Remove(ConcurrencyToken<Id> request)
        {
            if (request.Entity != default || string.IsNullOrWhiteSpace(request.Token))
                throw new ArgumentNullException(nameof(request));
            await _repository.Remove(request);
            var count = await _uow.SaveAsync();
            return count;
        }

        public async Task<int> Update(ConcurrencyToken<Product> request)
        {
            if (request.Entity == null || string.IsNullOrWhiteSpace(request.Token))
                throw new ArgumentNullException(nameof(request));
            await _repository.Update(request);
            var count = await _uow.SaveAsync();
            return count;
        }
    }
}
