using Confluent.Kafka;
using KafkaWriter.Config;

namespace KafkaWriter
{
    public class Publisher
    {
        private readonly KafkaPublisherConfig _config;
        private readonly MessageConfig _messageConfig;

        public Publisher(KafkaPublisherConfig config, MessageConfig messageConfig)
        {
            _config = config;
            _messageConfig = messageConfig;
        }

        public async Task<string> WriteToKafka()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _config.BootstrapServers,
                SaslUsername = _config.UserName,
                SaslPassword = _config.Password,
                SecurityProtocol = _config.SecurityProtocol,
                SaslMechanism = _config.Mechanism,
                MessageMaxBytes = _config.MaxMessageBytes
            };

            string key = $"Kafka-Writer-Example";

            try
            {
                using var producer = new ProducerBuilder<string, string>(config).Build();

                await producer.ProduceAsync(_config.TopicName, new Message<string, string>
                {
                    Key = key,
                    Value = File.ReadAllText(_messageConfig.FilePath)
                });

                producer.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message, $"Error occurred while publishing.");
            }

            return key;
        }
    }
}
