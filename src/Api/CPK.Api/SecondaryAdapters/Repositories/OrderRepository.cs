using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPK.Api.SecondaryAdapters.Dto;
using CPK.OrdersModule.Entities;
using CPK.OrdersModule.SecondaryPorts;
using Microsoft.EntityFrameworkCore;

namespace CPK.Api.SecondaryAdapters.Repositories
{
    internal sealed class OrderRepository : IOrderRepository
    {
        private readonly CpkContext _context;

        public OrderRepository(CpkContext context)
        {
            _context = context;
        }

        public void Add(Order order)
        {
            var dto = new OrderDto();
            dto.Id = order.Id.Value;
            dto.Lines = order.Lines.Select(l => new OrderLineDto()
            {
                Quantity = (int)l.Quantity,
                OrderId = dto.Id,
                ProductId = l.Product.Id
            })
                .ToList();
            dto.BuyerId = order.Buyer.Id;
            dto.Status = order.State;
            dto.Address = order.Address.Value;
            _context.Orders.Add(dto);
        }

        public async Task<Order> Get(OrderId id)
        {
            var order = await _context.Orders.Include(x => x.Lines).FirstOrDefaultAsync(x => x.Id == id.Value);
            if (order == null)
                return null;
            return order.ToOrder();
        }

        public async Task<List<Order>> Get(Client buyer)
        {
            var dto = await _context
                .Orders
                .Include(x => x.Lines)
                .ThenInclude(l => l.Product)
                .Where(x => x.BuyerId == buyer.Id)
                .ToListAsync();
            return dto.Select(x => x.ToOrder()).ToList();
        }

        public void Update(Order order)
        {
            var dto = new OrderDto();
            dto.Id = order.Id.Value;
            dto.Lines = order.Lines.Select(l => new OrderLineDto()
            {
                Order = dto,
                Quantity = (int)l.Quantity,
                Product = new ProductDto(l.Product, null)
            })
                .ToList();
            _context.Orders.Update(dto);
        }

        private async Task<List<Order>> GetWithStatus(Client buyer, OrderStatus status)
        {
            var dto = await _context
                .Orders
                .Include(x => x.Lines)
                .Where(x => x.BuyerId == buyer.Id)
                .Where(x => x.Status == status)
                .ToListAsync();
            return dto.Select(x => x.ToOrder()).ToList();
        }
    }
}
