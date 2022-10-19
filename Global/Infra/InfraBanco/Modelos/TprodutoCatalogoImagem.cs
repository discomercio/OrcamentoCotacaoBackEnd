using Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    public class TprodutoCatalogoImagem : IModel
    {
        public int Id { get; set; }

        public int IdProdutoCatalogo { get; set; }
        
        public int IdTipoImagem { get; set; }
        
        public string Caminho { get; set; }

        public int Ordem { get; set; }

        public TprodutoCatalogo TprodutoCatalogo { get; set; }

        public TprodutoCatalogoImagemTipo TprodutoCatalogoImagemTipo { get; set; }
    }
}

