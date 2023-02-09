using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.Dashoard
{
    public class DashoardResponse
    {

        public string NumeroOrcamento { get; set; }
        public string Vendedor { get; set; }
        public string Parceiro { get; set; }
        public string VendedorParceiro { get; set; }
        public int? IdIndicadorVendedor { get; set; }
        public DateTime? DtExpiracao { get; set; }

    }
}
