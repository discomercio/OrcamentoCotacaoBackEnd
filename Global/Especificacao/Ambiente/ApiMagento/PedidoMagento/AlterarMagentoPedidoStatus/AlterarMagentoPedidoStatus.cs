using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.AlterarMagentoPedidoStatus
{
    class CadastrarPedido
    {
        private readonly global::ApiMagento.Controllers.PedidoMagentoController pedidoMagentoController;

        public CadastrarPedido()
        {
            pedidoMagentoController = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<global::ApiMagento.Controllers.PedidoMagentoController>();
        }

        private string tokenAcesso = "";

        public void GivenDadoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBase(this);
            tokenAcesso = Ambiente.ApiMagento.InjecaoDependencias.TokenAcessoApiMagento();
        }

        public void WhenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Informo(p0, p1, this);
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
            Testes.Utils.LogTestes.LogOperacoes2.ErroStatusCode(statusCode, this);
            Testes.Utils.LogTestes.LogOperacoes2.ChamadaController(pedidoMagentoController.GetType(), "AlterarMagentoPedidoStatus", this);
            Microsoft.AspNetCore.Mvc.ActionResult<MagentoBusiness.MagentoDto.MarketplaceDto.MarketplaceResultadoDto> ret = pedidoMagentoController.ObterCodigoMarketplace(tokenAcesso).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;
            Testes.Utils.StatusCodes.TestarStatusCode(statusCode, res);
        }
    }
}
