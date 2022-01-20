using System;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class OrcamentoResponseViewModel : IViewModelResponse
    {

        public long Id { get; set; }
        
        public DateTime Validade { get; set; }

        public string Observacao { get; set; }

        public string NomeCliente { get; set; }

        public string NomeObra { get; set; }

        public string Vendedor { get; set; }

        public string Email { get; set; }

        public string Parceiro { get; set; }

        public string Telefone { get; set; }

        public bool ConcordaWhatsapp { get; set; }

        public string VendedorParceiro { get; set; }

        public string Uf { get; set; }

        public string Tipo { get; set; }

    }
}
