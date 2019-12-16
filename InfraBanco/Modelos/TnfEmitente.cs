using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_NFe_EMITENTE")]
    public class TnfEmitente
    {
        [Key]
        [Required]
        [Column("id")]
        public short Id { get; set; }

        [Column("id_boleto_cedente")]
        [Required]
        public short Id_Boleto_Cedente { get; set; }

        [Column("braspag_id_boleto_cedente")]
        [Required]
        public short Braspag_Id_Boleto_Cedente { get; set; }

        [Column("st_ativo")]
        [Required]
        public byte St_Ativo { get; set; }

        [Column("apelido")]
        [MaxLength(20)]
        [Required]
        public string Apelido { get; set; }

        [Column("cnpj")]
        [MaxLength(14)]
        [Required]
        public string Cnpj { get; set; }

        [Column("razao_social")]
        [MaxLength(160)]
        [Required]
        public string Razao_Social { get; set; }

        [Column("endereco")]
        [MaxLength(80)]
        public string Endereco { get; set; }

        [Column("endereco_numero")]
        [MaxLength(20)]
        public string Endereco_Numero { get; set; }

        [Column("endereco_complemento")]
        [MaxLength(60)]
        public string Endereco_Complemento { get; set; }

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

        [Column("NFe_st_emitente_padrao")]
        [Required]
        public byte NFe_st_emitente_padrao { get; set; }

        //[old_NFe_serie_NF] esta coluna mudou o nome na base de homologação deles
        [Column("old_NFe_serie_NF")]
        [Required]
        public int NFe_serie_NF { get; set; }
        //[old_NFe_numero_NF] esta coluna mudou o nome na base de homologação deles
        [Column("old_NFe_numero_NF")]
        [Required]
        public int NFe_numero_NF { get; set; }

        [Column("NFe_T1_servidor_BD")]
        [MaxLength(160)]
        public string NFe_T1_servidor_BD { get; set; }

        [Column("NFe_T1_nome_BD")]
        [MaxLength(40)]
        public string NFe_T1_nome_BD { get; set; }

        [Column("NFe_T1_usuario_BD")]
        [MaxLength(40)]
        public string NFe_T1_usuario_BD { get; set; }

        [Column("NFe_T1_senha_BD")]
        [MaxLength(160)]
        public string NFe_T1_senha_BD { get; set; }

        [Column("dt_cadastro")]
        [Required]
        public DateTime Dt_Cadastro { get; set; }

        [Column("dt_hr_cadastro")]
        [Required]
        public DateTime Dt_hr_cadastro { get; set; }

        [Column("usuario_cadastro")]
        [MaxLength(10)]
        [Required]
        public string Usuario_Cadastro { get; set; }

        [Column("dt_ult_atualizacao")]
        [Required]
        public DateTime Dt_Ult_Atualizacao { get; set; }

        [Column("dt_hr_ult_atualizacao")]
        [Required]
        public DateTime Dt_Hr_Ult_Atualizacao { get; set; }

        [Column("usuario_ult_atualizacao")]
        [Required]
        [MaxLength(10)]
        public string Usuario_Ult_Atualizacao { get; set; }

        [Column("st_habilitado_ctrl_estoque")]
        [Required]
        public byte St_Habilitado_Ctrl_Estoque { get; set; }

        [Column("ordem")]
        [Required]
        public short Ordem { get; set; }

        [Column("texto_fixo_especifico")]
        [MaxLength(2000)]
        public string Texto_Fixo_Especifico { get; set; }

    }
}
