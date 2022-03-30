using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Produto.Dados
{
    public class ProdutoCatalogoPropriedadeDados
    {
        public int id { get; set; }
        [Required]
        public short IdCfgTipoPropriedade { get; set; }
        [Required]
        public short IdCfgTipoPermissaoEdicaoCadastro { get; set; }
        [Required]
        public short IdCfgDataType { get; set; }
        [Required]
        public string descricao { get; set; }
        [Required]
        public bool oculto { get; set; }
        [Required]
        public int ordem { get; set; }
        [Required]
        public DateTime dt_cadastro { get; set; }
        [Required]
        public string usuario_cadastro { get; set; }

    }
}
        
        

    

    

    
