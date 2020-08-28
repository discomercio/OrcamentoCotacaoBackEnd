using InfraBanco.Constantes;
using System;
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

        public static Cliente.Dados.EnderecoCadastralClientePrepedidoDados EnderecoCadastralClientePrepedidoDados_De_EnderecoCadastralClienteMagentoDto(EnderecoCadastralClienteMagentoDto endCadastralMagento)
        {
            Cliente.Dados.EnderecoCadastralClientePrepedidoDados ret = new Cliente.Dados.EnderecoCadastralClientePrepedidoDados();
            ret.Endereco_logradouro = endCadastralMagento.Endereco_logradouro;
            ret.Endereco_numero = endCadastralMagento.Endereco_numero;
            ret.Endereco_complemento = endCadastralMagento.Endereco_complemento;
            ret.Endereco_bairro = endCadastralMagento.Endereco_bairro;
            ret.Endereco_cidade = endCadastralMagento.Endereco_cidade;
            ret.Endereco_uf = endCadastralMagento.Endereco_uf;
            ret.Endereco_cep = endCadastralMagento.Endereco_cep;
            ret.Endereco_email = endCadastralMagento.Endereco_email;
            ret.Endereco_email_xml = endCadastralMagento.Endereco_email_xml;
            ret.Endereco_nome = endCadastralMagento.Endereco_nome;
            ret.Endereco_ddd_res = endCadastralMagento.Endereco_ddd_res == null ? "" : endCadastralMagento.Endereco_ddd_res;
            ret.Endereco_tel_res = endCadastralMagento.Endereco_tel_res == null ? "" : endCadastralMagento.Endereco_tel_res;
            ret.Endereco_ddd_com = endCadastralMagento.Endereco_ddd_com;
            ret.Endereco_tel_com = endCadastralMagento.Endereco_tel_com;
            ret.Endereco_ramal_com = endCadastralMagento.Endereco_ramal_com;
            ret.Endereco_ddd_cel = endCadastralMagento.Endereco_ddd_cel;
            ret.Endereco_tel_cel = endCadastralMagento.Endereco_tel_cel;
            ret.Endereco_ddd_com_2 = endCadastralMagento.Endereco_ddd_com_2;
            ret.Endereco_tel_com_2 = endCadastralMagento.Endereco_tel_com_2;
            ret.Endereco_ramal_com_2 = endCadastralMagento.Endereco_ramal_com_2;
            ret.Endereco_tipo_pessoa = endCadastralMagento.Endereco_tipo_pessoa;
            ret.Endereco_cnpj_cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(endCadastralMagento.Endereco_cnpj_cpf.Trim());
            ret.Endereco_produtor_rural_status = endCadastralMagento.Endereco_tipo_pessoa == Constantes.ID_PF ?
                (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO : 
                (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL;
            ret.Endereco_contribuinte_icms_status = endCadastralMagento.Endereco_tipo_pessoa == Constantes.ID_PF ?
                (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO :
                (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL;
            ret.Endereco_ie = "";
            ret.Endereco_rg = "";

            return ret;
        }
    }
}
