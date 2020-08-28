using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll;
using PrepedidoBusiness.Bll.ClienteBll;
using PrepedidoBusiness.Bll.PrepedidoBll;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesPrepedidoUnisBll
{
    [Collection("Testes não multithread porque o banco é unico")]
    public class TesteDetalhesPrepedido : TestesPrepedidoUnisBll
    {
        public TesteDetalhesPrepedido(InicializarBanco.InicializarBancoGeral inicializarBanco, ITestOutputHelper Output, PrePedidoUnisBll prepedidoUnisBll,
            ClienteUnisBll clienteUnisBll, PrepedidoBll prepedidoBll, ClienteBll clienteBll) :
            base(inicializarBanco, Output, prepedidoUnisBll, clienteUnisBll, prepedidoBll, clienteBll)
        {

        }

        [Fact]
        public void DetalhesPrepedido()
        {
            Teste(c =>
            {
                c.DetalhesPrepedido.St_Entrega_Imediata = 6;
            }, "Valor de Entrega Imediata inválida!", true);

            Teste(c =>
            {
                c.DetalhesPrepedido.St_Entrega_Imediata = 1;
                c.DetalhesPrepedido.PrevisaoEntregaData = DateTime.Now.Date.AddDays(-1); 
            }, "Favor informar a data de 'Entrega Imediata' posterior a data atual!", true);
        }
    }
}
