using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay.Recorrencia.Gestao.Domain.Entities
{
    public class TemplateMensagem
    {
        public string idMensagem { get; set; }

        public string? txTemplate { get; set; }

        public DateTime dataUltimaAtualizacao { get; set; }
    }
}
