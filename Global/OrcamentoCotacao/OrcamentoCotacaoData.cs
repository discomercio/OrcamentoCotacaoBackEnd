using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
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
                                         idVendedor = o.IdVendedor,
                                         idIndicador = o.IdIndicador,
                                         idIndicadorVendedor = o.IdIndicadorVendedor,
                                         tipoCliente = o.TipoCliente,
                                         stEntregaImediata = o.StEtgImediata,
                                         dataEntregaImediata = o.PrevisaoEntregaData,
                                         status = o.Status,

                                         //Cliente
                                         nomeCliente = o.NomeCliente,
                                         email = o.Email,
                                         telefone = o.Telefone,
                                         uf = o.UF,
                                     })
                            .FirstOrDefault();

                    if (orcamento != null)
                    {
                        var u3 = db.TorcamentistaEindicadorVendedor.FirstOrDefault(x => x.Id == orcamento.idUsuarioCadastro)?.Nome;
                        var u2 = db.TorcamentistaEindicador.FirstOrDefault(x => x.IdIndicador == orcamento.idUsuarioCadastro)?.Apelido;
                        var u1 = db.Tusuario.FirstOrDefault(x => x.Id == orcamento.idUsuarioCadastro)?.Usuario;

                        orcamento.usuarioCadastro = u3 == null ? u2 == null ? u1 == null ? null : u1 : u2 : u3;

                        orcamento.vendedorParceiro = db.TorcamentistaEindicadorVendedor.FirstOrDefault(x => x.Id == orcamento.idIndicadorVendedor)?.Nome;
                        orcamento.parceiro = db.TorcamentistaEindicador.FirstOrDefault(x => x.IdIndicador == orcamento.idIndicador)?.Apelido;
                        orcamento.vendedor = db.Tusuario.FirstOrDefault(x => x.Id == orcamento.idVendedor)?.Usuario;

                        return orcamento;
                    }

                    return null;
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
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacao> PorFiltro(TorcamentoCotacaoFiltro obj)
        {
            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var listaStatus = db.TcfgOrcamentoCotacaoStatus.ToList();
                    var saida = from c in db.TorcamentoCotacao select c;

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
                                join u in db.Tusuario on c.IdVendedor equals u.Id
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

        public TorcamentoCotacao AtualizarComTransacao(TorcamentoCotacao model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(TorcamentoCotacao obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }
    }
}
