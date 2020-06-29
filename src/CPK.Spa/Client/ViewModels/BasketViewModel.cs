using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.Spa.Client.Core.Models;
using CPK.Spa.Client.Core.Services;

namespace CPK.Spa.Client.ViewModels
{
    public class BasketViewModel
    {
        private readonly IBasketService _service;

        public BasketViewModel(IBasketService service)
        {
            _service = service;
        }

        public string Error => _service.Error;
        public IReadOnlyList<LineModel> Model => _service.Model;

        public async Task OnInitializedAsync()
        {
            await _service.Load();
        }

        public Task Add(ProductModel product) => _service.Add(product);
        public Task Remove(ProductModel product) => _service.Remove(product);
        public Task Clear() => _service.Clear();
    }
}
