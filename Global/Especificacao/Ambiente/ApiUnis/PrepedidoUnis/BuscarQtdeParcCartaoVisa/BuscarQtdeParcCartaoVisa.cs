using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using PrepedidoUnisBusiness.UnisDto.FormaPagtoUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa
{
    class BuscarQtdeParcCartaoVisa
    {
        private readonly PrepedidoAPIUnis.Controllers.PrepedidoUnisController prepedidoUnisController;
        private readonly Testes.Utils.LogTestes.LogTestes logTestes = Testes.Utils.LogTestes.LogTestes.GetInstance();
        public BuscarQtdeParcCartaoVisa()
        {
            prepedidoUnisController = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<PrepedidoAPIUnis.Controllers.PrepedidoUnisController>();
        }

        private string tokenAcesso = "";

        public void GivenDadoBase()
        {
            tokenAcesso = Ambiente.ApiUnis.InjecaoDependencias.TokenAcessoApiUnis();
        }

        public void WhenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes.Informo(p0, p1, this.GetType());
            switch (p0)
            {
                case "TokenAcesso":
                    tokenAcesso = p1;
                    break;
                default:
                    Assert.Equal("", $"{p0} desconhecido");
                    break;
            }
        }
        public void ThenErroStatusCode(int statusCode)
        {
            Testes.Utils.LogTestes.LogOperacoes.ErroStatusCode(statusCode, this.GetType());
            Testes.Utils.LogTestes.LogOperacoes.ChamadaController(prepedidoUnisController.GetType(), "BuscarQtdeParcCartaoVisa", this.GetType());
            Microsoft.AspNetCore.Mvc.ActionResult<QtdeParcCartaoVisaResultadoUnisDto> ret = prepedidoUnisController.BuscarQtdeParcCartaoVisa(tokenAcesso).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;
            Testes.Utils.StatusCodes.TestarStatusCode(statusCode, res);
        }

    }
}
