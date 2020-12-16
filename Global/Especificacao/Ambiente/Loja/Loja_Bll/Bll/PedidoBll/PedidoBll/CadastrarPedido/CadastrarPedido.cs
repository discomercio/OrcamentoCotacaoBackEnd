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
                case "EndEtg_nome":
                    pedidoDto.EnderecoEntrega ??= new global::Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro();
                    pedidoDto.EnderecoEntrega.EndEtg_nome = p1;
                    break;
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
                case "EndEtg_endereco_complemento":
                    pedidoDto.EnderecoEntrega ??= new global::Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro();
                    pedidoDto.EnderecoEntrega.EndEtg_endereco_complemento = p1;
                    break;
                case "EndEtg_cidade":
                    pedidoDto.EnderecoEntrega ??= new global::Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro();
                    pedidoDto.EnderecoEntrega.EndEtg_cidade = p1;
                    break;
                case "EndEtg_obs":
                    pedidoDto.EnderecoEntrega ??= new global::Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro();
                    pedidoDto.EnderecoEntrega.EndEtg_cod_justificativa = p1;
                    break;

                case "EndEtg_ie":
                    pedidoDto.EnderecoEntrega ??= new global::Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro();
                    pedidoDto.EnderecoEntrega.EndEtg_ie = p1;
                    break;
                case "EndEtg_contribuinte_icms_status":
                    InfraBanco.Constantes.Constantes.ContribuinteICMS valorContribuinteICMS;
                    switch (p1)
                    {
                        case "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL":
                            valorContribuinteICMS = InfraBanco.Constantes.Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL;
                            break;
                        case "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO":
                            valorContribuinteICMS = InfraBanco.Constantes.Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO;
                            break;
                        case "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM":
                            valorContribuinteICMS = InfraBanco.Constantes.Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM;
                            break;
                        case "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO":
                            valorContribuinteICMS = InfraBanco.Constantes.Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO;
                            break;
                        default:
                            Assert.Equal("", $"{p1} desconhecido em Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.WhenInformo em EndEtg_contribuinte_icms_status");
                            valorContribuinteICMS = InfraBanco.Constantes.Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL;
                            break;
                    }

                    pedidoDto.EnderecoEntrega.EndEtg_contribuinte_icms_status = (byte)valorContribuinteICMS;
                    break;

                case "EndEtg_produtor_rural_status":
                    InfraBanco.Constantes.Constantes.ProdutorRual valorProdutorRural;
                    switch (p1)
                    {
                        case "COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL":
                            valorProdutorRural = InfraBanco.Constantes.Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL;
                            break;
                        case "COD_ST_CLIENTE_PRODUTOR_RURAL_NAO":
                            valorProdutorRural = InfraBanco.Constantes.Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO;
                            break;
                        case "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM":
                            valorProdutorRural = InfraBanco.Constantes.Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM;
                            break;
                        default:
                            Assert.Equal("", $"{p1} desconhecido em Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.WhenInformo em EndEtg_produtor_rural_status");
                            valorProdutorRural = InfraBanco.Constantes.Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL;
                            break;
                    }

                    pedidoDto.EnderecoEntrega.EndEtg_produtor_rural_status = (byte)valorProdutorRural;
                    break;

                default:
                    Assert.Equal("", $"{p0} desconhecido na rotina Especificacao.Ambiente.Loja.Loja.Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.AbstractInformo");
                    break;
            }
        }

    }
}
