using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Dto.Pedido.DetalhesPedido
{
    public class BlocoNotasDtoPedido
    {
        public DateTime Dt_Hora_Cadastro { get; set; }
        public string Usuario { get; set; }
        public string Loja { get; set; }
        public string Mensagem { get; set; }



        public static List<BlocoNotasDtoPedido> ListaBlocoNotasDtoPedido_De_BlocoNotasPedidoDados(IEnumerable<BlocoNotasPedidoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<BlocoNotasDtoPedido>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(BlocoNotasDtoPedido_De_BlocoNotasPedidoDados(p));
            return ret;
        }
        public static BlocoNotasDtoPedido BlocoNotasDtoPedido_De_BlocoNotasPedidoDados(BlocoNotasPedidoDados origem)
        {
            if (origem == null) return null;
            return new BlocoNotasDtoPedido()
            {
                Dt_Hora_Cadastro = origem.Dt_Hora_Cadastro,
                Usuario = origem.Usuario,
                Loja = origem.Loja,
                Mensagem = origem.Mensagem
            };
        }
    }
}
