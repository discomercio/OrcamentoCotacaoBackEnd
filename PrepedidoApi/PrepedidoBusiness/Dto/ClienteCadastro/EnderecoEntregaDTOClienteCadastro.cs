using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrepedidoBusiness.Dto.ClienteCadastro
{
    public class EnderecoEntregaDtoClienteCadastro
    {
        public bool OutroEndereco { get; set; }
        public string EndEtg_endereco { get; set; }
        public string EndEtg_endereco_numero { get; set; }
        public string EndEtg_endereco_complemento { get; set; }
        public string EndEtg_bairro { get; set; }
        public string EndEtg_cidade { get; set; }
        public string EndEtg_uf { get; set; }
        public string EndEtg_cep { get; set; }
        //codigo da justificativa, preenchdio quando está criando (do spa para a api)
        public string EndEtg_cod_justificativa { get; set; }
        //descrição da justificativa, preenchdio para mostrar (da api para o spa)
        public string EndEtg_descricao_justificativa { get; set; }
        //se foi selecionado um endereco diferente para a entrega (do spa para a api)

        //Novos campos
        public string EndEtg_email { get; set; }
        public string EndEtg_email_xml { get; set; }
        public string EndEtg_nome { get; set; }
        public string EndEtg_ddd_res { get; set; }
        public string EndEtg_tel_res { get; set; }
        public string EndEtg_ddd_com { get; set; }
        public string EndEtg_tel_com { get; set; }
        public string EndEtg_ramal_com { get; set; }
        public string EndEtg_ddd_cel { get; set; }
        public string EndEtg_tel_cel { get; set; }
        public string EndEtg_ddd_com_2 { get; set; }
        public string EndEtg_tel_com_2 { get; set; }
        public string EndEtg_ramal_com_2 { get; set; }
        public string EndEtg_tipo_pessoa { get; set; }
        public string EndEtg_cnpj_cpf { get; set; }
        public byte EndEtg_contribuinte_icms_status { get; set; }
        public byte EndEtg_produtor_rural_status { get; set; }
        public string EndEtg_ie { get; set; }
        public string EndEtg_rg { get; set; }
        public byte St_memorizacao_completa_enderecos { get; set; }

        public static EnderecoEntregaDtoClienteCadastro EnderecoEntregaDtoClienteCadastro_De_EnderecoEntregaClienteCadastroDados(Cliente.Dados.EnderecoEntregaClienteCadastroDados origem)
        {
            var ret = new EnderecoEntregaDtoClienteCadastro()
            {
                OutroEndereco = origem.OutroEndereco,
                EndEtg_endereco = origem.EndEtg_endereco,
                EndEtg_endereco_numero = origem.EndEtg_endereco_numero,
                EndEtg_endereco_complemento = origem.EndEtg_endereco_complemento,
                EndEtg_bairro = origem.EndEtg_bairro,
                EndEtg_cidade = origem.EndEtg_cidade,
                EndEtg_uf = origem.EndEtg_uf,
                EndEtg_cep = origem.EndEtg_cep,
                EndEtg_cod_justificativa = origem.EndEtg_cod_justificativa,
                EndEtg_descricao_justificativa = origem.EndEtg_descricao_justificativa,
                EndEtg_email = origem.EndEtg_email,
                EndEtg_email_xml = origem.EndEtg_email_xml,
                EndEtg_nome = origem.EndEtg_nome,
                EndEtg_ddd_res = origem.EndEtg_ddd_res,
                EndEtg_tel_res = origem.EndEtg_tel_res,
                EndEtg_ddd_com = origem.EndEtg_ddd_com,
                EndEtg_tel_com = origem.EndEtg_tel_com,
                EndEtg_ramal_com = origem.EndEtg_ramal_com,
                EndEtg_ddd_cel = origem.EndEtg_ddd_cel,
                EndEtg_tel_cel = origem.EndEtg_tel_cel,
                EndEtg_ddd_com_2 = origem.EndEtg_ddd_com_2,
                EndEtg_tel_com_2 = origem.EndEtg_tel_com_2,
                EndEtg_ramal_com_2 = origem.EndEtg_ramal_com_2,
                EndEtg_tipo_pessoa = origem.EndEtg_tipo_pessoa,
                EndEtg_cnpj_cpf = origem.EndEtg_cnpj_cpf,
                EndEtg_contribuinte_icms_status = origem.EndEtg_contribuinte_icms_status,
                EndEtg_produtor_rural_status = origem.EndEtg_produtor_rural_status,
                EndEtg_ie = origem.EndEtg_ie,
                EndEtg_rg = origem.EndEtg_rg,
                St_memorizacao_completa_enderecos = origem.St_memorizacao_completa_enderecos
            };
            return ret;
        }

        public static Cliente.Dados.EnderecoEntregaClienteCadastroDados EnderecoEntregaClienteCadastroDados_De_EnderecoEntregaDtoClienteCadastro(EnderecoEntregaDtoClienteCadastro  origem)
        {
            var ret = new Cliente.Dados.EnderecoEntregaClienteCadastroDados()
            {
                OutroEndereco = origem.OutroEndereco,
                EndEtg_endereco = origem.EndEtg_endereco,
                EndEtg_endereco_numero = origem.EndEtg_endereco_numero,
                EndEtg_endereco_complemento = origem.EndEtg_endereco_complemento,
                EndEtg_bairro = origem.EndEtg_bairro,
                EndEtg_cidade = origem.EndEtg_cidade,
                EndEtg_uf = origem.EndEtg_uf,
                EndEtg_cep = origem.EndEtg_cep,
                EndEtg_cod_justificativa = origem.EndEtg_cod_justificativa,
                EndEtg_descricao_justificativa = origem.EndEtg_descricao_justificativa,
                EndEtg_email = origem.EndEtg_email,
                EndEtg_email_xml = origem.EndEtg_email_xml,
                EndEtg_nome = origem.EndEtg_nome,
                EndEtg_ddd_res = origem.EndEtg_ddd_res,
                EndEtg_tel_res = origem.EndEtg_tel_res,
                EndEtg_ddd_com = origem.EndEtg_ddd_com,
                EndEtg_tel_com = origem.EndEtg_tel_com,
                EndEtg_ramal_com = origem.EndEtg_ramal_com,
                EndEtg_ddd_cel = origem.EndEtg_ddd_cel,
                EndEtg_tel_cel = origem.EndEtg_tel_cel,
                EndEtg_ddd_com_2 = origem.EndEtg_ddd_com_2,
                EndEtg_tel_com_2 = origem.EndEtg_tel_com_2,
                EndEtg_ramal_com_2 = origem.EndEtg_ramal_com_2,
                EndEtg_tipo_pessoa = origem.EndEtg_tipo_pessoa,
                EndEtg_cnpj_cpf = origem.EndEtg_cnpj_cpf,
                EndEtg_contribuinte_icms_status = origem.EndEtg_contribuinte_icms_status,
                EndEtg_produtor_rural_status = origem.EndEtg_produtor_rural_status,
                EndEtg_ie = origem.EndEtg_ie,
                EndEtg_rg = origem.EndEtg_rg,
                St_memorizacao_completa_enderecos = origem.St_memorizacao_completa_enderecos
            };
            return ret;
        }
    }
}