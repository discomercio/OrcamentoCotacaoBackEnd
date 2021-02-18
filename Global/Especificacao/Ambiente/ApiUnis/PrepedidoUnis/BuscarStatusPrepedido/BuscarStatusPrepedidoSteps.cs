using Especificacao.Testes.Utils.InjecaoDependencia;
using Especificacao.Testes.Utils.ListaDependencias;
using InfraBanco;
using InfraBanco.Constantes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PrepedidoUnisBusiness.UnisDto.FormaPagtoUnisDto;
using PrepedidoUnisBusiness.UnisDto.PrepedidoUnisDto;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.BuscarStatusPrepedido
{
    [Binding, Scope(Tag = "Ambiente.ApiUnis.PrepedidoUnis.BuscarStatusPrepedido.BuscarStatusPrepedido")]
    public class BuscarStatusPrepedido
    {
        private readonly Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido cadastrarPrepedido = new CadastrarPrepedido.CadastrarPrepedido();
        private readonly PrepedidoAPIUnis.Controllers.PrepedidoUnisController prepedidoUnisController;
        private readonly ContextoBdProvider contextoBdProvider;

        public BuscarStatusPrepedido()
        {
            prepedidoUnisController = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<PrepedidoAPIUnis.Controllers.PrepedidoUnisController>();
            contextoBdProvider = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<ContextoBdProvider>();
            //este é feito dentro dele mesmo
            RegistroDependencias.AdicionarDependencia(
                "Ambiente.ApiUnis.PrepedidoUnis.BuscarStatusPrepedido.BuscarStatusPrepedidoListaDependencias", this,
                "Ambiente.ApiUnis.PrepedidoUnis.BuscarStatusPrepedido.BuscarStatusPrepedido"
                );

            tokenAcesso = Ambiente.ApiUnis.InjecaoDependencias.TokenAcessoApiUnis();
        }

        private string tokenAcesso = "";
        private string orcamento = "";

        [Given(@"Informo ""(.*)"" = ""(.*)""")]
        public void GivenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Informo(p0, p1, this);
            switch (p0)
            {
                case "TokenAcesso":
                    tokenAcesso = p1;
                    break;
                case "orcamento":
                    if (p1 == "especial: prepedido criado")
                        p1 = cadastrarPrepedido.UltimoPrePedidoResultadoUnisDto?.IdPrePedidoCadastrado ?? "";
                    orcamento = p1;
                    break;
                default:
                    Assert.Equal("", $"{p0} desconhecido");
                    break;
            }
        }

        [Then(@"Erro status code ""(.*)""")]
        public void ThenErroStatusCode(int statusCode)
        {
            Testes.Utils.LogTestes.LogOperacoes2.ErroStatusCode(statusCode, this);
            ActionResult res = AcessarMetodo();
            Testes.Utils.StatusCodes.TestarStatusCode(statusCode, res);
        }

        private ActionResult AcessarMetodo()
        {
            Testes.Utils.LogTestes.LogOperacoes2.ChamadaController(prepedidoUnisController.GetType(), "BuscarStatusPrepedido", this);
            Microsoft.AspNetCore.Mvc.ActionResult<BuscarStatusPrepedidoRetornoUnisDto> ret = prepedidoUnisController.BuscarStatusPrepedido(tokenAcesso, orcamento).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;
            return res;
        }

        [Given(@"Criar prepedido")]
        public void GivenCriarPrepedido()
        {
            cadastrarPrepedido.GivenPedidoBase();
            cadastrarPrepedido.ThenSemNenhumErro();
        }

        [Then(@"Resposta ""(.*)"" = ""(.*)""")]
        public void ThenResposta(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Resposta($"\"{p0}\" = \"{p1}\"", this);

            ActionResult res = AcessarMetodo();
            BuscarStatusPrepedidoRetornoUnisDto buscarStatusPrepedidoRetornoUnisDto = (BuscarStatusPrepedidoRetornoUnisDto)((Microsoft.AspNetCore.Mvc.OkObjectResult)res).Value;

            switch (p0)
            {
                case "St_orc_virou_pedido":
                    Assert.Equal(bool.Parse(p1), buscarStatusPrepedidoRetornoUnisDto.St_orc_virou_pedido);
                    break;
                default:
                    Assert.Equal("", $"{p0} desconhecido");
                    break;
            }
        }

        [Given(@"Alterar prepedido criado, passar para pedido")]
        public void GivenAlterarPrepedidoCriadoPassarParaPedido()
        {
            Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido cadastrarPedido = new ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido();
            cadastrarPedido.GivenPedidoBase();
            cadastrarPedido.ThenSemNenhumErro();
            Assert.NotNull(cadastrarPedido.UltimoAcessoFeito);
            if (cadastrarPedido.UltimoAcessoFeito == null)
                return;
            MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto pedidoResultadoMagentoDto
                            = (MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto)((Microsoft.AspNetCore.Mvc.OkObjectResult)cadastrarPedido.UltimoAcessoFeito.Result).Value;

            using var db = contextoBdProvider.GetContextoGravacaoParaUsing();
            Assert.NotNull(cadastrarPrepedido.UltimoPrePedidoResultadoUnisDto);
            if (cadastrarPrepedido.UltimoPrePedidoResultadoUnisDto == null)
                return;

            var prepedido = (from p in db.Torcamentos
                             where p.Orcamento == (cadastrarPrepedido.UltimoPrePedidoResultadoUnisDto.IdPrePedidoCadastrado)
                             select p).First();
            prepedido.St_Orc_Virou_Pedido = (short)1;
            db.Update(prepedido);

            var pedido = (from p in db.Tpedidos
                          where p.Pedido == pedidoResultadoMagentoDto.IdPedidoCadastrado
                          select p).First();
            pedido.Orcamento = orcamento;
            db.Update(pedido);

            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Resposta ""(.*)"" = ""(.*)""")]
        public void GivenResposta(string p0, string p1)
        {
            ThenResposta(p0, p1);
        }

    }
}
