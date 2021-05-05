using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.ClienteDto
{
    public class DadosClienteCadastroDto
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
        public DateTime? Nascimento { get; set; }
        public string Sexo { get; set; }
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


        public static DadosClienteCadastroDto DadosClienteCadastroDto_De_DadosClienteCadastroDados(Cliente.Dados.DadosClienteCadastroDados origem)
        {
            if (origem == null) return null;
            return new DadosClienteCadastroDto()
            {
                Loja = origem.Loja,
                Indicador_Orcamentista = origem.Indicador_Orcamentista,
                Vendedor = origem.Vendedor,
                Id = origem.Id,
                Cnpj_Cpf = origem.Cnpj_Cpf,
                Rg = origem.Rg,
                Ie = origem.Ie,
                Contribuinte_Icms_Status = origem.Contribuinte_Icms_Status,
                Tipo = origem.Tipo,
                Observacao_Filiacao = origem.Observacao_Filiacao,
                Nascimento = origem.Nascimento,
                Sexo = origem.Sexo,
                Nome = origem.Nome,
                ProdutorRural = origem.ProdutorRural,
                Endereco = origem.Endereco,
                Numero = origem.Numero,
                Complemento = origem.Complemento,
                Bairro = origem.Bairro,
                Cidade = origem.Cidade,
                Uf = origem.Uf,
                Cep = origem.Cep,
                DddResidencial = origem.DddResidencial,
                TelefoneResidencial = origem.TelefoneResidencial,
                DddComercial = origem.DddComercial,
                TelComercial = origem.TelComercial,
                Ramal = origem.Ramal,
                DddCelular = origem.DddCelular,
                Celular = origem.Celular,
                TelComercial2 = origem.TelComercial2,
                DddComercial2 = origem.DddComercial2,
                Ramal2 = origem.Ramal2,
                Email = origem.Email,
                EmailXml = origem.EmailXml,
                Contato = origem.Contato
            };
        }
        public static Cliente.Dados.EnderecoCadastralClientePrepedidoDados EnderecoCadastralClientePrepedidoDados_De_DadosClienteCadastroDto(DadosClienteCadastroDto dadosClienteCadastroDto)
        {
            if (dadosClienteCadastroDto == null) return null;
            return new Cliente.Dados.EnderecoCadastralClientePrepedidoDados()
            {
                St_memorizacao_completa_enderecos = true,
                Endereco_logradouro = dadosClienteCadastroDto.Endereco,
                Endereco_numero = dadosClienteCadastroDto.Numero,
                Endereco_complemento = dadosClienteCadastroDto.Complemento,
                Endereco_bairro = dadosClienteCadastroDto.Bairro,
                Endereco_cidade = dadosClienteCadastroDto.Cidade,
                Endereco_uf = dadosClienteCadastroDto.Uf,
                Endereco_cep = dadosClienteCadastroDto.Cep,
                Endereco_email = dadosClienteCadastroDto.Email,
                Endereco_email_xml = dadosClienteCadastroDto.EmailXml,
                Endereco_nome = dadosClienteCadastroDto.Nome,
                Endereco_ddd_res = dadosClienteCadastroDto.DddResidencial,
                Endereco_tel_res = dadosClienteCadastroDto.TelefoneResidencial,
                Endereco_ddd_com = dadosClienteCadastroDto.DddComercial,
                Endereco_tel_com = dadosClienteCadastroDto.TelComercial,
                Endereco_ramal_com = dadosClienteCadastroDto.Ramal,
                Endereco_ddd_cel = dadosClienteCadastroDto.DddCelular,
                Endereco_tel_cel = dadosClienteCadastroDto.Celular,
                Endereco_ddd_com_2 = dadosClienteCadastroDto.DddComercial2,
                Endereco_tel_com_2 = dadosClienteCadastroDto.TelComercial2,
                Endereco_ramal_com_2 = dadosClienteCadastroDto.Ramal2,
                Endereco_tipo_pessoa = dadosClienteCadastroDto.Tipo,
                Endereco_cnpj_cpf = dadosClienteCadastroDto.Cnpj_Cpf,
                Endereco_contribuinte_icms_status = dadosClienteCadastroDto.Contribuinte_Icms_Status,
                Endereco_produtor_rural_status = dadosClienteCadastroDto.ProdutorRural,
                Endereco_ie = dadosClienteCadastroDto.Ie,
                Endereco_rg = dadosClienteCadastroDto.Rg,
                Endereco_contato = dadosClienteCadastroDto.Contato
            };
        }

        public static Cliente.Dados.DadosClienteCadastroDados DadosClienteCadastroDados_De_DadosClienteCadastroDto(DadosClienteCadastroDto dadosClienteCadastroDto)
        {
            if (dadosClienteCadastroDto == null) return null;
            return new Cliente.Dados.DadosClienteCadastroDados()
            {
                Loja = dadosClienteCadastroDto.Loja,
                Indicador_Orcamentista = dadosClienteCadastroDto.Indicador_Orcamentista,
                Vendedor = dadosClienteCadastroDto.Vendedor,
                Id = dadosClienteCadastroDto.Id,
                Cnpj_Cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(dadosClienteCadastroDto.Cnpj_Cpf),
                Rg = dadosClienteCadastroDto.Rg,
                Ie = dadosClienteCadastroDto.Ie,
                Contribuinte_Icms_Status = dadosClienteCadastroDto.Contribuinte_Icms_Status,
                Tipo = dadosClienteCadastroDto.Tipo,
                Observacao_Filiacao = dadosClienteCadastroDto.Observacao_Filiacao,
                Nascimento = dadosClienteCadastroDto.Nascimento,
                Sexo = dadosClienteCadastroDto.Sexo??"",
                Nome = dadosClienteCadastroDto.Nome,
                ProdutorRural = dadosClienteCadastroDto.ProdutorRural,
                Endereco = dadosClienteCadastroDto.Endereco,
                Numero = dadosClienteCadastroDto.Numero,
                Complemento = dadosClienteCadastroDto.Complemento,
                Bairro = dadosClienteCadastroDto.Bairro,
                Cidade = dadosClienteCadastroDto.Cidade,
                Uf = dadosClienteCadastroDto.Uf,
                Cep = dadosClienteCadastroDto.Cep.Replace("-", ""),
                DddResidencial = dadosClienteCadastroDto.DddResidencial,
                TelefoneResidencial = dadosClienteCadastroDto.TelefoneResidencial,
                DddComercial = dadosClienteCadastroDto.DddComercial,
                TelComercial = dadosClienteCadastroDto.TelComercial,
                Ramal = dadosClienteCadastroDto.Ramal,
                DddCelular = dadosClienteCadastroDto.DddCelular,
                Celular = dadosClienteCadastroDto.Celular,
                TelComercial2 = dadosClienteCadastroDto.TelComercial2,
                DddComercial2 = dadosClienteCadastroDto.DddComercial2,
                Ramal2 = dadosClienteCadastroDto.Ramal2,
                Email = dadosClienteCadastroDto.Email,
                EmailXml = dadosClienteCadastroDto.EmailXml,
                Contato = dadosClienteCadastroDto.Contato
            };
        }
    }
}
