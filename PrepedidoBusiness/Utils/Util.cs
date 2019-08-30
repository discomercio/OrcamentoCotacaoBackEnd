using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using InfraBanco;

namespace PrepedidoBusiness.Utils
{
    public class Util
    {
        //private readonly InfraBanco.ContextoProvider contextoProvider;

        //public Util(InfraBanco.ContextoProvider contextoProvider)
        //{
        //    this.contextoProvider = contextoProvider;
        //}

        public static string FormatCpf_Cnpj_Ie(string cpf_cnpj)
        {
            //caso esteja vazio, não formatamos
            if (!UInt64.TryParse(cpf_cnpj, out UInt64 convertido))
                return cpf_cnpj;

            if (cpf_cnpj.Length > 11)
            {
                if (cpf_cnpj.Length > 12)
                    return convertido.ToString(@"00\.000\.000\/0000\-00");
                else
                    return convertido.ToString(@"000\.000\.000\.000");
            }
            else
            {
                return convertido.ToString(@"000\.000\.000\-00");
            }

        }

        public static string ObterDescricao_Cod(string grupo, string cod, ContextoProvider contextoProvider)
        {
            var db = contextoProvider.GetContexto();

            var desc = from c in db.TcodigoDescricaos
                       where c.Grupo == grupo && c.Codigo == cod
                       select c.Descricao;

            string result = desc.FirstOrDefault();

            if (result == null || result == "")
                return "Código não cadastrado (" + cod + ")";

            return result;
        }
    }
}
