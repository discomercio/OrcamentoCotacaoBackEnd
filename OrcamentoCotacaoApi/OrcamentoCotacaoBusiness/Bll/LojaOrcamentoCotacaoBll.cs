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
            List<float> lstAlcadas = new List<float>();

            PercMaxDescEComissaoResponseViewModel retorno = new PercMaxDescEComissaoResponseViewModel();
            retorno.PercMaxComissao = tLoja.Perc_Max_Comissao;

            foreach (var permissao in lstPermissoes)
            {
                if (tipoCliente == Constantes.ID_PF)
                {
                    if (permissao == Constantes.COMISSAO_DESCONTO_ALCADA_1)
                        lstAlcadas.Add(tLoja.Perc_max_comissao_e_desconto_alcada1_pf);

                    if (permissao == Constantes.COMISSAO_DESCONTO_ALCADA_2)
                        lstAlcadas.Add(tLoja.Perc_max_comissao_e_desconto_alcada2_pf);

                    if (permissao == Constantes.COMISSAO_DESCONTO_ALCADA_3)
                        lstAlcadas.Add(tLoja.Perc_max_comissao_e_desconto_alcada3_pf);
                }
                if (tipoCliente == Constantes.ID_PJ)
                {
                    if (permissao == Constantes.COMISSAO_DESCONTO_ALCADA_1)
                        lstAlcadas.Add(tLoja.Perc_max_comissao_e_desconto_alcada1_pj);

                    if (permissao == Constantes.COMISSAO_DESCONTO_ALCADA_2)
                        lstAlcadas.Add(tLoja.Perc_max_comissao_e_desconto_alcada2_pj);

                    if (permissao == Constantes.COMISSAO_DESCONTO_ALCADA_3)
                        lstAlcadas.Add(tLoja.Perc_max_comissao_e_desconto_alcada3_pj);
                }
            }

            if (lstAlcadas.Count > 0)
            {
                retorno.PercMaxComissaoEDesconto = lstAlcadas.Max();
                return retorno;
            }

            return null;
        }

        public LojaViewModel BuscarLojaEstilo(string loja)
        {
            return _lojaBll.BuscarLojaEstilo(loja);
        }
    }
}
