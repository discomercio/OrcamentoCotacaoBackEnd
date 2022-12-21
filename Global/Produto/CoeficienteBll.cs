using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Produto.Dados;

namespace Produto
{
    public class CoeficienteBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public CoeficienteBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<IEnumerable<CoeficienteDados>> BuscarListaCoeficientesFabricantesDistinct(List<string> fabricantesDistinct)
        {
            List<CoeficienteDados> lstRetorno = new List<CoeficienteDados>();
            if (fabricantesDistinct.Any())
            {
                List<CoeficienteDados> lstCoeficiente;

                using (var db = contextoProvider.GetContextoLeitura())
                {
                    //trazendo toda lista
                    var lstCoeficienteTask = await (from c in db.TpercentualCustoFinanceiroFornecedor
                                                    select new CoeficienteDados
                                                    {
                                                        Fabricante = c.Fabricante,
                                                        TipoParcela = c.Tipo_Parcelamento,
                                                        QtdeParcelas = c.Qtde_Parcelas,
                                                        Coeficiente = c.Coeficiente
                                                    }).ToListAsync();

                    lstCoeficiente = lstCoeficienteTask;
                }

                foreach (var fabricante in fabricantesDistinct)
                {
                    foreach (var c in lstCoeficiente)
                    {
                        if (fabricante == c.Fabricante)
                        {
                            lstRetorno.Add(c);
                        }
                    }
                }
            }

            return lstRetorno;
        }

        public async Task<IDictionary<string, CoeficienteDados>> BuscarListaCoeficientesFabricantesHistoricoDistinct(
            List<string> fabricantesDistinct, 
            string tipoParcela, 
            short qtdeParcelas, 
            DateTime dataRefCoeficiente)
        {
            Dictionary<string, CoeficienteDados> lstRetorno = new Dictionary<string, CoeficienteDados>();
            if (fabricantesDistinct.Any())
            {
                List<IGrouping<string, CoeficienteDados>> lstCoeficienteGroup;

                using (var db = contextoProvider.GetContextoLeitura())
                {
                    //trazendo toda lista
                    var lstCoeficienteTask = from c in db.TpercentualCustoFinanceiroFornecedorHistorico
                                             where
                                             c.Qtde_Parcelas == qtdeParcelas &&
                                             c.Tipo_Parcelamento == tipoParcela
                                             select new CoeficienteDados
                                             {
                                                 Fabricante = c.Fabricante,
                                                 TipoParcela = c.Tipo_Parcelamento,
                                                 QtdeParcelas = c.Qtde_Parcelas,
                                                 Coeficiente = c.Coeficiente,
                                                 Data = c.Data
                                             };

                    lstCoeficienteGroup = await lstCoeficienteTask.GroupBy(x => x.Fabricante).ToListAsync();
                }

                foreach (var coeficienteGroup in lstCoeficienteGroup)
                {
                    var cRetorno = coeficienteGroup.Where(x => x.Data.Date == dataRefCoeficiente.Date).FirstOrDefault();
                    foreach (var c in coeficienteGroup)
                    {
                        if (c.Data <= dataRefCoeficiente && c.Data > cRetorno.Data )
                        {
                            cRetorno = c;
                        }
                    }
                    lstRetorno.Add(cRetorno.Fabricante, cRetorno);
                }
            }

            return lstRetorno;
        }

        public async Task<IEnumerable<IEnumerable<CoeficienteDados>>> BuscarListaCoeficientesFornecedores(List<string> lstFornecedores)
        {
            var lstRetorno = new List<List<CoeficienteDados>>();

            using (var db = contextoProvider.GetContextoLeitura())
            {
                foreach (var f in lstFornecedores)
                {
                    List<CoeficienteDados> lstCoeficienteTask = await (from c in db.TpercentualCustoFinanceiroFornecedor
                                                                       where c.Fabricante == f
                                                                       select new CoeficienteDados
                                                                       {
                                                                           Fabricante = c.Fabricante,
                                                                           TipoParcela = c.Tipo_Parcelamento,
                                                                           QtdeParcelas = c.Qtde_Parcelas,
                                                                           Coeficiente = c.Coeficiente
                                                                       }).ToListAsync();
                    if (lstCoeficienteTask.Count > 0)
                    {
                        lstRetorno.Add(lstCoeficienteTask);
                    }
                }
            }

            return lstRetorno;
        }
    }
}
