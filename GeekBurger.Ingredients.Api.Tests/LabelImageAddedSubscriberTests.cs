using AutoFixture;
using AutoMapper;
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
        private Fixture _fixture;
        private IQueueClient _queue;
        private IMapper _mapper;
        private ServiceBusSettings _serviceBusSettings;
        private IUnitOfWork _unitOfWork;

        public LabelImageAddedSubscriberTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = config.CreateMapper();

            _serviceBusSettings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection(nameof(ServiceBusSettings))
                .Get<ServiceBusSettings>();

            _queue = Substitute.For<IQueueClient>();


            _unitOfWork = Substitute.For<IUnitOfWork>();

            _fixture = new Fixture();

        }

        [Fact]
        public async Task Upon_label_image_added_message_received_should_save_it_on_product_repository()
        {
            //Arrange
            Func<Message, CancellationToken, Task> call = null;

            _queue.When(q => q.RegisterMessageHandler(Arg.Any<Func<Message, CancellationToken, Task>>(), Arg.Any<MessageHandlerOptions>()))
                .Do(c => call = c.Arg<Func<Message, CancellationToken, Task>>());


            var labelImageAddedListener = new LabelImageAddedSubscriber(_mapper, _queue, _unitOfWork);

            var messageObject = _fixture.Create<LabelImageAddedMessage>();
            var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageObject));

            //Act
            await call(new Message(messageBody), new CancellationToken());

            //Assert
            await _unitOfWork.ProductRepository.Received().SaveAsync(Arg.Any<Product>());
        }
    }
}
