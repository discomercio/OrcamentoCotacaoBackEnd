using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using PrepedidoUnisBusiness.UnisDto.FormaPagtoUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
{
    class CadastrarPedido
    {
        private readonly global::ApiMagento.Controllers.PedidoMagentoController pedidoMagentoController;
        private readonly Testes.Utils.LogTestes logTestes = Testes.Utils.LogTestes.GetInstance();
        readonly global::MagentoBusiness.UtilsMagento.ConfiguracaoApiMagento configuracaoApiMagento = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<global::MagentoBusiness.UtilsMagento.ConfiguracaoApiMagento>();

        public CadastrarPedido()
        {
            pedidoMagentoController = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<global::ApiMagento.Controllers.PedidoMagentoController>();
        }

        private MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto pedidoMagentoDto = new MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto();

        public void GivenDadoBase()
        {
            pedidoMagentoDto = CadastrarPedidoDados.PedidoBase();
            pedidoMagentoDto.TokenAcesso = Ambiente.ApiMagento.InjecaoDependencias.TokenAcessoApiMagento();
        }

        public void WhenInformo(string p0, string p1)
        {
            switch (p0)
            {
                case "appsettings.Orcamentista":
                    configuracaoApiMagento.DadosOrcamentista.Orcamentista = p1;
                    break;

                case "TokenAcesso":
                    pedidoMagentoDto.TokenAcesso = p1;
                    break;
                default:
                    Assert.Equal("", $"{p0} desconhecido");
                    break;
            }
        }
        public void ThenErroStatusCode(int statusCode)
        {
            logTestes.LogMensagem("pedidoMagentoController.CadastrarPedido");
            Microsoft.AspNetCore.Mvc.ActionResult<MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto> ret
                = pedidoMagentoController.CadastrarPedido(pedidoMagentoDto).Result;
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
        public void ThenSemNenhumErro()
        {
            ThenErro(null, false);
        }

        public void ThenErro(string? erro, bool erroDeveExistir)
        {
            if (erro != null)
                erro = Testes.Utils.MapeamentoMensagens.MapearMensagem(this.GetType().FullName, erro);

            logTestes.LogMensagem($"pedidoMagentoController.CadastrarPedido ThenErro({erro}, {erroDeveExistir})");
            Microsoft.AspNetCore.Mvc.ActionResult<MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto> ret
                = pedidoMagentoController.CadastrarPedido(pedidoMagentoDto).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;

            //deve ter retornado 200
            if (res.GetType() != typeof(Microsoft.AspNetCore.Mvc.OkObjectResult))
                Assert.Equal("", "Tipo não é OkObjectResult");

            MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto pedidoResultadoMagentoDto
                = (MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto)((Microsoft.AspNetCore.Mvc.OkObjectResult)res).Value;

            if (erroDeveExistir)
                Assert.Contains(erro, pedidoResultadoMagentoDto.ListaErros);
            else
            {
                if (erro == null)
                    Assert.Empty(pedidoResultadoMagentoDto.ListaErros);
                else
                    Assert.DoesNotContain(erro, pedidoResultadoMagentoDto.ListaErros);
            }

        }
    }
}
