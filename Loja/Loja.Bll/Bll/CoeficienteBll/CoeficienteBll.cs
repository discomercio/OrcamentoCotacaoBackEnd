using Loja.Bll.Dto.CoeficienteDto;
using Loja.Bll.Dto.PedidoDto.DetalhesPedido;
using Loja.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Bll.CoeficienteBll
{
    public class CoeficienteBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoBdProvider;

        public CoeficienteBll(InfraBanco.ContextoBdProvider contextoBdProvider)
        {
            this.contextoBdProvider = contextoBdProvider;
        }

        public async Task<IEnumerable<CoeficienteDto>> BuscarListaCoeficientes(List<PedidoProdutosDtoPedido> produtos)
        {
            List<CoeficienteDto> lstRetorno = new List<CoeficienteDto>();

            List<string> fabricantesDistinct = (from c in produtos
                                                select c.Fabricante).Distinct().ToList();
            if (fabricantesDistinct.Any())
            {
                var db = contextoBdProvider.GetContextoLeitura();

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

        public async Task<IEnumerable<CoeficienteDto>> BuscarListaCompletaCoeficientes()
        {
            List<CoeficienteDto> lstRetorno = new List<CoeficienteDto>();

            var db = contextoBdProvider.GetContextoLeitura();

            //trazendo toda lista
            var lstCoeficienteTask = from c in db.TpercentualCustoFinanceiroFornecedors
                                     select new CoeficienteDto
                                     {
                                         Fabricante = c.Fabricante,
                                         TipoParcela = c.Tipo_Parcelamento,
                                         QtdeParcelas = c.Qtde_Parcelas,
                                         Coeficiente = c.Coeficiente
                                     };

            lstRetorno = await lstCoeficienteTask.ToListAsync();

            return lstRetorno;
        }
    }
}
