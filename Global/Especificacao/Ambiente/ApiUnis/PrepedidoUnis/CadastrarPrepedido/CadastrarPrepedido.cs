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
        private readonly Testes.Utils.LogTestes.LogTestes logTestes = Testes.Utils.LogTestes.LogTestes.GetInstance();

        public CadastrarPrepedido()
        {
            prepedidoUnisController = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<PrepedidoAPIUnis.Controllers.PrepedidoUnisController>();
        }

        private PrePedidoUnisDto prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBase();
        public void GivenPrepedidoBase()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes.DadoBase(this.GetType());
            prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBase();
        }
        public void GivenPedidoBaseComEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes.DadoBaseComEnderecoDeEntrega(this.GetType());
            prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBaseComEnderecoDeEntrega();
        }

        public void WhenPedidoBaseClientePF()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes.DadoBaseComEnderecoDeEntrega(this.GetType());
            prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBaseClientePF();
        }

        public void WhenPedidoBaseClientePJ()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes.DadoBaseComEnderecoDeEntrega(this.GetType());
            prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBaseClientePJ();
        }

        public void WhenPedidoBase()
        {
            GivenPrepedidoBase();
        }

        public void WhenInformo(string p0, string p1)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes.Informo(p0, p1, this.GetType());
            switch (p0)
            {
                case "TokenAcesso":
                    prePedidoUnisDto.TokenAcesso = p1;
                    break;
                case "CPF/CNPJ":
                    prePedidoUnisDto.Cnpj_Cpf = p1;
                    break;

                //endetg
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
                case "EndEtg_cidade":
                    prePedidoUnisDto.EnderecoEntrega.EndEtg_cidade = p1;
                    break;
                case "EndEtg_obs":
                    prePedidoUnisDto.EnderecoEntrega.EndEtg_cod_justificativa = p1;
                    break;

                default:
                    Assert.Equal("", $"{p0} desconhecido");
                    break;
            }
        }
        public void ThenErroStatusCode(int statusCode)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes.ErroStatusCode(statusCode, this.GetType());
            Testes.Utils.LogTestes.LogOperacoes.ChamadaController(prepedidoUnisController.GetType(), "CadastrarPrepedido", this.GetType());
            Microsoft.AspNetCore.Mvc.ActionResult<PrePedidoResultadoUnisDto> ret = prepedidoUnisController.CadastrarPrepedido(prePedidoUnisDto).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;
            Testes.Utils.StatusCodes.TestarStatusCode(statusCode, res);
        }

        public void ThenErro(string p0)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes.Erro(p0, this.GetType());
            ThenErro(p0, true);
        }
        public void ThenSemErro(string p0)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes.SemErro(p0, this.GetType());
            ThenErro(p0, false);
        }
        public void ThenSemNenhumErro()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes.SemNenhumErro(this.GetType());
            ThenErro(null, false);
        }

        private void ThenErro(string? erro, bool erroDeveExistir)
        {
            if (ignorarFeature) return;

            Testes.Utils.LogTestes.LogOperacoes.ChamadaController(prepedidoUnisController.GetType(), "CadastrarPrepedido", this.GetType());

            Microsoft.AspNetCore.Mvc.ActionResult<PrePedidoResultadoUnisDto> ret = prepedidoUnisController.CadastrarPrepedido(prePedidoUnisDto).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;

            //deve ter retornado 200
            if (res.GetType() != typeof(Microsoft.AspNetCore.Mvc.OkObjectResult))
                Assert.Equal("", "Tipo não é OkObjectResult");

            PrePedidoResultadoUnisDto prePedidoResultadoUnisDto = (PrePedidoResultadoUnisDto)((Microsoft.AspNetCore.Mvc.OkObjectResult)res).Value;

            Testes.Pedido.HelperImplementacaoPedido.ThenErroCompararacaoMensagens(erro, erroDeveExistir, prePedidoResultadoUnisDto.ListaErros, this.GetType());
        }

        private bool ignorarFeature = false;
        public void GivenIgnorarFeatureNoAmbiente2(string p0)
        {
            Testes.Pedido.PedidoPassosComuns.IgnorarFeatureNoAmbiente(p0, ref ignorarFeature, this.GetType());
        }
    }
}
