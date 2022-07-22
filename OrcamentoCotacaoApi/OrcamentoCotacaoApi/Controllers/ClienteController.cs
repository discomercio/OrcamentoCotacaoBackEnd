using InfraBanco.Constantes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepedidoBusiness.Bll;
using PrepedidoBusiness.Dto.ClienteCadastro;
using System.Collections.Generic;
using System.Threading.Tasks;
using UtilsGlobais;

namespace PrepedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Autenticacao.RoleAcesso)]
    public class ClienteController : ControllerBase
    {
        private readonly InfraIdentity.IServicoDecodificarToken servicoDecodificarToken;
        private readonly ClientePrepedidoBll clientePrepedidoBll;

        public ClienteController(
            InfraIdentity.IServicoDecodificarToken servicoDecodificarToken,
            ClientePrepedidoBll clientePrepedidoBll)
        {
            this.servicoDecodificarToken = servicoDecodificarToken;
            this.clientePrepedidoBll = clientePrepedidoBll;
        }

        [HttpGet("buscarCliente/{cnpj_cpf}")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarCliente(string cnpj_cpf)
        {
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            ClienteCadastroDto dadosCliente = await clientePrepedidoBll.BuscarCliente(cnpj_cpf, apelido.Trim());

            if (dadosCliente == null)
                return NoContent();

            return Ok(dadosCliente);
        }

        [HttpPost("atualizarClienteparcial")]
        public async Task<IActionResult> AtualizarClienteParcial(DadosClienteCadastroDto dadosClienteCadastroDto)
        {
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            List<string> retorno = await clientePrepedidoBll.AtualizarClienteParcial(apelido.Trim(), dadosClienteCadastroDto,
                Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS);
            
            if (retorno == null)
                return NoContent();

            return Ok(retorno);
        }

        [HttpGet("listarBancosCombo")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListaBancosCombo()
        {
            IEnumerable<ListaBancoDto> listaBancos = await clientePrepedidoBll.ListarBancosCombo();

            if (listaBancos == null)
                return BadRequest();

            return Ok(listaBancos);
        }

        [HttpPost("cadastrarCliente")]
        public async Task<IActionResult> CadastrarCliente(ClienteCadastroDto clienteDto)
        {
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            return Ok(await clientePrepedidoBll.CadastrarCliente(clienteDto, apelido.Trim()));
        }

        [HttpGet("listarComboJustificaEndereco")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarComboJustificaEndereco()
        {
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            return Ok(await clientePrepedidoBll.ListarComboJustificaEndereco(apelido.Trim()));
        }
    }
}