using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Pedido.Criacao.Execucao.UtilsLoja;
using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Execucao
{
    class VerificarPagtoPreferencial
    {
        /*
         * loja/PedidoNovoConfirma.asp
         * de
         * ANALISA O PERCENTUAL DE COMISSÃO+DESCONTO
         * dim perc_comissao_e_desconto_a_utilizar
         * ..... até .....
		 * perc_comissao_e_desconto_a_utilizar = rCD.perc_max_comissao_e_desconto_nivel2
		 * 
		 * ou seja, o cáclulo do perc_comissao_e_desconto_a_utilizar
         * */

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0054:Use compound assignment", Justification = "Estilo de código")]
        public static async Task<float> Calcular_perc_comissao_e_desconto_a_utilizar(PedidoCriacaoDados pedido,
            Pedido.Criacao.Execucao.UtilsLoja.PercentualMaxDescEComissao PercentualMaxDescEComissao, ContextoBdProvider contextoBdProvider)
        {

            /*
			'	OBTÉM A RELAÇÃO DE MEIOS DE PAGAMENTO PREFERENCIAIS (QUE FAZEM USO O PERCENTUAL DE COMISSÃO+DESCONTO NÍVEL 2)
				dim rP, vMPN2
				set rP = get_registro_t_parametro(ID_PARAMETRO_PercMaxComissaoEDesconto_Nivel2_MeiosPagto)
				if Trim("" & rP.id) <> "" then
					vMPN2 = Split(rP.campo_texto, ",")
					for i=Lbound(vMPN2) to Ubound(vMPN2)
						vMPN2(i) = Trim("" & vMPN2(i))
						next
				else
					redim vMPN2(0)
					vMPN2(0) = ""
					end if
			*/
            /* 4- busca get_registro_t_parametro(ID_PARAMETRO_PercMaxComissaoEDesconto_Nivel2_MeiosPagto) */
            Tparametro? tParametro = await UtilsGlobais.Util.BuscarRegistroParametro(
                Constantes.ID_PARAMETRO_PercMaxComissaoEDesconto_Nivel2_MeiosPagto, contextoBdProvider);
            List<string> vMPN2 = new List<string>();
            if (tParametro != null && !string.IsNullOrEmpty(tParametro.Id))
            {
                //a verificação é feita na linha 380 ate 388
                vMPN2 = tParametro.Campo_texto.Split(',').ToList();
            }



            var tipoPessoaJuridica = pedido.Cliente.Tipo.PessoaJuridica();
            float perc_comissao_e_desconto_a_utilizar;
            //'	ANALISA O PERCENTUAL DE COMISSÃO+DESCONTO

            if (tipoPessoaJuridica)
                perc_comissao_e_desconto_a_utilizar = PercentualMaxDescEComissao.Perc_Max_Comissao_E_Desconto_Pj;
            else
                perc_comissao_e_desconto_a_utilizar = PercentualMaxDescEComissao.Perc_Max_Comissao_E_Desconto;



            //trata as diversas formas de pagamento
            perc_comissao_e_desconto_a_utilizar = VerificarNivel2(PercentualMaxDescEComissao, vMPN2, tipoPessoaJuridica,
                perc_comissao_e_desconto_a_utilizar, pedido.FormaPagtoCriacao.Rb_forma_pagto,
                Constantes.COD_FORMA_PAGTO_A_VISTA, pedido.FormaPagtoCriacao.Op_av_forma_pagto);

            perc_comissao_e_desconto_a_utilizar = VerificarNivel2(PercentualMaxDescEComissao, vMPN2, tipoPessoaJuridica,
                perc_comissao_e_desconto_a_utilizar, pedido.FormaPagtoCriacao.Rb_forma_pagto,
                Constantes.COD_FORMA_PAGTO_PARCELA_UNICA, pedido.FormaPagtoCriacao.Op_pu_forma_pagto);

            perc_comissao_e_desconto_a_utilizar = VerificarNivel2(PercentualMaxDescEComissao, vMPN2, tipoPessoaJuridica,
                perc_comissao_e_desconto_a_utilizar, pedido.FormaPagtoCriacao.Rb_forma_pagto,
                Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO, Constantes.ID_FORMA_PAGTO_CARTAO);

            perc_comissao_e_desconto_a_utilizar = VerificarNivel2(PercentualMaxDescEComissao, vMPN2, tipoPessoaJuridica,
                perc_comissao_e_desconto_a_utilizar, pedido.FormaPagtoCriacao.Rb_forma_pagto,
                Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA, Constantes.ID_FORMA_PAGTO_CARTAO_MAQUINETA);



            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
            {
                //'	Identifica e contabiliza o valor da entrada
                var blnPreferencial = false;
                var s_pg = (pedido.FormaPagtoCriacao.Op_pce_entrada_forma_pagto ?? "").Trim();
                if (!String.IsNullOrWhiteSpace(s_pg))
                {
                    foreach (var vMPN2_i in vMPN2)
                    {
                        //'	O meio de pagamento selecionado é um dos preferenciais
                        if (s_pg.Trim() == vMPN2_i.Trim())
                        {
                            blnPreferencial = true;
                            break;
                        }
                    }
                }

                decimal vlNivel1 = 0, vlNivel2 = 0;
                if (blnPreferencial)
                    vlNivel2 = pedido.FormaPagtoCriacao.C_pce_entrada_valor ?? 0;
                else
                    vlNivel1 = pedido.FormaPagtoCriacao.C_pce_entrada_valor ?? 0;

                //'	Identifica e contabiliza o valor das parcelas
                blnPreferencial = false;
                s_pg = (pedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto ?? "").Trim();
                if (!String.IsNullOrWhiteSpace(s_pg))
                {
                    foreach (var vMPN2_i in vMPN2)
                    {
                        //'	O meio de pagamento selecionado é um dos preferenciais
                        if (s_pg.Trim() == vMPN2_i.Trim())
                        {
                            blnPreferencial = true;
                            break;
                        }
                    }
                }

                //a variavel vlNivel1 efetivamente não é usada. Mantivemos para reaproveitar o fluxo do ASP.
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                if (blnPreferencial)
                    vlNivel2 = vlNivel2 + (pedido.FormaPagtoCriacao.C_pce_prestacao_qtde ?? 0) * (pedido.FormaPagtoCriacao.C_pce_prestacao_valor ?? 0);
                else
                    vlNivel1 = vlNivel1 + (pedido.FormaPagtoCriacao.C_pce_prestacao_qtde ?? 0) * (pedido.FormaPagtoCriacao.C_pce_prestacao_valor ?? 0);
#pragma warning restore IDE0059 // Unnecessary assignment of a value

                //'	O montante a pagar por meio de pagamento preferencial é maior que 50% do total?
                if (vlNivel2 > (pedido.Valor.Vl_total / 2))
                {
                    if (tipoPessoaJuridica)
                        perc_comissao_e_desconto_a_utilizar = PercentualMaxDescEComissao.Perc_Max_Comissao_E_Desconto_Nivel2_Pj;
                    else
                        perc_comissao_e_desconto_a_utilizar = PercentualMaxDescEComissao.Perc_Max_Comissao_E_Desconto_Nivel2;

                }

            }
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
            {
                //'	Identifica e contabiliza o valor da 1ª parcela
                var blnPreferencial = false;
                var s_pg = (pedido.FormaPagtoCriacao.Op_pse_prim_prest_forma_pagto ?? "").Trim();
                if (!String.IsNullOrWhiteSpace(s_pg))
                {
                    foreach (var vMPN2_i in vMPN2)
                    {
                        //'	O meio de pagamento selecionado é um dos preferenciais
                        if (s_pg.Trim() == vMPN2_i.Trim())
                        {
                            blnPreferencial = true;
                            break;
                        }
                    }
                }

                decimal vlNivel1 = 0, vlNivel2 = 0;
                if (blnPreferencial)
                    vlNivel2 = pedido.FormaPagtoCriacao.C_pse_prim_prest_valor ?? 0;
                else
                    vlNivel1 = pedido.FormaPagtoCriacao.C_pse_prim_prest_valor ?? 0;

                //'	Identifica e contabiliza o valor das parcelas
                blnPreferencial = false;
                s_pg = (pedido.FormaPagtoCriacao.Op_pse_demais_prest_forma_pagto ?? "").Trim();
                if (!String.IsNullOrWhiteSpace(s_pg))
                {
                    foreach (var vMPN2_i in vMPN2)
                    {
                        //'	O meio de pagamento selecionado é um dos preferenciais
                        if (s_pg.Trim() == vMPN2_i.Trim())
                        {
                            blnPreferencial = true;
                            break;
                        }
                    }
                }

                //a variavel vlNivel1 efetivamente não é usada. Mantivemos para reaproveitar o fluxo do ASP.
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                if (blnPreferencial)
                    vlNivel2 = vlNivel2 + (pedido.FormaPagtoCriacao.C_pse_demais_prest_qtde ?? 0) * (pedido.FormaPagtoCriacao.C_pse_demais_prest_valor ?? 0);
                else
                    vlNivel1 = vlNivel1 + (pedido.FormaPagtoCriacao.C_pse_demais_prest_qtde ?? 0) * (pedido.FormaPagtoCriacao.C_pse_demais_prest_valor ?? 0);
#pragma warning restore IDE0059 // Unnecessary assignment of a value

                //'	O montante a pagar por meio de pagamento preferencial é maior que 50% do total?
                if (vlNivel2 > (pedido.Valor.Vl_total / 2))
                {
                    if (tipoPessoaJuridica)
                        perc_comissao_e_desconto_a_utilizar = PercentualMaxDescEComissao.Perc_Max_Comissao_E_Desconto_Nivel2_Pj;
                    else
                        perc_comissao_e_desconto_a_utilizar = PercentualMaxDescEComissao.Perc_Max_Comissao_E_Desconto_Nivel2;

                }
            }

            return perc_comissao_e_desconto_a_utilizar;
        }

        private static float VerificarNivel2(PercentualMaxDescEComissao PercentualMaxDescEComissao, List<string> vMPN2, bool tipoPessoaJuridica, float perc_comissao_e_desconto_a_utilizar, string rb_forma_pagto, string rb_forma_pagto_tratar, string op_forma_pagto)
        {

            /*
             * modelo de um bloco:
                            if rb_forma_pagto = COD_FORMA_PAGTO_A_VISTA then
                                s_pg = Trim(op_av_forma_pagto)
                                if s_pg <> "" then
                                    for i=Lbound(vMPN2) to Ubound(vMPN2)
                                    '	O meio de pagamento selecionado é um dos preferenciais
                                        if Trim("" & s_pg) = Trim("" & vMPN2(i)) then
                                            if EndCob_tipo_pessoa = ID_PJ then
                                                perc_comissao_e_desconto_a_utilizar = rCD.perc_max_comissao_e_desconto_nivel2_pj
                                            else
                                                perc_comissao_e_desconto_a_utilizar = rCD.perc_max_comissao_e_desconto_nivel2
                                                end if
                                            exit for
                                            end if
                                        next
                                    end if
                        */

            if (rb_forma_pagto == rb_forma_pagto_tratar)
            {
                string s_pg = (op_forma_pagto ?? "").Trim();
                if (!String.IsNullOrWhiteSpace(s_pg))
                {
                    foreach (var vMPN2_i in vMPN2)
                    {
                        //'	O meio de pagamento selecionado é um dos preferenciais
                        if (s_pg.Trim() == vMPN2_i.Trim())
                        {
                            if (tipoPessoaJuridica)
                                perc_comissao_e_desconto_a_utilizar = PercentualMaxDescEComissao.Perc_Max_Comissao_E_Desconto_Nivel2_Pj;
                            else
                                perc_comissao_e_desconto_a_utilizar = PercentualMaxDescEComissao.Perc_Max_Comissao_E_Desconto_Nivel2;
                            break;
                        }
                    }
                }
            }

            return perc_comissao_e_desconto_a_utilizar;
        }


    }
}
