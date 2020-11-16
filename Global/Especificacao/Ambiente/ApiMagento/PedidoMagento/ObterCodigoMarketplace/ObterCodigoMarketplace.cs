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
        private readonly Testes.Utils.LogTestes.LogTestes logTestes = Testes.Utils.LogTestes.LogTestes.GetInstance();
        public CadastrarPedido()
        {
            pedidoMagentoController = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<global::ApiMagento.Controllers.PedidoMagentoController>();
        }

        private string tokenAcesso = "";

        public void GivenDadoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes.DadoBase(this.GetType());
            tokenAcesso = Ambiente.ApiMagento.InjecaoDependencias.TokenAcessoApiMagento();
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
            Testes.Utils.LogTestes.LogOperacoes.ChamadaController(pedidoMagentoController.GetType(), "ObterCodigoMarketplace", this.GetType());
            Microsoft.AspNetCore.Mvc.ActionResult<MagentoBusiness.MagentoDto.MarketplaceDto.MarketplaceResultadoDto> ret = pedidoMagentoController.ObterCodigoMarketplace(tokenAcesso).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;
            Testes.Utils.StatusCodes.TestarStatusCode(statusCode, res);
        }

    }
}
