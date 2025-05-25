using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay.Recorrencia.Gestao.Domain.Entities
{
    public class DetalheDadosCobranca
    {
        public string IdOperacao { get; set; }
        public string IdRecorrencia { get; set; }
        public decimal VlOperacao { get; set; }
        public DateTime DtPagto { get; set; }
        public string NomeUsuarioRecebedor { get; set; }
        public string CpfCnpjUsuarioRecebedor { get; set; }
        public string ParticipanteDoUsuarioRecebedor { get; set; }
        public string CpfCnpjUsuarioPagador { get; set; }
        public string NumeroContrato { get; set; }
        public string DescObjetoContrato { get; set; }
    }
}
