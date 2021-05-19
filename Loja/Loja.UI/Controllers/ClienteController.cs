using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Loja.Bll;
using Loja.Bll.Dto.ClienteDto;
using Loja.UI.Models.Cliente;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Loja.Bll.Bll.AcessoBll;
using Loja.Bll.Util;
using Microsoft.Extensions.Logging;

namespace Loja.UI.Controllers
{
    public class ClienteController : Controller
    {
        private readonly Bll.ClienteBll.ClienteBll clienteBll;
        private readonly UsuarioAcessoBll usuarioAcessoBll;
        private readonly Configuracao configuracao;
        private readonly ILogger<UsuarioLogado> loggerUsuarioLogado;

        public ClienteController(Bll.ClienteBll.ClienteBll clienteBll, UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao,
            ILogger<UsuarioLogado> loggerUsuarioLogado)
        {
            this.clienteBll = clienteBll;
            this.usuarioAcessoBll = usuarioAcessoBll;
            this.configuracao = configuracao;
            this.loggerUsuarioLogado = loggerUsuarioLogado;
        }
        public IActionResult Index()
        {

            //passar o DadosCienteDto para essa view
            //verificar se o modelo/cpf esta vazio
            //se o modelo/cpf estiver vazio, mostrar a modal com a opção de cadastrar um novo cliente
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ValidarCliente(string cpf_cnpj)
        {
            cpf_cnpj = Loja.Bll.Util.Util.SoDigitosCpf_Cnpj(cpf_cnpj);

            bool novoCliente = false;
            if (!await clienteBll.ValidarCliente(cpf_cnpj))
            {
                //colocar uma msg de erro para retornar para a tela
                novoCliente = true;
                return Json(novoCliente);
                //return RedirectToAction("Index", new { cpf_cnpj = cpf_cnpj, novoCliente = novoCliente });
            }
            else
                return Json(novoCliente = false);
            //return RedirectToAction("BuscarCliente", new { cpf_cnpj = cpf_cnpj, novoCliente = novoCliente }); //editar cliente
        }

        

        public async Task<IActionResult> BuscarCliente(string cpf_cnpj, bool novoCliente)
        {           
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);
            bool permiteEditar = Loja.Bll.Util.Util.OpercaoPermitida(Loja.Bll.Constantes.Constantes.OP_LJA_EDITA_CLIENTE_DADOS_CADASTRAIS
                , usuarioLogado.S_lista_operacoes_permitidas) ? true : false;

            ClienteCadastroDto clienteCadastroDto = new ClienteCadastroDto();
            if (!novoCliente)
                clienteCadastroDto = await clienteBll.BuscarCliente(UtilsGlobais.Util.SoDigitosCpf_Cnpj(cpf_cnpj), usuarioLogado.Usuario_atual);
            if (novoCliente)
            { 
                clienteCadastroDto.DadosCliente = new DadosClienteCadastroDto();
                clienteCadastroDto.DadosCliente.Cnpj_Cpf = cpf_cnpj;

                if (UtilsGlobais.Util.SoDigitosCpf_Cnpj(cpf_cnpj).Length == 11)
                    clienteCadastroDto.DadosCliente.Tipo = "PF";
                if (UtilsGlobais.Util.SoDigitosCpf_Cnpj(cpf_cnpj).Length == 14)
                    clienteCadastroDto.DadosCliente.Tipo = "PJ";
            }
            var lstInd = (await clienteBll.BuscarListaIndicadores(clienteCadastroDto.DadosCliente?.Indicador_Orcamentista,
                usuarioLogado.Usuario_atual, usuarioLogado.Loja_atual_id)).ToList();
            var lstJustificativas = (await clienteBll.ListarComboJustificaEndereco(usuarioLogado.Usuario_atual)).ToList();
            var lstBancos = (await clienteBll.ListarBancosCombo()).ToList();
            //receber tudo que ele precisa para ser criado corretamente.
            ClienteCadastroViewModel cliente = new ClienteCadastroViewModel(permiteEditar, novoCliente, clienteCadastroDto,
                lstInd, lstJustificativas, lstBancos);
           
            return View("DadosCliente", cliente);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarCliente(bool permiteEditar,
            Loja.Bll.Dto.ClienteDto.DadosClienteCadastroDto dados,
            List<Loja.Bll.Dto.ClienteDto.RefComercialDtoCliente> lstRefComercial,
            List<Loja.Bll.Dto.ClienteDto.RefBancariaDtoCliente> lstRefBancaria,
            Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro EndEntrega,
            bool cadastrando)
        {
            /*afazer: NECESSÁRIO FAZER O TRATAMENTO PARA ERROS 
             * CRIAR A TELA DE ERRO
             * Edu sugeriu usar o erro já existente no MVC
             */
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);

            ClienteCadastroDto clienteCadastroDto = new ClienteCadastroDto();
            clienteCadastroDto.DadosCliente = dados;
            clienteCadastroDto.RefBancaria = lstRefBancaria;
            clienteCadastroDto.RefComercial = lstRefComercial;

            if (cadastrando)
            {
                //vamos cadastrar o cliente
                List<string> lstRetorno = (await clienteBll.CadastrarCliente(clienteCadastroDto, usuarioLogado.Usuario_atual)).ToList();

                if (lstRetorno.Count > 1)
                {
                    //afazer: tratar erro para retornar para usuário
                    //deu erro vamos redirecionar 
                    //return RedirectToAction("Error", "Home", new { lstErros = lstRetorno });
                }

                return RedirectToAction("BuscarCliente", new { cpf_cnpj = clienteCadastroDto.DadosCliente.Cnpj_Cpf, novoCliente = false });
            }

            if (!cadastrando)
            {
                List<string> lstErros = (await clienteBll.AtualizarClienteParcial(usuarioLogado.Usuario_atual, clienteCadastroDto)).ToList();
                if (lstErros.Count > 0)
                {
                    //return RedirectToAction("Error", "Home", new { lstErros = lstRetorno });
                }
            }

            Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto pedidoDto = new Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto();
            pedidoDto.DadosCliente = new DadosClienteCadastroDto();
            pedidoDto.DadosCliente = dados;

            if (EndEntrega != null)
            {
                if (EndEntrega.EndEtg_cep != null)
                {
                    //vamos validar o endereço de entrega no Global/Prepedido
                    pedidoDto.EnderecoEntrega = new EnderecoEntregaDtoClienteCadastro();
                    //vamos normalizar o cep enviado antes de armazenar na session
                    EndEntrega.EndEtg_cep = EndEntrega.EndEtg_cep.Replace("-", "");
                }
                pedidoDto.EnderecoEntrega = EndEntrega;
            }

            return RedirectToAction("Indicador_SelecaoCD", "Pedido", new { pedidoDto = pedidoDto });
        }
    }
}