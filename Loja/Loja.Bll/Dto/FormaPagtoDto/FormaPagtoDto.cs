using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.FormaPagtoDto
{
    public class FormaPagtoDto
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
