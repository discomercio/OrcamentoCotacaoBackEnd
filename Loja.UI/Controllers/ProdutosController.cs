using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Loja.UI.Models.Produtos;
using Loja.Bll.Dto.ProdutoDto;
using Microsoft.AspNetCore.Http;

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
    }
}