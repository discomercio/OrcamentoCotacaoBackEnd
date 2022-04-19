using Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

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
        public int IdUsuarioUltAtualizacao { get; set; }
        public DateTime? DataHoraUltAtualizacao { get; set; }

        public virtual TorcamentoCotacao TorcamentoCotacao { get; set; }

        public virtual ICollection<TorcamentoCotacaoItemUnificado> TorcamentoCotacaoItemUnificados { get; set; }
    }
}
