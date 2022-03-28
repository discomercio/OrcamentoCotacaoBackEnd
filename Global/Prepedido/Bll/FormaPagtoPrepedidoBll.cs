using FormaPagamento;
using FormaPagamento.Dados;
using PrepedidoBusiness.Dto.FormaPagto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoBusiness.Bll
{
    public class FormaPagtoPrepedidoBll
    {
        private readonly FormaPagtoBll formaPagtoBll;

        public FormaPagtoPrepedidoBll(FormaPagtoBll formaPagtoBll)
        {
            this.formaPagtoBll = formaPagtoBll;
        }
        public async Task<FormaPagtoDto> ObterFormaPagto(string apelido, string tipo_pessoa)
        {
            FormaPagtoDados ret = await formaPagtoBll.ObterFormaPagto(apelido, tipo_pessoa,
                InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS);
            return FormaPagtoDto.FormaPagtoDto_De_FormaPagtoDados(ret);
        }
    }
}
