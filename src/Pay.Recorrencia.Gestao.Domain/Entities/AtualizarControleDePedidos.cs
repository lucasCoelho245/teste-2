using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay.Recorrencia.Gestao.Domain.Entities
{
    public class AtualizarControleDePedidos
    {
        public string tpJornada { get; set; }
        public string idRecorrecina { get; set; }
        public string situacaoJornada { get; set; }
        public string dataUltimaAtualizacao { get; set; }
    }
}
