﻿using PrepedidoBusiness.Dto.ClienteCadastro;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto
{
    public class DadosClienteCadastroUnisDto
    {
        [Required]
        [MaxLength(20)]
        public string Indicador_Orcamentista { get; set; }

        //[MaxLength(10)]
        //public string UsuarioCadastro { get; set; }

        /// <summary>
        /// Id: deixar em branco para cadastrar
        /// </summary>
        [MaxLength(12)]
        public string Id { get; private set; }

        [MaxLength(18)]
        public string Cnpj_Cpf { get; set; }

        [MaxLength(20)]
        public string Rg { get; set; }

        [MaxLength(20)]
        public string Ie { get; set; }

        /// <summary>
        /// Contribuinte_Icms_Status: INICIAL = 0, NAO = 1, SIM = 2, ISENTO = 3
        /// </summary>
        [Required]
        public byte Contribuinte_Icms_Status { get; set; }

        /// <summary>
        /// Tipo = "PF", "PJ"
        /// </summary>
        [MaxLength(2)]
        public string Tipo { get; set; }

        [MaxLength(60)]
        public string Observacao_Filiacao { get; set; }

        public DateTime? Nascimento { get; set; }

        /// <summary>
        /// Sexo = "M", "F"
        /// </summary>
        [MaxLength(1)]
        public string Sexo { get; set; }

        [MaxLength(60)]
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// ProdutorRural: COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL = 0, COD_ST_CLIENTE_PRODUTOR_RURAL_NAO = 1, COD_ST_CLIENTE_PRODUTOR_RURAL_SIM = 2
        /// </summary>
        [Required]
        public byte ProdutorRural { get; set; }

        [MaxLength(80)]
        public string Endereco { get; set; }

        [MaxLength(20)]
        public string Numero { get; set; }

        [MaxLength(60)]
        public string Complemento { get; set; }

        [MaxLength(72)]
        public string Bairro { get; set; }

        [MaxLength(60)]
        public string Cidade { get; set; }

        [MaxLength(2)]
        public string Uf { get; set; }

        [MaxLength(8)]
        public string Cep { get; set; }

        [MaxLength(4)]
        public string DddResidencial { get; set; }

        [MaxLength(11)]
        public string TelefoneResidencial { get; set; }

        [MaxLength(4)]
        public string DddComercial { get; set; }

        [MaxLength(11)]
        public string TelComercial { get; set; }

        [MaxLength(4)]
        public string Ramal { get; set; }

        [MaxLength(2)]
        public string DddCelular { get; set; }

        [MaxLength(9)]
        public string Celular { get; set; }

        [MaxLength(9)]
        public string TelComercial2 { get; set; }

        [MaxLength(2)]
        public string DddComercial2 { get; set; }

        [MaxLength(4)]
        public string Ramal2 { get; set; }

        [MaxLength(60)]
        public string Email { get; set; }

        [MaxLength(60)]
        public string EmailXml { get; set; }

        [MaxLength(30)]
        public string Contato { get; set; }

        //verificar se iremos salvar com o código da ITS, pq nós que iremos cadastrar o cliente
        //[Required]
        //public int Sistema_responsavel_cadastro { get; set; }

        //[Required]
        //public int Sistema_responsavel_atualizacao { get; set; }

        public static DadosClienteCadastroUnisDto DadosClienteCadastroUnisDtoDeDadosClienteCadastroDto(DadosClienteCadastroDto clienteCadastroDto)
        {
            var ret = new DadosClienteCadastroUnisDto()
            {
                Indicador_Orcamentista = clienteCadastroDto.Indicador_Orcamentista,
                Id = clienteCadastroDto.Id,
                Cnpj_Cpf = clienteCadastroDto.Cnpj_Cpf.Trim(),
                Rg = clienteCadastroDto.Rg,
                Ie = clienteCadastroDto.Ie,
                Contribuinte_Icms_Status = clienteCadastroDto.Contribuinte_Icms_Status,
                Tipo = clienteCadastroDto.Tipo,
                Observacao_Filiacao = clienteCadastroDto.Observacao_Filiacao,
                Nascimento = clienteCadastroDto.Nascimento,
                Sexo = clienteCadastroDto.Sexo,
                Nome = clienteCadastroDto.Nome,
                ProdutorRural = clienteCadastroDto.ProdutorRural,
                Endereco = clienteCadastroDto.Endereco,
                Numero = clienteCadastroDto.Numero,
                Complemento = clienteCadastroDto.Complemento,
                Bairro = clienteCadastroDto.Bairro,
                Cidade = clienteCadastroDto.Cidade,
                Uf = clienteCadastroDto.Uf,
                Cep = clienteCadastroDto.Cep,
                DddResidencial = clienteCadastroDto.DddResidencial,
                TelefoneResidencial = clienteCadastroDto.TelefoneResidencial,
                DddComercial = clienteCadastroDto.DddComercial,
                TelComercial = clienteCadastroDto.TelComercial,
                Ramal = clienteCadastroDto.Ramal,
                DddCelular = clienteCadastroDto.DddCelular,
                Celular = clienteCadastroDto.Celular,
                TelComercial2 = clienteCadastroDto.TelComercial2,
                DddComercial2 = clienteCadastroDto.DddComercial2,
                Ramal2 = clienteCadastroDto.Ramal2,
                Email = clienteCadastroDto.Email,
                EmailXml = clienteCadastroDto.EmailXml,
                Contato = clienteCadastroDto.Contato
            };
            return ret;
        }

        public static DadosClienteCadastroDto DadosClienteCadastroDtoDeDadosClienteCadastroUnisDto(DadosClienteCadastroUnisDto dadosClienteUnis, string loja)
        {
            var ret = new DadosClienteCadastroDto()
            {
                Indicador_Orcamentista = dadosClienteUnis.Indicador_Orcamentista,
                Loja = loja,
                Nome = dadosClienteUnis.Nome,
                Cnpj_Cpf = PrepedidoBusiness.Utils.Util.SoDigitosCpf_Cnpj(dadosClienteUnis.Cnpj_Cpf.Trim()),
                Tipo = dadosClienteUnis.Tipo,
                Sexo = dadosClienteUnis.Sexo == null ? "" : dadosClienteUnis.Sexo,
                Rg = dadosClienteUnis.Rg == null ? "" : dadosClienteUnis.Rg,
                Nascimento = dadosClienteUnis.Nascimento,
                DddCelular = dadosClienteUnis.DddCelular,
                Celular = dadosClienteUnis.Celular,
                DddResidencial = dadosClienteUnis.DddResidencial == null ? "" : dadosClienteUnis.DddResidencial,
                TelefoneResidencial = dadosClienteUnis.TelefoneResidencial == null ? "" : dadosClienteUnis.TelefoneResidencial,
                DddComercial = dadosClienteUnis.DddComercial,
                TelComercial = dadosClienteUnis.TelComercial,
                Ramal = dadosClienteUnis.Ramal,
                DddComercial2 = dadosClienteUnis.DddComercial2,
                TelComercial2 = dadosClienteUnis.TelComercial2,
                Ramal2 = dadosClienteUnis.Ramal2,
                Ie = dadosClienteUnis.Ie,
                ProdutorRural = dadosClienteUnis.ProdutorRural,
                Contribuinte_Icms_Status = dadosClienteUnis.Contribuinte_Icms_Status,
                Email = dadosClienteUnis.Email,
                EmailXml = dadosClienteUnis.EmailXml,
                Vendedor = "",// esse campo não é utilizado em TCliente
                Cep = dadosClienteUnis.Cep,
                Endereco = dadosClienteUnis.Endereco,
                Numero = dadosClienteUnis.Numero,
                Bairro = dadosClienteUnis.Bairro,
                Cidade = dadosClienteUnis.Cidade,
                Uf = dadosClienteUnis.Uf,
                Complemento = dadosClienteUnis.Complemento,
                Contato = dadosClienteUnis.Contato
            };
            
            return ret;
        }
    }
}
