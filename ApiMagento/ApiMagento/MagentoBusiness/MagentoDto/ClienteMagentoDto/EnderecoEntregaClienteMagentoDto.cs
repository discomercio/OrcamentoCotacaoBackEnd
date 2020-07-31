using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.ClienteMagentoDto
{
    public class EnderecoEntregaClienteMagentoDto
    {
        [MaxLength(80)]
        public string EndEtg_endereco { get; set; }

        [MaxLength(20)]
        public string EndEtg_endereco_numero { get; set; }

        [MaxLength(60)]
        public string EndEtg_endereco_complemento { get; set; }

        [MaxLength(72)]
        public string EndEtg_bairro { get; set; }

        [MaxLength(60)]
        public string EndEtg_cidade { get; set; }

        [MaxLength(2)]
        public string EndEtg_uf { get; set; }

        [MaxLength(8)]
        public string EndEtg_cep { get; set; }

        [MaxLength(60)]
        public string EndEtg_email { get; set; }

        [MaxLength(60)]
        public string EndEtg_email_xml { get; set; }

        [MaxLength(60)]
        public string EndEtg_nome { get; set; }

        [MaxLength(4)]
        public string EndEtg_ddd_res { get; set; }

        [MaxLength(11)]
        public string EndEtg_tel_res { get; set; }

        [MaxLength(4)]
        public string EndEtg_ddd_com { get; set; }

        [MaxLength(11)]
        public string EndEtg_tel_com { get; set; }

        [MaxLength(4)]
        public string EndEtg_ramal_com { get; set; }

        [MaxLength(2)]
        public string EndEtg_ddd_cel { get; set; }

        [MaxLength(9)]
        public string EndEtg_tel_cel { get; set; }

        [MaxLength(2)]
        public string EndEtg_ddd_com_2 { get; set; }

        [MaxLength(9)]
        public string EndEtg_tel_com_2 { get; set; }

        [MaxLength(4)]
        public string EndEtg_ramal_com_2 { get; set; }

        /// <summary>
        /// EndEtg_tipo_pessoa = "PF", "PJ"
        /// <hr />
        /// </summary>
        [MaxLength(2)]
        public string EndEtg_tipo_pessoa { get; set; }

        [MaxLength(14)]
        public string EndEtg_cnpj_cpf { get; set; }

        /*
         * 
estes 4 campos não são enviados pelo magento
se o cliente for PJ, o pedido terá st_etg_imediata = não e esses campos serão alimetnados manualemtne de alguma forma
se o clinte for PF, estes campos são desnecessários 
        [Required]
        public byte EndEtg_contribuinte_icms_status { get; set; }

        [Required]
        public byte EndEtg_produtor_rural_status { get; set; }

        [MaxLength(20)]
        public string EndEtg_ie { get; set; }

        [MaxLength(20)]
        public string EndEtg_rg { get; set; }
        */

    }
}
