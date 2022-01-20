using System;
using System.Collections.Generic;
using System.Text;

namespace FormaPagamento.Dados
{
     public class FormaPagtoDados
    {
        public List<AvistaDados> ListaAvista { get; set; }
        public List<ParcUnicaDados> ListaParcUnica { get; set; }
        public bool ParcCartaoInternet{get; set; }
        public bool ParcCartaoMaquineta { get; set; }
        public List<ParcComEntradaDados> ListaParcComEntrada { get; set; }
        public List<ParcComEntPrestacaoDados> ListaParcComEntPrestacao { get; set; }
        public List<ParcSemEntradaPrimPrestDados> ListaParcSemEntPrimPrest { get; set; }
        public List<ParcSemEntPrestacaoDados> ListaParcSemEntPrestacao { get; set; }
    }
}
