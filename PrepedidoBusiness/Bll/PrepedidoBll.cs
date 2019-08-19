using PrepedidoBusiness;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using System.Linq;
using InfraBanco.Modelos;
using PrepedidoBusiness.Utils;
using PrepedidoBusiness.Dtos.Prepedido;

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
            //toda vez precisamos de uma nova conexao para os casos em que houver transacao
            var db = contextoProvider.GetContexto();
            var lista = from r in db.Torcamentos
                        where r.Orcamentista == orcamentista
                        orderby r.Orcamento
                        select r.Orcamento;
            var res = lista.AsEnumerable();
            return await Task.FromResult(res);
        }

        public async Task<IEnumerable<string>> ListarCpfCnpjPrepedidosCombo(string apelido)
        {
            var db = contextoProvider.GetContexto();

            var lista = from c in db.Torcamentos.Include(r => r.Tcliente)
                        where c.Orcamentista == apelido
                        orderby c.Tcliente.Cnpj_Cpf
                        select c.Tcliente.Cnpj_Cpf;

            var ret = await lista.Distinct().ToListAsync();
            List<string> cpfCnpjFormat = new List<string>();

            foreach (string cpf in ret)
            {
                cpfCnpjFormat.Add(Util.FormatCpf_Cnpj(cpf));
            }

            return cpfCnpjFormat;
        }

        public enum TipoBuscaPrepedido
        {
            Todos = 0, NaoViraramPedido = 1, SomenteViraramPedido = 2
        }
        public async Task<IEnumerable<PrepedidosCadastradosDtoPrepedido>> ListarPrePedidos(string apelido, TipoBuscaPrepedido tipoBusca, 
            string clienteBusca, string numeroPrePedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            var db = contextoProvider.GetContexto();

            var lst = db.Torcamentos.
                Where(r => r.Orcamentista == apelido);

            //filtro conforme o tipo do prepedido
            switch (tipoBusca)
            {
                case TipoBuscaPrepedido.Todos:
                    //sem filtro adicional
                    break;
                case TipoBuscaPrepedido.NaoViraramPedido:
                    lst = lst.Where(r => (r.St_Orcamento == "" || r.St_Orcamento == null)
                        && (r.St_Fechamento == "" || r.St_Fechamento == null));
                    break;
                case TipoBuscaPrepedido.SomenteViraramPedido:
                    lst = lst.Where(c => c.St_Orc_Virou_Pedido == 1);
                    break;
            }
            if (!string.IsNullOrEmpty(clienteBusca))
                lst = lst.Where(r => r.Tcliente.Nome.Contains(clienteBusca));
            if (!string.IsNullOrEmpty(numeroPrePedido))
                lst = lst.Where(r => r.Orcamento.Contains(numeroPrePedido));
            if (dataInicial.HasValue)
                lst = lst.Where(r => r.Data >= dataInicial.Value);
            if (dataFinal.HasValue)
                lst = lst.Where(r => r.Data <= dataFinal.Value);


            //COLOCAR O STATUS DO PEDIDO PARA PREPEDIDOS QUE VIRARAM PEDIDOS
            var lstfinal = lst.Select(r => new PrepedidosCadastradosDtoPrepedido
            {
                //A LOJA NÃO IRÁ MAIS APARECER
                Status = "Pré-Pedido",
                DataPrePedido = r.Data_Hora,
                NumeroPrepedido = r.Orcamento,
                NomeCliente = r.Tcliente.Nome,
                ValoTotal = r.Vl_Total
            });            

            var res = lstfinal.AsEnumerable();
            return await Task.FromResult(res);
        }

        //ESSE METODO ESTA SENDO EXECUTADO NO METODO ListarPrePedidos(parametros)
        //public async Task<IEnumerable<PrepedidoQueViraramPedidoDtoPedido>> ListarPrePedidosQueViraramPedidos(string apelido, DateTime? inicio, DateTime? fim)
        //{
        //    var db = contextoProvider.GetContexto();

        //    var lst = from c in db.Torcamentos.Include(r => r.Tcliente).Include(r => r.Tpedido)
        //              where c.Orcamentista == apelido &&
        //                    c.St_Orc_Virou_Pedido == 1 &&
        //                    c.Tpedido.Data >= inicio && c.Tpedido.Data <= DateTime.Now
        //              orderby c.Numero_Loja, c.Data, c.Orcamento
        //              select new PrepedidoQueViraramPedidoDtoPedido
        //              {
        //                  NumeroPrePedido = c.Orcamento,
        //                  DataOrcamento = c.Data,
        //                  NumeroPedido = c.Pedido,
        //                  DataPedido = c.Tpedido.Data,
        //                  NomeCliente = c.Tcliente.Nome,
        //                  Vl_Total_Familia = c.Tpedido.Vl_Total_Familia,
        //                  St_Entrega = c.Tpedido.St_Entrega

        //              };

        //    var res = lst.AsEnumerable();
        //    return await Task.FromResult(res);
        //}

        public void RemoverPrePedido(string numeroPrePedido, string apelido)
        {
            var db = contextoProvider.GetContexto();

            Torcamento prePedido = db.Torcamentos.
                Where(
                        r => r.Orcamentista == apelido &&
                        r.Orcamento == numeroPrePedido &&
                        (r.St_Orcamento == "" || r.St_Orcamento == null)
                      ).Single();

            var t = db.TclienteRefBancarias;

            prePedido.St_Orcamento = "CAN";
            prePedido.Cancelado_Data = DateTime.Now;
            prePedido.Cancelado_Usuario = apelido;
            db.SaveChanges();
        }


    }
}
