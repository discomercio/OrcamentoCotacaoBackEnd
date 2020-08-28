﻿using InfraBanco.Constantes;
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

        public static Cliente.Dados.EnderecoEntregaClienteCadastroDados EnderecoEntregaDeEnderecoEntregaClienteMagentoDto(EnderecoEntregaClienteMagentoDto endEtg, bool outroEnd)
        {
            Cliente.Dados.EnderecoEntregaClienteCadastroDados ret = new Cliente.Dados.EnderecoEntregaClienteCadastroDados();
            ret.OutroEndereco = outroEnd;

            if (outroEnd)
            {
                ret.EndEtg_endereco = endEtg.EndEtg_endereco;
                ret.EndEtg_endereco_numero = endEtg.EndEtg_endereco_numero;
                ret.EndEtg_endereco_complemento = endEtg.EndEtg_endereco_complemento;
                ret.EndEtg_bairro = endEtg.EndEtg_bairro;
                ret.EndEtg_cidade = endEtg.EndEtg_cidade;
                ret.EndEtg_uf = endEtg.EndEtg_uf;
                ret.EndEtg_cep = endEtg.EndEtg_cep;
                //ret.EndEtg_cod_justificativa = endEtg.EndEtg_cod_justificativa; **Verificar o que passar para esse campo
                ret.EndEtg_email = endEtg.EndEtg_email;
                ret.EndEtg_email_xml = endEtg.EndEtg_email_xml;
                ret.EndEtg_nome = endEtg.EndEtg_nome;
                ret.EndEtg_ddd_res = endEtg.EndEtg_ddd_res;
                ret.EndEtg_tel_res = endEtg.EndEtg_tel_res;
                ret.EndEtg_ddd_com = endEtg.EndEtg_ddd_com;
                ret.EndEtg_tel_com = endEtg.EndEtg_tel_com;
                ret.EndEtg_ramal_com = endEtg.EndEtg_ramal_com;
                ret.EndEtg_ddd_cel = endEtg.EndEtg_ddd_cel;
                ret.EndEtg_tel_cel = endEtg.EndEtg_tel_cel;
                ret.EndEtg_ddd_com_2 = endEtg.EndEtg_ddd_com_2;
                ret.EndEtg_tel_com_2 = endEtg.EndEtg_tel_com_2;
                ret.EndEtg_ramal_com_2 = endEtg.EndEtg_ramal_com_2;
                ret.EndEtg_tipo_pessoa = endEtg.EndEtg_tipo_pessoa;
                ret.EndEtg_cnpj_cpf = endEtg.EndEtg_cnpj_cpf;
                ret.EndEtg_produtor_rural_status = endEtg.EndEtg_tipo_pessoa == Constantes.ID_PF ?
                    (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO :
                    (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL;
                ret.EndEtg_contribuinte_icms_status = endEtg.EndEtg_tipo_pessoa == Constantes.ID_PF ?
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO :
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL;
                ret.EndEtg_ie = "";
                ret.EndEtg_rg = "";
            }

            return ret;
        }
    }
}