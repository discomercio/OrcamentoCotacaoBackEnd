using Especificacao.Testes.Pedido;
using Especificacao.Testes.Utils.ListaDependencias;
using System;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

/*
 * esta classe serve para implementar muitos dos features.
 * Os steps são bem parecidos.
 * Ele usa a tag para saber quem faz a omplementação efetiva
 * */

namespace Especificacao.Especificacao.Pedido
{
    [Binding]
    [Scope(Tag = "Especificacao.Pedido.Passo10.CamposSimples")]
    [Scope(Tag = "Especificacao.Pedido.Passo20.EnderecoEntrega")]
    [Scope(Tag = "Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj")]
    [Scope(Tag = "Especificacao.Pedido.FluxoCriacaoPedido")]
    [Scope(Tag = "Especificacao.Pedido.Passo30")]
    [Scope(Tag = "Especificacao.Pedido.Passo40")]
    public class PedidoComunsSteps : PedidoPassosComuns
    {
        public PedidoComunsSteps(FeatureContext featureContext)
        {
            var tags = featureContext.FeatureInfo.Tags.ToList();

            if (tags.Contains("Especificacao.Pedido.Passo10.CamposSimples"))
            {
                var imp = new Especificacao.Pedido.PedidoSteps();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                    "Especificacao.Pedido.Passo10.CamposSimplesPfListaDependencias");
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                    "Especificacao.Pedido.Passo10.CamposSimplesPjListaDependencias");
                return;
            }
            if (tags.Contains("Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj"))
            {
                var imp = new Especificacao.Pedido.PedidoSteps();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                    "Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj.ClientePjListaDependencias");
                return;
            }
            if (tags.Contains("Especificacao.Pedido.Passo20.EnderecoEntrega"))
            {
                var imp = new Especificacao.Pedido.PedidoSteps();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                    "Especificacao.Pedido.Passo20.EnderecoEntrega.EnderecoEntregaListaDependencias");
                return;
            }
            if (tags.Contains("Especificacao.Pedido.FluxoCriacaoPedido"))
            {
                var imp = new Especificacao.Pedido.PedidoSteps();
                base.AdicionarImplementacao(imp);
                //este nao tem registro de dependencias
                //RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                //    "Especificacao.Pedido.FluxoCriacaoPedidoDependencias");
                return;
            }
            if (tags.Contains("Especificacao.Pedido.Passo30"))
            {
                throw new NotImplementedException("Implementar as dependências");
            }
            if (tags.Contains("Especificacao.Pedido.Passo40"))
            {
                var imp = new Especificacao.Pedido.PedidoSteps();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                    "Especificacao.Pedido.Passo40.Passo40ListaDependencias");
                return;
            }
        }

        [Given(@"Pedido base cliente PF")]
        [When(@"Pedido base cliente PF")]
        new public void GivenPedidoBaseClientePF()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseClientePF(this);
            base.GivenPedidoBaseClientePF();
        }

        [Given(@"Pedido base cliente PJ")]
        [When(@"Pedido base cliente PJ")]
        new public void GivenPedidoBaseClientePJ()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseClientePJ(this);
            base.GivenPedidoBaseClientePJ();
        }


        [Given(@"Pedido base")]
        [When(@"Pedido base")]
        [Then(@"Pedido base")]
        new public void GivenPedidoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBase(this);
            base.GivenPedidoBase();
        }

        [Given(@"Pedido base cliente PJ com endereço de entrega")]
        new public void GivenPedidoBaseClientePJComEnderecoDeEntrega()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseClientePJComEnderecoDeEntrega(this);
            base.GivenPedidoBaseClientePJComEnderecoDeEntrega();
        }

        [Given(@"Pedido base cliente PJ com endereço de entrega PJ")]
        public void GivenPedidoBaseClientePJComEnderecoDeEntregaPJ()
        {
            GivenPedidoBaseClientePJComEnderecoDeEntrega();
            WhenInformo("EndEtg_tipo_pessoa", "PJ");
            WhenInformo("EndEtg_cnpj_cpf", "76297703000195");
            WhenInformo("EndEtg_produtor_rural_status", "COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL");
            WhenInformo("EndEtg_contribuinte_icms_status", "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL");

            WhenInformo("EndEtg_ddd_res", "");
            WhenInformo("EndEtg_tel_res", "");
            WhenInformo("EndEtg_ddd_cel", "");
            WhenInformo("EndEtg_tel_cel", "");
            WhenInformo("EndEtg_ddd_com", "11");
            WhenInformo("EndEtg_tel_com", "12345678");
            WhenInformo("EndEtg_rg", "");
        }
        [Given(@"Pedido base cliente PJ com endereço de entrega PF")]
        public void GivenPedidoBaseClientePJComEnderecoDeEntregaPF()
        {
            GivenPedidoBaseClientePJComEnderecoDeEntrega();
            WhenInformo("EndEtg_tipo_pessoa", "PF");
            WhenInformo("EndEtg_cnpj_cpf", "35270445824");
            WhenInformo("EndEtg_produtor_rural_status", "COD_ST_CLIENTE_PRODUTOR_RURAL_NAO");
            WhenInformo("EndEtg_contribuinte_icms_status", "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO");

            WhenInformo("EndEtg_ddd_res", "11");
            WhenInformo("EndEtg_tel_res", "12345678");
            WhenInformo("EndEtg_ddd_com", "");
            WhenInformo("EndEtg_tel_com", "");
            WhenInformo("EndEtg_rg", "12345678");
        }

        [Given(@"Pedido base COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA")]
        public void GivenPedidoBaseCOD_FORMA_PAGTO_PARCELADO_COM_ENTRADA()
        {
            GivenPedidoBaseClientePJ();
            WhenInformo("FormaPagtoCriacao.Tipo_Parcelamento", "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA");
            WhenInformo("FormaPagtoCriacao.Op_pce_entrada_forma_pagto", "1");
            WhenInformo("FormaPagtoCriacao.Op_pce_prestacao_forma_pagto", "1");
            WhenInformo("FormaPagtoCriacao.C_pce_prestacao_valor", ((3470.24 - 1) / 3).ToString());
            WhenInformo("FormaPagtoCriacao.C_pce_entrada_valor", "1");
            WhenInformo("FormaPagtoCriacao.C_pce_prestacao_periodo", "1");
            WhenInformo("FormaPagtoCriacao.CustoFinancFornecTipoParcelamento", "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA");
            WhenInformo("FormaPagtoCriacao.CustoFinancFornecQtdeParcelas", "3");
            WhenInformo("FormaPagtoCriacao.C_pce_prestacao_qtde", "3");
        }

        [When(@"Endereço de entrega do estado ""(.*)""")]
        public void WhenEnderecoDeEntregaDoEstado(string p0)
        {
            //somente SP ou BA
            Testes.Utils.LogTestes.LogOperacoes2.EnderecoDeEntregaDoEstado(p0, this);
            string[] permitidos = { "SP", "BA" };
            Assert.Contains(p0, permitidos);
            if (p0 == "SP")
                EnderecoDeEntregaDoEstado("02408150", "São Paulo", "Água Fria", "Rua Francisco Pecoraro", "SP");
            if (p0 == "BA")
                EnderecoDeEntregaDoEstado("40290050", "Salvador", "Acupe de Brotas", "Rua Prado Moraes", "BA");
        }
        private void EnderecoDeEntregaDoEstado(string EndEtg_cep, string EndEtg_cidade, string EndEtg_bairro, string EndEtg_endereco, string EndEtg_uf)
        {
            WhenInformo("EndEtg_cep", EndEtg_cep);
            WhenInformo("EndEtg_cidade", EndEtg_cidade);
            WhenInformo("EndEtg_bairro", EndEtg_bairro);
            WhenInformo("EndEtg_endereco", EndEtg_endereco);
            WhenInformo("EndEtg_uf", EndEtg_uf);
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        new public void WhenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Informo(p0, p1, this);
            base.WhenInformo(p0, p1);
        }

        [Then(@"Erro ""(.*)""")]
        new public void ThenErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Erro(p0, this);
            base.ThenErro(p0);
        }

        [Then(@"Sem [Ee]rro ""(.*)""")]
        new public void ThenSemErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.SemErro(p0, this);
            base.ThenSemErro(p0);
        }

        [Given(@"No ambiente ""(.*)"" erro ""(.*)"" é ""(.*)""")]
        [Given(@"No ambiente ""(.*)"" mapear erro ""(.*)"" para ""(.*)""")]
        public void GivenNoAmbienteErroE(string p0, string p1, string p2)
        {
            Testes.Utils.MapeamentoMensagens.GivenNoAmbienteErroE(p0, p1, p2);
        }

        //o parâmetro é o nome da classe que efetivamente implementa
        [Given(@"Ignorar cenário no ambiente ""(.*)""")]
        new public void GivenIgnorarCenarioNoAmbiente(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.IgnorarCenarioNoAmbiente(p0, this);
            base.GivenIgnorarCenarioNoAmbiente(p0);
        }

        [Given(@"Pedido base com endereço de entrega")]
        new public void GivenPedidoBaseComEnderecoDeEntrega()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseComEnderecoDeEntrega(this);
            base.GivenPedidoBaseComEnderecoDeEntrega();
        }

        [Then(@"Sem nenhum erro")]
        new public void ThenSemNenhumErro()
        {
            Testes.Utils.LogTestes.LogOperacoes2.SemNenhumErro(this);
            base.ThenSemNenhumErro();
        }

        [When(@"Lista de itens ""(.*)"" informo ""(.*)"" = ""(.*)""")]
        public void WhenListaDeItensInformo(int numeroItem, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensInformo(numeroItem, campo, valor, this);
            base.ListaDeItensInformo(numeroItem, campo, valor);
        }

        [When(@"Recalcular totais do pedido")]
        public void WhenRecalcularTotaisDoPedido()
        {
            Testes.Utils.LogTestes.LogOperacoes2.RecalcularTotaisDoPedido(this);
            base.RecalcularTotaisDoPedido();
        }
        [When(@"Deixar forma de pagamento consistente")]
        public void WhenDeixarFormaDePagamentoConsistente()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DeixarFormaDePagamentoConsistente(this);
            base.DeixarFormaDePagamentoConsistente();
        }
        [When(@"Lista de itens com ""(.*)"" itens")]
        public void WhenListaDeItensComXitens(int p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensComXitens(p0, this);
            base.ListaDeItensComXitens(p0);
        }

    }
}
