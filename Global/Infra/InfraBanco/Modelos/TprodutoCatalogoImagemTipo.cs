using Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    public class TprodutoCatalogoImagemTipo : IModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public List<TprodutoCatalogoImagem> TprodutoCatalogoImagems { get; set; }
    }
}
