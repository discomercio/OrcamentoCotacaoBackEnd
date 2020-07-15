using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesClienteUnisBll
{
    [Collection("Testes não multithread porque o banco é unico")]
    public class InscricaoEstadual
    {
        private readonly ClienteUnisBll clienteUnisBll;

        public InscricaoEstadual(ClienteUnisBll clienteUnisBll)
        {
            this.clienteUnisBll = clienteUnisBll;
        }
        [Fact]
        public void ValidarInscricaoEstadual()
        {
            Assert.False(clienteUnisBll.VerificarInscricaoEstadualValida("", "SP").Result.InscricaoEstadualValida);
            Assert.False(clienteUnisBll.VerificarInscricaoEstadualValida("137784242", "").Result.InscricaoEstadualValida);

            Assert.True(clienteUnisBll.VerificarInscricaoEstadualValida("0377-842.42", "AP").Result.InscricaoEstadualValida);
            Assert.True(clienteUnisBll.VerificarInscricaoEstadualValida("037784242", "AP").Result.InscricaoEstadualValida);

            Assert.False(clienteUnisBll.VerificarInscricaoEstadualValida("1037784242", "AP").Result.InscricaoEstadualValida);
            Assert.False(clienteUnisBll.VerificarInscricaoEstadualValida("137784242", "AP").Result.InscricaoEstadualValida);
            Assert.False(clienteUnisBll.VerificarInscricaoEstadualValida("137784242", "SP").Result.InscricaoEstadualValida);

            Assert.True(clienteUnisBll.VerificarInscricaoEstadualValida("306.381.115.656", "SP").Result.InscricaoEstadualValida);
            Assert.True(clienteUnisBll.VerificarInscricaoEstadualValida("306381115.656", "SP").Result.InscricaoEstadualValida);
            Assert.False(clienteUnisBll.VerificarInscricaoEstadualValida("306381115.156", "SP").Result.InscricaoEstadualValida);

            Assert.True(clienteUnisBll.VerificarInscricaoEstadualValida("252386736", "SC").Result.InscricaoEstadualValida);
            Assert.False(clienteUnisBll.VerificarInscricaoEstadualValida("152386736", "SC").Result.InscricaoEstadualValida);

            Assert.True(clienteUnisBll.VerificarInscricaoEstadualValida("374.077.082.111", "SP").Result.InscricaoEstadualValida);
            Assert.False(clienteUnisBll.VerificarInscricaoEstadualValida("374.077.082.211", "SP").Result.InscricaoEstadualValida);

        }

    }
}
