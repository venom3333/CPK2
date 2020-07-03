using System;
using System.Threading.Tasks;
using CPK.ProductCategoriesModule.Dto;
using CPK.ProductCategoriesModule.Entities;
using CPK.ProductCategoriesModule.PrimaryPorts;
using CPK.ProductCategoriesModule.SecondaryPorts;
using CPK.SharedModule;
using CPK.SharedModule.Entities;

namespace CPK.ProductCategoriesModule.PrimaryAdapters
{
    public sealed class ProductCategoriesService : IProductCategoriesService
    {
        private readonly IProductCategoriesUow _uow;
        private readonly IProductCategoriesRepository _repository;

        public ProductCategoriesService(IProductCategoriesUow uow, IProductCategoriesRepository repository)
        {
            _uow = uow;
            _repository = repository;
        }

        public async Task<int> Add(ProductCategory request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            _repository.Add(request);
            return await _uow.SaveAsync();
        }

        public async Task<PageResult<ConcurrencyToken<ProductCategory>>> Get(ProductCategoriesFilter request)
        {
            if (request == default)
                throw new ArgumentOutOfRangeException(nameof(request));
            var productCategory = await _repository.Get(request);
            var total = await _repository.Count(request);
            return new PageResult<ConcurrencyToken<ProductCategory>>(request.PageFilter, productCategory, (uint)total);
        }

        public async Task<int> Remove(ConcurrencyToken<Id> request)
        {
            if (request.Entity == default || string.IsNullOrWhiteSpace(request.Token))
                throw new ArgumentNullException(nameof(request));
            await _repository.Remove(request);
            var count = await _uow.SaveAsync();
            return count;
        }

        public async Task<int> Update(ConcurrencyToken<ProductCategory> request)
        {
            if (request.Entity == null || string.IsNullOrWhiteSpace(request.Token))
                throw new ArgumentNullException(nameof(request));
            await _repository.Update(request);
            var count = await _uow.SaveAsync();
            return count;
        }
    }
}
