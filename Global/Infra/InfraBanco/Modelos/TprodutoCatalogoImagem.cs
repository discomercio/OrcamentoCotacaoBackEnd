using Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    public class TprodutoCatalogoImagem : IModel
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("id_produto_catalogo")]
        public int IdProdutoCatalogo { get; set; }

        [Column("id_tipo_imagem")]
        public int IdTipoImagem { get; set; }

        [Column("caminho")]
        public string Caminho { get; set; }

        [Column("ordem")]
        public int Ordem { get; set; }

        public TprodutoCatalogo TprodutoCatalogo { get; set; }

        public TprodutoCatalogoImagemTipo TprodutoCatalogoImagemTipo { get; set; }
    }
}

