using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.ProdutoCatalogo
{
    public class CadastroProdutoCatalogoImagemResponse : UtilsGlobais.RequestResponse.ResponseBase
    {
        public TprodutoCatalogoImagem TprodutoCatalogoImagem { get; set; }
    }
}
