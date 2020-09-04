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
        private readonly Testes.Utils.LogTestes logTestes = Testes.Utils.LogTestes.GetInstance();

        public CadastrarPrepedido()
        {
            prepedidoUnisController = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<PrepedidoAPIUnis.Controllers.PrepedidoUnisController>();
        }

        private PrePedidoUnisDto prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBase();
        public void GivenPrepedidoBase()
        {
            prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBase();
        }
        public void WhenPedidoBase()
        {
            GivenPrepedidoBase();
        }

        public void WhenInformo(string p0, string p1)
        {
            switch (p0)
            {
                case "TokenAcesso":
                    prePedidoUnisDto.TokenAcesso = p1;
                    break;
                case "CPF/CNPJ":
                    prePedidoUnisDto.Cnpj_Cpf = p1;
                    break;
                default:
                    Assert.Equal("", $"{p0} desconhecido");
                    break;
            }
        }
        public void ThenErroStatusCode(int statusCode)
        {
            logTestes.LogMensagem("prepedidoUnisController.CadastrarPrepedido ThenErroStatusCode");
            Microsoft.AspNetCore.Mvc.ActionResult<PrePedidoResultadoUnisDto> ret = prepedidoUnisController.CadastrarPrepedido(prePedidoUnisDto).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;
            Testes.Utils.StatusCodes.TestarStatusCode(statusCode, res);
        }

        public void ThenErro(string p0)
        {
            ThenErro(p0, true);
        }
        public void ThenSemErro(string p0)
        {
            ThenErro(p0, false);
        }

        public void ThenErro(string erro, bool erroDeveExistir)
        {
            erro = Testes.Pedido.PedidoPassosComuns.MapearMensagem(this.GetType().FullName, erro);
            logTestes.LogMensagem($"prepedidoUnisController.CadastrarPrepedido ThenErro({erro}, {erroDeveExistir})");

            Microsoft.AspNetCore.Mvc.ActionResult<PrePedidoResultadoUnisDto> ret = prepedidoUnisController.CadastrarPrepedido(prePedidoUnisDto).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;

            //deve ter retornado 200
            if (res.GetType() != typeof(Microsoft.AspNetCore.Mvc.OkObjectResult))
                Assert.Equal("", "Tipo não é OkObjectResult");

            PrePedidoResultadoUnisDto prePedidoResultadoUnisDto = (PrePedidoResultadoUnisDto)((Microsoft.AspNetCore.Mvc.OkObjectResult)res).Value;

            if (erroDeveExistir)
                Assert.Contains(erro, prePedidoResultadoUnisDto.ListaErros);
            else
                Assert.DoesNotContain(erro, prePedidoResultadoUnisDto.ListaErros);

        }

        //código duplicado!!!!
        private bool ignorarFeature = false;
        public void GivenIgnorarFeatureNoAmbiente(string p0)
        {
            var typeFullName = this.GetType().FullName;
            if (typeFullName == null)
            {
                Assert.Equal("", "sem this.GetType().FullName");
                return;
            }

            //mal resolvido: temos um Especificacao na frente.... bom, tiramos!
            typeFullName = typeFullName.Replace("Especificacao.Ambiente.", "Ambiente.");
            typeFullName = typeFullName.Replace("Especificacao.Especificacao.", "Especificacao.");

            if (typeFullName == p0)
                ignorarFeature = true;
        }
    }
}
