using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using PrepedidoUnisBusiness.UnisDto.FormaPagtoUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.ObterCodigoMarketplace
{
    class CadastrarPedido
    {
        private readonly global::ApiMagento.Controllers.PedidoMagentoController pedidoMagentoController;
        private readonly Testes.Utils.LogTestes logTestes = Testes.Utils.LogTestes.GetInstance();
        public CadastrarPedido()
        {
            pedidoMagentoController = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<global::ApiMagento.Controllers.PedidoMagentoController>();
        }

        private string tokenAcesso = "";

        public void GivenDadoBase()
        {
            tokenAcesso = Ambiente.ApiMagento.InjecaoDependencias.TokenAcessoApiMagento();
        }

        public void WhenInformo(string p0, string p1)
        {
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
            logTestes.LogMensagem("pedidoMagentoController.ObterCodigoMarketplace");
            Microsoft.AspNetCore.Mvc.ActionResult<MagentoBusiness.MagentoDto.MarketplaceDto.MarketplaceResultadoDto> ret = pedidoMagentoController.ObterCodigoMarketplace(tokenAcesso).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;
            Testes.Utils.StatusCodes.TestarStatusCode(statusCode, res);
        }

    }
}
