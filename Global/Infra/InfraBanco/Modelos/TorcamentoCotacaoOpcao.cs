using Interfaces;
using System;

namespace InfraBanco.Modelos
{
    public class TorcamentoCotacaoOpcao : IModel
    {
        public int Id { get; set; }
        public int IdOrcamentoCotacao { get; set; }
        public float PercRT { get; set; }
        public int Sequencia { get; set; }
        public int IdTipoUsuarioContextoCadastro { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataHoraCadastro { get; set; }
        public int IdTipoUsuarioContextoUltAtualizacao { get; set; }
        public string UsuarioUltimaAlteracao { get; set; }
        public DateTime? DataUltimaAlteracao { get; set; }

        public virtual TorcamentoCotacao TorcamentoCotacao { get; set; }
    }
}
