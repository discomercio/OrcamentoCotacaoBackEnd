using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Loja.Bll.FormaPagtoBll;
using Loja.Bll.ClienteBll;
using Loja.Bll.Dto.ClienteDto;
using Loja.Bll.PedidoBll;
using Loja.Bll.ProdutoBll;
using Loja.UI.Models.Cliente;
using Loja.UI.Models.Pedido;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Loja.Bll.CoeficienteBll;

namespace Loja.UI.Controllers
{
    public class PedidoController : Controller
    {
        private readonly PedidoBll pedidoBll;
        private readonly ProdutoBll produtoBll;
        private readonly ClienteBll clienteBll;
        private readonly FormaPagtoBll formaPagtoBll;
        private readonly CoeficienteBll coeficienteBll;

        public PedidoController(PedidoBll pedidoBll, ProdutoBll produtoBll, ClienteBll clienteBll, FormaPagtoBll formaPagtoBll, CoeficienteBll coeficienteBll)
        {
            this.pedidoBll = pedidoBll;
            this.produtoBll = produtoBll;
            this.clienteBll = clienteBll;
            this.formaPagtoBll = formaPagtoBll;
            this.coeficienteBll = coeficienteBll;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> IniciarNovoPedido(EnderecoEntregaDtoClienteCadastro enderecoEntrega)
        {
            //O ideal seria armazenar o endereço de entrega em uma Sesion, para conforme formos inserindo os dados
            //para o pedido eles sejam armazenados para cadastrar o pedido.

            string usuario = HttpContext.Session.GetString("usuario_atual");
            string loja = HttpContext.Session.GetString("loja_atual");
            string id_cliente = HttpContext.Session.GetString("cliente_selecionado");
            string cpf_cnpj = HttpContext.Session.GetString("cpf_cnpj");

            /*afazer:
             * no ViewModel:
             * passar os dados do cliente
             * lista de produtos
             * dados de pagamento para inclusão
             * 

            /* -precisamos da lista de operações permitidas que esta guardada na Session para verificar na tela
             * -indicadores: sem indicação / com indicação => lista de indicadores para dropdown / 
             *               com RA => percentual de comissão e Sem RA / ref de pedido Bonshop = lista
             * -tipo de seleção do CD: automático / manual => lista
             * -acesso a classe Constantes
             */

            //exemplo para gravar e pegar um objeto na session
            var teste = HttpContext.Session.GetString("pedidoDto");
            Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto pedidoDto = 
                JsonConvert.DeserializeObject<Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto>(teste);
                

            ProdutosFormaPagtoViewModel viewModel = new ProdutosFormaPagtoViewModel();
            //vamos montar o model para mostrar na tela
            //buscamos os produtos
            viewModel.ProdutoCombo = await produtoBll.ListaProdutosCombo(loja, id_cliente);
            //buscamos o nome do cliente
            ClienteCadastroDto cliente = await clienteBll.BuscarCliente(cpf_cnpj, usuario);
            viewModel.NomeCliente = cliente.DadosCliente.Nome;

            //buscamos a lista com as possiveis formas de pagamentos
            viewModel.FormaPagto = await formaPagtoBll.ObterFormaPagto(usuario, cliente.DadosCliente.Tipo);
            var lstpagtoTask = await formaPagtoBll.ObterFormaPagto(usuario, cliente.DadosCliente.Tipo);
            List<SelectListItem> lstPagto = new List<SelectListItem>();
            lstPagto.Add(new SelectListItem { Value = "0", Text = "Selecione" });
            
            //afazer: verificar com o Edu se tem como criar lista de lista com selectlist

            var lstEnumPagto = await formaPagtoBll.MontarListaFormaPagto(usuario, cliente.DadosCliente.Tipo);
            viewModel.EnumFormaPagto = new SelectList(lstEnumPagto, "Value", "Text");

            //busca a lista de coeficientes para calcular as prestações do pedido
            viewModel.ListaCoeficiente = new List<Bll.Dto.CoeficienteDto.CoeficienteDto>();
            var lstCoeficiente = await coeficienteBll.BuscarListaCompletaCoeficientes();

            //busca Permite_RA_Status
            viewModel.Permite_RA_Status = await pedidoBll.Obter_Permite_RA_Status(cliente.DadosCliente.Indicador_Orcamentista);

            return View(viewModel);
        }

    }
}