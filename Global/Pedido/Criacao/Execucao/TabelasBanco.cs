using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedido.Criacao.Execucao
{
    class TabelasBanco
    {
        private readonly PedidoCriacaoDados Pedido;
        private readonly PedidoCriacaoRetornoDados Retorno;
        private readonly Pedido.Criacao.PedidoCriacao Criacao;
        public TabelasBanco(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
        {
            this.Pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));
            this.Retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            this.Criacao = pedidoCriacao ?? throw new ArgumentNullException(nameof(pedidoCriacao));
        }


        //este pode ser null sim
        public TorcamentistaEindicador? Indicador { get; private set; } = null;


        public async Task Inicializar()
        {
            var tasks = new List<Task>
            {
                InicializarTpercentualCustoFinanceiroFornecedors(),
                InicializarTprodutoLoja()
            };
            foreach (var t in tasks)
                await t;
        }

        #region TpercentualCustoFinanceiroFornecedors
        public class TpercentualCustoFinanceiroFornecedors_Coeficiente_Dado
        {
            public readonly string Fabricante;
            public readonly float Coeficiente;
            public readonly string Tipo_Parcelamento;
            public readonly short Qtde_Parcelas;

            public TpercentualCustoFinanceiroFornecedors_Coeficiente_Dado(string fabricante, float coeficiente, string tipo_Parcelamento, short qtde_Parcelas)
            {
                Fabricante = fabricante ?? throw new ArgumentNullException(nameof(fabricante));
                Coeficiente = coeficiente;
                Tipo_Parcelamento = tipo_Parcelamento ?? throw new ArgumentNullException(nameof(tipo_Parcelamento));
                Qtde_Parcelas = qtde_Parcelas;
            }
        }
        public List<TpercentualCustoFinanceiroFornecedors_Coeficiente_Dado> TpercentualCustoFinanceiroFornecedors_Coeficiente { get; private set; } = new List<TpercentualCustoFinanceiroFornecedors_Coeficiente_Dado>();

        private async Task InicializarTpercentualCustoFinanceiroFornecedors()
        {
            if (!String.IsNullOrEmpty(Pedido.Ambiente.Indicador))
                Indicador = await Criacao.PrepedidoBll.BuscarTorcamentista(Pedido.Ambiente.Indicador);

            var db = Criacao.ContextoProvider.GetContextoLeitura();
            var listaFabricantes = Pedido.ListaProdutos.Select(c => c.Fabricante).Distinct().ToList();
            TpercentualCustoFinanceiroFornecedors_Coeficiente = await (from c in db.TpercentualCustoFinanceiroFornecedors
                                                                       where listaFabricantes.Contains(c.Fabricante) &&
                                                                           c.Tipo_Parcelamento == Criacao.Execucao.C_custoFinancFornecTipoParcelamento &&
                                                                           c.Qtde_Parcelas == Criacao.Execucao.C_custoFinancFornecQtdeParcelas
                                                                       select new TpercentualCustoFinanceiroFornecedors_Coeficiente_Dado(
                                                                           c.Fabricante, c.Coeficiente,
                                                                           c.Tipo_Parcelamento, c.Qtde_Parcelas))
                                                         .ToListAsync();

        }
        #endregion

        #region TprodutoLoja
        public List<TprodutoLoja> TprodutoLoja_Include_Tprodtuo_Tfabricante { get; private set; } = new List<TprodutoLoja>();

        private async Task InicializarTprodutoLoja()
        {
            if (!String.IsNullOrEmpty(Pedido.Ambiente.Indicador))
                Indicador = await Criacao.PrepedidoBll.BuscarTorcamentista(Pedido.Ambiente.Indicador);

            var db = Criacao.ContextoProvider.GetContextoLeitura();
            //basta que tenha o produto!
            var listaProdutos = Pedido.ListaProdutos.Select(c => c.Produto).Distinct().ToList();
            TprodutoLoja_Include_Tprodtuo_Tfabricante = await (from c in db.TprodutoLojas.Include(x => x.Tproduto).Include(x => x.Tfabricante)
                                                               where listaProdutos.Contains(c.Tproduto.Produto) &&
                                                               c.Loja == Pedido.Ambiente.Loja
                                                               select c).ToListAsync();
        }
        #endregion

    }
}
