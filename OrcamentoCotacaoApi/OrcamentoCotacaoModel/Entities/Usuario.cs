using System;

namespace OrcamentoCotacaoModel.Entities
{
    public class Usuario
    {
        public long Id { get; set; }

        public string Nome { get; set; }
        public string IdVendedor { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public string IdParceiro { get; set; }

        public string Telefone { get; set; }

        public string Celular { get; set; }

        public bool Ativo { get; set; }

        public int TipoUsuario { get; set; }

        public string UsuarioCadastro { get; set; }

        public DateTime DataCadastro { get; set; }

        public string UsuarioUltimaAlteracao { get; set; }

        public DateTime? DataUltimaAlteracao { get; set; }
    }
}
