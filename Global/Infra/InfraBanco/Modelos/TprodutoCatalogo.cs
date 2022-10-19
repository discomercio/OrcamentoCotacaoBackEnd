using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace InfraBanco.Modelos
{
    public class TprodutoCatalogo : IModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Fabricante { get; set; }
        public string Descricao { get; set; }
        public string Produto { get; set; }
        public string UsuarioCadastro { get; set; }
        public string UsuarioEdicao { get; set; }
        public DateTime DtCadastro { get; set; }
        public DateTime? DtEdicao { get; set; }
        public bool Ativo { get; set; }

        public List<TprodutoCatalogoItem> campos { get; set; }
        public TprodutoCatalogoImagem imagem { get; set; }

    }
}
