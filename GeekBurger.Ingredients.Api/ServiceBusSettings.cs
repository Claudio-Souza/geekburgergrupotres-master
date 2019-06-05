namespace GeekBurger.Ingredients.Api
{
    public class ServiceBusSettings
    {
        public string SubscriptionName { get; set; }
        public string TopicName { get; set; }
        public string LabelImageAddedQueueName { get; set; }
    }
}
