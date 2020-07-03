using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.Spa.Client.Core.Models;
using CPK.Spa.Client.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace CPK.Spa.Client.Core.Services
{
    public class ProductCategoriesService : IProductCategoriesService
    {
        private readonly IApiRepository _repository;
        private readonly ILogger<ProductCategoriesService> _logger;
        private PageResultModel<ProductCategoryModel> _page;

        public ProductCategoriesService(IApiRepository repository, ILogger<ProductCategoriesService> logger)
        {
            _repository = repository;
            _logger = logger;
            _page = new PageResultModel<ProductCategoryModel>();
        }
        public string Error { get; private set; }
        public IReadOnlyList<ProductCategoryModel> List => _page?.Value?.AsReadOnly();
        public int TotalCount => _page?.TotalCount ?? 0;
        public async Task Load(ProductCategoriesFilterModel filter)
        {
            _logger.LogDebug("LOAD CATEGORIES!");
            var (r, e) = await _repository.GetFilteredProductCategories(filter);
            _page = r;
            Error = e;
        }

        public string ImageUri(Guid id) => _repository.GetFullUrl($"files/{id}");

        public async Task Create(ProductCategoryModel model)
        {
            var(result, error) = await _repository.CreateCategory(model);
            ShowErrorIfNecessary(error);
        }

        public async Task Update(ProductCategoryModel model)
        {
            var(result, error) = await _repository.UpdateCategory(model);
            ShowErrorIfNecessary(error);
        }

        public async Task Delete(ProductCategoryModel model)
        {
            var(result, error) = await _repository.RemoveCategory(model.Id, model.Version);
            ShowErrorIfNecessary(error);
        }

        private void ShowErrorIfNecessary(string error) {
            if (!string.IsNullOrWhiteSpace(error)) Error = error.ToString();
        }
    }
}
