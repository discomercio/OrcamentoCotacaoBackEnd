using InfraBanco;
using InfraBanco.Constantes;
using Microsoft.EntityFrameworkCore;
using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava15
{
    partial class Grava15 : PassoBaseGravacao
    {
        public async Task PedidoMagentoRepetido()
        {
            /*
            #2) Seria necessário tratar a possibilidade de ocorrer acesso concorrente entre o cadastramento semi-automático e a integração.
            #Em ambos os casos, seria importante verificar no instante final antes da efetivar o cadastramento do pedido se o número Magento e,
            #caso exista, o número do pedido marketplace já estão cadastrados em algum pedido c/ st_entrega válido (diferente de cancelado).

            */
            //fazer com 2 campos separadamente; pedido_bs_x_marketplace e pedido_bs_x_ac
            await PedidoMagentoRepetidoCampo(true);
            await PedidoMagentoRepetidoCampo(false);
        }

        public async Task PedidoMagentoRepetidoCampo(bool campo_pedido_bs_x_marketplace)
        {

            /*
             * 
            #Isto está feito no ASP abaixo:
            #loja/PedidoNovoConfirma.asp
            #ATENÇÃO: fazer com 2 campos separadamente; pedido_bs_x_marketplace e pedido_bs_x_ac
            #se loja = NUMERO_LOJA_ECOMMERCE_AR_CLUBE --- mas fazemos para qualquer loja!

            #'	VERIFICA SE HÁ PEDIDO JÁ CADASTRADO COM O MESMO Nº PEDIDO MAGENTO (POSSÍVEL CADASTRO EM DUPLICIDADE)
            #	if alerta = "" then
            #		if s_pedido_ac <> "" then
            #			s = "SELECT" & _
            ......
            #				" FROM t_PEDIDO tP" & _
            #				" WHERE" & _
            #					" (tP.st_entrega <> '" & ST_ENTREGA_CANCELADO & "')" & _
            #					" AND (pedido_bs_x_ac = '" & s_pedido_ac & "')" & _

            */

            IQueryable<string>? existentesQuery = null;
            if (campo_pedido_bs_x_marketplace && !string.IsNullOrWhiteSpace(Pedido.Marketplace.Pedido_bs_x_marketplace))
            {
                existentesQuery = from p in ContextoBdGravacao.Tpedidos
                                  where p.Pedido_Bs_X_Marketplace == Pedido.Marketplace.Pedido_bs_x_marketplace
                                  && p.St_Entrega != Constantes.ST_ENTREGA_CANCELADO
                                  select p.Pedido;
            }
            if (!campo_pedido_bs_x_marketplace && !string.IsNullOrWhiteSpace(Pedido.Marketplace.Pedido_bs_x_ac))
            {
                existentesQuery = from p in ContextoBdGravacao.Tpedidos
                                  where p.Pedido_Bs_X_Ac == Pedido.Marketplace.Pedido_bs_x_ac
                                  && p.St_Entrega != Constantes.ST_ENTREGA_CANCELADO
                                  select p.Pedido;
            }
            if (existentesQuery == null)
                return;
            //transformamos em lista porque tem, no máximo, 1
            var existentesLista = existentesQuery.ToList();
            if (!existentesLista.Any())
                return;

            var pedidoLista = (from p in existentesLista select p).ToList();


            /*
            #					" AND (" & _
            #						"tP.pedido NOT IN (" & _
            #							"SELECT DISTINCT" & _
            #								" pedido" & _
            #							" FROM t_PEDIDO_DEVOLUCAO tPD" & _
            #							" WHERE" & _
            #								" (tPD.pedido = tP.pedido)" & _
            #								" AND (tPD.status IN (" & _
            #									COD_ST_PEDIDO_DEVOLUCAO__FINALIZADA & "," & _
            #									COD_ST_PEDIDO_DEVOLUCAO__MERCADORIA_RECEBIDA & "," & _
            #									COD_ST_PEDIDO_DEVOLUCAO__EM_ANDAMENTO & "," & _
            #									COD_ST_PEDIDO_DEVOLUCAO__CADASTRADA & _
            #									")" & _
            #								")" & _
            #							")" & _
            #						")" & _
            */
            var statusPermitidos = new List<byte>
            {
                byte.Parse(Constantes.COD_ST_PEDIDO_DEVOLUCAO__FINALIZADA),
                byte.Parse(Constantes.COD_ST_PEDIDO_DEVOLUCAO__MERCADORIA_RECEBIDA),
                byte.Parse(Constantes.COD_ST_PEDIDO_DEVOLUCAO__EM_ANDAMENTO),
                byte.Parse(Constantes.COD_ST_PEDIDO_DEVOLUCAO__CADASTRADA)
            };
            var t_pedido_devolucao_task = (from tpd in ContextoBdGravacao.TpedidoDevolucaos
                                           where pedidoLista.Contains(tpd.Pedido)
                                           && statusPermitidos.Contains(tpd.Status)
                                           select tpd.Pedido).ToListAsync();
            /*
            #					" AND (" & _
            #						"tP.pedido NOT IN (" & _
            #							"SELECT DISTINCT" & _
            #								" pedido" & _
            #							" FROM t_PEDIDO_ITEM_DEVOLVIDO tPID" & _
            #							" WHERE" & _
            #								" (tPID.pedido = tP.pedido)" & _
            #							")" & _
            #						")"
            #			set rs = cn.execute(s)
            */
            var t_pedido_item_devolvido_task = (from tpid in ContextoBdGravacao.TpedidoItemDevolvidos where pedidoLista.Contains(tpid.Pedido) select tpid.Pedido).ToListAsync();


            //acessa o banco
            var t_pedido_devolucao = await t_pedido_devolucao_task;
            var t_pedido_item_devolvido = await t_pedido_item_devolvido_task;
            //agora verificamos se sobra algum
            var sobrandoPedido = (from p in existentesLista
                                  where !t_pedido_devolucao.Contains(p) && !t_pedido_item_devolvido.Contains(p)
                                  select p)
                            .FirstOrDefault();
            if (sobrandoPedido == null)
                return;
            var sobrando = (from p in ContextoBdGravacao.Tpedidos
                            where p.Pedido == sobrandoPedido
                            select new
                            {
                                p.Pedido,
                                p.Pedido_Bs_X_Ac,
                                p.Pedido_Bs_X_Marketplace,
                                p.Vendedor,
                                p.Data_Hora,
                                p.Endereco_cnpj_cpf,
                                p.Endereco_nome
                            }).First();

            /*
        #			if Not rs.Eof then
        #				alerta=texto_add_br(alerta)
        #				alerta=alerta & "O nº pedido Magento " & Trim("" & rs("pedido_bs_x_ac")) & " já está cadastrado no pedido " & Trim("" & rs("pedido"))
        #				alerta=texto_add_br(alerta)
        #				alerta=alerta & "Data de cadastramento do pedido: " & formata_data_hora_sem_seg(rs("data_hora"))
        #				alerta=texto_add_br(alerta)
        #				alerta=alerta & "Cadastrado por: " & Trim("" & rs("vendedor"))
        #				if Ucase(Trim("" & rs("vendedor"))) <> Ucase(Trim("" & rs("nome_vendedor"))) then alerta=alerta & " (" & Trim("" & rs("nome_vendedor")) & ")"
        #				alerta=texto_add_br(alerta)
        #				alerta=alerta & "Cliente: " & cnpj_cpf_formata(Trim("" & rs("cnpj_cpf"))) & " - " & Trim("" & rs("nome_cliente"))
        #				end if 'if Not rs.Eof
        #			end if 'if s_pedido_ac <> ""
        #		end if 'if alerta = ""
        */
            var sobrando_nome_vendedor = (from u in ContextoBdGravacao.Tusuarios where u.Usuario == (sobrando.Vendedor ?? "") select u.Nome).FirstOrDefault();

            var nomeCampo = "Pedido_Bs_X_Ac " + (sobrando.Pedido_Bs_X_Ac ?? "");
            if (campo_pedido_bs_x_marketplace)
                nomeCampo = "Pedido_Bs_X_Marketplace " + (sobrando.Pedido_Bs_X_Marketplace ?? "");

            var msg = "";
            msg += "O nº pedido Magento " + nomeCampo + " já está cadastrado no pedido " + sobrando.Pedido;
            msg += UtilsGlobais.Util.EnterParaMensagemErro();
            msg += "Data de cadastramento do pedido: " + (sobrando.Data_Hora?.ToShortDateString() ?? "Sem data") + " " + (sobrando.Data_Hora?.ToShortTimeString());
            msg += UtilsGlobais.Util.EnterParaMensagemErro();
            msg += "Cadastrado por: " + (sobrando.Vendedor ?? "");
            if ((sobrando.Vendedor ?? "").ToUpper().Trim() != (sobrando_nome_vendedor ?? "").ToUpper().Trim())
                msg += " (" + (sobrando_nome_vendedor ?? "").ToUpper().Trim() + ")";
            msg += UtilsGlobais.Util.EnterParaMensagemErro();
            msg += "Cliente: " + UtilsGlobais.Util.FormatCpf_Cnpj_Ie(sobrando.Endereco_cnpj_cpf) + " - " + (sobrando.Endereco_nome ?? "").Trim();
            Retorno.ListaErros.Add(msg);
        }
    }
}
