using Pedido.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.PedidoUnisDto
{
    public class BlocoNotasUnisDto
    {
        public DateTime Dt_Hora_Cadastro { get; set; }
        public string Usuario { get; set; }
        public string Loja { get; set; }
        public string Mensagem { get; set; }



        public static List<BlocoNotasUnisDto> ListaBlocoNotasUnisDto_De_BlocoNotasPedidoDados(IEnumerable<BlocoNotasPedidoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<BlocoNotasUnisDto>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(BlocoNotasUnisDto_De_BlocoNotasPedidoDados(p));
            return ret;
        }
        public static BlocoNotasUnisDto BlocoNotasUnisDto_De_BlocoNotasPedidoDados(BlocoNotasPedidoDados origem)
        {
            if (origem == null) return null;
            return new BlocoNotasUnisDto()
            {
                Dt_Hora_Cadastro = origem.Dt_Hora_Cadastro,
                Usuario = origem.Usuario,
                Loja = origem.Loja,
                Mensagem = origem.Mensagem
            };
        }
    }
}
