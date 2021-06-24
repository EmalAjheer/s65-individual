using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace user_service.Producers
{
    public class KafkaProducer
    {
        private readonly ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        private void SendToMessagebus(string topic, Guid id)
        {
            //is het nodig om elke keer een producer aan te maken? bij de consumer maak je het maar 1 keer aan. 
            //kijk hier nog even naar.
            using (var producer =
                new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var test = producer.ProduceAsync(topic, new Message<Null, string> { Value = id.ToString() })
                        .GetAwaiter()
                        .GetResult();
                    Console.WriteLine(test + "users deletion");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Oops, something went horribly wrong: {e}");
                }
            }
        }

        public void DeleteAllTweets(string topic, Guid id)
        {
            SendToMessagebus(topic, id);
        }
    }
}
