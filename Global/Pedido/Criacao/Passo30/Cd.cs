using InfraBanco.Constantes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo30
{
    partial class Passo30
    {
        private async Task Cd()
        {
			//Scenario: Validar permissão de CD
			if (!Criacao.Execucao.UsuarioPermissao.Operacao_permitida(Constantes.OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD)
				&& Pedido.Ambiente.Id_nfe_emitente_selecao_manual != 0)
			{
				Retorno.ListaErros.Add("Usuário não tem permissão de especificar o CD");
			}

			if (Pedido.Ambiente.Id_nfe_emitente_selecao_manual == 0)
				return;


			//Scenario: Validar CD escolhido caso manual
			//#Valores do c_id_nfe_emitente_selecao_manual:
			//#strSql = "SELECT" & _
			//#			" id," & _
			//#			" apelido," & _
			//#			" razao_social" & _
			//#		" FROM t_NFe_EMITENTE" & _
			//#		" WHERE" & _
			//#			" (st_ativo <> 0)" & _
			//#			" AND (st_habilitado_ctrl_estoque <> 0)"

			var cdPermitido = await (from cd in Criacao.ContextoProvider.GetContextoLeitura().TnfEmitente
							   where cd.Id == Pedido.Ambiente.Id_nfe_emitente_selecao_manual
							   && cd.St_Ativo != 0
							   && cd.St_Habilitado_Ctrl_Estoque != 0
							   select cd.Id).AnyAsync();
			if(!cdPermitido)
				Retorno.ListaErros.Add($"O CD selecionado manualmente é inválido (Id_nfe_emitente_selecao_manual: {Pedido.Ambiente.Id_nfe_emitente_selecao_manual})");
		}
	}
}
