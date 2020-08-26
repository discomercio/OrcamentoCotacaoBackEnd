using Cliente;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.ClienteCadastro
{
    public class ListaBancoDto
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }


        public static List<ListaBancoDto> ListaBancoDtoDeBancoDadosLista(IEnumerable<Cliente.Dados.ListaBancoDados> listaBancoDados)
        {
            var ret = new List<ListaBancoDto>();
            foreach (var p in listaBancoDados)
                ret.Add(ListaBancoDtoDeBancoDados(p));
            return ret;
        }

        public static ListaBancoDto ListaBancoDtoDeBancoDados(Cliente.Dados.ListaBancoDados listaBancoDados)
        {
            return new ListaBancoDto()
            {
                Codigo = listaBancoDados.Codigo,
                Descricao = listaBancoDados.Descricao
            };
        }
    }
}
