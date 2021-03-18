using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Prepedido;
using Prepedido.Dados.DetalhesPrepedido;
using Xunit;

namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao.Passo25
{
    [Binding, Scope(Tag = "Especificacao.Pedido.Passo60.Gravacao.Passo25")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class verifica_se_as_regras_associadas_aos_produtos_estao_okStep
    {
        public readonly InfraBanco.ContextoBdProvider contextoBdProvider;

        public verifica_se_as_regras_associadas_aos_produtos_estao_okStep()
        {
            var servicos = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos();
            this.contextoBdProvider = servicos.GetRequiredService<InfraBanco.ContextoBdProvider>();
        }

        List<Produto.RegrasCrtlEstoque.RegrasBll> Lst_ctrlRegra = new List<Produto.RegrasCrtlEstoque.RegrasBll>();
        readonly List<string> lstErros = new List<string>();
        readonly PrePedidoDados prepedidoDados = Newtonsoft.Json.JsonConvert.DeserializeObject<PrePedidoDados>(PrepedidoDados);


        [Given(@"Chamar ObtemCtrlEstoqueProdutoRegra")]
        public void GivenChamarObtemCtrlEstoqueProdutoRegra()
        {
            //var db = contextoBdProvider.GetContextoLeitura();
            Lst_ctrlRegra = PrepedidoBll.ObtemCtrlEstoqueProdutoRegra(contextoBdProvider, prepedidoDados, lstErros).Result;

            Assert.NotNull(Lst_ctrlRegra);
            if (lstErros.Count != 0)
            {
                foreach (var erro in lstErros)
                    Assert.Equal("Erro", erro);
            }
        }

        [Given(@"Lista de CtrlRegra alterar registro de TwmsRegraCd do produto = ""(.*)"", campo ""(.*)"" = ""(.*)""")]
        public void GivenAlterarRegistroDaListaDeCtrlRegraCampo(string produto, string campo, string valor)
        {
            var t_wmsRegraCd = Lst_ctrlRegra.Where(x => x.Produto == produto).Select(x => x.TwmsRegraCd).ToList();

            switch (campo)
            {
                case "id":
                    if (!int.TryParse(valor, out int v))
                        Assert.Equal("Erro", $"O campo {campo.ToUpper()} com valor = {valor}, não é do tipo inteiro.");
                    t_wmsRegraCd[0].Id = v;
                    Lst_ctrlRegra[0].TwmsRegraCd = t_wmsRegraCd[0];
                    break;
                case "st_inativo":
                    if (!byte.TryParse(valor, out byte b))
                        Assert.Equal("Erro", $"O campo {campo.ToUpper()} com valor = {valor}, não é do tipo byte.");
                    t_wmsRegraCd[0].St_inativo = b;
                    break;
                default:
                    Assert.Equal("", $"{campo} desconhecido");
                    break;
            }
        }

        [Given(@"Chamar VerificarRegrasAssociadasAosProdutos")]
        public void GivenChamarVerificarRegrasAssociadasAosProdutos()
        {
            Produto.ProdutoGeralBll.VerificarRegrasAssociadasAosProdutos(Lst_ctrlRegra, lstErros,
                prepedidoDados.DadosCliente.Uf, prepedidoDados.DadosCliente.Tipo, 0);
        }

        [Given(@"Lista de CtrlRegra alterar registro de TwmsRegraCdUf do produto = ""(.*)"", campo ""(.*)"" = ""(.*)""")]
        public void GivenListaDeCtrlRegraAlterarRegistroDeTwmsRegraCdUfDoProdutoCampo(string produto, string campo, string valor)
        {
            var t_wmsRegraCdUf = Lst_ctrlRegra.Where(x => x.Produto == produto).Select(x => x.TwmsRegraCdXUf).ToList();

            switch (campo)
            {
                case "st_inativo":
                    if (!byte.TryParse(valor, out byte b))
                        Assert.Equal("Erro", $"O campo {campo.ToUpper()} com valor = {valor}, não é do tipo byte.");
                    t_wmsRegraCdUf[0].St_inativo = b;
                    break;
                default:
                    Assert.Equal("", $"{campo} desconhecido");
                    break;
            }
        }

        [Given(@"Lista de CtrlRegra alterar registro de TwmsRegraCdXUfXPessoa do produto = ""(.*)"", campo ""(.*)"" = ""(.*)""")]
        public void GivenListaDeCtrlRegraAlterarRegistroDeTwmsRegraCdXUfXPessoaDoProdutoCampo(string produto, string campo, string valor)
        {
            var t_wmsRegraCdXUfXPessoa = Lst_ctrlRegra.Where(x => x.Produto == produto).Select(x => x.TwmsRegraCdXUfXPessoa).ToList();

            switch (campo)
            {
                case "st_inativo":
                    if (!byte.TryParse(valor, out byte b))
                        Assert.Equal("Erro", $"O campo {campo.ToUpper()} com valor = {valor}, não é do tipo byte.");
                    t_wmsRegraCdXUfXPessoa[0].St_inativo = b;
                    break;
                case "spe_id_nfe_emitente":
                    if (!int.TryParse(valor, out int v))
                        Assert.Equal("Erro", $"O campo {campo.ToUpper()} com valor = {valor}, não é do tipo inteiro.");
                    t_wmsRegraCdXUfXPessoa[0].Spe_id_nfe_emitente = v;
                    break;
                default:
                    Assert.Equal("", $"{campo} desconhecido");
                    break;
            }
        }

        [Given(@"Lista de CtrlRegra alterar registro de TwmsCdXUfXPessoaXCd do produto = ""(.*)"" e id_nfe_emitente = ""(.*)"", campo ""(.*)"" = ""(.*)""")]
        public void GivenListaDeCtrlRegraAlterarRegistroDeTwmsCdXUfXPessoaXCdDoProdutoCampo(string produto, int id_nfe_emitente, string campo, string valor)
        {
            var lst_t_wmsCdXUfXPessoaXCd = Lst_ctrlRegra.Where(x => x.Produto == produto).Select(x => x.TwmsCdXUfXPessoaXCd).ToList();
            var t_wmsCdXUfXPessoaXCd = lst_t_wmsCdXUfXPessoaXCd[0].Where(x => x.Id_nfe_emitente == id_nfe_emitente).Select(x => x).FirstOrDefault();
            switch (campo)
            {
                case "st_inativo":
                    if (!byte.TryParse(valor, out byte b))
                        Assert.Equal("Erro", $"O campo {campo.ToUpper()} com valor = {valor}, não é do tipo byte.");
                    t_wmsCdXUfXPessoaXCd.St_inativo = b;
                    break;
                
                default:
                    Assert.Equal("", $"{campo} desconhecido");
                    break;
            }
        }


        [Then(@"Erro ""(.*)""")]
        public void ThenErro(string erro)
        {
            Assert.Contains(erro, lstErros);

        }

        private static readonly string PrepedidoDados = @"
        {
          ""CorHeader"": null,
          ""TextoHeader"": null,
          ""CanceladoData"": null,
          ""NumeroPrePedido"": null,
          ""DataHoraPedido"": null,
          ""Hora_Prepedido"": null,
          ""DadosCliente"": {
            ""Loja"": ""202"",
            ""Indicador_Orcamentista"": ""KONAR"",
            ""Vendedor"": null,
            ""Id"": ""000000645478"",
            ""Cnpj_Cpf"": ""35270445824"",
            ""Rg"": ""304480484"",
            ""Ie"": ""991.599.691.505"",
            ""Contribuinte_Icms_Status"": 2,
            ""Tipo"": ""PF"",
            ""Observacao_Filiacao"": """",
            ""Nascimento"": ""1984-06-19T03:00:00Z"",
            ""Sexo"": ""M"",
            ""Nome"": ""Gabriel Prada Teodoro"",
            ""ProdutorRural"": 2,
            ""Endereco"": ""Rua Francisco Pecoraro"",
            ""Numero"": ""97"",
            ""Complemento"": ""casa 01"",
            ""Bairro"": ""Água Fria"",
            ""Cidade"": ""São Paulo"",
            ""Uf"": ""SP"",
            ""Cep"": ""02408150"",
            ""DddResidencial"": ""11"",
            ""TelefoneResidencial"": ""25321634"",
            ""DddComercial"": """",
            ""TelComercial"": """",
            ""Ramal"": """",
            ""DddCelular"": ""11"",
            ""Celular"": ""981603313"",
            ""TelComercial2"": """",
            ""DddComercial2"": """",
            ""Ramal2"": """",
            ""Email"": ""gabriel.parada.teodoro@teste.com"",
            ""EmailXml"": """",
            ""Contato"": """"
          },
          ""EnderecoCadastroClientePrepedido"": {
            ""St_memorizacao_completa_enderecos"": false,
            ""Endereco_logradouro"": ""Rua Professor Fábio Fanucchi"",
            ""Endereco_numero"": ""420"",
            ""Endereco_complemento"": """",
            ""Endereco_bairro"": ""Jardim São Paulo(Zona Norte)"",
            ""Endereco_cidade"": ""São Paulo"",
            ""Endereco_uf"": ""SP"",
            ""Endereco_cep"": ""02045080"",
            ""Endereco_email"": ""gabrie@gmail.com"",
            ""Endereco_email_xml"": """",
            ""Endereco_nome"": ""Gabriel Prada Teodoro"",
            ""Endereco_ddd_res"": ""11"",
            ""Endereco_tel_res"": ""12213333"",
            ""Endereco_ddd_com"": """",
            ""Endereco_tel_com"": """",
            ""Endereco_ramal_com"": """",
            ""Endereco_ddd_cel"": ""11"",
            ""Endereco_tel_cel"": ""981603313"",
            ""Endereco_ddd_com_2"": """",
            ""Endereco_tel_com_2"": """",
            ""Endereco_ramal_com_2"": """",
            ""Endereco_tipo_pessoa"": ""PF"",
            ""Endereco_cnpj_cpf"": ""35270445824"",
            ""Endereco_contribuinte_icms_status"": 0,
            ""Endereco_produtor_rural_status"": 1,
            ""Endereco_ie"": """",
            ""Endereco_rg"": """",
            ""Endereco_contato"": """"
          },
          ""EnderecoEntrega"": {
            ""OutroEndereco"": false,
            ""EndEtg_endereco"": null,
            ""EndEtg_endereco_numero"": null,
            ""EndEtg_endereco_complemento"": null,
            ""EndEtg_bairro"": null,
            ""EndEtg_cidade"": null,
            ""EndEtg_uf"": null,
            ""EndEtg_cep"": null,
            ""EndEtg_cod_justificativa"": null,
            ""EndEtg_descricao_justificativa"": null,
            ""EndEtg_email"": null,
            ""EndEtg_email_xml"": null,
            ""EndEtg_nome"": null,
            ""EndEtg_ddd_res"": null,
            ""EndEtg_tel_res"": null,
            ""EndEtg_ddd_com"": null,
            ""EndEtg_tel_com"": null,
            ""EndEtg_ramal_com"": null,
            ""EndEtg_ddd_cel"": null,
            ""EndEtg_tel_cel"": null,
            ""EndEtg_ddd_com_2"": null,
            ""EndEtg_tel_com_2"": null,
            ""EndEtg_ramal_com_2"": null,
            ""EndEtg_tipo_pessoa"": null,
            ""EndEtg_cnpj_cpf"": null,
            ""EndEtg_contribuinte_icms_status"": 0,
            ""EndEtg_produtor_rural_status"": 0,
            ""EndEtg_ie"": null,
            ""EndEtg_rg"": null,
            ""St_memorizacao_completa_enderecos"": false
          },
          ""ListaProdutos"": [
            {
              ""Fabricante"": ""003"",
              ""Produto"": ""003220"",
              ""Descricao"": null,
              ""Qtde"": 2,
              ""CustoFinancFornecPrecoListaBase"": 626.58,
              ""Preco_Lista"": 659.60,
              ""Preco_Venda"": 659.60,
              ""Preco_NF"": 694.05,
              ""CustoFinancFornecCoeficiente"": 1.0527,
              ""Desc_Dado"": 0.0,
              ""VlTotalItem"": 0.0,
              ""VlTotalRA"": 0.0,
              ""Comissao"": null,
              ""TotalItemRA"": 1388.10,
              ""TotalItem"": 1319.20,
              ""Obs"": null,
              ""Permite_Ra_Status"": 1,
              ""BlnTemRa"": true,
              ""Qtde_estoque_total_disponivel"": null
            },
            {
              ""Fabricante"": ""003"",
              ""Produto"": ""003221"",
              ""Descricao"": null,
              ""Qtde"": 2,
              ""CustoFinancFornecPrecoListaBase"": 939.87,
              ""Preco_Lista"": 989.40,
              ""Preco_Venda"": 989.40,
              ""Preco_NF"": 1041.07,
              ""CustoFinancFornecCoeficiente"": 1.0527,
              ""Desc_Dado"": 0.0,
              ""VlTotalItem"": 0.0,
              ""VlTotalRA"": 0.0,
              ""Comissao"": null,
              ""TotalItemRA"": 2082.14,
              ""TotalItem"": 1978.80,
              ""Obs"": null,
              ""Permite_Ra_Status"": 1,
              ""BlnTemRa"": true,
              ""Qtde_estoque_total_disponivel"": null
            }
          ],
          ""TotalFamiliaParcelaRA"": 0.0,
          ""PermiteRAStatus"": 1,
          ""OpcaoPossuiRA"": null,
          ""CorTotalFamiliaRA"": null,
          ""PercRT"": null,
          ""Vl_total_NF"": 3470.24,
          ""Vl_total"": 3298.0,
          ""DetalhesPrepedido"": {
            ""Observacoes"": """",
            ""NumeroNF"": null,
            ""EntregaImediata"": ""2"",
            ""EntregaImediataData"": null,
            ""BemDeUso_Consumo"": 1,
            ""InstaladorInstala"": 2,
            ""GarantiaIndicador"": null,
            ""FormaDePagamento"": null,
            ""DescricaoFormaPagamento"": null,
            ""PrevisaoEntrega"": null
          },
          ""FormaPagto"": null,
          ""FormaPagtoCriacao"": {
            ""Rb_forma_pagto"": ""2"",
            ""Op_av_forma_pagto"": null,
            ""Op_pu_forma_pagto"": null,
            ""C_pu_valor"": null,
            ""C_pu_vencto_apos"": null,
            ""C_pc_qtde"": 1,
            ""C_pc_valor"": 3470.24,
            ""C_pc_maquineta_qtde"": null,
            ""C_pc_maquineta_valor"": null,
            ""Op_pce_entrada_forma_pagto"": null,
            ""C_pce_entrada_valor"": null,
            ""Op_pce_prestacao_forma_pagto"": null,
            ""C_pce_prestacao_qtde"": null,
            ""C_pce_prestacao_valor"": null,
            ""C_pce_prestacao_periodo"": null,
            ""Op_pse_prim_prest_forma_pagto"": null,
            ""C_pse_prim_prest_valor"": null,
            ""C_pse_prim_prest_apos"": null,
            ""Op_pse_demais_prest_forma_pagto"": null,
            ""C_pse_demais_prest_qtde"": null,
            ""C_pse_demais_prest_valor"": null,
            ""C_pse_demais_prest_periodo"": null,
            ""C_forma_pagto"": null,
            ""Descricao_meio_pagto"": null,
            ""Tipo_parcelamento"": 0,
            ""CustoFinancFornecTipoParcelamento"": ""SE"",
            ""CustoFinancFornecQtdeParcelas"": 1,
            ""Qtde_Parcelas_Para_Exibicao"": 0
          },
          ""St_Orc_Virou_Pedido"": false,
          ""NumeroPedido"": null
        }
        ";
    }
}
