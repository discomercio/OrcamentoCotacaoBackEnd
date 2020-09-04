using Pedido.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.PedidoUnisDto
{
    public class BlocoNotasDevolucaoMercadoriasUnisDto
    {
        public DateTime Dt_Hr_Cadastro { get; set; }
        public string Usuario { get; set; }
        public string Loja { get; set; }
        public string Mensagem { get; set; }


        public static List<BlocoNotasDevolucaoMercadoriasUnisDto> ListaBlocoNotasDevolucaoMercadoriasUnisDto_De_BlocoNotasDevolucaoMercadoriasPedidoDados(IEnumerable<BlocoNotasDevolucaoMercadoriasPedidoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<BlocoNotasDevolucaoMercadoriasUnisDto>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(BlocoNotasDevolucaoMercadoriasUnisDto_De_BlocoNotasDevolucaoMercadoriasPedidoDados(p));
            return ret;
        }
        public static BlocoNotasDevolucaoMercadoriasUnisDto BlocoNotasDevolucaoMercadoriasUnisDto_De_BlocoNotasDevolucaoMercadoriasPedidoDados(BlocoNotasDevolucaoMercadoriasPedidoDados origem)
        {
            if (origem == null) return null;
            return new BlocoNotasDevolucaoMercadoriasUnisDto()
            {
                Dt_Hr_Cadastro = origem.Dt_Hr_Cadastro,
                Usuario = origem.Usuario,
                Loja = origem.Loja,
                Mensagem = origem.Mensagem
            };
        }
    }
}
