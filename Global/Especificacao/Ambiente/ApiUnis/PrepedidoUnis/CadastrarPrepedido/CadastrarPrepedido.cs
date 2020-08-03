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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0066:Convert switch statement to expression", Justification = "<Prefiro assim...>")]
        public void WhenInformo(string p0, string p1)
        {
            switch (p0)
            {
                case "TokenAcesso":
                    prePedidoUnisDto.TokenAcesso = p1;
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
    }
}