using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace InfraBanco.Modelos
{
    public class TprodutoCatalogo : IModel
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("fabricante")]
        public string Fabricante { get; set; }

        [Column("descricao_completa")]
        public string Descricao { get; set; }

        [Column("produto")]
        public string Produto { get; set; }

        [Column("usuario_cadastro")]
        public string UsuarioCadastro { get; set; }

        [Column("usuario_edicao")]
        public string UsuarioEdicao { get; set; }

        [Column("dt_cadastro")]
        public DateTime DtCadastro { get; set; }

        [Column("dt_edicao")]
        public DateTime? DtEdicao { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; }

        public List<TprodutoCatalogoItem> campos { get; set; }
        public TprodutoCatalogoImagem imagem { get; set; }
        public Tfabricante Tfabricante { get; set; }

    }
}
