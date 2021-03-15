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
        private async Task Inicializarindicador()
        {
            if (!String.IsNullOrEmpty(Pedido.Ambiente.Indicador))
                Indicador = await Criacao.PrepedidoBll.BuscarTorcamentista(Pedido.Ambiente.Indicador);
        }


        public async Task Inicializar()
        {
            var tasks = new List<Task>
            {
                Inicializarindicador(),
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
            var tipoParcela = Pedido.FormaPagtoCriacao.CustoFinancFornecTipoParcelamento;
            var qtdeParcelas = Pedido.FormaPagtoCriacao.CustoFinancFornecQtdeParcelas;
            TpercentualCustoFinanceiroFornecedors_Coeficiente = await (from c in db.TpercentualCustoFinanceiroFornecedors
                                                                       where listaFabricantes.Contains(c.Fabricante) &&
                                                                           c.Tipo_Parcelamento == tipoParcela &&
                                                                           c.Qtde_Parcelas == qtdeParcelas
                                                                       select new TpercentualCustoFinanceiroFornecedors_Coeficiente_Dado(
                                                                           c.Fabricante, c.Coeficiente,
                                                                           c.Tipo_Parcelamento, c.Qtde_Parcelas))
                                                         .ToListAsync();

        }
        #endregion

        #region TprodutoLoja
        public List<TprodutoLoja> TprodutoLoja_Include_Tprodtuo_Tfabricante_Validado { get; private set; } = new List<TprodutoLoja>();

        private IEnumerable<TprodutoLoja> TprodutoLoja_Query(string fabricante, string produto)
        {
            return (from p in TprodutoLoja_Include_Tprodtuo_Tfabricante_Validado
                    where p.Fabricante == fabricante && p.Produto == produto && p.Loja == Pedido.Ambiente.Loja
                    select p);
        }

        private async Task InicializarTprodutoLoja()
        {
            var db = Criacao.ContextoProvider.GetContextoLeitura();
            //basta que tenha o produto!
            var listaProdutos = Pedido.ListaProdutos.Select(c => c.Produto).Distinct().ToList();
            TprodutoLoja_Include_Tprodtuo_Tfabricante_Validado = await (from c in db.TprodutoLojas.Include(x => x.Tproduto).Include(x => x.Tfabricante)
                                                                        where listaProdutos.Contains(c.Tproduto.Produto) &&
                                                                        c.Loja == Pedido.Ambiente.Loja
                                                                        select c).ToListAsync();

            //já validamos que todos os produtos da lista existem uma e somente uma vez
            foreach (var linha_pedido in Pedido.ListaProdutos)
            {
                var query = TprodutoLoja_Query(linha_pedido.Fabricante, linha_pedido.Produto);
                if (query.Count() != 1)
                {
                    Retorno.ListaErros.Add($"Produto {linha_pedido.Produto} do fabricante {linha_pedido.Fabricante} NÃO está cadastrado para a loja {Pedido.Ambiente.Loja}");
                }
                /*
                onde validamos alguns campos: 
                CustoFinancFornecPrecoListaBase_Conferencia: validado em Prepedido.ValidacoesPrepedidoBll.ConfrontarProdutos
                CustoFinancFornecCoeficiente_Conferencia: validado em Prepedido.ValidacoesPrepedidoBll.ValidarCustoFinancFornecCoeficiente
                Preco_Lista: validado em Prepedido.ValidacoesPrepedidoBll.ConfrontarProdutos
                */

            }
        }

        #endregion

    }
}
