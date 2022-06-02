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
                var valorBd = await (from l in Criacao.ContextoProvider.GetContextoLeitura().Tcontrole
                                     where l.Id_Nsu == Constantes.ID_PARAM_CAD_VL_APROV_AUTO_ANALISE_CREDITO
                                     select l.Nsu).ToListAsync();
                if (valorBd.Any())
                {
                    CultureInfo FormatarEmPortugues = new CultureInfo("pt-BR");
                    Criacao.Execucao.Vl_aprov_auto_analise_credito = decimal.Parse(valorBd[0], FormatarEmPortugues);
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
                var valorBd = await (from l in Criacao.ContextoProvider.GetContextoLeitura().Tloja
                                     where l.Loja == Pedido.Ambiente.Loja_indicou
                                     select l.Comissao_indicacao).ToListAsync();
                if (valorBd.Any())
                {
                    var valor = valorBd[0];
                    Criacao.Execucao.Comissao_loja_indicou = valor;
                }
                else
                {
                    Retorno.ListaErros.Add($"Loja {Pedido.Ambiente.Loja_indicou} não está cadastrada.");
                }
            }
        }
    }
}
