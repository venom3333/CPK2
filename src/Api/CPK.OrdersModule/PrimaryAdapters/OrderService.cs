using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.OrdersModule.Entities;
using CPK.OrdersModule.PrimaryPorts;
using CPK.OrdersModule.SecondaryPorts;
using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.OrdersModule.PrimaryAdapters
{
    public sealed class OrderService : IOrderService
    {
        private readonly IOrdersUow _uow;
        private readonly IOrderRepository _repository;

        public OrderService(IOrdersUow uow, IOrderRepository repository)
        {
            _uow = uow;
            _repository = repository;
        }

        public Task<List<Order>> Get(Client request)
        {
            if (request.Equals(default))
                throw new ApiException(ApiExceptionCode.BuyerNotExist);
            return _repository.Get(request);
        }

        public async Task<Order> Get(OrderId request)
        {
            var order = await _repository.Get(request);
            if (order == null)
                throw new ApiException(ApiExceptionCode.OrderNotExist);
            return order;
        }

        public async Task<OrderId> Create(Order request)
        {
            Validator.Begin(request, nameof(request))
                .NotNull()
                .ThrowApiException(nameof(OrderService), nameof(Create));
            _repository.Add(request);
            await _uow.SaveAsync();
            return request.Id;
        }

        public async Task<int> Delivered(OrderId id)
        {
            var order = await _repository.Get(id);
            if (order == null)
                throw new ApiException(ApiExceptionCode.OrderNotExist);
            order.Delivered();
            _repository.Update(order);
            return await _uow.SaveAsync();
        }
    }
}
