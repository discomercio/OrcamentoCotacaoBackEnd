﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.ClienteMagentoDto
{
    public class EnderecoCadastralClienteMagentoDto
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

        /// <summary>
        /// Endereco_tipo_pessoa = "PF", "PJ"
        /// <hr />
        /// </summary>
        [MaxLength(2)]
        public string Endereco_tipo_pessoa { get; set; }

        [MaxLength(14)]
        public string Endereco_cnpj_cpf { get; set; }

        /*
         * 
Ao chegar um pedido, se o cliente não exisditr, cadstramos ele imediatamente.
Ao cadastrar o cliente:
- se for PF, assumimos Endereco_produtor_rural_status = COD_ST_CLIENTE_PRODUTOR_RURAL_NAO e Endereco_contribuinte_icms_status = NAO
- se for PJ, deixar o pedido st_etg_imediata = 1 (não)
	e colocar Endereco_contribuinte_icms_status = inicial, Endereco_ie = vazio

        public byte Endereco_contribuinte_icms_status { get; set; }

        public byte Endereco_produtor_rural_status { get; set; }

        [MaxLength(20)]
        public string Endereco_ie { get; set; }

        [MaxLength(20)]
        public string Endereco_rg { get; set; }
*/

        [MaxLength(30)]
        public string Endereco_contato { get; set; }
    }
}
