using KafkaWriter.Helpers;

namespace KafkaWriter.Config
{
    public class AppSettingConstants
    {
        public static readonly string KafkaPublisherConfig = ReflectionHelpers.GetPropertyName(() => KafkaPublisherConfig);
        public static readonly string KafkaConsumerConfig = ReflectionHelpers.GetPropertyName(() => KafkaConsumerConfig);
        public static readonly string MessageConfig = ReflectionHelpers.GetPropertyName(() => MessageConfig);
    }
}