using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                //convertemos para int para não passar o texto
                pagina = (int)pagina
            });
            return ret;
        }
        public enum ListaPaginasColors
        {
            //Pedidos
            Pedidos_com_Credito_Pendente,
            Pedidos_com_Credito_Pendente_Vendas,
            Pedidos_Pendentes_Cartao_de_Credito,
            Pedidos_com_Endereco_Pendente,
            Pesquisa_pedidos_anteriormente_efetuados_por_um_cliente_nesta_loja,
            //Pré-Pedido
            Prepedido_em_Aberto,
            Relatorio_Multicriterio_Prepedido,
            //Produto -> afazer precisamos de tela para inserir dados para buscar
            //Relatórios/Pedidos
            Relatorio_de_Pedidos_Indicadores,
            Relatorio_de_Pedidos_Indicadores_Processado,
            Relatorio_de_Pedidos_Indicadores_Preview,
            Relatorio_Multicriterio_de_Pedidos,
            Pedidos_Colocados_no_Mes,
            Relatorio_de_Pedidos_Cancelados,
            //Relatórios/Marketplace
            Registro_de_Pedidos_de_Marketplace_Nao_Recebidos_Pelo_Cliente,
            Registro_de_Pedidos_de_Marketplace_Recebidos_Pelo_Cliente,
            //Relatórios/Vendas
            Vendas,
            Vendas_com_Desconto_Superior,
            Estoque_de_Venda_Antigo,
            Estoque_de_Vendas,
            Relatorio_Gerencial_de_Vendas,
            Relatorio_de_Vendas_por_Boleto,
            Meio_de_Divulgacao,
            //Faturamento
            Faturamento_Antigo,
            Faturamento,
            //Ocorrências e Chamados
            Relatorio_de_Pre_Devolucoes,
            Produtos_no_Estoque_de_Devolucao,
            Devolucao_de_Produtos,
            Relatorio_de_Divergencia_Cliente_Indicador,
            Ocorrencias,
            Relatorio_de_Estatisticas_de_Ocorrencias,
            Acompanhamento_de_Chamados,
            Relatorio_de_Chamados,
            Relatorio_de_Estatisticas_de_Chamados,
            //Vendedores/Indicadores
            Comissao_aos_Vendedores,
            Comissao_aos_Vendedores_Sintetico_Tabela_Progressiva,
            Comissao_aos_Vendedores_Analitico_Tabela_Progressiva,
            Pesquisa_de_Indicadores,
            Checagem_de_Novos_Parceiros,
            Relatorio_de_Metas_do_Indicador,
            Relatorio_de_Performance_por_Indicador,
            Relatorio_de_Indicadores_sem_Atividade_Recente,
            //INDICADORES SEM ATIVIDADE RECENTE
            Indicadores_sem_atividade_recente,
            //Outros
            Pesquisar_CEP,
            altera_senha,
            NOVO_PEDIDO_CADASTRO_DE_CLIENTES,
            INDICADORES,
            OUTRAS_FUNCOES_Ler_Quadro_de_Avisos_somente_nao_lidos,
            OUTRAS_FUNCOES_Ler_Quadro_de_Avisos_todos_os_avisos,
            OUTRAS_FUNCOES_Funcoes_Administrativas,
            pagina_inicial_do_colors
        }

        private readonly SiteColorsBll siteColorsBll;
        private readonly Configuracao configuracao;
        private readonly ClienteBll clienteBll;
        private readonly UsuarioAcessoBll usuarioAcessoBll;
        private readonly ILogger<UsuarioLogado> loggerUsuarioLogado;

        public SiteColorsController(SiteColorsBll siteColorsBll, Configuracao configuracao, ClienteBll clienteBll,
            UsuarioAcessoBll usuarioAcessoBll,
            ILogger<UsuarioLogado> loggerUsuarioLogado)
        {
            this.siteColorsBll = siteColorsBll;
            this.configuracao = configuracao;
            this.clienteBll = clienteBll;
            this.usuarioAcessoBll = usuarioAcessoBll;
            this.loggerUsuarioLogado = loggerUsuarioLogado;
        }
        public async Task<IActionResult> Index(ListaPaginasColors? pagina)
        {
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);

            var sessionCtrlInfo = await siteColorsBll.MontaSessionCtrlInfo(usuarioLogado);
            var paginaUrl = pagina switch
            {
                //Pedidos
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
                ListaPaginasColors.pagina_inicial_do_colors => "resumo.asp",
                _ => "resumo.asp",
            };
            var model = new Models.SiteColors.SiteColorsViewModel(sessionCtrlInfo, paginaUrl, configuracao);

            return View(model);
        }
    }
}