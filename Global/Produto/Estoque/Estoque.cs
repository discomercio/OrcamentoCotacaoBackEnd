using InfraBanco;
using Produto.RegrasCrtlEstoque;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Produto.Estoque
{
    public static class Estoque
    {
        public static async Task Estoque_verifica_disponibilidade_integral_v2(ContextoBdGravacao contextoBdGravacao, int id_nfe_emitente, t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD item)
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
    }
}

