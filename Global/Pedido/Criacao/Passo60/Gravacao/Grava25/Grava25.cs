using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable IDE0054 // Use compound assignment
namespace Pedido.Criacao.Passo60.Gravacao.Grava25
{
    class Grava25 : PassoBaseGravacao
    {
        public Grava25(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao, Execucao.Gravacao gravacao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao, gravacao)
        {
        }

        public async Task ExecutarAsync()
        {

            //Passo25:  
            //	VERIFICA SE AS REGRAS ASSOCIADAS AOS PRODUTOS ESTÃO OK - linha 1010
            //		Verifica se todos os produtos possuem regra ativa e não bloqueada e ao menos um CD ativo.
            //	'NO CASO DE SELEÇÃO MANUAL DO CD, VERIFICA SE O CD SELECIONADO ESTÁ HABILITADO EM TODAS AS REGRAS - linha 1047
            //		No caso de CD manual, verifica se o CD tem regra ativa


            //PedidoNovoConfirma.asp linha mais ou menos 1073
            //'VERIFICA SE AS REGRAS ASSOCIADAS AOS PRODUTOS ESTÃO OK
            Produto.ProdutoGeralBll.VerificarRegrasAssociadasAosProdutos(Gravacao.ListaRegrasControleEstoque,
                Retorno.ListaErros, Pedido.EnderecoCadastralCliente.Endereco_uf, Pedido.EnderecoCadastralCliente.Endereco_tipo_pessoa,
                Pedido.Ambiente.Id_nfe_emitente_selecao_manual);

            await CdSelecionadoEstaHabilitadoEmTodasAsRegras();

            MarcarErroSeNaoExsitir();

        }

        public async Task CdSelecionadoEstaHabilitadoEmTodasAsRegras()
        {
            /*
            'NO CASO DE SELEÇÃO MANUAL DO CD, VERIFICA SE O CD SELECIONADO ESTÁ HABILITADO EM TODAS AS REGRAS
                if id_nfe_emitente_selecao_manual <> 0 then
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

            if (Pedido.Ambiente.Id_nfe_emitente_selecao_manual == 0)
                return;

            StringBuilder alerta_aux = new StringBuilder();
            foreach (var vProdRegra_iRegra in Gravacao.ListaRegrasControleEstoque)
            {
                var blnAchou = false;
                var blnDesativado = false;
                if (!string.IsNullOrWhiteSpace(vProdRegra_iRegra.Produto))
                {
                    foreach (var regraPessoa_vCD_iCD in vProdRegra_iRegra.TwmsCdXUfXPessoaXCd)
                    {
                        if (regraPessoa_vCD_iCD.Id_nfe_emitente == Pedido.Ambiente.Id_nfe_emitente_selecao_manual)
                        {
                            blnAchou = true;
                            if (regraPessoa_vCD_iCD.St_inativo == 1)
                            {
                                blnDesativado = true;
                            }
                            break;
                        }
                    }
                }
                if (!blnAchou)
                {
                    if (alerta_aux.Length > 0)
                        alerta_aux.Append(UtilsGlobais.Util.EnterParaMensagemErro());
                    alerta_aux.Append("Produto (" + vProdRegra_iRegra.Fabricante
                        + ")" + vProdRegra_iRegra.Produto + ": regra '" + vProdRegra_iRegra.TwmsRegraCd.Apelido
                        + "' (Id=" + vProdRegra_iRegra.TwmsRegraCd.Id + ") não permite o CD '"
                        + await (UtilsGlobais.Util.Obtem_apelido_empresa_NFe_emitente_Gravacao(Pedido.Ambiente.Id_nfe_emitente_selecao_manual, ContextoBdGravacao)) + "'");
                }
                else if (blnDesativado)
                {
                    //'16/09/2017: FOI REALIZADA UMA ALTERAÇÃO P/ QUE A SELEÇÃO MANUAL DE CD PERMITA O USO DE CD DESATIVADO
                    //'alerta_aux=texto_add_br(alerta_aux)
                    //'alerta_aux=alerta_aux & "Produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & ": regra '" & vProdRegra(iRegra).regra.apelido & "' (Id=" & vProdRegra(iRegra).regra.id & ") define o CD '" & obtem_apelido_empresa_NFe_emitente(id_nfe_emitente_selecao_manual) & "' como 'desativado'"
                }
            }

            if (alerta_aux.Length > 0)
            {
                var alerta = "O CD selecionado manualmente não pode ser usado devido aos seguintes motivos:";
                alerta = alerta + UtilsGlobais.Util.EnterParaMensagemErro();
                alerta = alerta + alerta_aux.ToString();
                Retorno.ListaErros.Add(alerta);
            }

        }

        private void MarcarErroSeNaoExsitir()
        {
            //se ja tem algum erro, nao precisa mais
            if (Retorno.ListaErros.Count != 0)
                return;

            //garante que tenha rgistrado o erro
            foreach (var regra in Gravacao.ListaRegrasControleEstoque)
            {
                if (!regra.St_Regra_ok)
                    Retorno.ListaErros.Add("Ocorreu algum erro na leitura das regras de consumo de estoque.");
            }

        }
    }
}
#pragma warning restore IDE0054 // Use compound assignment
