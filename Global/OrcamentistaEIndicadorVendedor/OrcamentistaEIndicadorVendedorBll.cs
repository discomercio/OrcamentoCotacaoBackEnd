using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcamentistaEIndicadorVendedor
{
    public class OrcamentistaEIndicadorVendedorBll : BaseBLL<TorcamentistaEIndicadorVendedor, TorcamentistaEIndicadorVendedorFiltro>
    {
        public OrcamentistaEIndicadorVendedorData _data { get; set; }

        public OrcamentistaEIndicadorVendedorBll(ContextoBdProvider contextoBdProvider) : base(new OrcamentistaEIndicadorVendedorData(contextoBdProvider))
        {
            _data = (OrcamentistaEIndicadorVendedorData)base.data;
        }
    }
}
