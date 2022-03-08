using InfraBanco.Constantes;
using Loja;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class LojaOrcamentoCotacaoBll
    {
        private readonly LojaBll _lojaBll;

        public LojaOrcamentoCotacaoBll(LojaBll lojaBll)
        {
            _lojaBll = lojaBll;
        }

        public PercMaxDescEComissaoResponseViewModel BuscarPercMaxPorLoja(string loja, int? tipoUsuario)
        {
            if (Constantes.TipoUsuarioPerfil.getUsuarioPerfil((Constantes.TipoUsuario)tipoUsuario) == Constantes.eTipoUsuarioPerfil.INDICADOR_PARCEIRO)
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
            }

            return null;
        }
    }
}
