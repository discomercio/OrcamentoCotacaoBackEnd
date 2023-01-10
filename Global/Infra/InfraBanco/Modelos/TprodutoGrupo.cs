using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos
{
    public class TprodutoGrupo : IModel
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public byte Inativo { get; set; }
        public DateTime DtHrCadastro { get; set; }
        public string UsuarioCadastro { get; set; }
        public DateTime DtHrUltAtualizacao { get; set; }
        public string UsuarioUltAtualizacao { get; set; }
    }
}
