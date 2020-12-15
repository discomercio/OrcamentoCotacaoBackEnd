using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfraBanco;
using Loja.Bll.Bll.AcessoBll;
using Loja.Bll.ClienteBll;
using Loja.Bll.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Loja.UI.Controllers
{
    public class SiteColorsController : Controller
    {
        //para montar as URLs
        public static string UrlAction(IUrlHelper urlHelper, ListaPaginasColors pagina)
        {
            var ret = urlHelper.Action("Index", "SiteColors", new
            {
                //queremos um núemro pequeno e não-sequencial
                //pagina = (int)(Math.Abs(pagina.ToString().GetHashCode()))
                pagina = (int)pagina
            }); ;
            return ret;
        }
        public static string UrlAction(IUrlHelper urlHelper, ListaPaginasColors pagina, string param)
        {
            var ret = urlHelper.Action("Index", "SiteColors", new
            {
                //queremos um núemro pequeno e não-sequencial
                //pagina = (int)(Math.Abs(pagina.ToString().GetHashCode())),
                pagina = (int)pagina,
                param
            });
            return ret;
        }

        public enum ListaPaginasColors
        {
            //ATENÇÃO: NÃO MUDAR OS NOMES DESTE ENUM
            //eles não aparecem para o usuário, mas o hashcode da string faz parte das URLs. Se o usuário salva um favorito
            // e a gente mudar a string, o favorito vai parar de funcionar

            //Pedidos
            Pedidos_com_Credito_Pendente = 97531,
            Pedidos_com_Credito_Pendente_Vendas = 97513,
            Pedidos_Pendentes_Cartao_de_Credito = 97135,
            Pedidos_com_Endereco_Pendente = 97153,
            Pesquisa_pedidos_anteriormente_efetuados_por_um_cliente_nesta_loja = 97351,
            //Pré-Pedido
            Prepedido_em_Aberto = 86420,
            Relatorio_Multicriterio_Prepedido = 86402,
            //Produto -> afazer precisamos de tela para inserir dados para buscar
            //Relatórios/Pedidos
            Relatorio_de_Pedidos_Indicadores = 79531,
            Relatorio_de_Pedidos_Indicadores_Processado = 79513,
            Relatorio_de_Pedidos_Indicadores_Preview = 79351,
            Relatorio_Multicriterio_de_Pedidos = 79315,
            Pedidos_Colocados_no_Mes = 79153,
            Relatorio_de_Pedidos_Cancelados = 79135,
            //Relatórios/Marketplace
            Registro_de_Pedidos_de_Marketplace_Nao_Recebidos_Pelo_Cliente = 68420,
            Registro_de_Pedidos_de_Marketplace_Recebidos_Pelo_Cliente = 68402,
            //Relatórios/Vendas
            Vendas = 59731,
            Vendas_com_Desconto_Superior = 59713,
            Estoque_de_Venda_Antigo = 59371,
            Estoque_de_Vendas = 59317,
            Relatorio_Gerencial_de_Vendas = 59173,
            Relatorio_de_Vendas_por_Boleto = 59137,
            Meio_de_Divulgacao = 57931,
            //Faturamento
            Faturamento_Antigo = 48620,
            Faturamento = 48602,
            //Ocorrências e Chamados
            Relatorio_de_Pre_Devolucoes = 39751,
            Produtos_no_Estoque_de_Devolucao = 39715,
            Devolucao_de_Produtos = 39571,
            Relatorio_de_Divergencia_Cliente_Indicador = 39517,
            Ocorrencias = 39157,
            Relatorio_de_Estatisticas_de_Ocorrencias = 39175,
            Acompanhamento_de_Chamados = 37951,
            Relatorio_de_Chamados = 37915,
            Relatorio_de_Estatisticas_de_Chamados = 35971,
            //Vendedores/Indicadores
            Comissao_aos_Vendedores = 28640,
            Comissao_aos_Vendedores_Sintetico_Tabela_Progressiva = 28604,
            Comissao_aos_Vendedores_Analitico_Tabela_Progressiva = 28406,
            Pesquisa_de_Indicadores = 28460,
            Checagem_de_Novos_Parceiros = 28064,
            Relatorio_de_Metas_do_Indicador = 28046,
            Relatorio_de_Performance_por_Indicador = 26840,
            Relatorio_de_Indicadores_sem_Atividade_Recente = 26804,
            //INDICADORES SEM ATIVIDADE RECENTE
            Indicadores_sem_atividade_recente = 19753,
            //Outros
            Pesquisar_CEP = 08642,
            altera_senha = 08624,
            NOVO_PEDIDO_CADASTRO_DE_CLIENTES = 08426,
            INDICADORES = 08462,
            OUTRAS_FUNCOES_Ler_Quadro_de_Avisos_somente_nao_lidos = 08246,
            OUTRAS_FUNCOES_Ler_Quadro_de_Avisos_todos_os_avisos = 08264,
            OUTRAS_FUNCOES_Funcoes_Administrativas = 06842,

            //página do pedido
            Orcamento_asp = 96351, //precisa de parâmetro, orcamento_selecionado
            Pedido = 96315,

            pagina_inicial_do_colors = 96531
        }

        private readonly SiteColorsBll siteColorsBll;
        private readonly Configuracao configuracao;
        private readonly ClienteBll clienteBll;
        private readonly UsuarioAcessoBll usuarioAcessoBll;
        private readonly ILogger<UsuarioLogado> loggerUsuarioLogado;
        private readonly ContextoBdProvider contextoProvider;

        public SiteColorsController(SiteColorsBll siteColorsBll, Configuracao configuracao, ClienteBll clienteBll,
            UsuarioAcessoBll usuarioAcessoBll, ILogger<UsuarioLogado> loggerUsuarioLogado,
            ContextoBdProvider contextoProvider)
        {
            this.siteColorsBll = siteColorsBll;
            this.configuracao = configuracao;
            this.clienteBll = clienteBll;
            this.usuarioAcessoBll = usuarioAcessoBll;
            this.loggerUsuarioLogado = loggerUsuarioLogado;
            this.contextoProvider = contextoProvider;
        }
        public async Task<IActionResult> Index(int? pagina, string param)
        {
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);
            
            //é necessário mandar para TrataSessaoExpirada.asp
            if (usuarioLogado.SessaoAtiva)
            {
                //vamos verificar se SessionCtrlTicket = null e SessionCtrlDtHrLogon = null
                if (await usuarioLogado.SessionCtrlDtHrLogon(contextoProvider) == null ||
                    await usuarioLogado.SessionCtrlTicket(contextoProvider) == null)
                {
                    bool atualizou = await usuarioAcessoBll.AtualizarSessionCtrlTicket(usuarioLogado);

                    //se não atualizar???7
                    if (!atualizou)
                        return RedirectToAction("Index", "Home"); // ou podemos mostrar uma mensagem de erro
                }
            }

            ListaPaginasColors? paginaEnum = null;
            //a página é quem tem Math.Abs(pagina.ToString().GetHashCode())
            if (pagina.HasValue)
            {
                foreach (ListaPaginasColors i in Enum.GetValues(typeof(ListaPaginasColors)))
                {
                    if ((int)i == pagina)
                    {
                        paginaEnum = i;
                        break;
                    }
                }
            }

            var paginaUrl = paginaEnum switch
            {
                ListaPaginasColors.Pedidos_com_Credito_Pendente => "RelPedidosCredPendFiltro.asp",
                ListaPaginasColors.Pedidos_com_Credito_Pendente_Vendas => "RelPedidosCredPendVendasFiltro.asp",
                ListaPaginasColors.Pedidos_Pendentes_Cartao_de_Credito => "RelPedidosPendentesCartaoFiltro.asp",
                ListaPaginasColors.Pedidos_com_Endereco_Pendente => "RelPedidosEnderecoPendenteFiltro.asp",
                ListaPaginasColors.Pesquisa_pedidos_anteriormente_efetuados_por_um_cliente_nesta_loja => "RelPedidosAnteriores.asp",
                //Pré-pedidos
                ListaPaginasColors.Prepedido_em_Aberto => "OrcamentosEmAberto.asp",
                ListaPaginasColors.Relatorio_Multicriterio_Prepedido => "RelOrcamentosMCrit.asp",
                //Produtos => afazer
                //Relatórios/Pedidos
                ListaPaginasColors.Relatorio_de_Pedidos_Indicadores => "RelComissaoIndicadores.asp",
                ListaPaginasColors.Relatorio_de_Pedidos_Indicadores_Processado => "RelComissaoIndicadoresConsultaPagos.asp",
                ListaPaginasColors.Relatorio_de_Pedidos_Indicadores_Preview => "RelComissaoIndicadoresConsultaExec.asp",
                ListaPaginasColors.Relatorio_Multicriterio_de_Pedidos => "RelPedidosMCrit.asp",
                ListaPaginasColors.Pedidos_Colocados_no_Mes => "RelPedidosColocados.asp",
                ListaPaginasColors.Relatorio_de_Pedidos_Cancelados => "RelPedidoCancelado.asp",
                //Relatórios/Marketplace
                ListaPaginasColors.Registro_de_Pedidos_de_Marketplace_Nao_Recebidos_Pelo_Cliente => "RelPedidosMktplaceNaoRecebidos.asp",
                ListaPaginasColors.Registro_de_Pedidos_de_Marketplace_Recebidos_Pelo_Cliente => "RelPedidosMktplaceRecebidos.asp",
                //Relatórios/Vendas
                ListaPaginasColors.Vendas => "RelVendasVariante.asp",
                ListaPaginasColors.Vendas_com_Desconto_Superior => "FiltroPeriodo.asp?pagina_destino=RelVendasAbaixoMin.asp&titulo_relatorio=Vendas com Desconto Superior&filtro_obrigatorio_data_inicio=S&filtro_obrigatorio_data_termino=S",
                ListaPaginasColors.Estoque_de_Venda_Antigo => "RelPosicaoEstoque.asp",
                ListaPaginasColors.Estoque_de_Vendas => "RelEstoqueVendaCmvPv.asp",
                ListaPaginasColors.Relatorio_Gerencial_de_Vendas => "RelGerencialVendasFiltro.asp",
                ListaPaginasColors.Relatorio_de_Vendas_por_Boleto => "RelVendasPorBoletoFiltro.asp",
                ListaPaginasColors.Meio_de_Divulgacao => "RelPedidosColocadosMidia.asp",
                //Faturamento
                ListaPaginasColors.Faturamento_Antigo => "RelVendas.asp",
                ListaPaginasColors.Faturamento => "RelVendasCmvPv.asp",
                //Ocorrências e Chamados
                ListaPaginasColors.Relatorio_de_Pre_Devolucoes => "RelPedidoPreDevolucao.asp",
                ListaPaginasColors.Produtos_no_Estoque_de_Devolucao => "RelEstoqueDevolucaoFiltro.asp",
                ListaPaginasColors.Devolucao_de_Produtos => "RelDevolucao.asp",
                ListaPaginasColors.Relatorio_de_Divergencia_Cliente_Indicador => "RelDivergenciaClienteIndicadorFiltro.asp",
                ListaPaginasColors.Ocorrencias => "RelPedidoOcorrenciaExec.asp",
                ListaPaginasColors.Relatorio_de_Estatisticas_de_Ocorrencias => "RElPedidoOcorrenciaEstatisticasFiltro.asp",
                ListaPaginasColors.Acompanhamento_de_Chamados => "RelAcompanhamentoChamadosFiltro.asp",
                ListaPaginasColors.Relatorio_de_Chamados => "RelPedidoChamadoFiltro.asp",
                ListaPaginasColors.Relatorio_de_Estatisticas_de_Chamados => "RelPedidoChamadoEstatisticasFiltro.asp",
                //Vendedores/Indicadores
                ListaPaginasColors.Comissao_aos_Vendedores => "RelComissao.asp",
                ListaPaginasColors.Comissao_aos_Vendedores_Sintetico_Tabela_Progressiva => "RelComissaoTabelaProgressivaSintetico.asp",
                ListaPaginasColors.Comissao_aos_Vendedores_Analitico_Tabela_Progressiva => "RelComissaoTabelaProgressivaAnalitico.asp",
                ListaPaginasColors.Pesquisa_de_Indicadores => "PesquisaDeIndicadoresFiltro.asp",
                ListaPaginasColors.Checagem_de_Novos_Parceiros => "RelChecagemNovosParceirosFiltro.asp",
                ListaPaginasColors.Relatorio_de_Metas_do_Indicador => "RelMetasIndicadorFiltro.asp",
                ListaPaginasColors.Relatorio_de_Performance_por_Indicador => "RelPerformanceIndicadorFiltro.asp",
                ListaPaginasColors.Relatorio_de_Indicadores_sem_Atividade_Recente => "RelIndicadoresSemAtivRec.asp",
                //INDICADORES SEM ATIVIDADE RECENTE
                ListaPaginasColors.Indicadores_sem_atividade_recente => "RelIndicadoresSemAtivRec.asp",
                //Outros
                ListaPaginasColors.Pesquisar_CEP => "../Global/AjaxCepPesqPopup.asp?ModoApenasConsulta=S",
                ListaPaginasColors.altera_senha => "senha.asp",
                ListaPaginasColors.NOVO_PEDIDO_CADASTRO_DE_CLIENTES => "",//esse já temos no Pedido/Novo Pedido
                ListaPaginasColors.INDICADORES => "",//precisa de tela para fazer a busca
                ListaPaginasColors.OUTRAS_FUNCOES_Ler_Quadro_de_Avisos_somente_nao_lidos => "quadroavisomostra.asp",
                //para a página abaixo será necessário passar parâmetro na url
                ListaPaginasColors.OUTRAS_FUNCOES_Ler_Quadro_de_Avisos_todos_os_avisos => "quadroavisomostra.asp?opcao_selecionada=S",
                ListaPaginasColors.OUTRAS_FUNCOES_Funcoes_Administrativas => "MenuFuncoesAdministrativas.asp",

                //Pedidos
                ListaPaginasColors.Pedido => "pedido.asp?pedido_selecionado=" + param ?? "",
                ListaPaginasColors.Orcamento_asp => "Orcamento.asp?orcamento_selecionado=" + param ?? "",

                ListaPaginasColors.pagina_inicial_do_colors => "resumo.asp",
                _ => "resumo.asp",
            };


            var sessionCtrlInfo = await siteColorsBll.MontaSessionCtrlInfo(usuarioLogado);

            var model = new Models.SiteColors.SiteColorsViewModel(sessionCtrlInfo, paginaUrl, configuracao);

            return View(model);
        }
    }
}