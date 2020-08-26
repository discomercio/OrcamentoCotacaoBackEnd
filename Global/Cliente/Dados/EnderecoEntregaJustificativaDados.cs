using System;
using System.Collections.Generic;
using System.Text;

namespace Cliente.Dados
{
    public class EnderecoEntregaJustificativaDados
    {
        //codigo da justificativa, preenchdio quando está criando (do spa para a api)
        public string EndEtg_cod_justificativa { get; set; }
        //descrição da justificativa, preenchdio para mostrar (da api para o spa)
        public string EndEtg_descricao_justificativa { get; set; }
    }
}
