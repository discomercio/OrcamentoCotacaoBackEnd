using InfraBanco;
using InfraBanco.Modelos;
using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InfraBanco.Constantes;

namespace Pedido
{
    public class MontarLogPedidoBll
    {
        private readonly PedidoBll pedidoBll;
        private readonly Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll pedidoVisualizacaoBll;
        private readonly Prepedido.MontarLogPrepedidoBll montarLogPrepedidoBll;

        public MontarLogPedidoBll(PedidoBll pedidoBll, Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll pedidoVisualizacaoBll,
            Prepedido.MontarLogPrepedidoBll montarLogPrepedidoBll)
        {
            this.pedidoBll = pedidoBll;
            this.pedidoVisualizacaoBll = pedidoVisualizacaoBll;
            this.montarLogPrepedidoBll = montarLogPrepedidoBll;
        }

        public async Task<string> MontarLogPedido(string pedido, ContextoBdGravacao dbgravacao, PedidoCriacaoDados pedidoCriacao)
        {
            /* OBS LOG PEDIDO => 
             *  Na tabela t_LOG => data completa / usuario logado / loja / 
             *  pedido se splitado salva o último filhote / operação / complemento = log
             */

            /* OBS 2
             * precisaremos apenas do número do pedido base para fazer uma busca na t_PEDIDO
             * irá retornar uma lista de pedido que contém o pedido pai
             * iremos montar igual ao que o asp faz em PedidoNovoConfirma.asp linha 2696 até 2893
             */
            string log = "";

            List<Tpedido> lstPedido = await (from c in dbgravacao.Tpedidos
                                             where c.Pedido.Contains(pedido)
                                             select c).ToListAsync();

            if (lstPedido != null)
            {
                if (lstPedido.Count > 0)
                {
                    foreach (var p in lstPedido)
                    {

                        //pedido pai e vamos montar o log com base nele
                        if (p.Pedido == pedido)
                            log = await MontarCamposPedidoPai(p, pedidoCriacao);

                        //adiciona detalhes sobre o auto-split
                        if (p.Pedido.Contains("-"))
                            log += "criar método que montar o log filhote";
                    }
                }
            }

            return log;
        }

        private async Task<string> MontarCamposPedidoPai(Tpedido pedido, PedidoCriacaoDados pedidoCriacao)
        {
            string log = "";

            log = "vl total=" + pedido.Vl_Total_Familia + "; ";
            log += "indicador=" + (!string.IsNullOrEmpty(pedido.Indicador) ? pedido.Indicador : "\"\"") + "; ";
            log += "vl_total_NF=" + pedido.Vl_Total_NF + "; ";
            log += "vl_total_RA=" + pedido.Vl_Total_RA + "; ";
            log += "perc_RT=" + pedido.Perc_RT + "; ";
            log += "qtde_parcelas=" + pedido.Qtde_Parcelas + "; ";

            if (!string.IsNullOrEmpty(pedido.Forma_Pagto))
                log += "forma_pagto=" + pedido.Forma_Pagto + "; ";
            if (!string.IsNullOrEmpty(pedido.Servicos))
                log += "servicos=" + pedido.Servicos + "; ";

            //OBS: ESSE CAMPO NÃO EXISTE EM NOSSO MAPEAMENTO
            //fiz uma busca no asp e não encontrei nada passando valor para esse campos
            //if (pedido.Vl_servicos != 0)
            //    log += "vl_servicos=" + pedido.Vl_servicos + "; ";

            if (!string.IsNullOrEmpty(pedido.St_Recebido))
                log += "st_recebido=" + pedido.St_Recebido + "; ";

            log += "st_etg_imediata=" + pedido.St_Etg_Imediata + "; ";
            if (pedido.St_Etg_Imediata == (short)InfraBanco.Constantes.Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO)
                log += "(previsão de entrega: " + pedido.PrevisaoEntregaData + ") ;";

            log += "StBemUsoConsumo=" + pedido.StBemUsoConsumo + "; ";
            log += "InstaladorInstalaStatus=" + pedido.InstaladorInstalaStatus + "; ";

            if (!string.IsNullOrEmpty(pedido.Obs_1))
                log += "obs_1=" + pedido.Obs_1 + "; ";
            if (!string.IsNullOrEmpty(pedido.Nfe_Texto_Constar))
                log += "NFe_texto_constar=" + pedido.Nfe_Texto_Constar + "; ";
            if (!string.IsNullOrEmpty(pedido.Nfe_XPed))
                log += "NFe_xPed=" + pedido.Nfe_XPed + "; ";
            if (!string.IsNullOrEmpty(pedido.Obs_2))
                log += "obs_2=" + pedido.Obs_2 + "; ";
            if (!string.IsNullOrEmpty(pedido.Pedido_Bs_X_Ac))
                log += "pedido_bs_x_ac=" + pedido.Pedido_Bs_X_Ac + "; ";
            if (!string.IsNullOrEmpty(pedido.Pedido_Bs_X_Marketplace))
                log += "pedido_bs_x_marketplace=" + pedido.Pedido_Bs_X_Marketplace + "; ";
            if (!string.IsNullOrEmpty(pedido.Marketplace_codigo_origem))
                log += "marketplace_codigo_origem=" + pedido.Marketplace_codigo_origem + "; ";
            if (!string.IsNullOrEmpty(pedido.Loja_Indicou))
            {
                log += "loja_indicou=" + pedido.Loja_Indicou + "; ";
                log += "comissao_loja_indicou=" + pedido.Comissao_Loja_Indicou + "; ";
            }

            if (pedido.Analise_Credito == short.Parse(InfraBanco.Constantes.Constantes.COD_AN_CREDITO_OK))
                log += "análise crédito OK (<=" + decimal.Parse(
                    await pedidoBll.LeParametroControle(Constantes.ID_PARAM_CAD_VL_APROV_AUTO_ANALISE_CREDITO)) + "; ";
            else log += "status da análise crédito: " + pedido.Analise_Credito + " - " +
                    pedidoVisualizacaoBll.DescricaoAnaliseCredito(Convert.ToString(pedido.Analise_Credito)) + "; ";

            //Forma de Pagamento(nova versão)
            //montamos os campos para inserir no log
            string camposAInserir = montarLogPrepedidoBll.MontarCamposAInserirFormaPagto(pedidoCriacao.FormaPagtoCriacao);
            //custo financeiro
            camposAInserir += "custoFinancFornecTipoParcelamento|custoFinancFornecQtdeParcelas|";
            //adiciona os campos de de forma de pagto formatado para log
            log += UtilsGlobais.Util.MontaLogInserir(pedido, log, camposAInserir, true);
            //endereço de cobrança
            log += MontarEndereçoCobranca(pedidoCriacao.EnderecoCadastralCliente);
            //endereço de entrega
            log += pedidoCriacao.EnderecoEntrega.OutroEndereco ?
                montarLogPrepedidoBll.MontaCamposAInserirEnderecoEntrega(pedidoCriacao.EnderecoEntrega) : " Endereço entrega=mesmo do cadastro|";
            //transportadora seleção automática
            log += !string.IsNullOrEmpty(pedido.Transportadora_Id) ?
                MontarTransportadora(pedido.Transportadora_Id, pedido.Transportadora_Selecao_Auto_Cep) : "Escolha automática de transportadora=N";
            //campos soltos: GarantiaIndicadorStatus / perc_desagio_RA_liquida / pedido_bs_x_at / pedido_bs_x_marketplace /
            //               cod_origem_pedido / indicador
            log += MontarCamposSoltos(pedido);

            //fim
            //juntar com o log da montagem de itens

            return await Task.FromResult(log);
        }

        public string MontarCamposAInserirItensPedido(string log, List<cl_ITEM_PEDIDO_NOVO> v_item, string s_log_cliente_indicador)
        {
            string logItem = "";

            foreach (var i in v_item)
            {
                //vamos montar os campos a inserir
                string campos_a_inserir = "";
                campos_a_inserir = MontarCamposAInserirItens(i);

                logItem += "\n\r";

                logItem = MontaLogInserirItens(i, campos_a_inserir, logItem);

            }

            log += logItem;
            return log;
        }

        private string MontarCamposAInserirItens(cl_ITEM_PEDIDO_NOVO item)
        {
            string campos_a_inserir = "";

            campos_a_inserir = "preco_lista|desc_dado|preco_venda|preco_NF|custoFinancFornecCoeficiente|custoFinancFornecPrecoListaBase|";

            if (item.Qtde_estoque_vendido != 0) campos_a_inserir += "estoque_vendido|";
            if (item.Qtde_estoque_sem_presenca != 0) campos_a_inserir += "estoque_sem_presenca|";
            if (item.Abaixo_min_status != 0)
                campos_a_inserir += "abaixo_min_status|abaixo_min_autorizacao|abaixo_min_autorizador|" +
                    "abaixo_min_superv_autorizador";

            return campos_a_inserir;
        }

        private string MontaLogInserirItens(cl_ITEM_PEDIDO_NOVO item, string campos_a_inserir, string log)
        {
            //montamos o produto ex: 1x001001(001);
            log += item.Qtde + "x" + item.produto;
            if (item.Fabricante != "" && item.Fabricante != null)
            {
                log += "(" + item.Fabricante + ");";
            }

            PropertyInfo[] property = item.GetType().GetProperties();
            string[] split = campos_a_inserir.Split('|');
            foreach (var s in split)
            {
                foreach (var c in property)
                {
                    //pegando o real nome da coluna 
                    ColumnAttribute column = (ColumnAttribute)Attribute.GetCustomAttribute(c, typeof(ColumnAttribute));
                    if (column != null)
                    {
                        string coluna = column.Name;
                        if (s == coluna)
                        {
                            if (coluna == "preco_lista")
                                log = log + "\r";
                            //pegando o valor coluna
                            var value = (c.GetValue(item, null));
                            if (string.IsNullOrEmpty(value.ToString()))
                                log = log + coluna + "=" + "\"\"" + "; ";
                            else
                                log = log + coluna + "=" + value + "; ";
                        }
                    }
                }

            }

            return log;
        }

        private string MontarEndereçoCobranca(Cliente.Dados.EnderecoCadastralClientePrepedidoDados end)
        {
            string campos_a_inserir = "";

            campos_a_inserir = " Endereço cobrança=" + FormatarEnderecoCobrancaParaLog(end);
            campos_a_inserir += "(email=" + (string.IsNullOrEmpty(end.Endereco_email) ? "\"\"" : end.Endereco_email) + ", ";
            campos_a_inserir += "email_xml=" + (string.IsNullOrEmpty(end.Endereco_email_xml) ? "\"\"" : end.Endereco_email_xml) + ", ";
            campos_a_inserir += "nome=" + (string.IsNullOrEmpty(end.Endereco_nome) ? "\"\"" : end.Endereco_nome) + ", ";
            campos_a_inserir += "ddd_res=" + (string.IsNullOrEmpty(end.Endereco_ddd_res) ? "\"\"" : end.Endereco_ddd_res) + ", ";
            campos_a_inserir += "tel_res=" + (string.IsNullOrEmpty(end.Endereco_tel_res) ? "\"\"" : end.Endereco_tel_res) + ", ";
            campos_a_inserir += "ddd_com=" + (string.IsNullOrEmpty(end.Endereco_ddd_com) ? "\"\"" : end.Endereco_ddd_com) + ", ";
            campos_a_inserir += "tel_com=" + (string.IsNullOrEmpty(end.Endereco_tel_com) ? "\"\"" : end.Endereco_tel_com) + ", ";
            campos_a_inserir += "ramal_com=" + (string.IsNullOrEmpty(end.Endereco_ramal_com) ? "\"\"" : end.Endereco_ramal_com) + ", ";
            campos_a_inserir += "ddd_cel=" + (string.IsNullOrEmpty(end.Endereco_ddd_cel) ? "\"\"" : end.Endereco_ddd_cel) + ", ";
            campos_a_inserir += "tel_cel=" + (string.IsNullOrEmpty(end.Endereco_tel_cel) ? "\"\"" : end.Endereco_tel_cel) + ", ";
            campos_a_inserir += "ddd_com_2=" + (string.IsNullOrEmpty(end.Endereco_ddd_com_2) ? "\"\"" : end.Endereco_ddd_com_2) + ", ";
            campos_a_inserir += "tel_com_2=" + (string.IsNullOrEmpty(end.Endereco_tel_com_2) ? "\"\"" : end.Endereco_tel_com_2) + ", ";
            campos_a_inserir += "ramal_com_2=" + (string.IsNullOrEmpty(end.Endereco_ramal_com_2) ? "\"\"" : end.Endereco_ramal_com_2) + ", ";
            campos_a_inserir += "tipo_pessoa=" + (string.IsNullOrEmpty(end.Endereco_tipo_pessoa) ? "\"\"" : end.Endereco_tipo_pessoa) + ", ";
            campos_a_inserir += "cnpj_cpf=" + (string.IsNullOrEmpty(end.Endereco_cnpj_cpf) ? "\"\"" : end.Endereco_cnpj_cpf) + ", ";
            campos_a_inserir += "contribuinte_icms_status=" + end.Endereco_contribuinte_icms_status + ", ";
            campos_a_inserir += "produtor_rural_status=" + end.Endereco_produtor_rural_status + ", ";
            campos_a_inserir += "ie=" + (string.IsNullOrEmpty(end.Endereco_ie) ? "\"\"" : end.Endereco_ie) + ", ";
            campos_a_inserir += "rg=" + (string.IsNullOrEmpty(end.Endereco_rg) ? "\"\"" : end.Endereco_rg) + ")";

            return campos_a_inserir;
        }

        private string FormatarEnderecoCobrancaParaLog(Cliente.Dados.EnderecoCadastralClientePrepedidoDados end)
        {
            return end.Endereco_logradouro + ", " + end.Endereco_numero + " " + end.Endereco_complemento +
                " - " + end.Endereco_bairro + " - " + end.Endereco_cidade + " - " + end.Endereco_uf + " - "
                + end.Endereco_cep;
        }

        private string MontarTransportadora(string selAutoTransportadoraId, string transpSelAutoCep)
        {
            string retorno = "";

            retorno = "Escolha automática de transportadora=S; ";
            retorno += "Transportadora=" + selAutoTransportadoraId + "; ";
            retorno += "CEP relacionado=" + transpSelAutoCep + "; ";

            return retorno;
        }

        private string MontarCamposSoltos(Tpedido pedido)
        {
            string retorno = "";

            retorno += "GarantiaIndicadorStatus=" + pedido.GarantiaIndicadorStatus + "; ";
            retorno = "perc_desagio_RA_liquida=" + pedido.Perc_Desagio_RA_Liquida + "; ";
            retorno += "pedido_bs_x_at=" + (!string.IsNullOrEmpty(pedido.Pedido_Bs_X_At) ? pedido.Pedido_Bs_X_At : "\"\"") + "; ";
            if (pedido.Loja == Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE)
                if (!string.IsNullOrEmpty(pedido.Pedido_Bs_X_Marketplace))
                {
                    retorno += "numero_pedido_marketplace=" + pedido.Pedido_Bs_X_Marketplace + "; ";
                    retorno += "cod_origem_pedido=" + pedido.Marketplace_codigo_origem + "; ";
                }
            //afazer: verificar se essa msg será utilizada
            //if operacao_origem = OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO then
            //        if s_log <> "" then s_log = s_log & ";"
            //        s_log = s_log & " Operação de origem: cadastramento semi-automático de pedido do e-commerce (nº Magento=" & c_numero_magento & ", t_MAGENTO_API_PEDIDO_XML.id=" & id_magento_api_pedido_xml & ")"
            //        end if
            //    end if ' if Not rs.Eof

            //indicador
            retorno += !string.IsNullOrEmpty(pedido.Indicador) ? pedido.Indicador : "";

            return retorno;
        }
    }

}
