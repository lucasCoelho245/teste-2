namespace Pay.Recorrencia.Gestao.Producer.Models
{
    public class InputParametersKafkaProducer
    {
        /// <summary>
        /// Activate the Kafka Producer
        /// </summary>
        public bool Ativar { get; set; } = false;

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
        /// Security Parameters that will be used by the Producer
        /// </summary>
        public InputSecurityParametersKafka? SecurityParameters { get; set; }
        
        
        
        public string? TopicControleJornada { get; set; }
    
    
    }

    public class InputParameterskafkaProducer
    {
        /// <summary>
        /// Name of the Topic that should be used to produce the message
        /// </summary>
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
        
        
        
        public string? TopicControleJornada { get; set; }
    
    
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
