using Pedido;
using PrepedidoUnisBusiness.UnisDto.PedidoUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PrepedidoUnisBusiness.UnisBll.PedidoUnisBll
{
    public class PedidoUnisBll
    {
        private readonly PedidoBll pedidoBll;
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public PedidoUnisBll(PedidoBll pedidoBll, InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.pedidoBll = pedidoBll;
            this.contextoProvider = contextoProvider;
        }

        public async Task<PedidoUnisDto> BuscarPedido(string pedido)
        {
            PedidoUnisDto pedidoUnis = new PedidoUnisDto();

            //vamos buscar o orcamentista do pedido informado para passar para a busca de montagem do pedido no global
            var db = contextoProvider.GetContextoLeitura();

            string orcamentista = (from c in db.Tpedidos
                                   where c.Pedido == pedido
                                   select c.Orcamentista).FirstOrDefault();

            Pedido.Dados.DetalhesPedido.PedidoDados ret = await pedidoBll.BuscarPedido(orcamentista.Trim(), pedido);



            //afazer: converter 
            //afazer: retornar

            return await Task.FromResult(pedidoUnis);
        }
    }
}
