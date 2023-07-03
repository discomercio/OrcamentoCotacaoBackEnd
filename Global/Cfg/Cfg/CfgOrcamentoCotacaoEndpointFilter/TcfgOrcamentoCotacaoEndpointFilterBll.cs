using Cfg.CfgOrcamentoCotacaoEmailTemplate;
using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cfg.CfgOrcamentoCotacaoEndpointFilter
{
    public class TcfgOrcamentoCotacaoEndpointFilterBll : BaseBLL<TcfgOrcamentoCotacaoEndpointFilter, TcfgOrcamentoCotacaoEndpointFilterFiltro>
    {
        private TcfgOrcamentoCotacaoEndpointFilterData _data { get; set; }

        public TcfgOrcamentoCotacaoEndpointFilterBll(ContextoBdProvider contextoBdProvider) : base(new TcfgOrcamentoCotacaoEndpointFilterData(contextoBdProvider))
        {
            _data = new TcfgOrcamentoCotacaoEndpointFilterData(contextoBdProvider);
        }
    }
}
