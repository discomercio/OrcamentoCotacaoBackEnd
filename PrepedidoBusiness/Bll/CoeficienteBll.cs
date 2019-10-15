using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using PrepedidoBusiness.Dto.Produto;
using Microsoft.EntityFrameworkCore;

namespace PrepedidoBusiness.Bll
{
    public class CoeficienteBll
    {
        private readonly InfraBanco.ContextoProvider contextoProvider;

        public CoeficienteBll(InfraBanco.ContextoProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<IEnumerable<CoeficienteDto>> BuscarListaCoeficientes(List<PrepedidoProdutoDtoPrepedido> produtos)
        {
            List<CoeficienteDto> lstRetorno = new List<CoeficienteDto>();

            List<string> fabricantesDistinct = (from c in produtos
                                  select c.Fabricante).Distinct().ToList();
            if (fabricantesDistinct.Any())
            {
                var db = contextoProvider.GetContextoLeitura();

                //trazendo toda lista
                var lstCoeficienteTask = from c in db.TpercentualCustoFinanceiroFornecedors
                                         select new CoeficienteDto
                                         {
                                             Fabricante = c.Fabricante,
                                             TipoParcela = c.Tipo_Parcelamento,
                                             QtdeParcelas = c.Qtde_Parcelas,
                                             Coeficiente = c.Coeficiente
                                         };
                var lstCoeficiente = await lstCoeficienteTask.ToListAsync();

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
    }
}
