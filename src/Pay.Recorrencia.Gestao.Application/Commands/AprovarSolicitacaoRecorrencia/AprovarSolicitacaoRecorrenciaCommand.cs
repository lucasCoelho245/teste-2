using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Enums;

namespace Pay.Recorrencia.Gestao.Application.Commands.AprovarSolicitacaoRecorrencia
{
    public class AprovarSolicitacaoRecorrenciaCommand : IRequest<MensagemPadraoResponse>
    {
        [Required]
        public string IdSolicRecorrencia { get; set; }

        [Required]
        public string IdRecorrencia { get; set; }

        [Required]
        [RegularExpression("RCUR", ErrorMessage = "O valor de TipoRecorrencia deve ser 'RCUR'.")]
        public string TipoRecorrencia { get; set; }

        [Required]
        [RegularExpression("MIAN|MNTH|QURT|WEEK|YEAR", ErrorMessage = "O valor de TipoFrequencia deve ser um dos seguintes: MIAN, MNTH, QURT, WEEK, YEAR.")]
        public string TipoFrequencia { get; set; }

        [Required]
        public DateTime DataInicialRecorrencia { get; set; }

        public DateTime? DataFinalRecorrencia { get; set; }

        //[Required]
        //[RegularExpression("PNDG|CCLD|CFDB", ErrorMessage = "O valor de SituacaoSolicRecorrencia deve ser um dos seguintes: PNDG, CCLD, CFDB.")]
        //public string? SituacaoSolicRecorrencia { get; set; }

        //[RegularExpression("BRL", ErrorMessage = "O valor de CodigoMoedaSolicRecorr deve ser 'BRL'.")]
        public string? CodigoMoedaSolicRecorr { get; set; }

        public decimal? ValorFixoSolicRecorrencia { get; set; }

        public bool? IndicadorValorMin { get; set; }

        public decimal? ValorMinRecebedorSolicRecorr { get; set; }

        [Required]
        public string NomeUsuarioRecebedor { get; set; }

        [Required]
        public string CpfCnpjUsuarioRecebedor { get; set; }

        [Required]
        public string ParticipanteDoUsuarioRecebedor { get; set; }

        [Required]
        public string CpfCnpjUsuarioPagador { get; set; }

        [Required]
        public int ContaUsuarioPagador { get; set; }

        public int? AgenciaUsuarioPagador { get; set; }

        public string ParticipanteDoUsuarioPagador { get; set; }

        public string? NomeDevedor { get; set; }

        public string? CpfCnpjDevedor { get; set; }

        [Required]
        public string NumeroContrato { get; set; }

        public string? DescObjetoContrato { get; set; }

        [Required]
        public DateTime DataHoraCriacaoRecorr { get; set; }

        [Required]
        public DateTime DataHoraCriacaoSolicRecorr { get; set; }

        [Required]
        public DateTime DataHoraExpiracaoSolicRecorr { get; set; }

        [Required]
        public DateTime DataUltimaAtualizacao { get; set; }

        public decimal? ValorMaximoAutorizado { get; set; }

        public TipoJornada TpJornada { get; set; }



        public class AprovarSolicRecBanco : AprovarSolicitacaoRecorrenciaCommand
        {
            public string IdAutorizacao { get; set; }

            public string SituacaoSolicRecorrencia { get;set; }

            public string IdRecorrencia { get;set; }

        }


    }
}
