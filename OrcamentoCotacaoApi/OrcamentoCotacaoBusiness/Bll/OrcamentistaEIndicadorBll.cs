using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using OrcamentoCotacaoBusiness.Models.Request.OrcamentistaIndicador;
using OrcamentoCotacaoBusiness.Models.Response.Orcamentista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class OrcamentistaEIndicadorBll
    {

        private readonly OrcamentistaEindicador.OrcamentistaEIndicadorBll orcamentistaEIndicadorBll;

        public OrcamentistaEIndicadorBll(OrcamentistaEindicador.OrcamentistaEIndicadorBll orcamentistaEIndicadorBll)
        {
            this.orcamentistaEIndicadorBll = orcamentistaEIndicadorBll;
        }

        public List<TorcamentistaEindicador> BuscarParceiros(TorcamentistaEindicadorFiltro filtro)
        {
            var parceiros = orcamentistaEIndicadorBll.PorFiltro(filtro);

            if (parceiros == null)
            {
                throw new ArgumentException("Parceiros não encontrado!");
            }

            return parceiros;
        }

        public TorcamentistaEindicador BuscarParceiroPorApelido(TorcamentistaEindicadorFiltro filtro)
        {
            var parceiro = orcamentistaEIndicadorBll.PorFiltro(filtro).FirstOrDefault();

            return parceiro;
        }

        public ParceirosComboResponse BuscarParceirosCombo(BuscarParceiroRequest request)
        {
            try
            {
                var response = new ParceirosComboResponse();
                response.Sucesso = true;

                TorcamentistaEindicadorFiltro filtro = new TorcamentistaEindicadorFiltro();
                filtro.vendedorId = request.Vendedor;
                filtro.Lojas = new List<string>();
                filtro.Lojas = request.Lojas;

                var parceiros = orcamentistaEIndicadorBll.PorFiltro(filtro);

                response.Parceiros = new List<ParceiroComboResponse>();
                foreach (var parceiro in parceiros)
                {
                    var parca = new ParceiroComboResponse();
                    parca.Id = parceiro.IdIndicador;
                    parca.RazaoSocial = parceiro.Razao_social_nome_iniciais_em_maiusculas;
                    response.Parceiros.Add(parca);
                }
                response.Parceiros = response.Parceiros.OrderBy(x => x.RazaoSocial).Distinct().ToList();

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
