﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.ClienteUnisDto
{
    public class EnderecoCadastralClientePrepedidoUnisDto
    {
        [MaxLength(80)]
        public string Endereco_logradouro { get; set; }

        [MaxLength(20)]
        public string Endereco_numero { get; set; }

        [MaxLength(60)]
        public string Endereco_complemento { get; set; }

        [MaxLength(72)]
        public string Endereco_bairro { get; set; }

        [MaxLength(60)]
        public string Endereco_cidade { get; set; }

        [MaxLength(2)]
        public string Endereco_uf { get; set; }

        [MaxLength(8)]
        public string Endereco_cep { get; set; }

        [MaxLength(60)]
        public string Endereco_email { get; set; }

        [MaxLength(60)]
        public string Endereco_email_xml { get; set; }

        [MaxLength(60)]
        public string Endereco_nome { get; set; }

        [MaxLength(4)]
        public string Endereco_ddd_res { get; set; }

        [MaxLength(11)]
        public string Endereco_tel_res { get; set; }

        [MaxLength(4)]
        public string Endereco_ddd_com { get; set; }

        [MaxLength(11)]
        public string Endereco_tel_com { get; set; }

        [MaxLength(4)]
        public string Endereco_ramal_com { get; set; }

        [MaxLength(2)]
        public string Endereco_ddd_cel { get; set; }

        [MaxLength(9)]
        public string Endereco_tel_cel { get; set; }

        [MaxLength(2)]
        public string Endereco_ddd_com_2 { get; set; }

        [MaxLength(9)]
        public string Endereco_tel_com_2 { get; set; }

        [MaxLength(4)]
        public string Endereco_ramal_com_2 { get; set; }

        [MaxLength(2)]
        public string Endereco_tipo_pessoa { get; set; }

        [MaxLength(14)]
        public string Endereco_cnpj_cpf { get; set; }

        [Required]
        public byte Endereco_contribuinte_icms_status { get; set; }

        [Required]
        public byte Endereco_produtor_rural_status { get; set; }

        [MaxLength(20)]
        public string Endereco_ie { get; set; }

        [MaxLength(20)]
        public string Endereco_rg { get; set; }

        [MaxLength(30)]
        public string Endereco_contato { get; set; }
    }
}
