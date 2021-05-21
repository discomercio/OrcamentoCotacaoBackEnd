using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido
{
    //o nome do arquivo é diferente do nome da classe para ficar mais fácil de localizar no "Find All References". É que mistura do da loja e do magento.
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

        protected override void AbstractLimparEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.LimparEnderecoDeEntrega(this);
            pedidoDto.EnderecoEntrega = new global::Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro();
        }
        protected override void AbstractLimparDadosCadastraisEEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.LimparDadosCadastraisEEnderecoDeEntrega(this);
            pedidoDto.EnderecoEntrega = new global::Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro();
            pedidoDto.DadosCliente = new global::Loja.Bll.Dto.ClienteDto.DadosClienteCadastroDto();
        }

        private Pedido.Dados.Criacao.PedidoCriacaoRetornoDados? UltimoPedidoCriacaoRetornoDados = null;
        protected override List<string> AbstractListaErros()
        {
            var ret1 = pedidoBll.CadastrarPedido(pedidoDto, lojaUsuario, usuario, vendedorExterno,
                100000, 100000, 100000, 100000,
                0.1M, 0.1M, 12);
            UltimoPedidoCriacaoRetornoDados = ret1.Result;
            List<string> erros = new List<string>();
            erros.AddRange(UltimoPedidoCriacaoRetornoDados.ListaErros);
            erros.AddRange(UltimoPedidoCriacaoRetornoDados.ListaErrosValidacao);
            return erros;
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
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, pedidoDto.DetalhesNF))
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

                //do DadosCliente
                case "Endereco_nome":
                    pedidoDto.DadosCliente.Nome = valor;
                    break;
                case "Endereco_logradouro":
                    pedidoDto.DadosCliente.Endereco = valor;
                    break;
                case "Endereco_numero":
                    pedidoDto.DadosCliente.Numero = valor;
                    break;
                case "Endereco_complemento":
                    pedidoDto.DadosCliente.Complemento = valor;
                    break;
                case "Endereco_bairro":
                    pedidoDto.DadosCliente.Bairro = valor;
                    break;
                case "Endereco_cidade":
                    pedidoDto.DadosCliente.Cidade = valor;
                    break;
                case "Endereco_contato":
                    pedidoDto.DadosCliente.Contato = valor;
                    break;
                case "Endereco_rg":
                    pedidoDto.DadosCliente.Rg = valor;
                    break;
                case "Endereco_Email":
                    pedidoDto.DadosCliente.Email = valor;
                    break;
                case "Endereco_EmailXml":
                    pedidoDto.DadosCliente.EmailXml = valor;
                    break;


                default:
                    Assert.Equal("", $"{campo} desconhecido na rotina Especificacao.Ambiente.Loja.Loja.Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.AbstractInformo");
                    break;
            }
        }

        protected override string? AbstractPedidoPaiGerado()
        {
            var ultimo = UltimoPedidoCriacaoRetornoDados;
            if (ultimo == null)
                return null;

            return ultimo.Id;
        }

        protected override List<string> AbstractPedidosFilhotesGerados()
        {
            var ultimo = UltimoPedidoCriacaoRetornoDados;
            if (ultimo == null)
                return new List<string>();

            return ultimo.ListaIdPedidosFilhotes;
        }

        protected override List<string> AbstractPedidosGerados()
        {
            var ultimo = UltimoPedidoCriacaoRetornoDados;
            if (ultimo == null)
                return new List<string>();

            List<string> lstPedidos = new List<string>();
            if (!string.IsNullOrEmpty(ultimo.Id))
                lstPedidos.Add(ultimo.Id);

            if (ultimo.ListaIdPedidosFilhotes.Count > 0)
            {
                foreach (var filhotes in ultimo.ListaIdPedidosFilhotes)
                {
                    lstPedidos.Add(filhotes);
                }
            }

            return lstPedidos;
        }
    }
}
