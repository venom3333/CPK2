using System;
using System.Linq;
using System.Threading.Tasks;

using CPK.Api.Models;
using CPK.ProductCategoriesModule.Dto;
using CPK.ProductCategoriesModule.Entities;
using CPK.ProductCategoriesModule.PrimaryPorts;
using CPK.ProductsModule.Dto;
using CPK.ProductsModule.Entities;
using CPK.ProductsModule.PrimaryPorts;
using CPK.SharedModule;
using CPK.SharedModule.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace CPK.Api.Controllers
{
    [ApiController]
    [Route("api/v1/productcategories")]
    [Authorize(Roles = "admin")]
    public sealed class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoriesService _service;
        private readonly IHostEnvironment _environment;

        private const uint MaxTake = 1000;

        public ProductCategoriesController(IProductCategoriesService service, IHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        [AllowAnonymous]
        [HttpPost("filter")]
        public async Task<IActionResult> GetAll(ProductCategoriesPageModel productsPage)
        {
            var productCategories = await _service.Get(new ProductCategoriesFilter(new PageFilter(productsPage.Skip, productsPage.Take, MaxTake), productsPage.Title, productsPage.Descending, productsPage.OrderBy));
            return Ok(new PageResultModel<ProductCategoryModel>()
            {
                ProductsFilter = productCategories.PageFilter,
                TotalCount = productCategories.TotalCount,
                Value = productCategories.Value.Select(p => new ProductCategoryModel(p)).ToList()
            });
        }

        [HttpPost]
        public async Task<Guid> Add(AddProductCategoryModel model)
        {
            var id = Guid.NewGuid();
            await _service.Add(new ProductCategory(new Id(id), new Title(model.Title), new ProductCategoryShortDescription(model.ShortDescription), new Image(model.ImageId)));
            return id;
        }

        [HttpPut]
        public async Task<int> Update(ProductCategoryModel model)
        {
            return await _service.Update(model.ToProductCategory());
        }

        [HttpDelete("{id}")]
        public async Task<int> Remove(Guid id, string version)
        {
            return await _service.Remove(new ConcurrencyToken<Id>(version, new Id(id)));
        }
    }
}
