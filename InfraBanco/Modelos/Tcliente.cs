using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_CLIENTE")]
    public class Tcliente
    {
        [Column("id")]
        [Required]
        [MaxLength(12)]
        public string Id { get; set; }

        [Column("cnpj_cpf")]
        [MaxLength(14)]
        public string Cnpj_Cpf { get; set; }

        [Column("tipo")]
        [MaxLength(2)]
        public string Tipo { get; set; }

        [Column("ie")]
        [MaxLength(20)]
        public string Ie { get; set; }

        [Column("rg")]
        [MaxLength(20)]
        public string Rg { get; set; }

        [Column("nome")]
        [MaxLength(60)]
        [Required]
        public string Nome { get; set; }

        [Column("sexo")]
        [MaxLength(1)]
        public string Sexo { get; set; }

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

        [Column("ddd_res")]
        [MaxLength(4)]
        public string Ddd_Res { get; set; }

        [Column("tel_res")]
        [MaxLength(11)]
        public string Tel_Res { get; set; }

        [Column("ddd_com")]
        [MaxLength(4)]
        public string Ddd_Com { get; set; }

        [Column("tel_com")]
        [MaxLength(11)]
        public string Tel_Com { get; set; }

        [Column("ramal_com")]
        [MaxLength(4)]
        public string Ramal_Com { get; set; }

        [Column("contato")]
        [MaxLength(30)]
        public string Contato { get; set; }

        [Column("dt_nasc")]
        public DateTime? Dt_Nasc { get; set; }

        [Column("filiacao")]
        [MaxLength(60)]
        public string Filiacao { get; set; }

        [Column("obs_crediticias")]
        [MaxLength(50)]
        public string Obs_crediticias { get; set; }

        [Column("midia")]
        [MaxLength(3)]
        public string Midia { get; set; }

        [Column("email")]
        [MaxLength(60)]
        public string Email { get; set; }

        [Column("dt_cadastro")]
        [Required]
        public DateTime Dt_Cadastro { get; set; }

        [Column("dt_ult_atualizacao")]
        [Required]
        public DateTime Dt_Ult_Atualizacao { get; set; }

        [Column("usuario_cadastro")]
        [MaxLength(20)]
        public string Usuario_Cadastrado { get; set; }

        [Column("usuario_ult_atualizacao")]
        [MaxLength(20)]
        public string Usuario_Ult_Atualizacao { get; set; }

        [Column("indicador")]
        [MaxLength(20)]
        public string Indicador { get; set; }

        [Column("endereco_numero")]
        [MaxLength(20)]
        public string Endereco_Numero { get; set; }

        [Column("endereco_complemento")]
        [MaxLength(60)]
        public string Endereco_Complemento { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("nome_iniciais_em_maiusculas")]
        [MaxLength(60)]
        public string Nome_Iniciais_Em_Maiusculas { get; private set; }

        [Column("contribuinte_icms_status")]
        [Required]
        public byte Contribuinte_Icms_Status { get; set; }

        [Column("contribuinte_icms_data")]
        public DateTime? Contribuinte_Icms_Data { get; set; }

        [Column("contribuinte_icms_data_hora")]
        public DateTime? Contribuinte_Icms_Data_Hora { get; set; }

        [Column("contribuinte_icms_usuario")]
        [MaxLength(20)]
        public string Contribuinte_Icms_Usuario { get; set; }

        [Column("produtor_rural_status")]
        [Required]
        public byte Produtor_Rural_Status { get; set; }

        [Column("produtor_rural_data")]
        public DateTime? Produtor_Rural_Data { get; set; }

        [Column("produtor_rural_data_hora")]
        public DateTime? Produtor_Rural_Data_Hora { get; set; }

        [Column("produtor_rural_usuario")]
        [MaxLength(20)]
        public string Produtor_Rural_Usuario { get; set; }

        [Column("email_xml")]
        [MaxLength(60)]
        public string Email_Xml { get; set; }

        [Column("ddd_cel")]
        [MaxLength(2)]
        public string Ddd_Cel { get; set; }

        [Column("tel_cel")]
        [MaxLength(9)]
        public string Tel_Cel { get; set; }

        [Column("ddd_com_2")]
        [MaxLength(2)]
        public string Ddd_Com_2 { get; set; }

        [Column("tel_com_2")]
        [MaxLength(9)]
        public string Tel_Com_2 { get; set; }

        [Column("ramal_com_2")]
        [MaxLength(4)]
        public string Ramal_Com_2 { get; set; }

        [Column("sistema_responsavel_cadastro")]
        [Required]
        public int Sistema_responsavel_cadastro { get; set; }

        [Column("sistema_responsavel_atualizacao")]
        [Required]
        public int Sistema_responsavel_atualizacao { get; set; }

    }
}