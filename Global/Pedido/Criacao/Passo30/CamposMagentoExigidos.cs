using InfraBanco.Constantes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo30
{
    partial class Passo30
    {
        private async Task CamposMagentoExigidos()
        {
            await ConfigurarBlnPedidoECommerceCreditoOkAutomatico();
            await Cadastramento_de_pedidos_do_site_magento_da_bonshop();
        }

        //o código é muito próximo ao do ConfigurarBlnPedidoECommerceCreditoOkAutomatico
        private async Task Cadastramento_de_pedidos_do_site_magento_da_bonshop()
        {
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
                	end if 'if isLojaBonshop(loja) And (operacao_origem = OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO)
                    */
            if (!await UtilsGlobais.Util.IsLojaBonshop(Pedido.Ambiente.Loja, Criacao.ContextoProvider))
                return;
            if (Pedido.Ambiente.Operacao_origem != Constantes.Op_origem__pedido_novo.OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO)
                return;
            Exigir_Pedido_bs_x_ac();
            Validar_digito_Pedido_bs_x_ac("2");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0054:Use compound assignment", Justification = "Estilo de código")]
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
                Exigir_Pedido_bs_x_ac();
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
            Validar_digito_Pedido_bs_x_ac("1");

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

        private void Validar_digito_Pedido_bs_x_ac(string digito_para_loja)
        {
            if (!string.IsNullOrWhiteSpace(Pedido.Marketplace.Pedido_bs_x_ac))
            {
                if (Regex.IsMatch(Pedido.Marketplace.Pedido_bs_x_ac, @"\D"))
                    Retorno.ListaErros.Add("O número Magento deve conter apenas dígitos");

                while (Pedido.Marketplace.Pedido_bs_x_ac.Length < 9)
                {
                    if (Pedido.Marketplace.Pedido_bs_x_ac.Length == 8)
                        Pedido.Marketplace.Pedido_bs_x_ac = digito_para_loja + Pedido.Marketplace.Pedido_bs_x_ac;
                    else
                        Pedido.Marketplace.Pedido_bs_x_ac = "0" + Pedido.Marketplace.Pedido_bs_x_ac;
                }
                if (Pedido.Marketplace.Pedido_bs_x_ac.Substring(0, 1) != digito_para_loja)
                    Retorno.ListaErros.Add("O número do pedido Magento inicia com dígito inválido para a loja " + Pedido.Ambiente.Loja);
            }
        }

        private void Exigir_Pedido_bs_x_ac()
        {
            if (string.IsNullOrWhiteSpace(Pedido.Marketplace.Pedido_bs_x_ac))
            {
                Retorno.ListaErros.Add($"Informe o nº Magento (Pedido_bs_x_ac)");
            }
        }
    }
}

