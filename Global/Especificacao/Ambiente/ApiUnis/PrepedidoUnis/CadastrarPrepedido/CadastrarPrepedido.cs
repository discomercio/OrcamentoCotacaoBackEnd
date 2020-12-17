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

        public void WhenInformo(string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.Informo(campo, valor, this);
            switch (campo)
            {
                case "TokenAcesso":
                    prePedidoUnisDto.TokenAcesso = valor;
                    return;
                case "CPF/CNPJ":
                    prePedidoUnisDto.Cnpj_Cpf = valor;
                    return;
            }

            //acertos em campos
            if (campo == "vl_total_NF")
                campo = "ValorTotalDestePedidoComRA";

            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, prePedidoUnisDto))
                return;
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, prePedidoUnisDto.FormaPagtoCriacao))
                return;
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, prePedidoUnisDto.EnderecoEntrega))
                return;

            switch (campo)
            {
                case "EndEtg_obs":
                    prePedidoUnisDto.EnderecoEntrega.EndEtg_cod_justificativa = valor;
                    return;
                default:
                    Assert.Equal("", $"{campo} desconhecido na rotina Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.WhenInformo");
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
