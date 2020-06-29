using System.Collections.Generic;
using System.Linq;
using CPK.BasketModule.Entities;

namespace CPK.Api.SecondaryAdapters.Dto
{
    public sealed class BasketDto : EntityDto<string>
    {
        public List<BasketLineDto> Lines { get; set; }

        public BasketDto()
        {
            //For EF
            Lines = new List<BasketLineDto>();
        }

        public BasketDto(Basket basket)
        {
            Id = basket.Id.Value;
            Lines = basket.Lines
                .Select(l => new BasketLineDto(l, this))
                .ToList();
        }

        public Basket ToBasket(int maxSize) => new Basket(new BasketId(Id), BasketState.Create(Lines.Select(l => l.ToLine()), maxSize));
    }
}
