using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Loja.Modelos
{
    [Table("t_LOJA")]
    public class Tloja
    {
        [Column("loja")]
        [Key]
        [Required]
        [MaxLength(3)]
        public string Loja { get; set; }

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

        [Column("dt_cadastro")]
        public DateTime Dt_Cadastro { get; set; }

        [Column("dt_ult_atualizacao")]
        public DateTime Dt_Ult_Atualizacao { get; set; }

        [Column("comissao_indicacao")]
        public float? Comissao_Indicacao { get; set; }

        [Column("PercMaxSenhaDesconto")]
        public float PercMaxSenhaDesconto { get; set; }

        [Column("id_plano_contas_empresa")]
        public byte Id_Plano_Contas_Empresa { get; set; }

        [Column("id_plano_contas_grupo")]
        public short Id_Plano_Contas_Grupo { get; set; }

        [Column("id_plano_contas_conta")]
        public int Id_Plano_Contas_Conta { get; set; }

        [Column("natureza")]
        public char Natureza { get; set; }//verificar

        [Column("endereco_numero")]
        [MaxLength(20)]
        public string Endereco_Numero { get; set; }

        [Column("endereco_complemento")]
        [MaxLength(60)]
        public string Endereco_Complemento { get; set; }

        [Column("PercMaxDescSemZerarRT")]
        public float PercMaxDescSemZerarRT { get; set; }

        [Column("perc_max_comissao")]
        public float Perc_Max_Comissao { get; set; }

        [Column("perc_max_comissao_e_desconto")]
        public float Perc_Max_Comissao_E_Desconto { get; set; }

        [Column("perc_max_comissao_e_desconto_nivel2")]
        public float Perc_Max_Comissao_E_Desconto_Nivel2 { get; set; }

        [Column("perc_max_comissao_e_desconto_nivel2_pj")]
        public float Perc_Max_Comissao_E_Desconto_Nivel2_Pj { get; set; }

        [Column("perc_max_comissao_e_desconto_pj")]
        public float Perc_Max_Comissao_E_Desconto_Pj { get; set; }

        [Column("magento_api_urlWebService")]
        [MaxLength(1024)]
        public string Magento_Api_UrlWebService { get; set; }

        [Column("magento_api_username")]
        [MaxLength(80)]
        public string Magento_Api_Username { get; set; }

        [Column("magento_api_password")]
        [MaxLength(256)]
        public string Magento_Api_Password { get; set; }
    }
}
