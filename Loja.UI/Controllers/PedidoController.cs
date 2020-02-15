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
using Loja.Bll.Dto.PedidoDto.DetalhesPedido;
using Loja.Bll.Dto.PrepedidoDto.DetalhesPrepedido;
using Loja.Bll.Dto.PedidoDto;
using Loja.Bll.Bll.PedidoBll;
using Loja.Bll.Constantes;
using Loja.Bll.Bll.AcessoBll;

namespace Loja.UI.Controllers
{
    public class PedidoController : Controller
    {
        private readonly PedidoBll pedidoBll;
        private readonly ProdutoBll produtoBll;
        private readonly ClienteBll clienteBll;
        private readonly FormaPagtoBll formaPagtoBll;
        private readonly CoeficienteBll coeficienteBll;
        private readonly CancelamentoAutomaticoBll cancelamentoAutomaticoBll;
        private readonly UsuarioLogado usuarioLogado;

        public PedidoController(PedidoBll pedidoBll, ProdutoBll produtoBll, ClienteBll clienteBll, FormaPagtoBll formaPagtoBll, CoeficienteBll coeficienteBll,
            CancelamentoAutomaticoBll cancelamentoAutomaticoBll)
        {
            this.pedidoBll = pedidoBll;
            this.produtoBll = produtoBll;
            this.clienteBll = clienteBll;
            this.formaPagtoBll = formaPagtoBll;
            this.coeficienteBll = coeficienteBll;
            this.cancelamentoAutomaticoBll = cancelamentoAutomaticoBll;
            this.usuarioLogado = UsuarioLogado.ObterUsuarioLogado(User, HttpContext.Session);
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
            //aqui esta demorando
            var lstProdutosTask = produtoBll.ListaProdutosCombo(loja, id_cliente, pedidoDto);

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

            //inicializar para atribuir
            //viewModel.ListaProdutoCriacao = new List<Bll.Dto.PedidoDto.DetalhesPedido.PedidoProdutosDtoPedido>();
            viewModel.FormaPagtoCriacao = new FormaPagtoCriacaoDto();
            viewModel.ListaOperacoesPermitidas = HttpContext.Session.GetString("lista_operacoes_permitidas");
            viewModel.ProdutoCombo = await lstProdutosTask;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PreparaParaCadastrarPedido(
            List<PedidoProdutosDtoPedido> lst,
            FormaPagtoCriacaoDto pagtoForma,
            decimal PercentualRA,
            string Indicador_Orcamentista,
            int semIndicacao,
            int comIndicacao,
            int cdAutomatico,
            int cdManual,
            int ListaCD,
            float percComissao,
            int comRA,
            int semRA)
        {
            //necessário formatar o valor de desconto para colocar ponto
            string retorno = "";


            /*
             * pegar lista de operações permitidas
             * busca os dados do cliente
             * obtem percentual maximo de desconto por loja
             * busca o Tparametro ID_PARAMETRO_PercMaxComissaoEDesconto_Nivel2_MeiosPagto
             * obtem_PercVlPedidoLimiteRA
             * verificar a session de vendedor_externo
             * 
             */
            string usuario = HttpContext.Session.GetString("usuario_atual");
            string loja = HttpContext.Session.GetString("loja_atual");
            string id_cliente = HttpContext.Session.GetString("cliente_selecionado");
            string cpf_cnpj = HttpContext.Session.GetString("cpf_cnpj");
            string operaçoesPermitidas = HttpContext.Session.GetString("lista_operacoes_permitidas");

            //validar cliente
            if (await clienteBll.ValidarCliente(cpf_cnpj))
            {
                //fazer a montagem de PedidoDtoCriacao
                PedidoDtoCriacao pedidoDto = new PedidoDtoCriacao();
                pedidoDto.FormaPagtoCriacao = pagtoForma;
                pedidoDto.ListaProdutos = lst;

                List<string> lstRetorno = (await pedidoBll.PreparaParaCadastrarPedido(loja, id_cliente, usuario, Indicador_Orcamentista, operaçoesPermitidas,
                    cpf_cnpj, pedidoDto, semIndicacao, comIndicacao, cdAutomatico, cdManual, ListaCD, percComissao,
                    comRA, semRA)).ToList();

                //vamos colocar o pedidoCriacao na session para poder salvar na base depois
                if (lstRetorno.Count > 0)
                {
                    //vamos verificar as msgs que estão retornando


                }
            }
            else
            {
                //retornar erro
            }

            return Ok();
        }

        //montar a tela de observações 
        //validar tela

        //valida forma de pagto já foi analisado
        //Custo finanfornec já foi analisado
        //end entrega já foi aramzenado
        //opcão de venda sem estoque ja foi armazenado
        //detalhes de observações 332 ate 348
        //vendedor externo 350 ate 357
        //busca dados do cliente
        //pega percmaxcomissão
        //busca relação de pagto preferenciais
        //le orçamentista
        //verificar limite mensal de compras do indicador
        //calcular limite mensal de compras do indicador
        //vl_limite_mensal_disponivel = vl_limite_mensal - vl_limite_mensal_consumido
        //atribui os produto itens 433 ate 460
        //verifica se o pedido já foi gravado 463 ate 509
        //calcula valor total do pedido
        //Percentual de comissão e desconto já foi analisado rever?
        //identifica e verifica se é pagto preferencial e calcula  637 ate 712
        //busca produtos , busca percentual custo finananceiro, calcula desconto
        //recupera os produtos que concordou mesmo sem estoque 
        //faz a lógica, regras para consumo do estoque 931 ate 1297
        //busca valor de limite para aprovação automática 1300 ate 1307
        //busca percentual de comissão
        //valida indicador
        //valida garantia indicador, entrega imediata, uso comum TELA observações
        //consiste valor total da forma de pagto
        //busca transportadora que atende o cep 1378 ate 1400
        //valida endereço de entrega
        //cadastra o pedido 1715 ate 2016
        //processa estoque 2026 ate 2121
        //gera numero do pedido
        //seta o pedido  e estoque e cliente 2140 ate 2169
        //faz o status entrega do pedido
        //verifica a senha para desconto
        //verifica se o endereço de entrega já não foi usado por outro cliente 2244 ate 2302
        //verifica outros pedido 2304 ate 2458
        //verifica se endereço de entrega não é usado por outro parceiro 2461 ate 2518
        //verifica pedidos de outros clientes 2521 ate 2676
        //monta o log 2691 ate 2881
        //grava log 
        //commit



        public async Task<IActionResult> CancelamentoAutomatico()
        {
            bool consultaUniversalPedidoOrcamento = AcessoBll.operacao_permitida(Constantes.OP_LJA_CONSULTA_UNIVERSAL_PEDIDO_ORCAMENTO, HttpContext.Session);
            var model = new Loja.UI.Models.Pedido.CancelamentoAutomaticoViewModel();
            model.cancelamentoAutomaticoItems = await cancelamentoAutomaticoBll.DadosTela(consultaUniversalPedidoOrcamento, AcessoBll.ObterUsuario(HttpContext.Session), HttpContext.Session);
            model.MostrarLoja = AcessoBll.operacao_permitida(Constantes.OP_LJA_LOGIN_TROCA_RAPIDA_LOJA, HttpContext.Session);
            model.MostrarLoja = true; //TODO: listar somente as lojas permitidas
            model.ConsultaUniversalPedidoOrcamento = consultaUniversalPedidoOrcamento;
            return View(model);
        }
    }
}

