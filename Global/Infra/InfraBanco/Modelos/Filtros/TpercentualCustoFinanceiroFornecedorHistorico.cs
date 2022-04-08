using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TpercentualCustoFinanceiroFornecedorHistoricoFiltro: IFilter
    {
        public List<string> LstFabricantes { get; set; }
        public string TipoParcela { get; set; }
        public int QtdeParcelas { get; set; }
        public DateTime DataRefCoeficiente { get; set; }
    }
}
