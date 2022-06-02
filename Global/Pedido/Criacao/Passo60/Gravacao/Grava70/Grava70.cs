using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Pedido.Dados.Criacao;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava70
{
    class Grava70 : PassoBaseGravacao
    {
        public Grava70(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao, Execucao.Gravacao gravacao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao, gravacao)
        {
        }

        public async Task Executar(Tpedido tpedido, int indice_pedido)
        {
            //Passo70: ajustes adicionais no pedido pai
            await RegistrarSenhasDesconto(indice_pedido);
            CalcularRaLiquido(tpedido, indice_pedido);
            await AtualizarIndicador(indice_pedido);
        }

        private async Task AtualizarIndicador(int indice_pedido)
        {
            Gravacao.Log_cliente_indicador = "";

            //	INDICADOR: SE ESTE PEDIDO É COM INDICADOR E O CLIENTE AINDA NÃO TEM UM INDICADOR NO CADASTRO, ENTÃO CADASTRA ESTE. (Passo70 / CadastroIndicador.feature)

            /*
					if indice_pedido = 1 then
					'	INDICADOR: SE ESTE PEDIDO É COM INDICADOR E O CLIENTE AINDA NÃO TEM UM INDICADOR NO CADASTRO, ENTÃO CADASTRA ESTE.
						if rb_indicacao = "S" then
							if indicador_original = "" then
								s="UPDATE t_CLIENTE SET indicador='" & c_indicador & "' WHERE (id='" & cliente_selecionado & "')"
								cn.Execute(s)
								s_log_cliente_indicador = "Cadastrado o indicador '" & c_indicador & "' no cliente id=" & cliente_selecionado
								end if
							end if
						end if
						*/

            if (indice_pedido != 1)
                return;
            if (string.IsNullOrEmpty(Pedido.Ambiente.Indicador))
                return;

            //se já tem indicador, não atualizamos
            if (!string.IsNullOrEmpty(Pedido.Cliente.Indicador))
                return;

            var tcliente = await (from c in ContextoBdGravacao.Tcliente
                                  where c.Id == Execucao.Id_cliente
                                  select c).FirstAsync();
            if (tcliente.Indicador == Pedido.Ambiente.Indicador)
                return;

            tcliente.Indicador = Pedido.Ambiente.Indicador;
            ContextoBdGravacao.Update(tcliente);

            var s_log_cliente_indicador = "Cadastrado o indicador '" + Pedido.Ambiente.Indicador + "' no cliente id=" + Execucao.Id_cliente;
            Gravacao.Log_cliente_indicador = s_log_cliente_indicador;
        }


        private void CalcularRaLiquido(Tpedido tpedido, int indice_pedido)
        {
            // if Not calcula_total_RA_liquido_BD(id_pedido, vl_total_RA_liquido, msg_erro) then
            //	No pai e nos filhotes, atualiza campos de RA (Passo70/calcula_total_RA_liquido_BD.feture)

            /*
			 * 
					if Not calcula_total_RA_liquido_BD(id_pedido, vl_total_RA_liquido, msg_erro) then
						end if

					if indice_pedido = 1 then
						s = "SELECT * FROM t_PEDIDO WHERE (pedido='" & id_pedido & "')"
						if rs.State <> 0 then rs.Close
						rs.open s, cn
						rs("vl_total_RA_liquido") = vl_total_RA_liquido
						rs("qtde_parcelas_desagio_RA") = 0
						if vl_total_RA <> 0 then
							rs("st_tem_desagio_RA") = 1
						else
							rs("st_tem_desagio_RA") = 0
							end if
						rs.Update
						end if
						*/
            if (indice_pedido != 1)
                return;

            tpedido.Vl_Total_RA_Liquido = Calcula_total_RA_liquido_BD(tpedido);
            tpedido.Qtde_Parcelas_Desagio_RA = 0;
            if (Pedido.Valor.PedidoPossuiRa())
                tpedido.St_Tem_Desagio_RA = 1;
            else
                tpedido.St_Tem_Desagio_RA = 0;


        }
        private decimal Calcula_total_RA_liquido_BD(Tpedido tpedido)
        {
            /*


' ___________________________________________________________________________
' CALCULA TOTAL RA LIQUIDO BD
function calcula_total_RA_liquido_BD(byval id_pedido, byref vl_total_RA_liquido, byref msg_erro)
dim s
dim rs
dim rspb
dim id_pedido_base
dim vl_total_RA
dim percentual_desagio_RA_liquido

	calcula_total_RA_liquido_BD = False
	
	id_pedido = Trim("" & id_pedido)
	vl_total_RA_liquido = 0
	msg_erro = ""
	
	id_pedido_base = retorna_num_pedido_base(id_pedido)

	s = "SELECT " & _
			"*" & _
		" FROM t_PEDIDO" & _
		" WHERE" & _
			" (pedido='" & id_pedido_base & "')"
	set rspb = cn.Execute(s)
	if Err <> 0 then
		msg_erro = Cstr(Err) & ": " & Err.Description
		exit function
		end if

	if rspb.Eof then
		msg_erro = "Pedido-base " & id_pedido_base & " não foi encontrado."
		exit function
		end if

	percentual_desagio_RA_liquido = rspb("perc_desagio_RA_liquida")
	
'	OBTÉM OS VALORES TOTAIS DE NF, RA E VENDA
	vl_total_RA = 0
	s = "SELECT" & _
			" SUM(qtde*(preco_NF-preco_venda)) AS total_RA" & _
		" FROM t_PEDIDO_ITEM" & _
			" INNER JOIN t_PEDIDO" & _
				" ON (t_PEDIDO_ITEM.pedido=t_PEDIDO.pedido)" & _
		" WHERE" & _
			" (st_entrega<>'" & ST_ENTREGA_CANCELADO & "')" & _
			" AND (t_PEDIDO.pedido LIKE '" & id_pedido_base & BD_CURINGA_TODOS & "')"
	set rs = cn.Execute(s)
	if Err <> 0 then
		msg_erro = Cstr(Err) & ": " & Err.Description
		exit function
		end if

	if Not rs.Eof then
		if Not IsNull(rs("total_RA")) then vl_total_RA = rs("total_RA")
		end if
	
	vl_total_RA_liquido = CCur(vl_total_RA - (percentual_desagio_RA_liquido/100)*vl_total_RA)
	vl_total_RA_liquido = converte_numero(formata_moeda(vl_total_RA_liquido))
	
	calcula_total_RA_liquido_BD = True
end function

	*/

            var vl_total_RA = Pedido.Valor.Vl_total_RA;
            //o campo perc_desagio_RA_liquida já deve ter sido determinado
            var percentual_desagio_RA_liquido = tpedido.Perc_Desagio_RA_Liquida;
            var vl_total_RA_liquido = (vl_total_RA - (Convert.ToDecimal(percentual_desagio_RA_liquido / 100)) * vl_total_RA);
            return vl_total_RA_liquido;

        }

        private async Task RegistrarSenhasDesconto(int indice_pedido)
        {
            // SENHAS DE AUTORIZAÇÃO PARA DESCONTO SUPERIOR
            //	Caso tenha usado algum desconto superior ao limite, liberado pela t_DESCONTO, marca como usado(Passo70/Senhas_de_autorizacao_para_desconto_superior.feature)

            if (indice_pedido != 1)
                return;

            //'		SENHAS DE AUTORIZAÇÃO PARA DESCONTO SUPERIOR
            var gravacao_pendente = false;
            foreach (var v_desconto_k in Gravacao.V_desconto)
            {
                /*
					s = "SELECT * FROM t_DESCONTO" & _
						" WHERE (usado_status=0)" & _
						" AND (cancelado_status=0)" & _
						" AND (id='" & Trim(v_desconto(k)) & "')"
						*/
                var descontos = await (from d in ContextoBdGravacao.Tdesconto
                                       where d.Usado_status == 0
                                                        && d.Cancelado_status == 0
                                                        && d.Id == v_desconto_k
                                       select d).ToListAsync();

                if (descontos.Count() != 1)
                {
                    Retorno.ListaErros.Add($"Senha de autorização para desconto superior não encontrado (contagem diferente de 1).");
                    break;
                }
                var desconto = descontos.First();
                desconto.Usado_status = 1;
                desconto.Usado_data = DateTime.Now;
                if ((Pedido.Ambiente.Operacao_origem == Constantes.Op_origem__pedido_novo.OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO) && Execucao.BlnMagentoPedidoComIndicador)
                    desconto.Vendedor = Pedido.Ambiente.Vendedor;
                else
                    desconto.Vendedor = Pedido.Ambiente.Usuario;
                desconto.Usado_usuario = Pedido.Ambiente.Usuario;
                ContextoBdGravacao.Update(desconto);
                gravacao_pendente = true;
            }

            if (gravacao_pendente)
                await ContextoBdGravacao.SaveChangesAsync();
        }
    }
}
