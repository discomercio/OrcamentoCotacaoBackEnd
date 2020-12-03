using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCollectionOrderer("Especificacao.Testes.TesteListaDependencias.OrdemTestesCollection", "Especificacao")]

namespace Especificacao.Testes.TesteListaDependencias
{
    class OrdemTestesCollection : Xunit.ITestCollectionOrderer
    {
        //todos os features que devam ser executados no fim do processo devem ter isto no nome do Feature: 
        private static readonly string NomeFeatureExecutadaNoFim3 = "ListaDependencias";
        private static readonly string NomeFeatureExecutadaNoFim1 = "VerificarQueExecutou";
        private static readonly string NomeFeatureExecutadaNoFim2 = "ListaExecucao";

        private static readonly string NomeFeatureFimTudo = "VerificacaoFinalListaDependencias";

        /*
         * 
         * infelizmente, essa ordenação não funciona com o OpenCover
         * ainda tentado resolver o problema
         * 
         * não adianta: nome da feature, nome do scenario, nome da tag, nome da rotina
         * opção: fazer a inicialização por membros estáticos e não depender da ordem. Mas isso vai complicar a escrita dos testes.
         * */

        public IEnumerable<Xunit.Abstractions.ITestCollection> OrderTestCollections(IEnumerable<Xunit.Abstractions.ITestCollection> testCollections)
        {
            /*
             * código auxiliar para testes
             * 
            foreach (var col in testCollections)
            {
                var x = col.DisplayName.Split(' ');
                var nomeClasse = x.Last();
                var info = Type.GetType(nomeClasse);
            }
            */

            //quem tem NomeFeatureExecutadaNoFim vai para o fim
            var ret = testCollections.OrderBy(collection =>
                    collection.DisplayName.Contains(NomeFeatureExecutadaNoFim1)
                    || collection.DisplayName.Contains(NomeFeatureExecutadaNoFim2)
                    || collection.DisplayName.Contains(NomeFeatureExecutadaNoFim3)
                    || collection.DisplayName.Contains(NomeFeatureFimTudo))
                .ThenBy(collection =>
                    collection.DisplayName.Contains(NomeFeatureFimTudo));
            return ret;
        }
    }

}
