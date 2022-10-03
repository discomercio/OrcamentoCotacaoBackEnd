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

        public async Task<List<TorcamentoCotacaoMensagemFiltro>> ObterListaMensagem(int IdOrcamentoCotacao)
        {
            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {

                return await (from ocm in db.TorcamentoCotacaoMensagem
                                                      join ocms in db.TorcamentoCotacaoMensagemStatus on ocm.Id equals ocms.IdOrcamentoCotacaoMensagem
                                                      where ocm.IdOrcamentoCotacao == IdOrcamentoCotacao
                               select new TorcamentoCotacaoMensagemFiltro()
                               {
                                   Id = ocm.Id,
                                   IdTipoUsuarioContextoRemetente = ocm.IdTipoUsuarioContextoRemetente,
                                   IdUsuarioRemetente = ocm.IdUsuarioRemetente,
                                   IdTipoUsuarioContextoDestinatario = ocm.IdTipoUsuarioContextoDestinatario,
                                   IdUsuarioDestinatario = ocm.IdUsuarioDestinatario,
                                   Lida = ocms.Lida,
                                   DataLida = ocms.DataHoraLida,
                                   DataHoraLida = ocms.DataHoraLida,
                                   Mensagem = ocm.Mensagem,
                                   DataCadastro = ocm.DataCadastro,
                                   DataHoraCadastro = ocm.DataHoraCadastro,
                                   PendenciaTratada = ocms.PendenciaTratada,
                                   DataPendenciaTratada = ocms.DataHoraPendenciaTratada,
                                   DataHoraPendenciaTratada = ocms.DataHoraPendenciaTratada
                               })
                               .OrderByDescending(x => x.Id)
                               .ToListAsync();
            }
        }

        public async Task<List<TorcamentoCotacaoMensagemFiltro>> ObterListaMensagemPendente(int IdOrcamentoCotacao)
        {
            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {

                return await (from ocm in db.TorcamentoCotacaoMensagem
                              join ocms in db.TorcamentoCotacaoMensagemStatus on ocm.Id equals ocms.IdOrcamentoCotacaoMensagem
                              where ocm.IdOrcamentoCotacao == IdOrcamentoCotacao && ocms.PendenciaTratada == false
                              select new TorcamentoCotacaoMensagemFiltro()
                              {
                                  Id = ocm.Id,
                                  IdTipoUsuarioContextoRemetente = ocm.IdTipoUsuarioContextoRemetente,
                                  IdUsuarioRemetente = ocm.IdUsuarioRemetente,
                                  IdTipoUsuarioContextoDestinatario = ocm.IdTipoUsuarioContextoDestinatario,
                                  IdUsuarioDestinatario = ocm.IdUsuarioDestinatario,
                                  Lida = ocms.Lida,
                                  DataLida = ocms.DataHoraLida,
                                  DataHoraLida = ocms.DataHoraLida,
                                  Mensagem = ocm.Mensagem,
                                  DataCadastro = ocm.DataCadastro,
                                  DataHoraCadastro = ocm.DataHoraCadastro,
                                  PendenciaTratada = ocms.PendenciaTratada,
                                  DataPendenciaTratada = ocms.DataHoraPendenciaTratada,
                                  DataHoraPendenciaTratada = ocms.DataHoraPendenciaTratada
                              })
                               .OrderByDescending(x => x.Id)
                               .ToListAsync();
            }
        }

        public int ObterQuantidadeMensagemPendente(int IdUsuarioRemetente)
        {
            var qtdMensagemPendente = 0;

            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {

                    var tOrcamentoCotacaoMensagem = from ocm in db.TorcamentoCotacaoMensagem
                                join ocms in db.TorcamentoCotacaoMensagemStatus on ocm.Id equals ocms.IdOrcamentoCotacaoMensagem
                                where ocm.IdUsuarioRemetente == IdUsuarioRemetente && ocms.PendenciaTratada == false
                                group ocm by ocm.IdOrcamentoCotacao into g
                                select new
                                {
                                    count = g.Count()
                                };
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
                var orcamentoCotacaoMensagemStatus = (from ocm in db.TorcamentoCotacaoMensagem
                                                join ocms in db.TorcamentoCotacaoMensagemStatus on ocm.Id equals ocms.IdOrcamentoCotacaoMensagem
                                                
                                                where ocm.IdOrcamentoCotacao == IdOrcamentoCotacao && ocm.IdUsuarioRemetente != idUsuarioRemetente

                                                select new TorcamentoCotacaoMensagemStatus()
                                                {
                                                    Id = ocms.Id,
                                                    IdOrcamentoCotacaoMensagem = ocms.IdOrcamentoCotacaoMensagem,
                                                    IdTipoUsuarioContexto = ocms.IdTipoUsuarioContexto,
                                                    IdUsuario = ocms.IdUsuario,
                                                    Lida = ocms.Lida,
                                                    DataLida = ocms.DataLida,
                                                    DataHoraLida = ocms.DataHoraLida,
                                                    PendenciaTratada = ocms.PendenciaTratada,
                                                    DataPendenciaTratada = ocms.DataPendenciaTratada,
                                                    DataHoraPendenciaTratada = ocms.DataHoraPendenciaTratada
                                                });

                if (orcamentoCotacaoMensagemStatus != null)
                {
                    foreach (var item in orcamentoCotacaoMensagemStatus)
                    {
                        item.Lida = true;
                        item.DataHoraLida = DateTime.Now;
                        item.DataLida = DateTime.Now;
                        item.IdUsuario = idUsuarioRemetente;

                        db.TorcamentoCotacaoMensagemStatus.Update(item);
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
                var orcamentoCotacaoMensagemStatus = (from ocm in db.TorcamentoCotacaoMensagem
                                                join ocms in db.TorcamentoCotacaoMensagemStatus on ocm.Id equals ocms.IdOrcamentoCotacaoMensagem
                                                where ocm.IdOrcamentoCotacao == IdOrcamentoCotacao
                
                                                select new TorcamentoCotacaoMensagemStatus()
                                                {
                                                    Id = ocms.Id,
                                                    IdOrcamentoCotacaoMensagem = ocms.IdOrcamentoCotacaoMensagem,
                                                    IdTipoUsuarioContexto = ocms.IdTipoUsuarioContexto,
                                                    IdUsuario = ocms.IdUsuario,
                                                    Lida = ocms.Lida,
                                                    DataLida = ocms.DataLida,
                                                    DataHoraLida = ocms.DataHoraLida,
                                                    PendenciaTratada = ocms.PendenciaTratada,
                                                    DataPendenciaTratada = ocms.DataPendenciaTratada,
                                                    DataHoraPendenciaTratada = ocms.DataHoraPendenciaTratada
                                                });


                if (orcamentoCotacaoMensagemStatus != null)
                {
                    foreach (var item in orcamentoCotacaoMensagemStatus)
                    {

                        item.PendenciaTratada = true;
                        item.DataHoraPendenciaTratada = DateTime.Now;
                        item.DataPendenciaTratada = DateTime.Now;

                        db.TorcamentoCotacaoMensagemStatus.Update(item);
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
                var orcamentoCotacaoMensagemStatus = (from ocm in db.TorcamentoCotacaoMensagem
                                                      join ocms in db.TorcamentoCotacaoMensagemStatus on ocm.Id equals ocms.IdOrcamentoCotacaoMensagem
                                                      where ocm.IdOrcamentoCotacao == IdOrcamentoCotacao

                                                      select new TorcamentoCotacaoMensagemStatus()
                                                      {
                                                          Id = ocms.Id,
                                                          IdOrcamentoCotacaoMensagem = ocms.IdOrcamentoCotacaoMensagem,
                                                          IdTipoUsuarioContexto = ocms.IdTipoUsuarioContexto,
                                                          IdUsuario = ocms.IdUsuario,
                                                          Lida = ocms.Lida,
                                                          DataLida = ocms.DataLida,
                                                          DataHoraLida = ocms.DataHoraLida,
                                                          PendenciaTratada = ocms.PendenciaTratada,
                                                          DataPendenciaTratada = ocms.DataPendenciaTratada,
                                                          DataHoraPendenciaTratada = ocms.DataHoraPendenciaTratada
                                                      });

                if (orcamentoCotacaoMensagemStatus != null)
                {
                    foreach (var item in orcamentoCotacaoMensagemStatus)
                    {
                        item.PendenciaTratada = false;
                        item.DataHoraPendenciaTratada = null;
                        item.DataPendenciaTratada = null;

                        db.TorcamentoCotacaoMensagemStatus.Update(item);
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
