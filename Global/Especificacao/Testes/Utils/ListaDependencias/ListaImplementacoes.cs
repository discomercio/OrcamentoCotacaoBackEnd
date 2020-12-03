using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Testes.Utils.ListaDependencias
{
    public class ListaImplementacoes<Implementacao>
    {
        protected List<Implementacao> implementacoes = new List<Implementacao>();
        private bool usado = false;

        public void AdicionarImplementacao(Implementacao implementacao)
        {
            //todos os Implementado precisam acontecer antes do resto
            if (usado)
                Assert.Equal("", $"Inicializando ambiente depois de algum passo. " + $"StackTrace: '{Environment.StackTrace}'");

            implementacoes.Add(implementacao);
        }

        //nao pode usar se não tiver um ambiente
        private void VerificarInicializado()
        {
            usado = true;
            if (implementacoes.Count == 0)
                Assert.Equal("", $"Sem nenhuma implementacao (ambiente)." + $"StackTrace: '{Environment.StackTrace}'");
        }

        public void Executar(Action<Implementacao> callback)
        {
            VerificarInicializado();
            foreach (var i in implementacoes)
                callback(i);
        }

    }
}
