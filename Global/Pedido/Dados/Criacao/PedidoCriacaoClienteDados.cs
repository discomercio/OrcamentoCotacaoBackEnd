using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoClienteDados
    {
        public PedidoCriacaoClienteDados(string id_cliente, string tipo, string indicador_Orcamentista, string loja, string cnpj_Cpf)
        {
            Id_cliente = id_cliente ?? throw new ArgumentNullException(nameof(id_cliente));
            Tipo = tipo ?? throw new ArgumentNullException(nameof(tipo));
            Indicador_Orcamentista = indicador_Orcamentista ?? throw new ArgumentNullException(nameof(indicador_Orcamentista));
            Loja = loja ?? throw new ArgumentNullException(nameof(loja));
            Cnpj_Cpf = cnpj_Cpf ?? throw new ArgumentNullException(nameof(cnpj_Cpf));
        }

        public string Id_cliente { get; set; }
        public string Tipo { get; set; }
        public string Indicador_Orcamentista { get; set; }
        public string Loja { get; set; }
        public string Cnpj_Cpf { get; set; }

        //todo: remover esta conversao
        static public PedidoCriacaoClienteDados PedidoCriacaoClienteDados_de_DadosClienteCadastroDados(Cliente.Dados.DadosClienteCadastroDados origem)
        {
            if (origem is null)
            {
                throw new ArgumentNullException(nameof(origem));
            }

            return new PedidoCriacaoClienteDados(
                cnpj_Cpf: origem.Cnpj_Cpf,
                id_cliente: origem.Id,
                indicador_Orcamentista: origem.Indicador_Orcamentista,
                loja: origem.Loja,
                tipo: origem.Tipo
            );
        }
    }
}
