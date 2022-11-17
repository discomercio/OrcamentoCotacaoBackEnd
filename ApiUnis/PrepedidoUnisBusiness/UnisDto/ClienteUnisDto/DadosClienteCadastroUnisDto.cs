using Prepedido.Dto;
using PrepedidoUnisBusiness.UnisDto.ClienteUnisDto;
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
        [Required]
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
        [Required]
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

        [MaxLength(60)]
        [Required]
        public string Endereco { get; set; }

        [MaxLength(60)]
        [Required]
        public string Numero { get; set; }

        [MaxLength(60)]
        public string Complemento { get; set; }

        [MaxLength(60)]
        [Required]
        public string Bairro { get; set; }

        [MaxLength(60)]
        [Required]
        public string Cidade { get; set; }

        [MaxLength(2)]
        [Required]
        public string Uf { get; set; }

        [MaxLength(8)]
        [Required]
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
        [Required]
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

        public static DadosClienteCadastroDto DadosClienteCadastroDtoDeEnderecoCadastralClientePrepedidoUnisDto(
            EnderecoCadastralClientePrepedidoUnisDto endCadastral, string indicadorOrcamentista, string loja, 
            string sexo, DateTime nascimento, string idCliente)
        {
            /* Ao criar um novo pré-pedido será permitido que seja alterado os dados do cliente 
             * sem que altere o cadastro do cliente na base de dados, 
             * foi incluído esses novos campos do Dto "EnderecoCadastralClientePrepedidoUnisDto", que recebe os dados de
             * cliente.
             * Estamos passando para o Dto "DadosClienteCadastroDto" para que seja feita a validação de dados dos novos
             * dados que poderão ser alterado
             * 
             */
            var ret = new DadosClienteCadastroDto()
            {
                Id = idCliente,
                Indicador_Orcamentista = indicadorOrcamentista.ToUpper(),
                Loja = loja,
                Nome = endCadastral.Endereco_nome,
                Cnpj_Cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(endCadastral.Endereco_cnpj_cpf.Trim()),
                Tipo = endCadastral.Endereco_tipo_pessoa,
                Sexo = sexo == null ? "" : sexo,
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
                Vendedor = "",// esse campo não é utilizado em TCliente
                Cep = endCadastral.Endereco_cep,
                Endereco = endCadastral.Endereco_logradouro,
                Numero = endCadastral.Endereco_numero,
                Bairro = endCadastral.Endereco_bairro,
                Cidade = endCadastral.Endereco_cidade,
                Uf = endCadastral.Endereco_uf,
                Complemento = endCadastral.Endereco_complemento,
                Contato = endCadastral.Endereco_contato
            };

            return ret;
        }

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
                Cnpj_Cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(dadosClienteUnis.Cnpj_Cpf.Trim()),
                Tipo = dadosClienteUnis.Tipo,
                Sexo = dadosClienteUnis.Sexo == null ? "" : dadosClienteUnis.Sexo,
                Rg = dadosClienteUnis.Rg == null ? "" : dadosClienteUnis.Rg,
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
                Contato = dadosClienteUnis.Contato,
                Observacao_Filiacao = dadosClienteUnis.Observacao_Filiacao
            };

            return ret;
        }
    }
}
