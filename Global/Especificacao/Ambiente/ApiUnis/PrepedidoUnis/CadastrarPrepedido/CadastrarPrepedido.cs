using Microsoft.Extensions.DependencyInjection;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido
{
    class CadastrarPrepedido : Testes.Pedido.IPedidoPassosComuns
    {
        private readonly PrepedidoAPIUnis.Controllers.PrepedidoUnisController prepedidoUnisController;

        public CadastrarPrepedido()
        {
            prepedidoUnisController = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<PrepedidoAPIUnis.Controllers.PrepedidoUnisController>();
        }

        private PrePedidoUnisDto prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBase();
        public void GivenPrepedidoBase()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBase(this);
            prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBase();
        }
        public void GivenPedidoBaseComEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseComEnderecoDeEntrega(this);
            prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBaseComEnderecoDeEntrega();
        }

        public void GivenPedidoBaseClientePF()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseComEnderecoDeEntrega(this);
            prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBaseClientePF();
        }

        public void GivenPedidoBaseClientePJ()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseComEnderecoDeEntrega(this);
            prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBaseClientePJ();
        }

        public void GivenPedidoBaseClientePJComEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseClientePJComEnderecoDeEntrega(this);
            prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBaseClientePJComEnderecoDeEntrega();
        }

        public void GivenPedidoBase()
        {
            GivenPrepedidoBase();
        }

        public void WhenInformo(string p0, string p1)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.Informo(p0, p1, this);
            switch (p0)
            {
                case "TokenAcesso":
                    prePedidoUnisDto.TokenAcesso = p1;
                    break;
                case "CPF/CNPJ":
                    prePedidoUnisDto.Cnpj_Cpf = p1;
                    break;

                //endetg
                case "EndEtg_nome":
                    prePedidoUnisDto.EnderecoEntrega.EndEtg_nome = p1;
                    break;
                case "EndEtg_bairro":
                    prePedidoUnisDto.EnderecoEntrega.EndEtg_bairro = p1;
                    break;
                case "EndEtg_cep":
                    prePedidoUnisDto.EnderecoEntrega.EndEtg_cep = p1;
                    break;
                case "EndEtg_endereco_numero":
                    prePedidoUnisDto.EnderecoEntrega.EndEtg_endereco_numero = p1;
                    break;
                case "EndEtg_uf":
                    prePedidoUnisDto.EnderecoEntrega.EndEtg_uf = p1;
                    break;
                case "EndEtg_endereco":
                    prePedidoUnisDto.EnderecoEntrega.EndEtg_endereco = p1;
                    break;
                case "EndEtg_endereco_complemento":
                    prePedidoUnisDto.EnderecoEntrega.EndEtg_endereco_complemento = p1;
                    break;
                case "EndEtg_cidade":
                    prePedidoUnisDto.EnderecoEntrega.EndEtg_cidade = p1;
                    break;
                case "EndEtg_obs":
                    prePedidoUnisDto.EnderecoEntrega.EndEtg_cod_justificativa = p1;
                    break;
                case "EndEtg_ie":
                    prePedidoUnisDto.EnderecoEntrega.EndEtg_ie = p1;
                    break;
                case "EndEtg_contribuinte_icms_status":
                    InfraBanco.Constantes.Constantes.ContribuinteICMS valor;
                    switch (p1)
                    {
                        case "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL":
                            valor = InfraBanco.Constantes.Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL;
                            break;
                        case "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO":
                            valor = InfraBanco.Constantes.Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO;
                            break;
                        case "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM":
                            valor = InfraBanco.Constantes.Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM;
                            break;
                        case "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO":
                            valor = InfraBanco.Constantes.Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO;
                            break;
                        default:
                            Assert.Equal("", $"{p1} desconhecido em Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.WhenInformo em EndEtg_contribuinte_icms_status");
                            valor = InfraBanco.Constantes.Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL;
                            break;
                    }

                    prePedidoUnisDto.EnderecoEntrega.EndEtg_contribuinte_icms_status = (byte)valor;
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

                    prePedidoUnisDto.EnderecoEntrega.EndEtg_produtor_rural_status = (byte)valorProdutorRural;
                    break;

                default:
                    Assert.Equal("", $"{p0} desconhecido na rotina Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.WhenInformo");
                    break;
            }
        }
        public void ThenErroStatusCode(int statusCode)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.ErroStatusCode(statusCode, this);
            Testes.Utils.LogTestes.LogOperacoes2.ChamadaController(prepedidoUnisController.GetType(), "CadastrarPrepedido", this);
            Microsoft.AspNetCore.Mvc.ActionResult<PrePedidoResultadoUnisDto> ret = prepedidoUnisController.CadastrarPrepedido(prePedidoUnisDto).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;
            Testes.Utils.StatusCodes.TestarStatusCode(statusCode, res);
        }

        public void ThenErro(string p0)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.Erro(p0, this);
            ThenErro(p0, true);
        }
        public void ThenSemErro(string p0)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.SemErro(p0, this);
            ThenErro(p0, false);
        }
        public void ThenSemNenhumErro()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.SemNenhumErro(this);
            ThenErro(null, false);
        }


        //para quando querem aessar o prepedido criado
        public PrePedidoResultadoUnisDto? UltimoPrePedidoResultadoUnisDto { get; private set; }

        private void ThenErro(string? erro, bool erroDeveExistir)
        {
            if (ignorarFeature) return;

            Testes.Utils.LogTestes.LogOperacoes2.ChamadaController(prepedidoUnisController.GetType(), "CadastrarPrepedido", this);

            Microsoft.AspNetCore.Mvc.ActionResult<PrePedidoResultadoUnisDto> ret = prepedidoUnisController.CadastrarPrepedido(prePedidoUnisDto).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;

            //deve ter retornado 200
            if (res.GetType() != typeof(Microsoft.AspNetCore.Mvc.OkObjectResult))
                Assert.Equal("", "Tipo não é OkObjectResult");

            UltimoPrePedidoResultadoUnisDto = (PrePedidoResultadoUnisDto)((Microsoft.AspNetCore.Mvc.OkObjectResult)res).Value;

            Testes.Pedido.HelperImplementacaoPedido.CompararMensagemErro(erro, erroDeveExistir, UltimoPrePedidoResultadoUnisDto.ListaErros, this);
        }

        private bool ignorarFeature = false;
        public void GivenIgnorarCenarioNoAmbiente(string p0)
        {
            Testes.Pedido.PedidoPassosComuns.IgnorarCenarioNoAmbiente(p0, ref ignorarFeature, this.GetType());
        }

    }
}
