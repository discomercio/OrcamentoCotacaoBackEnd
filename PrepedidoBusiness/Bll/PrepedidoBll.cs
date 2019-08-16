using PrepedidoBusiness;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using System.Linq;
using ArclubePrepedidosWebapi.Dtos.Prepedido;
using InfraBanco.Modelos;
using PrepedidoBusiness.Utils;

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
                                   
            var ret = lista.Distinct().AsEnumerable();
            List<string> cpfCnpjFormat = new List<string>();

            foreach (string cpf in ret)
            {
                cpfCnpjFormat.Add(Util.FormatCpf_Cnpj(cpf));
            }

            ret = cpfCnpjFormat;

            return await Task.FromResult(ret);
        }

        public async Task<IEnumerable<PrepedidosCadastradosDtoPrepedido>> ListarPrePedidos(string idCliente)
        {
            var db = contextoProvider.GetContexto();

            var lst = db.Torcamentos.
                Where(r => r.Orcamentista == idCliente &&
                        (r.St_Fechamento == "" || r.St_Fechamento == null) &&
                        (r.St_Orcamento == "" || r.St_Orcamento == null)).
                        Select(r => new PrepedidosCadastradosDtoPrepedido
                        {
                            Loja = r.Loja,
                            DataPrePedido = r.Data_Hora,
                            NumeroPrepedido = r.Orcamento,
                            NomeCliente = r.Orcamentista,
                            ValoTotal = r.Vl_Total
                        });
            #region exemploAcesso
            //var lista2 = db.Torcamentos.Where(r => r.Orcamentista == idPedido).OrderBy(r => r.Orcamento).
            //            Select(r => new PedidoDto()
            //            {
            //                nomecliente = r.Orcamentista,
            //                valorpediddo = r.Vl_Total
            //            });

            //var lista = from r in db.Torcamentos
            //            where r.Id_Cliente == idCliente
            //            orderby r.Orcamento
            //            select new PrepedidosCadastradosDtoPrepedido()
            //            {
            //                Loja = r.Loja,
            //                DataPrePedido = r.Data_Hora,
            //                NumeroPrepedido = r.Orcamento,
            //                NomeCliente = r.Orcamentista,
            //                ValoTotal = r.Vl_Total
            //            };
            #endregion
            var res = lst.AsEnumerable();
            return await Task.FromResult(res);
        }

        public void RemoverPrePedido(string numeroPrePedido, string apelido)
        {
            var db = contextoProvider.GetContexto();

            Torcamento prePedido = db.Torcamentos.
                Where(
                        r => r.Orcamentista == apelido &&
                        r.Orcamento == numeroPrePedido &&
                        (r.St_Orcamento == "" || r.St_Orcamento == null)
                      ).Single();

            var t = db.TclienteRefBancaria;

            prePedido.St_Orcamento = "CAN";
            prePedido.Cancelado_Data = DateTime.Now;
            prePedido.Cancelado_Usuario = apelido;
            db.SaveChanges();
        }


    }
}
