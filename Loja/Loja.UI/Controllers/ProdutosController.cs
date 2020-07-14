using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Loja.UI.Models.Produtos;
using Loja.Bll.Dto.ProdutoDto;
using Microsoft.AspNetCore.Http;
using Loja.Bll.Dto.PedidoDto.DetalhesPedido;
using System.Text.Json;
using Loja.Bll.Bll.AcessoBll;
using Loja.Bll.Util;
using Microsoft.Extensions.Logging;
using Loja.Bll.ClienteBll;

namespace Loja.UI.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly Bll.ProdutoBll.ProdutoBll produtoBll;
        private readonly ClienteBll clienteBll;
        private readonly UsuarioAcessoBll usuarioAcessoBll;
        private readonly Configuracao configuracao;
        private readonly ILogger<UsuarioLogado> loggerUsuarioLogado;

        public ProdutosController(Bll.ProdutoBll.ProdutoBll produtoBll, ClienteBll clienteBll, UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao,
            ILogger<UsuarioLogado> loggerUsuarioLogado)
        {
            this.produtoBll = produtoBll;
            this.clienteBll = clienteBll;
            this.usuarioAcessoBll = usuarioAcessoBll;
            this.configuracao = configuracao;
            this.loggerUsuarioLogado = loggerUsuarioLogado;
        }
        public IActionResult Index(string fabricante)
        {
            return View("ConsultaListaPrecos");
        }

        public async Task<IActionResult> BuscaListaProdutosFabricante(string fabricante, string loja)
        {
            //var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);
            //string teste = HttpContext.Session.GetString("usuario");

            if (!String.IsNullOrEmpty(fabricante))
            {
                string fab = await produtoBll.BuscarFabricante(fabricante);

                List<ConsultaProdutosDto> consultaProdutos =
                    (await produtoBll.ConsultarListaProdutos(fabricante, loja)).ToList();


                ConsultaProdutosViewModel viewModel = new ConsultaProdutosViewModel();
                viewModel.Fabricante = fab;
                viewModel.LstProdutos = consultaProdutos;

                return View("ConsultaListaPrecos", viewModel);
            }
            else
                return View("ConsultaListaPrecos");

        }


        [HttpGet]
        public async Task<ProdutoValidadoComEstoqueDto> VerificarRegraProdutoCD(string produto, string id_nfe_emitente_selecao_manual)
        {
            List<string> lstRetorno = new List<string>();

            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);

            //string cpf_cnpj = HttpContext.Session.GetString("cpf_cnpj");

            PedidoProdutosDtoPedido prod = new PedidoProdutosDtoPedido();
            prod = JsonSerializer.Deserialize<PedidoProdutosDtoPedido>(produto);

            ProdutoValidadoComEstoqueDto produtoValidado = new ProdutoValidadoComEstoqueDto();
            //fazer a chamada do metodo
            produtoValidado = await produtoBll.VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado(prod, 
                Util.SoDigitosCpf_Cnpj(usuarioLogado.Cliente_Selecionado.DadosCliente.Cnpj_Cpf), int.Parse(id_nfe_emitente_selecao_manual));

            return produtoValidado;
        }

    }
}