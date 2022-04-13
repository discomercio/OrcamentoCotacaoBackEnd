using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
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

            if (parceiro == null) throw new ArgumentException("Parceiro não encontrado!");

            return parceiro;
        }
    }
}
