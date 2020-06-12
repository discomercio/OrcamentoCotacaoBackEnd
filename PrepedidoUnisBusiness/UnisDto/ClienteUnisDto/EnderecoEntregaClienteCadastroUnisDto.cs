using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto
{
    public class EnderecoEntregaClienteCadastroUnisDto
    {
        [Required]
        public bool OutroEndereco { get; set; }

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

        /// <summary>
        /// EndEtg_cod_justificativa: 
        ///     Casa de Veraneio = 1,
        ///     Doação = 2,
        ///     Nova Unidade da Empresa/filial = 3,
        ///     Parente do Proprietário (Pais, Filhos e Irmãos) = 4,
        ///     Residência do Proprietário = 5,
        ///     Endereço Comercial do Proprietário = 6,
        ///     Endereço da Obra = 8,
        ///     Endereço Novo Cliente = 9,
        ///     Acerto Interno = 10        
        /// </summary>
        [MaxLength(3)]
        public string EndEtg_cod_justificativa { get; set; }
        
        //Novo campos

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
        /// </summary>
        [MaxLength(2)]
        public string EndEtg_tipo_pessoa { get; set; }

        [MaxLength(14)]
        public string EndEtg_cnpj_cpf { get; set; }

        /// <summary>
        /// EndEtg_contribuinte_icms_status = 0, NAO = 1, SIM = 2, ISENTO = 3
        /// </summary>
        [Required]
        public byte EndEtg_contribuinte_icms_status { get; set; }

        /// <summary>
        /// EndEtg_produtor_rural_status: COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL = 0, COD_ST_CLIENTE_PRODUTOR_RURAL_NAO = 1, COD_ST_CLIENTE_PRODUTOR_RURAL_SIM = 2
        /// </summary>
        [Required]
        public byte EndEtg_produtor_rural_status { get; set; }

        [MaxLength(20)]
        public string EndEtg_ie { get; set; }

        [MaxLength(20)]
        public string EndEtg_rg { get; set; }
    }
}
