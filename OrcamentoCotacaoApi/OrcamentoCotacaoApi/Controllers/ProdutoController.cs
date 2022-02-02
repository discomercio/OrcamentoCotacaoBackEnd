using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Utils;
using OrcamentoCotacaoBusiness.Models.Response;

namespace OrcamentoCotacaoApi.BaseController
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    [ApiController]
    //[Authorize(Roles = Autenticacao.RoleAcesso)]
    public class ProdutoController : Controller
    {
        private readonly ILogger<ProdutoController> _logger;
        private readonly IMapper _mapper;
        private readonly OrcamentoCotacaoBusiness.Bll.ProdutoPrepedidoBll _produtoBll;
        private readonly InfraIdentity.IServicoDecodificarToken _servicoDecodificarToken;

        public ProdutoController(ILogger<ProdutoController> logger, IMapper mapper,
            OrcamentoCotacaoBusiness.Bll.ProdutoPrepedidoBll orcamentoBll,
            InfraIdentity.IServicoDecodificarToken servicoDecodificarToken)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._produtoBll = orcamentoBll;
            this._servicoDecodificarToken = servicoDecodificarToken;
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("buscarProduto")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarProduto(string loja, string uf, string tipo)
        {
            //para testar: http://localhost:60877/api/produto/buscarProduto
            string apelido = _servicoDecodificarToken.ObterApelidoOrcamentista(User);
            //nao usamos o apelido

            OrcamentoCotacaoBusiness.Dto.Produto.ProdutoComboDto ret = await _produtoBll.ListaProdutosComboApiArclube(loja, uf, tipo);

            return Ok(ret);
        }
    }
}