using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto.DetalhesPedido
{
    public class BlocoNotasDevolucaoMercadoriasDtoPedido
    {
        public DateTime Dt_Hr_Cadastro { get; set; }
        public string Usuario { get; set; }
        public string Loja { get; set; }
        public string Mensagem { get; set; }

        public static List<BlocoNotasDevolucaoMercadoriasDtoPedido> ListaBlocoNotasDevolucaoMercadoriasDtoPedido_De_BlocoNotasDevolucaoMercadoriasPedidoDados(IEnumerable<BlocoNotasDevolucaoMercadoriasPedidoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<BlocoNotasDevolucaoMercadoriasDtoPedido>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(BlocoNotasDevolucaoMercadoriasDtoPedido_De_BlocoNotasDevolucaoMercadoriasPedidoDados(p));
            return ret;
        }
        public static BlocoNotasDevolucaoMercadoriasDtoPedido BlocoNotasDevolucaoMercadoriasDtoPedido_De_BlocoNotasDevolucaoMercadoriasPedidoDados(BlocoNotasDevolucaoMercadoriasPedidoDados origem)
        {
            if (origem == null) return null;
            return new BlocoNotasDevolucaoMercadoriasDtoPedido()
            {
                Dt_Hr_Cadastro = origem.Dt_Hr_Cadastro,
                Usuario = origem.Usuario,
                Loja = origem.Loja,
                Mensagem = origem.Mensagem
            };
        }
    }
}
