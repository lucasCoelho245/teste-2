namespace Pay.Recorrencia.Gestao.Domain.DTO
{
    public class EnviarPushDTO
    {
        public string? titlePush { get; set; }
        public string? messagePush { get; set; }
        public string[]? destinatarios { get; set; }
    }
}
