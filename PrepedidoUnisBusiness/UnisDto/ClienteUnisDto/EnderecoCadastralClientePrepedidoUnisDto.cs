using PrepedidoBusiness.Dto.ClienteCadastro;
using PrepedidoBusiness.Utils;
using System;
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

        /// <summary>
        /// Endereco_tipo_pessoa = "PF", "PJ"
        /// </summary>
        [MaxLength(2)]
        public string Endereco_tipo_pessoa { get; set; }

        [MaxLength(14)]
        public string Endereco_cnpj_cpf { get; set; }

        /// <summary>
        /// Endereco_contribuinte_icms_status: INICIAL = 0, NAO = 1, SIM = 2, ISENTO = 3
        /// </summary>
        public byte Endereco_contribuinte_icms_status { get; set; }

        /// <summary>
        /// Endereco_produtor_rural_status: COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL = 0, COD_ST_CLIENTE_PRODUTOR_RURAL_NAO = 1, COD_ST_CLIENTE_PRODUTOR_RURAL_SIM = 2
        /// </summary>
        public byte Endereco_produtor_rural_status { get; set; }

        [MaxLength(20)]
        public string Endereco_ie { get; set; }

        [MaxLength(20)]
        public string Endereco_rg { get; set; }

        [MaxLength(30)]
        public string Endereco_contato { get; set; }

        public static EnderecoCadastralClientePrepedidoDto EnderecoCadastralClientePrepedidoDtoDeEnderecoCadastralClientePrepedidoUnisDto(EnderecoCadastralClientePrepedidoUnisDto endCadastral)
        {
            var ret = new EnderecoCadastralClientePrepedidoDto()
            {
                Endereco_logradouro = endCadastral.Endereco_logradouro,
                Endereco_numero = endCadastral.Endereco_numero,
                Endereco_complemento = endCadastral.Endereco_complemento,
                Endereco_bairro = endCadastral.Endereco_bairro,
                Endereco_cidade = endCadastral.Endereco_cidade,
                Endereco_uf = endCadastral.Endereco_uf,
                Endereco_cep = endCadastral.Endereco_cep,
                Endereco_email = endCadastral.Endereco_email,
                Endereco_email_xml = endCadastral.Endereco_email_xml,
                Endereco_nome = endCadastral.Endereco_nome,
                Endereco_ddd_res = endCadastral.Endereco_ddd_res == null ? "" : endCadastral.Endereco_ddd_res,
                Endereco_tel_res = endCadastral.Endereco_tel_res == null ? "" : endCadastral.Endereco_tel_res,
                Endereco_ddd_com = endCadastral.Endereco_ddd_com,
                Endereco_tel_com = endCadastral.Endereco_tel_com,
                Endereco_ramal_com = endCadastral.Endereco_ramal_com,
                Endereco_ddd_cel = endCadastral.Endereco_ddd_cel,
                Endereco_tel_cel = endCadastral.Endereco_tel_cel,
                Endereco_ddd_com_2 = endCadastral.Endereco_ddd_com_2,
                Endereco_tel_com_2 = endCadastral.Endereco_tel_com_2,
                Endereco_ramal_com_2 = endCadastral.Endereco_ramal_com_2,
                Endereco_tipo_pessoa = endCadastral.Endereco_tipo_pessoa,
                Endereco_cnpj_cpf = Util.SoDigitosCpf_Cnpj(endCadastral.Endereco_cnpj_cpf.Trim()),
                Endereco_contribuinte_icms_status = endCadastral.Endereco_contribuinte_icms_status,
                Endereco_produtor_rural_status = endCadastral.Endereco_produtor_rural_status,
                Endereco_ie = endCadastral.Endereco_ie,
                Endereco_rg = endCadastral.Endereco_rg == null ? "" : endCadastral.Endereco_rg,
                Endereco_contato = endCadastral.Endereco_contato
            };

            return ret;
        }
    }
}
