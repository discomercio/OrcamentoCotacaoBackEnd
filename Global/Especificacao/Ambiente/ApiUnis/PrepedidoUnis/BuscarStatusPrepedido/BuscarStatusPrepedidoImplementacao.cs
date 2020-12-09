using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using PrepedidoUnisBusiness.UnisDto.FormaPagtoUnisDto;
using PrepedidoUnisBusiness.UnisDto.PrepedidoUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.BuscarStatusPrepedido
{
    class BuscarStatusPrepedidoImplementacao
    {
        private readonly PrepedidoAPIUnis.Controllers.PrepedidoUnisController prepedidoUnisController;

        public BuscarStatusPrepedidoImplementacao()
        {
            prepedidoUnisController = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<PrepedidoAPIUnis.Controllers.PrepedidoUnisController>();
        }

        private string tokenAcesso = "";
        private string orcamento = "";
        public void GivenDadoBase()
        {
            tokenAcesso = Ambiente.ApiUnis.InjecaoDependencias.TokenAcessoApiUnis();
        }

        public void WhenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Informo(p0, p1, this);
            switch (p0)
            {
                case "TokenAcesso":
                    tokenAcesso = p1;
                    break;
                case "orcamento":
                    orcamento = p1;
                    break;
                default:
                    Assert.Equal("", $"{p0} desconhecido");
                    break;
            }
        }
        public void ThenErroStatusCode(int statusCode)
        {
            Testes.Utils.LogTestes.LogOperacoes2.ErroStatusCode(statusCode, this);
            Testes.Utils.LogTestes.LogOperacoes2.ChamadaController(prepedidoUnisController.GetType(), "BuscarStatusPrepedido", this);
            Microsoft.AspNetCore.Mvc.ActionResult<BuscarStatusPrepedidoRetornoUnisDto> ret = prepedidoUnisController.BuscarStatusPrepedido(tokenAcesso, orcamento).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;
            Testes.Utils.StatusCodes.TestarStatusCode(statusCode, res);
        }

    }
}
