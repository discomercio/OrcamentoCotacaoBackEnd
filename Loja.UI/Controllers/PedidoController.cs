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
using Loja.Bll.Util;
using Microsoft.Extensions.Logging;

//TODO: habilitar nullable no projeto todo
#nullable enable

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
        private readonly UsuarioAcessoBll usuarioAcessoBll;
        private readonly Configuracao configuracao;
        private readonly ILogger<UsuarioLogado> loggerUsuarioLogado;

        public PedidoController(PedidoBll pedidoBll, ProdutoBll produtoBll, ClienteBll clienteBll, FormaPagtoBll formaPagtoBll, CoeficienteBll coeficienteBll,
            CancelamentoAutomaticoBll cancelamentoAutomaticoBll, UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao,
            ILogger<UsuarioLogado> loggerUsuarioLogado)
        {
            this.pedidoBll = pedidoBll;
            this.produtoBll = produtoBll;
            this.clienteBll = clienteBll;
            this.formaPagtoBll = formaPagtoBll;
            this.coeficienteBll = coeficienteBll;
            this.cancelamentoAutomaticoBll = cancelamentoAutomaticoBll;
            this.usuarioAcessoBll = usuarioAcessoBll;
            this.configuracao = configuracao;
            this.loggerUsuarioLogado = loggerUsuarioLogado;
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
            viewModel.ComIndicacao = 0;

            //Montar o select do PedBonshop
            List<string> lstPedidoBonshop = (await clienteBll.BuscarListaPedidosBonshop(cpf_cnpj)).ToList();
            List<SelectListItem> lstPed = new List<SelectListItem>();
            lstPed.Add(new SelectListItem { Value = "0", Text = "Selecione" });
            for (int i = 0; i < lstPedidoBonshop.Count; i++)
            {
                lstPed.Add(new SelectListItem { Value = lstPedidoBonshop[i], Text = lstPedidoBonshop[i] });
            }
            viewModel.PedBonshop = new SelectList(lstCd, "Value", "Text");

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PreparaParaCadastrarPedido(
            decimal totalDestePedido,
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
            int semRA,
            string pedBonshop)
        {
            //necessário formatar o valor de desconto para colocar ponto


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
                var pedidoSession = HttpContext.Session.GetString("pedidoDto");
                PedidoDto pedidoDtoSession = JsonConvert.DeserializeObject<PedidoDto>(pedidoSession);
                pedidoDtoSession.FormaPagtoCriacao = pagtoForma;
                pedidoDtoSession.ListaProdutos = lst;

                List<string> lstRetorno = (await pedidoBll.PreparaParaCadastrarPedido(loja, id_cliente, usuario, Indicador_Orcamentista, operaçoesPermitidas,
                    cpf_cnpj, pedidoDtoSession, semIndicacao, comIndicacao, cdAutomatico, cdManual, ListaCD, percComissao,
                    comRA, semRA)).ToList();

                //vamos colocar o pedidoCriacao na session para poder salvar na base depois
                if (lstRetorno.Count > 0)
                {
                    //vamos verificar as msgs que estão retornando


                }
                else
                {
                    //vamos atribuir para session
                    if (true)
                    {
                        //pedidoDtoSession.ListaProdutos = pedidoDto.ListaProdutos;
                        //pedidoDtoSession.FormaPagtoCriacao = pedidoDto.FormaPagtoCriacao;
                        pedidoDtoSession.VlTotalDestePedido = totalDestePedido;
                        pedidoDtoSession.PercRT = percComissao;
                        pedidoDtoSession.PermiteRAStatus = (short)comRA;
                        pedidoDtoSession.CDManual = cdManual == 0 ? (short)0 : (short)1;
                        pedidoDtoSession.CDSelecionado = cdManual == 1 ? ListaCD : 0;
                        pedidoDtoSession.ComIndicador = comIndicacao == 1 ? 1 : 0;
                        pedidoDtoSession.NomeIndicador = comIndicacao == 1 ? Indicador_Orcamentista : null;
                        pedidoDtoSession.PedBonshop = pedBonshop;
                        //estamos atribuindo o pedidoDto com as inserções de dados
                        HttpContext.Session.SetString("pedidoDto", JsonConvert.SerializeObject(pedidoDtoSession));

                    }
                }
            }
            else
            {
                //retornar erro para modal
            }
            //vamos mandar para um controller para montar a modelView de Observações
            return RedirectToAction("ObeservacoesPedido");
        }


        public async Task<IActionResult> ObeservacoesPedido()
        {
            /*
             * montar a tela de observações 
             * variáveis necessárias:
             * Nome do cliente
             * Cpf ou cnpj
             * valor total do pedido
             * bool entrega imediata
             * bool bem consumo
             * bool instalador
             */

            //montar a viewModel
            ObservacoesViewModel viewModel = new ObservacoesViewModel();

            //vamos pegar a session de pedido para atribuir valores para a view
            var pedidoSession = HttpContext.Session.GetString("pedidoDto");
            PedidoDto pedidoDtoSession = JsonConvert.DeserializeObject<PedidoDto>(pedidoSession);

            viewModel.Cnpj_Cpf = pedidoDtoSession.DadosCliente.Cnpj_Cpf;
            viewModel.NomeCliente = pedidoDtoSession.DadosCliente.Nome;
            viewModel.VlTotalPedido = pedidoDtoSession.VlTotalDestePedido.ToString();

            if (pedidoDtoSession.DetalhesNF != null)
            {
                viewModel.Observacoes = pedidoDtoSession.DetalhesNF.Observacoes;

                viewModel.InstaladorInstala = pedidoDtoSession.DetalhesNF.InstaladorInstala == 2 ?
                    Constantes.COD_INSTALADOR_INSTALA_SIM : Constantes.COD_INSTALADOR_INSTALA_NAO;

                viewModel.BemConsumo = pedidoDtoSession.DetalhesNF.StBemUsoConsumo != 0 ?
                    Constantes.COD_ST_BEM_USO_CONSUMO_SIM : Constantes.COD_ST_BEM_USO_CONSUMO_NAO;

                viewModel.EntregaImediata = pedidoDtoSession.DetalhesNF.EntregaImediata != "1" ?
                    Constantes.COD_ETG_IMEDIATA_SIM : Constantes.COD_ETG_IMEDIATA_NAO;
            }
            else
            {
                viewModel.EntregaImediata = Constantes.COD_ETG_IMEDIATA_SIM;
                viewModel.BemConsumo = Constantes.COD_ST_BEM_USO_CONSUMO_SIM;
                viewModel.InstaladorInstala = Constantes.COD_INSTALADOR_INSTALA_SIM;
            }

            return await Task.FromResult(View(viewModel));
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarPedido(ObservacoesViewModel detalhesPedido)
        {
            string usuario = HttpContext.Session.GetString("usuario_atual");
            string loja = HttpContext.Session.GetString("loja_atual");
            string id_cliente = HttpContext.Session.GetString("cliente_selecionado");
            string cpf_cnpj = HttpContext.Session.GetString("cpf_cnpj");

            // vamos pegar a session de pedido para atribuir valores para a view
            var pedidoSession = HttpContext.Session.GetString("pedidoDto");
            PedidoDto pedidoDtoSession = JsonConvert.DeserializeObject<PedidoDto>(pedidoSession);


            pedidoDtoSession.DetalhesNF = new DetalhesNFPedidoDtoPedido();
            //instaladorInstala é 1 = não | 2= sim
            pedidoDtoSession.DetalhesNF.InstaladorInstala = detalhesPedido.InstaladorInstala == "2" ?
                short.Parse(Constantes.COD_INSTALADOR_INSTALA_SIM) :
                short.Parse(Constantes.COD_INSTALADOR_INSTALA_NAO);

            pedidoDtoSession.DetalhesNF.Observacoes = detalhesPedido.Observacoes;

            //StBenUsoConsumo é 1 = sim | 0 = não
            pedidoDtoSession.DetalhesNF.StBemUsoConsumo = detalhesPedido.BemConsumo != "0" ?
                short.Parse(Constantes.COD_ST_BEM_USO_CONSUMO_SIM) :
                short.Parse(Constantes.COD_ST_BEM_USO_CONSUMO_NAO);

            //entrega imediata é 1 = não | 2 = sim
            pedidoDtoSession.DetalhesNF.EntregaImediata = detalhesPedido.EntregaImediata.ToString() != "1" ?
                Constantes.COD_ETG_IMEDIATA_SIM : Constantes.COD_ETG_IMEDIATA_NAO;

            //teremos que passar a session para o metodo na bll para salvar o pedido
            //seguindo os passos da lista abaixo
            var retorno = (await pedidoBll.CadastrarPedido(pedidoDtoSession, loja, cpf_cnpj, usuario,
                pedidoDtoSession.CDSelecionado));
            if (retorno.ListaErros.Count() > 0)
            {
                //deu erro

            }


            //se esta tudo ok redirecionamos para a tela de Pedido

            return RedirectToAction("BuscarPedido", new { numPedido = retorno.NumeroPedidoCriado });
        }

        public async Task<IActionResult> BuscarPedido(string numPedido)
        {
            //pegar usuario e numPedido

            string usuario = HttpContext.Session.GetString("usuario_atual");
            string loja = HttpContext.Session.GetString("loja_atual");
            string id_cliente = HttpContext.Session.GetString("cliente_selecionado");
            string cpf_cnpj = HttpContext.Session.GetString("cpf_cnpj");

            PedidoDto ret = await pedidoBll.BuscarPedido(usuario.Trim(), numPedido);

            PedidoViewModel viewModel = new PedidoViewModel();

            viewModel.PedidoDto = ret;

            return View(viewModel);
        }





        public async Task<IActionResult> CancelamentoAutomatico()
        {
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);

            bool consultaUniversalPedidoOrcamento = usuarioLogado.Operacao_permitida(Constantes.OP_LJA_CONSULTA_UNIVERSAL_PEDIDO_ORCAMENTO);
            var model = new Loja.UI.Models.Pedido.CancelamentoAutomaticoViewModel();
            model.LojasDisponiveis = usuarioLogado.LojasDisponiveis;
            model.cancelamentoAutomaticoItems = await cancelamentoAutomaticoBll.DadosTela(consultaUniversalPedidoOrcamento, usuarioLogado, model.LojasDisponiveis);
            model.MostrarLoja = usuarioLogado.Operacao_permitida(Constantes.OP_LJA_LOGIN_TROCA_RAPIDA_LOJA);
            model.ConsultaUniversalPedidoOrcamento = consultaUniversalPedidoOrcamento;
            return View(model);
        }
    }
}

