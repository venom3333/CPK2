using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.Spa.Client.Core.Models;
using CPK.Spa.Client.Core.Repositories;

namespace CPK.Spa.Client.Core.Services
{
    public sealed class OrderService : IOrderService
    {
        private readonly IApiRepository _api;
        private List<OrderModel> _orders;
        public OrderService(IApiRepository api)
        {
            _api = api;
            _orders = new List<OrderModel>();
        }

        public string Error { get; private set; }
        public IReadOnlyList<OrderModel> Orders => _orders?.AsReadOnly();

        public async Task Create(IEnumerable<LineModel> lines, string address)
        {
            var (_, e) = await _api.CreateOrder(lines, address);
            Error = e;
            if (!string.IsNullOrWhiteSpace(e))
                return;
            await Load();
        }

        public async Task Load()
        {
            var (r, e) = await _api.GetOrders();
            _orders = r;
            Error = e;
        }
    }
}