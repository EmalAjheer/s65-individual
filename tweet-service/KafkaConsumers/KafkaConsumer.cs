using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using tweet_service.Interfaces;
using tweet_service.Services;

namespace tweet_service.KafkaConsumers
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory factory;

        public KafkaConsumer(IServiceScopeFactory factory)
        {
            this.factory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = factory.CreateScope();
            var tweetService = scope.ServiceProvider.GetRequiredService<ITweetService>();

            var conf = new ConsumerConfig
            {
                GroupId = "st_consumer_group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            await Task.Run(() =>
            {
                //moet je per se elke keer een nieuwe builder maken?
                using (var builder = new ConsumerBuilder<Ignore, string>(conf).Build())
                {
                    builder.Subscribe("delete_user_topic");

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var consumer = builder.Consume(stoppingToken);
                        var id = consumer.Message.Value;
                        Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");

                        // await tweetService.DeleteAllTweets(id);

                        tweetService.DeleteAllTweets(Guid.Parse(id));
                    }
                    builder.Close();
                }
            });
        }
    }
}
