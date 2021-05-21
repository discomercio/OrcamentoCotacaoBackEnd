using InfraBanco;
using Pedido.Dados.Criacao;
using System.Linq;
using System.Collections.Generic;
using static Pedido.Criacao.Execucao.Gravacao;

namespace Pedido.Criacao.Passo60.Gravacao.Grava55
{
    class Grava55 : PassoBaseGravacao
    {
        public Grava55(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao, Execucao.Gravacao gravacao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao, gravacao)
        {
        }

        public void Executar()
        {
            Gravacao.EmpresasAutoSplit = CalcularEmpresaAutoSplitDados();
            if (!Gravacao.EmpresasAutoSplit.Any())
                Retorno.ListaErros.Add("Erro: nenhuma empresa selecionada para consumir o estoque.");
        }
        private List<EmpresaAutoSplitDados> CalcularEmpresaAutoSplitDados()
        {

            //Passo55: Contagem de pedidos a serem gravados -Linha 1286
            //	'	CONTAGEM DE EMPRESAS QUE SERÃO USADAS NO AUTO-SPLIT, OU SEJA, A QUANTIDADE DE PEDIDOS QUE SERÁ CADASTRADA, 
            //	JÁ QUE CADA PEDIDO SE REFERE AO ESTOQUE DE UMA EMPRESA

            //	Conta todos os CDs que tem alguma quantidade solicitada.

            //		'	CONTAGEM DE EMPRESAS QUE SERÃO USADAS NO AUTO-SPLIT, OU SEJA, A QUANTIDADE DE PEDIDOS QUE SERÁ CADASTRADA, JÁ QUE CADA PEDIDO SE REFERE AO ESTOQUE DE UMA EMPRESA
            //dim vEmpresaAutoSplit
            //redim vEmpresaAutoSplit(0)
            //vEmpresaAutoSplit(UBound(vEmpresaAutoSplit)) = 0

            //dim qtde_empresa_selecionada, lista_empresa_selecionada
            //qtde_empresa_selecionada = 0
            //lista_empresa_selecionada = ""
            //if alerta = "" then
            //	for iRegra = LBound(vProdRegra) to UBound(vProdRegra)
            //		if Trim(vProdRegra(iRegra).produto) <> "" then
            //			for iCD = LBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD) to UBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD)
            //				if (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente > 0) And _
            //					((id_nfe_emitente_selecao_manual = 0) Or(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente = id_nfe_emitente_selecao_manual) ) then
            //					if vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada > 0 then
            //						s = "|" & vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente & "|"
            //						if Instr(lista_empresa_selecionada, s) = 0 then
            //						'	SE O CD AINDA NÃO CONSTA DA LISTA, INCLUI
            //							qtde_empresa_selecionada = qtde_empresa_selecionada + 1
            //							lista_empresa_selecionada = lista_empresa_selecionada & s
            //							if vEmpresaAutoSplit(UBound(vEmpresaAutoSplit)) <> 0 then
            //								redim preserve vEmpresaAutoSplit(UBound(vEmpresaAutoSplit) +1)
            //								vEmpresaAutoSplit(UBound(vEmpresaAutoSplit)) = 0
            //								end if
            //							vEmpresaAutoSplit(UBound(vEmpresaAutoSplit)) = vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente
            //							end if
            //						end if
            //					end if
            //				next
            //			end if
            //		next
            //	end if 'if alerta=""

            //'	CONTAGEM DE EMPRESAS QUE SERÃO USADAS NO AUTO-SPLIT, OU SEJA, A QUANTIDADE DE PEDIDOS QUE SERÁ CADASTRADA, JÁ QUE CADA PEDIDO SE REFERE AO ESTOQUE DE UMA EMPRESA
            List<Execucao.Gravacao.EmpresaAutoSplitDados> vEmpresaAutoSplit = new List<Execucao.Gravacao.EmpresaAutoSplitDados>();
            List<int> lista_empresa_selecionada = new List<int>();
            foreach (var vProdRegra_iRegra in Gravacao.ListaRegrasControleEstoque)
            {
                //vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD) é twmsCdXUfXPessoaXCd
                foreach (var twmsCdXUfXPessoaXCd in vProdRegra_iRegra.TwmsCdXUfXPessoaXCd)
                {
                    if ((twmsCdXUfXPessoaXCd.Id_nfe_emitente > 0) &&
                        ((Pedido.Ambiente.Id_nfe_emitente_selecao_manual == 0) || (twmsCdXUfXPessoaXCd.Id_nfe_emitente == Pedido.Ambiente.Id_nfe_emitente_selecao_manual)))
                    {
                        if (twmsCdXUfXPessoaXCd.Estoque_Qtde_Solicitado > 0)
                        {
                            if (!lista_empresa_selecionada.Contains(twmsCdXUfXPessoaXCd.Id_nfe_emitente))
                            {
                                //'	SE O CD AINDA NÃO CONSTA DA LISTA, INCLUI
                                lista_empresa_selecionada.Add(twmsCdXUfXPessoaXCd.Id_nfe_emitente);
                                vEmpresaAutoSplit.Add(new Execucao.Gravacao.EmpresaAutoSplitDados(twmsCdXUfXPessoaXCd.Id_nfe_emitente));
                            }
                        }
                    }
                }
            }
            return vEmpresaAutoSplit;
        }
    }
}
