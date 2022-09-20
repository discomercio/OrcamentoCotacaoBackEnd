using Cliente;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prepedido.Dto
{
    public class ListaBancoDto
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }


        public static List<ListaBancoDto> ListaBancoDto_De_BancoDadosLista(IEnumerable<Cliente.Dados.ListaBancoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<ListaBancoDto>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(ListaBancoDto_De_BancoDados(p));
            return ret;
        }

        public static ListaBancoDto ListaBancoDto_De_BancoDados(Cliente.Dados.ListaBancoDados listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            return new ListaBancoDto()
            {
                Codigo = listaBancoDados.Codigo,
                Descricao = listaBancoDados.Descricao
            };
        }
    }
}
