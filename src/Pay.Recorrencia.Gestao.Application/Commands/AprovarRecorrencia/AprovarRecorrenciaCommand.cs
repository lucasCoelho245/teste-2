using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Pay.Recorrencia.Gestao.Application.Commands.IncluirAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Enums;

namespace Pay.Recorrencia.Gestao.Application.Commands.AprovarRecorrencia
{
    public class AprovarRecorrenciaCommand : IRequest<MensagemPadraoResponse>
    {
        [Required]
        public string IdRecorrencia { get; set; }

        [Required]
        [RegularExpression("RCUR", ErrorMessage = "O valor de TipoRecorrencia deve ser 'RCUR'.")]
        public string TipoRecorrencia { get; set; }

        [Required]
        public string TipoFrequencia { get; set; }

        [Required]
        public DateTime DataInicialRecorrencia { get; set; }

        public DateTime? DataFinalRecorrencia { get; set; }

        [RegularExpression("BRL", ErrorMessage = "O valor de CodigoMoedaAutorizacaoRecorrencia deve ser 'BRL'.")]
        public string? CodigoMoedaSolicRecorr { get; set; } = "BRL";

        public decimal? ValorFixoSolicRecorrencia { get; set; }

        public bool? IndicadorValorMin { get; set; } // AJUSTAR

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

        [Required]
        public string ParticipanteDoUsuarioPagador { get; set; }

        public string? NomeDevedor { get; set; }

        public string? CpfCnpjDevedor { get; set; }

        [Required]
        public string NumeroContrato { get; set; }

        public string? DescObjetoContrato { get; set; }

        [Required]
        [RegularExpression("NAO_PERMITE|PERMITE_3R_7D", ErrorMessage = "TpRetentativa deve ser 'NAO_PERMITE' ou 'PERMITE_3R_7D'.")]
        public string TpRetentativa { get; set; }

        [Required]
        public DateTime DataHoraCriacaoRecorr { get; set; }

        [Required]
        public DateTime DataUltimaAtualizacao { get; set; }

        public decimal? ValorMaximoAutorizado { get; set; }

        [Required]
        [Range(1, 4, ErrorMessage = "TpJornada deve estar entre 1 e 4.")]
        public TipoJornada TpJornada { get; set; }
    
        public string? IdSolicRecorrencia { get; set; }
    }

    public class IncluirAutorizaCaoRecorrBanco : AprovarRecorrenciaCommand
    {
        [Required] // DEVIDO A CHAMADA DA HIST-015
        public string SituacaoRecorrencia { get; set; }

        [Required]// DEVIDO A CHAMADA DA HIST-015
        public bool FlagValorMaximoAutorizado { get; set; }

        [Required]// DEVIDO A CHAMADA DA HIST-015
        public bool FlagPermiteNotificacao { get; set; }
    }
}
