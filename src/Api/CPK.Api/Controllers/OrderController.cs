using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CPK.Api.Helpers;
using CPK.Api.Models;
using CPK.Api.SecondaryAdapters;
using CPK.BasketModule.Entities;
using CPK.BasketModule.PrimaryPorts;
using CPK.OrdersModule.Entities;
using CPK.OrdersModule.PrimaryPorts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPK.Api.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public sealed class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly IBasketService _basket;
        private readonly CpkContext _context;

        public OrderController(IOrderService service, IBasketService basket, CpkContext context)
        {
            _service = service;
            _basket = basket;
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public async Task<Guid> Create(CreateOrderFullFormModel model)
        {
            var buyer = User.GetId();
            var order = new Order(model.Lines.Select(x => x.ToOrderLine()), new Client(buyer), new Address(model.Address));
            var id = await _service.Create(order);
            return id.Value;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<Guid> CreateAndClearBasket(CreateOrderShortModel model)
        {
            using (var tran = await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted))
            {
                var buyer = User.GetId();
                var bid = new BasketId(buyer);
                var basket = await _basket.Get(bid);
                await _basket.Clear(bid);
                var order = new Order(
                    basket.Lines.Select(l => new OrderLine(new OrderProduct(l.Product.Id, l.Product.Title, l.Product.Price), l.Quantity)),
                    new Client(buyer),
                    new Address(model.Address)
                    );
                var id = await _service.Create(order);
                await tran.CommitAsync();
                return id.Value;
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<OrderModel> Get(Guid id)
        {
            var order = await _service.Get(new OrderId(id));
            if (order == null)
                return null;
            return new OrderModel(order);
        }

        [Authorize]
        [HttpGet]
        public async Task<List<OrderModel>> Get()
        {
            var orders = await _service.Get(new Client(User.GetId()));
            return orders.Select(x => new OrderModel(x)).ToList();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}/delivered")]
        public async Task<int> Delivered(Guid id)
        {
            return await _service.Delivered(new OrderId(id));
        }
    }
}
