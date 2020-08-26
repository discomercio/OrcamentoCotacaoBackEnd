using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrepedidoBusiness.Dto.ClienteCadastro
{
    //usado para montar o combo
    public class EnderecoEntregaJustificativaDto
    {
        //codigo da justificativa, preenchdio quando está criando (do spa para a api)
        public string EndEtg_cod_justificativa { get; set; }
        //descrição da justificativa, preenchdio para mostrar (da api para o spa)
        public string EndEtg_descricao_justificativa { get; set; }


        public static List<EnderecoEntregaJustificativaDto> EnderecoEntregaJustificativaDtoDeEnderecoEntregaJustificativaDadosLista(IEnumerable<Cliente.Dados.EnderecoEntregaJustificativaDados> enderecoEntregaJustificativaDados)
        {
            var ret = new List<EnderecoEntregaJustificativaDto>();
            foreach (var p in enderecoEntregaJustificativaDados)
                ret.Add(EnderecoEntregaJustificativaDtoDeEnderecoEntregaJustificativaDados(p));
            return ret;
        }

        public static EnderecoEntregaJustificativaDto EnderecoEntregaJustificativaDtoDeEnderecoEntregaJustificativaDados(Cliente.Dados.EnderecoEntregaJustificativaDados enderecoEntregaJustificativaDados)
        {
            return new EnderecoEntregaJustificativaDto()
            {
                EndEtg_cod_justificativa = enderecoEntregaJustificativaDados.EndEtg_cod_justificativa,
                EndEtg_descricao_justificativa = enderecoEntregaJustificativaDados.EndEtg_descricao_justificativa
            };
        }
    }
}