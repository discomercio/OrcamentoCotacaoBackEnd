using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_PRODUTO_CATALOGO")]
    public class TprodutoCatalogo : IModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string UsuarioCadastro { get; set; }
        public string UsuarioEdicao { get; set; }
        public DateTime DtCadastro { get; set; }
        public DateTime? DtEdicao { get; set; }
        public bool Ativo { get; set; }

        public List<TprodutoCatalogoItens> campos { get; set; }
        public List<TprodutoCatalogoImagem> imagens { get; set; }
    }
}
