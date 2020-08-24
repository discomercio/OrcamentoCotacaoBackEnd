using Microsoft.Extensions.DependencyInjection;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido
{
    class CadastrarPrepedido
    {
        private readonly PrepedidoAPIUnis.Controllers.PrepedidoUnisController prepedidoUnisController;
        public CadastrarPrepedido()
        {
            prepedidoUnisController = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<PrepedidoAPIUnis.Controllers.PrepedidoUnisController>();
        }

        private PrePedidoUnisDto prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBase();
        public void GivenPrepedidoBase()
        {
            prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBase();
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
                    throw new ArgumentException($"{p0} desconhecido");
            }
        }
        public void ThenErroStatusCode(int statusCode)
        {
            Microsoft.AspNetCore.Mvc.ActionResult<PrePedidoResultadoUnisDto> ret = prepedidoUnisController.CadastrarPrepedido(prePedidoUnisDto).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;
            switch (statusCode)
            {
                case 200:
                    if (res.GetType() != typeof(Microsoft.AspNetCore.Mvc.OkObjectResult))
                        Assert.Equal("", "Tipo não é OkObjectResult");
                    break;

                case 401:
                    if (res.GetType() != typeof(Microsoft.AspNetCore.Mvc.UnauthorizedResult))
                        Assert.Equal("", "Tipo não é UnauthorizedResult");
                    break;

                default:
                    throw new ArgumentException($"{statusCode} desconhecido");
            }
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
    }
}
