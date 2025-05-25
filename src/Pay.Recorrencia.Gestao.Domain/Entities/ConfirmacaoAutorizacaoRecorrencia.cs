using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pay.Recorrencia.Gestao.Domain.Enums;

namespace Pay.Recorrencia.Gestao.Domain.Entities
{
    public class ConfirmacaoAutorizacaoRecorrencia
    {
        public bool Status { get; set; }

        public string IdRecorrencia { get; set; }

        public string IdInformacaoStatus { get; set; }

        public string TipoFrequencia { get; set; }

        public string DataInicialRecorrencia { get; set; }

        public string? DataFinalRecorrencia { get; set; }

        public decimal ValorFixoSolicRecorrencia { get; set; }

        public string NomeUsuarioRecebedor { get; set; }

        public string CpfCnpjUsuarioRecebedor { get; set; }

        public string ParticipanteDoUsuarioRecebedor { get; set; }

        public string CpfCnpjUsuarioPagador { get; set; }

        public string ContaUsuarioPagador { get; set; }

        public string AgenciaUsuarioPagador { get; set; }

        public string ParticipanteDoUsuarioPagador { get; set; }

        public string CodMunIBGE { get; set; }

        public string NomeDevedor { get; set; }

        public string CpfCnpjDevedor { get; set; }

        public TipoJornada TpJornada { get; set; }

        public string SituacaoRecorrencia { get; set; }

        public string NumeroContrato { get; set; }

        public string DescObjetoContrato { get; set; }

        public string DataHoraCriacaoRecorr { get; set; }

        public string DataUltimaAtualizacao { get; set; }

        public void MapearTipoFrequencia(string codigo)
        {
            TipoFrequencia = codigo switch
            {
                "WEEK" => "SEMANAL",
                "MNTH" => "MENSAL",
                "QURT" => "TRIMESTRAL",
                "MIAN" => "SEMESTRAL",
                "YEAR" => "ANUAL",
                _ => "INDEFINIDO"
            };
        }
    }
}
