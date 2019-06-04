using AutoFixture;
using GeekBurger.Ingredients.DataLayer;
using GeekBurger.Ingredients.DomainModel;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NSubstitute;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GeekBurger.Ingredients.Api.Tests
{
    public class LabelImageAddedSubscriberTests
    {
        [Fact]
        public void new_test()
        {
            //Arrange
            var serviceBusSettings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection(nameof(ServiceBusSettings))
                .Get<ServiceBusSettings>();

            var queue = Substitute.For<IQueueClient>();


            var unitOfWork = Substitute.For<IUnitOfWork>();


            Func<Message, CancellationToken, Task> call = null;

            queue.When(q => q.RegisterMessageHandler(Arg.Any<Func<Message, CancellationToken, Task>>(), Arg.Any<MessageHandlerOptions>()))
                .Do(c => call = c.Arg<Func<Message, CancellationToken, Task>>());


            var labelImageAddedListener = new LabelImageAddedSubscriber(queue);

            var fixture = new Fixture();
            var messageObject = fixture.Create<LabelImageAddedMessage>();
            var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageObject));

            //Act
            call(new Message(messageBody), new CancellationToken());

            //Assert
            unitOfWork.ProductRepository.Received().Save(Arg.Any<Product>());
        }
    }
}
