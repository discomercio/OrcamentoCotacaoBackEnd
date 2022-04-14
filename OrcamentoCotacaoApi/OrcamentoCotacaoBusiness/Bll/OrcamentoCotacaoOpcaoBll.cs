using AutoMapper;
using InfraBanco;
using InfraBanco.Modelos;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class OrcamentoCotacaoOpcaoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoBdProvider;
        private readonly OrcamentoCotacaoOpcao.OrcamentoCotacaoOpcaoBll orcamentoCotacaoOpcaoBll;
        private readonly IMapper mapper;

        public OrcamentoCotacaoOpcaoBll(InfraBanco.ContextoBdProvider contextoBdProvider,
            OrcamentoCotacaoOpcao.OrcamentoCotacaoOpcaoBll orcamentoCotacaoOpcaoBll, IMapper mapper)
        {
            this.contextoBdProvider = contextoBdProvider;
            this.orcamentoCotacaoOpcaoBll = orcamentoCotacaoOpcaoBll;
            this.mapper = mapper;
        }

        public List<OrcamentoOpcaoResponseViewModel> SalvarOrcamentoOpcoesComTransacao(List<OrcamentoOpcaoRequestViewModel> orcamentoOpcoes,
            int idOrcamentoCotacao, int idTipoUsuarioContextoCadastro, int idUsuarioCadastro, ContextoBdGravacao contextoBdGravacao)
        {
            List<OrcamentoOpcaoResponseViewModel> orcamentoOpcaoResponseViewModel = new List<OrcamentoOpcaoResponseViewModel>();

            //OrcamentoOpcaoRequestViewModel      
            //ListaProdutos
            //FormaPagto
            //PercRT


            int seq = 0;
            foreach (var opcao in orcamentoOpcoes)
            {
                seq++;
                TorcamentoCotacaoOpcao torcamentoCotacaoOpcao = MontarTorcamentoCotacaoOpcao(opcao, idOrcamentoCotacao,
                    idTipoUsuarioContextoCadastro, idUsuarioCadastro, seq);

                var retornoOpcao = orcamentoCotacaoOpcaoBll.InserirComTransacao(torcamentoCotacaoOpcao, contextoBdGravacao);

                if (torcamentoCotacaoOpcao.Id == 0) throw new ArgumentException("Ops! Não gerou Id na opção de orçamento!");

                orcamentoOpcaoResponseViewModel.Add(mapper.Map<OrcamentoOpcaoResponseViewModel>(torcamentoCotacaoOpcao));

                //  2 - t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO / t_ORCAMENTO_COTACAO_OPCAO_PAGTO - usa t_ORCAMENTO_COTACAO.Id
                //  3 - t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN - usa t_ORCAMENTO_COTACAO_OPCAO_PAGTO.Id
                //  3 - t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO - usa t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO.Id


                
            }

            return orcamentoOpcaoResponseViewModel;
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
