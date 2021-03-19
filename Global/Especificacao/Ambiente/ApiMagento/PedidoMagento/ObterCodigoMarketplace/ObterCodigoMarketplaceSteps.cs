using Especificacao.Testes.Utils.InjecaoDependencia;
using Especificacao.Testes.Utils.ListaDependencias;
using InfraBanco.Constantes;
using MagentoBusiness.MagentoDto.MarketplaceDto;
using Microsoft.Extensions.DependencyInjection;
using PrepedidoUnisBusiness.UnisDto.FormaPagtoUnisDto;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.ObterCodigoMarketplace
{
    [Binding, Scope(Tag = "Ambiente.ApiMagento.PedidoMagento.ObterCodigoMarketplace.ObterCodigoMarketplace")]
    public class ObterCodigoMarketplaceSteps
    {
        private readonly global::ApiMagento.Controllers.PedidoMagentoController pedidoMagentoController;
        private readonly InfraBanco.ContextoBdProvider contextoBdProvider;

        public ObterCodigoMarketplaceSteps()
        {
            //este é feito dentro dele mesmo
            RegistroDependencias.AdicionarDependencia(
                "Ambiente.ApiMagento.PedidoMagento.ObterCodigoMarketplace.ObterCodigoMarketplaceListaDependencias", this,
                "Ambiente.ApiMagento.PedidoMagento.ObterCodigoMarketplace.ObterCodigoMarketplace"
                );


            pedidoMagentoController = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<global::ApiMagento.Controllers.PedidoMagentoController>();
            this.contextoBdProvider = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<InfraBanco.ContextoBdProvider>();
        }

        private readonly string tokenAcesso = Ambiente.ApiMagento.InjecaoDependencias.TokenAcessoApiMagento();

        [Then(@"Resposta com número certo de registros")]
        public void ThenRespostaComNumeroCertoDeRegistros()
        {
            Testes.Utils.LogTestes.LogOperacoes2.MensagemEspecial("ThenRespostaComNumeroCertoDeRegistros", this);

            Testes.Utils.LogTestes.LogOperacoes2.ChamadaController(pedidoMagentoController.GetType(), "ObterCodigoMarketplace", this);
            Microsoft.AspNetCore.Mvc.ActionResult<MagentoBusiness.MagentoDto.MarketplaceDto.MarketplaceResultadoDto> ret = pedidoMagentoController.ObterCodigoMarketplace(tokenAcesso).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;

            //deve ter retornado 200
            if (res.GetType() != typeof(Microsoft.AspNetCore.Mvc.OkObjectResult))
                Assert.Equal("", "Tipo não é OkObjectResult");

            MarketplaceResultadoDto marketplaceResultadoDto = (MarketplaceResultadoDto)((Microsoft.AspNetCore.Mvc.OkObjectResult)res).Value;
            Assert.Empty(marketplaceResultadoDto.ListaErros);

            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.Verificacao("db.TcodigoDescricaos", this);
            var db = contextoBdProvider.GetContextoLeitura();
            var lstTcodigo = from c in db.TcodigoDescricaos
                             where c.Grupo == InfraBanco.Constantes.Constantes.GRUPO_T_CODIGO_DESCRICAO__PEDIDOECOMMERCE_ORIGEM &&
                                   c.St_Inativo == 0
                             select c;
            Assert.Equal(lstTcodigo.Count(), marketplaceResultadoDto.ListaMarketplace.Count());
        }
    }
}
