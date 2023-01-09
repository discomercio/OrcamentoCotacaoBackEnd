using Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    public class TorcamentoCotacaoOpcao : IModel
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("IdOrcamentoCotacao")]
        public int IdOrcamentoCotacao { get; set; }

        [Column("PercRT")]
        public float PercRT { get; set; }

        [Column("Sequencia")]
        public int Sequencia { get; set; }

        [Column("IdTipoUsuarioContextoCadastro")]
        public int IdTipoUsuarioContextoCadastro { get; set; }

        [Column("IdUsuarioCadastro")]
        public int IdUsuarioCadastro { get; set; }

        [Column("DataCadastro")]
        public DateTime DataCadastro { get; set; }

        [Column("DataHoraCadastro")]
        public DateTime DataHoraCadastro { get; set; }

        [Column("IdTipoUsuarioContextoUltAtualizacao")]
        public int? IdTipoUsuarioContextoUltAtualizacao { get; set; }

        [Column("IdUsuarioUltAtualizacao")]
        public int? IdUsuarioUltAtualizacao { get; set; }

        [Column("DataHoraUltAtualizacao")]
        public DateTime? DataHoraUltAtualizacao { get; set; }

        public virtual TorcamentoCotacao TorcamentoCotacao { get; set; }

        public virtual List<TorcamentoCotacaoItemUnificado> TorcamentoCotacaoItemUnificados { get; set; }
    }
}
