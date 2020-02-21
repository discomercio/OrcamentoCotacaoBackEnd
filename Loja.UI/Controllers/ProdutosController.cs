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

namespace Loja.UI.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly Bll.ProdutoBll.ProdutoBll produtoBll;

        public ProdutosController(Bll.ProdutoBll.ProdutoBll produtoBll)
        {
            this.produtoBll = produtoBll;
        }
        public IActionResult Index(string fabricante)
        {
            return View("ConsultaListaPrecos");
        }
        public async Task<IActionResult> BuscaListaProdutosFabricante(string fabricante)
        {
            string teste = HttpContext.Session.GetString("usuario");
            if (!String.IsNullOrEmpty(fabricante))
            {
                string fab = await produtoBll.BuscarFabricante(fabricante);

                List<ConsultaProdutosDto> consultaProdutos =
                    (await produtoBll.ConsultarListaProdutos(fabricante)).ToList();


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

            string cpf_cnpj = HttpContext.Session.GetString("cpf_cnpj");

            PedidoProdutosDtoPedido prod = new PedidoProdutosDtoPedido();
            prod = JsonSerializer.Deserialize<PedidoProdutosDtoPedido>(produto);

            ProdutoValidadoComEstoqueDto produtoValidado = new ProdutoValidadoComEstoqueDto();
            //fazer a chamada do metodo
            produtoValidado = await produtoBll.VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado(prod, cpf_cnpj,
                int.Parse(id_nfe_emitente_selecao_manual));

            return produtoValidado;
        }
        
    }
}