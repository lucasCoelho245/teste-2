﻿namespace Pay.Recorrencia.Gestao.Consumer.Models
{
    public class InputParametersKafkaConsumer
    {
        /// <summary>
        /// Activate the Kafka Producer
        /// </summary>
        public bool ActivateKafka { get; set; } = false;

        /// <summary>
        /// Server that will be used to connect to Kafka
        /// </summary>
        public string? BootstrapServers { get; set; }

        /// <summary>
        /// Message Max Bytes
        /// </summary>
        public int? MessageMaxBytes { get; set; }

        /// <summary>
        /// Parameters that will be used by the Producer
        /// </summary>
        public InputParameterskafkaProducer? Producer { get; set; }

        /// <summary>
        /// Parameters that will be used by the Consumer
        /// </summary>
        public InputParameterskafkaConsumer? Consumer { get; set; }

        /// <summary>
        /// Security Parameters that will be used by the Producer/Consumer
        /// </summary>
        public InputSecurityParametersKafka? SecurityParameters { get; set; }
    }

    public class InputParameterskafkaProducer
    {       
        public string? TopicControleJornada { get; set; }

        public string? TopicInclusaoAutorizacaoRecorrencia { get; set; }

        public string? Topic { get; set; }

        /// <summary>
        /// Time in seconds that the producer will wait before flushing the messages
        /// </summary>
        public int ProducerFlushTime { get; set; } = 30;

        /// <summary>
        /// Defines the level of confirmation that Kafka received the message. Possible values: -1 = All, 1 = Only one confirmation, 0 no confirmation
        /// </summary>
        public Confluent.Kafka.Acks Acks { get; set; } = Confluent.Kafka.Acks.All;

        /// <summary>
        /// Name of the Application that is using the library
        /// </summary>
        public required string ApplicationName { get; set; }
    }

    public class InputParameterskafkaConsumer
    {
        
        public bool ActivateConsumer { get; set; }
        public string? GroupId { get; set; }
        public bool AutoOffsetResetEarliest { get; set; }
        public int ConsumerProccessMaxMS { get; set; }
        public bool EnableAutoCommit { get; set; }
        public string? TopicTo { get; set; }
        public int? HeartbeatIntervalMs { get; set; }
        public int? SessionTimeoutMs { get; set; }
        public int? MaxPollIntervalMs { get; set; }
        public string? Debug { get; set; }
        public List<KafkaConsumerMapping>? KafkaConsumerMappings { get; set; }
    }

    public class KafkaConsumerMapping
    {
        public string? Topic { get; set; }
        public string? ConsumerType { get; set; }
    }

    public class InputSecurityParametersKafka
    {
        /// <summary>
        /// Activate the SSL
        /// </summary>
        public bool ActivateSsl { get; set; }

        /// <summary>
        /// SASL User Name
        /// </summary>
        public string SaslUsername { get; set; }

        /// <summary>
        /// SASL Password
        /// </summary>
        public string SaslPassword { get; set; }

        /// <summary>
        /// Certificate Path
        /// </summary>
        public string CertifiedPath { get; set; }
    }
}
