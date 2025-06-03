using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay.Recorrencia.Gestao.Domain.Events
{
    public class NotificarClienteSolicitadoEvent
    {
        public string idMessagem { get; set; }
        public string cpfCnpjUsuarioPagador { get; set; }
        public string txConteudo { get; set; }

    }
}