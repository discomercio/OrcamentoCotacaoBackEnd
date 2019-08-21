using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_ORCAMENTISTA_E_INDICADOR")]
    public class TorcamentistaEindicador
    {
        [Key]
        [Column("apelido")]
        [MaxLength(20)]
        [Required]
        public string Apelido { get; set; }

        [Column("cnpj_cpf")]
        public string Cnpj_Cpf { get; set; }

        [Column("tipo")]
        [Required]
        [MaxLength(2)]
        public string Tipo { get; set; }

        [Column("ie_rg")]
        [MaxLength(20)]
        public string Ie_Rg { get; set; }

        [Column("razao_social_nome")]
        [MaxLength(60)]
        [Required]
        public string Razao_Social_Nome { get; set; }

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

        [Column("ddd_cel")]
        [MaxLength(2)]
        public string Ddd_Cel { get; set; }

        [Column("tel_cel")]
        [MaxLength(9)]
        public string Tel_Cel { get; set; }

        [Column("contato")]
        [MaxLength(40)]
        public string Contato { get; set; }

        [Column("banco")]
        [MaxLength(4)]
        public string Banco { get; set; }

        [Column("agencia")]
        [MaxLength(8)]
        public string Agencia { get; set; }

        [Column("conta")]
        [MaxLength(12)]
        public string Conta { get; set; }

        [Column("favorecido")]
        [MaxLength(40)]
        public string Favorecido { get; set; }

        [Column("loja")]
        [MaxLength(3)]
        public string Loja { get; set; }

        [Column("vendedor")]
        [MaxLength(10)]
        public string Vendedor { get; set; }

        [Column("hab_acesso_sistema")]
        public short? Hab_Acesso_Sistema { get; set; }

        [Column("status")]
        [MaxLength(1)]
        public string Status { get; set; }

        [Column("senha")]
        [MaxLength(10)]
        public string Senha { get; set; }

        [Column("datastamp")]
        [MaxLength(32)]
        public string Datastamp { get; set; }

        [Column("dt_ult_alteracao_senha")]
        public DateTime? Dt_Ult_Alteracao_Senha { get; set; }

        [Column("dt_cadastro")]
        public DateTime? Dt_Cadastro { get; set; }

        [Column("usuario_cadastro")]
        [MaxLength(10)]
        public string Usuario_Cadastro { get; set; }

        [Column("dt_ult_atualizacao")]
        public DateTime? Dt_Ult_Atualizacao { get; set; }

        [Column("usuario_ult_atualizacao")]
        [MaxLength(10)]
        public string Usuario_Ult_Atualizacao { get; set; }

        [Column("dt_ult_acesso")]
        public DateTime? Dt_Ult_Acesso { get; set; }

        [Column("timestamp")]
        public byte[] Timestamp { get; set; }

        [Column("desempenho_nota")]
        [MaxLength(1)]
        public string Desempenho_Nota { get; set; }

        [Column("desempenho_nota_data")]
        public DateTime? Desempenho_Nota_Data { get; set; }

        [Column("desempenho_nota_usuario")]
        [MaxLength(10)]
        public string Desempenho_Nota_Usuario { get; set; }

        [Column("perc_desagio_RA")]
        public float? Perc_Desagio_RA { get; set; }

        [Column("vl_limite_mensal")]
        [Required]
        public decimal Vl_Limite_Mensal { get; set; }

        [Column("email")]
        [MaxLength(60)]
        public string Email { get; set; }

        [Column("captador")]
        [MaxLength(10)]
        public string Captador { get; set; }

        [Column("checado_status")]
        [Required]
        public short Checado_Status { get; set; }

        [Column("checado_data")]
        public DateTime? Checado_Data { get; set; }

        [Column("checado_usuario")]
        [MaxLength(10)]
        public string Checado_Usuario { get; set; }

        [Column("obs")]
        [MaxLength(500)]
        public string Obs { get; set; }

        [Column("vl_meta")]
        [Required]
        public decimal Vl_Meta { get; set; }

        [Column("UsuarioUltAtualizVlMeta")]
        [MaxLength(10)]
        public string UsuarioUltAtualizVlMeta { get; set; }

        [Column("DtHrUltAtualizVlMeta")]
        public DateTime? DtHrUltAtualizVlMeta { get; set; }

        [Column("endereco_numero")]
        [MaxLength(20)]
        public string Endereco_Numero { get; set; }

        [Column("endereco_complemento")]
        [MaxLength(60)]
        public string Endereco_Complemento { get; set; }

        [Column("permite_RA_status")]
        [Required]
        public short Permite_RA_Status { get; set; }

        [Column("permite_RA_usuario")]
        [MaxLength(10)]
        public string Permite_RA_Usuario { get; set; }

        [Column("permite_RA_data_hora")]
        public DateTime? Permite_RA_Data_Hora { get; set; }

        [Column("forma_como_conheceu_codigo")]
        [MaxLength(20)]
        public string Forma_Como_Conheceu_Codigo { get; set; }

        [Column("forma_como_conheceu_usuario")]
        [MaxLength(10)]
        public string Forma_Como_Conheceu_Usuario { get; set; }

        [Column("forma_como_conheceu_data")]
        public DateTime? Forma_Como_Conheceu_Data { get; set; }

        [Column("forma_como_conheceu_data_hora")]
        public DateTime? Forma_Como_Conheceu_Data_Hora { get; set; }

        [Column("forma_como_conheceu_codigo_anterior")]
        [MaxLength(20)]
        public string Fomra_Como_Conheceu_Codigo_Anterior { get; set; }

        [Column("nome_fantasia")]
        [MaxLength(60)]
        public string Nome_Fantasia { get; set; }

        [Column("tipo_estabelecimento")]
        [Required]
        public short Tipo_Estabelecimento { get; set; }

        [Column("nextel")]
        [MaxLength(15)]
        public string Nextel { get; set; }

        [Column("email2")]
        [MaxLength(60)]
        public string Email2 { get; set; }

        [Column("email3")]
        [MaxLength(60)]
        public string Email3 { get; set; }

        [Column("razao_social_nome_iniciais_em_maiusculas")]
        [MaxLength(60)]
        public string Razao_Social_Nome_Iniciais_Em_Maiusculas { get; set; }

        [Column("st_reg_copiado_automaticamente")]
        [Required]
        public byte St_Reg_Copiado_Automaticamente { get; set; }

        [Column("dt_hr_reg_atualizado_automaticamente")]
        public DateTime? Dt_Hr_Reg_Atualizado_Automaticamente { get; set; }

        [Column("etq_endereco")]
        [MaxLength(80)]
        public string Etq_Endereco { get; set; }

        [Column("etq_endereco_numero")]
        [MaxLength(20)]
        public string Etq_Endereco_Numero { get; set; }

        [Column("etq_endereco_complemento")]
        [MaxLength(60)]
        public string Etq_Endereco_Complemento { get; set; }

        [Column("etq_bairro")]
        [MaxLength(72)]
        public string Etq_Bairro { get; set; }

        [Column("etq_cidade")]
        [MaxLength(60)]
        public string Etq_Cidade { get; set; }

        [Column("etq_uf")]
        [MaxLength(2)]
        public string Etq_Uf { get; set; }

        [Column("etq_cep")]
        [MaxLength(8)]
        public string Etq_Cep { get; set; }

        [Column("etq_email")]
        [MaxLength(60)]
        public string Etq_Email { get; set; }

        [Column("etq_ddd_1")]
        [MaxLength(2)]
        public string Etq_DDD_1 { get; set; }

        [Column("etq_tel_1")]
        [MaxLength(9)]
        public string Etq_Tel_1 { get; set; }

        [Column("etq_ddd_2")]
        [MaxLength(2)]
        public string Etq_DDD_2 { get; set; }

        [Column("etq_tel_2")]
        [MaxLength(9)]
        public string Etq_Tel_2 { get; set; }

        [Column("favorecido_cnpj_cpf")]
        [MaxLength(14)]
        public string Favorecido_Cnpj_Cpf { get; set; }

        [Column("agencia_dv")]
        [MaxLength(1)]
        public string Agencia_Dv { get; set; }

        [Column("conta_operacao")]
        [MaxLength(3)]
        public string Conta_Operacao { get; set; }

        [Column("conta_dv")]
        [MaxLength(2)]
        public string Conta_Dv { get; set; }

        [Column("tipo_conta")]
        [MaxLength(1)]
        public string Tipo_Conta { get; set; }

        [Column("vendedor_dt_ult_atualizacao")]
        public DateTime? Vendedor_Dt_Ult_Atualizacao { get; set; }

        [Column("vendedor_dt_hr_ult_atualizacao")]
        public DateTime? Vendedor_Dt_Hr_Ult_Atualzacao { get; set; }

        [Column("vendedor_usuario_ult_atualizacao")]
        [MaxLength(10)]
        public string Vendedor_Usuario_Ult_Atualizacao { get; set; }

        [Column("responsavel_principal")]
        [MaxLength(60)]
        public string Responsavel_Principal { get; set; }

    }
}