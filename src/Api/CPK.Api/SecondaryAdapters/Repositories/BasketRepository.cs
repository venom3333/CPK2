using System.Linq;
using System.Threading.Tasks;
using CPK.Api.SecondaryAdapters.Dto;
using CPK.BasketModule.Entities;
using CPK.BasketModule.SecondaryPorts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CPK.Api.SecondaryAdapters.Repositories
{
    internal sealed class BasketRepository : IBasketRepository
    {
        private readonly CpkContext _context;
        private readonly IConfig _config;
        private readonly IHttpContextAccessor _accessor;

        public BasketRepository(CpkContext context, IConfig config, IHttpContextAccessor accessor)
        {
            _context = context;
            _config = config;
            _accessor = accessor;
        }

        public async Task<Basket> Get(BasketId id)
        {
            var basket = await _context.Baskets.Include(b => b.Lines).ThenInclude(l => l.Product).FirstOrDefaultAsync(b => b.Id == id.Value);
            return basket?.ToBasket(_config.MaxBasketSize);
        }

        public async Task Save(Basket basket)
        {
            var dto = await _context.Baskets.Include(b => b.Lines)
                .FirstOrDefaultAsync(b => b.Id == basket.Id.Value);
            if (dto == null)
            {
                dto = new BasketDto(basket);
                _context.Baskets.Add(dto);
            }
            else
            {
                dto.Lines = basket.Lines.Select(l => new BasketLineDto(l, dto)).ToList();
                _context.Baskets.Update(dto);
            }
        }
    }
}
