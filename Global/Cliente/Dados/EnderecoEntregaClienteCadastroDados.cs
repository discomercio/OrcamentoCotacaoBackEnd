using System;
using System.Collections.Generic;
using System.Text;

namespace Cliente.Dados
{
    public class EnderecoEntregaClienteCadastroDados
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
    }
}
