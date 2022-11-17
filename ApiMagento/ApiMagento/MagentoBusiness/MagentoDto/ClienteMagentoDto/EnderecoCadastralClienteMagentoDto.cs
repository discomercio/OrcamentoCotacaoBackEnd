using InfraBanco.Constantes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

#nullable enable

namespace MagentoBusiness.MagentoDto.ClienteMagentoDto
{
    public class EnderecoCadastralClienteMagentoDto
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        [Required]
        [MaxLength(60)]
        public string Endereco_logradouro { get; set; }

        [Required]
        [MaxLength(60)]
        public string Endereco_numero { get; set; }

        /*
# Colocar a informação do ponto de referência no campo 'Constar na NF'. Comparar o conteúdo do ponto de referência
# com o campo complemento. Se forem iguais, não colocar em 'Constar na NF'. Se o campo complemento exceder o
# tamanho do BD e precisar ser truncado, copiá-lo no campo 'Constar na NF', junto com o ponto de referência.

Por isso, temos o MaxLength 800 aqui
*/
        [MaxLength(800)]
        public string? Endereco_complemento { get; set; }

        [Required]
        [MaxLength(60)]
        public string Endereco_bairro { get; set; }

        [Required]
        [MaxLength(60)]
        public string Endereco_cidade { get; set; }

        [Required]
        [MaxLength(2)]
        public string Endereco_uf { get; set; }

        [Required]
        [MaxLength(8)]
        public string Endereco_cep { get; set; }

        [MaxLength(60)]
        public string? Endereco_email { get; set; }

        [MaxLength(60)]
        public string? Endereco_email_xml { get; set; }

        [Required]
        [MaxLength(60)]
        public string Endereco_nome { get; set; }

        [MaxLength(4)]
        public string? Endereco_ddd_res { get; set; }

        [MaxLength(11)]
        public string? Endereco_tel_res { get; set; }

        [MaxLength(4)]
        public string? Endereco_ddd_com { get; set; }

        [MaxLength(11)]
        public string? Endereco_tel_com { get; set; }

        [MaxLength(4)]
        public string? Endereco_ramal_com { get; set; }

        [MaxLength(2)]
        public string? Endereco_ddd_cel { get; set; }

        [MaxLength(9)]
        public string? Endereco_tel_cel { get; set; }

        [MaxLength(2)]
        public string? Endereco_ddd_com_2 { get; set; }

        [MaxLength(9)]
        public string? Endereco_tel_com_2 { get; set; }

        [MaxLength(4)]
        public string? Endereco_ramal_com_2 { get; set; }

        /// <summary>
        /// Endereco_tipo_pessoa = "PF", "PJ". No momento somente é aceito PF.
        /// <hr />
        /// </summary>
        [Required]
        [MaxLength(2)]
        public string Endereco_tipo_pessoa { get; set; }

        [Required]
        [MaxLength(14)]
        public string Endereco_cnpj_cpf { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        /*
         * 
Ao chegar um pedido, se o cliente não exisditr, cadstramos ele imediatamente.
Ao cadastrar o cliente:
- se for PF, assumimos Endereco_produtor_rural_status = COD_ST_CLIENTE_PRODUTOR_RURAL_NAO e Endereco_contribuinte_icms_status = INICIAL
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
        public string? Endereco_contato { get; set; }

        [MaxLength(800)]
        public string? PontoReferencia { get; set; }


        public static Cliente.Dados.EnderecoCadastralClientePrepedidoDados EnderecoCadastralClientePrepedidoDados_De_EnderecoCadastralClienteMagentoDto(EnderecoCadastralClienteMagentoDto endCadastralMagento)
        {
#pragma warning disable IDE0017 // Simplify object initialization
            Cliente.Dados.EnderecoCadastralClientePrepedidoDados ret = new Cliente.Dados.EnderecoCadastralClientePrepedidoDados();
#pragma warning restore IDE0017 // Simplify object initialization
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
            ret.Endereco_ddd_res = endCadastralMagento.Endereco_ddd_res ?? "";
            ret.Endereco_tel_res = endCadastralMagento.Endereco_tel_res ?? "";
            ret.Endereco_ddd_com = endCadastralMagento.Endereco_ddd_com ?? "";
            ret.Endereco_tel_com = endCadastralMagento.Endereco_tel_com ?? "";
            ret.Endereco_ramal_com = endCadastralMagento.Endereco_ramal_com ?? "";
            ret.Endereco_ddd_cel = endCadastralMagento.Endereco_ddd_cel ?? "";
            ret.Endereco_tel_cel = endCadastralMagento.Endereco_tel_cel ?? "";
            ret.Endereco_ddd_com_2 = endCadastralMagento.Endereco_ddd_com_2 ?? "";
            ret.Endereco_tel_com_2 = endCadastralMagento.Endereco_tel_com_2 ?? "";
            ret.Endereco_ramal_com_2 = endCadastralMagento.Endereco_ramal_com_2 ?? "";
            ret.Endereco_tipo_pessoa = endCadastralMagento.Endereco_tipo_pessoa;
            ret.Endereco_cnpj_cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(endCadastralMagento.Endereco_cnpj_cpf.Trim());
            ret.Endereco_produtor_rural_status = endCadastralMagento.Endereco_tipo_pessoa == Constantes.ID_PF ?
                (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO :
                (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL;
            ret.Endereco_contribuinte_icms_status = endCadastralMagento.Endereco_tipo_pessoa == Constantes.ID_PF ?
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL :
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL; //inicial nos dois casos
            ret.Endereco_ie = "";
            ret.Endereco_rg = "";

            return ret;
        }

        public static Cliente.Dados.DadosClienteCadastroDados DadosClienteDeEnderecoCadastralClienteMagentoDto(
            EnderecoCadastralClienteMagentoDto dadosClienteMagento, string loja,
            string vendedor, string orcamentista, string id_cliente)
        {
            var ret = new Cliente.Dados.DadosClienteCadastroDados()
            {
                Id = id_cliente,
                Indicador_Orcamentista = orcamentista,
                Loja = loja,
                Vendedor = vendedor,
                Nome = dadosClienteMagento.Endereco_nome,
                Cnpj_Cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(dadosClienteMagento.Endereco_cnpj_cpf.Trim()),
                Tipo = dadosClienteMagento.Endereco_tipo_pessoa,
                Rg = "",
                DddCelular = dadosClienteMagento.Endereco_ddd_cel,
                Celular = dadosClienteMagento.Endereco_tel_cel,
                DddResidencial = dadosClienteMagento.Endereco_ddd_res ?? "",
                TelefoneResidencial = dadosClienteMagento.Endereco_tel_res ?? "",
                DddComercial = dadosClienteMagento.Endereco_ddd_com,
                TelComercial = dadosClienteMagento.Endereco_tel_com,
                Ramal = dadosClienteMagento.Endereco_ramal_com,
                DddComercial2 = dadosClienteMagento.Endereco_ddd_com_2,
                TelComercial2 = dadosClienteMagento.Endereco_tel_com_2,
                Ramal2 = dadosClienteMagento.Endereco_ramal_com_2,
                /*
                - se for PF, assumimos Endereco_produtor_rural_status = COD_ST_CLIENTE_PRODUTOR_RURAL_NAO e Endereco_contribuinte_icms_status = INICIAL
                - se for PJ, deixar o pedido st_etg_imediata = 1 (não)
                    e colocar Endereco_contribuinte_icms_status = inicial, Endereco_ie = vazio
                */
                Ie = "",
                ProdutorRural = dadosClienteMagento.Endereco_tipo_pessoa == Constantes.ID_PF ?
                    (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO :
                    (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL,
                Contribuinte_Icms_Status = dadosClienteMagento.Endereco_tipo_pessoa == Constantes.ID_PF ?
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL :
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL, //inicial nos dois casos
                Email = dadosClienteMagento.Endereco_email,
                EmailXml = dadosClienteMagento.Endereco_email_xml,
                Cep = dadosClienteMagento.Endereco_cep,
                Endereco = dadosClienteMagento.Endereco_logradouro,
                Numero = dadosClienteMagento.Endereco_numero,
                Bairro = dadosClienteMagento.Endereco_bairro,
                Cidade = dadosClienteMagento.Endereco_cidade,
                Uf = dadosClienteMagento.Endereco_uf,
                Complemento = dadosClienteMagento.Endereco_complemento,
                Contato = dadosClienteMagento.Endereco_contato
            };

            return ret;
        }

        public static EnderecoCadastralClienteMagentoDto EnderecoCadastralClienteMagentoDto_De_EnderecoEntregaClienteMagentoDto(
            EnderecoEntregaClienteMagentoDto origem, EnderecoCadastralClienteMagentoDto? ret)
        {
            ret ??= new EnderecoCadastralClienteMagentoDto();
            ret.Endereco_logradouro = origem.EndEtg_endereco;
            ret.Endereco_numero = origem.EndEtg_endereco_numero;
            ret.Endereco_complemento = origem.EndEtg_endereco_complemento;
            ret.Endereco_bairro = origem.EndEtg_bairro;
            ret.Endereco_cidade = origem.EndEtg_cidade;
            ret.Endereco_uf = origem.EndEtg_uf;
            ret.Endereco_cep = origem.EndEtg_cep;
            ret.Endereco_email = origem.EndEtg_email;
            ret.Endereco_email_xml = origem.EndEtg_email_xml;
            ret.Endereco_nome = origem.EndEtg_nome;
            ret.Endereco_ddd_res = origem.EndEtg_ddd_res;
            ret.Endereco_tel_res = origem.EndEtg_tel_res;
            ret.Endereco_ddd_com = origem.EndEtg_ddd_com;
            ret.Endereco_tel_com = origem.EndEtg_tel_com;
            ret.Endereco_ramal_com = origem.EndEtg_ramal_com;
            ret.Endereco_ddd_cel = origem.EndEtg_ddd_cel;
            ret.Endereco_tel_cel = origem.EndEtg_tel_cel;
            ret.Endereco_ddd_com_2 = origem.EndEtg_ddd_com_2;
            ret.Endereco_tel_com_2 = origem.EndEtg_tel_com_2;
            ret.Endereco_ramal_com_2 = origem.EndEtg_ramal_com_2;
            ret.Endereco_tipo_pessoa = origem.EndEtg_tipo_pessoa;
            ret.Endereco_cnpj_cpf = origem.EndEtg_cnpj_cpf;
            //este não temos: ret.Endereco_contato 
            //vamos truncar origem.EndEtg_endereco_complemento
            if (origem.EndEtg_endereco_complemento?.Length > Constantes.MAX_TAMANHO_CAMPO_ENDERECO_COMPLEMENTO)
                ret.Endereco_complemento = origem.EndEtg_endereco_complemento.Substring(0, Constantes.MAX_TAMANHO_CAMPO_ENDERECO_COMPLEMENTO);
            
            ret.PontoReferencia = origem.PontoReferencia;
            return ret;
        }

    }
}
