﻿using InfraBanco.Constantes;
using InfraBanco.Modelos;
using PrepedidoBusiness.Dto.ClienteCadastro;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace PrepedidoBusiness.Bll.PrepedidoBll
{
    public class MontarLogPrepedidoBll
    {
        public string MontarCamposAInserirPrepedido(Torcamento torcamento, PrePedidoDto prepedido)
        {
            //montamos os valores a serem inseridos
            string campos_a_inserir = MontarCamposAInserirValorTotal(torcamento.Vl_Total.ToString());
            //montamos alguns detalhes do prepedido
            campos_a_inserir += MontarCamposAInserirDetalhes(torcamento);
            //montamos a forma de pagto selecionada
            campos_a_inserir += MontarCamposAInserirFormaPagto(prepedido.FormaPagtoCriacao);
            //montamos os custos
            campos_a_inserir += "custoFinancFornecTipoParcelamento|custoFinancFornecQtdeParcelas|";
            //montamos os novos campos de endereço cadastral
            campos_a_inserir += MontarCamposDadosCadastrais();

            //montamos os campos de endereço de entrega se existir
            campos_a_inserir += prepedido.EnderecoEntrega.OutroEndereco ?
                MontaCamposAInserirEnderecoEntrega(prepedido.EnderecoEntrega) : " Endereço entrega=mesmo do cadastro|";

            campos_a_inserir += "InstaladorInstalaStatus|GarantiaIndicadorStatus|perc_desagio_RA_liquida";

            return campos_a_inserir;
        }

        private string MontarCamposAInserirValorTotal(string vlTotal)
        {
            string campos_a_inserir = "";
            //estamos recendo esse param de entrada, pois esse campos vl total não tem
            campos_a_inserir = "vl total=" + vlTotal + "|vl_total_NF|vl_total_RA|qtde_parcelas|perc_RT|midia|";

            return campos_a_inserir;
        }

        private string MontarCamposAInserirDetalhes(Torcamento orcamento)
        {
            string campos_a_inserir = "";

            if (orcamento.Forma_Pagamento != "" && orcamento.Forma_Pagamento != null)
            {
                campos_a_inserir += "forma_pagto|";
            }
            if (orcamento.Servicos != "" && orcamento.Servicos != null)
            {
                campos_a_inserir += "servicos|";
            }
            if (orcamento.Vl_Servicos.ToString() != "" && orcamento.Vl_Servicos != 0)
            {
                campos_a_inserir += "vl_servicos|";
            }
            if (orcamento.St_Etg_Imediata.ToString() != "")
            {
                campos_a_inserir += "st_etg_imediata|";
            }
            if(orcamento.St_Etg_Imediata == (short)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO)
            {
                campos_a_inserir += "(previsão de entrega: " + orcamento.PrevisaoEntregaData.ToString() + ")|";
            }
            if (orcamento.StBemUsoConsumo.ToString() != "")
            {
                campos_a_inserir += "StBemUsoConsumo|";
            }
            if (orcamento.Obs_1 != "" && orcamento.Obs_1 != null)
            {
                campos_a_inserir += "obs_1|";
            }
            if (orcamento.Obs_2 != "" && orcamento.Obs_2 != null)
            {
                campos_a_inserir += "obs_2|";
            }

            return campos_a_inserir;
        }

        private string MontarCamposAInserirFormaPagto(FormaPagtoCriacaoDto forma_pagto_criacao)
        {
            string campos_a_inserir = "";
            campos_a_inserir += "tipo_parcelamento|";

            if (forma_pagto_criacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
                campos_a_inserir = "av_forma_pagto|";
            else if (forma_pagto_criacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
            {
                campos_a_inserir += "pu_forma_pagto|pu_valor|pu_vencto_apos|";
            }
            else if (forma_pagto_criacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
                campos_a_inserir += "pc_qtde_parcelas|pc_valor_parcela|";
            else if (forma_pagto_criacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                campos_a_inserir += "pc_maquineta_qtde_parcelas|pc_maquineta_valor_parcela|";
            else if (forma_pagto_criacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
                campos_a_inserir += "pce_forma_pagto_entrada|pce_forma_pagto_prestacao|pce_entrada_valor|pce_prestacao_qtde|" +
                    "pce_prestacao_valor|pce_prestacao_periodo|";
            else if (forma_pagto_criacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
                campos_a_inserir += "pse_forma_pagto_prim_prest|pse_forma_pagto_demais_prest|pse_prim_prest_valor|pse_prim_prest_apos|" +
                    "pse_demais_prest_qtde|pse_demais_prest_valor|pse_demais_prest_periodo|";


            return campos_a_inserir;
        }

        private string MontarCamposDadosCadastrais()
        {
            string campos_a_inserir = "endereco_logradouro|endereco_bairro|endereco_cidade|endereco_uf|" +
                "endereco_cep|endereco_numero|endereco_email|endereco_email_xml|endereco_nome|endereco_ddd_res" +
                "endereco_tel_res|endereco_ddd_com|endereco_tel_com|endereco_ramal_com|endereco_ddd_cel" +
                "endereco_tel_cel|endereco_ddd_com_2|endereco_tel_com_2|endereco_ramal_com_2|endereco_tipo_pessoa" +
                "endereco_cnpj_cpf|endereco_contribuinte_icms_status|endereco_produtor_rural_status|endereco_ie" +
                "endereco_rg|endereco_contato";

            return campos_a_inserir;
        }

        private string MontaCamposAInserirEnderecoEntrega(EnderecoEntregaDtoClienteCadastro end)
        {
            string campos_a_inserir = "";

            if (end.OutroEndereco)
            {
                campos_a_inserir = " Endereço entrega = " + FormatarEnderecoEntregaParaLog(end) +
                    " [EndEtg_cod_justificativa=" + end.EndEtg_cod_justificativa + "]";
                campos_a_inserir += "(email=" + end.EndEtg_email + ", ";
                campos_a_inserir += "email_xml=" + end.EndEtg_email_xml + ", ";
                campos_a_inserir += "nome=" + end.EndEtg_nome + ", ";
                campos_a_inserir += "ddd_res=" + end.EndEtg_ddd_res + ", ";
                campos_a_inserir += "tel_res=" + end.EndEtg_tel_res + ", ";
                campos_a_inserir += "ddd_com=" + end.EndEtg_ddd_com + ", ";
                campos_a_inserir += "tel_com=" + end.EndEtg_tel_com + ", ";
                campos_a_inserir += "ramal_com=" + end.EndEtg_ramal_com + ", ";
                campos_a_inserir += "ddd_cel=" + end.EndEtg_ddd_cel + ", ";
                campos_a_inserir += "tel_cel=" + end.EndEtg_tel_cel + ", ";
                campos_a_inserir += "ddd_com_2=" + end.EndEtg_ddd_com_2 + ", ";
                campos_a_inserir += "tel_com_2=" + end.EndEtg_tel_com_2 + ", ";
                campos_a_inserir += "ramal_com_2=" + end.EndEtg_ramal_com_2 + ", ";
                campos_a_inserir += "tipo_pessoa=" + end.EndEtg_tipo_pessoa + ", ";
                campos_a_inserir += "cnpj_cpf=" + end.EndEtg_cnpj_cpf + ", ";
                campos_a_inserir += "contribuinte_icms_status=" + end.EndEtg_contribuinte_icms_status + ", ";
                campos_a_inserir += "produtor_rural_status=" + end.EndEtg_produtor_rural_status + ", ";
                campos_a_inserir += "ie=" + end.EndEtg_ie + ", ";
                campos_a_inserir += "rg=" + end.EndEtg_rg + "); ";
            }

            return campos_a_inserir;
        }

        private string FormatarEnderecoEntregaParaLog(EnderecoEntregaDtoClienteCadastro end)
        {
            return end.EndEtg_endereco + ", " + end.EndEtg_endereco_numero + " " + end.EndEtg_endereco_complemento +
                " - " + end.EndEtg_bairro + " - " + end.EndEtg_cidade + " - " + end.EndEtg_uf + " - "
                + end.EndEtg_cep;
        }

        public string MontarCamposAInserirItensPrepedido(List<TorcamentoItem> lstOrcItens, string log)
        {
            string logItem = "";
            foreach (var i in lstOrcItens)
            {
                //vamos montar os campos a inserir
                string campos_a_inserir = "";
                campos_a_inserir = MontarCamposAInserirItens(i);

                logItem += "\n";

                logItem = MontaLogInserirItens(i, campos_a_inserir, logItem);

            }
            log += logItem;
            return log;
        }

        private string MontarCamposAInserirItens(TorcamentoItem item)
        {
            string campos_a_inserir = "";

            campos_a_inserir = "preco_lista|desc_dado|preco_venda|preco_NF|obs|custoFinancFornecCoeficiente|custoFinancFornecPrecoListaBase|";

            if (item.Qtde_Spe > 0)
            {
                campos_a_inserir += "spe|";
            }
            if (item.Abaixo_Min_Status != 0 && item.Abaixo_Min_Status != null)
            {
                campos_a_inserir += "abaixo_min_status|abaixo_min_autorizacao|abaixo_min_autorizador|abaixo_min_superv_autorizador";
            }

            return campos_a_inserir;
        }

        private string MontaLogInserirItens(TorcamentoItem item, string campos_a_inserir, string log)
        {
            //montamos o produto ex: 1x001001(001);
            log += item.Qtde + "x" + item.Produto;
            if (item.Fabricante != "" && item.Fabricante != null)
            {
                log += "(" + item.Fabricante + ");";
            }

            PropertyInfo[] property = item.GetType().GetProperties();

            foreach (var c in property)
            {

                //pegando o real nome da coluna 
                ColumnAttribute column = (ColumnAttribute)Attribute.GetCustomAttribute(c, typeof(ColumnAttribute));
                if (column != null)
                {
                    string coluna = column.Name;
                    if (campos_a_inserir.Contains("preco_NF"))
                    {

                    }
                    if (campos_a_inserir.Contains(coluna))
                    {

                        //pegando o valor coluna
                        var value = (c.GetValue(item, null));
                        log = log + coluna + "=" + value + "; ";
                    }
                }
            }

            return log;
        }
    }
}