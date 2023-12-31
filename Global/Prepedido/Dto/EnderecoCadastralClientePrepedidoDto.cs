﻿using Cliente.Dados;
using InfraBanco.Constantes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prepedido.Dto
{
    public class EnderecoCadastralClientePrepedidoDto
    {
        public bool St_memorizacao_completa_enderecos { get; set; }
        public string Endereco_logradouro { get; set; }
        public string Endereco_numero { get; set; }
        public string Endereco_complemento { get; set; }
        public string Endereco_bairro { get; set; }
        public string Endereco_cidade { get; set; }
        public string Endereco_uf { get; set; }
        public string Endereco_cep { get; set; }
        public string Endereco_email { get; set; }
        public string Endereco_email_xml { get; set; }
        public string Endereco_nome { get; set; }
        public string Endereco_ddd_res { get; set; }
        public string Endereco_tel_res { get; set; }
        public string Endereco_ddd_com { get; set; }
        public string Endereco_tel_com { get; set; }
        public string Endereco_ramal_com { get; set; }
        public string Endereco_ddd_cel { get; set; }
        public string Endereco_tel_cel { get; set; }
        public string Endereco_ddd_com_2 { get; set; }
        public string Endereco_tel_com_2 { get; set; }
        public string Endereco_ramal_com_2 { get; set; }
        public string Endereco_tipo_pessoa { get; set; }
        public string Endereco_cnpj_cpf { get; set; }
        public byte Endereco_contribuinte_icms_status { get; set; }
        public byte Endereco_produtor_rural_status { get; set; }
        public string Endereco_ie { get; set; }
        public string Endereco_rg { get; set; }
        public string Endereco_contato { get; set; }
        public int? IdOrcamentoCotacao { get; set; }
        public int? IdIndicadorVendedor { get; set; }
        public float? Perc_max_comissao_padrao { get; set; }
        public float? Perc_max_comissao_e_desconto_padrao { get; set; }
        public string Vendedor { get; set; }
        public static EnderecoCadastralClientePrepedidoDto EnderecoCadastralClientePrepedidoDto_De_EnderecoCadastralClientePrepedidoDados(EnderecoCadastralClientePrepedidoDados origem)
        {
            if (origem == null) return null;
            return new EnderecoCadastralClientePrepedidoDto()
            {
                St_memorizacao_completa_enderecos = origem.St_memorizacao_completa_enderecos,
                Endereco_logradouro = origem.Endereco_logradouro,
                Endereco_numero = origem.Endereco_numero,
                Endereco_complemento = origem.Endereco_complemento,
                Endereco_bairro = origem.Endereco_bairro,
                Endereco_cidade = origem.Endereco_cidade,
                Endereco_uf = origem.Endereco_uf,
                Endereco_cep = origem.Endereco_cep,
                Endereco_email = origem.Endereco_email,
                Endereco_email_xml = origem.Endereco_email_xml,
                Endereco_nome = origem.Endereco_nome,
                Endereco_ddd_res = origem.Endereco_ddd_res,
                Endereco_tel_res = origem.Endereco_tel_res,
                Endereco_ddd_com = origem.Endereco_ddd_com,
                Endereco_tel_com = origem.Endereco_tel_com,
                Endereco_ramal_com = origem.Endereco_ramal_com,
                Endereco_ddd_cel = origem.Endereco_ddd_cel,
                Endereco_tel_cel = origem.Endereco_tel_cel,
                Endereco_ddd_com_2 = origem.Endereco_ddd_com_2,
                Endereco_tel_com_2 = origem.Endereco_tel_com_2,
                Endereco_ramal_com_2 = origem.Endereco_ramal_com_2,
                Endereco_tipo_pessoa = origem.Endereco_tipo_pessoa,
                Endereco_cnpj_cpf = origem.Endereco_cnpj_cpf,
                Endereco_contribuinte_icms_status = origem.Endereco_contribuinte_icms_status,
                Endereco_produtor_rural_status = origem.Endereco_produtor_rural_status,
                Endereco_ie = origem.Endereco_ie,
                Endereco_rg = origem.Endereco_rg,
                Endereco_contato = origem.Endereco_contato,
                IdOrcamentoCotacao = origem.IdOrcamentoCotacao,
                IdIndicadorVendedor = origem.IdIndicadorVendedor,
                Perc_max_comissao_padrao = origem.Perc_max_comissao_e_desconto_padrao,
                Perc_max_comissao_e_desconto_padrao = origem.Perc_max_comissao_padrao,
                Vendedor = origem.Vendedor
            };
        }
        public static EnderecoCadastralClientePrepedidoDados EnderecoCadastralClientePrepedidoDados_De_EnderecoCadastralClientePrepedidoDto(EnderecoCadastralClientePrepedidoDto origem)
        {
            if (origem == null) return null;
            return new EnderecoCadastralClientePrepedidoDados()
            {
                St_memorizacao_completa_enderecos = origem.St_memorizacao_completa_enderecos,
                Endereco_logradouro = origem.Endereco_logradouro,
                Endereco_numero = origem.Endereco_numero,
                Endereco_complemento = origem.Endereco_complemento,
                Endereco_bairro = origem.Endereco_bairro,
                Endereco_cidade = origem.Endereco_cidade,
                Endereco_uf = origem.Endereco_uf,
                Endereco_cep = origem.Endereco_cep,
                Endereco_email = origem.Endereco_email,
                Endereco_email_xml = origem.Endereco_email_xml,
                Endereco_nome = origem.Endereco_nome,
                Endereco_ddd_res = origem.Endereco_ddd_res,
                Endereco_tel_res = origem.Endereco_tel_res,
                Endereco_ddd_com = origem.Endereco_ddd_com,
                Endereco_tel_com = origem.Endereco_tel_com,
                Endereco_ramal_com = origem.Endereco_ramal_com,
                Endereco_ddd_cel = origem.Endereco_ddd_cel,
                Endereco_tel_cel = origem.Endereco_tel_cel,
                Endereco_ddd_com_2 = origem.Endereco_ddd_com_2,
                Endereco_tel_com_2 = origem.Endereco_tel_com_2,
                Endereco_ramal_com_2 = origem.Endereco_ramal_com_2,
                Endereco_tipo_pessoa = origem.Endereco_tipo_pessoa,
                Endereco_cnpj_cpf = origem.Endereco_cnpj_cpf,
                Endereco_contribuinte_icms_status = origem.Endereco_contribuinte_icms_status,
                Endereco_produtor_rural_status = origem.Endereco_produtor_rural_status,
                Endereco_ie = origem.Endereco_ie,
                Endereco_rg = origem.Endereco_rg,
                Endereco_contato = origem.Endereco_contato,
                IdOrcamentoCotacao = origem.IdOrcamentoCotacao,
                IdIndicadorVendedor = origem.IdIndicadorVendedor,
                Perc_max_comissao_padrao = origem.Perc_max_comissao_e_desconto_padrao,
                Perc_max_comissao_e_desconto_padrao = origem.Perc_max_comissao_padrao,
                Vendedor = origem.Vendedor
            };
        }
        public static EnderecoCadastralClientePrepedidoDto EnderecoCadastralClientePrepedidoDto_De_DadosClienteCadastroDto(DadosClienteCadastroDto dadosClienteCadastroDto)
        {
            EnderecoCadastralClientePrepedidoDto enderecoCadastralClientePrepedidoDto = new EnderecoCadastralClientePrepedidoDto();

            enderecoCadastralClientePrepedidoDto.Endereco_cep = dadosClienteCadastroDto.Cep;
            enderecoCadastralClientePrepedidoDto.Endereco_logradouro = dadosClienteCadastroDto.Endereco;
            enderecoCadastralClientePrepedidoDto.Endereco_numero = dadosClienteCadastroDto.Numero;
            enderecoCadastralClientePrepedidoDto.Endereco_bairro = dadosClienteCadastroDto.Bairro;
            enderecoCadastralClientePrepedidoDto.Endereco_cidade = dadosClienteCadastroDto.Cidade;
            enderecoCadastralClientePrepedidoDto.Endereco_uf = dadosClienteCadastroDto.Uf;
            enderecoCadastralClientePrepedidoDto.Endereco_complemento = dadosClienteCadastroDto.Complemento;
            enderecoCadastralClientePrepedidoDto.Endereco_cnpj_cpf = dadosClienteCadastroDto.Cnpj_Cpf;
            enderecoCadastralClientePrepedidoDto.Endereco_nome = dadosClienteCadastroDto.Nome;
            enderecoCadastralClientePrepedidoDto.Endereco_rg = dadosClienteCadastroDto.Rg;
            enderecoCadastralClientePrepedidoDto.Endereco_tipo_pessoa = dadosClienteCadastroDto.Tipo;
            enderecoCadastralClientePrepedidoDto.Endereco_ddd_cel = dadosClienteCadastroDto.DddCelular;
            enderecoCadastralClientePrepedidoDto.Endereco_ddd_res = dadosClienteCadastroDto.DddResidencial;

            enderecoCadastralClientePrepedidoDto.Endereco_tel_cel = dadosClienteCadastroDto.Celular;
            enderecoCadastralClientePrepedidoDto.Endereco_tel_res = dadosClienteCadastroDto.TelefoneResidencial;
            //if (dadosClienteCadastroDto.Tipo == Constantes.ID_PF)
            //{
            //    enderecoCadastralClientePrepedidoDto.Endereco_tel_cel = dadosClienteCadastroDto.DddCelular != null ?
            //      dadosClienteCadastroDto.DddCelular + dadosClienteCadastroDto.Celular : "";

            //    enderecoCadastralClientePrepedidoDto.Endereco_tel_res = dadosClienteCadastroDto.DddResidencial != null ?
            //      dadosClienteCadastroDto.DddResidencial + dadosClienteCadastroDto.TelefoneResidencial : "";
            //}
            enderecoCadastralClientePrepedidoDto.Endereco_ddd_com = dadosClienteCadastroDto.DddComercial;
            enderecoCadastralClientePrepedidoDto.Endereco_tel_com = dadosClienteCadastroDto.TelComercial;
            //enderecoCadastralClientePrepedidoDto.Endereco_tel_com = dadosClienteCadastroDto.DddComercial != null && dadosClienteCadastroDto.TelComercial != null ?
            //  dadosClienteCadastroDto.DddComercial + dadosClienteCadastroDto.TelComercial : "";
            enderecoCadastralClientePrepedidoDto.Endereco_ramal_com = dadosClienteCadastroDto.Ramal;
            enderecoCadastralClientePrepedidoDto.Endereco_ddd_com_2 = dadosClienteCadastroDto.DddComercial2;
            enderecoCadastralClientePrepedidoDto.Endereco_tel_com_2 = dadosClienteCadastroDto.TelComercial2;
            enderecoCadastralClientePrepedidoDto.Endereco_ddd_com_2 = dadosClienteCadastroDto.DddComercial2;
            //enderecoCadastralClientePrepedidoDto.Endereco_tel_com_2 =
            //  dadosClienteCadastroDto.Tipo == Constantes.ID_PJ && dadosClienteCadastroDto.DddComercial2 != null ?
            //    dadosClienteCadastroDto.DddComercial2 + dadosClienteCadastroDto.TelComercial2 : "";
            //enderecoCadastralClientePrepedidoDto.Endereco_ramal_com_2 = dadosClienteCadastroDto.Tipo == Constantes.ID_PJ ?
            //  dadosClienteCadastroDto.Ramal2 : "";
            enderecoCadastralClientePrepedidoDto.Endereco_email = dadosClienteCadastroDto.Email;
            enderecoCadastralClientePrepedidoDto.Endereco_email_xml = dadosClienteCadastroDto.EmailXml;
            enderecoCadastralClientePrepedidoDto.Endereco_produtor_rural_status = dadosClienteCadastroDto.ProdutorRural;
            enderecoCadastralClientePrepedidoDto.Endereco_contribuinte_icms_status = dadosClienteCadastroDto.Contribuinte_Icms_Status;
            enderecoCadastralClientePrepedidoDto.Endereco_ie = dadosClienteCadastroDto.Ie;
            enderecoCadastralClientePrepedidoDto.Endereco_contato = dadosClienteCadastroDto.Contato;
            enderecoCadastralClientePrepedidoDto.St_memorizacao_completa_enderecos = true;

            enderecoCadastralClientePrepedidoDto.IdOrcamentoCotacao = dadosClienteCadastroDto.IdOrcamentoCotacao;
            enderecoCadastralClientePrepedidoDto.IdIndicadorVendedor = dadosClienteCadastroDto.IdIndicadorVendedor;
            enderecoCadastralClientePrepedidoDto.Perc_max_comissao_padrao = dadosClienteCadastroDto.Perc_max_comissao_e_desconto_padrao;
            enderecoCadastralClientePrepedidoDto.Perc_max_comissao_e_desconto_padrao = dadosClienteCadastroDto.Perc_max_comissao_padrao;
            enderecoCadastralClientePrepedidoDto.Vendedor = dadosClienteCadastroDto.Vendedor;

            return enderecoCadastralClientePrepedidoDto;
        }
    }
}
