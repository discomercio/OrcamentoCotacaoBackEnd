using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava50
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0054:Use compound assignment", Justification = "Estilo de código")]
    class Grava50 : PassoBaseGravacao
    {
        public Grava50(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao, Execucao.Gravacao gravacao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao, gravacao)
        {
        }

        public void Executar()
        {


            //Passo50: ANALISA A QUANTIDADE DE PEDIDOS QUE SERÃO CADASTRADOS(AUTO-SPLIT) -linha 1184
            //			'	OS CD'S ESTÃO ORDENADOS DE ACORDO C/ A PRIORIZAÇÃO DEFINIDA PELA REGRA DE CONSUMO DO ESTOQUE
            //			'	SE O PRIMEIRO CD HABILITADO NÃO PUDER ATENDER INTEGRALMENTE A QUANTIDADE SOLICITADA DO PRODUTO,
            //			'	A QUANTIDADE RESTANTE SERÁ CONSUMIDA DOS DEMAIS CD'S.
            //			'	SE HOUVER ALGUMA QUANTIDADE RESIDUAL P/ FICAR NA LISTA DE PRODUTOS SEM PRESENÇA NO ESTOQUE:
            //			'		1) SELEÇÃO AUTOMÁTICA DE CD: A QUANTIDADE PENDENTE FICARÁ ALOCADA NO CD DEFINIDO P/ TAL
            //			'		2) SELEÇÃO MANUAL DE CD: A QUANTIDADE PENDENTE FICARÁ ALOCADA NO CD SELECIONADO MANUALMENTE

            //	Para cada produto:
            //			Aloca a quantidade solicitada nos CDs ordenados até alocar todos.
            //		Se não conseguir alocar todos, marca a quantidade residual no CD manual ou no CD de t_WMS_REGRA_CD_X_UF_X_PESSOA.spe_id_nfe_emitente


            ZerarQtde_solicitada();
            AlocarProdutos();
        }
        private void ZerarQtde_solicitada()
        {

            /*
            '	ANALISA A QUANTIDADE DE PEDIDOS QUE SERÃO CADASTRADOS (AUTO-SPLIT)
            '	INICIALIZA O CAMPO 'qtde_solicitada', POIS ELE IRÁ CONTROLAR A QUANTIDADE A SER ALOCADA NO ESTOQUE DE CADA EMPRESA
                if alerta = "" then
                    for iRegra=LBound(vProdRegra) to UBound(vProdRegra)
                        for iCD=LBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD) to UBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD)
                            vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada = 0
                            next
                        next
                    end if 'if alerta=""
                */

            foreach (var regra in Gravacao.ListaRegrasControleEstoque)
            {
                foreach (var vCD in regra.TwmsCdXUfXPessoaXCd)
                    vCD.Estoque_Qtde_Solicitado = 0;
            }
        }

        private void AlocarProdutos()
        {


            /*
        '	REALIZA A ANÁLISE DA QUANTIDADE DE PEDIDOS NECESSÁRIA (AUTO-SPLIT)
            dim qtde_a_alocar
            if alerta = "" then
                for iItem=Lbound(v_item) to Ubound(v_item)
                    if Trim(v_item(iItem).produto) <> "" then
                    '	OS CD'S ESTÃO ORDENADOS DE ACORDO C/ A PRIORIZAÇÃO DEFINIDA PELA REGRA DE CONSUMO DO ESTOQUE
                    '	SE O PRIMEIRO CD HABILITADO NÃO PUDER ATENDER INTEGRALMENTE A QUANTIDADE SOLICITADA DO PRODUTO,
                    '	A QUANTIDADE RESTANTE SERÁ CONSUMIDA DOS DEMAIS CD'S.
                    '	SE HOUVER ALGUMA QUANTIDADE RESIDUAL P/ FICAR NA LISTA DE PRODUTOS SEM PRESENÇA NO ESTOQUE:
                    '		1) SELEÇÃO AUTOMÁTICA DE CD: A QUANTIDADE PENDENTE FICARÁ ALOCADA NO CD DEFINIDO P/ TAL
                    '		2) SELEÇÃO MANUAL DE CD: A QUANTIDADE PENDENTE FICARÁ ALOCADA NO CD SELECIONADO MANUALMENTE
                        qtde_a_alocar = v_item(iItem).qtde
                        for iRegra=LBound(vProdRegra) to UBound(vProdRegra)
                            if qtde_a_alocar = 0 then exit for

                            if Trim(vProdRegra(iRegra).produto) <> "" then
                                for iCD=LBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD) to UBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD)
                                    if qtde_a_alocar = 0 then exit for

                                    if (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente > 0) And _
                                        ( (id_nfe_emitente_selecao_manual = 0) Or (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente = id_nfe_emitente_selecao_manual) ) then
                                        'VERIFICA SE O CD ESTÁ HABILITADO
                                        'IMPORTANTE: A SELEÇÃO MANUAL DE CD PERMITE O USO DE CD DESATIVADO
                                        if (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).st_inativo = 0) Or (id_nfe_emitente_selecao_manual <> 0) then
                                            if (vProdRegra(iRegra).fabricante = v_item(iItem).fabricante) And (vProdRegra(iRegra).produto = v_item(iItem).produto) then
                                                if vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_estoque >= qtde_a_alocar then
                                                '	HÁ QUANTIDADE DISPONÍVEL SUFICIENTE PARA INTEGRALMENTE
                                                    vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada = qtde_a_alocar
                                                    qtde_a_alocar = 0
                                                elseif vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_estoque > 0 then
                                                '	A QUANTIDADE DISPONÍVEL NO ESTOQUE É INSUFICIENTE P/ ATENDER INTEGRALMENTE À QUANTIDADE SOLICITADA,
                                                '	PORTANTO, A QUANTIDADE DISPONÍVEL NESTE CD SERÁ CONSUMIDA P/ ATENDER PARCIALMENTE À REQUISIÇÃO E A
                                                '	QUANTIDADE REMANESCENTE SERÁ ATENDIDA PELO PRÓXIMO CD DA LISTA OU ENTÃO SERÁ COLOCADA NA LISTA DE
                                                '	PRODUTOS SEM PRESENÇA NO ESTOQUE DO CD SELECIONADO P/ TAL NA REGRA.
                                                    vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada = vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_estoque
                                                    qtde_a_alocar = qtde_a_alocar - vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_estoque
                                                    end if
                                                end if
                                            end if
                                        end if
                                    next
                                end if
                            next
    */

            foreach (var v_item_iItem in Pedido.ListaProdutos)
            {
                int qtde_a_alocar = v_item_iItem.Qtde;
                foreach (var vProdRegra_iRegra in Gravacao.ListaRegrasControleEstoque)
                {
                    if (qtde_a_alocar == 0)
                        break;

                    //vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD) é twmsCdXUfXPessoaXCd
                    foreach (var twmsCdXUfXPessoaXCd in vProdRegra_iRegra.TwmsCdXUfXPessoaXCd)
                    {
                        if (qtde_a_alocar == 0)
                            break;

                        if ((twmsCdXUfXPessoaXCd.Id_nfe_emitente > 0) &&
                            ((Pedido.Ambiente.Id_nfe_emitente_selecao_manual == 0) || (twmsCdXUfXPessoaXCd.Id_nfe_emitente == Pedido.Ambiente.Id_nfe_emitente_selecao_manual)))
                        {
                            //'VERIFICA SE O CD ESTÁ HABILITADO
                            //'IMPORTANTE: A SELEÇÃO MANUAL DE CD PERMITE O USO DE CD DESATIVADO
                            if ((twmsCdXUfXPessoaXCd.St_inativo == 0) || (Pedido.Ambiente.Id_nfe_emitente_selecao_manual != 0))
                            {
                                if ((vProdRegra_iRegra.Fabricante == v_item_iItem.Fabricante) && (vProdRegra_iRegra.Produto == v_item_iItem.Produto))
                                {
                                    if (twmsCdXUfXPessoaXCd.Estoque_Qtde_Estoque >= qtde_a_alocar)
                                    {
                                        //'	HÁ QUANTIDADE DISPONÍVEL SUFICIENTE PARA INTEGRALMENTE
                                        twmsCdXUfXPessoaXCd.Estoque_Qtde_Solicitado = (short)qtde_a_alocar;
                                        qtde_a_alocar = 0;
                                    }
                                    else
                                    {
                                        if (twmsCdXUfXPessoaXCd.Estoque_Qtde_Estoque != null && twmsCdXUfXPessoaXCd.Estoque_Qtde_Estoque > 0)
                                        {
                                            //'	A QUANTIDADE DISPONÍVEL NO ESTOQUE É INSUFICIENTE P/ ATENDER INTEGRALMENTE À QUANTIDADE SOLICITADA,
                                            //'	PORTANTO, A QUANTIDADE DISPONÍVEL NESTE CD SERÁ CONSUMIDA P/ ATENDER PARCIALMENTE À REQUISIÇÃO E A
                                            //'	QUANTIDADE REMANESCENTE SERÁ ATENDIDA PELO PRÓXIMO CD DA LISTA OU ENTÃO SERÁ COLOCADA NA LISTA DE
                                            //'	PRODUTOS SEM PRESENÇA NO ESTOQUE DO CD SELECIONADO P/ TAL NA REGRA.
                                            twmsCdXUfXPessoaXCd.Estoque_Qtde_Solicitado = twmsCdXUfXPessoaXCd.Estoque_Qtde_Estoque;
                                            qtde_a_alocar = qtde_a_alocar - twmsCdXUfXPessoaXCd.Estoque_Qtde_Estoque.Value;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                /*
                                '	RESTOU SALDO A ALOCAR NA LISTA DE PRODUTOS SEM PRESENÇA NO ESTOQUE?
                                    if qtde_a_alocar > 0 then
                                    '	LOCALIZA E ALOCA A QUANTIDADE PENDENTE:
                                    '		1) SELEÇÃO AUTOMÁTICA DE CD: A QUANTIDADE PENDENTE FICARÁ ALOCADA NO CD DEFINIDO P/ TAL
                                    '		2) SELEÇÃO MANUAL DE CD: A QUANTIDADE PENDENTE FICARÁ ALOCADA NO CD SELECIONADO MANUALMENTE
                                        for iRegra=LBound(vProdRegra) to UBound(vProdRegra)
                                            if qtde_a_alocar = 0 then exit for

                                            if Trim(vProdRegra(iRegra).produto) <> "" then
                                                for iCD=LBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD) to UBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD)
                                                    if qtde_a_alocar = 0 then exit for

                                                    if id_nfe_emitente_selecao_manual = 0 then
                                                        'MODO DE SELEÇÃO AUTOMÁTICO
                                                        if ( (vProdRegra(iRegra).fabricante = v_item(iItem).fabricante) And (vProdRegra(iRegra).produto = v_item(iItem).produto) ) And _
                                                            (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente > 0) And _
                                                            (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente = vProdRegra(iRegra).regra.regraUF.regraPessoa.spe_id_nfe_emitente) then
                                                            vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada = vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada + qtde_a_alocar
                                                            qtde_a_alocar = 0
                                                            exit for
                                                            end if
                                                    else
                                                        'MODO DE SELEÇÃO MANUAL
                                                        if ( (vProdRegra(iRegra).fabricante = v_item(iItem).fabricante) And (vProdRegra(iRegra).produto = v_item(iItem).produto) ) And _
                                                            (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente > 0) And _
                                                            (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente = id_nfe_emitente_selecao_manual) then
                                                            vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada = vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada + qtde_a_alocar
                                                            qtde_a_alocar = 0
                                                            exit for
                                                            end if
                                                        end if
                                                    next
                                                end if
                                            next
                                        end if
*/
                //'	RESTOU SALDO A ALOCAR NA LISTA DE PRODUTOS SEM PRESENÇA NO ESTOQUE?
                if (qtde_a_alocar > 0)
                {
                    //'	LOCALIZA E ALOCA A QUANTIDADE PENDENTE:
                    //'		1) SELEÇÃO AUTOMÁTICA DE CD: A QUANTIDADE PENDENTE FICARÁ ALOCADA NO CD DEFINIDO P/ TAL
                    //'		2) SELEÇÃO MANUAL DE CD: A QUANTIDADE PENDENTE FICARÁ ALOCADA NO CD SELECIONADO MANUALMENTE
                    foreach (var vProdRegra_iRegra in Gravacao.ListaRegrasControleEstoque)
                    {
                        if (qtde_a_alocar == 0)
                            break;

                        //vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD) é twmsCdXUfXPessoaXCd
                        foreach (var twmsCdXUfXPessoaXCd in vProdRegra_iRegra.TwmsCdXUfXPessoaXCd)
                        {
                            if (qtde_a_alocar == 0)
                                break;

                            if (Pedido.Ambiente.Id_nfe_emitente_selecao_manual == 0)
                            {
                                //'MODO DE SELEÇÃO AUTOMÁTICO
                                if (((vProdRegra_iRegra.Fabricante == v_item_iItem.Fabricante) && (vProdRegra_iRegra.Produto == v_item_iItem.Produto)) &&
                                  (twmsCdXUfXPessoaXCd.Id_nfe_emitente > 0) &&
                                  (twmsCdXUfXPessoaXCd.Id_nfe_emitente == vProdRegra_iRegra.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente))
                                {
                                    twmsCdXUfXPessoaXCd.Estoque_Qtde_Solicitado = twmsCdXUfXPessoaXCd.Estoque_Qtde_Solicitado ?? 0;
                                    twmsCdXUfXPessoaXCd.Estoque_Qtde_Solicitado = (short)(twmsCdXUfXPessoaXCd.Estoque_Qtde_Solicitado + qtde_a_alocar);
                                    qtde_a_alocar = 0;
                                    break;
                                }
                            }
                            else
                            {
                                //'MODO DE SELEÇÃO MANUAL
                                if (((vProdRegra_iRegra.Fabricante == v_item_iItem.Fabricante) && (vProdRegra_iRegra.Produto == v_item_iItem.Produto)) &&
                                  (twmsCdXUfXPessoaXCd.Id_nfe_emitente > 0) &&
                                  (twmsCdXUfXPessoaXCd.Id_nfe_emitente == Pedido.Ambiente.Id_nfe_emitente_selecao_manual))
                                {
                                    twmsCdXUfXPessoaXCd.Estoque_Qtde_Solicitado = twmsCdXUfXPessoaXCd.Estoque_Qtde_Solicitado ?? 0;
                                    twmsCdXUfXPessoaXCd.Estoque_Qtde_Solicitado = (short)(twmsCdXUfXPessoaXCd.Estoque_Qtde_Solicitado + qtde_a_alocar);
                                    qtde_a_alocar = 0;
                                    break;
                                }
                            }
                        }
                    }
                }

                /*
                                                '	HOUVE FALHA EM ALOCAR A QUANTIDADE REMANESCENTE?
                                                    if qtde_a_alocar > 0 then
                                                        alerta=texto_add_br(alerta)
                                                        alerta=alerta & "Falha ao processar a alocação de produtos no estoque: restaram " & qtde_a_alocar & " unidades do produto (" & v_item(iItem).fabricante & ")" & v_item(iItem).produto & " que não puderam ser alocados na lista de produtos sem presença no estoque de nenhum CD"
                                                        end if
                                                    end if
                                                next
                                            end if 'if alerta=""

                                         * */

                //'	HOUVE FALHA EM ALOCAR A QUANTIDADE REMANESCENTE?
                if (qtde_a_alocar > 0)
                    Retorno.ListaErros.Add("Falha ao processar a alocação de produtos no estoque: restaram " + qtde_a_alocar +
                        " unidades do produto (" + v_item_iItem.Fabricante + ")" + v_item_iItem.Produto +
                        " que não puderam ser alocados na lista de produtos sem presença no estoque de nenhum CD");
            }
        }
    }
}


