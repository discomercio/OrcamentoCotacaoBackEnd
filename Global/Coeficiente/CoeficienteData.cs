using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coeficiente
{
    public class CoeficienteData: BaseData<TpercentualCustoFinanceiroFornecedorHistorico, TpercentualCustoFinanceiroFornecedorHistoricoFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public CoeficienteData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TpercentualCustoFinanceiroFornecedorHistorico Atualizar(TpercentualCustoFinanceiroFornecedorHistorico obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TpercentualCustoFinanceiroFornecedorHistorico obj)
        {
            throw new NotImplementedException();
        }

        public TpercentualCustoFinanceiroFornecedorHistorico Inserir(TpercentualCustoFinanceiroFornecedorHistorico obj)
        {
            throw new NotImplementedException();
        }

        public List<TpercentualCustoFinanceiroFornecedorHistorico> PorFiltro(TpercentualCustoFinanceiroFornecedorHistoricoFiltro obj)
        {
            try
            {
                using(var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var saida = from c in db.TpercentualCustoFinanceiroFornecedorHistoricos
                                             select c;

                    if(obj.LstFabricantes.Distinct().Count() > 0)
                    {
                        saida = saida.Where(x => obj.LstFabricantes.Contains(x.Fabricante));
                    }
                    if (!string.IsNullOrEmpty(obj.TipoParcela))
                    {
                        saida = saida.Where(x => x.Tipo_Parcelamento == obj.TipoParcela);
                    }
                    if(obj.QtdeParcelas != 0)
                    {
                        saida = saida.Where(x => x.Qtde_Parcelas == obj.QtdeParcelas);
                    }
                    if(DateTime.TryParse(obj.DataRefCoeficiente.ToString(), out var value))
                    {
                        saida = saida.Where(x => x.Data.Date == obj.DataRefCoeficiente.Date);
                    }
                    if (obj.Page.HasValue && obj.RecordsPerPage.HasValue)
                    {
                        saida = saida.Skip((int)(obj.Page * obj.RecordsPerPage.Value)).Take(obj.RecordsPerPage.GetValueOrDefault());
                    }

                    return saida.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
