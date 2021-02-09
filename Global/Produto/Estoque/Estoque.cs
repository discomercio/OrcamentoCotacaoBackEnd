using InfraBanco;
using Produto.RegrasCrtlEstoque;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InfraBanco.Modelos;
using System;

namespace Produto.Estoque
{
#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG

    public static class Estoque
    {
        public static async Task Estoque_verifica_disponibilidade_integral_v2(
            ContextoBdGravacao contextoBdGravacao,
            int id_nfe_emitente,
            t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD item)
        {
            /*

            ' ---------------------------------------------------------------
            '   ESTOQUE_VERIFICA_DISPONIBILIDADE_INTEGRAL_V2
            '   Retorno da função:
            '      False - Ocorreu falha ao tentar verificar o estoque.
            '      True - Conseguiu fazer a verificação do estoque.
            '   Esta função consulta o banco de dados para verificar se
            '   existem produtos suficientes no "estoque de venda" para
            '   atender ao pedido.
            '   Note que os produtos a serem analisados são informados
            '   através do parâmetro 'item', que é um objeto da
            '   classe cl_CTRL_ESTOQUE_PEDIDO_ITEM_NOVO
            function estoque_verifica_disponibilidade_integral_v2(ByVal id_nfe_emitente, byref item)
            dim s
            dim rs
                estoque_verifica_disponibilidade_integral_v2=False
                with item
                    .qtde_estoque=0
                    if (.qtde_solicitada > 0) And (Trim(.produto)<>"") then
                        'Calcula quantidade em estoque no CD especificado
                        s = "SELECT" & _
                                " Sum(qtde - qtde_utilizada) AS saldo" & _
                            " FROM t_ESTOQUE INNER JOIN t_ESTOQUE_ITEM ON (t_ESTOQUE.id_estoque=t_ESTOQUE_ITEM.id_estoque)" & _
                            " WHERE" & _
                                " (t_ESTOQUE.id_nfe_emitente = " & Trim("" & id_nfe_emitente) & ")" & _
                                " AND (t_ESTOQUE.fabricante='" & .fabricante & "')" & _
                                " AND (produto='" & .produto & "')" & _
                                " AND ((qtde-qtde_utilizada) > 0)"
                        set rs=cn.Execute(s)
                        if Err<>0 then exit function
                        if Not rs.Eof then
                            if Not IsNull(rs("saldo")) then if IsNumeric(rs("saldo")) then .qtde_estoque=CLng(rs("saldo"))
                            end if
                        if rs.State <> 0 then rs.Close
                        if Err<>0 then exit function

                        'Calcula quantidade em estoque global (quantidade total disponível em todos os CD's)
                        s = "SELECT" & _
                                " Sum(qtde - qtde_utilizada) AS saldo" & _
                            " FROM t_ESTOQUE INNER JOIN t_ESTOQUE_ITEM ON (t_ESTOQUE.id_estoque=t_ESTOQUE_ITEM.id_estoque)" & _
                            " WHERE" & _
                                " (t_ESTOQUE.fabricante='" & .fabricante & "')" & _
                                " AND (produto='" & .produto & "')" & _
                                " AND ((qtde-qtde_utilizada) > 0)" & _
                                " AND (" & _
                                    "(t_ESTOQUE.id_nfe_emitente = " & Trim("" & id_nfe_emitente) & ")" & _
                                    " OR " & _
                                    "(" & _
                                        "t_ESTOQUE.id_nfe_emitente IN " & _
                                        "(SELECT id FROM t_NFe_EMITENTE WHERE (st_habilitado_ctrl_estoque = 1) AND (st_ativo = 1))" & _
                                    ")" & _
                                ")"
                        set rs=cn.Execute(s)
                        if Err<>0 then exit function
                        if Not rs.Eof then
                            if Not IsNull(rs("saldo")) then if IsNumeric(rs("saldo")) then .qtde_estoque_global=CLng(rs("saldo"))
                            end if
                        if rs.State <> 0 then rs.Close
                        if Err<>0 then exit function
                        end if
                    end with
                estoque_verifica_disponibilidade_integral_v2=True
            end function
            */
            item.Estoque_Qtde_Estoque = 0;
            if (item.Estoque_Qtde_Solicitado > 0)
            {
                {
                    //desempenho: podemos fazer um cache dessas consultas

                    //'Calcula quantidade em estoque no CD especificado
                    var saldoSql = from t_ESTOQUE_ITEM in contextoBdGravacao.TestoqueItems
                                   join t_ESTOQUE in contextoBdGravacao.Testoques on t_ESTOQUE_ITEM.Id_estoque equals t_ESTOQUE.Id_estoque
                                   where t_ESTOQUE.Id_nfe_emitente == id_nfe_emitente
                                       && t_ESTOQUE_ITEM.Fabricante == item.Estoque_Fabricante
                                       && t_ESTOQUE_ITEM.Produto == item.Estoque_Produto
                                       && (t_ESTOQUE_ITEM.Qtde - t_ESTOQUE_ITEM.Qtde_utilizada > 0)
                                   select (t_ESTOQUE_ITEM.Qtde - t_ESTOQUE_ITEM.Qtde_utilizada);
                    int? saldo = saldoSql.Sum();
                    if (saldo != null)
                        item.Estoque_Qtde_Estoque = (short)saldo.Value;
                }

                {
                    //'Calcula quantidade em estoque global (quantidade total disponível em todos os CD's)

                    //"(SELECT id FROM t_NFe_EMITENTE WHERE (st_habilitado_ctrl_estoque = 1) AND (st_ativo = 1))" & _
                    var lista_nfe_emitente_estoque_habilitado = await
                        (from nfe in contextoBdGravacao.TnfEmitentes
                         where nfe.St_Habilitado_Ctrl_Estoque == 1 && nfe.St_Ativo == 1
                         select nfe.Id).ToListAsync();

                    var saldoSql = from t_ESTOQUE_ITEM in contextoBdGravacao.TestoqueItems
                                   join t_ESTOQUE in contextoBdGravacao.Testoques on t_ESTOQUE_ITEM.Id_estoque equals t_ESTOQUE.Id_estoque
                                   where t_ESTOQUE_ITEM.Fabricante == item.Estoque_Fabricante
                                       && t_ESTOQUE_ITEM.Produto == item.Estoque_Produto
                                       && (t_ESTOQUE_ITEM.Qtde - t_ESTOQUE_ITEM.Qtde_utilizada > 0)
                                       && lista_nfe_emitente_estoque_habilitado.Contains(t_ESTOQUE.Id_nfe_emitente)
                                   select (t_ESTOQUE_ITEM.Qtde - t_ESTOQUE_ITEM.Qtde_utilizada);

                    int? saldo = saldoSql.Sum();
                    if (saldo != null)
                        item.Estoque_Qtde_Estoque_Global = (short)saldo.Value;
                }
            }
        }


        //async nao pode ter parametros ref ou out, então temos que encapsular em uma classe para ler a resposta
        public class QuantidadeEncapsulada { public short Valor; }
        public static async Task<bool> Estoque_produto_saida_v2(string id_usuario, string id_pedido, short id_nfe_emitente,
            string id_fabricante, string id_produto, int qtde_a_sair, int qtde_autorizada_sem_presenca,
            QuantidadeEncapsulada qtde_estoque_vendido, QuantidadeEncapsulada qtde_estoque_sem_presenca,
            List<string> lstErros, ContextoBdGravacao dbGravacao)
        {
            //' --------------------------------------------------------------------
            //'   ESTOQUE_PRODUTO_SAIDA_V2
            //'   Retorno da função:
            //'      False - Ocorreu falha ao tentar movimentar o estoque.
            //'      True - Conseguiu fazer a movimentação do estoque.
            //'   IMPORTANTE: sempre chame esta rotina dentro de uma transação para 
            //'      garantir a consistência dos registros entre as várias tabelas.
            //'   Esta função processa a saída dos produtos do "estoque de venda"
            //'   para o "estoque vendido".  No caso de não haver produtos sufi-
            //'   cientes no "estoque de venda" e desde que esteja autorizado
            //'   através do parâmetro "qtde_autorizada_sem_presenca", os produtos
            //'   que faltam são colocados automaticamente na lista de produtos
            //'   vendidos sem presença no estoque.


            //nao queremos esses warnings
#pragma warning disable IDE0017 // Simplify object initialization
#pragma warning disable IDE0054 // Use compound assignment


            qtde_estoque_vendido.Valor = 0;
            qtde_estoque_sem_presenca.Valor = 0;


            if (qtde_a_sair <= 0 || string.IsNullOrEmpty(id_produto) || string.IsNullOrEmpty(id_pedido))
            {
                return true;
            }

            //'	OBTÉM OS "LOTES" DO PRODUTO DISPONÍVEIS NO ESTOQUE (POLÍTICA FIFO)
            //s_sql = "SELECT" & _
            //			" t_ESTOQUE.id_estoque," & _
            //			" (qtde - qtde_utilizada) AS saldo" & _
            //		" FROM t_ESTOQUE" & _
            //			" INNER JOIN t_ESTOQUE_ITEM ON" & " (t_ESTOQUE.id_estoque=t_ESTOQUE_ITEM.id_estoque)" & _
            //		" WHERE" & _
            //			" (t_ESTOQUE.id_nfe_emitente = " & Trim("" & id_nfe_emitente) & ")" & _
            //			" AND (t_ESTOQUE_ITEM.fabricante='" & id_fabricante & "')" & _
            //			" AND (produto='" & id_produto & "')" & _
            //			" AND ((qtde - qtde_utilizada) > 0)" & _
            //		" ORDER BY" & _
            //			" data_entrada," & _
            //			" t_ESTOQUE.id_estoque"
            var lotes = await (from ei in dbGravacao.TestoqueItems
                               join e in dbGravacao.Testoques on ei.Id_estoque equals e.Id_estoque
                               where e.Id_nfe_emitente == id_nfe_emitente &&
                                     ei.Fabricante == id_fabricante &&
                                     ei.Produto == id_produto &&
                                     (ei.Qtde - ei.Qtde_utilizada) > 0
                               orderby e.Data_entrada, ei.Id_estoque
                               select new
                               {
                                   ei.Id_estoque,
                                   Saldo = (short?)(ei.Qtde - ei.Qtde_utilizada)
                               }).ToListAsync();


            //'	ARMAZENA AS ENTRADAS NO ESTOQUE CANDIDATAS À SAÍDA DE PRODUTOS
            List<string> v_estoque = new List<string>();
            int qtde_disponivel = 0;
            foreach (var lote in lotes)
            {
                if (lote.Saldo.HasValue)
                {
                    v_estoque.Add(lote.Id_estoque);
                    qtde_disponivel += lote.Saldo.Value;
                }
            }

            //NÃO HÁ PRODUTOS SUFICIENTES NO ESTOQUE!!
            if ((qtde_a_sair - qtde_autorizada_sem_presenca) > qtde_disponivel)
            {
                lstErros.Add("Produto " + id_produto + " do fabricante " + id_fabricante + ": faltam " +
                    ((qtde_a_sair - qtde_autorizada_sem_presenca) - qtde_disponivel) + " unidades no estoque (" +
                    UtilsGlobais.Util.Obtem_apelido_empresa_NFe_emitente_Gravacao(id_nfe_emitente, dbGravacao) +
                    ") para poder atender ao pedido.");
                return false;
            }

            //REALIZA A SAÍDA DO ESTOQUE!!
            int qtde_movimentada = 0;
            foreach (var v_estoque_atual in v_estoque)
            {
                //A QUANTIDADE NECESSÁRIA JÁ FOI RETIRADA DO ESTOQUE!!
                if (qtde_movimentada >= qtde_a_sair)
                {
                    break;
                }

                //' T_ESTOQUE_ITEM: SAÍDA DE PRODUTOS
                //  s_sql = "SELECT qtde, qtde_utilizada, data_ult_movimento FROM t_ESTOQUE_ITEM WHERE" & _
                //          " (id_estoque = '" & Trim(v_estoque(iv)) & "')" & _
                //          " AND (fabricante = '" & id_fabricante & "')" & _
                //          " AND (produto = '" & id_produto & "')"

                //se não localizar o registro vai dar erro
                TestoqueItem testoqueItem = await (from c in dbGravacao.TestoqueItems
                                                   where c.Id_estoque == v_estoque_atual &&
                                                         c.Fabricante == id_fabricante &&
                                                         c.Produto == id_produto
                                                   select c).FirstAsync();

                int qtde_movto = 0;
                var qtde_aux = testoqueItem.Qtde ?? 0;
                var qtde_utilizada_aux = testoqueItem.Qtde_utilizada ?? 0;
                if ((qtde_a_sair - qtde_movimentada) > (qtde_aux - qtde_utilizada_aux))
                {
                    //QUANTIDADE DE PRODUTOS DESTE ITEM DE ESTOQUE É INSUFICIENTE P/ ATENDER O PEDIDO
                    qtde_movto = qtde_aux - qtde_utilizada_aux;
                }
                else
                {
                    //QUANTIDADE DE PRODUTOS DESTE ITEM SOZINHO É SUFICIENTE P/ ATENDER O PEDIDO
                    qtde_movto = qtde_a_sair - qtde_movimentada;
                }

                testoqueItem.Qtde_utilizada = (short)(qtde_utilizada_aux + qtde_movto);
                testoqueItem.Data_ult_movimento = DateTime.Now.Date;

                dbGravacao.Update(testoqueItem);
                //feito abaixo: await dbGravacao.SaveChangesAsync();

                //CONTABILIZA QUANTIDADE MOVIMENTADA
                qtde_movimentada = qtde_movimentada + qtde_movto;


                //REGISTRA O MOVIMENTO DE SAÍDA NO ESTOQUE
                string id_estoqueMovimentoNovo = await UtilsGlobais.Util.GerarNsu(dbGravacao, InfraBanco.Constantes.Constantes.NSU_ID_ESTOQUE_MOVTO);
                if (string.IsNullOrEmpty(id_estoqueMovimentoNovo))
                {
                    lstErros.Add("Falha ao tentar gerar um número identificador para o registro de movimento no estoque. ");
                    return false;
                }

                TestoqueMovimento testoqueMovimento = new TestoqueMovimento();
                testoqueMovimento.Id_Movimento = id_estoqueMovimentoNovo;
                testoqueMovimento.Data = DateTime.Now.Date;
                testoqueMovimento.Hora = UtilsGlobais.Util.HoraParaBanco(DateTime.Now);
                testoqueMovimento.Usuario = id_usuario;
                testoqueMovimento.Id_Estoque = v_estoque_atual;
                testoqueMovimento.Fabricante = id_fabricante;
                testoqueMovimento.Produto = id_produto;
                testoqueMovimento.Qtde = (short)qtde_movto;
                testoqueMovimento.Operacao = InfraBanco.Constantes.Constantes.OP_ESTOQUE_VENDA;
                testoqueMovimento.Estoque = InfraBanco.Constantes.Constantes.ID_ESTOQUE_VENDIDO;
                testoqueMovimento.Pedido = id_pedido;
                testoqueMovimento.Loja = "";
                testoqueMovimento.Kit = 0;
                testoqueMovimento.Kit_id_estoque = "";
                dbGravacao.Add(testoqueMovimento);
                //feito abaixo: await dbGravacao.SaveChangesAsync();

                //T_ESTOQUE: ATUALIZA DATA DO ÚLTIMO MOVIMENTO
                Testoque testoque = await (from c in dbGravacao.Testoques
                                           where c.Id_estoque == v_estoque_atual
                                           select c).FirstAsync();

                testoque.Data_ult_movimento = DateTime.Now.Date;

                dbGravacao.Update(testoque);
                await dbGravacao.SaveChangesAsync();

                //JÁ CONSEGUIU ALOCAR TUDO?
                if (qtde_movimentada >= qtde_a_sair)
                {
                    break;
                }
            }

            //NÃO CONSEGUIU MOVIMENTAR A QUANTIDADE SUFICIENTE
            if (qtde_movimentada < (qtde_a_sair - qtde_autorizada_sem_presenca))
            {
                lstErros.Add("Produto " + id_produto + " do fabricante " + id_fabricante + ": faltam " +
                    ((qtde_a_sair - qtde_autorizada_sem_presenca) - qtde_movimentada) +
                    " unidades no estoque para poder atender ao pedido.");
                return false;
            }

            //REGISTRA A VENDA SEM PRESENÇA NO ESTOQUE
            if (qtde_movimentada < qtde_a_sair)
            {
                //REGISTRA O MOVIMENTO DE SAÍDA NO ESTOQUE
                var id_estoque_movto = await UtilsGlobais.Util.GerarNsu(dbGravacao, InfraBanco.Constantes.Constantes.NSU_ID_ESTOQUE_MOVTO);
                if (string.IsNullOrEmpty(id_estoque_movto))
                {
                    lstErros.Add("Falha ao tentar gerar um número identificador para o registro de movimento no estoque.");
                    return false;
                }

                qtde_estoque_sem_presenca.Valor = (short)(qtde_a_sair - qtde_movimentada);

                TestoqueMovimento testoqueMovimento = new TestoqueMovimento();
                testoqueMovimento.Id_Movimento = id_estoque_movto;
                testoqueMovimento.Data = DateTime.Now.Date;
                testoqueMovimento.Hora = UtilsGlobais.Util.HoraParaBanco(DateTime.Now);
                testoqueMovimento.Usuario = id_usuario;
                testoqueMovimento.Id_Estoque = "";// está sem presença no estoque
                testoqueMovimento.Fabricante = id_fabricante;
                testoqueMovimento.Produto = id_produto;
                testoqueMovimento.Qtde = qtde_estoque_sem_presenca.Valor;
                testoqueMovimento.Operacao = InfraBanco.Constantes.Constantes.OP_ESTOQUE_VENDA;
                testoqueMovimento.Estoque = InfraBanco.Constantes.Constantes.ID_ESTOQUE_SEM_PRESENCA;
                testoqueMovimento.Pedido = id_pedido;
                testoqueMovimento.Loja = "";
                testoqueMovimento.Kit = 0;
                testoqueMovimento.Kit_id_estoque = "";
                dbGravacao.Add(testoqueMovimento);
                await dbGravacao.SaveChangesAsync();
            }

            qtde_estoque_vendido.Valor = (short)qtde_movimentada;


            //Log de movimentação do estoque
            await Grava_log_estoque_v2(id_usuario, id_nfe_emitente, id_fabricante, id_produto,
                (short)qtde_a_sair, qtde_estoque_vendido.Valor, InfraBanco.Constantes.Constantes.OP_ESTOQUE_LOG_VENDA,
                InfraBanco.Constantes.Constantes.ID_ESTOQUE_VENDA, InfraBanco.Constantes.Constantes.ID_ESTOQUE_VENDIDO,
                "", "", "", id_pedido, "", "", "", dbGravacao);

            if (qtde_estoque_sem_presenca.Valor > 0)
            {
                await Grava_log_estoque_v2(id_usuario, id_nfe_emitente, id_fabricante, id_produto,
                    qtde_estoque_sem_presenca.Valor, qtde_estoque_sem_presenca.Valor,
                    InfraBanco.Constantes.Constantes.OP_ESTOQUE_LOG_VENDA_SEM_PRESENCA, "",
                    InfraBanco.Constantes.Constantes.ID_ESTOQUE_SEM_PRESENCA, "", "", "", id_pedido, "", "", "", dbGravacao);
            }
            return true;
#pragma warning restore IDE0054 // Use compound assignment
#pragma warning restore IDE0017 // Simplify object initialization
        }

        private static async Task Grava_log_estoque_v2(string strUsuario, short id_nfe_emitente, string strFabricante,
            string strProduto, short intQtdeSolicitada, short intQtdeAtendida, string strOperacao,
            string strCodEstoqueOrigem, string strCodEstoqueDestino, string strLojaEstoqueOrigem,
            string strLojaEstoqueDestino, string strPedidoEstoqueOrigem, string strPedidoEstoqueDestino,
            string strDocumento, string strComplemento, string strIdOrdemServico, ContextoBdGravacao contexto)
        {
#pragma warning disable IDE0017 // Simplify object initialization
            TestoqueLog testoqueLog = new TestoqueLog();

            testoqueLog.data = DateTime.Now.Date;
            testoqueLog.Data_hora = DateTime.Now;
            testoqueLog.Usuario = strUsuario;
            testoqueLog.Id_nfe_emitente = id_nfe_emitente;
            testoqueLog.Fabricante = strFabricante;
            testoqueLog.Produto = strProduto;
            testoqueLog.Qtde_solicitada = intQtdeSolicitada;
            testoqueLog.Qtde_atendida = intQtdeAtendida;
            testoqueLog.Operacao = strOperacao;
            testoqueLog.Cod_estoque_origem = strCodEstoqueOrigem;
            testoqueLog.Cod_estoque_destino = strCodEstoqueDestino;
            testoqueLog.Loja_estoque_origem = strLojaEstoqueOrigem;
            testoqueLog.Loja_estoque_destino = strLojaEstoqueDestino;
            testoqueLog.Pedido_estoque_origem = strPedidoEstoqueOrigem;
            testoqueLog.Pedido_estoque_destino = strPedidoEstoqueDestino;
            testoqueLog.Documento = strDocumento;
            testoqueLog.Complemento = strComplemento.Length > 80 ? strComplemento.Substring(0, 80) : strComplemento;
            testoqueLog.Id_ordem_servico = strIdOrdemServico;
            contexto.Add(testoqueLog);
            await contexto.SaveChangesAsync();
#pragma warning restore IDE0017 // Simplify object initialization
        }

    }
#endif

}

