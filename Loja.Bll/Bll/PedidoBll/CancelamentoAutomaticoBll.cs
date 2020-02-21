using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Loja.Bll.Bll.AcessoBll;
using Loja.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Loja.Bll.PedidoBll;

namespace Loja.Bll.Bll.PedidoBll
{
    public class CancelamentoAutomaticoBll
    {
        private readonly ContextoBdProvider contextoProvider;
        private readonly PedidoLogBll pedidoLogBll;
        private readonly Loja.Bll.PedidoBll.PedidoBll pedidoBll;

        public CancelamentoAutomaticoBll(ContextoBdProvider contextoProvider, Loja.Bll.PedidoBll.PedidoLogBll pedidoLogBll,
            Loja.Bll.PedidoBll.PedidoBll pedidoBll)
        {
            this.contextoProvider = contextoProvider;
            this.pedidoLogBll = pedidoLogBll;
            this.pedidoBll = pedidoBll;
        }
        public class CancelamentoAutomaticoItem
        {
            public int NumeroLinha { get; set; }
            public DateTime? DataFinal { get; set; }
            public string Pedido { get; set; }
            public string Vendedor { get; set; }
            public string NomeDoCliente { get; set; }
            public string Analise_credito_descricao { get; set; }
            public string Loja { get; set; }
        }

        public async Task<List<CancelamentoAutomaticoItem>> DadosTela(bool ConsultaUniversalPedidoOrcamento, UsuarioLogado usuarioLogado,
            List<UsuarioAcessoBll.LojaPermtidaUsuario> listaLojas)
        {
            var strWhereBase = " (t1.st_entrega <> '" + Constantes.Constantes.ST_ENTREGA_ENTREGUE + "')" +
                                        " AND (t1.st_entrega <> '" + Constantes.Constantes.ST_ENTREGA_CANCELADO + "')" +
                                        " AND (t1.st_entrega <> '" + Constantes.Constantes.ST_ENTREGA_A_ENTREGAR + "')" +
                                        " AND (Coalesce(tPedBase.st_pagto, '') <> '" + Constantes.Constantes.ST_PAGTO_PAGO + "')" +
                                        " AND (Coalesce(tPedBase.st_pagto, '') <> '" + Constantes.Constantes.ST_PAGTO_PARCIAL + "')" +
                                        " AND (";
            strWhereBase += "(1=1)";
            foreach (var loja in listaLojas)
                strWhereBase += "OR (tPedBase.loja = '" + loja.Id + "')";
            strWhereBase += ")";

            //' A LISTA DE LOJAS IMUNES FOI DESATIVADA EM 24/10/2019 A PEDIDO DA LILIAN/CARLOS
            //'								" AND (tPedBase.loja NOT IN ('" & NUMERO_LOJA_VRF2 & "','" & NUMERO_LOJA_VRF3 & "','" & NUMERO_LOJA_VRF4 & "','" & NUMERO_LOJA_VRF5 & "','" & NUMERO_LOJA_VRF6 & "','" & NUMERO_LOJA_VRF7 & "','" & NUMERO_LOJA_VRF8 & "'))"


            if (!usuarioLogado.Operacao_permitida(Constantes.Constantes.OP_LJA_CONSULTA_UNIVERSAL_PEDIDO_ORCAMENTO))
            {
                strWhereBase = strWhereBase + " AND (tPedBase.vendedor = '" + usuarioLogado.Usuario + "')";
            }


            var PRAZO_EXIBICAO_CANCEL_AUTO_PEDIDO = 4;

            var strSqlVlPagoCartao = " Coalesce(" +
                            "(" +
                            "SELECT" +
                                " SUM(payment.valor_transacao)" +
                            " FROM t_PAGTO_GW_PAG pag INNER JOIN t_PAGTO_GW_PAG_PAYMENT payment ON (pag.id = payment.id_pagto_gw_pag)" +
                            " WHERE" +
                                " (pag.pedido = t1.pedido_base)" +
                                " AND" +
                                "(" +
                                    " (ult_GlobalStatus = '" + Constantes.Constantes.BRASPAG_PAGADOR_CARTAO_GLOBAL_STATUS__CAPTURADA + "')" +
                                    " OR" +
                                    " (ult_GlobalStatus = '" + Constantes.Constantes.BRASPAG_PAGADOR_CARTAO_GLOBAL_STATUS__AUTORIZADA + "')" +
                                ")" +
                            "), 0) AS vl_pago_cartao";



            var strSql = "SELECT " +
                                    "*" +
                                " FROM (" +
                                    "SELECT" +
                                        " t1.pedido," +
                                        " t1.pedido_base," +
                                        " tPedBase.loja," +
                                        " Coalesce(t1.obs_2, '') AS obs_2," +
                                        " t1.transportadora_selecao_auto_status," +
                                        " Coalesce(t1.transportadora_id, '') AS transportadora_id," +
                                        " t1.st_entrega," +
                                        " 'Pendente Cartão de Crédito' AS analise_credito_descricao," +
                                        " " + Constantes.Constantes.PRAZO_CANCEL_AUTO_PEDIDO_PENDENTE_CARTAO_CREDITO + " AS prazo_cancelamento," +
                                        " tPedBase.analise_credito," +
                                        " tPedBase.data_hora AS analise_credito_data," +
                                        " tPedBase.data AS analise_credito_data_sem_hora," +
                                        " tPedBase.vendedor," +
                                        " nome," +
                                        " Coalesce(Datediff(day, tPedBase.data, Convert(datetime, Convert(varchar(10), getdate(), 121), 121)), 0) AS dias_decorridos," +
                                        " (" +
                                            "SELECT Count(*) FROM t_PEDIDO t2 WHERE (t2.pedido_base = t1.pedido_base) AND (t2.st_auto_split = 0) AND (t2.tamanho_num_pedido > " + Constantes.Constantes.TAM_MIN_ID_PEDIDO + ")" +
                                        ") AS qtde_pedido_filhote," +
                                        strSqlVlPagoCartao +
                                    " FROM t_PEDIDO t1" +
                                    " INNER JOIN t_PEDIDO AS tPedBase ON (t1.pedido_base=tPedBase.pedido)" +
                                    " INNER JOIN t_CLIENTE on (t1.id_cliente = t_CLIENTE.id)" +
                                    " WHERE" +
                                        strWhereBase + " AND (" +
                                            "(tPedBase.analise_credito = " + Constantes.Constantes.COD_AN_CREDITO_ST_INICIAL + ") AND (tPedBase.st_forma_pagto_somente_cartao = 1)" +
                                            " AND (Coalesce(Datediff(day, tPedBase.data, getdate()), 0) > (" + Constantes.Constantes.PRAZO_CANCEL_AUTO_PEDIDO_PENDENTE_CARTAO_CREDITO + " - " + PRAZO_EXIBICAO_CANCEL_AUTO_PEDIDO + "))" +
                                        ")" +
                                    " UNION " +
                                    "SELECT" +
                                        " t1.pedido," +
                                        " t1.pedido_base," +
                                        " tPedBase.loja," +
                                        " Coalesce(t1.obs_2, '') AS obs_2," +
                                        " t1.transportadora_selecao_auto_status," +
                                        " Coalesce(t1.transportadora_id, '') AS transportadora_id," +
                                        " t1.st_entrega," +
                                        " 'Crédito OK (aguardando depósito)' AS analise_credito_descricao," +
                                        " " + Constantes.Constantes.PRAZO_CANCEL_AUTO_PEDIDO_CREDITO_OK_AGUARDANDO_DEPOSITO + " AS prazo_cancelamento," +
                                        " tPedBase.analise_credito," +
                                        " tPedBase.analise_credito_data," +
                                        " tPedBase.analise_credito_data_sem_hora," +
                                        " tPedBase.vendedor," +
                                        " nome," +
                                        " Coalesce(Datediff(day, tPedBase.analise_credito_data_sem_hora, Convert(datetime, Convert(varchar(10), getdate(), 121), 121)), 0) AS dias_decorridos," +
                                        " (" +
                                            "SELECT Count(*) FROM t_PEDIDO t2 WHERE (t2.pedido_base = t1.pedido_base) AND (t2.st_auto_split = 0) AND (t2.tamanho_num_pedido > " + Constantes.Constantes.TAM_MIN_ID_PEDIDO + ")" +
                                        ") AS qtde_pedido_filhote," +
                                        strSqlVlPagoCartao +
                                    " FROM t_PEDIDO t1" +
                                    " INNER JOIN t_PEDIDO AS tPedBase ON (t1.pedido_base=tPedBase.pedido)" +
                                    " INNER JOIN t_CLIENTE on (t1.id_cliente = t_CLIENTE.id)" +
                                    " WHERE" +
                                        strWhereBase +
                                        " AND (" +
                                            "(tPedBase.analise_credito = " + Constantes.Constantes.COD_AN_CREDITO_OK_AGUARDANDO_DEPOSITO + ")" +
                                            " AND (tPedBase.analise_credito_data_sem_hora IS NOT NULL)" +
                                            " AND (Coalesce(Datediff(day, tPedBase.analise_credito_data_sem_hora, getdate()), 0) > (" + Constantes.Constantes.PRAZO_CANCEL_AUTO_PEDIDO_CREDITO_OK_AGUARDANDO_DEPOSITO + " -  " + PRAZO_EXIBICAO_CANCEL_AUTO_PEDIDO + "))" +
                                        ")" +
                                    " UNION " +
                                    "SELECT" +
                                        " t1.pedido," +
                                        " t1.pedido_base," +
                                        " tPedBase.loja," +
                                        " Coalesce(t1.obs_2, '') AS obs_2," +
                                        " t1.transportadora_selecao_auto_status," +
                                        " Coalesce(t1.transportadora_id, '') AS transportadora_id," +
                                        " t1.st_entrega," +
                                        " 'Pendente Vendas' AS analise_credito_descricao," +
                                        " " + Constantes.Constantes.PRAZO_CANCEL_AUTO_PEDIDO_PENDENTE_VENDAS + " AS prazo_cancelamento," +
                                        " tPedBase.analise_credito," +
                                        " tPedBase.analise_credito_data," +
                                        " tPedBase.analise_credito_data_sem_hora," +
                                        " tPedBase.vendedor," +
                                        " nome," +
                                        " Coalesce(Datediff(day, tPedBase.analise_credito_data_sem_hora, Convert(datetime, Convert(varchar(10), getdate(), 121), 121)), 0) AS dias_decorridos," +
                                        " (" +
                                            "SELECT Count(*) FROM t_PEDIDO t2 WHERE (t2.pedido_base = t1.pedido_base) AND (t2.st_auto_split = 0) AND (t2.tamanho_num_pedido > " + Constantes.Constantes.TAM_MIN_ID_PEDIDO + ")" +
                                        ") AS qtde_pedido_filhote," +
                                        strSqlVlPagoCartao +
                                    " FROM t_PEDIDO t1" +
                                    " INNER JOIN t_PEDIDO AS tPedBase ON (t1.pedido_base=tPedBase.pedido)" +
                                    " INNER JOIN t_CLIENTE on (t1.id_cliente = t_CLIENTE.id)" +
                                    " WHERE" +
                                        strWhereBase +
                                        " AND (" +
                                            "(tPedBase.analise_credito = " + Constantes.Constantes.COD_AN_CREDITO_PENDENTE_VENDAS + ")" +
                                            " AND (tPedBase.analise_credito_data_sem_hora IS NOT NULL)" +
                                            " AND (Coalesce(Datediff(day, tPedBase.analise_credito_data_sem_hora, getdate()), 0) >  (" + Constantes.Constantes.PRAZO_CANCEL_AUTO_PEDIDO_PENDENTE_VENDAS + " -  " + PRAZO_EXIBICAO_CANCEL_AUTO_PEDIDO + "))" +
                                        ")" +
                                    ") t" +
                                " WHERE" +
                                    " (qtde_pedido_filhote = 0)" +
                                    " AND (LEN(obs_2) = 0)" +
                                    " AND (vl_pago_cartao = 0)" +
                                    " AND ((transportadora_selecao_auto_status = 1) OR (LEN(Coalesce(transportadora_id,'')) = 0))" +
                                " ORDER BY" +
                                    " analise_credito_data_sem_hora," +
                                    " analise_credito," +
                                    " pedido";


            List<CancelamentoAutomaticoItem> ret = new List<CancelamentoAutomaticoItem>();

            int numeroLinha = 0;
            //using (var db = contextoProvider.GetContextoLeitura().GetContextoBdBasicoParaSql())
            //{
            //    using (var command = db.Database.GetDbConnection().CreateCommand())
            //    {
            //        command.CommandText = strSql;

            //        db.Database.OpenConnection();
            //        using (var result = await command.ExecuteReaderAsync())
            //        {
            //            while (result.Read())
            //            {
            //                numeroLinha++;

            //                DateTime? dataFinal = null;
            //                if (result["analise_credito_data_sem_hora"].GetType() == typeof(DateTime))
            //                {
            //                    dataFinal = ((DateTime)result["analise_credito_data_sem_hora"]);
            //                    if (dataFinal.HasValue && result["prazo_cancelamento"].GetType() == typeof(int))
            //                    {
            //                        dataFinal = dataFinal.Value.AddDays((int)result["prazo_cancelamento"]);
            //                    }
            //                }

            //                ret.Add(new CancelamentoAutomaticoItem
            //                {
            //                    NumeroLinha = numeroLinha,
            //                    DataFinal = dataFinal,
            //                    Pedido = result["pedido"].ToString(),
            //                    Vendedor = result["vendedor"].ToString(),
            //                    NomeDoCliente = result["nome"].ToString(),
            //                    Analise_credito_descricao = result["analise_credito_descricao"].ToString(),
            //                    Loja = listaLojas.Where(r => r.Id == result["loja"].ToString()).FirstOrDefault()?.Nome
            //                });
            //            }
            //        }

            //    }

            //    //ret.AddRange(DadosDeTeste());

            //    //está ordenando por data CRESCENTE
            //    ret = ret.OrderBy(r => r.DataFinal).ToList();
            //    return ret;
            //}
            return null;
        }
    }
}


