using Especificacao.Testes.Utils.ListaDependencias;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Testes.Pedido
{
    public class PedidoPassosComuns : ListaImplementacoes<IPedidoPassosComuns>, IPedidoPassosComuns
    {
        public void GivenPedidoBase()
        {
            if (ignorarFeature) return;
            Utils.LogTestes.LogOperacoes2.DadoBase(this);
            base.Executar(i => i.GivenPedidoBase());
        }

        public void GivenPedidoBaseClientePF()
        {
            if (ignorarFeature) return;
            Utils.LogTestes.LogOperacoes2.DadoBaseClientePF(this);
            base.Executar(i => i.GivenPedidoBaseClientePF());
        }

        public void GivenPedidoBaseClientePJ()
        {
            if (ignorarFeature) return;
            Utils.LogTestes.LogOperacoes2.DadoBaseClientePJ(this);
            base.Executar(i => i.GivenPedidoBaseClientePJ());
        }

        public void GivenPedidoBaseClientePJComEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Utils.LogTestes.LogOperacoes2.DadoBaseClientePJComEnderecoDeEntrega(this);
            base.Executar(i => i.GivenPedidoBaseClientePJComEnderecoDeEntrega());
        }

        public void WhenInformo(string p0, string p1)
        {
            if (ignorarFeature) return;
            Utils.LogTestes.LogOperacoes2.Informo(p0, p1, this);
            base.Executar(i => i.WhenInformo(p0, p1));
        }

        public void ThenSemErro(string mensagem)
        {
            if (ignorarFeature) return;
            mensagem = Utils.MapeamentoMensagens.MapearMensagem(this.GetType().FullName, mensagem);
            Utils.LogTestes.LogOperacoes2.SemErro(mensagem, this);
            base.Executar(i => i.ThenSemErro(mensagem));
        }

        public void ThenErro(string mensagem)
        {
            if (ignorarFeature) return;
            mensagem = Utils.MapeamentoMensagens.MapearMensagem(this.GetType().FullName, mensagem);
            Utils.LogTestes.LogOperacoes2.Erro(mensagem, this);
            base.Executar(i => i.ThenErro(mensagem));
        }

        public void ThenSemNenhumErro()
        {
            if (ignorarFeature) return;
            Utils.LogTestes.LogOperacoes2.SemNenhumErro(this);
            base.Executar(i => i.ThenSemNenhumErro());
        }
        public void ListaDeItensInformo(int numeroItem, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensInformo(numeroItem, campo, valor, this);
            base.Executar(i => i.ListaDeItensInformo(numeroItem, campo, valor));
        }

        public void RecalcularTotaisDoPedido()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.RecalcularTotaisDoPedido(this);
            base.Executar(i => i.RecalcularTotaisDoPedido());
        }
        public void DeixarFormaDePagamentoConsistente()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DeixarFormaDePagamentoConsistente(this);
            base.Executar(i => i.DeixarFormaDePagamentoConsistente());
        }
        public void TabelaT_PEDIDORegistroPaiCriadoVerificarCampo(string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PEDIDO", "pedido", "registro pai criado", campo, valor, this);
            base.Executar(i => i.TabelaT_PEDIDORegistroPaiCriadoVerificarCampo(campo, valor));
        }
        public void TabelaT_PEDIDO_ITEMRegistroCriadoVerificarCampo(int item, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PEDIDO_ITEM", "pedido", "registro pai criado", campo, valor, this);
            base.Executar(i => i.TabelaT_PEDIDO_ITEMRegistroCriadoVerificarCampo(item, campo, valor));
        }
        public void TabelaT_PEDIDORegistrosFilhotesCriadosVerificarCampo(string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PEDIDO", "pedido", "registros filhotes criados", campo, valor, this);
            base.Executar(i => i.TabelaT_PEDIDORegistrosFilhotesCriadosVerificarCampo(campo, valor));
        }

        public void TabelaT_ESTOQUE_MOVIMENTORegistroPaiEProdutoVerificarCampo(string produto, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_ESTOQUE_MOVIMENTO", "produto", "verificar campos", campo, valor, this);
            base.Executar(i => i.TabelaT_ESTOQUE_MOVIMENTORegistroPaiEProdutoVerificarCampo(produto, campo, valor));
        }

        public void TabelaT_ESTOQUE_ITEMRegistroPaiEProdutoVerificarCampo(string produto, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_ESTOQUE_ITEM", "produto", "verificar campos", campo, valor, this);
            base.Executar(i => i.TabelaT_ESTOQUE_ITEMRegistroPaiEProdutoVerificarCampo(produto, campo, valor));
        }

        public void TabelaT_ESTOQUERegistroPaiVerificarCampo(string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_ESTOQUE", "pedido", "verificar campos", campo, valor, this);
            base.Executar(i => i.TabelaT_ESTOQUERegistroPaiVerificarCampo(campo, valor));
        }

        public void TabelaT_ESTOQUE_LOGPedidoGeradoVerificarCampo(string produto, string operacao, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_ESTOQUE_LOG", "pedido", "verificar campos", campo, valor, this);
            base.Executar(i => i.TabelaT_ESTOQUE_LOGPedidoGeradoVerificarCampo(produto, operacao, campo, valor));
        }

        public void TabelaT_LOGPedidoGeradoEOperacaoVerificarCampo(string operacao, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_LOG", "pedido", "verificar campos", campo, valor, this);
            base.Executar(i => i.TabelaT_LOGPedidoGeradoEOperacaoVerificarCampo(operacao, campo, valor));
        }

        public void TabelaT_PRODUTO_X_WMS_REGRA_CDFabricanteEProdutoVerificarCampo(string fabricante, string produto, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PRODUTO_X_WMS_REGRA_CD", "fabricante e produto", "verificar campos", campo, valor, this);
            base.Executar(i => i.TabelaT_PRODUTO_X_WMS_REGRA_CDFabricanteEProdutoVerificarCampo(fabricante, produto, campo, valor));
        }

        public void ListaDeItensComXitens(int p0)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensComXitens(p0, this);
            base.Executar(i => i.ListaDeItensComXitens(p0));
        }

        private bool ignorarFeature = false;
        public void GivenIgnorarCenarioNoAmbiente(string p0)
        {
            IgnorarCenarioNoAmbiente(p0, ref ignorarFeature, this.GetType());
            base.Executar(i => i.GivenIgnorarCenarioNoAmbiente(p0));
        }

        public static void IgnorarCenarioNoAmbiente(string p0, ref bool ignorarFeature, Type getType)
        {
            //comando especial: desabilitar o teste se estiver rodando contra o sql server real
            if (p0.ToUpper() == "UsarSqlServerNosTestesAutomatizados".ToUpper())
            {
                if (Testes.Utils.InjecaoDependencia.ProvedorServicos.UsarSqlServerNosTestesAutomatizados)
                    ignorarFeature = true;
                return;
            }

            var typeFullName = getType.FullName;
            if (typeFullName == null)
            {
                Assert.Equal("", "sem this.GetType().FullName");
                return;
            }

            //mal resolvido: temos um Especificacao na frente.... bom, tiramos!
            typeFullName = typeFullName.Replace("Especificacao.Ambiente.", "Ambiente.");
            typeFullName = typeFullName.Replace("Especificacao.Especificacao.", "Especificacao.");

            if (typeFullName == p0)
                ignorarFeature = true;
        }

        public void GivenPedidoBaseComEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            base.Executar(i => i.GivenPedidoBaseComEnderecoDeEntrega());
        }


    }
}
