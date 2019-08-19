using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Utils
{
    public class Util
    {
        //Formata CPF e CNPJ
        public static string FormatCpf_Cnpj(string cpf_cnpj)
        {
            if (cpf_cnpj.Length > 11)
            {
                return Convert.ToUInt64(cpf_cnpj).ToString(@"00\.000\.000\/0000\-00");
            }
            else
            {
                return Convert.ToUInt64(cpf_cnpj).ToString(@"000\.000\.000\-00");
            }

        }
    }
}
