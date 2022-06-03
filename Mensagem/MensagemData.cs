using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mensagem
{
    public class MensagemData
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public MensagemData(InfraBanco.ContextoBdProvider contextoProvider)
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
                                           x.PendenciaTratada == null ||
                                           x.PendenciaTratada == false
                                           )
                                           .ToListAsync();
            }
        }

        public bool EnviarMensagem(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem)
        {
            var saida = false;

            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    db.TorcamentoCotacaoMensagem.Add(
                        new TorcamentoCotacaoMensagem
                        {
                            IdOrcamentoCotacao = orcamentoCotacaoMensagem.IdOrcamentoCotacao,
                            IdTipoUsuarioContextoRemetente = (Int16)orcamentoCotacaoMensagem.IdTipoUsuarioContextoRemetente,
                            IdUsuarioRemetente = orcamentoCotacaoMensagem.IdUsuarioRemetente,
                            IdTipoUsuarioContextoDestinatario = (Int16)orcamentoCotacaoMensagem.IdTipoUsuarioContextoDestinatario,
                            IdUsuarioDestinatario = orcamentoCotacaoMensagem.IdUsuarioDestinatario,
                            PendenciaTratada = false,
                            Lida = false,
                            Mensagem = orcamentoCotacaoMensagem.Mensagem,
                            DataCadastro = DateTime.Now,
                            DataHoraCadastro = DateTime.Now
                        }); ; ;

                    db.SaveChanges();
                    db.transacao.Commit();
                    saida = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return saida;
        }

        public bool MarcarMensagemComoLida(int IdOrcamentoCotacao)
        {
            var saida = false;

            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var orcamentoCotacaoMensagem = db.TorcamentoCotacaoMensagem.Where(item => item.IdOrcamentoCotacao == IdOrcamentoCotacao && item.IdUsuarioDestinatario == IdUsuarioDestinatario);

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

        public bool MarcarMensagemPendenciaTratada(int IdOrcamentoCotacao)
        {
            var saida = false;

            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var orcamentoCotacaoMensagem = db.TorcamentoCotacaoMensagens.Where(item => item.IdOrcamentoCotacao == IdOrcamentoCotacao);

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

    }
}
