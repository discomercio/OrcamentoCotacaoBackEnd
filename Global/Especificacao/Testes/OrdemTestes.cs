using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCollectionOrderer("Especificacao.Testes.OrdemTestesCollection", "Especificacao")]

namespace Especificacao.Testes
{
    class OrdemTestesCollection : Xunit.ITestCollectionOrderer
    {
        //todos os features que devam ser executados no fim do processo devem ter isto no nome do Feature: 
        private static readonly string NomeFeatureExecutadaNoFim = "VerificarQueExecutou";

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
            var ret = testCollections.OrderBy(collection => collection.DisplayName.Contains(NomeFeatureExecutadaNoFim));
            return ret;
        }
    }

}
