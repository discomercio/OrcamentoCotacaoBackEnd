using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;

namespace OrcamentoCotacao
{
    public class OrcamentoCotacaoData:BaseData<TorcamentoCotacao, TorcamentoCotacaoFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public OrcamentoCotacaoData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TorcamentoCotacao Atualizar(TorcamentoCotacao obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TorcamentoCotacao obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacao Inserir(TorcamentoCotacao obj)
        {
            try
            {
                using(var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    db.TorcamentoCotacao.Add(obj);
                    db.SaveChanges();
                    db.transacao.Commit();
                    return obj;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public TorcamentoCotacao InserirComTransacao(TorcamentoCotacao model, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.TorcamentoCotacao.Add(model);
            contextoBdGravacao.SaveChanges();
            return model;
        }

        public List<TorcamentoCotacao> PorFilroComTransacao(TorcamentoCotacao model, TorcamentoCotacao obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacao> PorFilroComTransacao(TorcamentoCotacaoFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacao> PorFiltro(TorcamentoCotacaoFiltro obj)
        {
            try
            {
                using(var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var listaStatus = db.TcfgOrcamentoCotacaoStatus.ToList();
                    var saida = from c in db.TorcamentoCotacao select c;

                    if (obj.LimitarData)
                    {
                        saida = saida.Where(x => x.DataCadastro > DateTime.Now.AddDays(-60));
                    }
                    if (!string.IsNullOrEmpty(obj.Loja))
                    {
                        saida = saida.Where(x => x.Loja == obj.Loja);
                    }
                    if (obj.Tusuario)
                    {
                        saida = from c in saida
                                join u in db.Tusuarios on c.IdVendedor equals u.Id
                                select new TorcamentoCotacao()
                                {
                                    Id = c.Id,
                                    AceiteWhatsApp = c.AceiteWhatsApp,
                                    DataCadastro = c.DataCadastro,
                                    DataHoraCadastro = c.DataHoraCadastro,
                                    DataHoraUltAtualizacao = c.DataHoraUltAtualizacao,
                                    DataHoraUltRenovacao = c.DataHoraUltRenovacao,
                                    DataHoraUltStatus = c.DataHoraUltStatus,
                                    DataUltStatus = c.DataUltStatus,
                                    Email = c.Email,
                                    GarantiaIndicadorStatus = c.GarantiaIndicadorStatus,
                                    IdIndicador = c.IdIndicador,
                                    IdIndicadorVendedor = c.IdIndicadorVendedor,
                                    IdOrcamento = c.IdOrcamento,
                                    IdPedido = c.IdPedido,
                                    IdTipoUsuarioContextoCadastro = c.IdTipoUsuarioContextoCadastro,
                                    IdTipoUsuarioContextoUltAtualizacao = c.IdTipoUsuarioContextoUltAtualizacao,
                                    IdTipoUsuarioContextoUltStatus = c.IdTipoUsuarioContextoUltStatus,
                                    IdUsuarioCadastro = c.IdUsuarioCadastro,
                                    IdUsuarioUltAtualizacao = c.IdUsuarioUltAtualizacao,
                                    IdUsuarioUltRenovacao = c.IdUsuarioUltRenovacao,
                                    IdUsuarioUltStatus = c.IdUsuarioUltStatus,
                                    IdVendedor = c.IdVendedor,
                                    InstaladorInstalaStatus = c.InstaladorInstalaStatus,
                                    Loja = c.Loja,
                                    NomeCliente = c.NomeCliente,
                                    NomeObra = c.NomeObra,
                                    Observacao = c.Observacao,
                                    PrevisaoEntregaData = c.PrevisaoEntregaData,
                                    QtdeRenovacao = c.QtdeRenovacao,
                                    Status = c.Status,
                                    StatusNome = listaStatus.FirstOrDefault(x => x.Id == c.Status).Descricao,
                                    StEtgImediata = c.StEtgImediata,
                                    Telefone = c.Telefone,
                                    TipoCliente = c.TipoCliente,
                                    UF = c.UF,
                                    Validade = c.Validade,
                                    ValidadeAnterior = c.ValidadeAnterior,
                                    Tusuarios = u
                                };
                    }

                    return saida.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
