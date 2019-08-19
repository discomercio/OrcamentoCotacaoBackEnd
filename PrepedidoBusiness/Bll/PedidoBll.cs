using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using PrepedidoBusiness.Dtos.Pedido;

namespace PrepedidoBusiness.Bll
{
    public class PedidoBll
    {
        private readonly InfraBanco.ContextoProvider contextoProvider;

        public PedidoBll(InfraBanco.ContextoProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<IEnumerable<string>> ListarNumerosPedidoCombo(string apelido)
        {
            var db = contextoProvider.GetContexto();

            var lista = from p in db.Tpedidos
                        where p.Orcamentista == apelido
                        orderby p.Pedido
                        select p.Pedido;

            var res = lista.AsEnumerable();
            return await Task.FromResult(res);
        }

        public enum TipoBuscaPedido
        {
            Todos = 0, PedidosEncerrados = 1, PedidosEmAndamento = 2
        }


        public async Task<IEnumerable<PedidoDtoPedido>> ListarPedidos(string apelido, TipoBuscaPedido tipoBusca,
            string clienteBusca, string numeroPedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            var db = contextoProvider.GetContexto();

            var lista = db.Tpedidos.Include(r => r.Tcliente).
                Where(r => r.Indicador == apelido);

            switch (tipoBusca)
            {
                case TipoBuscaPedido.Todos:
                    //sem filtro
                    break;
                case TipoBuscaPedido.PedidosEncerrados:
                    lista = lista.Where(r => r.St_Entrega == "ETG" || r.St_Entrega == "CAN");
                    break;
                case TipoBuscaPedido.PedidosEmAndamento:
                    lista = lista.Where(r => r.St_Entrega != "ETG" && r.St_Entrega != "CAN");
                    break;
            }

            if (!string.IsNullOrEmpty(clienteBusca))
                lista = lista.Where(r => r.Tcliente.Nome.Contains(clienteBusca));
            if (!string.IsNullOrEmpty(numeroPedido))
                lista = lista.Where(r => r.Pedido.Contains(numeroPedido));
            if (dataInicial.HasValue)
                lista = lista.Where(r => r.Data >= dataInicial.Value);
            if (dataFinal.HasValue)
                lista = lista.Where(r => r.Data <= dataFinal.Value);

            var listaFinal = lista.Select(r => new PedidoDtoPedido
            {
                NomeCliente = r.Tcliente.Nome_Iniciais_Em_Maiusculas,
                NumeroPedido = r.Pedido,
                DataPedido = r.Data,
                Status = r.St_Entrega,
                ValorTotal = 0
            });

            var res = listaFinal.AsEnumerable();
            return await Task.FromResult(res);
        }
    }
}
