﻿using InfraBanco.Modelos;
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
                return await db.TorcamentoCotacaoMensagens
                                           .Where(x => x.IdOrcamentoCotacao == IdOrcamentoCotacao)
                                           .OrderByDescending(x => x.Id)
                                           .ToListAsync();
            }
        }

        public async Task<List<TorcamentoCotacaoMensagem>> ObterListaMensagemPendente(int IdOrcamentoCotacao, int IdUsuarioDestinatario)
        {
            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                return await db.TorcamentoCotacaoMensagens
                                           .Where(x => x.IdOrcamentoCotacao == IdOrcamentoCotacao &&
                                           x.Lida == false &&
                                           x.IdUsuarioDestinatario == IdUsuarioDestinatario)
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
                    db.TorcamentoCotacaoMensagens.Add(
                        new TorcamentoCotacaoMensagem
                        {
                            IdOrcamentoCotacao = orcamentoCotacaoMensagem.IdOrcamentoCotacao,
                            IdTipoUsuarioContextoRemetente = (Int16)orcamentoCotacaoMensagem.IdTipoUsuarioContextoRemetente,
                            IdUsuarioRemetente = orcamentoCotacaoMensagem.IdUsuarioRemetente,
                            IdTipoUsuarioContextoDestinatario = (Int16)orcamentoCotacaoMensagem.IdTipoUsuarioContextoDestinatario,
                            IdUsuarioDestinatario = orcamentoCotacaoMensagem.IdUsuarioDestinatario,
                            Lida = false,
                            Mensagem = orcamentoCotacaoMensagem.Mensagem,
                            DataCadastro = DateTime.Now,
                            DataHoraCadastro = DateTime.Now
                        }); ;

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

        public bool MarcarMensagemComoLida(int IdOrcamentoCotacao, int IdUsuarioDestinatario)
        {
            var saida = false;

            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var orcamentoCotacaoMensagem = db.TorcamentoCotacaoMensagens.Where(item => item.IdOrcamentoCotacao == IdOrcamentoCotacao && item.IdUsuarioDestinatario == IdUsuarioDestinatario);

                if (orcamentoCotacaoMensagem != null)
                {
                    foreach (var item in orcamentoCotacaoMensagem)
                    {
                        item.Lida = true;
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
