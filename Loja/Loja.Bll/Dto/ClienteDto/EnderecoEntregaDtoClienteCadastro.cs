using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.ClienteDto
{
    public class EnderecoEntregaDtoClienteCadastro
    {
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
        public bool OutroEndereco { get; set; }
    }
}
