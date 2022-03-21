using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;

namespace Coeficiente
{
    public class CoeficienteBll : BaseBLL<TpercentualCustoFinanceiroFornecedorHistorico, TpercentualCustoFinanceiroFornecedorHistoricoFiltro>
    {
        private CoeficienteData _data { get; set; }
        public CoeficienteBll(ContextoBdProvider contextoBdProvider) : base(new CoeficienteData(contextoBdProvider))
        {
            _data = new CoeficienteData(contextoBdProvider);
        }
    }
}
