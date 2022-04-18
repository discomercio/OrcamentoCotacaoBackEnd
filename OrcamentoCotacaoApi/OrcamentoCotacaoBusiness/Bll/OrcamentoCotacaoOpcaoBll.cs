using AutoMapper;
using InfraBanco;
using InfraBanco.Modelos;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Collections.Generic;
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

        public OrcamentoCotacaoOpcaoBll(InfraBanco.ContextoBdProvider contextoBdProvider,
            OrcamentoCotacaoOpcao.OrcamentoCotacaoOpcaoBll orcamentoCotacaoOpcaoBll, IMapper mapper,
            ProdutoOrcamentoCotacaoBll produtoOrcamentoCotacaoBll)
        {
            this.contextoBdProvider = contextoBdProvider;
            this.orcamentoCotacaoOpcaoBll = orcamentoCotacaoOpcaoBll;
            this.mapper = mapper;
            this.produtoOrcamentoCotacaoBll = produtoOrcamentoCotacaoBll;
        }

        public List<OrcamentoOpcaoResponseViewModel> CadastrarOrcamentoCotacaoOpcoesComTransacao(List<OrcamentoOpcaoRequestViewModel> orcamentoOpcoes,
            int idOrcamentoCotacao, int idTipoUsuarioContextoCadastro, int idUsuarioCadastro, ContextoBdGravacao contextoBdGravacao)
        {
            List<OrcamentoOpcaoResponseViewModel> orcamentoOpcaoResponseViewModels = new List<OrcamentoOpcaoResponseViewModel>();

            int seq = 0;
            foreach (var opcao in orcamentoOpcoes)
            {
                seq++;
                TorcamentoCotacaoOpcao torcamentoCotacaoOpcao = MontarTorcamentoCotacaoOpcao(opcao, idOrcamentoCotacao,
                    idTipoUsuarioContextoCadastro, idUsuarioCadastro, seq);

                var opcaoResponse = orcamentoCotacaoOpcaoBll.InserirComTransacao(torcamentoCotacaoOpcao, contextoBdGravacao);

                if (torcamentoCotacaoOpcao.Id == 0) throw new ArgumentException("Ops! Não gerou Id na opção de orçamento!");

                orcamentoOpcaoResponseViewModels.Add(mapper.Map<OrcamentoOpcaoResponseViewModel>(torcamentoCotacaoOpcao));
                //Precisamos salvar o item 2
                //  2 - t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO - principal
                var produtos = produtoOrcamentoCotacaoBll.CadastrarOrcamentoCotacaoOpcaoProdutosComTransacao(opcao.ListaProdutos,
                    opcaoResponse.Id, contextoBdGravacao);

                //  2 - t_ORCAMENTO_COTACAO_OPCAO_PAGTO - principal


                //precisamos do item 2 para salvar o 3
                //  3 - t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN - usa t_ORCAMENTO_COTACAO_OPCAO_PAGTO.Id e t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO


            }

            return orcamentoOpcaoResponseViewModels;
        }

        private TorcamentoCotacaoOpcao MontarTorcamentoCotacaoOpcao(OrcamentoOpcaoRequestViewModel opcao, int idOrcamentoCotacao,
            int idTipoUsuarioContextoCadastro, int idUsuarioCadastro, int sequencia)
        {
            return new TorcamentoCotacaoOpcao()
            {
                IdOrcamentoCotacao = idOrcamentoCotacao,
                PercRT = opcao.PercRT,
                Sequencia = sequencia,
                IdTipoUsuarioContextoCadastro = idTipoUsuarioContextoCadastro,
                IdUsuarioCadastro = idUsuarioCadastro,
                DataCadastro = DateTime.Now.Date,
                DataHoraCadastro = DateTime.Now
            };
        }
    }
}
