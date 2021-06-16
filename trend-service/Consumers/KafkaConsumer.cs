using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using trend_service.Interfaces;
using trend_service.Models;

namespace trend_service.Consumers
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory factory;

        public KafkaConsumer(IServiceScopeFactory factory)
        {
            this.factory = factory;
        }

       /* public Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = factory.CreateScope();
            var trendService = scope.ServiceProvider.GetRequiredService<ITrendService>();

            var conf = new ConsumerConfig
            {
                GroupId = "st_consumer_group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using (var builder = new ConsumerBuilder<Ignore,
                string>(conf).Build())
            {
                builder.Subscribe("trend_topic");
                var cancelToken = new CancellationTokenSource();
                try
                {
                    while (true)
                    {
                        var consumer = builder.Consume(cancelToken.Token);
                        var hashtag = JsonSerializer.Deserialize<Hashtag>(consumer.Message.Value);
                        Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");

                        trendService.PostHashtag(hashtag);
                    }
                }
                catch (Exception)
                {
                    builder.Close();
                }
            }
            return Task.CompletedTask;
        }*/

        /*public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }*/

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = factory.CreateScope();
            var trendService = scope.ServiceProvider.GetRequiredService<ITrendService>();

            var conf = new ConsumerConfig
            {
                GroupId = "st_consumer_group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            await Task.Run(() =>
            {
                using (var builder = new ConsumerBuilder<Ignore, string>(conf).Build())
                {
                    builder.Subscribe("trend_topic");

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var consumer = builder.Consume(stoppingToken);
                        var hashtag = JsonSerializer.Deserialize<Hashtag>(consumer.Message.Value);
                        Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");

                        trendService.PostHashtag(hashtag);
                    }
                    builder.Close();
                }
            });
        }
    }
}
