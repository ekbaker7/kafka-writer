using Confluent.Kafka;

namespace KafkaWriter.Config
{
    public class KafkaConsumerConfig
    {
        public string TopicName { get; set; } = string.Empty;
        public string BootstrapServers { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public SecurityProtocol SecurityProtocol { get; set; }
        public SaslMechanism Mechanism { get; set; }
        public AutoOffsetReset AutoOffsetReset { get; set; }
        public bool EnableAutoCommit { get; set; }
        public int? MaxMessageBytes { get; set; } 
    }
}
