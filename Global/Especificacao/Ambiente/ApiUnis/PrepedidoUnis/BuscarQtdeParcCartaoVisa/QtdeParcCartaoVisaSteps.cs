using Especificacao.Testes.Utils.InjecaoDependencia;
using Especificacao.Testes.Utils.ListaDependencias;
using InfraBanco.Constantes;
using Microsoft.Extensions.DependencyInjection;
using PrepedidoUnisBusiness.UnisDto.FormaPagtoUnisDto;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa
{
    [Binding, Scope(Tag = "Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.QtdeParcCartaoVisa")]
    public class QtdeParcCartaoVisaSteps
    {
        private readonly InfraBanco.ContextoBdProvider contextoBdProvider;
        private readonly PrepedidoAPIUnis.Controllers.PrepedidoUnisController prepedidoUnisController;
        private readonly Testes.Utils.LogTestes logTestes = Testes.Utils.LogTestes.GetInstance();

        public QtdeParcCartaoVisaSteps()
        {
            //este é feito dentro dele mesmo
            RegistroDependencias.AdicionarDependencia(
                "Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.BuscarQtdeParcCartaoVisaListaDependencias",
                "Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.QtdeParcCartaoVisa"
                );


            this.contextoBdProvider = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<InfraBanco.ContextoBdProvider>();
            prepedidoUnisController = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<PrepedidoAPIUnis.Controllers.PrepedidoUnisController>();
        }
        [Given(@"Limpar tabela ""(.*)""")]
        public void GivenLimparTabela(string p0)
        {
            Assert.Equal("t_PRAZO_PAGTO_VISANET", p0);
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing();
            foreach (var c in db.TprazoPagtoVisanets)
                db.TprazoPagtoVisanets.Remove(c);

            db.SaveChanges();
        }

        private InfraBanco.Modelos.TprazoPagtoVisanet tprazoPagtoVisanet = new InfraBanco.Modelos.TprazoPagtoVisanet();
        [Given(@"Gravar registro")]
        public void GivenGravarRegistro()
        {
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing();
            db.TprazoPagtoVisanets.Add(tprazoPagtoVisanet);
            db.SaveChanges();
        }

        [Given(@"Novo registro em ""(.*)""")]
        public void GivenNovoRegistroEm(string p0)
        {
            Assert.Equal("t_PRAZO_PAGTO_VISANET", p0);
            tprazoPagtoVisanet = new InfraBanco.Modelos.TprazoPagtoVisanet();
        }

        [Given(@"Novo registro ""(.*)"" = ""(.*)""")]
        public void GivenNovoRegistro(string p0, string p1)
        {
            switch (p0)
            {
                case "tipo":
                    switch (p1)
                    {
                        case "Constantes.COD_VISANET_PRAZO_PAGTO_LOJA":
                            tprazoPagtoVisanet.Tipo = Constantes.COD_VISANET_PRAZO_PAGTO_LOJA;
                            break;
                        default:
                            Assert.Equal("", $"{p1} desconhecido");
                            break;
                    }
                    break;
                case "qtde_parcelas":
                    if (!short.TryParse(p1, out short valor))
                        Assert.Equal("", $"{p1} não é numero");
                    tprazoPagtoVisanet.Qtde_parcelas = valor;
                    break;

                default:
                    Assert.Equal("", $"{p0} desconhecido");
                    break;
            }
        }

        private readonly string tokenAcesso = Ambiente.ApiUnis.InjecaoDependencias.TokenAcessoApiUnis();

        [Then(@"Resposta ""(.*)""")]
        public void ThenResposta(int p0)
        {
            logTestes.LogMensagem("prepedidoUnisController.BuscarQtdeParcCartaoVisa");
            Microsoft.AspNetCore.Mvc.ActionResult<QtdeParcCartaoVisaResultadoUnisDto> ret = prepedidoUnisController.BuscarQtdeParcCartaoVisa(tokenAcesso).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;

            //deve ter retornado 200
            if (res.GetType() != typeof(Microsoft.AspNetCore.Mvc.OkObjectResult))
                Assert.Equal("", "Tipo não é OkObjectResult");

            QtdeParcCartaoVisaResultadoUnisDto qtdeParcCartaoVisaResultadoUnisDto = (QtdeParcCartaoVisaResultadoUnisDto)((Microsoft.AspNetCore.Mvc.OkObjectResult)res).Value;
            Assert.Equal(p0, qtdeParcCartaoVisaResultadoUnisDto.QtdeParcCartaoVisa);
        }

        [Then(@"Erro status code ""(.*)""")]
        public void ThenErroStatusCode(int statusCode)
        {
            logTestes.LogMensagem("prepedidoUnisController.BuscarQtdeParcCartaoVisa");
            Microsoft.AspNetCore.Mvc.ActionResult<QtdeParcCartaoVisaResultadoUnisDto> ret = prepedidoUnisController.BuscarQtdeParcCartaoVisa(tokenAcesso).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;
            Testes.Utils.StatusCodes.TestarStatusCode(statusCode, res);
        }


        [AfterScenario]
        public void AfterScenario()
        {
            var servicos = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos();
            //precisa restaurar o banco
            var bd = new Testes.Utils.BancoTestes.InicializarBancoGeral(servicos.GetRequiredService<InfraBanco.ContextoBdProvider>(), servicos.GetRequiredService<InfraBanco.ContextoCepProvider>());
            bd.InicializarForcado();
        }


    }
}
