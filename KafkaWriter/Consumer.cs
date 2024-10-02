using Confluent.Kafka;
using KafkaWriter.Config;
using Newtonsoft.Json;

namespace KafkaWriter
{
    public class Consumer
    {
        private readonly KafkaConsumerConfig _config;
        private readonly MessageConfig _messageConfig;

        public Consumer(KafkaConsumerConfig config, MessageConfig messageConfig)
        {
            _config = config;
            _messageConfig = messageConfig;
        }

        public async Task ReadFromKafka(string key, CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                SaslUsername = _config.UserName,
                SaslPassword = _config.Password,
                SaslMechanism = _config.Mechanism,
                GroupId = _config.GroupId,
                BootstrapServers = _config.BootstrapServers,
                SecurityProtocol = _config.SecurityProtocol,
                AutoOffsetReset = _config.AutoOffsetReset,
                EnableAutoCommit = _config.EnableAutoCommit
            };

            using var builder = new ConsumerBuilder<string, string>(config).Build();

            try
            {
                builder.Subscribe(_config.TopicName);
                var cancelToken = new CancellationTokenSource();
                try
                {
                    Console.WriteLine($"Start Consuming. Looking for keys '{key}' in topic '{_config.TopicName}'.");
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var consumer = builder.Consume(cancelToken.Token);
                        Console.WriteLine($"Message found at {DateTime.Now:hh:mm:ss.fff tt} with key '{consumer.Message.Key}'.");
                        try
                        {
                            if (key == consumer.Message.Key)
                            {
                                Console.WriteLine($"Kafka Message with key '{key}' found.");
                                var convertedObject =
                                    JsonConvert.DeserializeObject(consumer.Message.Value);

                                Console.WriteLine("Deserialized object:");
                                Console.WriteLine(convertedObject!.ToString());
                                continue;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error Consuming: {e.Message}");
                            continue;
                        }
                    }
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"error: {e.Error.Reason}");
                    builder.Close();
                }

                await Task.CompletedTask;
            }
            catch (OperationCanceledException)
            {
                // Ensure the consumer leaves the group cleanly and final offsets are committed.
                builder.Close();
                await Task.CompletedTask;
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
