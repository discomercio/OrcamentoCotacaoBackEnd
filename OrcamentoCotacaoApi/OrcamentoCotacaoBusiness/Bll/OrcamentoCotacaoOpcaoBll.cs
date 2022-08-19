using AutoMapper;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using Loja;
using Loja.Dados;
using OrcamentoCotacao.Dto;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class OrcamentoCotacaoOpcaoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoBdProvider;
        private readonly OrcamentoCotacaoOpcao.OrcamentoCotacaoOpcaoBll orcamentoCotacaoOpcaoBll;
        private readonly IMapper mapper;
        private readonly ProdutoOrcamentoCotacaoBll produtoOrcamentoCotacaoBll;
        private readonly FormaPagtoOrcamentoCotacaoBll formaPagtoOrcamentoCotacaoBll;
        private readonly LojaBll _lojaBll;

        public OrcamentoCotacaoOpcaoBll(InfraBanco.ContextoBdProvider contextoBdProvider,
            OrcamentoCotacaoOpcao.OrcamentoCotacaoOpcaoBll orcamentoCotacaoOpcaoBll, IMapper mapper,
            ProdutoOrcamentoCotacaoBll produtoOrcamentoCotacaoBll,
            FormaPagtoOrcamentoCotacaoBll formaPagtoOrcamentoCotacaoBll,
            LojaBll _lojaBll)
        {
            this.contextoBdProvider = contextoBdProvider;
            this.orcamentoCotacaoOpcaoBll = orcamentoCotacaoOpcaoBll;
            this.mapper = mapper;
            this.produtoOrcamentoCotacaoBll = produtoOrcamentoCotacaoBll;
            this.formaPagtoOrcamentoCotacaoBll = formaPagtoOrcamentoCotacaoBll;
            this._lojaBll = _lojaBll;
        }

        public List<OrcamentoOpcaoResponseViewModel> CadastrarOrcamentoCotacaoOpcoesComTransacao(List<OrcamentoOpcaoRequestViewModel> orcamentoOpcoes,
            int idOrcamentoCotacao, UsuarioLogin usuarioLogado, ContextoBdGravacao contextoBdGravacao, string loja)
        {
            List<OrcamentoOpcaoResponseViewModel> orcamentoOpcaoResponseViewModels = new List<OrcamentoOpcaoResponseViewModel>();

            int seq = 0;
            foreach (var opcao in orcamentoOpcoes)
            {
                seq++;
                TorcamentoCotacaoOpcao torcamentoCotacaoOpcao = MontarTorcamentoCotacaoOpcao(opcao, idOrcamentoCotacao,
                    usuarioLogado, seq);

                var opcaoResponse = orcamentoCotacaoOpcaoBll.InserirComTransacao(torcamentoCotacaoOpcao, contextoBdGravacao);

                if (torcamentoCotacaoOpcao.Id == 0) throw new ArgumentException("Ops! Não gerou Id na opção de orçamento!");

                orcamentoOpcaoResponseViewModels.Add(mapper.Map<OrcamentoOpcaoResponseViewModel>(torcamentoCotacaoOpcao));

                var tOrcamentoCotacaoOpcaoPagtos = formaPagtoOrcamentoCotacaoBll.CadastrarOrcamentoCotacaoOpcaoPagtoComTransacao(opcao.FormaPagto, opcaoResponse.Id, contextoBdGravacao);

                var tOrcamentoCotacaoItemUnificados = produtoOrcamentoCotacaoBll.CadastrarOrcamentoCotacaoOpcaoProdutosUnificadosComTransacao(opcao,
                    opcaoResponse.Id, loja, contextoBdGravacao);

                if (tOrcamentoCotacaoOpcaoPagtos.Count == 0 || tOrcamentoCotacaoItemUnificados.Count == 0)
                    throw new ArgumentException("Ops! Não gerou Id ao salvar os pagamentos e produtos!");

                var tOrcamentoCotacaoOpcaoItemAtomicoCustoFin = produtoOrcamentoCotacaoBll.CadastrarProdutoAtomicoCustoFinComTransacao(tOrcamentoCotacaoOpcaoPagtos,
                    tOrcamentoCotacaoItemUnificados, opcao.ListaProdutos, loja, contextoBdGravacao);
            }

            return orcamentoOpcaoResponseViewModels;
        }

        private TorcamentoCotacaoOpcao MontarTorcamentoCotacaoOpcao(OrcamentoOpcaoRequestViewModel opcao, int idOrcamentoCotacao,
            UsuarioLogin usuarioLogado, int sequencia)
        {
            return new TorcamentoCotacaoOpcao()
            {
                IdOrcamentoCotacao = idOrcamentoCotacao,
                PercRT = opcao.PercRT,
                Sequencia = sequencia,
                IdTipoUsuarioContextoCadastro = (int)usuarioLogado.TipoUsuario,
                IdUsuarioCadastro = usuarioLogado.Id,
                DataCadastro = DateTime.Now.Date,
                DataHoraCadastro = DateTime.Now
            };


        }

        public void AtualizarOrcamentoOpcao(OrcamentoOpcaoResponseViewModel opcao, UsuarioLogin usuarioLogado,
            OrcamentoResponseViewModel orcamento)
        {
            var lstOpcaoAntiga = orcamentoCotacaoOpcaoBll.PorFiltro(new TorcamentoCotacaoOpcaoFiltro() { IdOrcamentoCotacao = opcao.IdOrcamentoCotacao });
            
            if (lstOpcaoAntiga == null) throw new ArgumentNullException("Falha ao buscar opção de orçamento");

            var opcaoAntiga = lstOpcaoAntiga.Where(x => x.Id == opcao.Id).FirstOrDefault();

            var opcaoNovo = new TorcamentoCotacaoOpcao()
            {
                Id = opcao.Id,
                IdOrcamentoCotacao = opcao.IdOrcamentoCotacao,
                PercRT = opcao.PercRT,
                Sequencia = opcao.Sequencia,
                DataCadastro = opcaoAntiga.DataCadastro,
                DataHoraCadastro = opcaoAntiga.DataHoraCadastro,
                IdUsuarioCadastro = opcaoAntiga.IdUsuarioCadastro,
                IdTipoUsuarioContextoCadastro = opcaoAntiga.IdTipoUsuarioContextoCadastro,
                IdTipoUsuarioContextoUltAtualizacao = (int)usuarioLogado.TipoUsuario,
                DataHoraUltAtualizacao = DateTime.Now,
                IdUsuarioUltAtualizacao = usuarioLogado.Id,
            };

            var formaPagtoAntiga = formaPagtoOrcamentoCotacaoBll.BuscarOpcaoFormasPagtos(opcao.Id);
            if (formaPagtoAntiga == null) throw new ArgumentException("Falha ao busca formas de pagamentos da opção!");

            using (var dbGravacao = contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    var tOpcao = orcamentoCotacaoOpcaoBll.AtualizarComTransacao(opcaoNovo, dbGravacao); 

                    tOpcao.TorcamentoCotacaoItemUnificados = produtoOrcamentoCotacaoBll
                        .AtualizarOrcamentoCotacaoOpcaoProdutosUnificadosComTransacao(opcao.ListaProdutos, opcao.Id, dbGravacao);
                    if (tOpcao.TorcamentoCotacaoItemUnificados == null) throw new ArgumentException("Falha ao atualizar os itens!");

                    tOpcao.TorcamentoCotacaoItemUnificados = produtoOrcamentoCotacaoBll
                        .AtualizarTorcamentoCotacaoOpcaoItemAtomicoComTransacao(opcao.ListaProdutos, opcao.Id,
                        tOpcao.TorcamentoCotacaoItemUnificados, dbGravacao);
                    if (tOpcao.TorcamentoCotacaoItemUnificados == null) throw new ArgumentException("Falha ao atualizar os itens!");

                    var topcaoPagtos = formaPagtoOrcamentoCotacaoBll
                        .AtualizarOrcamentoCotacaoOpcaoPagtoComTransacao(opcao, formaPagtoAntiga, dbGravacao);
                    if (topcaoPagtos == null) throw new ArgumentException("Falha ao atualizar as formas de pagamentos!");
                    
                    produtoOrcamentoCotacaoBll.AtualizarProdutoAtomicoCustoFinComTransacao(opcao, 
                        tOpcao.TorcamentoCotacaoItemUnificados, topcaoPagtos, dbGravacao, usuarioLogado, orcamento);

                    dbGravacao.transacao.Commit();
                }
                catch (Exception ex)
                {
                    dbGravacao.transacao.Rollback();
                    throw;
                }
            }
        }

        public List<OrcamentoOpcaoResponseViewModel> PorFiltro(TorcamentoCotacaoOpcaoFiltro filtro)
        {
            var orcamentoOpcoes = orcamentoCotacaoOpcaoBll.PorFiltro(filtro);

            if (orcamentoOpcoes == null) throw new ArgumentException("Falha ao buscar as opções do orçamento!");

            List<OrcamentoOpcaoResponseViewModel> orcamentoOpcoesResponse = new List<OrcamentoOpcaoResponseViewModel>();

            foreach (var opcao in orcamentoOpcoes)
            {
                var opcaoFormaPagtos = formaPagtoOrcamentoCotacaoBll.BuscarOpcaoFormasPagtos(opcao.Id);

                if (opcaoFormaPagtos == null) throw new ArgumentException("Pagamento da opção não encontrado!");

                var itensOpcao = produtoOrcamentoCotacaoBll.BuscarOpcaoProdutos(opcao.Id).Result;

                if (itensOpcao == null) throw new ArgumentException("Produtos da opção não encontrados!");

                OrcamentoOpcaoResponseViewModel opcaoResponse = new OrcamentoOpcaoResponseViewModel()
                {
                    Id = opcao.Id,
                    IdOrcamentoCotacao = filtro.IdOrcamentoCotacao,
                    PercRT = opcao.PercRT,
                    Sequencia = opcao.Sequencia,
                    VlTotal = itensOpcao.Sum(x => x.TotalItem),
                    ListaProdutos = itensOpcao,
                    FormaPagto = mapper.Map<List<FormaPagtoCriacaoResponseViewModel>>(opcaoFormaPagtos)
                };

                orcamentoOpcoesResponse.Add(opcaoResponse);
            }

            return orcamentoOpcoesResponse;
        }
    }
}
