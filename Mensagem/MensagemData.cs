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

        public bool EnviarMensagem(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem, InfraBanco.ContextoBdGravacao contextoBdGravacao, TorcamentoCotacaoEmailQueue torcamentoCotacaoEmailQueue = null)
        {
            var saida = false;

            try
            {

                if (torcamentoCotacaoEmailQueue != null)
                {

                    contextoBdGravacao.TorcamentoCotacaoMensagem.Add(
                        new TorcamentoCotacaoMensagem
                        {
                            IdOrcamentoCotacao = orcamentoCotacaoMensagem.IdOrcamentoCotacao,
                            IdTipoUsuarioContextoRemetente = (Int16)orcamentoCotacaoMensagem.IdTipoUsuarioContextoRemetente,
                            IdUsuarioRemetente = orcamentoCotacaoMensagem.IdUsuarioRemetente,
                            IdOrcamentoCotacaoEmailQueue = unchecked((int)torcamentoCotacaoEmailQueue.Id),
                            IdTipoUsuarioContextoDestinatario = (Int16)orcamentoCotacaoMensagem.IdTipoUsuarioContextoDestinatario,
                            IdUsuarioDestinatario = orcamentoCotacaoMensagem.IdUsuarioDestinatario,
                            PendenciaTratada = false,
                            Lida = false,
                            Mensagem = orcamentoCotacaoMensagem.Mensagem,
                            DataCadastro = DateTime.Now,
                            DataHoraCadastro = DateTime.Now
                        });
                }
                else
                {
                    contextoBdGravacao.TorcamentoCotacaoMensagem.Add(
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
                        });
                }

                contextoBdGravacao.SaveChanges();
                contextoBdGravacao.transacao.Commit();
                saida = true;            
            }
            catch (Exception e)
            {
                contextoBdGravacao.transacao.Rollback();
                throw new ArgumentException("Falha ao gravar orçamento!");
            }

            return saida;
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
