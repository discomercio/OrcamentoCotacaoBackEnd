
using InfraBanco.Constantes;
using PrepedidoBusiness.Bll;
using PrepedidoUnisBusiness.UnisDto.FormaPagtoUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoUnisBusiness.UnisBll.FormaPagtoUnisBll
{
    public class FormaPagtoUnisBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly PrepedidoBll prepedidoBll;
        private readonly FormaPagtoBll formaPagtoBll;

        public FormaPagtoUnisBll(InfraBanco.ContextoBdProvider contextoProvider, PrepedidoBll prepedidoBll,
            FormaPagtoBll formaPagtoBll)
        {
            this.contextoProvider = contextoProvider;
            this.prepedidoBll = prepedidoBll;
            this.formaPagtoBll = formaPagtoBll;
        }

        public async Task<FormaPagtoUnisDto> ObterFormaPagto(string apelido, string tipo_pessoa)
        {
            /*
             * Vamos validar os dados que estão sendo enviados antes de fazer a chamada
             * validar o orcamentista 
             * validar o tipo de pessoa
             */

            FormaPagtoUnisDto retorno = new FormaPagtoUnisDto();

            //valida orcamentista e o tipo de pessoa
            if (await prepedidoBll.ValidarOrcamentistaIndicador(apelido) &&
                (tipo_pessoa == Constantes.ID_PF || tipo_pessoa == Constantes.ID_PJ))
            {
                 retorno = FormaPagtoUnisDto.FormaPagtoUnisDtoDeFormaPagtoDto(
                    await formaPagtoBll.ObterFormaPagto(apelido, tipo_pessoa));
            }
            else
            {
                return retorno;
            }

            return retorno;
        }

        public async Task<int> BuscarQtdeParcCartaoVisa()
        {
            return await formaPagtoBll.BuscarQtdeParcCartaoVisa();
        }
    }
}
