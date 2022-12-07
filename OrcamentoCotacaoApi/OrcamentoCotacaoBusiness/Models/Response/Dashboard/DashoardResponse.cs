using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.Dashoard
{
    public class DashoardResponse : UtilsGlobais.RequestResponse.ResponseBase
    {

        public string NumeroOrcamento { get; set; }
        public string Vendedor { get; set; }
        public string Parceiro { get; set; }
        public string VendedorParceiro { get; set; }
        public int? IdIndicadorVendedor { get; set; }
        public DateTime? DtExpiracao { get; set; }
        public DateTime? DataServidor { get; set; }

    }
}
