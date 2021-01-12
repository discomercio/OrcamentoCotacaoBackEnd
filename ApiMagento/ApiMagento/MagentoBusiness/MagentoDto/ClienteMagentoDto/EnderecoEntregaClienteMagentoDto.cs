using InfraBanco.Constantes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.ClienteMagentoDto
{
    public class EnderecoEntregaClienteMagentoDto
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        [Required]
        [MaxLength(60)]
        public string EndEtg_endereco { get; set; }

        [MaxLength(60)]
        [Required]
        public string EndEtg_endereco_numero { get; set; }

        /*
# Colocar a informação do ponto de referência no campo 'Constar na NF'. Comparar o conteúdo do ponto de referência
# com o campo complemento. Se forem iguais, não colocar em 'Constar na NF'. Se o campo complemento exceder o
# tamanho do BD e precisar ser truncado, copiá-lo no campo 'Constar na NF', junto com o ponto de referência.

Por isso, temos o MaxLength 800 aqui
*/
        [MaxLength(800)]
        public string? EndEtg_endereco_complemento { get; set; }

        [Required]
        [MaxLength(60)]
        public string EndEtg_bairro { get; set; }

        [Required]
        [MaxLength(60)]
        public string EndEtg_cidade { get; set; }

        [Required]
        [MaxLength(2)]
        public string EndEtg_uf { get; set; }

        [Required]
        [MaxLength(8)]
        public string EndEtg_cep { get; set; }

        [MaxLength(60)]
        public string? EndEtg_email { get; set; }

        [MaxLength(60)]
        public string? EndEtg_email_xml { get; set; }

        [Required]
        [MaxLength(60)]
        public string EndEtg_nome { get; set; }

        [MaxLength(4)]
        public string? EndEtg_ddd_res { get; set; }

        [MaxLength(11)]
        public string? EndEtg_tel_res { get; set; }

        [MaxLength(4)]
        public string? EndEtg_ddd_com { get; set; }

        [MaxLength(11)]
        public string? EndEtg_tel_com { get; set; }

        [MaxLength(4)]
        public string? EndEtg_ramal_com { get; set; }

        [MaxLength(2)]
        public string? EndEtg_ddd_cel { get; set; }

        [MaxLength(9)]
        public string? EndEtg_tel_cel { get; set; }

        [MaxLength(2)]
        public string? EndEtg_ddd_com_2 { get; set; }

        [MaxLength(9)]
        public string? EndEtg_tel_com_2 { get; set; }

        [MaxLength(4)]
        public string? EndEtg_ramal_com_2 { get; set; }

        /// <summary>
        /// EndEtg_tipo_pessoa = "PF", "PJ"
        /// <hr />
        /// </summary>
        [Required]
        [MaxLength(2)]
        public string EndEtg_tipo_pessoa { get; set; }

        [Required]
        [MaxLength(14)]
        public string EndEtg_cnpj_cpf { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.


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

        [MaxLength(800)]
        public string? PontoReferencia { get; set; }

        public static Cliente.Dados.EnderecoEntregaClienteCadastroDados EnderecoEntregaDeEnderecoEntregaClienteMagentoDto(
            EnderecoEntregaClienteMagentoDto? endEtg, bool outroEnd, string EndEtg_cod_justificativa)
        {
            Cliente.Dados.EnderecoEntregaClienteCadastroDados ret = new Cliente.Dados.EnderecoEntregaClienteCadastroDados
            {
                OutroEndereco = outroEnd
            };

            if (outroEnd && endEtg != null)
            {
                ret.EndEtg_endereco = endEtg.EndEtg_endereco;
                ret.EndEtg_endereco_numero = endEtg.EndEtg_endereco_numero;
                ret.EndEtg_endereco_complemento = endEtg.EndEtg_endereco_complemento;
                ret.EndEtg_bairro = endEtg.EndEtg_bairro;
                ret.EndEtg_cidade = endEtg.EndEtg_cidade;
                ret.EndEtg_uf = endEtg.EndEtg_uf;
                ret.EndEtg_cep = endEtg.EndEtg_cep;
                ret.EndEtg_cod_justificativa = EndEtg_cod_justificativa;
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
                    (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO :
                    (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL;
                ret.EndEtg_contribuinte_icms_status = endEtg.EndEtg_tipo_pessoa == Constantes.ID_PF ?
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL :
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL; //inicial nos dois casos
                ret.EndEtg_ie = "";
                ret.EndEtg_rg = "";
            }

            return ret;
        }
    }
}
