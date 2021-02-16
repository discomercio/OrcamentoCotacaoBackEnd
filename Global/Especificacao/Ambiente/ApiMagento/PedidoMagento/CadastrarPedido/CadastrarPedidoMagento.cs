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
    //o nome do arquivo é diferente do nome da classe para ficar mais fácil de localizar no "Find All References". É que mistura do da loja e do magento.
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

        public MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto? UltimoPedidoResultadoMagentoDto()
        {
            var temp = UltimoAcessoFeito?.Result;
            if (temp == null)
                return null;
            MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto pedidoResultadoMagentoDto
            = (MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto)((Microsoft.AspNetCore.Mvc.OkObjectResult)temp).Value;
            return pedidoResultadoMagentoDto;
        }
        public Microsoft.AspNetCore.Mvc.ActionResult<MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto>? UltimoAcessoFeito { get; private set; } = null;
        private ActionResult AcessarControladorMagento()
        {
            Testes.Utils.LogTestes.LogOperacoes2.ChamadaController(pedidoMagentoController.GetType(), "CadastrarPedido", this);
            UltimoAcessoFeito = pedidoMagentoController.CadastrarPedido(pedidoMagentoDto).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = UltimoAcessoFeito.Result;
            return res;
        }

        protected override void AbstractRecalcularTotaisDoPedido()
        {
            decimal totalCompare = 0;
            decimal totalRaCompare = 0;
            foreach (var x in pedidoMagentoDto.ListaProdutos)
            {
                totalCompare += Math.Round((decimal)(x.Preco_Venda * x.Qtde), 2);
                totalRaCompare += Math.Round((decimal)(x.Preco_NF * x.Qtde), 2);
            }
            pedidoMagentoDto.VlTotalDestePedido = totalCompare;
            //nao temos este campo: pedidoMagentoDto.ValorTotalDestePedidoComRA = totalRaCompare;
        }
        protected override void AbstractListaDeItensComXitens(int numeroItens)
        {
            MagentoListaDeItensComXitens(numeroItens);
        }

        public void MagentoListaDeItensComXitens(int numeroItens)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensComXitens(numeroItens, this);
            numeroItens = numeroItens < 0 ? 0 : numeroItens;
            numeroItens = numeroItens > 100 ? 100 : numeroItens;
            var lp = pedidoMagentoDto.ListaProdutos;
            while (lp.Count < numeroItens)
                lp.Add(new MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoProdutoMagentoDto());
            while (lp.Count > numeroItens)
                lp.RemoveAt(lp.Count - 1);
        }

        protected override void AbstractLimparEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.LimparEnderecoDeEntrega(this);
            pedidoMagentoDto.EnderecoEntrega = new MagentoBusiness.MagentoDto.ClienteMagentoDto.EnderecoEntregaClienteMagentoDto();
        }
        protected override void AbstractLimparDadosCadastraisEEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.LimparDadosCadastraisEEnderecoDeEntrega(this);
            pedidoMagentoDto.EnderecoEntrega = new MagentoBusiness.MagentoDto.ClienteMagentoDto.EnderecoEntregaClienteMagentoDto();
            pedidoMagentoDto.EnderecoCadastralCliente = new MagentoBusiness.MagentoDto.ClienteMagentoDto.EnderecoCadastralClienteMagentoDto();
        }

        protected override void AbstractDeixarFormaDePagamentoConsistente()
        {
            Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido.EstaticoDeixarFormaDePagamentoConsistente(pedidoMagentoDto);
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

        protected override void AbstractListaDeItensInformo(int numeroItem, string campo, string valor)
        {
            MagentoListaDeItensInformo(numeroItem, campo, valor);
        }
        public void MagentoListaDeItensInformo(int numeroItem, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensInformo(numeroItem, campo, valor, this);
            var item = pedidoMagentoDto.ListaProdutos[numeroItem];
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, item))
                return;
            Assert.Equal("", $"{campo} desconhecido na rotina Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.ListaDeItensInformo");
        }

        protected override void AbstractInformo(string campo, string valor)
        {
            switch (campo)
            {
                case "limitePedidos.pedidoIgual":
                    configuracaoApiMagento.LimitePedidos.LimitePedidosExatamenteIguais_Numero = int.Parse(valor);
                    return;
                case "limitePedidos.porCpf":
                    configuracaoApiMagento.LimitePedidos.LimitePedidosMesmoCpfCnpj_Numero = int.Parse(valor);
                    return;
                case "appsettings.Indicador":
                    configuracaoApiMagento.DadosIndicador.Indicador = valor;
                    return;
                case "appsettings.Loja":
                    configuracaoApiMagento.DadosIndicador.Loja = valor;
                    return;

                case "TokenAcesso":
                    pedidoMagentoDto.TokenAcesso = valor;
                    return;

                case "CPF/CNPJ":
                case "Cnpj_Cpf":
                case "cnpj_cpf":
                    pedidoMagentoDto.Cnpj_Cpf = valor;
                    pedidoMagentoDto.EnderecoCadastralCliente.Endereco_cnpj_cpf = valor;
                    pedidoMagentoDto.EnderecoEntrega ??= new MagentoBusiness.MagentoDto.ClienteMagentoDto.EnderecoEntregaClienteMagentoDto();
                    pedidoMagentoDto.EnderecoEntrega.EndEtg_cnpj_cpf = valor;
                    return;
                case "pedidoMagentoDto.Cnpj_Cpf":
                    pedidoMagentoDto.Cnpj_Cpf = valor;
                    return;
                case "EnderecoCadastralCliente.Endereco_cnpj_cpf":
                    pedidoMagentoDto.EnderecoCadastralCliente.Endereco_cnpj_cpf = valor;
                    pedidoMagentoDto.EnderecoEntrega ??= new MagentoBusiness.MagentoDto.ClienteMagentoDto.EnderecoEntregaClienteMagentoDto();
                    pedidoMagentoDto.EnderecoEntrega.EndEtg_cnpj_cpf = valor;
                    return;

                case "EnderecoCadastralCliente.Endereco_tipo_pessoa":
                    pedidoMagentoDto.EnderecoCadastralCliente.Endereco_tipo_pessoa = valor;
                    pedidoMagentoDto.EnderecoEntrega ??= new MagentoBusiness.MagentoDto.ClienteMagentoDto.EnderecoEntregaClienteMagentoDto();
                    pedidoMagentoDto.EnderecoEntrega.EndEtg_tipo_pessoa = valor;
                    return;
            }

            //acertos em campos
            if (campo == "vl_total_NF")
                campo = "VlTotalDestePedido";

            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, pedidoMagentoDto))
                return;
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, pedidoMagentoDto.FormaPagtoCriacao))
                return;
            pedidoMagentoDto.EnderecoEntrega ??= new MagentoBusiness.MagentoDto.ClienteMagentoDto.EnderecoEntregaClienteMagentoDto();
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, pedidoMagentoDto.EnderecoEntrega))
                return;
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, pedidoMagentoDto.EnderecoCadastralCliente))
                return;

            switch (campo)
            {
                case "OutroEndereco":
                    if (bool.TryParse(valor, out bool valorBool))
                        pedidoMagentoDto.OutroEndereco = valorBool;
                    return;

                case "InfCriacaoPedido.Pedido_bs_x_ac":
                    pedidoMagentoDto.InfCriacaoPedido.Pedido_bs_x_ac = valor;
                    return;
                case "InfCriacaoPedido.Pedido_bs_x_marketplace":
                    pedidoMagentoDto.InfCriacaoPedido.Pedido_bs_x_marketplace = valor;
                    return;
                case "InfCriacaoPedido.Marketplace_codigo_origem":
                    pedidoMagentoDto.InfCriacaoPedido.Marketplace_codigo_origem = valor;
                    return;

                default:
                    Assert.Equal("", $"{campo} desconhecido na rotina Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.AbstractInformo");
                    break;
            }
        }

        protected override string? AbstractPedidoPaiGerado()
        {
            var ultimo = UltimoPedidoResultadoMagentoDto();
            if (ultimo == null)
                return null;

            return ultimo.IdPedidoCadastrado;
        }

        protected override List<string> AbstractPedidosFilhotesGerados()
        {
            var ultimo = UltimoPedidoResultadoMagentoDto();
            if (ultimo == null)
                return new List<string>();

            return ultimo.IdsPedidosFilhotes;
        }
    }
}
