using PrepedidoBusiness.Tipos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using System.Linq;

namespace PrepedidoBusiness.Bll
{
    public class PrepedidoBll
    {
        private readonly InfraBanco.ContextoProvider contextoProvider;

        public PrepedidoBll(InfraBanco.ContextoProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<IEnumerable<string>> ListarNumerosPrepedidosCombo(string orcamentista)
        {
            var db = contextoProvider.GetContexto();
            var lista = from r in db.Torcamentos
                            where r.Orcamentista == orcamentista
                            orderby r.Orcamento
                        select r.Orcamento;
            var res = lista.AsEnumerable();
            return await Task.FromResult(res);
        }

        /*

        public class PedidoDto
        {
            public string nomecliente;
            public decimal? valorpediddo;
        }
        public async Task<IEnumerable<PedidoDto>> BuscarPedidosDoCliente(string idCliente)
        {
            var db = contextoProvider.GetContexto();
            var lista = from r in db.Torcamentos
                        where r.Id_Cliente== idCliente
                        orderby r.Orcamento
                        select new PedidoDto() {
                             nomecliente=r.Orcamentista,
                              valorpediddo=r.Vl_Total
                        };

            
            var lista2 = db.Torcamentos.Where(r => r.Orcamentista == idPedido).OrderBy(r => r.Orcamento).
                        Select(r => new PedidoDto()
                        {
                            nomecliente = r.Orcamentista,
                            valorpediddo = r.Vl_Total
                        });
                        

            var res = lista.AsEnumerable();
            return await Task.FromResult(res);
        }
        */


    }
}
