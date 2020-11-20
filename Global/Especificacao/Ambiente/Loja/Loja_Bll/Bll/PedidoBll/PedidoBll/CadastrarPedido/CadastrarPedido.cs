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

        protected override void AbstractInformo(string p0, string p1)
        {
            switch (p0)
            {
                case "CPF/CNPJ":
                    pedidoDto.DadosCliente.Cnpj_Cpf = p1;
                    break;
                case "EnderecoCadastralCliente.Endereco_cnpj_cpf":
                    pedidoDto.DadosCliente.Cnpj_Cpf = p1;
                    break;

                case "EnderecoCadastralCliente.Endereco_tipo_pessoa":
                    pedidoDto.DadosCliente.Tipo = p1;
                    break;

                case "OutroEndereco":
                    if (bool.TryParse(p1, out bool valor))
                        pedidoDto.EnderecoEntrega.OutroEndereco = valor;
                    break;
                //endetg
                case "EndEtg_bairro":
                    pedidoDto.EnderecoEntrega ??= new global::Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro();
                    pedidoDto.EnderecoEntrega.EndEtg_bairro = p1;
                    break;
                case "EndEtg_cep":
                    pedidoDto.EnderecoEntrega ??= new global::Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro();
                    pedidoDto.EnderecoEntrega.EndEtg_cep = p1;
                    break;
                case "EndEtg_endereco_numero":
                    pedidoDto.EnderecoEntrega ??= new global::Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro();
                    pedidoDto.EnderecoEntrega.EndEtg_endereco_numero = p1;
                    break;
                case "EndEtg_uf":
                    pedidoDto.EnderecoEntrega ??= new global::Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro();
                    pedidoDto.EnderecoEntrega.EndEtg_uf = p1;
                    break;
                case "EndEtg_endereco":
                    pedidoDto.EnderecoEntrega ??= new global::Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro();
                    pedidoDto.EnderecoEntrega.EndEtg_endereco = p1;
                    break;
                case "EndEtg_cidade":
                    pedidoDto.EnderecoEntrega ??= new global::Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro();
                    pedidoDto.EnderecoEntrega.EndEtg_cidade = p1;
                    break;
                case "EndEtg_obs":
                    pedidoDto.EnderecoEntrega ??= new global::Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro();
                    pedidoDto.EnderecoEntrega.EndEtg_cod_justificativa = p1;
                    break;

                default:
                    Assert.Equal("", $"{p0} desconhecido na rotina Especificacao.Ambiente.Loja.Loja.Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.AbstractInformo");
                    break;
            }
        }

    }
}
