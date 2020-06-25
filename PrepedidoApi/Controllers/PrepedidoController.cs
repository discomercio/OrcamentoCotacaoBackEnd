using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;

namespace PrepedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Utils.Autenticacao.RoleAcesso)]
    public class PrepedidoController : ControllerBase
    {
        private readonly PrepedidoBusiness.Bll.PrepedidoBll.PrepedidoBll prepedidoBll;
        private readonly InfraIdentity.IServicoDecodificarToken servicoDecodificarToken;
        private readonly PrepedidoBusiness.Bll.FormaPagtoBll.FormaPagtoBll formaPagtoBll;
        private readonly PrepedidoBusiness.Bll.CoeficienteBll coeficienteBll;

        public PrepedidoController(PrepedidoBusiness.Bll.PrepedidoBll.PrepedidoBll prepedidoBll, 
            InfraIdentity.IServicoDecodificarToken servicoDecodificarToken,
            PrepedidoBusiness.Bll.FormaPagtoBll.FormaPagtoBll formaPagtoBll, 
            PrepedidoBusiness.Bll.CoeficienteBll coeficienteBll)
        {
            this.prepedidoBll = prepedidoBll;
            this.servicoDecodificarToken = servicoDecodificarToken;
            this.formaPagtoBll = formaPagtoBll;
            this.coeficienteBll = coeficienteBll;
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
            var ret = await prepedidoBll.ListarNumerosPrepedidosCombo(apelido);
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
            var lista = await prepedidoBll.ListarCpfCnpjPrepedidosCombo(apelido);

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
            var lista = await prepedidoBll.ListarPrePedidos(apelido,
                (PrepedidoBusiness.Bll.PrepedidoBll.PrepedidoBll.TipoBuscaPrepedido)tipoBusca,
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

            var ret = await prepedidoBll.RemoverPrePedido(numeroPrePedido, apelido);

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

            var ret = await prepedidoBll.BuscarPrePedido(apelido, numPrepedido);

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

            var ret = await prepedidoBll.ObtemPercentualVlPedidoRA();

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

            var ret = await prepedidoBll.Obter_Permite_RA_Status(apelido);

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

            var ret = await prepedidoBll.CadastrarPrepedido(prePedido, apelido.Trim());

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

            await prepedidoBll.DeletarOrcamentoExisteComTransacao(prePedido, apelido.Trim());

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

            var ret = await formaPagtoBll.ObterFormaPagto(apelido.Trim(), tipo_pessoa);

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

            var ret = await coeficienteBll.BuscarListaCoeficientes(lstProdutos);

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

            var ret = await coeficienteBll.BuscarListaCoeficientesFornecedores(lstFornecedores);

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

            var ret = await formaPagtoBll.BuscarQtdeParcCartaoVisa();

            return Ok(ret);
            //
        }
    }
}

