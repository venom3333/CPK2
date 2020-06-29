using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.Spa.Client.Core.Models;
using CPK.Spa.Client.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace CPK.Spa.Client.Core.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IApiRepository _repository;
        private readonly ILogger<ProductsService> _logger;
        private PageResultModel<ProductModel> _page;

        public ProductsService(IApiRepository repository, ILogger<ProductsService> logger)
        {
            _repository = repository;
            _logger = logger;
            _page = new PageResultModel<ProductModel>();
        }
        public string Error { get; private set; }
        public IReadOnlyList<ProductModel> Model => _page?.Value?.AsReadOnly();
        public int TotalCount => _page?.TotalCount ?? 0;
        public async Task Load(ProductsFilterModel filter)
        {
            _logger.LogDebug("LOAD PRODUCTS!");
            var (r, e) = await _repository.GetFiltered(filter);
            _page = r;
            Error = e;
        }

        public string ImageUri(ProductModel product) => _repository.GetFullUrl($"files/{product.ImageId}");
    }
}
