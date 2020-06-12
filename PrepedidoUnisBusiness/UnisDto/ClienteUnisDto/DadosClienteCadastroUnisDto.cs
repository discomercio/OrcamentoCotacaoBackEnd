using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto
{
    public class DadosClienteCadastroUnisDto
    {

        public string Loja { get; set; }

        [Required]
        [MaxLength(20)]
        public string Indicador_Orcamentista { get; set; }

        [MaxLength(10)]
        public string Vendedor { get; set; }

        [Required]
        [MaxLength(12)]
        public string Id { get; set; }

        [MaxLength(14)]
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
    }
}
