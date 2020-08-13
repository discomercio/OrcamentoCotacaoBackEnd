using InfraBanco.Constantes;
using PrepedidoBusiness.Dto.ClienteCadastro;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto
{
    public class EnderecoEntregaClienteCadastroUnisDto
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

        public static EnderecoEntregaDtoClienteCadastro EnderecoEntregaDtoClienteCadastroDeEnderecoEntregaClienteCadastroUnisDto(EnderecoEntregaClienteCadastroUnisDto endEtg, bool outroEnd)
        {
            EnderecoEntregaDtoClienteCadastro ret = new EnderecoEntregaDtoClienteCadastro();
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
                ret.EndEtg_cod_justificativa = endEtg.EndEtg_cod_justificativa;
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
                ret.EndEtg_rg = endEtg.EndEtg_rg;

                //teste para cair na validação
                ret.EndEtg_contribuinte_icms_status = endEtg.EndEtg_contribuinte_icms_status;
                ret.EndEtg_produtor_rural_status = endEtg.EndEtg_produtor_rural_status;
                ret.EndEtg_ie = endEtg.EndEtg_ie;

                //if (endEtg.EndEtg_produtor_rural_status != 
                //    (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM &&
                //    endEtg.EndEtg_tipo_pessoa == Constantes.ID_PF)
                //{
                //    ret.EndEtg_contribuinte_icms_status = (byte)0;
                //}
                //else
                //{
                //    ret.EndEtg_contribuinte_icms_status = endEtg.EndEtg_contribuinte_icms_status;
                //}

                //if(endEtg.EndEtg_tipo_pessoa == Constantes.ID_PJ)
                //{
                //    ret.EndEtg_produtor_rural_status = (byte)0;
                //}
                //else
                //{
                //    ret.EndEtg_produtor_rural_status = endEtg.EndEtg_produtor_rural_status;
                //}
                //if (endEtg.EndEtg_produtor_rural_status !=
                //    (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM &&
                //    endEtg.EndEtg_tipo_pessoa == Constantes.ID_PF)
                //{
                //    ret.EndEtg_ie = "";
                //}
                //else
                //{
                //    ret.EndEtg_ie = endEtg.EndEtg_ie;
                //}
                    
                
            }

            return ret;
        }

        public static EnderecoEntregaClienteCadastroUnisDto EnderecoEntregaClienteCadastroUnisDtoDeEnderecoEntregaDtoClienteCadastro(EnderecoEntregaDtoClienteCadastro endEtg)
        {
            var ret = new EnderecoEntregaClienteCadastroUnisDto()
            {
                EndEtg_endereco = endEtg.EndEtg_endereco,
                EndEtg_endereco_numero = endEtg.EndEtg_endereco_numero,
                EndEtg_endereco_complemento = endEtg.EndEtg_endereco_complemento,
                EndEtg_bairro = endEtg.EndEtg_bairro,
                EndEtg_cidade = endEtg.EndEtg_cidade,
                EndEtg_uf = endEtg.EndEtg_uf,
                EndEtg_cep = endEtg.EndEtg_cep,
                EndEtg_cod_justificativa = endEtg.EndEtg_cod_justificativa,
                EndEtg_email = endEtg.EndEtg_email,
                EndEtg_email_xml = endEtg.EndEtg_email_xml,
                EndEtg_nome = endEtg.EndEtg_nome,
                EndEtg_ddd_res = endEtg.EndEtg_ddd_res,
                EndEtg_tel_res = endEtg.EndEtg_tel_res,
                EndEtg_ddd_com = endEtg.EndEtg_ddd_com,
                EndEtg_tel_com = endEtg.EndEtg_tel_com,
                EndEtg_ramal_com = endEtg.EndEtg_ramal_com,
                EndEtg_ddd_cel = endEtg.EndEtg_ddd_cel,
                EndEtg_tel_cel = endEtg.EndEtg_tel_cel,
                EndEtg_ddd_com_2 = endEtg.EndEtg_ddd_com_2,
                EndEtg_tel_com_2 = endEtg.EndEtg_tel_com_2,
                EndEtg_ramal_com_2 = endEtg.EndEtg_ramal_com_2,
                EndEtg_tipo_pessoa = endEtg.EndEtg_tipo_pessoa,
                EndEtg_cnpj_cpf = endEtg.EndEtg_cnpj_cpf,
                EndEtg_contribuinte_icms_status = endEtg.EndEtg_contribuinte_icms_status,
                EndEtg_produtor_rural_status = endEtg.EndEtg_produtor_rural_status,
                EndEtg_ie = endEtg.EndEtg_ie,
                EndEtg_rg = endEtg.EndEtg_rg,
            };
            return ret;
        }
    }
}
