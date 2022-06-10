using AutoMapper;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using OrcamentoCotacao.Dto;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
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

        public OrcamentoCotacaoOpcaoBll(InfraBanco.ContextoBdProvider contextoBdProvider,
            OrcamentoCotacaoOpcao.OrcamentoCotacaoOpcaoBll orcamentoCotacaoOpcaoBll, IMapper mapper,
            ProdutoOrcamentoCotacaoBll produtoOrcamentoCotacaoBll, FormaPagtoOrcamentoCotacaoBll formaPagtoOrcamentoCotacaoBll)
        {
            this.contextoBdProvider = contextoBdProvider;
            this.orcamentoCotacaoOpcaoBll = orcamentoCotacaoOpcaoBll;
            this.mapper = mapper;
            this.produtoOrcamentoCotacaoBll = produtoOrcamentoCotacaoBll;
            this.formaPagtoOrcamentoCotacaoBll = formaPagtoOrcamentoCotacaoBll;
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
