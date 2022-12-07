using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cfg.CfgOperacao
{
    public class CfgOperacaoBll : BaseBLL<TcfgOperacao, TcfgOperacaoFiltro>
    {
        private CfgOperacaoData _data { get; set; }

    public CfgOperacaoBll(ContextoBdProvider contextoBdProvider) : base(new CfgOperacaoData(contextoBdProvider))
    {
        _data = new CfgOperacaoData(contextoBdProvider);
    }
}
}
