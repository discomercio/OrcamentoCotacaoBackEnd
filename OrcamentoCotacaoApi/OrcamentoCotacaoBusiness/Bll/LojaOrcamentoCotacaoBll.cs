using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Loja;
using Loja.Dto;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento;
using System;
using System.Collections.Generic;
using System.Linq;
using static OrcamentoCotacaoBusiness.Enums.Enums;

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

            if (retorno != null)
            {
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

        public PercMaxDescEComissaoResponseViewModel BuscarPercMaxPorLojaAlcada(string loja, string tipoCliente, List<string> lstPermissoes)
        {
            var tLojas = PorFiltro(new TlojaFiltro() { Loja = loja });
            if (tLojas.Count == 0) throw new ArgumentException("Falha ao buscar os percentuais de desconto e comissão!");

            var tLoja = tLojas.FirstOrDefault();
            List<PercMaxDescEComissaoResponseViewModel> lstAlcadas = new List<PercMaxDescEComissaoResponseViewModel>();

            PercMaxDescEComissaoResponseViewModel retorno = new PercMaxDescEComissaoResponseViewModel();

            var permissoesAlcadas = new List<string>()
            {
                Constantes.COMISSAO_DESCONTO_ALCADA_1,
                Constantes.COMISSAO_DESCONTO_ALCADA_2,
                Constantes.COMISSAO_DESCONTO_ALCADA_3
            };

            var permissao2 = lstPermissoes.Where(x => permissoesAlcadas.Contains(x)).ToList();

            var maiorAlcada = permissao2.Max();

            if (maiorAlcada == null) return null;

            if (maiorAlcada == Constantes.COMISSAO_DESCONTO_ALCADA_1)
            {
                if (tipoCliente == Constantes.ID_PF)
                {
                    retorno.PercMaxComissao = tLoja.Perc_max_comissao_alcada1;
                    retorno.PercMaxComissaoEDesconto = tLoja.Perc_max_comissao_e_desconto_alcada1_pf;
                }
                if (tipoCliente == Constantes.ID_PJ)
                {
                    retorno.PercMaxComissao = tLoja.Perc_max_comissao_alcada1;
                    retorno.PercMaxComissaoEDesconto = tLoja.Perc_max_comissao_e_desconto_alcada1_pj;
                }

            }
            if (maiorAlcada == Constantes.COMISSAO_DESCONTO_ALCADA_2)
            {
                if (tipoCliente == Constantes.ID_PF)
                {
                    retorno.PercMaxComissao = tLoja.Perc_max_comissao_alcada2;
                    retorno.PercMaxComissaoEDesconto = tLoja.Perc_max_comissao_e_desconto_alcada2_pf;
                }
                if (tipoCliente == Constantes.ID_PJ)
                {
                    retorno.PercMaxComissao = tLoja.Perc_max_comissao_alcada2;
                    retorno.PercMaxComissaoEDesconto = tLoja.Perc_max_comissao_e_desconto_alcada2_pj;
                }
            }
            if (maiorAlcada == Constantes.COMISSAO_DESCONTO_ALCADA_3)
            {
                if (tipoCliente == Constantes.ID_PF)
                {
                    retorno.PercMaxComissao = tLoja.Perc_max_comissao_alcada3;
                    retorno.PercMaxComissaoEDesconto = tLoja.Perc_max_comissao_e_desconto_alcada3_pf;
                }
                if (tipoCliente == Constantes.ID_PJ)
                {
                    retorno.PercMaxComissao = tLoja.Perc_max_comissao_alcada3;
                    retorno.PercMaxComissaoEDesconto = tLoja.Perc_max_comissao_e_desconto_alcada3_pj;
                }
            }

            return retorno;
        }

        public LojaViewModel BuscarLojaEstilo(string loja)
        {
            return _lojaBll.BuscarLojaEstilo(loja);
        }
    }
}
