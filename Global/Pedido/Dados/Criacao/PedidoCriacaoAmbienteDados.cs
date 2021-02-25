using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoAmbienteDados
    {
        public PedidoCriacaoAmbienteDados(string loja, string vendedor, string usuario, bool comIndicador, string indicador, string orcamentista, int id_nfe_emitente_selecao_manual, bool venda_Externa, bool opcaoVendaSemEstoque, string loja_indicou,
            InfraBanco.Constantes.Constantes.Op_origem__pedido_novo operacao_origem,
            InfraBanco.Constantes.Constantes.Cod_site id_param_site)
        {
            //pode vir com null
            indicador ??= "";
            orcamentista ??= "";
            loja_indicou ??= "";

            Loja = loja ?? throw new ArgumentNullException(nameof(loja));
            Vendedor = vendedor ?? throw new ArgumentNullException(nameof(vendedor));
            Usuario = usuario ?? throw new ArgumentNullException(nameof(usuario));
            ComIndicador = comIndicador;
            Indicador = indicador ?? throw new ArgumentNullException(nameof(indicador));
            Orcamentista = orcamentista ?? throw new ArgumentNullException(nameof(orcamentista));
            Id_nfe_emitente_selecao_manual = id_nfe_emitente_selecao_manual;
            Venda_Externa = venda_Externa;
            OpcaoVendaSemEstoque = opcaoVendaSemEstoque;
            Loja_indicou = loja_indicou ?? throw new ArgumentNullException(nameof(loja_indicou));

            //vazio ou OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO
            Operacao_origem = operacao_origem;
            Id_param_site = id_param_site;
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
        //orcamentista, somente quando um prepedido vira pedido
        public string Orcamentista { get; }

        //Armazena o id do centro de distribuição selecionado manualmente
        //Obs: armazena "0" caso seja automático
        public int Id_nfe_emitente_selecao_manual { get; }

        //Armazena se é venda externa
        public bool Venda_Externa { get; }

        //Flag para saber se o cliente aceitou finalizar o pedido mesmo com produto sem estoque
        public bool OpcaoVendaSemEstoque { get; }

        public string Loja_indicou { get; }

        public InfraBanco.Constantes.Constantes.Op_origem__pedido_novo Operacao_origem { get; }
        public InfraBanco.Constantes.Constantes.Cod_site Id_param_site { get; }
    }
}
