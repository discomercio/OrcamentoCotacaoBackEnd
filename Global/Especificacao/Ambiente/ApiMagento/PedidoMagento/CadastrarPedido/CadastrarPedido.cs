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
    class CadastrarPedido : Testes.Pedido.HelperImplementacaoPedido
    {
        private readonly global::ApiMagento.Controllers.PedidoMagentoController pedidoMagentoController;
        readonly global::MagentoBusiness.UtilsMagento.ConfiguracaoApiMagento configuracaoApiMagento = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<global::MagentoBusiness.UtilsMagento.ConfiguracaoApiMagento>();

        public CadastrarPedido()
        {
            pedidoMagentoController = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<global::ApiMagento.Controllers.PedidoMagentoController>();
        }

        private MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto pedidoMagentoDto = new MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto();


        public void ThenErroStatusCode(int statusCode)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.ErroStatusCode(statusCode, this);
            ActionResult res = AcessarControladorMagento();
            Testes.Utils.StatusCodes.TestarStatusCode(statusCode, res);
        }

        public Microsoft.AspNetCore.Mvc.ActionResult<MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto>? UltimoAcessoFeito { get; private set; } = null;
        private ActionResult AcessarControladorMagento()
        {
            Testes.Utils.LogTestes.LogOperacoes2.ChamadaController(pedidoMagentoController.GetType(), "CadastrarPedido", this);
            UltimoAcessoFeito = pedidoMagentoController.CadastrarPedido(pedidoMagentoDto).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = UltimoAcessoFeito.Result;
            return res;
        }

        protected override List<string> AbstractListaErros()
        {
            ActionResult res = AcessarControladorMagento();

            //deve ter retornado 200
            if (res.GetType() != typeof(Microsoft.AspNetCore.Mvc.OkObjectResult))
                Assert.Equal("", "Tipo não é OkObjectResult");

            MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto pedidoResultadoMagentoDto
                = (MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto)((Microsoft.AspNetCore.Mvc.OkObjectResult)res).Value;

            return pedidoResultadoMagentoDto.ListaErros;
        }

        protected override void AbstractDadoBase()
        {
            pedidoMagentoDto = CadastrarPedidoDados.PedidoBase();
            pedidoMagentoDto.TokenAcesso = Ambiente.ApiMagento.InjecaoDependencias.TokenAcessoApiMagento();
        }

        protected override void AbstractDadoBaseComEnderecoDeEntrega()
        {
            pedidoMagentoDto = CadastrarPedidoDados.PedidoBaseComEnderecoDeEntrega();
            pedidoMagentoDto.TokenAcesso = Ambiente.ApiMagento.InjecaoDependencias.TokenAcessoApiMagento();
        }

        protected override void AbstractDadoBaseClientePJComEnderecoDeEntrega()
        {
            pedidoMagentoDto = CadastrarPedidoDados.PedidoBaseClientePjComEnderecoDeEntrega();
            pedidoMagentoDto.TokenAcesso = Ambiente.ApiMagento.InjecaoDependencias.TokenAcessoApiMagento();
        }
        protected override void AbstractDadoBaseClientePF()
        {
            pedidoMagentoDto = CadastrarPedidoDados.PedidoBaseClientePF();
            pedidoMagentoDto.TokenAcesso = Ambiente.ApiMagento.InjecaoDependencias.TokenAcessoApiMagento();
        }
        protected override void AbstractDadoBaseClientePJ()
        {
            pedidoMagentoDto = CadastrarPedidoDados.PedidoBaseClientePJ();
            pedidoMagentoDto.TokenAcesso = Ambiente.ApiMagento.InjecaoDependencias.TokenAcessoApiMagento();
        }

        protected override void AbstractInformo(string p0, string p1)
        {
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
                case "EndEtg_nome":
                    pedidoMagentoDto.EnderecoEntrega ??= new MagentoBusiness.MagentoDto.ClienteMagentoDto.EnderecoEntregaClienteMagentoDto();
                    pedidoMagentoDto.EnderecoEntrega.EndEtg_nome = p1;
                    break;
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
                case "EndEtg_endereco_complemento":
                    pedidoMagentoDto.EnderecoEntrega ??= new MagentoBusiness.MagentoDto.ClienteMagentoDto.EnderecoEntregaClienteMagentoDto();
                    pedidoMagentoDto.EnderecoEntrega.EndEtg_endereco_complemento = p1;
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
                    Assert.Equal("", $"{p0} desconhecido na rotina Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.AbstractInformo");
                    break;
            }
        }

    }
}
