using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfraBanco
{
    public class ContextoBd
    {
        private readonly ContextoBdBasico contexto;
        internal ContextoBd(ContextoBdBasico contexto)
        {
            this.contexto = contexto;
            //sem nenhum rastreamento de mudanças na conexao
            //se ligamos: não podemos fazer vários acessos usando o mesmo contexto com async/await
            //se ligamos: toda a documentação diz que a performance é bem melhor
            //contexto.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public IQueryable<Tcliente> Tclientes { get => contexto.Tclientes; }
        public IQueryable<Torcamento> Torcamentos { get => contexto.Torcamentos; }
        public IQueryable<TclienteRefBancaria> TclienteRefBancarias { get => contexto.TclienteRefBancarias; }
        public IQueryable<Tpedido> Tpedidos { get => contexto.Tpedidos; }
        public IQueryable<TorcamentistaEindicador> TorcamentistaEindicadors { get => contexto.TorcamentistaEindicadors; }
        public IQueryable<TsessaoHistorico> TsessaoHistoricos { get => contexto.TsessaoHistoricos; }
        public IQueryable<Tusuario> Tusuarios { get => contexto.Tusuarios; }
        public IQueryable<Tproduto> Tprodutos { get => contexto.Tprodutos; }
        public IQueryable<TprodutoLoja> TprodutoLojas { get => contexto.TprodutoLojas; }
        public IQueryable<TpedidoItem> TpedidoItems { get => contexto.TpedidoItems; }
        public IQueryable<TpedidoItemDevolvido> TpedidoItemDevolvidos { get => contexto.TpedidoItemDevolvidos; }
        public IQueryable<TpedidoPerda> TpedidoPerdas { get => contexto.TpedidoPerdas; }
        public IQueryable<TpedidoPagamento> TpedidoPagamentos { get => contexto.TpedidoPagamentos; }
        public IQueryable<TestoqueMovimento> TestoqueMovimentos { get => contexto.TestoqueMovimentos; }
        public IQueryable<Ttransportadora> Ttransportadoras { get => contexto.Ttransportadoras; }
        public IQueryable<TpedidoBlocosNotas> TpedidoBlocosNotas { get => contexto.TpedidoBlocosNotas; }
        public IQueryable<TcodigoDescricao> TcodigoDescricaos { get => contexto.TcodigoDescricaos; }
        public IQueryable<TpedidoOcorrenciaMensagem> TpedidoOcorrenciaMensagems { get => contexto.TpedidoOcorrenciaMensagems; }
        public IQueryable<TpedidoOcorrencia> TpedidoOcorrencias { get => contexto.TpedidoOcorrencias; }
        public IQueryable<TpedidoItemDevolvidoBlocoNotas> TpedidoItemDevolvidoBlocoNotas { get => contexto.TpedidoItemDevolvidoBlocoNotas; }
        public IQueryable<TorcamentoItem> TorcamentoItems { get => contexto.TorcamentoItems; }
        public IQueryable<Tbanco> Tbancos { get => contexto.Tbancos; }
        public IQueryable<TclienteRefComercial> TclienteRefComercials { get => contexto.TclienteRefComercials; }
        public IQueryable<Tlog> Tlogs { get => contexto.Tlogs; }
        public IQueryable<Tloja> Tlojas { get => contexto.Tlojas; }
        public IQueryable<Tcontrole> Tcontroles { get => contexto.Tcontroles; }
        public IQueryable<TnfEmitente> TnfEmitentes { get => contexto.TnfEmitentes; }
        public IQueryable<TecProdutoComposto> TecProdutoCompostos { get => contexto.TecProdutoCompostos; }
        public IQueryable<Tfabricante> Tfabricantes { get => contexto.Tfabricantes; }
        public IQueryable<Tparametro> Tparametros { get => contexto.Tparametros; }
        public IQueryable<TpercentualCustoFinanceiroFornecedor> TpercentualCustoFinanceiroFornecedors { get => contexto.TpercentualCustoFinanceiroFornecedors; }
        public IQueryable<TprodutoXwmsRegraCd> TprodutoXwmsRegraCds { get => contexto.TprodutoXwmsRegraCds; }
        public IQueryable<TwmsRegraCd> TwmsRegraCds { get => contexto.TwmsRegraCds; }
        public IQueryable<TwmsRegraCdXUf> TwmsRegraCdXUfs { get => contexto.TwmsRegraCdXUfs; }
        public IQueryable<TwmsRegraCdXUfPessoa> TwmsRegraCdXUfPessoas { get => contexto.TwmsRegraCdXUfPessoas; }
        public IQueryable<TwmsRegraCdXUfXPessoaXCd> TwmsRegraCdXUfXPessoaXCds { get => contexto.TwmsRegraCdXUfXPessoaXCds; }
        public IQueryable<TecProdutoCompostoItem> TecProdutoCompostoItems { get => contexto.TecProdutoCompostoItems; }
        public IQueryable<Testoque> Testoques { get => contexto.Testoques; }
        public IQueryable<TestoqueItem> TestoqueItems { get => contexto.TestoqueItems; }
        public IQueryable<TprodutoXAlerta> TprodutoXAlertas { get => contexto.TprodutoXAlertas; }
        public IQueryable<TalertaProduto> TalertaProdutos { get => contexto.TalertaProdutos; }
        public IQueryable<TformaPagto> TformaPagtos { get => contexto.TformaPagtos; }
        public IQueryable<TorcamentistaEIndicadorRestricaoFormaPagto> torcamentistaEIndicadorRestricaoFormaPagtos { get => contexto.torcamentistaEIndicadorRestricaoFormaPagtos; }
    }
}
