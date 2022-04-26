using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Loja;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class LojaOrcamentoCotacaoBll
    {
        private readonly LojaBll _lojaBll;

        public LojaOrcamentoCotacaoBll(LojaBll lojaBll)
        {
            _lojaBll = lojaBll;
        }

        public List<Tloja> PorFiltro(TlojaFiltro obj)
        {
            return _lojaBll.PorFiltro(obj);
        }

        public PercMaxDescEComissaoResponseViewModel BuscarPercMaxPorLoja(string loja)
        {
            var retorno = _lojaBll.BuscarPercMaxPorLoja(loja);

            return new PercMaxDescEComissaoResponseViewModel
            {
                PercMaxComissao = retorno.PercMaxComissao,
                PercMaxComissaoEDesconto = retorno.PercMaxComissaoEDesconto,
                PercMaxComissaoEDescontoNivel2 = retorno.PercMaxComissaoEDescontoNivel2,
                PercMaxComissaoEDescontoNivel2PJ = retorno.PercMaxComissaoEDescontoNivel2PJ,
                PercMaxComissaoEDescontoPJ = retorno.PercMaxComissaoEDescontoPJ,
            };

            return null;
        }
    }
}
