using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPK.Spa.Client.Core.Models;
using CPK.Spa.Client.Core.Repositories;

namespace CPK.Spa.Client.Core.Services
{
    public class BasketService : IBasketService
    {
        private readonly IApiRepository _repository;
        private BasketModel _basket;

        public BasketService(IApiRepository repository)
        {
            _repository = repository;
            _basket = new BasketModel();
        }

        public string Error { get; private set; }
        public IReadOnlyList<LineModel> Model => _basket?.Lines?.AsReadOnly();
        public event EventHandler OnBasketItemsCountChanged;
        public long ItemsCount => _basket?.Lines?.Sum(l => l.Quantity) ?? 0;

        public async Task Load()
        {
            var count = ItemsCount;
            var (r, e) = await _repository.GetBasket();
            _basket = r;
            Error = e;
            if (string.IsNullOrWhiteSpace(Error) && count != ItemsCount)
                OnBasketItemsCountChanged?.Invoke(null, null);
        }

        public async Task Add(ProductModel product)
        {
            var (_, e) = await _repository.AddToBasket(product);
            Error = e;
            if (!string.IsNullOrWhiteSpace(e))
                return;
            await Load();
        }

        public async Task Remove(ProductModel product)
        {
            var (_, e) = await _repository.RemoveProduct(product.Id);
            Error = e;
            if (!string.IsNullOrWhiteSpace(e))
                return;
            await Load();
        }

        public async Task Clear()
        {
            var (_, e) = await _repository.ClearBasket();
            Error = e;
            if (!string.IsNullOrWhiteSpace(e))
                return;
            await Load();
        }
    }
}
