using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prepedido.Dto
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
        public string UsuarioCadastro { get; set; }
        public int? IdOrcamentoCotacao { get; set; }
        public int? IdIndicadorVendedor { get; set; }
        public float? Perc_max_comissao_padrao { get; set; }
        public float? Perc_max_comissao_e_desconto_padrao { get; set; }

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
                Contato = origem.Contato,
                UsuarioCadastro = "",
                IdOrcamentoCotacao = origem.IdOrcamentoCotacao,
                IdIndicadorVendedor = origem.IdIndicadorVendedor,
                Perc_max_comissao_padrao = origem.Perc_max_comissao_padrao,
                Perc_max_comissao_e_desconto_padrao = origem.Perc_max_comissao_e_desconto_padrao
            };
        }

        public static Cliente.Dados.DadosClienteCadastroDados DadosClienteCadastroDados_De_DadosClienteCadastroDto(DadosClienteCadastroDto origem)
        {
            if (origem == null) return null;
            return new Cliente.Dados.DadosClienteCadastroDados()
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
                Contato = origem.Contato,
                UsuarioCadastro = origem.UsuarioCadastro,
                IdOrcamentoCotacao = origem.IdOrcamentoCotacao,
                IdIndicadorVendedor = origem.IdIndicadorVendedor,
                Perc_max_comissao_padrao = origem.Perc_max_comissao_padrao,
                Perc_max_comissao_e_desconto_padrao = origem.Perc_max_comissao_e_desconto_padrao
            };
        }


    }
}