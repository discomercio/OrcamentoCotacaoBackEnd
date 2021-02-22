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
        private readonly Testes.Utils.LogTestes.LogTestes logTestes = Testes.Utils.LogTestes.LogTestes.GetInstance();

        public QtdeParcCartaoVisaSteps()
        {
            //este é feito dentro dele mesmo
            RegistroDependencias.AdicionarDependencia(
                "Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.BuscarQtdeParcCartaoVisaListaDependencias", this,
                "Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.QtdeParcCartaoVisa"
                );


            this.contextoBdProvider = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<InfraBanco.ContextoBdProvider>();
            prepedidoUnisController = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<PrepedidoAPIUnis.Controllers.PrepedidoUnisController>();
        }

        
        private readonly string tokenAcesso = Ambiente.ApiUnis.InjecaoDependencias.TokenAcessoApiUnis();

        [Then(@"Resposta ""(.*)""")]
        public void ThenResposta(int p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Resposta(p0, this);

            Testes.Utils.LogTestes.LogOperacoes2.ChamadaController(prepedidoUnisController.GetType(), "BuscarQtdeParcCartaoVisa", this);
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
            Testes.Utils.LogTestes.LogOperacoes2.ErroStatusCode(statusCode, this);

            Testes.Utils.LogTestes.LogOperacoes2.ChamadaController(prepedidoUnisController.GetType(), "BuscarQtdeParcCartaoVisa", this);
            Microsoft.AspNetCore.Mvc.ActionResult<QtdeParcCartaoVisaResultadoUnisDto> ret = prepedidoUnisController.BuscarQtdeParcCartaoVisa(tokenAcesso).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;
            Testes.Utils.StatusCodes.TestarStatusCode(statusCode, res);
        }

    }
}
