using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_PRODUTO_CATALOGO")]
    public class TprodutoCatalogo : IModel
    {
        //[Key]
        //[Column("id")]
        //[Required]
        public string Id { get; set; }

        //[Column("nome")]
        //[MaxLength(500)]
        //[Required]
        public string Nome { get; set; }

        //[Column("descricao")]
        //[MaxLength(500)]
        public string Descricao { get; set; }

        //[Required]
        //[Column("usuario_cadastro")]
        //[MaxLength(100)]
        public string UsuarioCadastro { get; set; }

        //[Column("usuario_edicao")]
        //[MaxLength(100)]
        public string UsuarioEdicao { get; set; }

        //[Column("dt_cadastro")]
        public DateTime DtCadastro { get; set; }

        //[Column("dt_edicao")]
        public DateTime? DtEdicao { get; set; }

        //[Column("ativo")]
        public bool Ativo { get; set; }

        public List<TprodutoCatalogoItens> campos { get; set; }
        public List<TprodutoCatalogoImagem> imagens { get; set; }
    }
}
