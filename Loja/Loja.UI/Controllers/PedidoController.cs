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
using Loja.Bll.Constantes;
using Loja.Bll.Bll.AcessoBll;
using Loja.Bll.Util;
using Loja.Bll.Dto.IndicadorDto;
using Microsoft.Extensions.Logging;
using Loja.Bll.Dto.LojaDto;
using Loja.Bll.Bll.pedidoBll;
using Pedido;
using InfraBanco;

#nullable enable

namespace Loja.UI.Controllers
{
    public class PedidoController : Controller
    {
        private readonly Bll.PedidoBll.PedidoBll pedidoBll;
        private readonly ProdutoBll produtoBll;
        private readonly ClienteBll clienteBll;
        private readonly FormaPagtoBll formaPagtoBll;
        private readonly CoeficienteBll coeficienteBll;
        private readonly CancelamentoAutomaticoBll cancelamentoAutomaticoBll;
        private readonly UsuarioAcessoBll usuarioAcessoBll;
        private readonly Configuracao configuracao;
        private readonly ILogger<UsuarioLogado> loggerUsuarioLogado;
        private readonly ContextoBdProvider contextoBdProvider;

        public PedidoController(Bll.PedidoBll.PedidoBll pedidoBll, ProdutoBll produtoBll, ClienteBll clienteBll, FormaPagtoBll formaPagtoBll, CoeficienteBll coeficienteBll,
            CancelamentoAutomaticoBll cancelamentoAutomaticoBll, UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao,
            ILogger<UsuarioLogado> loggerUsuarioLogado, InfraBanco.ContextoBdProvider contextoBdProvider)
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
            this.contextoBdProvider = contextoBdProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> IniciarNovoPedido(EnderecoEntregaDtoClienteCadastro enderecoEntrega)
        {
            //O ideal seria armazenar o endereço de entrega em uma Sesion, para conforme formos inserindo os dados
            //para o pedido eles sejam armazenados para cadastrar o pedido.
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);

            PedidoDto pedidoDto = usuarioLogado.PedidoDto;

            ProdutosFormaPagtoViewModel viewModel = new ProdutosFormaPagtoViewModel();

            //vamos montar o model para mostrar na tela

            //buscamos os produtos
            //aqui esta demorando
            var lstProdutosTask = produtoBll.ListaProdutosCombo(usuarioLogado.Loja_atual_id,
                usuarioLogado.Cliente_Selecionado.DadosCliente.Id, pedidoDto);
            viewModel.ProdutoCombo = await lstProdutosTask;

            //pegamos o clienteque esta na session
            viewModel.NomeCliente = usuarioLogado.Cliente_Selecionado.DadosCliente.Nome;
            viewModel.CpfCnpj = usuarioLogado.Cliente_Selecionado.DadosCliente.Cnpj_Cpf;
            viewModel.TipoCliente = usuarioLogado.Cliente_Selecionado.DadosCliente.Tipo;

            //buscamos a lista com as possiveis formas de pagamentos
            viewModel.FormaPagto = await formaPagtoBll.ObterFormaPagto(usuarioLogado.Usuario_atual,
                usuarioLogado.Cliente_Selecionado.DadosCliente.Tipo, usuarioLogado.Loja_atual_id,
                usuarioLogado.PedidoDto.ComIndicador);

            var lstEnumPagto = await formaPagtoBll.MontarListaFormaPagto(usuarioLogado.Usuario_atual,
                usuarioLogado.Cliente_Selecionado.DadosCliente.Tipo, usuarioLogado.Loja_atual_id,
                usuarioLogado.PedidoDto.ComIndicador);
            viewModel.EnumFormaPagto = new SelectList(lstEnumPagto, "Value", "Text");

            //busca a lista de coeficientes para calcular as prestações do pedido
            viewModel.ListaCoeficiente = new List<Bll.Dto.CoeficienteDto.CoeficienteDto>();
            var lstCoeficiente = await coeficienteBll.BuscarListaCompletaCoeficientes();
            viewModel.ListaCoeficiente = lstCoeficiente.ToList();

            //busca Permite_RA_Status
            //afazer: corrigir, ler o arquivo Coisas a fazer na LOJA
            //deixarei aqui para testar a tela de itens
            viewModel.Permite_RA_Status = usuarioLogado.PedidoDto.PermiteRAStatus;
            //if (viewModel.Permite_RA_Status == 1)
            //{
            //    //vamos obter o percentual de RA
            //    viewModel.PercentualRA = await pedidoBll.ObtemPercentualVlPedidoRA();
            //}

            viewModel.CdSelecionadoId = usuarioLogado.PedidoDto.CDSelecionado;

            //busca qtde de parcelas visa
            viewModel.QtdeParcVisa = await formaPagtoBll.BuscarQtdeParcCartaoVisa();

            viewModel.PercMaxDescEComissao = await pedidoBll.BuscarPercMaxPorLoja(usuarioLogado.Loja_atual_id);

            viewModel.PercComissao = usuarioLogado.PedidoDto.PercRT;

            //inicializar para atribuir
            viewModel.FormaPagtoCriacao = new FormaPagtoCriacaoDto();
            viewModel.ProdutoCombo = await lstProdutosTask;
            viewModel.ComIndicacao = 0;

            //lista de pagto preferenciais preciso mandar para a tela
            viewModel.MeiosPagtoPreferenciais = await pedidoBll.BuscarMeiosPagtoPreferenciais();

            //afazer: buscar lista de PESQUISA OS INDICADORES DA LOJA INFORMADA
            //viewModel.ListaObjetoSenhaDesconto = (await pedidoBll.BuscarSenhaDesconto(usuarioLogado.Cliente_Selecionado.DadosCliente.Id,
            //    usuarioLogado.Loja_atual_id)).ToList();

            //Montar o select do PedBonshop
            List<string> lstPedidoBonshop = (await clienteBll.BuscarListaPedidosBonshop(usuarioLogado.Cliente_Selecionado.DadosCliente.Cnpj_Cpf)).ToList();
            List<SelectListItem> lstPed = new List<SelectListItem>();
            lstPed.Add(new SelectListItem { Value = "0", Text = "Selecione" });
            for (int i = 0; i < lstPedidoBonshop.Count; i++)
            {
                lstPed.Add(new SelectListItem { Value = lstPedidoBonshop[i], Text = lstPedidoBonshop[i] });
            }
            viewModel.PedBonshop = new SelectList(lstPed, "Value", "Text");

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PreparaParaCadastrarPedido(decimal totalDestePedido,
            List<PedidoProdutosDtoPedido> lst, FormaPagtoCriacaoDto pagtoForma, float percComissao,
            decimal totalValorRABrutoInput, decimal totalValorRALiquidoInput)
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

            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);


            //validar cliente
            if (await clienteBll.ValidarCliente(
                Util.SoDigitosCpf_Cnpj(usuarioLogado.Cliente_Selecionado.DadosCliente.Cnpj_Cpf)))
            {
                PedidoDto pedidoDtoSession = usuarioLogado.PedidoDto;

                pedidoDtoSession.FormaPagtoCriacao = pagtoForma;
                pedidoDtoSession.ListaProdutos = lst;

                if (pedidoDtoSession.PercRT != percComissao)
                {
                    pedidoDtoSession.PercRT = percComissao;
                }

                //List<string> lstRetorno = (await pedidoBll.PreparaParaCadastrarPedido(usuarioLogado.Loja_atual_id,
                //    usuarioLogado.Cliente_Selecionado.DadosCliente.Id, usuarioLogado.Usuario_atual,
                //    usuarioLogado.S_lista_operacoes_permitidas, Util.SoDigitosCpf_Cnpj(usuarioLogado.Cliente_Selecionado.DadosCliente.Cnpj_Cpf),
                //    pedidoDtoSession)).ToList();

                List<string> lstRetorno = new List<string>();
                //vamos colocar o pedidoCriacao na session para poder salvar na base depois
                if (lstRetorno.Count > 0)
                {
                    //vamos verificar as msgs que estão retornando


                }
                else
                {
                    //vamos atribuir para session
                    pedidoDtoSession.VlTotalDestePedido = totalDestePedido;
                    //pedidoDtoSession.PedBonshop = pedBonshop;
                    //estamos atribuindo o pedidoDto com as inserções de dados
                    usuarioLogado.PedidoDto = pedidoDtoSession;

                }
            }
            else
            {
                //retornar erro para modal
            }
            //vamos mandar para um controller para montar a modelView de Observações
            return RedirectToAction("ObservacoesPedido");
        }


        public async Task<IActionResult> ObservacoesPedido()
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

            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);


            //montar a viewModel
            ObservacoesViewModel viewModel = new ObservacoesViewModel();

            //vamos pegar a session de pedido para atribuir valores para a view
            PedidoDto pedidoDtoSession = usuarioLogado.PedidoDto;

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
            // vamos pegar a session de pedido para atribuir valores para a view

            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);

            PedidoDto pedidoDtoSession = usuarioLogado.PedidoDto;

            pedidoDtoSession.DetalhesNF = new DetalhesNFPedidoDtoPedido();
            //instaladorInstala é 1 = não | 2= sim
            pedidoDtoSession.DetalhesNF.InstaladorInstala = detalhesPedido.InstaladorInstala == "2" ?
                short.Parse(Constantes.COD_INSTALADOR_INSTALA_SIM) :
                short.Parse(Constantes.COD_INSTALADOR_INSTALA_NAO);

            pedidoDtoSession.DetalhesNF.Observacoes = await Task.FromResult(detalhesPedido.Observacoes);

            //StBenUsoConsumo é 1 = sim | 0 = não
            pedidoDtoSession.DetalhesNF.StBemUsoConsumo = detalhesPedido.BemConsumo != "0" ?
                short.Parse(Constantes.COD_ST_BEM_USO_CONSUMO_SIM) :
                short.Parse(Constantes.COD_ST_BEM_USO_CONSUMO_NAO);

            //entrega imediata é 1 = não | 2 = sim
            pedidoDtoSession.DetalhesNF.EntregaImediata = detalhesPedido.EntregaImediata.ToString() != "1" ?
                Constantes.COD_ETG_IMEDIATA_SIM : Constantes.COD_ETG_IMEDIATA_NAO;

            //teremos que passar a session para o metodo na bll para salvar o pedido
            //seguindo os passos da lista abaixo
            //var retorno = (await pedidoBll.CadastrarPedido(pedidoDtoSession, usuarioLogado.Loja_atual_id,
            //    Util.SoDigitosCpf_Cnpj(usuarioLogado.Cliente_Selecionado.DadosCliente.Cnpj_Cpf), usuarioLogado.Usuario_atual,
            //    pedidoDtoSession.CDSelecionado, usuarioLogado.Vendedor_externo, efetivaPedidoBll));


            //if (retorno.ListaErros.Count() > 0)
            //{
            //    //deu erro

            //}

            //todo: afazer: vamos remover estas conversões; estão aqui temporariamente até a gente mudar o HTML e javscript
            foreach (var origem in pedidoDtoSession.ListaProdutos)
            {
                origem.Produto = origem.NumProduto;
                origem.CustoFinancFornecPrecoListaBase = origem.VlLista;
                origem.Preco_NF = origem.Preco_Lista ?? 0;
                origem.Desc_Dado = origem.Desconto;
                origem.Preco_Venda = origem.VlVenda ?? 0;
                origem.TotalItem = origem.VlTotalItem ?? 0;
                origem.TotalItemRA = origem.VlTotalItemComRA;
                //este precisamos acessar o banco
                origem.CustoFinancFornecCoeficiente = 0;
                    //from c in contextoBdProvider.GetContextoLeitura().
                    //                                   where c.Fabricante == origem.Fabricante && c.TipoParcela == siglaParc
                    //                                  select 
            }



            Pedido.Dados.Criacao.PedidoCriacaoRetornoDados ret = await pedidoBll.CadastrarPedido(pedidoDtoSession,
                usuarioLogado.Loja_atual_id, usuarioLogado.Usuario_atual, usuarioLogado.Vendedor_externo);

            return Ok(ret);
            //se esta tudo ok redirecionamos para a tela de Pedido
            //return RedirectToAction("BuscarPedido", new { numPedido = ret.Id });
            //return RedirectToAction("Index", "Cliente", new { numPedido = "pedido não foi salvo, implementando novo cadastro de pedido" });
        }

        public async Task<IActionResult> BuscarPedido(string numPedido)
        {
            //pegar usuario e numPedido
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);

            //PedidoDto ret = await pedidoBll.BuscarPedido(usuarioLogado.Usuario_atual.Trim(), numPedido);
            PedidoDto ret = new PedidoDto();

            PedidoViewModel viewModel = new PedidoViewModel();

            viewModel.PedidoDto = await Task.FromResult(ret);

            return View(viewModel);
        }

        //afazer:montar um metodo para criar a nova tela de indicação e seleção do CD
        public async Task<IActionResult> Indicador_SelecaoCD(EnderecoEntregaDtoClienteCadastro enderecoEntrega)
        {
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);

            PedidoDto pedidoDto = usuarioLogado.PedidoDto;

            Indicador_SelecaoCDViewModel viewModel = new Indicador_SelecaoCDViewModel();
            //pegamos o clienteque esta na session
            viewModel.NomeCliente = usuarioLogado.Cliente_Selecionado.DadosCliente.Nome;
            viewModel.CpfCnpj = usuarioLogado.Cliente_Selecionado.DadosCliente.Cnpj_Cpf;

            //buscamos o indicador original para fazer a comparação
            viewModel.IndicadorOriginal = usuarioLogado.Cliente_Selecionado.DadosCliente.Indicador_Orcamentista.ToString();

            //pegamos a loja atual para comparar no caso de indicador
            viewModel.LojaAtual = usuarioLogado.Loja_atual_id;

            //lista completa de indicadores
            List<IndicadorDto> lstIndicadores = (await pedidoBll.BuscarOrcamentistaEIndicadorListaCompleta(usuarioLogado.Usuario_atual,
                usuarioLogado.S_lista_operacoes_permitidas, usuarioLogado.Loja_atual_id)).ToList();
            List<SelectListItem> lstIndicador = new List<SelectListItem>();
            foreach (var i in lstIndicadores)
            {
                lstIndicador.Add(new SelectListItem { Value = i.Apelido, Text = i.Apelido + " - " + i.RazaoSocial });
            }
            viewModel.ListaIndicadores = new SelectList(lstIndicador, "Value", "Text");
            //viewModel.ListaIndicadores = (await pedidoBll.BuscarOrcamentistaEIndicadorListaCompleta(usuarioLogado.Usuario_atual,
            //    usuarioLogado.S_lista_operacoes_permitidas, usuarioLogado.Loja_atual_id)).ToList();

            //lista de cd's
            var lstSelecaoCd = (await produtoBll.WmsApelidoEmpresaNfeEmitenteMontaItensSelect(null)).ToList();
            List<SelectListItem> lstCd = new List<SelectListItem>();
            lstCd.Add(new SelectListItem { Value = "0", Text = "Selecione" });
            for (int i = 0; i < lstSelecaoCd.Count; i++)
            {
                lstCd.Add(new SelectListItem { Value = lstSelecaoCd[i][0], Text = lstSelecaoCd[i][1] });
            }
            viewModel.ListaCD = new SelectList(lstCd, "Value", "Text");

            viewModel.ListaOperacoesPermitidas = usuarioLogado.S_lista_operacoes_permitidas;

            //viewModel.PercMaxPorLoja = await pedidoBll.BuscarPercMaxPorLoja(usuarioLogado.Loja_atual_id);

            viewModel.ComIndicacao = 0;

            //Montar o select do PedBonshop
            //List<string> lstPedidoBonshop = (await clienteBll.BuscarListaPedidosBonshop(cpf_cnpj)).ToList();
            //List<SelectListItem> lstPed = new List<SelectListItem>();
            //lstPed.Add(new SelectListItem { Value = "0", Text = "Selecione" });
            //for (int i = 0; i < lstPedidoBonshop.Count; i++)
            //{
            //    lstPed.Add(new SelectListItem { Value = lstPedidoBonshop[i], Text = lstPedidoBonshop[i] });
            //}
            viewModel.PedBonshop = new SelectList(lstCd, "Value", "Text");
            return View(viewModel);
        }


        public async Task<IActionResult> Cadastrar_Indicador_SelecaoCD(string comIndicacao, int cdAutomatico, int cdManual,
            int ListaCD, float percComissao, int comRA, string indicador)
        {
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);
            List<string> lstErros = new List<string>();

            //vamos validar os dados e armazenar na session
            if (await clienteBll.ValidarCliente(
                Util.SoDigitosCpf_Cnpj(usuarioLogado.Cliente_Selecionado.DadosCliente.Cnpj_Cpf)))
            {
                //vamos validar em outro lugar, pois esta grande
                List<string> lstRetorno = (await pedidoBll.ValidarIndicador_SelecaoCD(usuarioLogado.Loja_atual_id, usuarioLogado.PedidoDto.DadosCliente.Id,
                    usuarioLogado.Usuario_atual, usuarioLogado.S_lista_operacoes_permitidas,
                    Util.SoDigitosCpf_Cnpj(usuarioLogado.Cliente_Selecionado.DadosCliente.Cnpj_Cpf), int.Parse(comIndicacao),
                    cdAutomatico, cdManual, ListaCD, percComissao, comRA, indicador)).ToList();


                if (lstErros.Count > 0)
                {
                    //vamos verificar as msgs que estão retornando e retornar os erros

                }
                else
                {
                    //vamos atribuir para session
                    //afazer: o usuarioLogado não esta alterando os valores do pedidoDto
                    PedidoDto pedidoDto = new PedidoDto();
                    pedidoDto = usuarioLogado.PedidoDto;
                    pedidoDto.PercRT = percComissao;
                    pedidoDto.PermiteRAStatus = (short)comRA;
                    pedidoDto.OpcaoPossuiRA = comRA == 1 ? "S":"N";
                    pedidoDto.CDManual = cdManual == 0 ? (short)0 : (short)1;
                    pedidoDto.CDSelecionado = cdManual == 1 ? ListaCD : 0;
                    pedidoDto.ComIndicador = int.Parse(comIndicacao) != 0 ? 1 : 0;
                    pedidoDto.NomeIndicador = int.Parse(comIndicacao) == 1 ? indicador : null;


                    //afazer: PedBonShop
                    pedidoDto.PedBonshop = "";

                    usuarioLogado.PedidoDto = pedidoDto;


                }
            }


            return RedirectToAction("IniciarNovoPedido");
        }

        public async Task<IActionResult> CancelamentoAutomatico()
        {
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);

            return View(await CancelamentoAutomaticoDados(usuarioLogado, cancelamentoAutomaticoBll));
        }

        private static async Task<Loja.UI.Models.Pedido.CancelamentoAutomaticoViewModel> CancelamentoAutomaticoDados(UsuarioLogado usuarioLogado,
            CancelamentoAutomaticoBll cancelamentoAutomaticoBll)
        {
            var dadosTelaRetorno = await cancelamentoAutomaticoBll.DadosTela(usuarioLogado);
            var itensLoja = (from i in dadosTelaRetorno.cancelamentoAutomaticoItems group i by i.LojaId into g select new Models.Comuns.ListaLojasViewModel.ItemLoja { Loja = g.Key, NumeroItens = g.Count() });
            var model = new Loja.UI.Models.Pedido.CancelamentoAutomaticoViewModel(dadosTelaRetorno.cancelamentoAutomaticoItems,
                new Models.Comuns.ListaLojasViewModel(usuarioLogado, itensLoja.ToList()));
            //desligamos o combo de lojas
            model.ListaLojasViewModel.MostrarLoja = dadosTelaRetorno.consultaUniversalPedidoOrcamento;
            return model;
        }

        public async Task<IActionResult> ListarUltimosPedidos()
        {
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);

            var lista = await pedidoBll.ListaUltimosPedidos(usuarioLogado.Loja_atual_id);
            List<UltimosPedidosViewModel> model = new List<UltimosPedidosViewModel>();

            foreach (var i in lista)
            {
                model.Add(new UltimosPedidosViewModel
                {
                    Data = i.Data,
                    Pedido = i.Pedido,
                    St_Entrega = i.St_Entrega,
                    Vendedor = i.Vendedor,
                    CnpjCpf = i.CnpjCpf,
                    NomeIniciaisEmMaiusculas = i.NomeIniciaisEmMaiusculas,
                    AnaliseCredito = i.AnaliseCredito,
                    AnaliseCreditoPendenteVendasMotivo = i.AnaliseCreditoPendenteVendasMotivo
                });
            }

            return View(model);
        }
    }
}

