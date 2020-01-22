using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Loja.Modelos
{
    [Table("t_FABRICANTE")]
    public class Tfabricante
    {
        [Key]
        [Column("fabricante")]
        [MaxLength(4)]
        [Required]
        public string Fabricante { get; set; }        

        [Column("cnpj")]
        [MaxLength(14)]
        public string Cnpj { get; set; }

        [Column("ie")]
        [MaxLength(20)]
        public string Ie { get; set; }

        [Column("nome")]
        [MaxLength(30)]
        public string Nome { get; set; }

        [Column("razao_social")]
        [MaxLength(60)]
        public string Razao_Social { get; set; }

        [Column("endereco")]
        [MaxLength(80)]
        public string Endereco { get; set; }

        [Column("bairro")]
        [MaxLength(72)]
        public string Bairro { get; set; }

        [Column("cidade")]
        [MaxLength(60)]
        public string Cidade { get; set; }

        [Column("uf")]
        [MaxLength(2)]
        public string Uf { get; set; }

        [Column("cep")]
        [MaxLength(8)]
        public string Cep { get; set; }

        [Column("ddd")]
        [MaxLength(4)]
        public string Ddd { get; set; }

        [Column("telefone")]
        [MaxLength(11)]
        public string Telefone { get; set; }

        [Column("fax")]
        [MaxLength(11)]
        public string Fax { get; set; }

        [Column("contato")]
        [MaxLength(40)]
        public string Contato { get; set; }

        [Column("dt_cadastro")]
        [Required]
        public DateTime Dt_Cadastro { get; set; }

        [Column("dt_ult_atualizacao")]
        [Required]
        public DateTime Dt_Ult_Atualizacao { get; set; }

        [Column("markup")]
        public float Markup { get; set; }

        [Column("endereco_numero")]
        [MaxLength(20)]
        public string Endereco_Numero { get; set; }

        [Column("endereco_complemento")]
        [MaxLength(60)]
        public string Endereco_Complemento { get; set; }

        [Column("perc_agio")]
        public float? Perc_Agio { get; set; }
    }
}
