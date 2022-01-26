using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class OrcamentistaEIndicadorVendedorResponseViewModel : IViewModelResponse
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Indicador { get; set; }
        public bool Ativo { get; set; }
        public int Id { get; set; }
    }
}
