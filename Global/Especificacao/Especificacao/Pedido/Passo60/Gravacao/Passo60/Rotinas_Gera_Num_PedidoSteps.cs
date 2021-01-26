using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao.Passo60
{
    [Binding, Scope(Tag = "Especificacao.Pedido.Passo60.Gravacao.Passo60")]
    public class Rotinas_Gera_Num_PedidoSteps
    {
        private readonly InfraBanco.ContextoBdProvider contextoBdProvider;
        public Rotinas_Gera_Num_PedidoSteps()
        {
            contextoBdProvider = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<InfraBanco.ContextoBdProvider>();
        }

        [Given(@"Colocar NSU do InfraBanco\.Constantes\.Constantes\.NSU_PEDIDO com ""(.*)""")]
        public void GivenColocarNSUDoInfraBanco_Constantes_Constantes_NSU_PEDIDOCom(string p0)
        {
            using var dbgravacao = contextoBdProvider.GetContextoGravacaoParaUsing();
            var controle = (from c in dbgravacao.Tcontroles
                            where c.Id_Nsu == InfraBanco.Constantes.Constantes.NSU_PEDIDO
                            select c).First();
            controle.Nsu = p0;
            dbgravacao.Update(controle);
            dbgravacao.SaveChanges();
            dbgravacao.transacao.Commit();
        }

        [Given(@"Colocar Ano_Letra_Seq do InfraBanco\.Constantes\.Constantes\.NSU_PEDIDO com ""(.*)""")]
        public void GivenColocarAno_Letra_SeqDoInfraBanco_Constantes_Constantes_NSU_PEDIDOCom(string p0)
        {
            using var dbgravacao = contextoBdProvider.GetContextoGravacaoParaUsing();
            var controle = (from c in dbgravacao.Tcontroles
                            where c.Id_Nsu == InfraBanco.Constantes.Constantes.NSU_PEDIDO
                            select c).First();
            controle.Ano_Letra_Seq = p0;
            dbgravacao.Update(controle);
            dbgravacao.SaveChanges();
            dbgravacao.transacao.Commit();
        }

        [Then(@"Gera_num_pedido_pai = ""(.*)"" sem erro")]
        public void ThenGera_Num_Pedido_PaiSemErro(string p0)
        {
            var listaErros = new List<string>();
            using var ContextoBdGravacao = contextoBdProvider.GetContextoGravacaoParaUsing();
            var idPedidoBase = global::Pedido.Criacao.Passo60.Gravacao.Grava60.Gera_num_pedido.Gera_num_pedido_pai(listaErros, ContextoBdGravacao).Result;
            Assert.Empty(listaErros);
            Assert.Equal(p0, idPedidoBase);
        }

        [Then(@"Gera_num_pedido_pai com erro contendo ""(.*)""")]
        public void ThenGera_Num_Pedido_PaiComErroContendo(string p0)
        {
            var listaErros = new List<string>();
            using var ContextoBdGravacao = contextoBdProvider.GetContextoGravacaoParaUsing();
            var idPedidoBase = global::Pedido.Criacao.Passo60.Gravacao.Grava60.Gera_num_pedido.Gera_num_pedido_pai(listaErros, ContextoBdGravacao).Result;
            //somente um erro
            Assert.Single(listaErros);
            Assert.Contains(p0, listaErros[0]);
        }
    }
}
