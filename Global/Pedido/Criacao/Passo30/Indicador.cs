using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InfraBanco.Constantes;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo30
{
    partial class Passo30
    {
        private async Task Indicador()
        {
            /*
			#loja/PedidoNovoConsiste.asp
			#		if rb_indicacao = "" then
			#			alerta = "Informe se o pedido é com indicação ou não."
			#		elseif rb_indicacao = "S" then
			#			if c_indicador = "" then
			#				alerta = "Informe quem é o indicador."
			#			elseif rb_RA = "" then
			#				alerta = "Informe se o pedido possui RA ou não."
			#				end if
			*/
            if (Pedido.Ambiente.ComIndicador && String.IsNullOrWhiteSpace(Pedido.Ambiente.Indicador))
                Retorno.ListaErros.Add("Informe quem é o indicador.");

            /*
            #' ___________________________________________________________________________
            #' INDICADORES MONTA ITENS SELECT
            #' LEMBRE-SE: O ORÇAMENTISTA É CONSIDERADO AUTOMATICAMENTE UM INDICADOR!!
            #function indicadores_monta_itens_select(byval id_default, byref strResp, byref strJsScript)
            #
            #	if ID_PARAM_SITE = COD_SITE_ASSISTENCIA_TECNICA then
            #		strSql = "SELECT " & _
            #					"*" & _
            #				" FROM t_ORCAMENTISTA_E_INDICADOR" & _
            #				" WHERE" & _
            #					" (status = 'A')" & _
            #				" ORDER BY" & _
            #					" apelido"
            */
            if (!String.IsNullOrWhiteSpace(Pedido.Ambiente.Indicador))
            {
                IQueryable<InfraBanco.Modelos.TorcamentistaEindicador> listaIndicadores;
                if (Pedido.Ambiente.Id_param_site == InfraBanco.Constantes.Constantes.Cod_site.COD_SITE_ASSISTENCIA_TECNICA)
                {
                    listaIndicadores = (from orc in Criacao.ContextoProvider.GetContextoLeitura().TorcamentistaEindicadors
                                        where orc.Status == "A"
                                        select orc);
                }
                else
                {
                    /*
                    #	else
                    #		'10/01/2020 - Unis - Desativação do acesso dos vendedores a todos os parceiros da Unis
                    #		if (False And isLojaVrf(loja)) Or (loja = NUMERO_LOJA_ECOMMERCE_AR_CLUBE) then
                    #		'	TODOS OS VENDEDORES COMPARTILHAM OS MESMOS INDICADORES
                    #			strSql = "SELECT " & _
                    #						"*" & _
                    #					" FROM t_ORCAMENTISTA_E_INDICADOR" & _
                    #					" WHERE" & _
                    #						" (status = 'A')" & _
                    #						" AND (loja = '" & loja & "')" & _
                    #					" ORDER BY" & _
                    #						" apelido"
                    #		elseif (loja = NUMERO_LOJA_OLD03) Or (loja = NUMERO_LOJA_OLD03_BONIFICACAO) Or (operacao_permitida(OP_LJA_SELECIONAR_QUALQUER_INDICADOR_EM_PEDIDO_NOVO, s_lista_operacoes_permitidas)) then
                    #		'	OLD03: LISTA COMPLETA DOS INDICADORES LIBERADA
                    #			strSql = "SELECT " & _
                    #						"*" & _
                    #					" FROM t_ORCAMENTISTA_E_INDICADOR" & _
                    #					" WHERE" & _
                    #						" (status = 'A')" & _
                    #					" ORDER BY" & _
                    #						" apelido"
                    */
                    if (Pedido.Ambiente.Loja == Constantes.NUMERO_LOJA_OLD03
                        || Pedido.Ambiente.Loja == Constantes.NUMERO_LOJA_OLD03_BONIFICACAO
                        || Criacao.Execucao.UsuarioPermissao.Operacao_permitida(Constantes.OP_LJA_SELECIONAR_QUALQUER_INDICADOR_EM_PEDIDO_NOVO))
                    {
                        listaIndicadores = (from orc in Criacao.ContextoProvider.GetContextoLeitura().TorcamentistaEindicadors
                                            where orc.Status == "A"
                                            select orc);
                    }
                    else
                    {
                        /*
                        #		else
                        #			strSql = "SELECT " & _
                        #						"*" & _
                        #					" FROM t_ORCAMENTISTA_E_INDICADOR" & _
                        #					" WHERE" & _
                        #						" (status = 'A')" & _
                        #						" AND (vendedor = '" & usuario & "')" & _
                        #					" ORDER BY" & _
                        #						" apelido"
                        #			end if
                        #		end if
                        */
                        listaIndicadores = (from orc in Criacao.ContextoProvider.GetContextoLeitura().TorcamentistaEindicadors
                                            where orc.Status == "A"
                                            && orc.Vendedor == Pedido.Ambiente.Usuario
                                            select orc);
                    }
                }
                //Pedido.Ambiente.Indicador é o t_ORCAMENTISTA_E_INDICADOR.apelido
                var existe = await (from orc in listaIndicadores
                                    where orc.Apelido == Pedido.Ambiente.Indicador.ToUpper()
                                    select new { orc.Apelido, orc.Permite_RA_Status } ).ToListAsync();
                if (!existe.Any())
                    Retorno.ListaErros.Add($"Indicador inválido: {Pedido.Ambiente.Indicador}.");


                /*
                 * validação feita em 
                 * Pedido.Criacao.Passo50.Passo50.ValidarRA()
                 * 
                 * Lá também se valida o indicador, mas aqui a validação do indicador é mais restritiva
                 * 
                    #loja/PedidoNovoConfirma.asp
                    #Se não tiver indicador e tentar criar um pedido com RA, tem que dar erro
                    #	dim permite_RA_status
                    #	permite_RA_status = 0
                    #	if alerta = "" then
                    #		if c_indicador <> "" then
                    #			if Not le_orcamentista_e_indicador(c_indicador, r_orcamentista_e_indicador, msg_erro) then
                    #				alerta = "Falha ao recuperar os dados do indicador!!<br>" & msg_erro
                    #			else
                    #				permite_RA_status = r_orcamentista_e_indicador.permite_RA_status
                    #				end if
                    #			end if
                    #		end if
                */

            }


            if (String.IsNullOrWhiteSpace(Pedido.Ambiente.Indicador) && Pedido.Valor.PedidoPossuiRa())
            {
                Retorno.ListaErros.Add("Necessário indicador para usar RA");
            }


            /*
            #if (f.c_loja.value != NUMERO_LOJA_ECOMMERCE_AR_CLUBE) {
            #if (f.c_indicador.value == "") {
            #if (f.c_perc_RT.value != "") {
            #if (parseFloat(f.c_perc_RT.value.replace(',','.')) > 0) {
            # alert('Não é possível gravar o pedido com o campo "Indicador" vazio e "COM(%)" maior do que zero!!');
            # f.c_perc_RT.focus();
            # return;
            * */
            if (Pedido.Ambiente.Loja != Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE
            && string.IsNullOrWhiteSpace(Pedido.Ambiente.Indicador)
            && Pedido.Valor.Perc_RT != 0)
            {
                Retorno.ListaErros.Add("Não é possível gravar o pedido com o campo \"Indicador\" vazio e \"COM(%)\" maior do que zero!!");
            }
        }
    }
}
