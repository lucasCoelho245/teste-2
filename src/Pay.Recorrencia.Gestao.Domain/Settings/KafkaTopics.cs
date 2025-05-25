namespace Pay.Recorrencia.Gestao.Domain.Settings
{
    public class AllTopics
    {
        public string InclusaoAutorizacaoRecorrencia { get; set; }
        public string AtualizarControleJornada { get; set; }
    }
    public class KafkaTopics
    {
        public AllTopics AllTopics { get; set; }
    }
}