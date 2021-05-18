using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfraBanco.Constantes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PrepedidoBusiness.Bll;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;

namespace PrepedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Utils.Autenticacao.RoleAcesso)]
    public class PrepedidoController : ControllerBase
    {
        private readonly Prepedido.PrepedidoBll prepedidoBll;
        private readonly PrepedidoApiBll prepedidoApiBll;
        private readonly InfraIdentity.IServicoDecodificarToken servicoDecodificarToken;
        private readonly Prepedido.FormaPagto.FormaPagtoBll formaPagtoBll;
        private readonly FormaPagtoPrepedidoBll formaPagtoPrepedidoBll;
        private readonly CoeficientePrepedidoBll coeficientePrepedidoBll;
        private readonly IConfiguration configuration;
        public PrepedidoController(Prepedido.PrepedidoBll prepedidoBll,
            PrepedidoBusiness.Bll.PrepedidoApiBll prepedidoApiBll,
            InfraIdentity.IServicoDecodificarToken servicoDecodificarToken,
            Prepedido.FormaPagto.FormaPagtoBll formaPagtoBll,
            PrepedidoBusiness.Bll.FormaPagtoPrepedidoBll formaPagtoPrepedidoBll,
            PrepedidoBusiness.Bll.CoeficientePrepedidoBll coeficientePrepedidoBll,
            IConfiguration configuration)
        {
            this.prepedidoBll = prepedidoBll;
            this.prepedidoApiBll = prepedidoApiBll;
            this.servicoDecodificarToken = servicoDecodificarToken;
            this.formaPagtoBll = formaPagtoBll;
            this.formaPagtoPrepedidoBll = formaPagtoPrepedidoBll;
            this.coeficientePrepedidoBll = coeficientePrepedidoBll;
            this.configuration = configuration;
        }

        //para teste, anonimo
#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("listarNumerosPrepedidosCombo")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarNumerosPrepedidosCombo()
        {
            //para testar: http://localhost:60877/api/prepedido/listarNumerosPrepedidosCombo
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            IEnumerable<string> ret = await prepedidoBll.ListarNumerosPrepedidosCombo(apelido);
            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("listarCpfCnpjPrepedidosCombo")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarCpfCnpjPrepedidosCombo()
        {
            //para testar :http://localhost:60877/api/prepedido/listarCpfCnpjPrepedidosCombo
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            IEnumerable<string> lista = await prepedidoBll.ListarCpfCnpjPrepedidosCombo(apelido);

            return Ok(lista);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("listarPrePedidos")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarPrePedidos(int tipoBusca, string clienteBusca, string numeroPrePedido,
            DateTime? dataInicial, DateTime? dataFinal)
        {
            //para testar: http://localhost:60877/api/prepedido/listarPrePedidos
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            IEnumerable<PrepedidoBusiness.Dto.Prepedido.PrepedidosCadastradosDtoPrepedido> lista = await prepedidoApiBll.ListarPrePedidos(apelido,
                (Prepedido.PrepedidoBll.TipoBuscaPrepedido)tipoBusca,
                clienteBusca, numeroPrePedido, dataInicial, dataFinal);
            return Ok(lista);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpPost("removerPrePedido/{numeroPrePedido}")]
        public async Task<IActionResult> RemoverPrePedido(string numeroPrePedido)
        {
            //para testar: http://localhost:60877/api/prepedido/removerPrePedido/{numeroPrePedido}
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            if (numeroPrePedido == null || numeroPrePedido == "")
            {
                return NotFound();
            }

            bool ret = await prepedidoBll.RemoverPrePedido(numeroPrePedido, apelido);

            if (ret == true)
                return Ok();
            else
                return NotFound();
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("buscarPrepedido")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarPrePedido(string numPrepedido)
        {
            //para testar: http://localhost:60877/api/prepedido/buscarPrepedido
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            PrePedidoDto ret = await prepedidoApiBll.BuscarPrePedido(apelido, numPrepedido);

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("obtemPercentualVlPedidoRA")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ObtemPercentualVlPedidoRA()
        {
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            decimal ret = await prepedidoBll.ObtemPercentualVlPedidoRA();

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("obter_permite_ra_status")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Obter_Permite_RA_Status()
        {
            //para testar: http://localhost:60877/api/prepedido/obter_permite_ra_status
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            short ret = await prepedidoBll.Obter_Permite_RA_Status(apelido);

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpPost("cadastrarPrepedido")]
        public async Task<IActionResult> CadastrarPrepedido(PrePedidoDto prePedido)
        {
            //para testar: http://localhost:60877/api/prepedido/cadastrarPrepedido
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<Utils.Configuracao>();
            //LIMITE_ARREDONDAMENTO_PRECO_VENDA_ORCAMENTO_ITEM fixo em 1 centavo
            IEnumerable<string> ret = await prepedidoApiBll.CadastrarPrepedido(prePedido, apelido.Trim(), 0.01M, true,
                Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS, appSettings.LimiteItens);

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpPost("deletarPrepedido")]
        public async Task<IActionResult> DeletarPrePedido(PrePedidoDto prePedido)
        {
            //para testar: http://localhost:60877/api/prepedido/deletarPrepedido
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            await prepedidoApiBll.DeletarOrcamentoExisteComTransacao(prePedido, apelido.Trim());

            return Ok();
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("buscarFormasPagto")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarFormasPagto(string tipo_pessoa)
        {
            //para testar: http://localhost:60877/api/prepedido/buscarFormasPagto
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            PrepedidoBusiness.Dto.FormaPagto.FormaPagtoDto ret = await formaPagtoPrepedidoBll.ObterFormaPagto(apelido.Trim(), tipo_pessoa);

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpPost("buscarCoeficiente")]
        public async Task<IActionResult> BuscarCoeficiente(
            List<PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido.PrepedidoProdutoDtoPrepedido> lstProdutos)
        {
            //para testar: http://localhost:60877/api/prepedido/buscarCoeficiente
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            IEnumerable<PrepedidoBusiness.Dto.Produto.CoeficienteDto> ret = await coeficientePrepedidoBll.BuscarListaCoeficientes(lstProdutos);

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpPost("buscarCoeficienteFornecedores")]
        public async Task<IActionResult> BuscarCoeficienteFornecedores(List<string> lstFornecedores)
        {
            //para testar: http://localhost:60877/api/prepedido/buscarCoeficiente
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            IEnumerable<IEnumerable<PrepedidoBusiness.Dto.Produto.CoeficienteDto>> ret = await coeficientePrepedidoBll.BuscarListaCoeficientesFornecedores(lstFornecedores);

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("buscarQtdeParcCartaoVisa")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarQtdeParcCartaoVisa()
        {
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            int ret = await formaPagtoBll.BuscarQtdeParcCartaoVisa();

            return Ok(ret);
            //
        }
    }
}

