using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GeekBurger.Ingredients.DataLayer;
using GeekBurger.Ingredients.DomainModel;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace GeekBurger.Ingredients.Api
{
    public class LabelImageAddedSubscriber
    {
        private readonly IMapper _mapper;
        private IQueueClient _queue;
        private IUnitOfWork _unitOfWork;

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
            await Task.Factory.StartNew(() =>
            {
                var content = Encoding.UTF8.GetString(message.Body);

                var labelImageAddedMessage = JsonConvert.DeserializeObject<LabelImageAddedMessage>(content);
                
                var product = _mapper.Map<Product>(labelImageAddedMessage);
                _unitOfWork.ProductRepository.SaveAsync(product);
            });
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            throw new NotImplementedException();
        }
    }
}
