using System;
using System.Collections.Generic;
using System.Text;

namespace Especificacao.Testes.Utils.ExecucaoCruzada
{
    public class ListaAmbientes<Implementacao>
    {
        private List<Implementacao> implementacoes = new List<Implementacao>();
        private bool usado = false;

        public void GivenImplementadoEm(string ambiente, Implementacao implementacao)
        {
            //todos os Implementado precisam acontecer antes do resto
            if (usado)
                throw new ArgumentException($"Inicializando ambiente {ambiente} depois de algum passo");

            implementacoes.Add(implementacao);
        }

        //nao pode usar se não tiver um ambiente
        private void VerificarInicializado()
        {
            usado = true;
            if (implementacoes.Count == 0)
                throw new ArgumentException($"Sem nenhum ambiente.");
        }

        public void ExecutarTodos(Action<Implementacao> callback)
        {
            VerificarInicializado();
            foreach (var i in implementacoes)
                callback(i);
        }

    }
}
