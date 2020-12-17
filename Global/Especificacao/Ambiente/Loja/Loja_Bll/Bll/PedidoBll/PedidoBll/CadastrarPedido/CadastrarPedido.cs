using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido
{
    class CadastrarPedido : Testes.Pedido.HelperImplementacaoPedido
    {
        private readonly global::Loja.Bll.PedidoBll.PedidoBll pedidoBll;

        public CadastrarPedido()
        {
            pedidoBll = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<global::Loja.Bll.PedidoBll.PedidoBll>();
        }

        private global::Loja.Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto pedidoDto = new global::Loja.Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto();
        private string lojaUsuario = "";
        private string usuario = "";
        private bool vendedorExterno = false;

        protected override List<string> AbstractListaErros()
        {
            var ret1 = pedidoBll.CadastrarPedido(pedidoDto, lojaUsuario, usuario, vendedorExterno);
            var ret = ret1.Result;
            return ret.ListaErros;
        }

        protected override void AbstractDadoBase()
        {
            pedidoDto = CadastrarPedidoDados.PedidoBase(out lojaUsuario, out usuario, out vendedorExterno);
        }
        protected override void AbstractDadoBaseClientePF()
        {
            pedidoDto = CadastrarPedidoDados.PedidoBaseClientePF(out lojaUsuario, out usuario, out vendedorExterno);
        }
        protected override void AbstractDadoBaseClientePJ()
        {
            pedidoDto = CadastrarPedidoDados.PedidoBaseClientePJ(out lojaUsuario, out usuario, out vendedorExterno);
        }

        protected override void AbstractDadoBaseComEnderecoDeEntrega()
        {
            pedidoDto = CadastrarPedidoDados.PedidoBaseComEnderecoDeEntrega(out lojaUsuario, out usuario, out vendedorExterno);
        }
        protected override void AbstractDadoBaseClientePJComEnderecoDeEntrega()
        {
            pedidoDto = CadastrarPedidoDados.PedidoBaseClientePJComEnderecoDeEntrega(out lojaUsuario, out usuario, out vendedorExterno);
        }

        protected override void AbstractInformo(string campo, string valor)
        {
            switch (campo)
            {
                case "CPF/CNPJ":
                    pedidoDto.DadosCliente.Cnpj_Cpf = valor;
                    return;
                case "EnderecoCadastralCliente.Endereco_cnpj_cpf":
                    pedidoDto.DadosCliente.Cnpj_Cpf = valor;
                    return;

                case "EnderecoCadastralCliente.Endereco_tipo_pessoa":
                    pedidoDto.DadosCliente.Tipo = valor;
                    return;
            }

            //acertos em campos
            if (campo == "vl_total_NF")
                campo = "ValorTotalDestePedidoComRA";

            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, pedidoDto))
                return;
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, pedidoDto.FormaPagtoCriacao))
                return;
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, pedidoDto.DadosCliente))
                return;
            pedidoDto.EnderecoEntrega ??= new global::Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro();
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, pedidoDto.EnderecoEntrega))
                return;

            switch (campo)
            {
                case "EndEtg_obs":
                    pedidoDto.EnderecoEntrega ??= new global::Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro();
                    pedidoDto.EnderecoEntrega.EndEtg_cod_justificativa = valor;
                    break;

                default:
                    Assert.Equal("", $"{campo} desconhecido na rotina Especificacao.Ambiente.Loja.Loja.Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.AbstractInformo");
                    break;
            }
        }

    }
}
