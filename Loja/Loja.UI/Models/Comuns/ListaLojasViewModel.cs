using Loja.Bll.Bll.AcessoBll;
using Loja.Bll.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace Loja.UI.Models.Comuns
{
    public class ListaLojasViewModel
    {
        public ListaLojasViewModel(UsuarioLogado usuarioLogado, List<ItemLoja>? itensLoja)
        {
            ConsultaUniversalPedidoOrcamento = usuarioLogado.Operacao_permitida(Constantes.OP_LJA_CONSULTA_UNIVERSAL_PEDIDO_ORCAMENTO);
            LojasDisponiveis = usuarioLogado.LojasDisponiveis;
            MostrarLoja = usuarioLogado.Operacao_permitida(Constantes.OP_LJA_LOGIN_TROCA_RAPIDA_LOJA);
            ItensLoja = itensLoja;
        }

        public bool ConsultaUniversalPedidoOrcamento { get; set; }
        public List<Loja.Bll.Bll.AcessoBll.UsuarioAcessoBll.LojaPermtidaUsuario> LojasDisponiveis { get; set; }
        public bool MostrarLoja { get; set; }
        public string NomeTabelaHtml { get; set; } = "tabeladados";
        public int NumeroColunaDaLoja { get; set; } = 1;

        //para mostrar o número de registros, opcional
        public class ItemLoja
        {
            public string Loja { get; set; } = "";
            public int NumeroItens { get; set; } = 0;
        }
        public List<ItemLoja>? ItensLoja { get; set; }
    }
}
