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
            string lstOperacoesPermitidas = HttpContext.Session.GetString("lista_operacoes_permitidas");

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
            viewModel.ProdutoCombo = await produtoBll.ListaProdutosCombo(loja, id_cliente, pedidoDto);

            //buscamos o nome do cliente
            ClienteCadastroDto cliente = await clienteBll.BuscarCliente(cpf_cnpj, usuario);
            viewModel.NomeCliente = cliente.DadosCliente.Nome;

            //buscamos a lista com as possiveis formas de pagamentos
            viewModel.FormaPagto = await formaPagtoBll.ObterFormaPagto(usuario, cliente.DadosCliente.Tipo);

            //afazer: verificar com o Edu se tem como criar lista de lista com selectlist

            var lstEnumPagto = await formaPagtoBll.MontarListaFormaPagto(usuario, cliente.DadosCliente.Tipo);
            viewModel.EnumFormaPagto = new SelectList(lstEnumPagto, "Value", "Text");

            //busca a lista de coeficientes para calcular as prestações do pedido
            viewModel.ListaCoeficiente = new List<Bll.Dto.CoeficienteDto.CoeficienteDto>();
            var lstCoeficiente = await coeficienteBll.BuscarListaCompletaCoeficientes();
            viewModel.ListaCoeficiente = lstCoeficiente.ToList();

            //busca Permite_RA_Status
            viewModel.Permite_RA_Status = await pedidoBll.Obter_Permite_RA_Status(cliente.DadosCliente.Indicador_Orcamentista);

            //busca qtde de parcelas visa
            viewModel.QtdeParcVisa = await formaPagtoBll.BuscarQtdeParcCartaoVisa();

            //Lista para carregar no select de Indicadores
            var lstInd = (await produtoBll.BuscarOrcamentistaEIndicadorParaProdutos(usuario, lstOperacoesPermitidas, loja)).ToList();
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem { Value = "0", Text = "Selecione" });
            for (int i = 0; i < lstInd.Count; i++)
            {
                lst.Add(new SelectListItem { Value = lstInd[i], Text = lstInd[i] });
            }
            viewModel.LstIndicadores = new SelectList(lst, "Value", "Text");

            var lstSelecaoCd = (await produtoBll.WmsApelidoEmpresaNfeEmitenteMontaItensSelect(null)).ToList();
            List<SelectListItem> lstCd = new List<SelectListItem>();
            lstCd.Add(new SelectListItem { Value = "0", Text = "Selecione" });
            for (int i = 0; i < lstSelecaoCd.Count; i++)
            {
                lstCd.Add(new SelectListItem { Value = lstSelecaoCd[i][0], Text = lstSelecaoCd[i][1] });
            }
            viewModel.ListaCD = new SelectList(lstCd, "Value", "Text");

            viewModel.PercMaxDescEComissao = await pedidoBll.ObterPercentualMaxDescEComissao(loja);

            return View(viewModel);
        }      

    }
}