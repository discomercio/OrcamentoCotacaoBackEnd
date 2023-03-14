using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Response.Cfg.CfgParametro
{
    public class CfgParametroResponse
    {
        public int Id { get; set; }
        public short IdCfgDataType { get; set; }
        public string Sigla { get; set; }
        public string Descricao { get; set; }
    }
}
