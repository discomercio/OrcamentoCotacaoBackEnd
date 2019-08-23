using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using PrepedidoBusiness.Dtos.Pedido;
using PrepedidoBusiness.Utils;

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
                    //SE TIVER QUE INCLUIR OS PEDIDO CANCELADOS É SÓ DESCOMENTAR A LINHA ABAIXO
                    //lista = lista.Where(r => r.St_Entrega != "CAN");
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
                ValorTotal = r.Vl_Total_NF
            }).OrderByDescending(r => r.DataPedido);


            //colocar as mensagens de status
            var listaComStatus = await listaFinal.ToListAsync();
            foreach (var pedido in listaComStatus)
            {
                if (pedido.Status == "ESP")
                    pedido.Status = "Em espera";
                if (pedido.Status == "SPL")
                    pedido.Status = "Split possível";
                if (pedido.Status == "SEP")
                    pedido.Status = "Separar";
                if (pedido.Status == "AET")
                    pedido.Status = "A entregar";
                if (pedido.Status == "ETG")
                    pedido.Status = "Entrega";
                if (pedido.Status == "CAN")
                    pedido.Status = "Cancelado";
            }
            return await Task.FromResult(listaComStatus);
        }
                
        public async Task<IEnumerable<string>> ListarCpfCnpjPedidosCombo(string apelido)
        {
            var db = contextoProvider.GetContexto();

            var lista = (from c in db.Tpedidos.Include(r => r.Tcliente)
                        where c.Orcamentista == apelido
                        orderby c.Tcliente.Cnpj_Cpf
                        select c.Tcliente.Cnpj_Cpf).Distinct();

            var ret = await lista.Distinct().ToListAsync();
            List<string> cpfCnpjFormat = new List<string>();

            foreach (string cpf in ret)
            {
                cpfCnpjFormat.Add(Util.FormatCpf_Cnpj(cpf));
            }

            return cpfCnpjFormat;
        }
    }
}
