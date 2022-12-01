using FormaPagamento;
using InfraBanco.Constantes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OrcamentoCotacaoApi.Controllers;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using Prepedido.Bll;
using Prepedido.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UtilsGlobais;

namespace PrepedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Autenticacao.RoleAcesso)]
    public class PrepedidoController : BaseController
    {
        private readonly PrepedidoBll prepedidoBll;
        private readonly PrepedidoApiBll prepedidoApiBll;
        private readonly InfraIdentity.IServicoDecodificarToken servicoDecodificarToken;
        private readonly FormaPagtoBll formaPagtoBll;
        private readonly FormaPagtoPrepedidoBll formaPagtoPrepedidoBll;
        private readonly CoeficientePrepedidoBll coeficientePrepedidoBll;
        private readonly IConfiguration configuration;
        private readonly PermissaoBll permissaoBll;

        public PrepedidoController(
            PrepedidoBll prepedidoBll,
            Prepedido.Bll.PrepedidoApiBll prepedidoApiBll,
            InfraIdentity.IServicoDecodificarToken servicoDecodificarToken,
            FormaPagtoBll formaPagtoBll,
            Prepedido.Bll.FormaPagtoPrepedidoBll formaPagtoPrepedidoBll,
            Prepedido.Bll.CoeficientePrepedidoBll coeficientePrepedidoBll,
            IConfiguration configuration,
            PermissaoBll permissaoBll)
        {
            this.prepedidoBll = prepedidoBll;
            this.prepedidoApiBll = prepedidoApiBll;
            this.servicoDecodificarToken = servicoDecodificarToken;
            this.formaPagtoBll = formaPagtoBll;
            this.formaPagtoPrepedidoBll = formaPagtoPrepedidoBll;
            this.coeficientePrepedidoBll = coeficientePrepedidoBll;
            this.configuration = configuration;
            this.permissaoBll = permissaoBll;
        }

        [HttpGet("listarNumerosPrepedidosCombo")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarNumerosPrepedidosCombo()
        {
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            
            return Ok(await prepedidoBll.ListarNumerosPrepedidosCombo(apelido));
        }

        [HttpGet("listarCpfCnpjPrepedidosCombo")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarCpfCnpjPrepedidosCombo()
        {
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            return Ok(await prepedidoBll.ListarCpfCnpjPrepedidosCombo(apelido));
        }

        [HttpGet("listarPrePedidos")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarPrePedidos(int tipoBusca, string clienteBusca, string numeroPrePedido,
            DateTime? dataInicial, DateTime? dataFinal)
        {
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            IEnumerable<Prepedido.Dto.PrepedidosCadastradosDtoPrepedido> lista = await prepedidoApiBll.ListarPrePedidos(apelido,
                (PrepedidoBll.TipoBuscaPrepedido)tipoBusca,
                clienteBusca, numeroPrePedido, dataInicial, dataFinal);

            return Ok(lista);
        }

        [HttpPost("removerPrePedido/{numeroPrePedido}")]
        public async Task<IActionResult> RemoverPrePedido(string numeroPrePedido)
        {
            var permissao = this.ObterPermissaoPrePedido(numeroPrePedido);

            if (!permissao.CancelarPrePedido)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            bool ret = await prepedidoBll.RemoverPrePedido(numeroPrePedido, apelido);

            if (ret)
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpGet("buscarPrepedido")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarPrePedido(string numPrepedido)
        {
            var permissao = this.ObterPermissaoPrePedido(numPrepedido);

            if (!permissao.VisualizarPrePedido)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            return Ok(await prepedidoApiBll.BuscarPrePedido(apelido, numPrepedido));
        }

        [HttpGet("obtemPercentualVlPedidoRA")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ObtemPercentualVlPedidoRA()
        {
            return Ok(await prepedidoBll.ObtemPercentualVlPedidoRA());
        }

        [HttpGet("obter_permite_ra_status")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Obter_Permite_RA_Status()
        {
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            return Ok(await prepedidoBll.Obter_Permite_RA_Status(apelido));
        }

        [HttpPost("cadastrarPrepedido")]
        public async Task<IActionResult> CadastrarPrepedido(PrePedidoDto prePedido)
        {
            var permissao = this.ObterPermissaoInclusaoPrePedido();

            if (!permissao.IncluirPrePedido)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<Configuracao>();
            //LIMITE_ARREDONDAMENTO_PRECO_VENDA_ORCAMENTO_ITEM fixo em 1 centavo
            IEnumerable<string> ret = await prepedidoApiBll.CadastrarPrepedido(prePedido, apelido.Trim(), 0.01M,
                appSettings.VerificarPrepedidoRepetido,
                Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS, appSettings.LimiteItens);

            return Ok(ret);
        }

        [HttpPost("deletarPrepedido")]
        public async Task<IActionResult> DeletarPrePedido(PrePedidoDto prePedido)
        {
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            await prepedidoApiBll.DeletarOrcamentoExisteComTransacao(prePedido, apelido.Trim());

            return Ok();
        }

        [HttpGet("buscarFormasPagto")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarFormasPagto(string tipo_pessoa)
        {
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            return Ok(await formaPagtoPrepedidoBll.ObterFormaPagto(apelido.Trim(), tipo_pessoa));
        }

        [HttpPost("buscarCoeficiente")]
        public async Task<IActionResult> BuscarCoeficiente(
            List<Prepedido.Dto.PrepedidoProdutoDtoPrepedido> lstProdutos)
        {
            return Ok(await coeficientePrepedidoBll.BuscarListaCoeficientes(lstProdutos));
        }

        [HttpPost("buscarCoeficienteFornecedores")]
        public async Task<IActionResult> BuscarCoeficienteFornecedores(List<string> lstFornecedores)
        {
            return Ok(await coeficientePrepedidoBll.BuscarListaCoeficientesFornecedores(lstFornecedores));
        }

        [HttpGet("buscarQtdeParcCartaoVisa")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarQtdeParcCartaoVisa()
        {
            return Ok(await formaPagtoBll.BuscarQtdeParcCartaoVisa());
        }

        private PermissaoPrePedidoResponse ObterPermissaoPrePedido(string IdPrePedido)
        {
            return this.permissaoBll.RetornarPermissaoPrePedido(new PermissaoPrePedidoRequest()
            {
                IdPrePedido = IdPrePedido,
                IdUsuario = LoggedUser.Id,
                PermissoesUsuario = LoggedUser.Permissoes,
                TipoUsuario = LoggedUser.TipoUsuario.Value,
                Usuario = LoggedUser.Apelido
            }).Result;
        }

        private PermissaoIncluirPrePedidoResponse ObterPermissaoInclusaoPrePedido()
        {
            return this.permissaoBll.RetornarPermissaoIncluirPrePedido(new PermissaoIncluirPrePedidoRequest()
            {
                PermissoesUsuario = LoggedUser.Permissoes
            }).Result;
        }
    }
}