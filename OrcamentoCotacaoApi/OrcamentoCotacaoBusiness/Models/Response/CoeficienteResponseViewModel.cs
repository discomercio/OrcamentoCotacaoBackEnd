using InfraBanco.Modelos;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class CoeficienteResponseViewModel : IViewModelResponse
    {
        public string Fabricante { get; set; }
        public string TipoParcela { get; set; }
        public int QtdeParcelas { get; set; }
        public float Coeficiente { get; set; }


        public static CoeficienteResponseViewModel CoeficienteResponseViewModel_De_TpercentualCustoFinanceiroFornecedorHistorico(TpercentualCustoFinanceiroFornecedorHistorico origem)
        {
            if (origem == null) return null;
            return new CoeficienteResponseViewModel()
            {
                Fabricante = origem.Fabricante,
                TipoParcela = origem.Tipo_Parcelamento,
                QtdeParcelas = origem.Qtde_Parcelas,
                Coeficiente = origem.Coeficiente
            };
        }

        internal static List<CoeficienteResponseViewModel> ListaCoeficienteResponseViewModel_De_ListaTpercentualCustoFinanceiroFornecedorHistorico(List<TpercentualCustoFinanceiroFornecedorHistorico> origem)
        {
            if (origem == null) return null;
            var ret = new List<CoeficienteResponseViewModel>();
            if (origem != null)
                foreach (var p in origem)
                    ret.Add(CoeficienteResponseViewModel_De_TpercentualCustoFinanceiroFornecedorHistorico(p));
            return ret;
        }
    }
}
