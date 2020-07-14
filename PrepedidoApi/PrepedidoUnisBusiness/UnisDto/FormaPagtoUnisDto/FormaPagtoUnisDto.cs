using PrepedidoBusiness.Dto.FormaPagto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.FormaPagtoUnisDto
{
    public class FormaPagtoUnisDto : FormaPagtoDto
    {

        public static FormaPagtoUnisDto FormaPagtoUnisDtoDeFormaPagtoDto(FormaPagtoDto formaPagto)
        {
            FormaPagtoUnisDto formaPagtoUnis = new FormaPagtoUnisDto()
            {
                ListaAvista = formaPagto.ListaAvista,
                ListaParcUnica = formaPagto.ListaParcUnica,
                ListaParcComEntrada = formaPagto.ListaParcComEntrada,
                ListaParcComEntPrestacao = formaPagto.ListaParcComEntPrestacao,
                ListaParcSemEntPrestacao = formaPagto.ListaParcSemEntPrestacao,
                ListaParcSemEntPrimPrest = formaPagto.ListaParcSemEntPrimPrest,
                ParcCartaoInternet = formaPagto.ParcCartaoInternet,
                ParcCartaoMaquineta = formaPagto.ParcCartaoMaquineta
            };

            return formaPagtoUnis;
        }
    }
}
