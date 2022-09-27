using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OrcamentoCotacaoMensagem
{
    public class OrcamentoCotacaoMensagemData
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public OrcamentoCotacaoMensagemData(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<List<TorcamentoCotacaoMensagem>> ObterListaMensagem(int IdOrcamentoCotacao)
        {
            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                return await db.TorcamentoCotacaoMensagem
                                           .Where(x => x.IdOrcamentoCotacao == IdOrcamentoCotacao)
                                           .OrderByDescending(x => x.Id)
                                           .ToListAsync();
            }
        }

        public async Task<List<TorcamentoCotacaoMensagem>> ObterListaMensagemPendente(int IdOrcamentoCotacao)
        {
            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                return await db.TorcamentoCotacaoMensagem
                               .Where(x => x.IdOrcamentoCotacao == IdOrcamentoCotacao &&
                                x.PendenciaTratada == false).ToListAsync();

            }
        }

        public int ObterQuantidadeMensagemPendente(int IdUsuarioRemetente)
        {
            var qtdMensagemPendente = 0;

            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    qtdMensagemPendente = db.TorcamentoCotacaoMensagem.Where(x => x.IdUsuarioRemetente == IdUsuarioRemetente && x.PendenciaTratada == false).GroupBy(x => x.IdOrcamentoCotacao).Count();
                }


            }
            catch (Exception e)
            {
                throw e;
            }

            return qtdMensagemPendente;
        }

        public TorcamentoCotacaoMensagem InserirComTransacao(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem, InfraBanco.ContextoBdGravacao contextoBdGravacao, TorcamentoCotacaoEmailQueue torcamentoCotacaoEmailQueue = null)
        {

            var orcamentoCotacaoMensagemModel = new TorcamentoCotacaoMensagem();

            orcamentoCotacaoMensagemModel.IdOrcamentoCotacao = orcamentoCotacaoMensagem.IdOrcamentoCotacao;
            orcamentoCotacaoMensagemModel.IdTipoUsuarioContextoRemetente = (Int16)orcamentoCotacaoMensagem.IdTipoUsuarioContextoRemetente;
            orcamentoCotacaoMensagemModel.IdUsuarioRemetente = orcamentoCotacaoMensagem.IdUsuarioRemetente;

            if (torcamentoCotacaoEmailQueue != null)
                orcamentoCotacaoMensagemModel.IdOrcamentoCotacaoEmailQueue = unchecked((int)torcamentoCotacaoEmailQueue.Id);

            orcamentoCotacaoMensagemModel.IdTipoUsuarioContextoDestinatario = (Int16)orcamentoCotacaoMensagem.IdTipoUsuarioContextoDestinatario;
            orcamentoCotacaoMensagemModel.IdUsuarioDestinatario = orcamentoCotacaoMensagem.IdUsuarioDestinatario;
            orcamentoCotacaoMensagemModel.PendenciaTratada = false;
            orcamentoCotacaoMensagemModel.Lida = false;
            orcamentoCotacaoMensagemModel.Mensagem = orcamentoCotacaoMensagem.Mensagem;
            orcamentoCotacaoMensagemModel.DataCadastro = DateTime.Now;
            orcamentoCotacaoMensagemModel.DataHoraCadastro = DateTime.Now;

            try
            {
                contextoBdGravacao.TorcamentoCotacaoMensagem.Add(orcamentoCotacaoMensagemModel);
                contextoBdGravacao.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }                                 

            return orcamentoCotacaoMensagemModel;
        }

        public bool MarcarLida(int IdOrcamentoCotacao, int idUsuarioRemetente)
        {
            var saida = false;

            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var orcamentoCotacaoMensagem = db.TorcamentoCotacaoMensagem.Where(item => item.IdOrcamentoCotacao == IdOrcamentoCotacao && item.IdUsuarioRemetente != idUsuarioRemetente);

                if (orcamentoCotacaoMensagem != null)
                {
                    foreach (var item in orcamentoCotacaoMensagem)
                    {
                        item.Lida = true;
                        item.DataHoraLida = DateTime.Now;
                        item.DataLida = DateTime.Now;
                    }
                }

                db.SaveChanges();
                db.transacao.Commit();
                saida = true;

            }

            return saida;
        }


        public bool MarcarPendencia(int IdOrcamentoCotacao)
        {
            var saida = false;

            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var orcamentoCotacaoMensagem = db.TorcamentoCotacaoMensagem.Where(item => item.IdOrcamentoCotacao == IdOrcamentoCotacao);

                if (orcamentoCotacaoMensagem != null)
                {
                    foreach (var item in orcamentoCotacaoMensagem)
                    {
                        item.PendenciaTratada = true;
                        item.DataHoraPendenciaTratada = DateTime.Now;
                        item.DataPendenciaTratada = DateTime.Now;
                    }
                }

                db.SaveChanges();
                db.transacao.Commit();
                saida = true;

            }

            return saida;
        }

        public bool DesmarcarPendencia(int IdOrcamentoCotacao)
        {
            var saida = false;

            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var orcamentoCotacaoMensagem = db.TorcamentoCotacaoMensagem.Where(item => item.IdOrcamentoCotacao == IdOrcamentoCotacao);

                if (orcamentoCotacaoMensagem != null)
                {
                    foreach (var item in orcamentoCotacaoMensagem)
                    {
                        item.PendenciaTratada = false;
                        item.DataHoraPendenciaTratada = null;
                        item.DataPendenciaTratada = null;
                    }
                }

                db.SaveChanges();
                db.transacao.Commit();
                saida = true;

            }

            return saida;
        }

    }
}
