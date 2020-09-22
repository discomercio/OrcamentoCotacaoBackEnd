using Loja.Data;
using Loja.Modelos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Loja.Bll.PedidoBll
{
    public class PedidoLogBll
    {
        public PedidoLogBll()
        {
        }

        //afazer: passar o ContextoBd para ContextoBdGravacao
        //o contextoBdProvider é o provider normal mesmo
        public async Task<string> PedidoLogInclusao(LojaContextoBd contextoBd, string id_pedido_base, string apelido,
            Loja.Bll.PedidoBll.PedidoBll pedidoBll, string rb_forma_pagto, bool usar_rb_end_entrega,
            string operacao_origem, LojaContextoBdProvider contextoBdProvider, string c_numero_magento,
            string id_magento_api_pedido_xml, string s_log_cliente_indicador)
        {
            //PedidoNovoConfirma.asp
            //linhas 2707 - s = "SELECT * FROM t_PEDIDO WHERE (pedido = '" & id_pedido_base & "')"
            // até 2901 - grava_log usuario, loja, id_pedido, cliente_selecionado, OP_LOG_PEDIDO_NOVO, s_log

            //'	LOG
            var pedido = await (from p in contextoBd.Tpedidos
                                where p.Pedido == id_pedido_base
                                select p).FirstAsync();

            string log = "";
            string campos_a_incluir = "vl_total|rb_RA|indicador|vl_total_NF|vl_total_RA|perc_RT|qtde_parcelas|";
            if (!string.IsNullOrWhiteSpace(pedido.Forma_Pagto))
                campos_a_incluir += "forma_pagto|";
            if (!string.IsNullOrWhiteSpace(pedido.Servicos))
                campos_a_incluir += "servicos|";
            if (pedido.Vl_Servicos != 0)
                campos_a_incluir += "vl_servicos|";
            if (!string.IsNullOrWhiteSpace(pedido.St_Recebido))
                campos_a_incluir += "st_recebido|";
            //sempre entra
            campos_a_incluir += "st_etg_imediata|";
            //sempre entra
            campos_a_incluir += "StBemUsoConsumo|";
            //sempre entra
            campos_a_incluir += "InstaladorInstalaStatus|";
            if (!string.IsNullOrWhiteSpace(pedido.Obs_1))
                campos_a_incluir += "obs_1|";
            if (!string.IsNullOrWhiteSpace(pedido.Nfe_Texto_Constar))
                campos_a_incluir += "NFe_texto_constar|";
            if (!string.IsNullOrWhiteSpace(pedido.Nfe_XPed))
                campos_a_incluir += "NFe_xPed|";
            if (!string.IsNullOrWhiteSpace(pedido.Obs_2))
                campos_a_incluir += "obs_2|";
            if (!string.IsNullOrWhiteSpace(pedido.Pedido_Bs_X_Ac))
                campos_a_incluir += "pedido_bs_x_ac|";
            if (!string.IsNullOrWhiteSpace(pedido.Pedido_Bs_X_Marketplace))
                campos_a_incluir += "pedido_bs_x_marketplace|";
            if (!string.IsNullOrWhiteSpace(pedido.St_Recebido))
                campos_a_incluir += "st_Recebido|";
            if (!string.IsNullOrWhiteSpace(pedido.Loja_Indicou))
            {
                campos_a_incluir += "loja_indicou|";
                campos_a_incluir += "comissao_loja_indicou|";
            }

            //especial
            log = Loja.Bll.Util.Util.MontaLogCamposIncluir(pedido, log, campos_a_incluir);

            if (pedido.Analise_Credito == int.Parse(Constantes.Constantes.COD_AN_CREDITO_OK))
            {
                float vl_aprov_auto_analise_credito = 0;
                var valor = await (from c in contextoBd.Tcontroles
                                   where c.Id_Nsu == Constantes.Constantes.ID_PARAM_CAD_VL_APROV_AUTO_ANALISE_CREDITO
                                   select c).FirstOrDefaultAsync();
                if (valor != null)
                    if (float.TryParse(valor.Nsu, out float valorConvertido))
                        vl_aprov_auto_analise_credito = valorConvertido;
                log += "; análise crédito OK (<=" + vl_aprov_auto_analise_credito.ToString() + ")";
            }
            else
            {
                log += "; status da análise crédito: " + pedido.Analise_Credito;
                //log += await pedidoBll.ObterAnaliseCredito(Convert.ToString(pedido.Analise_Credito), id_pedido_base, apelido);
            }




            //'	Forma de Pagamento (nova versão)
            campos_a_incluir = "";
            //sempre entra
            campos_a_incluir += "tipo_parcelamento|";
            if (rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA)
                campos_a_incluir += "av_forma_pagto|";
            else if (rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
            {
                campos_a_incluir += "pu_forma_pagto|";
                campos_a_incluir += "pu_valor|";
                campos_a_incluir += "pu_vencto_apos|";
            }
            else if (rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
            {
                campos_a_incluir += "pc_qtde_parcelas|";
                campos_a_incluir += "pc_valor_parcela|";
            }
            else if (rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
            {
                campos_a_incluir += "pc_maquineta_qtde_parcelas|";
                campos_a_incluir += "pc_maquineta_valor_parcela|";
            }
            else if (rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
            {
                campos_a_incluir += "pce_forma_pagto_entrada|";
                campos_a_incluir += "pce_forma_pagto_prestacao|";
                campos_a_incluir += "pce_entrada_valor|";
                campos_a_incluir += "pce_prestacao_qtde|";
                campos_a_incluir += "pce_prestacao_valor|";
                campos_a_incluir += "pce_prestacao_periodo|";
            }
            else if (rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
            {
                campos_a_incluir += "pse_forma_pagto_prim_prest|";
                campos_a_incluir += "pse_forma_pagto_demais_prest|";
                campos_a_incluir += "pse_prim_prest_valor|";
                campos_a_incluir += "pse_prim_prest_apos|";
                campos_a_incluir += "pse_demais_prest_qtde|";
                campos_a_incluir += "pse_demais_prest_valor|";
                campos_a_incluir += "pse_demais_prest_periodo|";
            }



            campos_a_incluir += "custoFinancFornecTipoParcelamento|";
            campos_a_incluir += "custoFinancFornecQtdeParcelas|";

            log = Loja.Bll.Util.Util.MontaLogCamposIncluir(pedido, log, campos_a_incluir);

            campos_a_incluir = "";
            log += "; Endereço cobrança ";
            var tcliente = await (from cliente in contextoBd.Tclientes where cliente.Id == pedido.Id_Cliente select cliente).FirstAsync();
            campos_a_incluir += "endereco|";
            campos_a_incluir += "endereco_numero|";
            campos_a_incluir += "endereco_complemento|";
            campos_a_incluir += "bairro|";
            campos_a_incluir += "cidade|";
            campos_a_incluir += "uf|";
            campos_a_incluir += "cep|";
            log = Loja.Bll.Util.Util.MontaLogCamposIncluir(tcliente, log, campos_a_incluir);

            var blnParametro = await Loja.Bll.Util.Util.BuscarRegistroParametro(Constantes.Constantes.ID_PARAMETRO_Flag_Pedido_MemorizacaoCompletaEnderecos,
                contextoBdProvider.GetContextoLeitura());
            var blnUsarMemorizacaoCompletaEnderecos = false;
            if (blnParametro != null)
                blnUsarMemorizacaoCompletaEnderecos = blnParametro.Campo_inteiro == 1;
            if (blnUsarMemorizacaoCompletaEnderecos)
            {
                campos_a_incluir = "";
                campos_a_incluir += "email|";
                campos_a_incluir += "email_xml|";
                campos_a_incluir += "nome|";
                campos_a_incluir += "ddd_res|";
                campos_a_incluir += "tel_res|";
                campos_a_incluir += "ddd_com|";
                campos_a_incluir += "tel_com|";
                campos_a_incluir += "ramal_com|";
                campos_a_incluir += "ddd_cel|";
                campos_a_incluir += "tel_cel|";
                campos_a_incluir += "ddd_com_2|";
                campos_a_incluir += "tel_com_2|";
                campos_a_incluir += "ramal_com_2|";
                campos_a_incluir += "tipo|";
                campos_a_incluir += "cnpj_cpf|";
                campos_a_incluir += "contribuinte_icms_status|";
                campos_a_incluir += "produtor_rural_status|";
                campos_a_incluir += "ie|";
                campos_a_incluir += "rg|";
                log = Loja.Bll.Util.Util.MontaLogCamposIncluir(tcliente, log, campos_a_incluir);
            }


            if (usar_rb_end_entrega)
            {
                log += "; Endereço entrega ";
                campos_a_incluir = "";
                campos_a_incluir += "EndEtg_endereco|";
                campos_a_incluir += "EndEtg_endereco_numero|";
                campos_a_incluir += "EndEtg_endereco_complemento|";
                campos_a_incluir += "EndEtg_bairro|";
                campos_a_incluir += "EndEtg_cidade|";
                campos_a_incluir += "EndEtg_uf|";
                campos_a_incluir += "EndEtg_cep|";
                campos_a_incluir += "EndEtg_Obs|";
                log = Loja.Bll.Util.Util.MontaLogCamposIncluir(pedido, log, campos_a_incluir);
                if (blnUsarMemorizacaoCompletaEnderecos)
                {
                    //obs: esses campos não existem no pedido!
                    campos_a_incluir = "";
                    campos_a_incluir += "EndEtg_email|";
                    campos_a_incluir += "EndEtg_email_xml|";
                    campos_a_incluir += "EndEtg_nome|";
                    campos_a_incluir += "EndEtg_ddd_res|";
                    campos_a_incluir += "EndEtg_tel_res|";
                    campos_a_incluir += "EndEtg_ddd_com|";
                    campos_a_incluir += "EndEtg_tel_com|";
                    campos_a_incluir += "EndEtg_ramal_com|";
                    campos_a_incluir += "EndEtg_ddd_cel|";
                    campos_a_incluir += "EndEtg_tel_cel|";
                    campos_a_incluir += "EndEtg_ddd_com_2|";
                    campos_a_incluir += "EndEtg_tel_com_2|";
                    campos_a_incluir += "EndEtg_ramal_com_2|";
                    campos_a_incluir += "EndEtg_tipo_pessoa|";
                    campos_a_incluir += "EndEtg_cnpj_cpf|";
                    campos_a_incluir += "EndEtg_contribuinte_icms_status|";
                    campos_a_incluir += "EndEtg_produtor_rural_status|";
                    campos_a_incluir += "EndEtg_ie|";
                    campos_a_incluir += "EndEtg_rg|";
                    log = Loja.Bll.Util.Util.MontaLogCamposIncluir(pedido, log, campos_a_incluir);
                }
            }
            else
            {
                log += "; Endereço entrega=mesmo do cadastro";
            }



            // -------------------------------------
            // -------------------------------------
            // transportadora
            if (string.IsNullOrWhiteSpace(pedido.Transportadora_Id))
            {
                log += "; Escolha automática de transportadora=N";
            }
            else
            {
                log += "; Escolha automática de transportadora=S";
                campos_a_incluir = "";
                campos_a_incluir += "Transportadora_Id|";
                campos_a_incluir += "transportadora_selecao_auto_cep|";
                log = Loja.Bll.Util.Util.MontaLogCamposIncluir(pedido, log, campos_a_incluir);
            }


            campos_a_incluir = "";
            campos_a_incluir += "GarantiaIndicadorStatus|";
            campos_a_incluir += "perc_desagio_RA_liquida|";
            campos_a_incluir += "pedido_bs_x_at|";
            log = Loja.Bll.Util.Util.MontaLogCamposIncluir(pedido, log, campos_a_incluir);


            if (pedido.Loja == Constantes.Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE)
            {

                campos_a_incluir = "";
                if (!string.IsNullOrWhiteSpace(pedido.Pedido_Bs_X_Marketplace))
                    campos_a_incluir += "pedido_bs_x_marketplace|";
                campos_a_incluir += "marketplace_codigo_origem|";
                log = Loja.Bll.Util.Util.MontaLogCamposIncluir(pedido, log, campos_a_incluir);
            }


            if (operacao_origem == Constantes.Constantes.OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO)
            {
                log += "; Operação de origem: cadastramento semi-automático de pedido do e-commerce (nº Magento=" + c_numero_magento + ", t_MAGENTO_API_PEDIDO_XML.id=" + id_magento_api_pedido_xml + ")";
            }


            if (!string.IsNullOrWhiteSpace(s_log_cliente_indicador))
            {
                log += s_log_cliente_indicador;
            }


            //'	MONTA LOG DOS ITENS
            var tpedidoItems = await (from tpi in contextoBd.TpedidoItems where tpi.Pedido == pedido.Pedido select tpi).ToListAsync();
            foreach (var tpi in tpedidoItems)
            {
                if (log != "")
                    log = log + ";" + "\n";

                /*
                 * 
                 * melhor fazr o log do registro completo
                 * 
                 * NÃO estamos fazendo log do qtde_estoque_vendido e do qtde_estoque_sem_presenca
                 * 
                 * 
                log_produto_monta(.qtde, .fabricante, .produto) & _
                            "; preco_lista=" & formata_texto_log(.preco_lista) & _
                            "; desc_dado=" & formata_texto_log(.desc_dado) & _
                            "; preco_venda=" & formata_texto_log(.preco_venda) & _
                            "; preco_NF=" & formata_texto_log(.preco_NF) & _
                            "; custoFinancFornecCoeficiente=" & formata_texto_log(.custoFinancFornecCoeficiente) & _
                            "; custoFinancFornecPrecoListaBase=" & formata_texto_log(.custoFinancFornecPrecoListaBase)
                    if .qtde_estoque_vendido <> 0 then s_log = s_log & "; estoque_vendido=" & formata_texto_log(.qtde_estoque_vendido)
                    if .qtde_estoque_sem_presenca <> 0 then s_log = s_log & "; estoque_sem_presenca=" & formata_texto_log(.qtde_estoque_sem_presenca)
                    if converte_numero(.abaixo_min_status) <> 0 then
                        s_log = s_log & _
                                "; abaixo_min_status=" & formata_texto_log(.abaixo_min_status) & _
                                "; abaixo_min_autorizacao=" & formata_texto_log(.abaixo_min_autorizacao) & _
                                "; abaixo_min_autorizador=" & formata_texto_log(.abaixo_min_autorizador) & _
                                "; abaixo_min_superv_autorizador=" & formata_texto_log(.abaixo_min_superv_autorizador)
                        end if

                */

                log = Loja.Bll.Util.Util.MontaLog(tpi, log, null);

            }

            return log;
        }

        //todo: precisa receber essa variável como parametro! a vLogAutoSplit
        public string PedidoLogInclusao_AutoSplit()
        { 

                /*
            '	ADICIONA DETALHES SOBRE O AUTO-SPLIT
                blnAchou = False
                for i = LBound(vLogAutoSplit) to UBound(vLogAutoSplit)
                    if Trim("" & vLogAutoSplit(i)) <> "" then
                        if s_log <> "" then s_log = s_log & chr(13)
                        if Not blnAchou then
                            s_log = s_log & "Detalhes do auto-split: Modo de seleção do CD = " & rb_selecao_cd
                            if rb_selecao_cd = MODO_SELECAO_CD__MANUAL then s_log = s_log & "; id_nfe_emitente = " & c_id_nfe_emitente_selecao_manual
                            s_log = s_log & chr(13)
                            blnAchou = True
                            end if
                        s_log = s_log & vLogAutoSplit(i)
                        end if
                    next
                */

                return "";
        }
    }
}
