using Interfaces;
using System;

namespace InfraBanco.Modelos
{
    public class TorcamentoCotacao : IModel
    {
        public int Id { get; set; }
        public string NomeCliente { get; set; }
        public string NomeObra { get; set; }
        public string Vendedor { get; set; }
        public string Email { get; set; }
        public string Parceiro { get; set; }
        public string Telefone { get; set; }
        public string ConcordaWhatsapp { get; set; }
        public string VendedorParceiro { get; set; }
        public string UF { get; set; }
        public string Tipo { get; set; }
        public DateTime Validade { get; set; }
        public string Loja { get; set; }
        public int Status { get; set; }
        public string Observacao { get; set; }
        public string UsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public string UsuarioUltimaAlteracao { get; set; }
        public DateTime? DataUltimaAlteracao { get; set; }

        public virtual TorcamentoCotacaoStatus TorcamentoCotacaoStatus { get; set; }
    }
}
