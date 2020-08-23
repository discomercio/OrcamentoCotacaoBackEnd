using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Testes.Utils.ListaDependencias
{
    public class RegistroDependencias
    {
        //esta existe porque lista TODAS as especificacoes que tenham sido criadas
        //o primeiro níel é a implementação; o segundo é a especificação
        private static readonly Dictionary<string, List<string>> ambientes = new Dictionary<string, List<string>>();

        public static void AdicionarDependencia(string ambiente, string especificacao)
        {
            if (!ambientes.ContainsKey(ambiente))
                ambientes.Add(ambiente, new List<string>());
            if (!ambientes[ambiente].Contains(especificacao))
                ambientes[ambiente].Add(especificacao);
        }

        public static void VerificarQueUsou(string ambiente, string especificacao, ref bool deuErro)
        {
            if (!ambientes.ContainsKey(ambiente))
            {
                deuErro = true;
                Assert.Equal("", $"implementacao nunca foi definida: {ambiente}");
            }

            //este teste somente passa se executar todos os testes
            if (!ambientes[ambiente].Contains(especificacao))
                deuErro = true;
            Assert.Contains(especificacao, ambientes[ambiente]);
        }

    }
}
