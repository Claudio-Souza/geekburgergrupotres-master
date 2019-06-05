using AutoMapper;
using GeekBurger.Ingredients.DataLayer;
using GeekBurger.Ingredients.DomainModel;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Api.Subscribers
{
    public class LabelImageAddedSubscriber
    {
        private readonly IMapper _mapper;
        private readonly IQueueClient _queue;
        private readonly IUnitOfWork _unitOfWork;

        public LabelImageAddedSubscriber(IMapper mapper, IQueueClient queue, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _queue = queue;

            var messageHandlerOptions = new MessageHandlerOptions(this.ExceptionReceivedHandler)
            {
                AutoComplete = false,
                MaxConcurrentCalls = 3
            };

            _queue.RegisterMessageHandler(this.ReceivedMessage, messageHandlerOptions);

            _unitOfWork = unitOfWork;
        }

        private async Task ReceivedMessage(Message message, CancellationToken cancellationToken)
        {
            var content = Encoding.UTF8.GetString(message.Body);

            var labelImageAddedMessage = JsonConvert.DeserializeObject<LabelImageAddedMessage>(content);

            var product = _mapper.Map<Product>(labelImageAddedMessage);
            await _unitOfWork.ProductRepository.SaveAsync(product);
        }

        private async Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            await _unitOfWork.LogRepository.SaveAsync(arg.Exception.ToString());
        }

    }
}
