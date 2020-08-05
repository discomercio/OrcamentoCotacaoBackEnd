using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Testes.Utils.ExecucaoCruzada
{
    public class ListaEspecificacoes
    {
        //esta existe porque lista TODAS as especificacoes que tenham sido criadas
        private static readonly List<string> especificacoes = new List<string>();
        public ListaEspecificacoes(string especificacao)
        {
            especificacoes.Add(especificacao);
        }

        public static void VerificarQueExecutou(string especificacao)
        {
            //este teste somente passa se executar todos os testes
            Assert.Contains(especificacao, especificacoes);
        }

    }
}
