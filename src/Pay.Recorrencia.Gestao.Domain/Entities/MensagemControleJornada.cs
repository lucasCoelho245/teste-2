namespace Pay.Recorrencia.Gestao.Domain.Entities
{
    public class MensagemControleJornada : ControleJornadaEntrada
    {
        public string? CodigoDeErro { get; set; }
        public DateTime DtErro { get; set; } = DateTime.UtcNow;
    }
}
