﻿using Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_ORCAMENTISTA_E_INDICADOR")]
    public class TorcamentistaEindicador : IModel
    {
        [Key]
        [Column("apelido")]
        [MaxLength(20)]
        [Required]
        public string Apelido { get; set; }

        [Required]
        [Column("Id")]
        public int IdIndicador { get; set; }

        [Column("cnpj_cpf")]
        [MaxLength(14)]
        public string Cnpj_cpf { get; set; }

        [Column("razao_social_nome")]
        [MaxLength(60)]
        [Required]
        public string Razao_Social_Nome { get; set; }

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

        [Column("dt_ult_atualizacao")]
        public DateTime? Dt_Ult_Atualizacao { get; set; }

        [Column("dt_ult_acesso")]
        public DateTime? Dt_Ult_Acesso { get; set; }

        [Column("permite_RA_status")]
        [Required]
        public short Permite_RA_Status { get; set; }

        [Column("ddd")]
        [MaxLength(4)]
        public string Ddd { get; set; }

        [Column("telefone")]
        [MaxLength(11)]
        public string Telefone { get; set; }

        [Column("ddd_cel")]
        [MaxLength(4)]
        public string Ddd_cel { get; set; }

        [Column("tel_cel")]
        [MaxLength(11)]
        public string Tel_cel { get; set; }

        [Column("perc_desagio_RA")]
        public float? Perc_Desagio_RA { get; set; }

        [Column("vl_limite_mensal", TypeName = "money")]
        [Required]
        public decimal Vl_Limite_Mensal { get; set; }

        [Column("endereco")]
        [MaxLength(80)]
        public string Endereco { get; set; }

        [Column("endereco_numero")]
        [MaxLength(20)]
        public string Endereco_Numero { get; set; }

        [Column("cep")]
        [MaxLength(8)]
        public string Cep { get; set; }

        [Column("bairro")]
        [MaxLength(72)]
        public string Bairro { get; set; }

        [Column("cidade")]
        [MaxLength(60)]
        public string Cidade { get; set; }

        [Column("uf")]
        [MaxLength(2)]
        public string Uf { get; set; }

        [Column("endereco_complemento")]
        [MaxLength(60)]
        public string Endereco_Complemento { get; set; }

        [Column("razao_social_nome_iniciais_em_maiusculas")]
        [MaxLength(60)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Razao_social_nome_iniciais_em_maiusculas { get; set; }

        [Column("QtdeConsecutivaFalhaLogin")]
        public int QtdeConsecutivaFalhaLogin { get; set; }

        [Column("StLoginBloqueadoAutomatico")]
        public bool StLoginBloqueadoAutomatico { get; set; }

        [Column("DataHoraBloqueadoAutomatico")]
        public DateTime? DataHoraBloqueadoAutomatico { get; set; }

        [Column("EnderecoIpBloqueadoAutomatico ")]
        public string EnderecoIpBloqueadoAutomatico { get; set; }

        [Column("nome_fantasia")]
        [MaxLength(60)]
        public string NomeFantasia { get; set; }
    }
}