using InfraBanco.Constantes;
using PrepedidoBusiness.Dto.ClienteCadastro;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UtilsGlobais;

namespace PrepedidoUnisBusiness.UnisDto.ClienteUnisDto
{
    public class EnderecoCadastralClientePrepedidoUnisDto
    {
        [MaxLength(80)]
        [Required]
        public string Endereco_logradouro { get; set; }

        [MaxLength(60)]
        [Required]
        public string Endereco_numero { get; set; }

        [MaxLength(60)]
        public string Endereco_complemento { get; set; }

        [MaxLength(72)]
        [Required]
        public string Endereco_bairro { get; set; }

        [MaxLength(60)]
        [Required]
        public string Endereco_cidade { get; set; }

        [MaxLength(2)]
        [Required]
        public string Endereco_uf { get; set; }

        [MaxLength(8)]
        [Required]
        public string Endereco_cep { get; set; }

        [MaxLength(60)]
        [Required]
        public string Endereco_email { get; set; }

        [MaxLength(60)]
        public string Endereco_email_xml { get; set; }

        [MaxLength(60)]
        [Required]
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
        [Required]
        public string Endereco_tipo_pessoa { get; set; }

        [MaxLength(14)]
        [Required]
        public string Endereco_cnpj_cpf { get; set; }

        /// <summary>
        /// Endereco_contribuinte_icms_status: INICIAL = 0, NAO = 1, SIM = 2, ISENTO = 3
        /// </summary>
        [Required]
        public byte Endereco_contribuinte_icms_status { get; set; }

        /// <summary>
        /// Endereco_produtor_rural_status: COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL = 0, COD_ST_CLIENTE_PRODUTOR_RURAL_NAO = 1, COD_ST_CLIENTE_PRODUTOR_RURAL_SIM = 2
        /// </summary>
        [Required]
        public byte Endereco_produtor_rural_status { get; set; }

        [MaxLength(20)]
        public string Endereco_ie { get; set; }

        [MaxLength(20)]
        public string Endereco_rg { get; set; }

        [MaxLength(30)]
        public string Endereco_contato { get; set; }

        public static EnderecoCadastralClientePrepedidoDto EnderecoCadastralClientePrepedidoDtoDeEnderecoCadastralClientePrepedidoUnisDto(EnderecoCadastralClientePrepedidoUnisDto endCadastral)
        {
            EnderecoCadastralClientePrepedidoDto ret = new EnderecoCadastralClientePrepedidoDto();
            ret.Endereco_logradouro = endCadastral.Endereco_logradouro;
            ret.Endereco_numero = endCadastral.Endereco_numero;
            ret.Endereco_complemento = endCadastral.Endereco_complemento;
            ret.Endereco_bairro = endCadastral.Endereco_bairro;
            ret.Endereco_cidade = endCadastral.Endereco_cidade;
            ret.Endereco_uf = endCadastral.Endereco_uf;
            ret.Endereco_cep = endCadastral.Endereco_cep;
            ret.Endereco_email = endCadastral.Endereco_email;
            ret.Endereco_email_xml = endCadastral.Endereco_email_xml;
            ret.Endereco_nome = endCadastral.Endereco_nome;
            ret.Endereco_ddd_res = endCadastral.Endereco_ddd_res == null ? "" : endCadastral.Endereco_ddd_res;
            ret.Endereco_tel_res = endCadastral.Endereco_tel_res == null ? "" : endCadastral.Endereco_tel_res;
            ret.Endereco_ddd_com = endCadastral.Endereco_ddd_com;
            ret.Endereco_tel_com = endCadastral.Endereco_tel_com;
            ret.Endereco_ramal_com = endCadastral.Endereco_ramal_com;
            ret.Endereco_ddd_cel = endCadastral.Endereco_ddd_cel;
            ret.Endereco_tel_cel = endCadastral.Endereco_tel_cel;
            ret.Endereco_ddd_com_2 = endCadastral.Endereco_ddd_com_2;
            ret.Endereco_tel_com_2 = endCadastral.Endereco_tel_com_2;
            ret.Endereco_ramal_com_2 = endCadastral.Endereco_ramal_com_2;
            ret.Endereco_tipo_pessoa = endCadastral.Endereco_tipo_pessoa;
            ret.Endereco_cnpj_cpf = Util.SoDigitosCpf_Cnpj(endCadastral.Endereco_cnpj_cpf.Trim());

            if(endCadastral.Endereco_tipo_pessoa == Constantes.ID_PF && 
                endCadastral.Endereco_produtor_rural_status !=
                (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
            {
                ret.Endereco_contribuinte_icms_status = (byte)0;
            }
            else
            {
                ret.Endereco_contribuinte_icms_status = endCadastral.Endereco_contribuinte_icms_status;
            }
            
            if(endCadastral.Endereco_tipo_pessoa == Constantes.ID_PJ)
            {
                ret.Endereco_produtor_rural_status = 0;
            }
            else
            {
                ret.Endereco_produtor_rural_status = endCadastral.Endereco_produtor_rural_status;
            }

            if(endCadastral.Endereco_produtor_rural_status !=
                (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM &&
                endCadastral.Endereco_tipo_pessoa == Constantes.ID_PF)
            {
                ret.Endereco_ie = "";
            }
            else
            {
                ret.Endereco_ie = endCadastral.Endereco_ie;
            }
            
            ret.Endereco_rg = endCadastral.Endereco_rg == null ? "" : endCadastral.Endereco_rg;
            ret.Endereco_contato = endCadastral.Endereco_contato;

            return ret;
        }

        public static Cliente.Dados.EnderecoCadastralClientePrepedidoDados EnderecoCadastralClientePrepedidoDadosDeEnderecoCadastralClientePrepedidoUnisDto(EnderecoCadastralClientePrepedidoUnisDto endCadastral)
        {
            Cliente.Dados.EnderecoCadastralClientePrepedidoDados ret = new Cliente.Dados.EnderecoCadastralClientePrepedidoDados();
            ret.Endereco_logradouro = endCadastral.Endereco_logradouro;
            ret.Endereco_numero = endCadastral.Endereco_numero;
            ret.Endereco_complemento = endCadastral.Endereco_complemento;
            ret.Endereco_bairro = endCadastral.Endereco_bairro;
            ret.Endereco_cidade = endCadastral.Endereco_cidade;
            ret.Endereco_uf = endCadastral.Endereco_uf;
            ret.Endereco_cep = endCadastral.Endereco_cep;
            ret.Endereco_email = endCadastral.Endereco_email;
            ret.Endereco_email_xml = endCadastral.Endereco_email_xml;
            ret.Endereco_nome = endCadastral.Endereco_nome;
            ret.Endereco_ddd_res = endCadastral.Endereco_ddd_res == null ? "" : endCadastral.Endereco_ddd_res;
            ret.Endereco_tel_res = endCadastral.Endereco_tel_res == null ? "" : endCadastral.Endereco_tel_res;
            ret.Endereco_ddd_com = endCadastral.Endereco_ddd_com;
            ret.Endereco_tel_com = endCadastral.Endereco_tel_com;
            ret.Endereco_ramal_com = endCadastral.Endereco_ramal_com;
            ret.Endereco_ddd_cel = endCadastral.Endereco_ddd_cel;
            ret.Endereco_tel_cel = endCadastral.Endereco_tel_cel;
            ret.Endereco_ddd_com_2 = endCadastral.Endereco_ddd_com_2;
            ret.Endereco_tel_com_2 = endCadastral.Endereco_tel_com_2;
            ret.Endereco_ramal_com_2 = endCadastral.Endereco_ramal_com_2;
            ret.Endereco_tipo_pessoa = endCadastral.Endereco_tipo_pessoa;
            ret.Endereco_cnpj_cpf = Util.SoDigitosCpf_Cnpj(endCadastral.Endereco_cnpj_cpf.Trim());

            if (endCadastral.Endereco_tipo_pessoa == Constantes.ID_PF &&
                endCadastral.Endereco_produtor_rural_status !=
                (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
            {
                ret.Endereco_contribuinte_icms_status = (byte)0;
            }
            else
            {
                ret.Endereco_contribuinte_icms_status = endCadastral.Endereco_contribuinte_icms_status;
            }

            if (endCadastral.Endereco_tipo_pessoa == Constantes.ID_PJ)
            {
                ret.Endereco_produtor_rural_status = 0;
            }
            else
            {
                ret.Endereco_produtor_rural_status = endCadastral.Endereco_produtor_rural_status;
            }

            if (endCadastral.Endereco_produtor_rural_status !=
                (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM &&
                endCadastral.Endereco_tipo_pessoa == Constantes.ID_PF)
            {
                ret.Endereco_ie = "";
            }
            else
            {
                ret.Endereco_ie = endCadastral.Endereco_ie;
            }

            ret.Endereco_rg = endCadastral.Endereco_rg == null ? "" : endCadastral.Endereco_rg;
            ret.Endereco_contato = endCadastral.Endereco_contato;

            return ret;
        }
    }
}
