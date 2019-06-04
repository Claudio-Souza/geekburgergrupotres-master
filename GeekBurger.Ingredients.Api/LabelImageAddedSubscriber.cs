using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GeekBurger.Ingredients.DomainModel;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace GeekBurger.Ingredients.Api
{
    public class LabelImageAddedSubscriber
    {
        private IQueueClient _queue;

        public LabelImageAddedSubscriber(IQueueClient queue)
        {
            _queue = queue;

            var messageHandlerOptions = new MessageHandlerOptions(this.ExceptionReceivedHandler)
            {
                AutoComplete = false,
                MaxConcurrentCalls = 3
            };

            _queue.RegisterMessageHandler(this.ReceivedMessage, messageHandlerOptions);
        }

        private async Task ReceivedMessage(Message message, CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(() =>
            {
                var content = Encoding.UTF8.GetString(message.Body);

                var labelImageAddedMessage = JsonConvert.DeserializeObject<LabelImageAddedMessage>(content);
                var x = 1;
            });
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            throw new NotImplementedException();
        }
    }
}
