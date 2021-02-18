using InfraBanco.Constantes;
using Microsoft.EntityFrameworkCore;
using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Validacao
{
    class ConfigurarVariaveis
    {
        private readonly PedidoCriacaoDados Pedido;
        private readonly PedidoCriacaoRetornoDados Retorno;
        private readonly Pedido.Criacao.PedidoCriacao Criacao;
        public ConfigurarVariaveis(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
        {
            this.Pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));
            this.Retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            this.Criacao = pedidoCriacao ?? throw new ArgumentNullException(nameof(pedidoCriacao));
        }

        public async Task Executar()
        {
            await ConfigurarVariaveisExecutar();

            //todo: passar estas duas rotinas para o passo30, CamposMagentoExigidos
            await ConfigurarBlnPedidoECommerceCreditoOkAutomatico();
            await Cadastramento_de_pedidos_do_site_magento_da_bonshop();
        }

        private async Task ConfigurarVariaveisExecutar()
        {


            /*
'	OBTÉM O VALOR LIMITE P/ APROVAÇÃO AUTOMÁTICA DA ANÁLISE DE CRÉDITO
	if alerta = "" then
		s = "SELECT nsu FROM t_CONTROLE WHERE (id_nsu = '" & ID_PARAM_CAD_VL_APROV_AUTO_ANALISE_CREDITO & "')"
		set rs = cn.execute(s)
		if Not rs.Eof then
			vl_aprov_auto_analise_credito = converte_numero(rs("nsu"))
			end if
		if rs.State <> 0 then rs.Close
		end if
	*/
            {
                Criacao.Execucao.Vl_aprov_auto_analise_credito = 0;
                var valorBd = await (from l in Criacao.ContextoProvider.GetContextoLeitura().Tcontroles
                                     where l.Id_Nsu == Constantes.ID_PARAM_CAD_VL_APROV_AUTO_ANALISE_CREDITO
                                     select l.Nsu).ToListAsync();
                if (valorBd.Count() > 0)
                {
                    CultureInfo FormatarEmPortugues = new CultureInfo("pt-BR");
                    decimal.Parse(valorBd[0], FormatarEmPortugues);
                }
            }
            /*
        '	OBTÉM O PERCENTUAL DA COMISSÃO
            if alerta = "" then
                if s_loja_indicou<>"" then
                    s = "SELECT loja, comissao_indicacao FROM t_LOJA WHERE (loja='" & s_loja_indicou & "')"
                    set rs = cn.execute(s)
                    if Not rs.Eof then
                        comissao_loja_indicou = rs("comissao_indicacao")
                    else
                        alerta = "Loja " & s_loja_indicou & " não está cadastrada."
                        end if
                    end if
                end if

            */

            Criacao.Execucao.Comissao_loja_indicou = 0;
            if (!string.IsNullOrWhiteSpace(Pedido.Ambiente.Loja_indicou))
            {
                var valorBd = await (from l in Criacao.ContextoProvider.GetContextoLeitura().Tlojas
                                     where l.Loja == Pedido.Ambiente.Loja_indicou
                                     select l.Comissao_indicacao).ToListAsync();
                if (valorBd.Count() > 0)
                {
                    var valor = valorBd[0];
                    Criacao.Execucao.Comissao_loja_indicou = valor;
                }
            }
        }

        //embora não seja uma configuração de variáveis e sim uma validação, o código é mito próximo ao do ConfigurarBlnPedidoECommerceCreditoOkAutomatico
        private async Task Cadastramento_de_pedidos_do_site_magento_da_bonshop()
        {
            //todo: colocar isto
            /*
                'TRATAMENTO PARA CADASTRAMENTO DE PEDIDOS DO SITE MAGENTO DA BONSHOP
                if isLojaBonshop(loja) And (operacao_origem = OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO) then
                    if alerta = "" then
                        if s_pedido_ac = "" then
                            alerta=texto_add_br(alerta)
                            alerta=alerta & "Informe o nº pedido Magento"
                            end if

                        if s_pedido_ac <> "" then
                            if s_pedido_ac <> retorna_so_digitos(s_pedido_ac) then
                                alerta=texto_add_br(alerta)
                                alerta=alerta & "O número do pedido Magento deve conter apenas dígitos"
                                end if

                            do while Len(s_pedido_ac) < 9
                                if Len(s_pedido_ac) = 8 then
                                    s_pedido_ac = "2" & s_pedido_ac
                                else
                                    s_pedido_ac = "0" & s_pedido_ac
                                    end if
                                Loop

                            if Left(s_pedido_ac, 1) <> "2" then
                                alerta=texto_add_br(alerta)
                                alerta=alerta & "O número do pedido Magento inicia com dígito inválido para a loja " & loja
                                end if
                            end if 'if s_pedido_ac <> ""
                        end if 'if alerta = ""

                '	VERIFICA SE HÁ PEDIDO JÁ CADASTRADO COM O MESMO Nº PEDIDO MAGENTO (POSSÍVEL CADASTRO EM DUPLICIDADE)
                    if alerta = "" then
                        if s_pedido_ac <> "" then
                            s = "SELECT" & _
                                    " tP.pedido," & _
                                    " tP.pedido_bs_x_ac," & _
                                    " tP.data_hora," & _
                                    " tP.vendedor," & _
                                    " tU.nome AS nome_vendedor," & _
                                    " tP.usuario_cadastro," & _
                                    " tUC.nome AS nome_usuario_cadastro," & _
                                    " tP.endereco_cnpj_cpf AS cnpj_cpf," & _
                                    " tP.endereco_nome AS nome_cliente" & _
                                " FROM t_PEDIDO tP" & _
                                    " LEFT JOIN t_USUARIO tU ON (tP.vendedor = tU.usuario)" & _
                                    " LEFT JOIN t_USUARIO tUC ON (tP.usuario_cadastro = tUC.usuario)" & _
                                " WHERE" & _
                                    " (tP.st_entrega <> '" & ST_ENTREGA_CANCELADO & "')" & _
                                    " AND (pedido_bs_x_ac = '" & s_pedido_ac & "')" & _
                                    " AND (" & _
                                        "tP.pedido NOT IN (" & _
                                            "SELECT DISTINCT" & _
                                                " pedido" & _
                                            " FROM t_PEDIDO_DEVOLUCAO tPD" & _
                                            " WHERE" & _
                                                " (tPD.pedido = tP.pedido)" & _
                                                " AND (status IN (" & _
                                                    COD_ST_PEDIDO_DEVOLUCAO__FINALIZADA & "," & _
                                                    COD_ST_PEDIDO_DEVOLUCAO__MERCADORIA_RECEBIDA & "," & _
                                                    COD_ST_PEDIDO_DEVOLUCAO__EM_ANDAMENTO & "," & _
                                                    COD_ST_PEDIDO_DEVOLUCAO__CADASTRADA & _
                                                    ")" & _
                                                ")" & _
                                            ")" & _
                                        ")" & _
                                    " AND (" & _
                                        "tP.pedido NOT IN (" & _
                                            "SELECT DISTINCT" & _
                                                " pedido" & _
                                            " FROM t_PEDIDO_ITEM_DEVOLVIDO tPID" & _
                                            " WHERE" & _
                                                " (tPID.pedido = tP.pedido)" & _
                                            ")" & _
                                        ")"
                            set rs = cn.execute(s)
                            if Not rs.Eof then
                                alerta=texto_add_br(alerta)
                                alerta=alerta & "O nº pedido Magento " & Trim("" & rs("pedido_bs_x_ac")) & " já está cadastrado no pedido " & Trim("" & rs("pedido"))
                                alerta=texto_add_br(alerta)
                                alerta=alerta & "Data de cadastramento do pedido: " & formata_data_hora_sem_seg(rs("data_hora"))
                                alerta=texto_add_br(alerta)
                                if UCase(Trim("" & rs("vendedor"))) = UCase(Trim("" & rs("usuario_cadastro"))) then
                                    alerta=alerta & "Cadastrado por: " & Trim("" & rs("vendedor"))
                                    if Ucase(Trim("" & rs("vendedor"))) <> Ucase(Trim("" & rs("nome_vendedor"))) then alerta=alerta & " (" & Trim("" & rs("nome_vendedor")) & ")"
                                else
                                    alerta=alerta & "Cadastrado por: " & Trim("" & rs("usuario_cadastro"))
                                    if Ucase(Trim("" & rs("usuario_cadastro"))) <> Ucase(Trim("" & rs("nome_usuario_cadastro"))) then alerta=alerta & " (" & Trim("" & rs("nome_usuario_cadastro")) & ")"
                                    alerta=texto_add_br(alerta)
                                    alerta=alerta & "Vendedor: " & Trim("" & rs("vendedor"))
                                    if Ucase(Trim("" & rs("vendedor"))) <> Ucase(Trim("" & rs("nome_vendedor"))) then alerta=alerta & " (" & Trim("" & rs("nome_vendedor")) & ")"
                                    end if
                                alerta=texto_add_br(alerta)
                                alerta=alerta & "Cliente: " & cnpj_cpf_formata(Trim("" & rs("cnpj_cpf"))) & " - " & Trim("" & rs("nome_cliente"))
                                end if 'if Not rs.Eof
                            end if 'if s_pedido_ac <> "" then
                        end if 'if alerta = "" then
                    end if 'if isLojaBonshop(loja) And (operacao_origem = OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO)
                    */
        }
        private async Task ConfigurarBlnPedidoECommerceCreditoOkAutomatico()
        {

            /*
                'TRATAMENTO PARA CADASTRAMENTO DE PEDIDOS DO SITE MAGENTO DO ARCLUBE
                dim blnPedidoECommerceCreditoOkAutomatico
                blnPedidoECommerceCreditoOkAutomatico = False
                if loja = NUMERO_LOJA_ECOMMERCE_AR_CLUBE then
                */
            Criacao.Execucao.BlnPedidoECommerceCreditoOkAutomatico = false;
            if (Pedido.Ambiente.Loja != Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE)
                return;

            /*
            if s_origem_pedido = "" then
                alerta = "Informe a origem do pedido"
                end if
            */
            if (string.IsNullOrWhiteSpace(Pedido.Marketplace.Marketplace_codigo_origem))
            {
                Retorno.ListaErros.Add($"Informe a origem do pedido (Marketplace_codigo_origem)");
                return;
            }


            /*
             *
        if alerta = "" then
        '	PARA PEDIDOS DO ARCLUBE, É PERMITIDO FICAR SEM O Nº MAGENTO SOMENTE NOS SEGUINTES CASOS:
        '		1) PEDIDO ORIGINADO PELO TELEVENDAS
        '		2) PEDIDO GERADO CONTRA A TRANSPORTADORA (EM CASOS QUE A TRANSPORTADORA SE RESPONSABILIZA PELA REPOSIÇÃO DE MERCADORIA EXTRAVIADA)
            if (Trim(s_origem_pedido) <> "002") And (Trim(s_origem_pedido) <> "019") then
                if s_pedido_ac = "" then
                    alerta=texto_add_br(alerta)
                    alerta=alerta & "Informe o nº Magento"
                    end if
                end if
                */
            //s_pedido_ac = Pedido.Marketplace.Pedido_bs_x_ac
            //s_origem_pedido = Pedido.Marketplace.Marketplace_codigo_origem
            if (Pedido.Marketplace.Marketplace_codigo_origem.Trim() != "002"
                && Pedido.Marketplace.Marketplace_codigo_origem.Trim() != "019")
            {
                if (string.IsNullOrWhiteSpace(Pedido.Marketplace.Pedido_bs_x_ac))
                {
                    Retorno.ListaErros.Add($"Informe o nº Magento (Pedido_bs_x_ac)");
                }
            }

            /*
        if s_pedido_ac <> "" then
            if s_pedido_ac <> retorna_so_digitos(s_pedido_ac) then
                alerta=texto_add_br(alerta)
                alerta=alerta & "O número Magento deve conter apenas dígitos"
            end if

            do while Len(s_pedido_ac) < 9
                if Len(s_pedido_ac) = 8 then
                    s_pedido_ac = "1" & s_pedido_ac
                else
                    s_pedido_ac = "0" & s_pedido_ac
                    end if
                Loop

            if Left(s_pedido_ac, 1) <> "1" then
                alerta=texto_add_br(alerta)
                alerta=alerta & "O número do pedido Magento inicia com dígito inválido para a loja " & loja
                end if
            end if 'if s_pedido_ac <> ""
            */

            if (!string.IsNullOrWhiteSpace(Pedido.Marketplace.Pedido_bs_x_ac))
            {
                if (Regex.IsMatch(Pedido.Marketplace.Pedido_bs_x_ac, @"\D"))
                    Retorno.ListaErros.Add("O número Magento deve conter apenas dígitos");

                while (Pedido.Marketplace.Pedido_bs_x_ac.Length < 9)
                {
                    if (Pedido.Marketplace.Pedido_bs_x_ac.Length == 8)
                        Pedido.Marketplace.Pedido_bs_x_ac = "1" + Pedido.Marketplace.Pedido_bs_x_ac;
                    else
                        Pedido.Marketplace.Pedido_bs_x_ac = "0" + Pedido.Marketplace.Pedido_bs_x_ac;
                }

            }
            /*
        s = "SELECT * FROM t_CODIGO_DESCRICAO WHERE (grupo = 'PedidoECommerce_Origem') AND (codigo = '" & s_origem_pedido & "')"
        set rs = cn.execute(s)
        if rs.Eof then
            alerta=texto_add_br(alerta)
            alerta=alerta & "Código de origem do pedido (marketplace) não cadastrado: " & s_origem_pedido
        else
        */
            var codigoDescricaoOrigem = await (from l in Criacao.ContextoProvider.GetContextoLeitura().TcodigoDescricaos
                                               where l.Grupo == Constantes.GRUPO_T_CODIGO_DESCRICAO__PEDIDOECOMMERCE_ORIGEM
                                                  && l.Codigo == Pedido.Marketplace.Marketplace_codigo_origem
                                               select new { l.Descricao, l.Codigo_pai }).ToListAsync();
            if (codigoDescricaoOrigem.Count() <= 0)
            {
                Retorno.ListaErros.Add("Código de origem do pedido (marketplace) não cadastrado: " + (Pedido.Marketplace.Marketplace_codigo_origem ?? ""));
            }
            else
            {
                /*
            '	PROCESSA OS PARÂMETROS DEFINIDOS PARA A ORIGEM (GRUPO)
                s = "SELECT * FROM T_CODIGO_DESCRICAO WHERE (grupo = 'PedidoECommerce_Origem_Grupo') AND (codigo = '" & Trim("" & rs("codigo_pai")) & "')"
                set rs2 = cn.execute(s)
                if Not rs2.Eof then
                '	OBTÉM O PERCENTUAL DE COMISSÃO DO MARKETPLACE
                    perc_RT = rs2("parametro_campo_real")
                '	DEVE COLOCAR AUTOMATICAMENTE COM 'CRÉDITO OK'?
                    if rs2("parametro_1_campo_flag") = 1 then blnPedidoECommerceCreditoOkAutomatico = True
                '	Nº PEDIDO MARKETPLACE É OBRIGATÓRIO?
                    if rs2("parametro_2_campo_flag") = 1 then
                        if s_numero_mktplace = "" then
                            alerta=texto_add_br(alerta)
                            alerta=alerta & "Informe o nº do pedido do marketplace (" & Trim("" & rs("descricao")) & ")"
                            end if
                        end if
                    end if 'if Not rs2.Eof then
                */
                var codigo_pai = codigoDescricaoOrigem[0].Codigo_pai?.Trim() ?? "";
                var codigoDescricaoOrigemGrupo = await (from l in Criacao.ContextoProvider.GetContextoLeitura().TcodigoDescricaos
                                                        where l.Grupo == Constantes.GRUPO_T_CODIGO_DESCRICAO__PEDIDOECOMMERCE_ORIGEM_GRUPO
                                                           && l.Codigo == codigo_pai
                                                        select new
                                                        {
                                                            l.Descricao,
                                                            l.Codigo_pai,
                                                            l.Parametro_campo_real,
                                                            l.Parametro_1_campo_flag,
                                                            l.Parametro_2_campo_flag
                                                        }).ToListAsync();
                if (codigoDescricaoOrigemGrupo.Count() > 0)
                {
                    var codigoDescricaoOrigemGrupoRegistro = codigoDescricaoOrigemGrupo[0];
                    //'	OBTÉM O PERCENTUAL DE COMISSÃO DO MARKETPLACE
                    Pedido.Valor.Perc_RT = codigoDescricaoOrigemGrupoRegistro.Parametro_campo_real;
                    //'	DEVE COLOCAR AUTOMATICAMENTE COM 'CRÉDITO OK'?
                    if (codigoDescricaoOrigemGrupoRegistro.Parametro_1_campo_flag == 1)
                        Criacao.Execucao.BlnPedidoECommerceCreditoOkAutomatico = true;
                    //'	Nº PEDIDO MARKETPLACE É OBRIGATÓRIO?
                    if (codigoDescricaoOrigemGrupoRegistro.Parametro_2_campo_flag == 1)
                    {
                        if (string.IsNullOrWhiteSpace(Pedido.Marketplace.Pedido_bs_x_marketplace))
                        {
                            Retorno.ListaErros.Add("Informe o nº do pedido do marketplace (" + (codigoDescricaoOrigem[0].Descricao ?? "") + ")");
                        }
                    }
                }

                /*
                if s_numero_mktplace <> "" then
                    s = ""
                    For i = 1 To Len(s_numero_mktplace)
                        c = Mid(s_numero_mktplace, i, 1)
                        If IsNumeric(c) Or c = chr(45) Then s = s & c
                        Next
                    if s_numero_mktplace <> s then
                        alerta=texto_add_br(alerta)
                        alerta=alerta & "O número Marketplace deve conter apenas dígitos e hífen"
                        end if
                    end if
                    */
                if (!String.IsNullOrWhiteSpace(Pedido.Marketplace.Pedido_bs_x_marketplace))
                {
                    var digitosEhifen = "";
                    foreach (char c in Pedido.Marketplace.Pedido_bs_x_marketplace)
                    {
                        if ((c >= '0' && c <= '9') || c == '-')
                            digitosEhifen = digitosEhifen + c;
                    }
                    if (digitosEhifen != Pedido.Marketplace.Pedido_bs_x_marketplace)
                        Retorno.ListaErros.Add("O número Marketplace deve conter apenas dígitos e hífen");
                }
            }

        }
    }
}
