namespace Pay.Recorrencia.Gestao.Domain.Settings
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }
        public string GroupId { get; set; }
        public Dictionary<string, string> Topics { get; set; }
    }
}
