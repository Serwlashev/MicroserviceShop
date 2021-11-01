using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpate = await _orderRepository.GetByIdAsync(request.Id);
            if(orderToUpate is null)
            {
                _logger.LogError("Order not exist in database");
                throw NotFoundException(nameof(Order), request.Id);
            }

            _mapper.Map(request, orderToUpate, typeof(UpdateOrderCommand), typeof(Order));

            await _orderRepository.UpdateAsync(orderToUpate);

            _logger.LogInformation($"Order {orderToUpate.Id} is successfully updated.");

            return Unit.Value;
        }

        private Exception NotFoundException(string v, int id)
        {
            throw new NotImplementedException();
        }
    }
}
