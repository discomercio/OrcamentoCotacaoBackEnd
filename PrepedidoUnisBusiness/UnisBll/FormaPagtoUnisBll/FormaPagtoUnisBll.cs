
using PrepedidoUnisBusiness.UnisDto.FormaPagtoUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoUnisBusiness.UnisBll.FormaPagtoUnisBll
{
    public class FormaPagtoUnisBll
    {

        public async Task<FormaPagtoUnisDto> ObterFormaPagto(string apelido, string tipo_pessoa)
        {
            FormaPagtoUnisDto retorno = new FormaPagtoUnisDto();

            return retorno;
        }

        public async Task<int> BuscarQtdeParcCartaoVisa()
        {
            return 0;
        }
    }
}
