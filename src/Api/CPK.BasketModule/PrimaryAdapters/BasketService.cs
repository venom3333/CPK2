using System;
using System.Threading.Tasks;
using CPK.BasketModule.Entities;
using CPK.BasketModule.PrimaryPorts;
using CPK.BasketModule.SecondaryPorts;
using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.BasketModule.PrimaryAdapters
{
    public sealed class BasketService : IBasketService
    {
        private readonly IBasketUow _uow;
        private readonly IConfigRepository _config;
        private readonly IBasketRepository _repository;

        public BasketService(IBasketUow uow, IConfigRepository config, IBasketRepository repository)
        {
            _uow = uow;
            _config = config;
            _repository = repository;
        }

        public async Task<int> AddProduct(BasketId id, BasketProduct basketProduct)
        {
            Validator.Begin(basketProduct, nameof(basketProduct))
                .NotDefault()
                .ThrowApiException(nameof(BasketService), nameof(AddProduct));
            var basket = await _repository.Get(id);
            if (basket == null)
            {
                basket = new Basket(id, new EmptyBasketState(_config.GetMaxBasketSize()));
            }
            basket.Add(basketProduct);
            await _repository.Save(basket);
            return await _uow.SaveAsync();
        }

        public async Task<int> Clear(BasketId id)
        {
            if (id.Equals(default))
                throw new ArgumentNullException(nameof(id));

            var basket = await _repository.Get(id);
            if (basket == null)
                basket = new Basket(id, new EmptyBasketState(_config.GetMaxBasketSize()));
            else
                basket.Clear();
            await _repository.Save(basket);
            return await _uow.SaveAsync();
        }

        public async Task<Basket> Get(BasketId id)
        {
            var basket = await _repository.Get(id);
            if (basket == null)
            {
                basket = new Basket(id, new EmptyBasketState(_config.GetMaxBasketSize()));
                await _repository.Save(basket);
                await _uow.SaveAsync();
            }
            return basket;
        }



        public async Task<int> RemoveProduct(BasketId id, Guid basketId)
        {
            Validator.Begin(basketId, nameof(basketId))
                .NotDefault()
                .Map(id, nameof(id))
                .NotDefault()
                .ThrowApiException(nameof(BasketService), nameof(RemoveProduct));
            var basket = await _repository.Get(id);
            if (basket == null)
                return 0;
            else
                basket.Remove(basketId);
            await _repository.Save(basket);
            return await _uow.SaveAsync();
        }
    }
}
