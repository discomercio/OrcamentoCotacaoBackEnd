using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.EntityFrameworkCore;
using OrcamentoCotacao.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcamentoCotacao
{
    public class OrcamentoCotacaoData : BaseData<TorcamentoCotacao, TorcamentoCotacaoFiltro>
    {
        private readonly ContextoBdProvider _contextoProvider;

        public OrcamentoCotacaoData(ContextoBdProvider contextoProvider)
        {
            _contextoProvider = contextoProvider;
        }

        public TorcamentoCotacao Atualizar(TorcamentoCotacao obj)
        {
            using (var db = _contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                db.TorcamentoCotacao.Update(obj);
                db.SaveChanges();
                db.transacao.Commit();
            }

            return obj;
        }

        public OrcamentoCotacaoDto PorGuid(string guid)
        {
            try
            {
                using (var db = _contextoProvider.GetContextoLeitura())
                {
                    var orcamento = (from l in db.TorcamentoCotacaoLink
                                     join o in db.TorcamentoCotacao on l.IdOrcamentoCotacao equals o.Id
                                     join s in db.TcfgOrcamentoCotacaoStatus on o.Status equals s.Id
                                     join u in db.Tusuario on o.IdVendedor equals u.Id into gg
                                     from lj in gg.DefaultIfEmpty()
                                     where l.Guid == Guid.Parse(guid)
                                     orderby o.Id descending
                                     select new OrcamentoCotacaoDto
                                     {
                                         //Orcamento
                                         id = o.Id,
                                         nomeObra = o.NomeObra,
                                         validade = o.Validade,
                                         vendedor = lj.Usuario,
                                         idUsuarioCadastro = o.IdUsuarioCadastro,
                                         idTipoUsuarioContextoCadastro = o.IdTipoUsuarioContextoCadastro,
                                         idVendedor = o.IdVendedor,
                                         idIndicador = o.IdIndicador,
                                         idIndicadorVendedor = o.IdIndicadorVendedor,
                                         tipoCliente = o.TipoCliente,
                                         stEntregaImediata = o.StEtgImediata,
                                         dataEntregaImediata = o.PrevisaoEntregaData,
                                         status = o.Status,
                                         observacao = o.Observacao,
                                         contribuinteIcms = o.ContribuinteIcms,
                                         dataCadastro = o.DataCadastro,
                                         previsaoEntregaData = o.PrevisaoEntregaData,
                                         //Cliente
                                         nomeCliente = o.NomeCliente,
                                         email = o.Email,
                                         telefone = o.Telefone,
                                         uf = o.UF,
                                         statusDescricao = s.Descricao,
                                         statusOrcamentoCotacaoLink = l.Status,
                                         loja = o.Loja
                                     })
                            .FirstOrDefault();

                    return orcamento;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Excluir(TorcamentoCotacao obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacao Inserir(TorcamentoCotacao obj)
        {
            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
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
            var listaStatus = contextoBdGravacao.TcfgOrcamentoCotacaoStatus.ToList();
            var saida = from c in contextoBdGravacao.TorcamentoCotacao select c;

            if (saida == null) return null;

            saida = saida.OrderByDescending(a => a.DataHoraCadastro);

            if (obj.Id != 0)
            {
                saida = saida.Where(x => x.Id == obj.Id);
            }
            if (obj.LimitarData)
            {
                saida = saida.Where(x => x.DataCadastro > DateTime.Now.AddDays(-60));
            }
            if (!string.IsNullOrEmpty(obj.Loja))
            {
                saida = saida.Where(x => x.Loja == obj.Loja);
            }
            //if (!string.IsNullOrEmpty(obj.Vendedor))
            //{
            //    saida = saida.Where(x => x.IdVendedor == int.Parse(obj.Vendedor));
            //}
            //if (!string.IsNullOrEmpty(obj.Parceiro))
            //{
            //    saida = saida.Where(x => x.IdIndicador == int.Parse(obj.Parceiro));
            //}
            if (!string.IsNullOrEmpty(obj.Loja))
            {
                saida = saida.Where(x => x.Loja == obj.Loja);
            }
            if (obj.Tusuario)
            {
                saida = from c in saida
                        join u in contextoBdGravacao.Tusuario on c.IdVendedor equals u.Id
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

        public List<TorcamentoCotacao> PorFiltro(TorcamentoCotacaoFiltro obj)
        {
            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var saida = from c in db.TorcamentoCotacao select c;

                    if (saida == null)
                    {
                        return null;
                    }

                    saida = saida.OrderByDescending(a => a.DataHoraCadastro);

                    if (obj.StatusId != 0)
                    {
                        saida = saida.Where(x => x.Status == obj.StatusId);
                    }

                    if (obj.Id != 0)
                    {
                        saida = saida.Where(x => x.Id == obj.Id);
                    }

                    if (obj.LimitarData)
                    {
                        saida = saida.Where(x => x.DataCadastro > DateTime.Now.AddDays(-60));
                    }

                    if (obj.LimitarDataDashboard)
                    {
                        saida = saida.Where(x => x.Validade >= DateTime.Now.AddDays(-1));
                    }

                    if (!string.IsNullOrEmpty(obj.Loja))
                    {
                        saida = saida.Where(x => x.Loja == obj.Loja);
                    }

                    if (obj.IdVendedor != 0)
                    {
                        saida = saida.Where(x => x.IdVendedor == obj.IdVendedor);
                    }

                    if (obj.IdIndicador != 0)
                    {
                        saida = saida.Where(x => x.IdIndicador == obj.IdIndicador);
                    }

                    if (obj.IdIndicadorVendedor != 0)
                    {
                        saida = saida.Where(x => x.IdIndicadorVendedor == obj.IdIndicadorVendedor);
                    }

                    if (obj.Tusuario)
                    {
                        saida = from c in saida
                                join u in db.Tusuario on c.IdVendedor equals u.Id
                                join s in db.TcfgOrcamentoCotacaoStatus on c.Status equals s.Id
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
                                    StatusNome = s.Descricao,
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

        public TorcamentoCotacao AtualizarComTransacao(TorcamentoCotacao model, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.Update(model);
            contextoBdGravacao.SaveChanges();
            return model;
        }

        public void ExcluirComTransacao(TorcamentoCotacao obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public TcfgOrcamentoCotacaoStatus BuscarStatusParaOrcamentoCotacaoComtransacao(string status, ContextoBdGravacao dbGravacao)
        {
            var tcfgStatus = (from c in dbGravacao.TcfgOrcamentoCotacaoStatus
                              where c.Descricao.ToUpper() == status.ToUpper()
                              select c).FirstOrDefault();

            return tcfgStatus;
        }

        public IQueryable<Object> ConsultaGerencial(TorcamentoCotacaoConsultaGerencialFiltro filtro)
        {
            using (var db = _contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var saida = from o in db.TorcamentoCotacao
                            join u in db.Tusuario on o.IdVendedor equals u.Id
                            join p in db.TorcamentistaEindicador on o.IdIndicador equals p.IdIndicador into lf
                            from x in lf.DefaultIfEmpty()
                            select new
                            {
                                tOrcamentoCotacao = o,
                                tUsuario = u,
                                tOrcamentistaIndicador = x
                            };

                if (filtro.Lojas?.Count > 0) saida = saida.Where(x => filtro.Lojas.Contains(x.tOrcamentoCotacao.Loja));
                if (filtro.IdVendedor != 0) saida = saida.Where(x => x.tOrcamentoCotacao.IdVendedor == filtro.IdVendedor);
                if (filtro.ComParceiro != null)
                {
                    if ((bool)filtro.ComParceiro) saida = saida.Where(x => x.tOrcamentoCotacao.IdIndicador.HasValue);
                    if ((bool)!filtro.ComParceiro) saida = saida.Where(x => x.tOrcamentoCotacao.IdIndicador == null);
                }

                if (filtro.IdParceiro != 0) saida = saida.Where(x => x.tOrcamentoCotacao.IdIndicador == filtro.IdParceiro);
                if (filtro.IdVendedorParceiro != 0) saida = saida.Where(x => x.tOrcamentoCotacao.IdIndicadorVendedor == filtro.IdVendedorParceiro);
                if (!string.IsNullOrEmpty(filtro.Fabricante))
                {
                    saida = from s in saida
                            join op in db.TorcamentoCotacaoOpcao on s.tOrcamentoCotacao.Id equals op.IdOrcamentoCotacao
                            join pu in db.TorcamentoCotacaoItemUnificado on op.Id equals pu.IdOrcamentoCotacaoOpcao
                            join f in db.Tfabricante on pu.Fabricante equals f.Fabricante
                            where f.Fabricante == filtro.Fabricante
                            select s;
                }
                if (!string.IsNullOrEmpty(filtro.Grupo))
                {
                    saida = (from s in saida
                            join op in db.TorcamentoCotacaoOpcao on s.tOrcamentoCotacao.Id equals op.IdOrcamentoCotacao
                            join c in db.TorcamentoCotacaoItemUnificado on op.Id equals c.IdOrcamentoCotacaoOpcao
                            join d in db.TorcamentoCotacaoOpcaoItemAtomico on c.Id equals d.IdItemUnificado
                            join e in db.Tproduto on d.Produto equals e.Produto
                            join f in db.TprodutoGrupo on e.Grupo equals f.Codigo
                            where e.Grupo == filtro.Grupo
                            select s).Distinct();
                }
                if (filtro.Status?.Count() > 0) saida = saida.Where(x => filtro.Status.Contains(x.tOrcamentoCotacao.Status));
                if (filtro.DataCricaoInicio.HasValue) saida = saida.Where(x => x.tOrcamentoCotacao.DataCadastro >= filtro.DataCricaoInicio);
                if (filtro.DataCriacaoFim.HasValue) saida = saida.Where(x => x.tOrcamentoCotacao.DataCadastro <= filtro.DataCriacaoFim);
                if (filtro.DataCorrente.HasValue) saida = saida.Where(x => x.tOrcamentoCotacao.Validade.Date >= filtro.DataCorrente);
                if (filtro.Expirado) saida = saida.Where(x => x.tOrcamentoCotacao.Validade.Date < DateTime.Now.Date);
                if (filtro.MensagemPendente)
                {
                    saida = (from s in saida
                             join c in db.TorcamentoCotacaoMensagem on s.tOrcamentoCotacao.Id equals c.IdOrcamentoCotacao
                             join d in db.TorcamentoCotacaoMensagemStatus on c.Id equals d.IdOrcamentoCotacaoMensagem
                             where c.IdTipoUsuarioContextoRemetente == 4 &&
                                   d.Lida == false &&
                                   c.IdTipoUsuarioContextoDestinatario == d.IdTipoUsuarioContexto &&
                                   c.IdUsuarioDestinatario == d.IdUsuario
                             select s).Distinct();
                }
                if (!string.IsNullOrEmpty(filtro.NomeColunaOrdenacao))
                {
                    if (filtro.Ascendente) saida = saida.OrderBy(x => EF.Property<TorcamentoCotacao>(x.tOrcamentoCotacao, filtro.NomeColunaOrdenacao));
                    else saida = saida.OrderByDescending(x => EF.Property<TorcamentoCotacao>(x.tOrcamentoCotacao, filtro.NomeColunaOrdenacao));
                }

                return saida;
            }
        }
    }
}

