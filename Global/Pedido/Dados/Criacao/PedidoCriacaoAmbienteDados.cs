using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoAmbienteDados
    {
        public PedidoCriacaoAmbienteDados(string loja, string vendedor, string usuario, bool comIndicador, string indicador, string orcamentista, int idNfeSelecionadoManual, bool venda_Externa, bool opcaoVendaSemEstoque)
        {
            //pode vir com null
            indicador ??= "";
            orcamentista ??= "";

            Loja = loja ?? throw new ArgumentNullException(nameof(loja));
            Vendedor = vendedor ?? throw new ArgumentNullException(nameof(vendedor));
            Usuario = usuario ?? throw new ArgumentNullException(nameof(usuario));
            ComIndicador = comIndicador;
            Indicador = indicador ?? throw new ArgumentNullException(nameof(indicador));
            Orcamentista = orcamentista ?? throw new ArgumentNullException(nameof(orcamentista));
            IdNfeSelecionadoManual = idNfeSelecionadoManual;
            Venda_Externa = venda_Externa;
            OpcaoVendaSemEstoque = opcaoVendaSemEstoque;
        }

        //Armazena a loja do usuário logado
        public string Loja { get; }
        public string Vendedor { get; }

        //Armazena nome do usuário logado
        public string Usuario { get; }

        //Flag para saber se tem indicador selecionado 
        public bool ComIndicador { get; }

        //Armazena o nome do indicador selecionado
        public string Indicador { get; }
        public string Orcamentista { get; }

        //Armazena o id do centro de distribuição selecionado manualmente
        //Obs: armazena "0" caso seja automático
        public int IdNfeSelecionadoManual { get; }

        //Armazena se é venda externa
        public bool Venda_Externa { get; }

        //Flag para saber se o cliente aceitou finalizar o pedido mesmo com produto sem estoque
        public bool OpcaoVendaSemEstoque { get; }
    }
}
