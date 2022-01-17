using OrcamentoCotacao.Data.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacao.Data.Entities
{
    public class Orcamento
    {

        public long Id { get; set; }

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

        public DateTime Validade { get; set; }

        public string Observacao { get; set; }

        public string UsuarioCadastro { get; set; }

        public DateTime DataCadastro { get; set; }

        public string UsuarioUltimaAlteracao { get; set; }

        public DateTime? DataUltimaAlteracao { get; set; }



    }
}
