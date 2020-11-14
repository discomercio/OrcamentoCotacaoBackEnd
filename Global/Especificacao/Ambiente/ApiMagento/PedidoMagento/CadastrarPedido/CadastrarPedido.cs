using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using PrepedidoUnisBusiness.UnisDto.FormaPagtoUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
{
    class CadastrarPedido : Testes.Pedido.IPedidoPassosComuns
    {
        private readonly global::ApiMagento.Controllers.PedidoMagentoController pedidoMagentoController;
        private readonly Testes.Utils.LogTestes.LogTestes logTestes = Testes.Utils.LogTestes.LogTestes.GetInstance();
        readonly global::MagentoBusiness.UtilsMagento.ConfiguracaoApiMagento configuracaoApiMagento = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<global::MagentoBusiness.UtilsMagento.ConfiguracaoApiMagento>();

        public CadastrarPedido()
        {
            pedidoMagentoController = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<global::ApiMagento.Controllers.PedidoMagentoController>();
        }

        private MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto pedidoMagentoDto = new MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto();

        public void GivenDadoBase()
        {
            if (ignorarFeature) return;
            pedidoMagentoDto = CadastrarPedidoDados.PedidoBase();
            pedidoMagentoDto.TokenAcesso = Ambiente.ApiMagento.InjecaoDependencias.TokenAcessoApiMagento();
        }
        public void GivenDadoBaseComEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            pedidoMagentoDto = CadastrarPedidoDados.PedidoBaseComEnderecoDeEntrega();
            pedidoMagentoDto.TokenAcesso = Ambiente.ApiMagento.InjecaoDependencias.TokenAcessoApiMagento();
        }

        public void GivenPedidoBaseComEnderecoDeEntrega()
        {
            GivenDadoBaseComEnderecoDeEntrega();
        }

        public void WhenPedidoBase()
        {
            GivenDadoBase();
        }

        public void WhenInformo(string p0, string p1)
        {
            if (ignorarFeature) return;
            switch (p0)
            {
                case "appsettings.Orcamentista":
                    configuracaoApiMagento.DadosOrcamentista.Orcamentista = p1;
                    break;

                case "TokenAcesso":
                    pedidoMagentoDto.TokenAcesso = p1;
                    break;

                case "CPF/CNPJ":
                    pedidoMagentoDto.Cnpj_Cpf = p1;
                    pedidoMagentoDto.EnderecoCadastralCliente.Endereco_cnpj_cpf = p1;
                    break;
                case "pedidoMagentoDto.Cnpj_Cpf":
                    pedidoMagentoDto.Cnpj_Cpf = p1;
                    break;
                case "EnderecoCadastralCliente.Endereco_cnpj_cpf":
                    pedidoMagentoDto.EnderecoCadastralCliente.Endereco_cnpj_cpf = p1;
                    break;

                case "EnderecoCadastralCliente.Endereco_tipo_pessoa":
                    pedidoMagentoDto.EnderecoCadastralCliente.Endereco_tipo_pessoa = p1;
                    break;

                case "OutroEndereco":
                    if (bool.TryParse(p1, out bool valor))
                        pedidoMagentoDto.OutroEndereco = valor;
                    break;
                //endetg
                case "EndEtg_bairro":
                    pedidoMagentoDto.EnderecoEntrega ??= new MagentoBusiness.MagentoDto.ClienteMagentoDto.EnderecoEntregaClienteMagentoDto();
                    pedidoMagentoDto.EnderecoEntrega.EndEtg_bairro = p1;
                    break;
                case "EndEtg_cep":
                    pedidoMagentoDto.EnderecoEntrega ??= new MagentoBusiness.MagentoDto.ClienteMagentoDto.EnderecoEntregaClienteMagentoDto();
                    pedidoMagentoDto.EnderecoEntrega.EndEtg_cep = p1;
                    break;
                case "EndEtg_endereco_numero":
                    pedidoMagentoDto.EnderecoEntrega ??= new MagentoBusiness.MagentoDto.ClienteMagentoDto.EnderecoEntregaClienteMagentoDto();
                    pedidoMagentoDto.EnderecoEntrega.EndEtg_endereco_numero = p1;
                    break;
                case "EndEtg_uf":
                    pedidoMagentoDto.EnderecoEntrega ??= new MagentoBusiness.MagentoDto.ClienteMagentoDto.EnderecoEntregaClienteMagentoDto();
                    pedidoMagentoDto.EnderecoEntrega.EndEtg_uf = p1;
                    break;
                case "EndEtg_endereco":
                    pedidoMagentoDto.EnderecoEntrega ??= new MagentoBusiness.MagentoDto.ClienteMagentoDto.EnderecoEntregaClienteMagentoDto();
                    pedidoMagentoDto.EnderecoEntrega.EndEtg_endereco = p1;
                    break;
                case "EndEtg_cidade":
                    pedidoMagentoDto.EnderecoEntrega ??= new MagentoBusiness.MagentoDto.ClienteMagentoDto.EnderecoEntregaClienteMagentoDto();
                    pedidoMagentoDto.EnderecoEntrega.EndEtg_cidade = p1;
                    break;
                case "EndEtg_obs":
                    /* nao temos justificativa
                    pedidoMagentoDto.EnderecoEntrega ??= new MagentoBusiness.MagentoDto.ClienteMagentoDto.EnderecoEntregaClienteMagentoDto();
                    pedidoMagentoDto.EnderecoEntrega.EndEtg_cod_justificativa = p1;
                    */
                    break;


                case "InfCriacaoPedido.Pedido_bs_x_ac":
                    pedidoMagentoDto.InfCriacaoPedido.Pedido_bs_x_ac = p1;
                    break;
                case "InfCriacaoPedido.Pedido_bs_x_marketplace":
                    pedidoMagentoDto.InfCriacaoPedido.Pedido_bs_x_marketplace = p1;
                    break;
                case "InfCriacaoPedido.Marketplace_codigo_origem":
                    pedidoMagentoDto.InfCriacaoPedido.Marketplace_codigo_origem = p1;
                    break;

                default:
                    Assert.Equal("", $"{p0} desconhecido na rotina WhenInformo");
                    break;
            }
        }
        public void ThenErroStatusCode(int statusCode)
        {
            if (ignorarFeature) return;
            logTestes.LogMensagem("pedidoMagentoController.CadastrarPedido");
            Microsoft.AspNetCore.Mvc.ActionResult<MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto> ret
                = pedidoMagentoController.CadastrarPedido(pedidoMagentoDto).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;
            Testes.Utils.StatusCodes.TestarStatusCode(statusCode, res);
        }

        public void ThenErro(string p0)
        {
            if (ignorarFeature) return;
            ThenErro(p0, true);
        }
        public void ThenSemErro(string p0)
        {
            if (ignorarFeature) return;
            ThenErro(p0, false);
        }
        public void ThenSemNenhumErro()
        {
            if (ignorarFeature) return;
            ThenErro(null, false);
        }

        private void ThenErro(string? erro, bool erroDeveExistir)
        {
            if (ignorarFeature) return;
            if (erro != null)
                erro = Testes.Utils.MapeamentoMensagens.MapearMensagem(this.GetType().FullName, erro);

            logTestes.LogMensagem($"pedidoMagentoController.CadastrarPedido ThenErro({erro}, {erroDeveExistir})");
            Microsoft.AspNetCore.Mvc.ActionResult<MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto> ret
                = pedidoMagentoController.CadastrarPedido(pedidoMagentoDto).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;

            //deve ter retornado 200
            if (res.GetType() != typeof(Microsoft.AspNetCore.Mvc.OkObjectResult))
                Assert.Equal("", "Tipo não é OkObjectResult");

            MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto pedidoResultadoMagentoDto
                = (MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto)((Microsoft.AspNetCore.Mvc.OkObjectResult)res).Value;

            if (erroDeveExistir)
            {
                if (!pedidoResultadoMagentoDto.ListaErros.Contains(erro ?? ""))
                    logTestes.LogMensagem($"Erro: {erro} em {string.Join(" - ", pedidoResultadoMagentoDto.ListaErros)}");
                Assert.Contains(erro, pedidoResultadoMagentoDto.ListaErros);
            }
            else
            {
                if (erro == null)
                {
                    if (pedidoResultadoMagentoDto.ListaErros.Count != 0)
                        logTestes.LogMensagem($"Erro: {erro} em {string.Join(" - ", pedidoResultadoMagentoDto.ListaErros)}");
                    Assert.Empty(pedidoResultadoMagentoDto.ListaErros);
                }
                else
                    Assert.DoesNotContain(erro, pedidoResultadoMagentoDto.ListaErros);
            }

        }

        private bool ignorarFeature = false;
        public void GivenIgnorarFeatureNoAmbiente2(string p0)
        {
            Testes.Pedido.PedidoPassosComuns.IgnorarFeatureNoAmbiente(p0, ref ignorarFeature, this.GetType());
        }
    }
}
