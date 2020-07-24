using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace Especificacao.Testes.Utils.InjecaoDependencia
{
    public class ClasseInjetada
    {
        public string UmValorQualquer { get; set; } = "UmValorQualquer";
    }

    public class TesteInjecaoDependencia : ProvedorServicos
    {
        private readonly ClasseInjetada classeInjetada;
        private readonly InfraBanco.ContextoBdProvider contextoBdProvider;

        public TesteInjecaoDependencia()
        {
            this.classeInjetada = Servicos.GetRequiredService<ClasseInjetada>();
            this.contextoBdProvider = Servicos.GetRequiredService<InfraBanco.ContextoBdProvider>();
        }

        [Fact]
        public void TesteInjecao()
        {
            Assert.Equal("UmValorQualquer", this.classeInjetada.UmValorQualquer);

            //e vamos testar o acesso ao banco
            var contexto = contextoBdProvider.GetContextoLeitura();
            var clientes = from c in contexto.Tclientes
                           select c;
            Assert.NotEqual(-1, clientes.Count());
        }
    }
}
