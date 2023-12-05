using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.EntityFrameworkCore;
using Relatorios.Dto;
using Relatorios.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InfraBanco.Constantes.Constantes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Relatorios
{
    public class RelatoriosData
    {
        private readonly ContextoRelatorioProvider contexto;

        public RelatoriosData(ContextoRelatorioProvider _contexto)
        {
            contexto = _contexto;
        }

        public List<ItensOrcamentoDto> RelatorioItensOrcamento(ItensOrcamentosFiltro obj)
        {
            var response = new List<ItensOrcamentoDto>();
            using (var db = contexto.GetContextoLeitura())
            {
                using (var command = db.Database.GetDbConnection().CreateCommand())
                {
                    string sql = "SELECT tOC.Loja, tOC.Id AS Orcamento, tOC.IdOrcamento AS PrePedido, (SELECT TOP(1) pedido FROM dbo.t_PEDIDO tPedAux WHERE (tPedAux.pedido = tPedAux.pedido_base) AND (tPedAux.IdOrcamentoCotacao = tOC.Id) ORDER BY tPedAux.data_hora DESC) AS Pedido, (CASE WHEN (tOC.Status = 1) AND (tOC.Validade < Convert(date, getdate())) THEN 'Expirado' ELSE tCfgSt.Descricao END) AS Status, tVendedor.usuario AS Vendedor, tParceiro.apelido AS Indicador, tParceiroVendedor.Nome AS IndicadorVendedor, (CASE WHEN tOC.IdTipoUsuarioContextoCadastro = 1 THEN tUsuarioIntCadastro.usuario WHEN tOC.IdTipoUsuarioContextoCadastro = 2 THEN tParceiroCadastro.apelido WHEN tOC.IdTipoUsuarioContextoCadastro = 3 THEN 'VP ' + tParceiroVendedorCadastro.Nome END) AS UsuarioCadastro, (SELECT tPrePedAux.id_cliente FROM dbo.t_ORCAMENTO tPrePedAux WHERE tPrePedAux.IdOrcamentoCotacao = tOC.Id) AS IdCliente, tOC.UF, tOC.TipoCliente, (SELECT tCDAux.descricao FROM dbo.t_CODIGO_DESCRICAO tCDAux WHERE (tCDAux.grupo = 'Cliente_Contribuinte_Icms_Status') AND (tCDAux.codigo = tOC.ContribuinteIcms)) AS ContribuinteIcms, (CASE tOC.StEtgImediata WHEN 1 THEN 'Não' WHEN 2 THEN 'Sim' ELSE Convert(varchar(2), tOC.StEtgImediata) + ' - Desconhecido' END) AS EntregaImediata, (CASE tOC.StEtgImediata WHEN 1 THEN tOC.PrevisaoEntregaData ELSE NULL END) AS PrevisaoEntrega, (CASE tOC.InstaladorInstalaStatus WHEN 1 THEN 'Não' WHEN 2 THEN 'Sim' ELSE Convert(varchar(2), tOC.InstaladorInstalaStatus) + ' - Desconhecido' END) AS InstaladorInstala, tOpc.Sequencia AS NumOpcaoOrcamento, (SELECT tMeioPagAux.Descricao FROM dbo.t_ORCAMENTO_COTACAO_OPCAO tOpcAux INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_PAGTO tPagAux ON tOpcAux.Id = tPagAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_CFG_PAGTO_MEIO tMeioPagAux ON (tMeioPagAux.Id = tPagAux.av_forma_pagto) WHERE (tOpcAux.IdOrcamentoCotacao = tOC.Id) AND (tPagAux.Habilitado = 1) AND (tPagAux.tipo_parcelamento = 1) AND (tOpcAux.Id = tOpc.Id)) AS FormaPagtoAVista, (SELECT tFormaPagAux.Descricao FROM dbo.t_ORCAMENTO_COTACAO_OPCAO tOpcAux INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_PAGTO tPagAux ON tOpcAux.Id = tPagAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_CFG_PAGTO_FORMA tFormaPagAux ON tPagAux.tipo_parcelamento = tFormaPagAux.Id WHERE (tOpcAux.IdOrcamentoCotacao = tOC.Id) AND (tPagAux.Habilitado = 1) AND (tPagAux.tipo_parcelamento <> 1) AND (tOpcAux.Id = tOpc.Id)) AS FormaPagtoAPrazo, (SELECT CASE tPagAux.tipo_parcelamento WHEN 5 THEN 1 WHEN 2 THEN tPagAux.pc_qtde_parcelas WHEN 6 THEN tPagAux.pc_maquineta_qtde_parcelas WHEN 3 THEN (1 + tPagAux.pce_prestacao_qtde) WHEN 4 THEN (1 + tPagAux.pse_demais_prest_qtde) END FROM dbo.t_ORCAMENTO_COTACAO_OPCAO tOpcAux INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_PAGTO tPagAux ON tOpcAux.Id = tPagAux.IdOrcamentoCotacaoOpcao WHERE (tOpcAux.IdOrcamentoCotacao = tOC.Id) AND (tPagAux.Habilitado = 1) AND (tPagAux.tipo_parcelamento <> 1) AND (tOpcAux.Id = tOpc.Id)) AS QtdeParcelasFormaPagtoAPrazo, (CASE WHEN (SELECT Sequencia FROM dbo.t_ORCAMENTO_COTACAO_OPCAO tOpcAux INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_PAGTO tPagAux ON tOpcAux.Id = tPagAux.IdOrcamentoCotacaoOpcao WHERE (tOpcAux.IdOrcamentoCotacao = tOC.Id) AND (tPagAux.Aprovado = 1) AND (tOpcAux.Id = tOpc.Id)) IS NOT NULL THEN 'Sim' ELSE 'Não' END) AS OpcaoAprovada, (CASE WHEN (SELECT tPagAux.tipo_parcelamento FROM dbo.t_ORCAMENTO_COTACAO_OPCAO tOpcAux INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_PAGTO tPagAux ON tOpcAux.Id = tPagAux.IdOrcamentoCotacaoOpcao WHERE (tOpcAux.IdOrcamentoCotacao = tOC.Id) AND (tPagAux.Aprovado = 1) AND (tOpcAux.Id = tOpc.Id)) IS NULL THEN '' WHEN (SELECT tPagAux.tipo_parcelamento FROM dbo.t_ORCAMENTO_COTACAO_OPCAO tOpcAux INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_PAGTO tPagAux ON tOpcAux.Id = tPagAux.IdOrcamentoCotacaoOpcao WHERE (tOpcAux.IdOrcamentoCotacao = tOC.Id) AND (tPagAux.Aprovado = 1) AND (tOpcAux.Id = tOpc.Id)) = 1 THEN 'Á Vista' ELSE 'A Prazo' END) AS FormaPagtoOpcaoAprovada, tFabr.nome AS Fabricante, tUnif.Produto, tUnif.Qtde, (CASE WHEN tECComp.descricao IS NOT NULL THEN tECComp.descricao WHEN tProdNormal.descricao IS NOT NULL THEN tProdNormal.descricao ELSE '' END) AS DescricaoProduto, (CASE WHEN (tProdGrupo.descricao IS NULL) OR (tProdSubgrupo.descricao IS NULL) THEN Coalesce(tProdGrupo.descricao, '') + Coalesce(tProdSubgrupo.descricao, '') WHEN (tProdGrupo.descricao = tProdSubgrupo.descricao) THEN tProdGrupo.descricao ELSE tProdGrupo.descricao + ' - ' + tProdSubgrupo.descricao END) AS Categoria, (CASE WHEN (tProdGrupo.descricao IS NULL) OR (tProdSubgrupo.descricao IS NULL) THEN Coalesce(tProdGrupo.codigo + '§', '') + Coalesce('§' + tProdSubgrupo.codigo,'') WHEN (tProdGrupo.descricao = tProdSubgrupo.descricao) THEN tProdGrupo.codigo + '§' + tProdSubgrupo.codigo ELSE tProdGrupo.codigo + '§' + tProdSubgrupo.codigo END) AS GrupoSubgrupo, (SELECT SUM(tAtomAux.Qtde * tCustoFinAux.PrecoLista) FROM dbo.t_ORCAMENTO_COTACAO_OPCAO tOpcAux INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO tUnifAux ON tOpcAux.Id = tUnifAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_PAGTO tPagAux ON tOpcAux.Id = tPagAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO tAtomAux ON tUnifAux.Id = tAtomAux.IdItemUnificado INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN tCustoFinAux ON (tAtomAux.Id = tCustoFinAux.IdItemAtomico) AND (tPagAux.Id = tCustoFinAux.IdOpcaoPagto) WHERE (tOpcAux.IdOrcamentoCotacao = tOC.Id) AND (tPagAux.Habilitado = 1) AND (tPagAux.tipo_parcelamento = 1) AND (tOpcAux.Id = tOpc.Id) AND (tAtomAux.IdItemUnificado = tUnif.Id)) AS PrecoListaUnitAVista, (SELECT SUM(tAtomAux.Qtde * tCustoFinAux.PrecoLista) FROM dbo.t_ORCAMENTO_COTACAO_OPCAO tOpcAux INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO tUnifAux ON tOpcAux.Id = tUnifAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_PAGTO tPagAux ON tOpcAux.Id = tPagAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO tAtomAux ON tUnifAux.Id = tAtomAux.IdItemUnificado INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN tCustoFinAux ON (tAtomAux.Id = tCustoFinAux.IdItemAtomico) AND (tPagAux.Id = tCustoFinAux.IdOpcaoPagto) WHERE (tOpcAux.IdOrcamentoCotacao = tOC.Id) AND (tPagAux.Habilitado = 1) AND (tPagAux.tipo_parcelamento <> 1) AND (tOpcAux.Id = tOpc.Id) AND (tAtomAux.IdItemUnificado = tUnif.Id)) AS PrecoListaUnitAPrazo, (SELECT SUM(tAtomAux.Qtde * tCustoFinAux.PrecoNF) FROM dbo.t_ORCAMENTO_COTACAO_OPCAO tOpcAux INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO tUnifAux ON tOpcAux.Id = tUnifAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_PAGTO tPagAux ON tOpcAux.Id = tPagAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO tAtomAux ON tUnifAux.Id = tAtomAux.IdItemUnificado INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN tCustoFinAux ON (tAtomAux.Id = tCustoFinAux.IdItemAtomico) AND (tPagAux.Id = tCustoFinAux.IdOpcaoPagto) WHERE (tOpcAux.IdOrcamentoCotacao = tOC.Id) AND (tPagAux.Habilitado = 1) AND (tPagAux.tipo_parcelamento = 1) AND (tOpcAux.Id = tOpc.Id) AND (tAtomAux.IdItemUnificado = tUnif.Id)) AS PrecoNFUnitAVista, (SELECT SUM(tAtomAux.Qtde * tCustoFinAux.PrecoNF) FROM dbo.t_ORCAMENTO_COTACAO_OPCAO tOpcAux INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO tUnifAux ON tOpcAux.Id = tUnifAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_PAGTO tPagAux ON tOpcAux.Id = tPagAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO tAtomAux ON tUnifAux.Id = tAtomAux.IdItemUnificado INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN tCustoFinAux ON (tAtomAux.Id = tCustoFinAux.IdItemAtomico) AND (tPagAux.Id = tCustoFinAux.IdOpcaoPagto) WHERE (tOpcAux.IdOrcamentoCotacao = tOC.Id) AND (tPagAux.Habilitado = 1) AND (tPagAux.tipo_parcelamento <> 1) AND (tOpcAux.Id = tOpc.Id) AND (tAtomAux.IdItemUnificado = tUnif.Id)) AS PrecoNFUnitAPrazo, (SELECT CASE WHEN (Coalesce(SUM(tAtomAux.Qtde * tCustoFinAux.PrecoLista), 0) <> 0) THEN (100*((SUM(tAtomAux.Qtde * tCustoFinAux.PrecoLista) - SUM(tAtomAux.Qtde * tCustoFinAux.PrecoNF)) / SUM(tAtomAux.Qtde * tCustoFinAux.PrecoLista))) ELSE NULL END FROM dbo.t_ORCAMENTO_COTACAO_OPCAO tOpcAux INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO tUnifAux ON tOpcAux.Id = tUnifAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_PAGTO tPagAux ON tOpcAux.Id = tPagAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO tAtomAux ON tUnifAux.Id = tAtomAux.IdItemUnificado INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN tCustoFinAux ON (tAtomAux.Id = tCustoFinAux.IdItemAtomico) AND (tPagAux.Id = tCustoFinAux.IdOpcaoPagto) WHERE (tOpcAux.IdOrcamentoCotacao = tOC.Id) AND (tPagAux.Habilitado = 1) AND (tPagAux.tipo_parcelamento = 1) AND (tOpcAux.Id = tOpc.Id) AND (tAtomAux.IdItemUnificado = tUnif.Id) AND (tPagAux.Id IN (SELECT tCustoFinAux2.IdOpcaoPagto FROM dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN tCustoFinAux2 WHERE (tCustoFinAux2.IdOpcaoPagto = tCustoFinAux.IdOpcaoPagto) AND (tAtomAux.IdItemUnificado = tUnif.Id)))) AS DescontoAVista, (SELECT CASE WHEN (Coalesce(SUM(tAtomAux.Qtde * tCustoFinAux.PrecoLista), 0) <> 0) THEN (100*((SUM(tAtomAux.Qtde * tCustoFinAux.PrecoLista) - SUM(tAtomAux.Qtde * tCustoFinAux.PrecoNF)) / SUM(tAtomAux.Qtde * tCustoFinAux.PrecoLista))) ELSE NULL END FROM dbo.t_ORCAMENTO_COTACAO_OPCAO tOpcAux INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO tUnifAux ON tOpcAux.Id = tUnifAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_PAGTO tPagAux ON tOpcAux.Id = tPagAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO tAtomAux ON tUnifAux.Id = tAtomAux.IdItemUnificado INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN tCustoFinAux ON (tAtomAux.Id = tCustoFinAux.IdItemAtomico) AND (tPagAux.Id = tCustoFinAux.IdOpcaoPagto) WHERE (tOpcAux.IdOrcamentoCotacao = tOC.Id) AND (tPagAux.Habilitado = 1) AND (tPagAux.tipo_parcelamento <> 1) AND (tOpcAux.Id = tOpc.Id) AND (tAtomAux.IdItemUnificado = tUnif.Id) AND (tPagAux.Id IN (SELECT tCustoFinAux2.IdOpcaoPagto FROM dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN tCustoFinAux2 WHERE (tCustoFinAux2.IdOpcaoPagto = tCustoFinAux.IdOpcaoPagto) AND (tAtomAux.IdItemUnificado = tUnif.Id)))) AS DescontoAPrazo, (SELECT CASE WHEN (Coalesce(SUM(tAtomAux.Qtde * tCustoFinAux.PrecoLista), 0) <> 0) THEN (100*((SUM(tAtomAux.Qtde * tCustoFinAux.PrecoLista) - SUM(tAtomAux.Qtde * tCustoFinAux.PrecoNF)) / SUM(tAtomAux.Qtde * tCustoFinAux.PrecoLista))) ELSE NULL END FROM dbo.t_ORCAMENTO_COTACAO_OPCAO tOpcAux INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO tUnifAux ON tOpcAux.Id = tUnifAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_PAGTO tPagAux ON tOpcAux.Id = tPagAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO tAtomAux ON tUnifAux.Id = tAtomAux.IdItemUnificado INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN tCustoFinAux ON (tAtomAux.Id = tCustoFinAux.IdItemAtomico) AND (tPagAux.Id = tCustoFinAux.IdOpcaoPagto) WHERE (tOpcAux.IdOrcamentoCotacao = tOC.Id) AND (tPagAux.Habilitado = 1) AND (tPagAux.tipo_parcelamento = 1) AND (tOpcAux.Id = tOpc.Id) AND (tAtomAux.IdItemUnificado = tUnif.Id) AND (tPagAux.Id IN (SELECT tCustoFinAux2.IdOpcaoPagto FROM dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN tCustoFinAux2 WHERE (tCustoFinAux2.IdOpcaoPagto = tCustoFinAux.IdOpcaoPagto) AND (tAtomAux.IdItemUnificado = tUnif.Id) AND (tCustoFinAux2.StatusDescontoSuperior <> 0)))) AS DescSuperiorAVista, (SELECT CASE WHEN (Coalesce(SUM(tAtomAux.Qtde * tCustoFinAux.PrecoLista), 0) <> 0) THEN (100*((SUM(tAtomAux.Qtde * tCustoFinAux.PrecoLista) - SUM(tAtomAux.Qtde * tCustoFinAux.PrecoNF)) / SUM(tAtomAux.Qtde * tCustoFinAux.PrecoLista))) ELSE NULL END FROM dbo.t_ORCAMENTO_COTACAO_OPCAO tOpcAux INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO tUnifAux ON tOpcAux.Id = tUnifAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_PAGTO tPagAux ON tOpcAux.Id = tPagAux.IdOrcamentoCotacaoOpcao INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO tAtomAux ON tUnifAux.Id = tAtomAux.IdItemUnificado INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN tCustoFinAux ON (tAtomAux.Id = tCustoFinAux.IdItemAtomico) AND (tPagAux.Id = tCustoFinAux.IdOpcaoPagto) WHERE (tOpcAux.IdOrcamentoCotacao = tOC.Id) AND (tPagAux.Habilitado = 1) AND (tPagAux.tipo_parcelamento <> 1) AND (tOpcAux.Id = tOpc.Id) AND (tAtomAux.IdItemUnificado = tUnif.Id) AND (tPagAux.Id IN (SELECT tCustoFinAux2.IdOpcaoPagto FROM dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN tCustoFinAux2 WHERE (tCustoFinAux2.IdOpcaoPagto = tCustoFinAux.IdOpcaoPagto) AND (tAtomAux.IdItemUnificado = tUnif.Id) AND (tCustoFinAux2.StatusDescontoSuperior <> 0)))) AS DescSuperiorAPrazo, (SELECT tOpcAux.PercRT FROM dbo.t_ORCAMENTO_COTACAO_OPCAO tOpcAux WHERE (tOpcAux.IdOrcamentoCotacao = tOC.Id) AND (tOpcAux.Id = tOpc.Id)) AS Comissao, Convert(date, tOC.DataCadastro) AS DataCadastro, Convert(date, tOC.Validade) AS Validade FROM dbo.t_ORCAMENTO_COTACAO tOC INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO tOpc ON tOC.Id = tOpc.IdOrcamentoCotacao INNER JOIN dbo.t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO tUnif ON tOpc.Id = tUnif.IdOrcamentoCotacaoOpcao LEFT JOIN dbo.t_PRODUTO tProdNormal ON (tUnif.Fabricante = tProdNormal.fabricante) AND (tUnif.Produto = tProdNormal.produto) LEFT JOIN dbo.t_EC_PRODUTO_COMPOSTO tECComp ON (tUnif.Fabricante = tECComp.fabricante_composto) AND (tUnif.Produto = tECComp.produto_composto) LEFT JOIN dbo.t_FABRICANTE tFabr ON tUnif.Fabricante = tFabr.fabricante LEFT JOIN dbo.t_PRODUTO_GRUPO tProdGrupo ON tProdNormal.grupo = tProdGrupo.codigo LEFT JOIN dbo.t_PRODUTO_SUBGRUPO tProdSubgrupo ON tProdNormal.subgrupo = tProdSubgrupo.codigo LEFT JOIN dbo.t_CFG_ORCAMENTO_COTACAO_STATUS tCfgSt ON tOC.Status = tCfgSt.Id LEFT JOIN dbo.t_USUARIO tVendedor ON tOC.IdVendedor = tVendedor.Id LEFT JOIN dbo.t_ORCAMENTISTA_E_INDICADOR tParceiro ON tOC.IdIndicador = tParceiro.Id LEFT JOIN dbo.t_ORCAMENTISTA_E_INDICADOR_VENDEDOR tParceiroVendedor ON tOC.IdIndicadorVendedor = tParceiroVendedor.Id LEFT JOIN dbo.t_USUARIO tUsuarioIntCadastro ON (tOC.IdTipoUsuarioContextoCadastro = 1) AND (tOC.IdUsuarioCadastro = tUsuarioIntCadastro.Id) LEFT JOIN dbo.t_ORCAMENTISTA_E_INDICADOR tParceiroCadastro ON (tOC.IdTipoUsuarioContextoCadastro = 2) AND (tOC.IdUsuarioCadastro = tParceiroCadastro.Id) LEFT JOIN dbo.t_ORCAMENTISTA_E_INDICADOR_VENDEDOR tParceiroVendedorCadastro ON (tOC.IdTipoUsuarioContextoCadastro = 3) AND (tOC.IdUsuarioCadastro = tParceiroVendedorCadastro.Id) ";
                    //
                    //ORDER BY tOC.Id, tOpc.Sequencia, tUnif.Sequencia";

                    sql = sql + "WHERE (tOC.Status <> 5)";

                    if (obj.DtInicio.HasValue)
                    {
                        sql = sql + $" AND (tOc.DataCadastro >= '{obj.DtInicio.Value.ToString("yyyy-MM-dd")}')";
                    }
                    if (obj.DtFim.HasValue)
                    {
                        sql = sql + $" AND (tOc.DataCadastro <= '{obj.DtFim.Value.ToString("yyyy-MM-dd")}')";
                    }
                    if (obj.DtInicioExpiracao.HasValue)
                    {
                        sql = sql + $" AND (tOc.Validade >= '{obj.DtInicioExpiracao.Value.ToString("yyyy-MM-dd")}')";
                    }
                    if (obj.DtFimExpiracao.HasValue)
                    {
                        sql = sql + $" AND (tOc.Validade <= '{obj.DtFimExpiracao.Value.ToString("yyyy-MM-dd")}')";
                    }
                    if (obj.Lojas?.Count() > 0)
                    {
                        sql = sql + $" AND (tOc.Loja IN ({String.Join(", ", obj.Lojas)}))";
                    }
                    if (obj.Status?.Count() > 0)
                    {
                        sql = sql + $" AND (tOc.Status IN ({String.Join(",", obj.Status)}))";
                    }
                    if (obj.Vendedores?.Count() > 0)
                    {
                        sql = sql + $" AND (tOc.IdVendedor IN ({String.Join(",", obj.Vendedores)}))";
                    }
                    if (obj.ComIndicador != null && obj.ComIndicador == true)
                    {
                        sql = sql + $" AND (tOc.IdIndicador IS NOT NULL)";
                    }
                    if (obj.ComIndicador != null && obj.ComIndicador == false)
                    {
                        sql = sql + $" AND (tOc.IdIndicador IS NULL)";
                    }
                    if (obj.Parceiros?.Count() > 0)
                    {
                        sql = sql + $" AND (tOc.IdIndicador IN ({String.Join(", ", obj.Parceiros)}))";
                    }
                    if (obj.Fabricantes?.Count() > 0)
                    {
                        sql = sql + $" AND (tFabr.fabricante in ({String.Join(", ", obj.Fabricantes)}))";
                    }


                    sql = sql + " ORDER BY tOC.Id, tOpc.Sequencia, tUnif.Sequencia";

                    command.CommandText = sql;

                    db.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            var item = new ItensOrcamentoDto();
                            item.Loja = result["Loja"]?.ToString();
                            item.Orcamento = int.Parse(result["Orcamento"]?.ToString());
                            item.Prepedido = result["Prepedido"]?.ToString();
                            item.Pedido = result["Pedido"]?.ToString();
                            item.Status = result["Status"]?.ToString();
                            item.Vendedor = result["Vendedor"]?.ToString();
                            item.Parceiro = result["Indicador"]?.ToString();
                            item.VendedorParceiro = result["IndicadorVendedor"]?.ToString();
                            item.UsuarioCadastro = result["UsuarioCadastro"]?.ToString();
                            item.IdCliente = result["IdCliente"]?.ToString();
                            item.UF = result["UF"]?.ToString();
                            item.TipoCliente = result["TipoCliente"]?.ToString();
                            item.ContribuinteIcms = result["ContribuinteIcms"]?.ToString();
                            item.EntregaImediata = result["EntregaImediata"]?.ToString();
                            item.PrevisaoEntrega = result["PrevisaoEntrega"]?.ToString();
                            item.InstaladorInstala = result["InstaladorInstala"]?.ToString();
                            item.NumOpcaoOrcamento = int.Parse(result["NumOpcaoOrcamento"]?.ToString());
                            item.FormaPagtoAVista = result["FormaPagtoAVista"]?.ToString();
                            item.FormaPagtoAPrazo = result["FormaPagtoAPrazo"]?.ToString();
                            item.QtdeParcelasFormaPagtoAPrazo = !string.IsNullOrEmpty(result["QtdeParcelasFormaPagtoAPrazo"].ToString()) ? int.Parse(result["QtdeParcelasFormaPagtoAPrazo"].ToString()) : int.Parse("0");
                            item.OpcaoAprovada = result["OpcaoAprovada"]?.ToString();
                            item.FormaPagtoOpcaoAprovada = result["FormaPagtoOpcaoAprovada"]?.ToString();
                            item.Fabricante = result["Fabricante"]?.ToString();
                            item.Produto = result["Produto"]?.ToString();
                            item.Qtde = int.Parse(result["Qtde"]?.ToString());
                            item.Descricao = result["DescricaoProduto"]?.ToString();
                            item.Categoria = result["Categoria"]?.ToString();
                            item.GrupoSubgrupo = result["GrupoSubgrupo"]?.ToString();
                            item.PrecoListaUnitAVista = result["PrecoListaUnitAVista"]?.ToString();
                            item.PrecoListaUnitAPrazo = result["PrecoListaUnitAPrazo"]?.ToString();
                            item.PrecoNFUnitAVista = result["PrecoNFUnitAVista"]?.ToString();
                            item.PrecoNFUnitAPrazo = result["PrecoNFUnitAPrazo"]?.ToString();
                            item.DescontoAVista = result["DescontoAVista"]?.ToString();
                            item.DescontoAPrazo = result["DescontoAPrazo"]?.ToString();
                            item.DescSuperiorAVista = result["DescSuperiorAVista"]?.ToString();
                            item.DescSuperiorAPrazo = result["DescSuperiorAPrazo"]?.ToString();
                            item.Comissao = result["Comissao"]?.ToString();
                            item.Criacao = result["DataCadastro"]?.ToString();
                            item.Expiracao = result["Validade"].ToString();

                            response.Add(item);
                        }
                    }

                    if (!string.IsNullOrEmpty(obj.OpcoesOrcamento) && obj.OpcoesOrcamento == "Somente aprovadas")
                    {
                        response = response.Where(x => x.OpcaoAprovada == "Sim").ToList();
                    }
                    if (obj.Categorias?.Count() > 0)
                    {
                        response = response.Where(x => obj.Categorias.Contains(x.GrupoSubgrupo)).ToList();
                    }
                }
            }

            return response;
        }

        public List<DadosOrcamentoDto> RelatorioDadosOrcamento(DadosOrcamentosFiltro obj)
        {
            var response = new List<DadosOrcamentoDto>();
            using (var db = contexto.GetContextoLeitura())
            {
                List<object> list = new List<object>();
                using (var command = db.Database.GetDbConnection().CreateCommand())
                {
                    var t = "SELECT\r\n\tt.orcamento,\r\n\tCOUNT(t.opcao) as qtdeOpcao\r\nfrom\r\n\t(\r\n\tselect\r\n\t\ttoc.Id as orcamento,\r\n\t\ttoco.Id as opcao\r\n\tfrom\r\n\t\tt_ORCAMENTO_COTACAO toc\r\n\tinner join t_ORCAMENTO_COTACAO_OPCAO toco on\r\n\t\ttoc.Id = toco.IdOrcamentoCotacao\r\n\tGROUP by\r\n\t\ttoc.Id,\r\n\t\ttoco.Id\r\n) as t\r\nGROUP by\r\n\tt.orcamento";

                    command.CommandText = t;

                    db.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {

                        while (result.Read())
                        {
                            var item = new
                            {
                                orcamento = result[0],
                                qtdeOpcao = result[1]
                            };
                            list.Add(item);
                        }
                    }
                }
                var maiorQtdeOpcao = list.Max(x => x.GetType().GetProperty("qtdeOpcao").GetValue(x, null));

                int qtdeMaxOpcoes = obj.ParamLimiteQtdeMaxOpcao;
                if (obj.ParamLimiteQtdeMaxOpcao < int.Parse(maiorQtdeOpcao.ToString()))
                {
                    qtdeMaxOpcoes = int.Parse(maiorQtdeOpcao.ToString());
                }

                using (var command = db.Database.GetDbConnection().CreateCommand())
                {
                    string select = "SELECT";
                    string loja = " [tOC].[Loja],";
                    string orcamento = " [tOC].[Id] AS [Orcamento],";
                    string status = " (CASE WHEN ([tOC].[Status] = 1) AND ([tOC].[Validade] < Convert(date, getdate())) THEN 'Expirado' ELSE [tCfgSt].[Descricao] END) AS [Status],";
                    string prepedido = " [tOC].[IdOrcamento] AS [PrePedido],";
                    string pedido = " (SELECT TOP 1 pedido FROM [dbo].[t_PEDIDO] [tPedAux] WHERE ([tPedAux].[pedido] = [tPedAux].[pedido_base]) AND ([tPedAux].[IdOrcamentoCotacao] = [tOC].[Id])" +
                        " ORDER BY [tPedAux].[data_hora] DESC) AS [Pedido],";
                    string vendedor = " [tVendedor].[usuario] AS [Vendedor],";
                    string indicador = " [tParceiro].[apelido] AS [Indicador],";
                    string indicadorVendedor = " [tParceiroVendedor].[Nome] AS [IndicadorVendedor],";
                    string idCliente = " (SELECT [tPrePedAux].[id_cliente] FROM [dbo].[t_ORCAMENTO] [tPrePedAux] WHERE [tPrePedAux].[IdOrcamentoCotacao] = [tOC].[Id]) AS [IdCliente],";
                    string usuarioCadastro = " (CASE WHEN [tOC].[IdTipoUsuarioContextoCadastro] = 1 THEN [tUsuarioIntCadastro].[usuario] WHEN " +
                        "[tOC].[IdTipoUsuarioContextoCadastro] = 2 THEN [tParceiroCadastro].[apelido] WHEN " +
                        "[tOC].[IdTipoUsuarioContextoCadastro] = 3 THEN '[VP] ' + [tParceiroVendedorCadastro].[Nome] END) AS [UsuarioCadastro],";
                    string uf = " [tOC].[UF],";
                    string tipoCliente = " [tOC].[TipoCliente],";
                    string contribuinteICMS = " (SELECT [tCDAux].[descricao] FROM [dbo].[t_CODIGO_DESCRICAO] [tCDAux] WHERE ([tCDAux].[grupo] = 'Cliente_Contribuinte_Icms_Status') AND " +
                        "([tCDAux].[codigo] = [tOC].[ContribuinteIcms])) AS [ContribuinteIcms],";
                    string qtdeMsg = " (SELECT Count(*) FROM [dbo].[t_ORCAMENTO_COTACAO_MENSAGEM] [tMsg] WHERE ([tMsg].[IdOrcamentoCotacao] = [tOC].[Id]) AND " +
                        "([tMsg].[IdTipoUsuarioContextoRemetente] = 4) AND ([tMsg].[Id] IN (SELECT [tMsgSt].[IdOrcamentoCotacaoMensagem]" +
                        " FROM [dbo].[t_ORCAMENTO_COTACAO_MENSAGEM_STATUS] [tMsgSt] WHERE ([tMsgSt].[IdOrcamentoCotacaoMensagem] = [tMsg].[Id]) AND " +
                        "([tMsgSt].[PendenciaTratada] = 0)))) AS [QtdeMsgPendente],";
                    string entregaImediata = " (CASE [tOC].[StEtgImediata] WHEN 1 THEN 'Não' WHEN 2 THEN 'Sim' ELSE Convert(varchar(2), [tOC].[StEtgImediata]) + ' - Desconhecido' END) AS [EntregaImediata],";
                    string previsaoEntrega = " (CASE [tOC].[StEtgImediata] WHEN 1 THEN [tOC].[PrevisaoEntregaData] ELSE NULL END) AS [PrevisaoEntrega],";
                    string instaladorInstala = " (CASE [tOC].[InstaladorInstalaStatus] WHEN 1 THEN 'Não' WHEN 2 THEN 'Sim' " +
                        "ELSE Convert(varchar(2), [tOC].[InstaladorInstalaStatus]) + ' - Desconhecido' END) AS [InstaladorInstala],";

                    string sql = select + loja + orcamento + status + prepedido + pedido + vendedor + indicador + indicadorVendedor + idCliente + usuarioCadastro + uf + tipoCliente + contribuinteICMS + qtdeMsg +
                        entregaImediata + previsaoEntrega + instaladorInstala;

                    string opcoes = "";
                    for(int i = 0; i < qtdeMaxOpcoes; i++)
                    {
                        string comissaoOpcao = " (SELECT [tOpcAux].[PercRT] FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO] [tOpcAux] WHERE ([tOpcAux].[IdOrcamentoCotacao] = [tOC].[Id]) AND " +
                        $"([tOpcAux].[Sequencia] = {i + 1})) AS [ComissaoOpcao{i + 1}],";
                        string descontoMedioAvistaOpcao = " (SELECT CASE WHEN (Coalesce(SUM([tUnifAux].[Qtde] * [tAtomAux].[Qtde] * [tCustoFinAux].[PrecoLista]), 0) <> 0)" +
                            " THEN (100 *((SUM([tUnifAux].[Qtde] * [tAtomAux].[Qtde] * [tCustoFinAux].[PrecoLista]) - SUM([tUnifAux].[Qtde] * [tAtomAux].[Qtde] * [tCustoFinAux].[PrecoNF])) /" +
                            " SUM([tUnifAux].[Qtde] * [tAtomAux].[Qtde] * [tCustoFinAux].[PrecoLista]))) ELSE NULL END FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO] [tOpcAux] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO] [tUnifAux] ON [tOpcAux].[Id] = [tUnifAux].[IdOrcamentoCotacaoOpcao] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_PAGTO] [tPagAux] ON [tOpcAux].[Id] = [tPagAux].[IdOrcamentoCotacaoOpcao] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO] [tAtomAux] ON [tUnifAux].[Id] = [tAtomAux].[IdItemUnificado] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN] [tCustoFinAux] ON ([tAtomAux].[Id] = [tCustoFinAux].[IdItemAtomico]) AND" +
                            " ([tPagAux].[Id] = [tCustoFinAux].[IdOpcaoPagto]) WHERE ([tOpcAux].[IdOrcamentoCotacao] = [tOC].[Id]) AND ([tPagAux].[Habilitado] = 1) AND" +
                            $" ([tPagAux].[tipo_parcelamento] = 1) AND ([tOpcAux].[Sequencia] = {i + 1}) AND ([tPagAux].[Id] IN " +
                            "(SELECT [tCustoFinAux2].[IdOpcaoPagto] FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN] [tCustoFinAux2]" +
                            $" WHERE ([tCustoFinAux2].[IdOpcaoPagto] = [tCustoFinAux].[IdOpcaoPagto])))) AS [DescMedioAVistaOpcao{i + 1}],";
                        string descontoMedioAprazoOpcao = " (SELECT CASE WHEN (Coalesce(SUM([tUnifAux].[Qtde] * [tAtomAux].[Qtde] * [tCustoFinAux].[PrecoLista]), 0) <> 0) " +
                            "THEN (100 *((SUM([tUnifAux].[Qtde] * [tAtomAux].[Qtde] * [tCustoFinAux].[PrecoLista]) - SUM([tUnifAux].[Qtde] * [tAtomAux].[Qtde] * [tCustoFinAux].[PrecoNF])) /" +
                            " SUM([tUnifAux].[Qtde] * [tAtomAux].[Qtde] * [tCustoFinAux].[PrecoLista]))) ELSE NULL END FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO] [tOpcAux] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO] [tUnifAux] ON [tOpcAux].[Id] = [tUnifAux].[IdOrcamentoCotacaoOpcao] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_PAGTO] [tPagAux] ON [tOpcAux].[Id] = [tPagAux].[IdOrcamentoCotacaoOpcao] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO] [tAtomAux] ON [tUnifAux].[Id] = [tAtomAux].[IdItemUnificado] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN] [tCustoFinAux] ON ([tAtomAux].[Id] = [tCustoFinAux].[IdItemAtomico]) AND" +
                            " ([tPagAux].[Id] = [tCustoFinAux].[IdOpcaoPagto]) WHERE ([tOpcAux].[IdOrcamentoCotacao] = [tOC].[Id]) AND ([tPagAux].[Habilitado] = 1) AND ([tPagAux].[tipo_parcelamento] <> 1)" +
                            $" AND ([tOpcAux].[Sequencia] = {i + 1}) AND ([tPagAux].[Id] IN (SELECT [tCustoFinAux2].[IdOpcaoPagto] FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN] [tCustoFinAux2]" +
                            $" WHERE ([tCustoFinAux2].[IdOpcaoPagto] = [tCustoFinAux].[IdOpcaoPagto])))) AS [DescMedioAPrazoOpcao{i + 1}], ";
                        string formaPagtoAVistaOpcao = "(SELECT [tMeioPagAux].[Descricao] FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO] [tOpcAux] INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_PAGTO] [tPagAux] " +
                            "ON [tOpcAux].[Id] = [tPagAux].[IdOrcamentoCotacaoOpcao] INNER JOIN [dbo].[t_CFG_PAGTO_MEIO] [tMeioPagAux] ON ([tMeioPagAux].[Id] = [tPagAux].[av_forma_pagto])" +
                            $" WHERE ([tOpcAux].[IdOrcamentoCotacao] = [tOC].[Id]) AND ([tPagAux].[Habilitado] = 1) AND ([tPagAux].[tipo_parcelamento] = 1) AND ([tOpcAux].[Sequencia] = {i + 1})) AS [FormaPagtoAVistaOpcao{i + 1}],";
                        string valorFormaPagtoAVistaOpcao = "(SELECT SUM([tUnifAux].[Qtde] * [tAtomAux].[Qtde] * [tCustoFinAux].[PrecoNF]) FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO] [tOpcAux] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO] [tUnifAux] ON [tOpcAux].[Id] = [tUnifAux].[IdOrcamentoCotacaoOpcao] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_PAGTO] [tPagAux] ON [tOpcAux].[Id] = [tPagAux].[IdOrcamentoCotacaoOpcao] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO] [tAtomAux] ON [tUnifAux].[Id] = [tAtomAux].[IdItemUnificado] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN] [tCustoFinAux] ON ([tAtomAux].[Id] = [tCustoFinAux].[IdItemAtomico]) " +
                            "AND ([tPagAux].[Id] = [tCustoFinAux].[IdOpcaoPagto]) WHERE ([tOpcAux].[IdOrcamentoCotacao] = [tOC].[Id]) AND ([tPagAux].[Habilitado] = 1) AND " +
                            $"([tPagAux].[tipo_parcelamento] = 1) AND ([tOpcAux].[Sequencia] = {i + 1})) AS [ValorFormaPagtoAVistaOpcao{i + 1}], ";
                        string statusDescSuperiorAVistaOpcao = "(SELECT CASE WHEN (SELECT TOP 1 [tPagAux].[Id] FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO] [tOpcAux] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_PAGTO] [tPagAux] ON [tOpcAux].[Id] = [tPagAux].[IdOrcamentoCotacaoOpcao] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN] [tAtomCustoFinAux] ON [tPagAux].[Id] = [tAtomCustoFinAux].[IdOpcaoPagto] " +
                            "WHERE ([tOpcAux].[IdOrcamentoCotacao] = [tOC].[Id]) AND ([tPagAux].[Habilitado] = 1) AND ([tPagAux].[tipo_parcelamento] = 1) " +
                            $"AND ([tOpcAux].[Sequencia] = {i + 1}) AND ([tAtomCustoFinAux].[StatusDescontoSuperior] <> 0)) IS NOT NULL THEN 'Sim' ELSE 'Não' END) AS [StatusDescSuperiorAVistaOpcao{i + 1}], ";
                        string formaPagtoAPrazoOpcao = "(SELECT [tFormaPagAux].[Descricao] FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO] [tOpcAux] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_PAGTO] [tPagAux] ON [tOpcAux].[Id] = [tPagAux].[IdOrcamentoCotacaoOpcao] " +
                            "INNER JOIN [dbo].[t_CFG_PAGTO_FORMA] [tFormaPagAux] ON [tPagAux].[tipo_parcelamento] = [tFormaPagAux].[Id] " +
                            $"WHERE ([tOpcAux].[IdOrcamentoCotacao] = [tOC].[Id]) AND ([tPagAux].[Habilitado] = 1) AND ([tPagAux].[tipo_parcelamento] <> 1) AND ([tOpcAux].[Sequencia] = {i + 1})) " +
                            $"AS [FormaPagtoAPrazoOpcao{i + 1}],";
                        string valorFormaPagtoAPrazoOpcao = "(SELECT SUM([tUnifAux].[Qtde] * [tAtomAux].[Qtde] * [tCustoFinAux].[PrecoNF]) FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO] [tOpcAux] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO] [tUnifAux] ON [tOpcAux].[Id] = [tUnifAux].[IdOrcamentoCotacaoOpcao] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_PAGTO] [tPagAux] ON [tOpcAux].[Id] = [tPagAux].[IdOrcamentoCotacaoOpcao] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO] [tAtomAux] ON [tUnifAux].[Id] = [tAtomAux].[IdItemUnificado] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN] [tCustoFinAux] ON ([tAtomAux].[Id] = [tCustoFinAux].[IdItemAtomico]) " +
                            "AND ([tPagAux].[Id] = [tCustoFinAux].[IdOpcaoPagto]) WHERE ([tOpcAux].[IdOrcamentoCotacao] = [tOC].[Id]) AND ([tPagAux].[Habilitado] = 1) " +
                            $"AND ([tPagAux].[tipo_parcelamento] <> 1) AND ([tOpcAux].[Sequencia] = {i + 1})) AS [ValorFormaPagtoAPrazoOpcao{i + 1}],";
                        string qtdeParcelasFormaPagtoAPrazoOpcao = "(SELECT CASE [tPagAux].[tipo_parcelamento] WHEN 5 THEN 1 WHEN 2 THEN [tPagAux].[pc_qtde_parcelas] WHEN 6 THEN [tPagAux].[pc_maquineta_qtde_parcelas] " +
                            "WHEN 3 THEN (1 + [tPagAux].[pce_prestacao_qtde]) WHEN 4 THEN (1 + [tPagAux].[pse_demais_prest_qtde]) END FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO] [tOpcAux] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_PAGTO] [tPagAux] ON [tOpcAux].[Id] = [tPagAux].[IdOrcamentoCotacaoOpcao] " +
                            "WHERE ([tOpcAux].[IdOrcamentoCotacao] = [tOC].[Id]) AND ([tPagAux].[Habilitado] = 1) " +
                            $"AND ([tPagAux].[tipo_parcelamento] <> 1) AND ([tOpcAux].[Sequencia] = {i + 1})) AS [QtdeParcelasFormaPagtoAPrazoOpcao{i + 1}],";
                        string statusDescSuperiorAPrazoOpcao = "(SELECT CASE WHEN (SELECT TOP 1 [tPagAux].[Id] FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO] [tOpcAux] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_PAGTO] [tPagAux] ON [tOpcAux].[Id] = [tPagAux].[IdOrcamentoCotacaoOpcao] " +
                            "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN] [tAtomCustoFinAux] ON [tPagAux].[Id] = [tAtomCustoFinAux].[IdOpcaoPagto] " +
                            $"WHERE ([tOpcAux].[IdOrcamentoCotacao] = [tOC].[Id]) AND ([tPagAux].[Habilitado] = 1) AND ([tPagAux].[tipo_parcelamento] <> 1) AND ([tOpcAux].[Sequencia] = {i + 1}) " +
                            $"AND ([tAtomCustoFinAux].[StatusDescontoSuperior] <> 0)) IS NOT NULL THEN 'Sim' ELSE 'Não' END) AS [StatusDescSuperiorAPrazoOpcao{i + 1}],";

                        opcoes += comissaoOpcao + descontoMedioAvistaOpcao + descontoMedioAprazoOpcao + formaPagtoAVistaOpcao
                            + valorFormaPagtoAVistaOpcao + statusDescSuperiorAVistaOpcao + formaPagtoAPrazoOpcao + valorFormaPagtoAPrazoOpcao
                            + qtdeParcelasFormaPagtoAPrazoOpcao + statusDescSuperiorAPrazoOpcao;
                    }

                    string opcaoAprovada = "(SELECT [Sequencia] FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO] [tOpcAux] INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_PAGTO] [tPagAux] " +
                        "ON [tOpcAux].[Id] = [tPagAux].[IdOrcamentoCotacaoOpcao] WHERE ([tOpcAux].[IdOrcamentoCotacao] = [tOC].[Id]) AND ([tPagAux].[Aprovado] = 1)) AS [OpcaoAprovada], ";
                    string comissaoOpcaoAprovada = "(SELECT [tOpcAux].[PercRT] FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO] [tOpcAux] INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_PAGTO] [tPagAux] " +
                        "ON [tOpcAux].[Id] = [tPagAux].[IdOrcamentoCotacaoOpcao] WHERE ([tOpcAux].[IdOrcamentoCotacao] = [tOC].[Id]) AND ([tPagAux].[Aprovado] = 1)) AS [ComissaoOpcaoAprovada], ";
                    string descMedioOpcaoAprovada = "(SELECT CASE WHEN (Coalesce(SUM([tUnifAux].[Qtde] * [tAtomAux].[Qtde] * [tCustoFinAux].[PrecoLista]), 0) <> 0) " +
                        "THEN (100 *((SUM([tUnifAux].[Qtde] * [tAtomAux].[Qtde] * [tCustoFinAux].[PrecoLista]) - SUM([tUnifAux].[Qtde] * [tAtomAux].[Qtde] * [tCustoFinAux].[PrecoNF])) / " +
                        "SUM([tUnifAux].[Qtde] * [tAtomAux].[Qtde] * [tCustoFinAux].[PrecoLista]))) ELSE NULL END FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO] [tOpcAux] " +
                        "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO] [tUnifAux] ON [tOpcAux].[Id] = [tUnifAux].[IdOrcamentoCotacaoOpcao] " +
                        "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_PAGTO] [tPagAux] ON [tOpcAux].[Id] = [tPagAux].[IdOrcamentoCotacaoOpcao] " +
                        "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO] [tAtomAux] ON [tUnifAux].[Id] = [tAtomAux].[IdItemUnificado] " +
                        "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN] [tCustoFinAux] ON ([tAtomAux].[Id] = [tCustoFinAux].[IdItemAtomico]) " +
                        "AND ([tPagAux].[Id] = [tCustoFinAux].[IdOpcaoPagto]) WHERE ([tOpcAux].[IdOrcamentoCotacao] = [tOC].[Id]) AND ([tPagAux].[Aprovado] = 1) AND ([tPagAux].[Id] IN " +
                        "(SELECT [tCustoFinAux2].[IdOpcaoPagto] FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN] [tCustoFinAux2] " +
                        "WHERE ([tCustoFinAux2].[IdOpcaoPagto] = [tCustoFinAux].[IdOpcaoPagto])))) AS [DescMedioOpcaoAprovada], ";
                    string formaPagtoOpcaoAprovada = "(SELECT [tFormaPagAux].[Descricao] FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO] [tOpcAux] " +
                        "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_PAGTO] [tPagAux] ON [tOpcAux].[Id] = [tPagAux].[IdOrcamentoCotacaoOpcao] " +
                        "INNER JOIN [dbo].[t_CFG_PAGTO_FORMA] [tFormaPagAux] ON ([tPagAux].[tipo_parcelamento] = [tFormaPagAux].[Id]) " +
                        "WHERE ([tOpcAux].[IdOrcamentoCotacao] = [tOC].[Id]) AND ([tPagAux].[Aprovado] = 1)) AS [FormaPagtoOpcaoAprovada], ";
                    string valorFormaPagtoOpcaoAprovada = "(SELECT SUM([tUnifAux].[Qtde] * [tAtomAux].[Qtde] * [tCustoFinAux].[PrecoNF]) FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO] [tOpcAux] " +
                        "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO] [tUnifAux] ON [tOpcAux].[Id] = [tUnifAux].[IdOrcamentoCotacaoOpcao] " +
                        "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_PAGTO] [tPagAux] ON [tOpcAux].[Id] = [tPagAux].[IdOrcamentoCotacaoOpcao] " +
                        "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO] [tAtomAux] ON [tUnifAux].[Id] = [tAtomAux].[IdItemUnificado] " +
                        "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN] [tCustoFinAux] ON ([tAtomAux].[Id] = [tCustoFinAux].[IdItemAtomico]) " +
                        "AND ([tPagAux].[Id] = [tCustoFinAux].[IdOpcaoPagto]) WHERE ([tOpcAux].[IdOrcamentoCotacao] = [tOC].[Id]) AND ([tPagAux].[Aprovado] = 1)) AS [ValorFormaPagtoOpcaoAprovada], ";
                    string qtdeParcelasFormaOpcaoAprovada = "(SELECT CASE [tPagAux].[tipo_parcelamento] WHEN 5 THEN 1 WHEN 2 THEN [tPagAux].[pc_qtde_parcelas] WHEN 6 THEN [tPagAux].[pc_maquineta_qtde_parcelas] " +
                        "WHEN 3 THEN (1 + [tPagAux].[pce_prestacao_qtde]) WHEN 4 THEN (1 + [tPagAux].[pse_demais_prest_qtde]) END FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO] [tOpcAux] " +
                        "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_PAGTO] [tPagAux] ON [tOpcAux].[Id] = [tPagAux].[IdOrcamentoCotacaoOpcao] WHERE ([tOpcAux].[IdOrcamentoCotacao] = [tOC].[Id]) " +
                        "AND ([tPagAux].[Aprovado] = 1)) AS [QtdeParcelasFormaOpcaoAprovada], ";
                    string statusDescSuperiorOpcaoAprovada = "(SELECT CASE WHEN (SELECT TOP 1 [tPagAux].[Id] FROM [dbo].[t_ORCAMENTO_COTACAO_OPCAO] [tOpcAux] " +
                        "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_PAGTO] [tPagAux] ON [tOpcAux].[Id] = [tPagAux].[IdOrcamentoCotacaoOpcao] " +
                        "INNER JOIN [dbo].[t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN] [tAtomCustoFinAux] ON [tPagAux].[Id] = [tAtomCustoFinAux].[IdOpcaoPagto] " +
                        "WHERE ([tOpcAux].[IdOrcamentoCotacao] = [tOC].[Id]) AND ([tPagAux].[Aprovado] = 1) AND ([tAtomCustoFinAux].[StatusDescontoSuperior] <> 0)) IS NOT NULL THEN 'Sim' " +
                        "ELSE 'Não' END) AS [StatusDescSuperiorOpcaoAprovada], ";
                    string dataCadastro = "Convert(date, [tOC].[DataCadastro]) AS [DataCadastro], ";
                    string validade = "Convert(date, [tOC].[Validade]) AS [Validade] ";
                    string joins = " FROM [dbo].[t_ORCAMENTO_COTACAO] [tOC] " +
                        "LEFT JOIN [dbo].[t_CFG_ORCAMENTO_COTACAO_STATUS] [tCfgSt] ON [tOC].[Status] = [tCfgSt].[Id] " +
                        "LEFT JOIN [dbo].[t_USUARIO] [tVendedor] ON [tOC].[IdVendedor] = [tVendedor].[Id] " +
                        "LEFT JOIN [dbo].[t_ORCAMENTISTA_E_INDICADOR] [tParceiro] ON [tOC].[IdIndicador] = [tParceiro].[Id] " +
                        "LEFT JOIN [dbo].[t_ORCAMENTISTA_E_INDICADOR_VENDEDOR] [tParceiroVendedor] ON [tOC].[IdIndicadorVendedor] = [tParceiroVendedor].[Id] " +
                        "LEFT JOIN [dbo].[t_USUARIO] [tUsuarioIntCadastro] ON ([tOC].[IdTipoUsuarioContextoCadastro] = 1) AND ([tOC].[IdUsuarioCadastro] = [tUsuarioIntCadastro].[Id]) " +
                        "LEFT JOIN [dbo].[t_ORCAMENTISTA_E_INDICADOR] [tParceiroCadastro] ON ([tOC].[IdTipoUsuarioContextoCadastro] = 2) AND ([tOC].[IdUsuarioCadastro] = [tParceiroCadastro].[Id]) " +
                        "LEFT JOIN [dbo].[t_ORCAMENTISTA_E_INDICADOR_VENDEDOR] [tParceiroVendedorCadastro] ON ([tOC].[IdTipoUsuarioContextoCadastro] = 3) " +
                        "AND ([tOC].[IdUsuarioCadastro] = [tParceiroVendedorCadastro].[Id])";

                    sql = sql + opcoes + opcaoAprovada + comissaoOpcaoAprovada + descMedioOpcaoAprovada + formaPagtoOpcaoAprovada +
                        valorFormaPagtoOpcaoAprovada + qtdeParcelasFormaOpcaoAprovada + statusDescSuperiorOpcaoAprovada +
                        dataCadastro + validade + joins;

                    sql = sql + "WHERE ([tOC].[Status] <> 5)";

                    if (obj.DtInicio.HasValue)
                    {
                        sql = sql + $" AND (tOc.DataCadastro >= '{obj.DtInicio.Value.ToString("yyyy-MM-dd")}')";
                    }
                    if (obj.DtFim.HasValue)
                    {
                        sql = sql + $" AND (tOc.DataCadastro <= '{obj.DtFim.Value.ToString("yyyy-MM-dd")}')";
                    }
                    if (obj.DtInicioExpiracao.HasValue)
                    {
                        sql = sql + $" AND (tOc.Validade >= '{obj.DtInicioExpiracao.Value.ToString("yyyy-MM-dd")}')";
                    }
                    if (obj.DtFimExpiracao.HasValue)
                    {
                        sql = sql + $" AND (tOc.Validade <= '{obj.DtFimExpiracao.Value.ToString("yyyy-MM-dd")}')";
                    }
                    if (obj.Lojas?.Count() > 0)
                    {
                        sql = sql + $" AND (tOc.Loja IN ({String.Join(", ", obj.Lojas)}))";
                    }
                    if (obj.Status?.Count() > 0)
                    {
                        sql = sql + $" AND (tOc.Status IN ({String.Join(",", obj.Status)}))";
                    }
                    if (obj.Vendedores?.Count() > 0)
                    {
                        sql = sql + $" AND (tOc.IdVendedor IN ({String.Join(",", obj.Vendedores)}))";
                    }
                    if (obj.ComIndicador != null && obj.ComIndicador == true)
                    {
                        sql = sql + $" AND (tOc.IdIndicador IS NOT NULL)";
                    }
                    if (obj.ComIndicador != null && obj.ComIndicador == false)
                    {
                        sql = sql + $" AND (tOc.IdIndicador IS NULL)";
                    }
                    if (obj.Parceiros?.Count() > 0)
                    {
                        sql = sql + $" AND (tOc.IdIndicador IN ({String.Join(", ", obj.Parceiros)}))";
                    }

                    sql = sql + " ORDER BY\r\n\t[tOC].[Id]";

                    command.CommandText = sql;

                    db.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            var item = new DadosOrcamentoDto();
                            item.Loja = result["Loja"]?.ToString();
                            item.Orcamento = int.Parse(result["Orcamento"]?.ToString());
                            item.Status = result["Status"]?.ToString();
                            item.Prepedido = result["Prepedido"]?.ToString();
                            item.Pedido = result["Pedido"]?.ToString();
                            item.Vendedor = result["Vendedor"]?.ToString();
                            item.Parceiro = result["Indicador"]?.ToString();
                            item.VendedorParceiro = result["IndicadorVendedor"]?.ToString();
                            item.IdCliente = result["IdCliente"]?.ToString();
                            item.UsuarioCadastro = result["UsuarioCadastro"]?.ToString();
                            item.UF = result["UF"]?.ToString();
                            item.TipoCliente = result["TipoCliente"]?.ToString();
                            item.ContribuinteIcms = result["ContribuinteIcms"]?.ToString();
                            item.QtdeMsgPendente = !string.IsNullOrEmpty(result["QtdeMsgPendente"].ToString()) ? int.Parse(result["QtdeMsgPendente"].ToString()) : int.Parse("0");
                            item.EntregaImediata = result["EntregaImediata"]?.ToString();
                            item.PrevisaoEntrega = result["PrevisaoEntrega"]?.ToString();
                            item.InstaladorInstala = result["InstaladorInstala"]?.ToString();
                            item.ListaOpcoes = new List<OpcaoDadosOrcamento>();
                            for(int i = 0; i < qtdeMaxOpcoes; i++){
                                OpcaoDadosOrcamento opcao = new OpcaoDadosOrcamento();
                                opcao.ComissaoOpcao = !string.IsNullOrEmpty(result[$"ComissaoOpcao{i + 1}"]?.ToString()) ? float.Parse(result[$"ComissaoOpcao{i + 1}"]?.ToString()) : null;
                                opcao.DescMedioAVistaOpcao = result[$"DescMedioAVistaOpcao{i + 1}"].ToString();
                                opcao.DescMedioAPrazoOpcao = result[$"DescMedioAPrazoOpcao{i + 1}"].ToString();
                                opcao.FormaPagtoAVistaOpcao = result[$"FormaPagtoAVistaOpcao{i + 1}"]?.ToString();
                                opcao.ValorFormaPagtoAVistaOpcao = result[$"ValorFormaPagtoAVistaOpcao{i + 1}"]?.ToString();
                                opcao.StatusDescSuperiorAVistaOpcao = result[$"StatusDescSuperiorAVistaOpcao{i + 1}"]?.ToString();
                                opcao.FormaPagtoAPrazoOpcao = result[$"FormaPagtoAPrazoOpcao{i + 1}"]?.ToString();
                                opcao.ValorFormaPagtoAPrazoOpcao = result[$"ValorFormaPagtoAPrazoOpcao{i + 1}"]?.ToString();
                                opcao.QtdeParcelasFormaPagtoAPrazoOpcao = !string.IsNullOrEmpty(result[$"QtdeParcelasFormaPagtoAPrazoOpcao{i + 1}"].ToString()) ? int.Parse(result[$"QtdeParcelasFormaPagtoAPrazoOpcao{i + 1}"].ToString()) : null;
                                opcao.StatusDescSuperiorAPrazoOpcao = result[$"StatusDescSuperiorAPrazoOpcao{i + 1}"]?.ToString();
                                item.ListaOpcoes.Add(opcao);
                            }
                            //item.ComissaoOpcao1 = !string.IsNullOrEmpty(result["ComissaoOpcao1"]?.ToString()) ? float.Parse(result["ComissaoOpcao1"]?.ToString()) : null;
                            //item.DescMedioAVistaOpcao1 = result["DescMedioAVistaOpcao1"].ToString();
                            //item.DescMedioAPrazoOpcao1 = result["DescMedioAPrazoOpcao1"].ToString();
                            //item.FormaPagtoAVistaOpcao1 = result["FormaPagtoAVistaOpcao1"]?.ToString();
                            //item.ValorFormaPagtoAVistaOpcao1 = result["ValorFormaPagtoAVistaOpcao1"]?.ToString();
                            //item.StatusDescSuperiorAVistaOpcao1 = result["StatusDescSuperiorAVistaOpcao1"]?.ToString();
                            //item.FormaPagtoAPrazoOpcao1 = result["FormaPagtoAPrazoOpcao1"]?.ToString();
                            //item.ValorFormaPagtoAPrazoOpcao1 = result["ValorFormaPagtoAPrazoOpcao1"]?.ToString();
                            //item.QtdeParcelasFormaPagtoAPrazoOpcao1 = !string.IsNullOrEmpty(result["QtdeParcelasFormaPagtoAPrazoOpcao1"].ToString()) ? int.Parse(result["QtdeParcelasFormaPagtoAPrazoOpcao1"].ToString()) : null;
                            //item.StatusDescSuperiorAPrazoOpcao1 = result["StatusDescSuperiorAPrazoOpcao1"]?.ToString();
                            //item.ComissaoOpcao2 = !string.IsNullOrEmpty(result["ComissaoOpcao2"]?.ToString()) ? float.Parse(result["ComissaoOpcao2"]?.ToString()) : null;
                            //item.DescMedioAVistaOpcao2 = result["DescMedioAVistaOpcao2"].ToString();
                            //item.DescMedioAPrazoOpcao2 = result["DescMedioAPrazoOpcao2"].ToString();
                            //item.FormaPagtoAVistaOpcao2 = result["FormaPagtoAVistaOpcao2"]?.ToString();
                            //item.ValorFormaPagtoAVistaOpcao2 = result["ValorFormaPagtoAVistaOpcao2"].ToString();
                            //item.StatusDescSuperiorAVistaOpcao2 = result["StatusDescSuperiorAVistaOpcao2"]?.ToString();
                            //item.FormaPagtoAPrazoOpcao2 = result["FormaPagtoAPrazoOpcao2"]?.ToString();
                            //item.ValorFormaPagtoAPrazoOpcao2 = result["ValorFormaPagtoAPrazoOpcao2"].ToString();
                            //item.QtdeParcelasFormaPagtoAPrazoOpcao2 = !string.IsNullOrEmpty(result["QtdeParcelasFormaPagtoAPrazoOpcao2"].ToString()) ? int.Parse(result["QtdeParcelasFormaPagtoAPrazoOpcao2"].ToString()) : null;
                            //item.StatusDescSuperiorAPrazoOpcao2 = result["StatusDescSuperiorAPrazoOpcao2"]?.ToString();
                            //item.ComissaoOpcao3 = !string.IsNullOrEmpty(result["ComissaoOpcao3"].ToString()) ? float.Parse(result["ComissaoOpcao3"].ToString()) : null;
                            //item.DescMedioAVistaOpcao3 = result["DescMedioAVistaOpcao3"].ToString();
                            //item.DescMedioAPrazoOpcao3 = result["DescMedioAPrazoOpcao3"].ToString();
                            //item.FormaPagtoAVistaOpcao3 = result["FormaPagtoAVistaOpcao3"]?.ToString();
                            //item.ValorFormaPagtoAVistaOpcao3 = result["ValorFormaPagtoAVistaOpcao3"].ToString();
                            //item.StatusDescSuperiorAVistaOpcao3 = result["StatusDescSuperiorAVistaOpcao3"]?.ToString();
                            //item.FormaPagtoAPrazoOpcao3 = result["FormaPagtoAPrazoOpcao3"]?.ToString();
                            //item.ValorFormaPagtoAPrazoOpcao3 = result["ValorFormaPagtoAPrazoOpcao3"].ToString();
                            //item.QtdeParcelasFormaPagtoAPrazoOpcao3 = !string.IsNullOrEmpty(result["QtdeParcelasFormaPagtoAPrazoOpcao3"].ToString()) ? int.Parse(result["QtdeParcelasFormaPagtoAPrazoOpcao3"].ToString()) : null;
                            //item.StatusDescSuperiorAPrazoOpcao3 = result["StatusDescSuperiorAPrazoOpcao3"]?.ToString();
                            item.OpcaoAprovada = !string.IsNullOrEmpty(result["OpcaoAprovada"].ToString()) ? short.Parse(result["OpcaoAprovada"].ToString()) : null;
                            item.ComissaoOpcaoAprovada = !string.IsNullOrEmpty(result["ComissaoOpcaoAprovada"]?.ToString()) ? float.Parse(result["ComissaoOpcaoAprovada"]?.ToString()) : null;
                            item.DescMedioOpcaoAprovada = result["DescMedioOpcaoAprovada"].ToString();
                            item.FormaPagtoOpcaoAprovada = result["FormaPagtoOpcaoAprovada"]?.ToString();
                            item.ValorFormaPagtoOpcaoAprovada = result["ValorFormaPagtoOpcaoAprovada"].ToString();
                            item.QtdeParcelasFormaOpcaoAprovada = !string.IsNullOrEmpty(result["QtdeParcelasFormaOpcaoAprovada"].ToString()) ? int.Parse(result["QtdeParcelasFormaOpcaoAprovada"].ToString()) : null;
                            item.StatusDescSuperiorOpcaoAprovada = result["StatusDescSuperiorOpcaoAprovada"]?.ToString();
                            item.Criacao = result["DataCadastro"]?.ToString();
                            item.Expiracao = result["Validade"].ToString();

                            response.Add(item);
                        }
                    }
                }
            }

            return response;
        }
    }
}
