using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava25
{
    class Grava25 : PassoBaseGravacao
    {
        public Grava25(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao)
        {
        }

        public async Task Executar()
        {

            //Passo25:  
            //	VERIFICA SE AS REGRAS ASSOCIADAS AOS PRODUTOS ESTÃO OK - linha 1010
            //		Verifica se todos os produtos possuem regra ativa e não bloqueada e ao menos um CD ativo.
            //	'NO CASO DE SELEÇÃO MANUAL DO CD, VERIFICA SE O CD SELECIONADO ESTÁ HABILITADO EM TODAS AS REGRAS - linha 1047
            //		No caso de CD manual, verifica se o CD tem regra ativa


            //PedidoNovoConfirma.asp linha mais ou menos 1073
            //'VERIFICA SE AS REGRAS ASSOCIADAS AOS PRODUTOS ESTÃO OK
            VerificaRegrasAssociadasAosProdutos();
            CdSelecionadoEstaHabilitadoEmTodasAsRegras();

            MarcarErroSeNaoExsitir();

        }

        public void VerificaRegrasAssociadasAosProdutos()
        {
            /*
				if alerta="" then
					'VERIFICA SE AS REGRAS ASSOCIADAS AOS PRODUTOS ESTÃO OK
					for iRegra=LBound(vProdRegra) to UBound(vProdRegra)
						if Trim(vProdRegra(iRegra).produto) <> "" then
							if converte_numero(vProdRegra(iRegra).regra.id) = 0 then
								alerta=texto_add_br(alerta)
								alerta=alerta & "Produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " não possui regra de consumo do estoque associada"
							elseif vProdRegra(iRegra).regra.st_inativo = 1 then
								alerta=texto_add_br(alerta)
								alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " está desativada"
							elseif vProdRegra(iRegra).regra.regraUF.st_inativo = 1 then
								alerta=texto_add_br(alerta)
								alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " está bloqueada para a UF '" & EndCob_uf & "'"
							elseif vProdRegra(iRegra).regra.regraUF.regraPessoa.st_inativo = 1 then
								alerta=texto_add_br(alerta)
								alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " está bloqueada para clientes '" & descricao_tipo_pessoa & "' da UF '" & EndCob_uf & "'"
							elseif converte_numero(vProdRegra(iRegra).regra.regraUF.regraPessoa.spe_id_nfe_emitente) = 0 then
								alerta=texto_add_br(alerta)
								alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " não especifica nenhum CD para aguardar produtos sem presença no estoque para clientes '" & descricao_tipo_pessoa & "' da UF '" & EndCob_uf & "'"
							else
								qtde_CD_ativo = 0
								for iCD=LBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD) to UBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD)
									if converte_numero(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente) > 0 then
										if vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).st_inativo = 0 then
											qtde_CD_ativo = qtde_CD_ativo + 1
											end if
										end if
									next
								'A SELEÇÃO MANUAL DE CD PERMITE O USO DE CD DESATIVADO
								if (qtde_CD_ativo = 0) And (id_nfe_emitente_selecao_manual = 0) then
									alerta=texto_add_br(alerta)
									alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " não especifica nenhum CD ativo para clientes '" & descricao_tipo_pessoa & "' da UF '" & EndCob_uf & "'"
									end if
								end if
							end if
						next
					end if 'if alerta=""
*/

            //todo: fazer ewsta nritna no grava25
            // usar ProdutoGeralBll.VerificarRegrasAssociadasAosProdutos

        }
        public void CdSelecionadoEstaHabilitadoEmTodasAsRegras()
        {
            //todo: fazer ewsta nritna no grava25

            /*
                            'NO CASO DE SELEÇÃO MANUAL DO CD, VERIFICA SE O CD SELECIONADO ESTÁ HABILITADO EM TODAS AS REGRAS
                                if alerta="" then
                                if id_nfe_emitente_selecao_manual <> 0 then
                                    alerta_aux = ""
                                    for iRegra=LBound(vProdRegra) to UBound(vProdRegra)
                                        blnAchou = False
                                        blnDesativado = False
                                        if Trim(vProdRegra(iRegra).produto) <> "" then
                                            for iCD=LBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD) to UBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD)
                                                if vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente = id_nfe_emitente_selecao_manual then
                                                    blnAchou = True
                                                    if vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).st_inativo = 1 then blnDesativado = True
                                                    exit for
                                                    end if
                                                next
                                            end if

                                        if Not blnAchou then
                                            alerta_aux=texto_add_br(alerta_aux)
                                            alerta_aux=alerta_aux & "Produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & ": regra '" & vProdRegra(iRegra).regra.apelido & "' (Id=" & vProdRegra(iRegra).regra.id & ") não permite o CD '" & obtem_apelido_empresa_NFe_emitente(id_nfe_emitente_selecao_manual) & "'"
                                        elseif blnDesativado then
                                            '16/09/2017: FOI REALIZADA UMA ALTERAÇÃO P/ QUE A SELEÇÃO MANUAL DE CD PERMITA O USO DE CD DESATIVADO
                                            'alerta_aux=texto_add_br(alerta_aux)
                                            'alerta_aux=alerta_aux & "Produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & ": regra '" & vProdRegra(iRegra).regra.apelido & "' (Id=" & vProdRegra(iRegra).regra.id & ") define o CD '" & obtem_apelido_empresa_NFe_emitente(id_nfe_emitente_selecao_manual) & "' como 'desativado'"
                                            end if
                                        next

                                    if alerta_aux <> "" then
                                        alerta=texto_add_br(alerta)
                                        alerta=alerta & "O CD selecionado manualmente não pode ser usado devido aos seguintes motivos:"
                                        alerta=texto_add_br(alerta)
                                        alerta=alerta & alerta_aux
                                        end if
                                    end if
                                end if
                            */

        }

        private void MarcarErroSeNaoExsitir()
        {
            //se ja tem algum erro, nao precisa mais
            if (Retorno.ListaErros.Count != 0)
                return;

            //garante que tenha rgistrado o erro
            foreach (var regra in Execucao.Gravacao.ListaRegrasControleEstoque)
            {
                if (!regra.St_Regra_ok)
                    Retorno.ListaErros.Add("Ocorreu algum erro na leitura das regras de consumo de estoque.");
            }

        }
    }
}
