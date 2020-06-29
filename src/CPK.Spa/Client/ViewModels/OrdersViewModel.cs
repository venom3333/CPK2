using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CPK.Spa.Client.Core.Models;
using CPK.Spa.Client.Core.Services;
using Microsoft.AspNetCore.Components.Forms;

namespace CPK.Spa.Client.ViewModels
{
    public class OrdersViewModel
    {
        private readonly IOrderService _order;
        private readonly IBasketService _basket;

        public OrdersViewModel(IOrderService order, IBasketService basket)
        {
            _order = order;
            _basket = basket;
            OrderFormContext = new EditContext(this);
        }

        public bool CanCreateOrder => _basket.ItemsCount > 0;
        public string Error => _order.Error + _basket.Error;
        public IReadOnlyList<OrderModel> Model => _order.Orders;
        public decimal Sum => _basket.Model.Sum(m => m.Quantity * m.Product.Price);
        public EditContext OrderFormContext { get; }
        public OrderVmState State { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Address { get; set; }

        public void ChangeState(string value)
        {
            State = OrderVmState.List;
            if (string.IsNullOrWhiteSpace(value))
                return;
            if (Enum.TryParse(value, true, out OrderVmState state))
                State = state;
            if (_basket.ItemsCount == 0 && State == OrderVmState.Create)
                State = OrderVmState.List;
        }

        public async Task OnInitializedAsync()
        {
            await _order.Load();
            await _basket.Load();
        }

        public async Task Create()
        {
            if (!OrderFormContext.Validate())
                return;
            //Обычно тут бы надо бы сагу сделать для микросервиса корзины и покупок
            // и глубокое клонирование сделать автомаппером
            // но для учебного проекта и так сойдет.
            await _basket.Load();
            if (!string.IsNullOrWhiteSpace(_basket.Error))
                return;
            var lines = _basket.Model.Select(l => new LineModel()
            {
                Product = new ProductModel()
                {
                    Id = l.Product.Id,
                    Price = l.Product.Price,
                    Title = l.Product.Title,
                    Version = l.Product.Version
                },
                Quantity = l.Quantity
            }).ToList();
            await _basket.Clear();
            if (!string.IsNullOrWhiteSpace(_basket.Error))
                return;
            try
            {
                //Последней потому что это самая важная операция. С остальными ничего котострафического не произойдет
                await _order.Create(lines, Address);
            }
            catch
            {
                //Компенсирующие операции чтобы востановить прежнее состояние. 
                await Restore(lines);
                throw;
            }
            if (!string.IsNullOrWhiteSpace(_order.Error))
            {
                await Restore(lines);
            }
            State = OrderVmState.List;
        }

        private async Task Restore(IEnumerable<LineModel> lines)
        {
            //Тут надо бы сделать один метод на сервере который сразу коллекцию предметов корзины
            //принимает но мне для учебного проекта лень.
            foreach (var line in lines)
            {
                for (int i = 0; i < line.Quantity; i++)
                {
                    await _basket.Add(line.Product);
                }
            }
        }
    }
}
