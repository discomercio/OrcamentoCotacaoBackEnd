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
                    if (obj.ComParceiro != null && obj.ComParceiro == true)
                    {
                        sql = sql + $" AND (tOc.IdIndicador IS NOT NULL)";
                    }
                    if (obj.ComParceiro != null && obj.ComParceiro == false)
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
    }
}
