using InfraBanco;
using InfraBanco.Constantes;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava90
{
#pragma warning disable IDE0054 // Use compound assignment
    class Grava90 : PassoBaseGravacao
    {
        public Grava90(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao, Execucao.Gravacao gravacao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao, gravacao)
        {
        }

        private string Formata_moeda(decimal? valor) => UtilsGlobais.Log.Formata_moeda_log(valor);
        private string Formata_texto_log(string? valor) => UtilsGlobais.Log.Formata_texto_log(valor);
        private string Formata_texto_log_float(float? valor) => UtilsGlobais.Log.Formata_texto_log_numeros<float?>(valor);
        private string Formata_texto_log_short(short? valor) => UtilsGlobais.Log.Formata_texto_log_numeros<short?>(valor);
        private string Formata_texto_log_int(int? valor) => UtilsGlobais.Log.Formata_texto_log_numeros<int?>(valor);
        private string Formata_texto_log_decimal(decimal? valor) => UtilsGlobais.Log.Formata_texto_log_numeros<decimal?>(valor);
        private string Formata_data(DateTime? valor) => UtilsGlobais.Log.Formata_data_log(valor);
        private string Formata_perc_comissao(float? valor) => UtilsGlobais.Log.Formata_perc_comissao_log(valor);

        public async Task ExecutarAsync()
        {
            //salvamos aqui proque o GravaLog faz o savechanges sem ser async
            await ContextoBdGravacao.SaveChangesAsync();

            //Passo90: log(Passo90 / Log.feature)
            string s_log = "";

            /*
             * loja/PedidoNovoConfirma.asp
              
             
                '	LOG
                    if alerta = "" then
                        s = "SELECT * FROM t_PEDIDO WHERE (pedido = '" & id_pedido_base & "')"
                        set rs = cn.execute(s)
                        if Not rs.Eof then
                            s_log = "vl total=" & formata_moeda(vl_total)
                            s_log = s_log & "; RA=" & formata_texto_log(rb_RA)
                            s_log = s_log & "; indicador=" & formata_texto_log(rs("indicador"))

            ..... até .....

                        if s_log <> "" then
                            grava_log usuario, loja, id_pedido, cliente_selecionado, OP_LOG_PEDIDO_NOVO, s_log
                            end if
                        end if

            */

            /*
Exemplo de log de PEDIDO INCLUSÃO:

vl total=1.053,81; RA=""; indicador=""; vl_total_NF=1.053,81; vl_total_RA=0,00; perc_RT=0; qtde_parcelas=1; st_etg_imediata=2; StBemUsoConsumo=1; InstaladorInstalaStatus=2; status da análise crédito: 9 - Crédito OK (aguardando depósito); tipo_parcelamento=1; av_forma_pagto=2; custoFinancFornecTipoParcelamento=AV; custoFinancFornecQtdeParcelas=0; Endereço cobrança=R Geraldina Verônica Batista de Camargo, 400 - Jardim Yolanda - São José do Rio Preto - SP - 15061-620; Endereço entrega=Rua José Carneiro, 7-65 - Vila Falcão - Bauru - SP - 17050-100 [EndEtg_cod_justificativa=002]; Escolha automática de transportadora=S; Transportadora=ATIVA; CEP relacionado=17050-100; GarantiaIndicadorStatus=0; perc_desagio_RA_liquida=30; pedido_bs_x_at=;
1x004007(004); preco_lista=1053,81; desc_dado=0; preco_venda=1053,81; preco_NF=1053,81; custoFinancFornecCoeficiente=1; custoFinancFornecPrecoListaBase=1053,81; estoque_vendido=1
Detalhes do auto-split: Modo de seleção do CD = AUTOMATICO
191959N (DIS/ES)
(004)004007: Qtde Solicitada = 1, Qtde Sem Presença Autorizada = 0, Qtde Estoque Vendido = 1, Qtde Sem Presença = 0
*/
            var tpedido_pai = Gravacao.Tpedido_pai;

            s_log = s_log + "vl total=" + Formata_moeda(tpedido_pai.Vl_Total_Familia); //a variável vl_total é salva em vl_total_familia
            s_log = s_log + "; RA=" + Formata_texto_log(tpedido_pai.Opcao_Possui_RA); //levemente diferente; colocamos no log o que salvamos no banco porque não recebemos esse flag
            s_log = s_log + "; indicador=" + Formata_texto_log(tpedido_pai.Indicador);
            s_log = s_log + "; vl_total_NF=" + Formata_moeda(tpedido_pai.Vl_Total_NF);
            s_log = s_log + "; vl_total_RA=" + Formata_moeda(tpedido_pai.Vl_Total_RA);
            s_log = s_log + "; perc_RT=" + Formata_texto_log_float(tpedido_pai.Perc_RT);
            s_log = s_log + "; qtde_parcelas=" + Formata_texto_log_short(tpedido_pai.Qtde_Parcelas);
            if (!string.IsNullOrWhiteSpace(tpedido_pai.Forma_Pagto))
                s_log = s_log + "; forma_pagto= " + Formata_texto_log(tpedido_pai.Forma_Pagto);
            if (!string.IsNullOrWhiteSpace(tpedido_pai.Servicos))
                s_log = s_log + "; servicos = " + Formata_texto_log(tpedido_pai.Servicos);



            /*
            esse campo não existe em nosso mapeamento
            fiz uma busca no asp e não encontrei nada passando valor para esse campos

            if (Trim("" & tpedido_pai.vl_servicos"))<>"") And (Trim("" & tpedido_pai.vl_servicos"))<>"0") 
                s_log = s_log & "; vl_servicos=" & formata_texto_log(tpedido_pai.vl_servicos")) 
                */

            if (!string.IsNullOrWhiteSpace(tpedido_pai.St_Recebido))
                s_log = s_log + "; st_recebido=" + Formata_texto_log(tpedido_pai.St_Recebido);

            //este campo sempre tem um valor: if (!string.IsNullOrWhiteSpace(tpedido_pai.St_Etg_Imediata))
            //a mesma coisa aocntece com diversos campos que são numéricos
            s_log = s_log + "; st_etg_imediata=" + Formata_texto_log_short(tpedido_pai.St_Etg_Imediata);
            if (tpedido_pai.St_Etg_Imediata == (short)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO)
                s_log = s_log + " (previsão de entrega: " + Formata_data(tpedido_pai.PrevisaoEntregaData) + ")";
            s_log = s_log + "; StBemUsoConsumo=" + Formata_texto_log_short(tpedido_pai.StBemUsoConsumo);
            s_log = s_log + "; InstaladorInstalaStatus=" + Formata_texto_log_short(tpedido_pai.InstaladorInstalaStatus);
            if (!string.IsNullOrWhiteSpace(tpedido_pai.Obs_1))
                s_log = s_log + "; obs_1=" + Formata_texto_log(tpedido_pai.Obs_1);
            if (!string.IsNullOrWhiteSpace(tpedido_pai.Nfe_Texto_Constar))
                s_log = s_log + "; NFe_texto_constar = " + Formata_texto_log(tpedido_pai.Nfe_Texto_Constar);
            if (!string.IsNullOrWhiteSpace(tpedido_pai.Nfe_XPed))
                s_log = s_log + "; NFe_xPed = " + Formata_texto_log(tpedido_pai.Nfe_XPed);
            if (!string.IsNullOrWhiteSpace(tpedido_pai.Obs_2))
                s_log = s_log + "; obs_2 = " + Formata_texto_log(tpedido_pai.Obs_2);
            if (!string.IsNullOrWhiteSpace(tpedido_pai.Pedido_Bs_X_Ac))
                s_log = s_log + "; pedido_bs_x_ac = " + Formata_texto_log(tpedido_pai.Pedido_Bs_X_Ac);
            if (!string.IsNullOrWhiteSpace(tpedido_pai.Pedido_Bs_X_Marketplace))
                s_log = s_log + "; pedido_bs_x_marketplace = " + Formata_texto_log(tpedido_pai.Pedido_Bs_X_Marketplace);
            if (!string.IsNullOrWhiteSpace(tpedido_pai.Marketplace_codigo_origem))
                s_log = s_log + "; marketplace_codigo_origem = " + Formata_texto_log(tpedido_pai.Marketplace_codigo_origem);
            if (!string.IsNullOrWhiteSpace(tpedido_pai.Loja_Indicou))
            {
                s_log = s_log + "; loja_indicou=" + Formata_texto_log(tpedido_pai.Loja_Indicou);
                s_log = s_log + "; comissao_loja_indicou=" + Formata_perc_comissao(tpedido_pai.Comissao_Loja_Indicou) + "%";
            }
            if (tpedido_pai.Analise_Credito.ToString() == Constantes.COD_AN_CREDITO_OK)
            {
                s_log = s_log + "; análise crédito OK (<=" + Formata_moeda(Execucao.Vl_aprov_auto_analise_credito) + ")";
            }
            else
            {
                s_log = s_log + "; status da análise crédito: " + Formata_texto_log_short(tpedido_pai.Analise_Credito) + " - "
                    + Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll.Descricao_analise_credito_CadastroPedido(Convert.ToString(tpedido_pai.Analise_Credito));
            }


            //'	Forma de Pagamento (nova versão)
            //a variável rb_forma_pagto é salva em rs("tipo_parcelamento")
            s_log = s_log + "; tipo_parcelamento=" + Formata_texto_log_short(tpedido_pai.Tipo_Parcelamento);
            if (tpedido_pai.Tipo_Parcelamento == short.Parse(Constantes.COD_FORMA_PAGTO_A_VISTA))
            {
                s_log = s_log + "; av_forma_pagto=" + Formata_texto_log_short(tpedido_pai.Av_Forma_Pagto);
            }
            else if (tpedido_pai.Tipo_Parcelamento == short.Parse(Constantes.COD_FORMA_PAGTO_PARCELA_UNICA))
            {
                s_log = s_log + "; pu_forma_pagto=" + Formata_texto_log_short(tpedido_pai.Pu_Forma_Pagto);
                s_log = s_log + "; pu_valor=" + Formata_moeda(tpedido_pai.Pu_Valor);
                s_log = s_log + "; pu_vencto_apos=" + Formata_texto_log_short(tpedido_pai.Pu_Vencto_Apos);
            }
            else if (tpedido_pai.Tipo_Parcelamento == short.Parse(Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO))
            {
                s_log = s_log + "; pc_qtde_parcelas=" + Formata_texto_log_short(tpedido_pai.Pc_Qtde_Parcelas);
                s_log = s_log + "; pc_valor_parcela=" + Formata_moeda(tpedido_pai.Pc_Valor_Parcela);
            }
            else if (tpedido_pai.Tipo_Parcelamento == short.Parse(Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA))
            {
                s_log = s_log + "; pc_maquineta_qtde_parcelas=" + Formata_texto_log_short(tpedido_pai.Pc_Maquineta_Qtde_Parcelas);
                s_log = s_log + "; pc_maquineta_valor_parcela=" + Formata_moeda(tpedido_pai.Pc_Maquineta_Valor_Parcela);
            }
            else if (tpedido_pai.Tipo_Parcelamento == short.Parse(Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA))
            {
                s_log = s_log + "; pce_forma_pagto_entrada=" + Formata_texto_log_short(tpedido_pai.Pce_Forma_Pagto_Entrada);
                s_log = s_log + "; pce_forma_pagto_prestacao=" + Formata_texto_log_short(tpedido_pai.Pce_Forma_Pagto_Prestacao);
                s_log = s_log + "; pce_entrada_valor=" + Formata_moeda(tpedido_pai.Pce_Entrada_Valor);
                s_log = s_log + "; pce_prestacao_qtde=" + Formata_texto_log_short(tpedido_pai.Pce_Prestacao_Qtde);
                s_log = s_log + "; pce_prestacao_valor=" + Formata_moeda(tpedido_pai.Pce_Prestacao_Valor);
                s_log = s_log + "; pce_prestacao_periodo=" + Formata_texto_log_short(tpedido_pai.Pce_Prestacao_Periodo);
            }
            else if (tpedido_pai.Tipo_Parcelamento == short.Parse(Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA))
            {
                s_log = s_log + "; pse_forma_pagto_prim_prest=" + Formata_texto_log_short(tpedido_pai.Pse_Forma_Pagto_Prim_Prest);
                s_log = s_log + "; pse_forma_pagto_demais_prest=" + Formata_texto_log_short(tpedido_pai.Pse_Forma_Pagto_Demais_Prest);
                s_log = s_log + "; pse_prim_prest_valor=" + Formata_moeda(tpedido_pai.Pse_Prim_Prest_Valor);
                s_log = s_log + "; pse_prim_prest_apos=" + Formata_texto_log_short(tpedido_pai.Pse_Prim_Prest_Apos);
                s_log = s_log + "; pse_demais_prest_qtde=" + Formata_texto_log_short(tpedido_pai.Pse_Demais_Prest_Qtde);
                s_log = s_log + "; pse_demais_prest_valor=" + Formata_moeda(tpedido_pai.Pse_Demais_Prest_Valor);
                s_log = s_log + "; pse_demais_prest_periodo=" + Formata_texto_log_short(tpedido_pai.Pse_Demais_Prest_Periodo);
            }

            s_log = s_log + "; custoFinancFornecTipoParcelamento=" + Formata_texto_log(tpedido_pai.CustoFinancFornecTipoParcelamento);
            s_log = s_log + "; custoFinancFornecQtdeParcelas=" + Formata_texto_log_short(tpedido_pai.CustoFinancFornecQtdeParcelas);



            s_log = s_log + "; Endereço cobrança=" + UtilsGlobais.Util.Formata_endereco(
                tpedido_pai.Endereco_logradouro, tpedido_pai.Endereco_numero,
                tpedido_pai.Endereco_complemento, tpedido_pai.Endereco_bairro,
                tpedido_pai.Endereco_cidade, tpedido_pai.Endereco_uf, tpedido_pai.Endereco_cep);

            //if blnUsarMemorizacaoCompletaEnderecos then
            s_log = s_log
                    + " ("
                    + "email=" + (tpedido_pai.Endereco_email ?? "")
                    + ", email_xml=" + (tpedido_pai.Endereco_email_xml ?? "")
                    + ", nome=" + (tpedido_pai.Endereco_nome ?? "")
                    + ", ddd_res=" + (tpedido_pai.Endereco_ddd_res ?? "")
                    + ", tel_res=" + (tpedido_pai.Endereco_tel_res ?? "")
                    + ", ddd_com=" + (tpedido_pai.Endereco_ddd_com ?? "")
                    + ", tel_com=" + (tpedido_pai.Endereco_tel_com ?? "")
                    + ", ramal_com=" + (tpedido_pai.Endereco_ramal_com ?? "")
                    + ", ddd_cel=" + (tpedido_pai.Endereco_ddd_cel ?? "")
                    + ", tel_cel=" + (tpedido_pai.Endereco_tel_cel ?? "")
                    + ", ddd_com_2=" + (tpedido_pai.Endereco_ddd_com_2 ?? "")
                    + ", tel_com_2=" + (tpedido_pai.Endereco_tel_com_2 ?? "")
                    + ", ramal_com_2=" + (tpedido_pai.Endereco_ramal_com_2 ?? "")
                    + ", tipo_pessoa=" + (tpedido_pai.Endereco_tipo_pessoa ?? "")
                    + ", cnpj_cpf=" + (tpedido_pai.Endereco_cnpj_cpf ?? "")
                    + ", contribuinte_icms_status=" + (tpedido_pai.Endereco_contribuinte_icms_status.ToString())
                    + ", produtor_rural_status=" + (tpedido_pai.Endereco_produtor_rural_status.ToString())
                    + ", ie=" + (tpedido_pai.Endereco_ie ?? "")
                    + ", rg=" + (tpedido_pai.Endereco_rg ?? "")
                    + ", contato=" + (tpedido_pai.Endereco_contato ?? "")
                    + ")";


            if (tpedido_pai.St_End_Entrega != 0)
            {
                s_log = s_log + "; Endereço entrega=" + UtilsGlobais.Util.Formata_endereco(
                    tpedido_pai.EndEtg_Endereco, tpedido_pai.EndEtg_Endereco_Numero,
                    tpedido_pai.EndEtg_Endereco_Complemento, tpedido_pai.EndEtg_Bairro,
                    tpedido_pai.EndEtg_Cidade, tpedido_pai.EndEtg_UF, tpedido_pai.EndEtg_Cep)
                    + " [EndEtg_cod_justificativa=" + tpedido_pai.EndEtg_Cod_Justificativa + "]";
                //if blnUsarMemorizacaoCompletaEnderecos then
                s_log = s_log
                            + " ("
                            + "email=" + (tpedido_pai.EndEtg_email ?? "")
                            + ", email_xml=" + (tpedido_pai.EndEtg_email_xml ?? "")
                            + ", nome=" + (tpedido_pai.EndEtg_nome ?? "")
                            + ", ddd_res=" + (tpedido_pai.EndEtg_ddd_res ?? "")
                            + ", tel_res=" + (tpedido_pai.EndEtg_tel_res ?? "")
                            + ", ddd_com=" + (tpedido_pai.EndEtg_ddd_com ?? "")
                            + ", tel_com=" + (tpedido_pai.EndEtg_tel_com ?? "")
                            + ", ramal_com=" + (tpedido_pai.EndEtg_ramal_com ?? "")
                            + ", ddd_cel=" + (tpedido_pai.EndEtg_ddd_cel ?? "")
                            + ", tel_cel=" + (tpedido_pai.EndEtg_tel_cel ?? "")
                            + ", ddd_com_2=" + (tpedido_pai.EndEtg_ddd_com_2 ?? "")
                            + ", tel_com_2=" + (tpedido_pai.EndEtg_tel_com_2 ?? "")
                            + ", ramal_com_2=" + (tpedido_pai.EndEtg_ramal_com_2 ?? "")
                            + ", tipo_pessoa=" + (tpedido_pai.EndEtg_tipo_pessoa ?? "")
                            + ", cnpj_cpf=" + (tpedido_pai.EndEtg_cnpj_cpf ?? "")
                            + ", contribuinte_icms_status=" + (tpedido_pai.EndEtg_contribuinte_icms_status.ToString())
                            + ", produtor_rural_status=" + (tpedido_pai.EndEtg_produtor_rural_status.ToString())
                            + ", ie=" + (tpedido_pai.EndEtg_ie ?? "")
                            + ", rg=" + (tpedido_pai.EndEtg_rg ?? "")
                            + ")";
            }
            else
                s_log = s_log + "; Endereço entrega=mesmo do cadastro";


            if (string.IsNullOrEmpty(Execucao.Transportadora.Transportadora_Id))
            {

                s_log = s_log + "; Escolha automática de transportadora=N";
            }
            else
            {
                s_log = s_log + "; Escolha automática de transportadora=S";
                s_log = s_log + "; Transportadora=" + (Execucao.Transportadora.Transportadora_Id ?? "");
                s_log = s_log + "; CEP relacionado=" + UtilsGlobais.Util.Cep_formata(Execucao.Transportadora.Transportadora_Selecao_Auto_Cep);
            }

            s_log = s_log + "; GarantiaIndicadorStatus=" + (tpedido_pai.GarantiaIndicadorStatus.ToString());
            s_log = s_log + "; perc_desagio_RA_liquida=" + Formata_texto_log_float(tpedido_pai.Perc_Desagio_RA_Liquida);
            s_log = s_log + "; pedido_bs_x_at=" + (tpedido_pai.Pedido_Bs_X_At ?? "");

            if (tpedido_pai.Loja == Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE)
            {
                if (!string.IsNullOrWhiteSpace(tpedido_pai.Pedido_Bs_X_Marketplace))
                    s_log = s_log + "; numero_pedido_marketplace=" + (tpedido_pai.Pedido_Bs_X_Marketplace ?? "");
                s_log = s_log + "; cod_origem_pedido=" + (tpedido_pai.Marketplace_codigo_origem ?? "");
            }


            if (Pedido.Ambiente.Operacao_origem == Constantes.Op_origem__pedido_novo.OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO)
            {
                if (!string.IsNullOrWhiteSpace(s_log))
                    s_log = s_log + ";";
                s_log = s_log + " Operação de origem: cadastramento semi-automático de pedido do e-commerce (nº Magento Pedido_bs_x_ac="
                    + (Pedido.Marketplace.Pedido_bs_x_ac ?? "");
                //nao tesmo estas variáveis!
                //+ c_numero_magento + ", t_MAGENTO_API_PEDIDO_XML.id=" + id_magento_api_pedido_xml + ")";
            }

            if (!string.IsNullOrWhiteSpace(Gravacao.Log_cliente_indicador))
            {
                if (!string.IsNullOrWhiteSpace(s_log))
                    s_log = s_log + "; ";
                s_log = s_log + Gravacao.Log_cliente_indicador;
            }

            foreach (var produto in Gravacao.ProdutoGravacaoLista)
            {
                if (!string.IsNullOrWhiteSpace(s_log))
                    s_log = s_log + ";" + UtilsGlobais.Log.EnterParaLogBanco();

                s_log = s_log +
                        UtilsGlobais.Log.Log_produto_monta(produto.Pedido.Qtde, produto.Pedido.Fabricante, produto.Pedido.Produto) +
                        "; preco_lista=" + Formata_texto_log_decimal(produto.Pedido.Preco_Lista) +
                        "; desc_dado=" + Formata_texto_log_float(produto.Pedido.Desc_Dado) +
                        "; preco_venda=" + Formata_texto_log_decimal(produto.Pedido.Preco_Venda) +
                        "; preco_NF=" + Formata_texto_log_decimal(produto.Pedido.Preco_NF) +
                        "; custoFinancFornecCoeficiente=" + Formata_texto_log_float(produto.Pedido.CustoFinancFornecCoeficiente_Conferencia) +
                        "; custoFinancFornecPrecoListaBase=" + Formata_texto_log_decimal(produto.Pedido.CustoFinancFornecPrecoListaBase_Conferencia);
                if (produto.Qtde_estoque_vendido != 0)
                    s_log = s_log + "; estoque_vendido=" + Formata_texto_log_int(produto.Qtde_estoque_vendido);
                if (produto.Qtde_estoque_sem_presenca != 0)
                    s_log = s_log + "; estoque_sem_presenca=" + Formata_texto_log_int(produto.Qtde_estoque_sem_presenca);

                if (produto.Abaixo_min_status)
                {
                    s_log = s_log +
                            "; abaixo_min_status=" + Formata_texto_log_int(produto.Abaixo_min_status ? 1 : 0) +
                            "; abaixo_min_autorizacao=" + Formata_texto_log(produto.Abaixo_min_autorizacao) +
                            "; abaixo_min_autorizador=" + Formata_texto_log(produto.Abaixo_min_autorizador) +
                            "; abaixo_min_superv_autorizador=" + Formata_texto_log(produto.Abaixo_min_superv_autorizador);
                }
            }
            //'	ADICIONA DETALHES SOBRE O AUTO-SPLIT
            var blnAchou = false;
            foreach (var vLogAutoSplit_i in Gravacao.VlogAutoSplit)
            {
                if (!String.IsNullOrWhiteSpace(vLogAutoSplit_i))
                {
                    if (!string.IsNullOrWhiteSpace(s_log))
                        s_log = s_log + UtilsGlobais.Log.EnterParaLogBanco();

                    if (!blnAchou)
                    {
                        s_log = s_log + "Detalhes do auto-split: Modo de seleção do CD = ";
                        if (Pedido.Ambiente.Id_nfe_emitente_selecao_manual == 0)
                            s_log = s_log + Constantes.MODO_SELECAO_CD__AUTOMATICO;
                        else
                            s_log = s_log + Constantes.MODO_SELECAO_CD__MANUAL;

                        if (Pedido.Ambiente.Id_nfe_emitente_selecao_manual != 0)
                            s_log = s_log + "; id_nfe_emitente = " + Formata_texto_log_int(Pedido.Ambiente.Id_nfe_emitente_selecao_manual);
                        s_log = s_log + UtilsGlobais.Log.EnterParaLogBanco();
                        blnAchou = true;
                    }
                    s_log = s_log + vLogAutoSplit_i;
                }
            }

            if (!UtilsGlobais.Util.GravaLog(ContextoBdGravacao, Pedido.Ambiente.Usuario, Pedido.Ambiente.Loja, tpedido_pai.Pedido,
                           Pedido.Cliente.Id_cliente, InfraBanco.Constantes.Constantes.OP_LOG_PEDIDO_NOVO, s_log))
                Retorno.ListaErros.Add("Falha ao gravar log.");

        }
    }
#pragma warning restore IDE0054 // Use compound assignment
}
