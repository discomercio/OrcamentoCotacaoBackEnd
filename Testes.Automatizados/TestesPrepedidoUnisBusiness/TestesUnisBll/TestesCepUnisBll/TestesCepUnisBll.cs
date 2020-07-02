using PrepedidoUnisBusiness.UnisBll.CepUnisBll;
using System;
using System.Collections.Generic;
using System.Text;
using Testes.Automatizados.InicializarBanco;
using Xunit;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesCepUnisBll
{
    public class TestesCepUnisBll
    {
        private readonly CepUnisBll cepUnisBll;
        private readonly InicializarBancoCep inicializarCep;

        public TestesCepUnisBll(CepUnisBll cepUnisBll, InicializarBanco.InicializarBancoCep inicializarCep)
        {
            this.cepUnisBll = cepUnisBll;
            this.inicializarCep = inicializarCep;
        }

        /*
         * TODO: terminar testes

        [Fact]
        public void BuscarUfs()
        {
            IEnumerable<PrepedidoUnisBusiness.UnisDto.CepUnisDto.UFeMunicipiosUnisDto> res = cepUnisBll.BuscarUfs().Result;
            Assert.NotEmpty(res);
        }

    */

        [Fact]
        public void BuscarCep()
        {
            var res = cepUnisBll.BuscarCep(InicializarBancoCep.DadosCep.Cep).Result;
            Assert.NotNull(res);

            res = cepUnisBll.BuscarCep(InicializarBancoCep.DadosCep.CepNaoExiste).Result;
            Assert.Null(res);
        }

    }
}
