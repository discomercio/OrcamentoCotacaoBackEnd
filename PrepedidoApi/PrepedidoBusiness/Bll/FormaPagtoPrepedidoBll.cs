using PrepedidoBusiness.Dto.FormaPagto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoBusiness.Bll
{
    public class FormaPagtoPrepedidoBll
    {
        private readonly Prepedido.FormaPagto.FormaPagtoBll formaPagtoBll;

        public FormaPagtoPrepedidoBll(Prepedido.FormaPagto.FormaPagtoBll formaPagtoBll)
        {
            this.formaPagtoBll = formaPagtoBll;
        }
        public async Task<FormaPagtoDto> ObterFormaPagto(string apelido, string tipo_pessoa)
        {
            Prepedido.Dados.FormaPagto.FormaPagtoDados ret = await formaPagtoBll.ObterFormaPagto(apelido, tipo_pessoa);
            return FormaPagtoDto.FormaPagtoDto_De_FormaPagtoDados(ret);
        }
    }
}
