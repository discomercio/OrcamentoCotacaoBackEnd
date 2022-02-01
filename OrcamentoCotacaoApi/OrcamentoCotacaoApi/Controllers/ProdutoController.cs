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
using Produto;

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
        private readonly ProdutoGeralBll _produtoBll;
        private readonly InfraIdentity.IServicoDecodificarToken _servicoDecodificarToken;
        
        public ProdutoController(ILogger<ProdutoController> logger, IMapper mapper, ProdutoGeralBll orcamentoBll,
            InfraIdentity.IServicoDecodificarToken servicoDecodificarToken)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._produtoBll = orcamentoBll;
            this._servicoDecodificarToken = servicoDecodificarToken;
        }

//#if DEBUG
//        [AllowAnonymous]
//#endif
        //[HttpGet("buscarProduto")]
        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        //public async Task<IActionResult> BuscarProduto(string loja, string id_cliente)
        //{
        //    //para testar: http://localhost:60877/api/produto/buscarProduto
        //    string apelido = _servicoDecodificarToken.ObterApelidoOrcamentista(User);
        //    //nao usamos o apelido

        //    PrepedidoBusiness.Dto.Produto.ProdutoComboDto ret = await _produtoBll.ListaProdutosComboApiArclube(loja, id_cliente);
            
        //    return Ok(ret);
        //}

        //[HttpGet]
        //public async Task<ProdutoResponseViewModel> Get(int page, int pageItens, int idCliente)
        //{
        //    var user = User.Identity.Name;
        //    var tipoUsuario = Int32.Parse(User.Claims.FirstOrDefault(x => x.Type == "TipoUsuario").Value);

        //    _logger.LogInformation("Buscando lista de produtos");
        //    return await _produtoBll.ListaProdutosComboDados(page, pageItens, idCliente, (TipoUsuario)tipoUsuario, user);
        //}
    }
}