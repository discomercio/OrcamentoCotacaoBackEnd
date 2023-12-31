﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Cliente.Dados
{
    public class DadosClienteCadastroDados
    {
        public string Loja { get; set; }
        public string Indicador_Orcamentista { get; set; }
        public string Vendedor { get; set; }
        public string Id { get; set; }
        public string Cnpj_Cpf { get; set; }
        public string Rg { get; set; }
        public string Ie { get; set; }
        public byte Contribuinte_Icms_Status { get; set; }
        public string Tipo { get; set; }
        public string Observacao_Filiacao { get; set; }
        public string Nome { get; set; }
        public byte ProdutorRural { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
        public string DddResidencial { get; set; }
        public string TelefoneResidencial { get; set; }
        public string DddComercial { get; set; }
        public string TelComercial { get; set; }
        public string Ramal { get; set; }
        public string DddCelular { get; set; }
        public string Celular { get; set; }
        public string TelComercial2 { get; set; }
        public string DddComercial2 { get; set; }
        public string Ramal2 { get; set; }
        public string Email { get; set; }
        public string EmailXml { get; set; }
        public string Contato { get; set; }
        public string UsuarioCadastro { get; set; }
        public int? IdOrcamentoCotacao { get; set; }
        public int? IdIndicadorVendedor { get; set; }
        public float? Perc_max_comissao_padrao { get; set; }
        public float? Perc_max_comissao_e_desconto_padrao { get; set; }

        public static DadosClienteCadastroDados DadosClienteCadastroDadosDeEnderecoCadastralClientePrepedidoDados(
            EnderecoCadastralClientePrepedidoDados endCadastral, string indicadorOrcamentista, string loja, string idCliente, string usuarioCadastro)
        {
            if (endCadastral == null) return null;
            /* Ao criar um novo pré-pedido será permitido que seja alterado os dados do cliente 
             * sem que altere o cadastro do cliente na base de dados, 
             * foi incluído esses novos campos do Dto "EnderecoCadastralClientePrepedidoUnisDto", que recebe os dados de
             * cliente.
             * Estamos passando para o Dto "DadosClienteCadastroDto" para que seja feita a validação de dados dos novos
             * dados que poderão ser alterado
             * 
             */
            var ret = new DadosClienteCadastroDados()
            {
                Id = idCliente,
                Indicador_Orcamentista = (indicadorOrcamentista ?? "").ToUpper(),
                Loja = loja,
                Nome = endCadastral.Endereco_nome,
                Cnpj_Cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(endCadastral.Endereco_cnpj_cpf.Trim()),
                Tipo = endCadastral.Endereco_tipo_pessoa,
                Rg = endCadastral.Endereco_rg == null ? "" : endCadastral.Endereco_rg,
                DddCelular = endCadastral.Endereco_ddd_cel,
                Celular = endCadastral.Endereco_tel_cel,
                DddResidencial = endCadastral.Endereco_ddd_res == null ? "" : endCadastral.Endereco_ddd_res,
                TelefoneResidencial = endCadastral.Endereco_tel_res == null ? "" : endCadastral.Endereco_tel_res,
                DddComercial = endCadastral.Endereco_ddd_com,
                TelComercial = endCadastral.Endereco_tel_com,
                Ramal = endCadastral.Endereco_ramal_com,
                DddComercial2 = endCadastral.Endereco_ddd_com_2,
                TelComercial2 = endCadastral.Endereco_tel_com_2,
                Ramal2 = endCadastral.Endereco_ramal_com_2,
                Ie = endCadastral.Endereco_ie,
                ProdutorRural = endCadastral.Endereco_produtor_rural_status,
                Contribuinte_Icms_Status = endCadastral.Endereco_contribuinte_icms_status,
                Email = endCadastral.Endereco_email,
                EmailXml = endCadastral.Endereco_email_xml,
                Vendedor = string.IsNullOrEmpty(endCadastral.Vendedor) ? "" : endCadastral.Vendedor,// esse campo não é utilizado em TCliente
                Cep = UtilsGlobais.Util.Cep_SoDigito(endCadastral.Endereco_cep),
                Endereco = endCadastral.Endereco_logradouro,
                Numero = endCadastral.Endereco_numero,
                Bairro = endCadastral.Endereco_bairro,
                Cidade = endCadastral.Endereco_cidade,
                Uf = endCadastral.Endereco_uf,
                Complemento = endCadastral.Endereco_complemento,
                Contato = endCadastral.Endereco_contato,
                UsuarioCadastro = !string.IsNullOrEmpty(usuarioCadastro) ? usuarioCadastro : "",
                IdOrcamentoCotacao = endCadastral.IdOrcamentoCotacao,
                IdIndicadorVendedor = endCadastral.IdIndicadorVendedor,
                Perc_max_comissao_padrao = endCadastral.Perc_max_comissao_padrao,
                Perc_max_comissao_e_desconto_padrao = endCadastral.Perc_max_comissao_e_desconto_padrao
            };

            return ret;
        }
    }
}
