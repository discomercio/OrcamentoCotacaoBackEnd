using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Request.LoginHistorico
{
    public class LoginHistoricoRequest: UtilsGlobais.RequestResponse.RequestBase
    {
        public int IdUsuario { get; set; }
        public int SistemaResponsavel { get; set; }
    }
}
