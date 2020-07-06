using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;

using CPK.Contracts;
using CPK.Spa.Client.Core.HttpContext;
using CPK.Spa.Client.Core.Models;
using CPK.Spa.Client.Core.Models.News;
using CPK.Spa.Client.Core.Models.ProductCategories;
using CPK.Spa.Client.Core.Models.Products;
using MatBlazor;

namespace CPK.Spa.Client.Core.Repositories
{
    public sealed class ApiRepository : IApiRepository
    {
        private readonly ConfigModel _config;
        private readonly IHttpContext _http;

        public ApiRepository(ConfigModel config, IHttpContext http)
        {
            _config = config;
            _http = http;
        }

        public Task<(PageResultModel<ProductModel>, string)> GetFilteredProducts(ProductsFilterModel model)
        {
            var url = GetFullUrl("/products/filter");
            return _http.PostAsync<PageResultModel<ProductModel>>(url, model, false);
        }

        public Task<(int, string)> AddToBasket(ProductModel product)
        {
            var url = GetFullUrl($"/basket/lines/add");
            return _http.PostAsync<int>(url, product);
        }

        public Task<(BasketModel, string)> GetBasket()
        {
            var url = GetFullUrl("/basket");
            return _http.GetAsync<BasketModel>(url);
        }

        public Task<(int, string)> RemoveProduct(Guid id)
        {
            var url = GetFullUrl($"basket/lines/{id}/remove");
            return _http.PostAsync<int>(url, "");
        }

        public Task<(int, string)> ClearBasket()
        {
            var url = GetFullUrl("basket/lines");
            return _http.DeleteAsync<int>(url);
        }

        public Task<(Guid, string)> CreateOrder(IEnumerable<LineModel> lines, string address)
        {
            var url = GetFullUrl("orders");
            return _http.PostAsync<Guid>(url, new { lines, address });
        }

        public Task<(List<OrderModel>, string)> GetOrders()
        {
            var url = GetFullUrl("orders");
            return _http.GetAsync<List<OrderModel>>(url);
        }

        public string GetFullUrl(string path)
        {
            var uri = _config.ApiUri;
            uri = (uri.EndsWith('/') ? uri : uri + "/");
            path = path.StartsWith('/') ? path : ("/" + path);
            return $"{uri}api/v1{path}";
        }

        // Categories
        public Task<(PageResultModel<ProductCategoryModel>, string)> GetFilteredProductCategories(ProductCategoriesFilterModel model)
        {
            var url = GetFullUrl("/productCategories/filter");
            return _http.PostAsync<PageResultModel<ProductCategoryModel>>(url, model, false);
        }
        public Task<(Guid, string)> CreateCategory(ProductCategoryModel model)
        {
            var url = GetFullUrl("/productCategories/add");
            return _http.PostAsync<Guid>(url, model);
        }

        public Task<(int, string)> UpdateCategory(ProductCategoryModel model)
        {
            var url = GetFullUrl("/productCategories/Update");
            return _http.PutAsync<int>(url, model);
        }

        public Task<(int, string)> RemoveCategory(Guid id, string version)
        {
            var url = GetFullUrl($"/productCategories/remove/{id}/{version}");
            return _http.DeleteAsync<int>(url);
        }

        // News
        public Task<(PageResultModel<NewsModel>, string)> GetFilteredNews(NewsFilterModel model)
        {
            var url = GetFullUrl("/news/filter");
            return _http.PostAsync<PageResultModel<NewsModel>>(url, model, false);
        }
        public Task<(Guid, string)> CreateNews(NewsModel model)
        {
            var url = GetFullUrl("/news/add");
            return _http.PostAsync<Guid>(url, model);
        }

        public Task<(int, string)> UpdateNews(NewsModel model)
        {
            var url = GetFullUrl("/news/Update");
            return _http.PutAsync<int>(url, model);
        }

        public Task<(int, string)> RemoveNews(Guid id, string version)
        {
            var url = GetFullUrl($"/news/remove/{id}/{version}");
            return _http.DeleteAsync<int>(url);
        }
        
        // Files
        public async Task<(Guid, string)> UploadFile(FileModel model)
        {
            var url = GetFullUrl("/files/upload");
            var result = await _http.PostFileAsync<Guid>(url, model);
            return result;
        }

        public async Task<(FileModel, string)> GetFile(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
