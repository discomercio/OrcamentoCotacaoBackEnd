using Loja.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loja.Bll.Bll.AcessoBll
{
    public class UsuarioAcessoBll
    {
        private readonly ContextoBdProvider contextoProvider;

        public UsuarioAcessoBll(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public class Loja
        {
            public string Nome { get; set; }
            public string Id { get; set; }
        }
        public async Task<List<Loja>> Loja_troca_rapida_monta_itens_select(string strUsuario)
        {
            var ret = new List<Loja>();

            strUsuario = strUsuario ?? "";
            strUsuario = strUsuario.Trim();
            if (string.IsNullOrWhiteSpace(strUsuario))
                return ret;
            var lista = from loja in contextoProvider.GetContextoLeitura().Tlojas
                        select new Loja() { Id = loja.Loja, Nome = loja.Nome };
            return await lista.ToListAsync();

            //TODO: listar somente as lojas permitidas
            /*
             *TODO: AFAZER, precisa mapear a tabela 

            var lista = from loja in contextoProvider.GetContextoLeitura().loj
            ha_default=False

            if id_default = "" then
                strSql = "SELECT" & _
                            " tUL.loja, tL.nome" & _
                        " FROM t_USUARIO_X_LOJA tUL" & _
                            " INNER JOIN t_LOJA tL ON (tUL.loja=tL.loja)" & _
                        " WHERE" & _
                            " (usuario = '" & strUsuario & "')" & _
                        " ORDER BY" & _
                            " tUL.loja"
            else
            '	LEMBRE-SE: O USUÁRIO QUE TEM PERMISSÃO DE ACESSO A TODAS AS LOJAS PODE
            '	ACESSAR UMA LOJA QUE NÃO ESTÁ CADASTRADA EM t_USUARIO_X_LOJA
                strSql = "SELECT DISTINCT" & _
                            " loja, nome" & _
                        " FROM " & _ 
                            "(" & _ 
                                "SELECT" & _
                                    " tUL.loja, tL.nome" & _
                                " FROM t_USUARIO_X_LOJA tUL" & _
                                    " INNER JOIN t_LOJA tL ON (tUL.loja=tL.loja)" & _
                                " WHERE" & _
                                    " (usuario = '" & strUsuario & "')" & _
                                " UNION " & _
                                "SELECT" & _
                                    " loja, nome" & _
                                " FROM t_LOJA" & _
                                " WHERE" & _
                                    " (CONVERT(smallint, loja) = " & id_default & ")" & _
                            ") t__AUX" & _
                        " ORDER BY" & _
                            " loja"
                end if

            set r = cn.Execute(strSql)
            strResp = ""
            do while Not r.eof
                x = Trim("" & r("loja"))
                if (id_default<>"") And(id_default= x) then
                    strResp = strResp & "<option selected"
                    ha_default=True
                else
                    strResp = strResp & "<option"
                    end if
                strResp = strResp & " value='" & x & "'>"
                strResp = strResp & "&nbsp;" & Trim("" & r("nome")) & "&nbsp;&nbsp;&nbsp;"
                strResp = strResp & "</option>" & chr(13)
                r.MoveNext
                loop

            if Not ha_default then
                strResp = "<option selected value=''>&nbsp;</option>" & chr(13) & strResp
                end if

            loja_troca_rapida_monta_itens_select = strResp
            r.close
            set r = nothing
        end function
        */



        }
    }
}

