using System;
using System.Threading.Tasks;
using CPK.Api.Helpers;
using CPK.Api.Models;
using CPK.Api.Models.Products;
using CPK.BasketModule.Entities;
using CPK.BasketModule.PrimaryPorts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CPK.Api.Controllers
{
    [ApiController]
    [Route("api/v1/basket")]
    [Authorize]
    public sealed class BasketController : ControllerBase
    {
        private readonly IBasketService _basket;

        public BasketController(IBasketService basket)
        {
            _basket = basket;
        }

        [HttpGet]
        public async Task<BasketModel> Get()
        {
            var userId = User.GetId();
            var id = new BasketId(userId);
            var basket = await _basket.Get(id);
            return new BasketModel(basket);
        }

        [HttpPost("lines/add")]
        public async Task<int> Add(ProductModel product)
        {
            var userId = User.GetId();
            var basketId = new BasketId(userId);
            return await _basket.AddProduct(basketId, new BasketProduct(product.Id, product.Title, product.Price));
        }

        [HttpPost("lines/{id}/remove")]
        public async Task<int> Remove(Guid id)
        {
            var userId = User.GetId();
            var basketId = new BasketId(userId);
            return await _basket.RemoveProduct(basketId, id);
        }

        [HttpDelete("lines")]
        public async Task<int> Clear()
        {
            var userId = User.GetId();
            var basketId = new BasketId(userId);
            return await _basket.Clear(basketId);
        }
    }
}
