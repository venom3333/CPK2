﻿using System;
using System.Linq;
using System.Threading.Tasks;
using CPK.Api.Models;
using CPK.ProductsModule.Dto;
using CPK.ProductsModule.Entities;
using CPK.ProductsModule.PrimaryPorts;
using CPK.SharedModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace CPK.Api.Controllers
{
    [ApiController]
    [Route("api/v1/products")]
    [Authorize(Roles = "admin")]
    public sealed class ProductsController : ControllerBase
    {
        private readonly IProductsService _service;
        private readonly IHostEnvironment _environment;

        private const uint MAX_TAKE = 1000;

        public ProductsController(IProductsService service, IHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        [AllowAnonymous]
        [HttpPost("filter")]
        public async Task<IActionResult> GetAll(PageModel page)
        {
            var products = await _service.Get(new ProductsFilter(new PageFilter(page.Skip, page.Take, MAX_TAKE), page.Title, page.MinPrice, page.MaxPrice, page.Descending, page.OrderBy));
            return Ok(new PageResultModel<ProductModel>()
            {
                ProductsFilter = products.PageFilter,
                TotalCount = products.TotalCount,
                Value = products.Value.Select(p => new ProductModel(p)).ToList()
            });
        }

        [HttpPost]
        public async Task<Guid> Add(AddProductModel model)
        {
            var id = Guid.NewGuid();
            await _service.Add(new Product(new ProductId(id), new Title(model.Title), new Money(model.Price), new Image(model.ImageId)));
            return id;
        }

        [HttpPut]
        public async Task<int> Update(ProductModel product)
        {
            return await _service.Update(product.ToProduct());
        }

        [HttpDelete("{id}")]
        public async Task<int> Remove(Guid id, string version)
        {
            return await _service.Remove(new ConcurrencyToken<ProductId>(version, new ProductId(id)));
        }
    }
}