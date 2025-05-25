using Pay.Recorrencia.Gestao.Domain.DTO;

namespace Pay.Recorrencia.Gestao.Domain.Entities;

public class Jornada
{
    public string?   TpJornada                { get; set; }
    public string?   IdRecorrencia            { get; set; }
    public string?   IdE2E                    { get; set; }
    public string?   IdConciliacaoRecebedor   { get; set; }
    public string?   SituacaoJornada          { get; set; }
    public DateTime? DtAgendamento           { get; set; }
    public decimal?  VlAgendamento           { get; set; }
    public DateTime? DtPagamento             { get; set; }
    public DateTime? DataHoraCriacao         { get; set; }
    public DateTime? DataUltimaAtualizacao   { get; set; }
}

public class JornadaNonPagination
{
    public Jornada? Data { get; set; }
}

public class JornadaList
{
    public string? TpJornada { get; set; }
    public string? IdRecorrencia { get; set; }
    public string? IdE2E { get; set; }
    public string? IdConciliacaoRecebedor { get; set; }
}
public class ListaJornadaPaginada<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalItems { get; set; }
}