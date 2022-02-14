using FormaPagamento.Dados;
using System.Collections.Generic;

namespace PrepedidoBusiness.Dto.FormaPagto
{
    public class FormaPagtoResponseViewModel
    {
        public List<AvistaDto> ListaAvista { get; set; }
        public List<ParcUnicaDto> ListaParcUnica { get; set; }
        public bool ParcCartaoInternet { get; set; }
        public bool ParcCartaoMaquineta { get; set; }
        public List<ParcComEntradaDto> ListaParcComEntrada { get; set; }
        public List<ParcComEntPrestacaoDto> ListaParcComEntPrestacao { get; set; }
        public List<ParcSemEntradaPrimPrestDto> ListaParcSemEntPrimPrest { get; set; }
        public List<ParcSemEntPrestacaoDto> ListaParcSemEntPrestacao { get; set; }
    }
}

