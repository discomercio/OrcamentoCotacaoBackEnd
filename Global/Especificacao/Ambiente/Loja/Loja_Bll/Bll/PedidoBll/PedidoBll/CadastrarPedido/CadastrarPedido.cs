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
        protected override void AbstractListaDeItensComXitens(int numeroItens)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensComXitens(numeroItens, this);
            numeroItens = numeroItens < 0 ? 0 : numeroItens;
            numeroItens = numeroItens > 100 ? 100 : numeroItens;
            var lp = pedidoDto.ListaProdutos;
            while (lp.Count < numeroItens)
                lp.Add(new global::Loja.Bll.Dto.PedidoDto.DetalhesPedido.PedidoProdutosDtoPedido());
            while (lp.Count > numeroItens)
                lp.RemoveAt(lp.Count - 1);
        }
        protected override void AbstractRecalcularTotaisDoPedido()
        {
            decimal totalCompare = 0;
            decimal totalRaCompare = 0;
            foreach (var x in pedidoDto.ListaProdutos)
            {
                totalCompare += Math.Round((decimal)(x.Preco_Venda * x.Qtde ?? (short)0), 2);
                totalRaCompare += Math.Round((decimal)(x.Preco_NF * x.Qtde ?? (short)0), 2);
            }
            pedidoDto.VlTotalDestePedido = totalCompare;
            pedidoDto.ValorTotalDestePedidoComRA = totalRaCompare;
        }


        protected override List<string> AbstractListaErros()
        {
            var ret1 = pedidoBll.CadastrarPedido(pedidoDto, lojaUsuario, usuario, vendedorExterno);
            var ret = ret1.Result;
            return ret.ListaErros;
        }
        protected override void AbstractDeixarFormaDePagamentoConsistente()
        {
            Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido.EstaticoDeixarFormaDePagamentoConsistente(pedidoDto);
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

        protected override void AbstractListaDeItensInformo(int numeroItem, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensInformo(numeroItem, campo, valor, this);
            var item = pedidoDto.ListaProdutos[numeroItem];
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, item))
                return;
            Assert.Equal("", $"{campo} desconhecido na rotina Especificacao.Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.ListaDeItensInformo");
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
