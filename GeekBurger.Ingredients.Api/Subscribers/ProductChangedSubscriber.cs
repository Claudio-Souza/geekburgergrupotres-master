using AutoMapper;
using GeekBurger.Ingredients.DataLayer;
using Microsoft.Azure.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Api.Subscribers
{
    public class ProductChangedSubscriber
    {
        private readonly IMapper _mapper;
        private readonly ISubscriptionClient _subscriptionClient;
        private readonly IUnitOfWork _unitOfWork;

        public ProductChangedSubscriber(IMapper mapper, ISubscriptionClient subscriptionClient, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

            var messageHandlerOptions = new MessageHandlerOptions(this.ExceptionReceivedHandler)
            {
                AutoComplete = false,
                MaxConcurrentCalls = 3
            };

            subscriptionClient.RegisterMessageHandler(this.ReceivedMessage, messageHandlerOptions);

            _subscriptionClient = subscriptionClient;
        }

        private Task ReceivedMessage(Message message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            await _unitOfWork.LogRepository.SaveAsync(arg.Exception.ToString());
        }
    }
}
