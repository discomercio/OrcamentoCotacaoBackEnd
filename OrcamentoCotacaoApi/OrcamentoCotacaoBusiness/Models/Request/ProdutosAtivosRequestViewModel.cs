using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public class ProdutosAtivosRequestViewModel
    {
        public int idProduto { get; set; }
        public bool? propriedadeOculta { get; set; }
        public bool? propriedadeOcultaItem { get; set; }
    }
}
