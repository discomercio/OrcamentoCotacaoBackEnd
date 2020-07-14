using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.ClienteCadastro
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
    }
}
