using ClassesBase;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.EntityFrameworkCore;
using Orcamento.Dto;
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

        public OrcamentoCotacaoConsultaDto ConsultaOrcamento(TorcamentoFiltro filtro)
        {
            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var paraterQuery = (from p in db.TcfgParametro
                                       join unp in db.TcfgUnidadeNegocioParametro on p.Id equals unp.IdCfgParametro
                                       where p.Id == 20
                                        select unp.Valor).FirstOrDefault();

                    if (filtro.DtInicioExpiracao.HasValue && filtro.DtFimExpiracao.HasValue)
                    {
                        if (filtro.DtFimExpiracao.Value.Date < filtro.DtInicioExpiracao.Value.Date)
                        {
                            return new OrcamentoCotacaoConsultaDto()
                            {
                                Sucesso = false,
                                Mensagem = "Data 'Início da expiração' não deve ser menor que data 'Fim da expiração'!",
                                OrcamentoCotacaoLista = new List<OrcamentoCotacaoListaDto>(),
                                QtdeRegistros = 0
                            };
                        }

                        TimeSpan difference = filtro.DtFimExpiracao.Value.Date - filtro.DtInicioExpiracao.Value.Date;

                        if (difference.Days >= Convert.ToInt32(paraterQuery))
                        {
                            return new OrcamentoCotacaoConsultaDto()
                            {
                                Sucesso = false,
                                Mensagem = $"Número máximo de dias para o intervalo da consulta deve ser menor ou igual {paraterQuery}",
                                OrcamentoCotacaoLista = new List<OrcamentoCotacaoListaDto>(),
                                QtdeRegistros = 0
                            };
                        }
                    }

                    if (filtro.DtInicio.HasValue && filtro.DtFim.HasValue)
                    {
                        if (filtro.DtFim.Value.Date <= filtro.DtInicio.Value.Date)
                        {
                            return new OrcamentoCotacaoConsultaDto()
                            {
                                Sucesso = false,
                                Mensagem = "Data 'Fim da criação' não deve ser menor que data 'Início da criação'!",
                                OrcamentoCotacaoLista = new List<OrcamentoCotacaoListaDto>(),
                                QtdeRegistros = 0
                            };
                        }

                        TimeSpan difference = filtro.DtFim.Value.Date - filtro.DtInicio.Value.Date;

                        if (difference.Days > Convert.ToInt32(paraterQuery))
                        {
                            return new OrcamentoCotacaoConsultaDto()
                            {
                                Sucesso = false,
                                Mensagem = $"Número máximo de dias para o intervalo da consulta deve ser menor ou igual {paraterQuery}",
                                OrcamentoCotacaoLista = new List<OrcamentoCotacaoListaDto>(),
                                QtdeRegistros = 0
                            };
                        }
                    }

                    var query = from oc in db.TorcamentoCotacao

                                join s in db.TcfgOrcamentoCotacaoStatus on oc.Status equals s.Id

                                join v in db.Tusuario on oc.IdVendedor equals v.Id into vendedorTemp
                                from vendedor in vendedorTemp.DefaultIfEmpty()

                                join oei in db.TorcamentistaEindicador on oc.IdIndicador equals oei.IdIndicador into parceiroTemp
                                from parceiro in parceiroTemp.DefaultIfEmpty()

                                join oeiv in db.TorcamentistaEIndicadorVendedor on oc.IdIndicadorVendedor equals oeiv.Id into vendedorParceiroTemp
                                from vendedorParceiro in vendedorParceiroTemp.DefaultIfEmpty()

                                where oc.Status != (short)Constantes.eCfgOrcamentoCotacaoStatus.EXCLUIDO

                                select new Orcamento.Dto.OrcamentoCotacaoListaDto
                                {
                                    Loja = oc.Loja,
                                    NumeroOrcamento = oc.Id.ToString(),
                                    NumPedido = string.IsNullOrEmpty(oc.IdPedido) ? "-" : oc.IdPedido,
                                    NomeCliente = oc.NomeCliente,
                                    NomeObra = oc.NomeObra,
                                    Cliente_Obra = !string.IsNullOrEmpty(oc.NomeObra) ? $"{oc.NomeCliente} - {oc.NomeObra}" : oc.NomeCliente,
                                    IdVendedor = vendedor.Id,
                                    Vendedor = vendedor.Usuario,
                                    Parceiro = string.IsNullOrEmpty(parceiro.Apelido) ? "-" : parceiro.Apelido,
                                    VendedorParceiro = vendedorParceiro.Nome,
                                    IdIndicadorVendedor = vendedorParceiro.Id,
                                    Valor = "0",
                                    Status = (oc.Validade.Date < DateTime.Now.Date && s.Id == 1) ? "Expirado" : s.Descricao,
                                    IdStatus = (oc.Validade.Date < DateTime.Now.Date && s.Id == 1) ? 4 : s.Id,
                                    VistoEm = "",
                                    Mensagem = (from ocm in db.TorcamentoCotacaoMensagem
                                                join ocms in db.TorcamentoCotacaoMensagemStatus
                                                on ocm.Id equals ocms.IdOrcamentoCotacaoMensagem
                                                where ocm.IdOrcamentoCotacao == oc.Id && ocms.PendenciaTratada.Value == false
                                                orderby ocm.Id descending
                                                select ocm).Any() ? "Sim" : "Não",
                                    DtCadastro = oc.DataCadastro,
                                    DtExpiracao = oc.Validade,
                                    DtInicio = filtro.DtInicio,
                                    DtFim = filtro.DtFim,
                                    DataHoraCadastro = oc.DataHoraCadastro
                                };

                    #region Where
                    if (!string.IsNullOrEmpty(filtro.IdBaseBusca))
                    {
                        query = query.Where(x => Convert.ToInt32(x.NumeroOrcamento) <= Convert.ToInt32(filtro.IdBaseBusca));
                    }

                    if (!string.IsNullOrWhiteSpace(filtro.Loja))
                    {
                        query = query.Where(f => f.Loja == filtro.Loja);
                    }

                    if (filtro.Status != null && filtro.Status.Length > 0)
                    {
                        query = query.Where(f => filtro.Status.Contains(f.IdStatus.ToString()));
                    }

                    if (!string.IsNullOrWhiteSpace(filtro.Nome_numero))
                    {
                        int aux = 0;

                        if (int.TryParse(filtro.Nome_numero, out aux))
                        {
                            query = query.Where(f => f.NumeroOrcamento.Contains(aux.ToString()));
                        }
                        else
                        {
                            query = query.Where(f => 
                            f.NomeCliente.Contains(filtro.Nome_numero) 
                            || f.NomeObra.Contains(filtro.Nome_numero));
                        }
                    }

                    if (filtro.Vendedores != null && filtro.Vendedores.Length > 0)
                    {
                        query = query.Where(f => filtro.Vendedores.Contains(f.IdVendedor.ToString()));
                    }

                    if (!string.IsNullOrWhiteSpace(filtro.Vendedor))
                    {
                        query = query.Where(f => f.Vendedor == filtro.Vendedor);
                    }

                    if (filtro.Parceiros != null && filtro.Parceiros.Length > 0)
                    {
                        query = query.Where(f => filtro.Parceiros.Contains(f.Parceiro));
                    }

                    if (!string.IsNullOrEmpty(filtro.Parceiro))
                    {
                        query = query.Where(x => x.Parceiro == filtro.Parceiro);
                    }

                    if (filtro.VendedorParceiros != null && filtro.VendedorParceiros.Length > 0)
                    {
                        query = query.Where(f => filtro.VendedorParceiros.Contains(f.IdIndicadorVendedor.ToString()));
                    }

                    if(!string.IsNullOrEmpty(filtro.VendedorParceiro))
                    {
                        query = query.Where(x => x.VendedorParceiro == filtro.VendedorParceiro);
                    }

                    if (!string.IsNullOrWhiteSpace(filtro.Mensagem))
                    {
                        if(filtro.Mensagem == Constantes.SIM_TODAS)
                        {
                            query = query.Where(x => x.Mensagem == "Sim");
                        }
                        if(filtro.Mensagem == Constantes.SIM_SOMENTE_MINHAS)
                        {
                            query = query.Where(x => x.Parceiro == "-");
                            query = query.Where(x => x.IdVendedor == filtro.IdUsuario);
                            query = query.Where(x => x.Mensagem == "Sim");
                        }
                        if(filtro.Mensagem == Constantes.SIM_SOMENTE_TERCEIROS)
                        {
                            query = query.Where(x => x.Mensagem == "Sim");
                            query = query.Where(x => (x.IdVendedor == filtro.IdUsuario && x.Parceiro != "-") || x.IdVendedor != filtro.IdUsuario);
                            
                        }
                        if(filtro.Mensagem == Constantes.NAO)
                        {
                            query = query.Where(f => f.Mensagem == filtro.Mensagem);
                        }
                    }

                    if (filtro.DtInicioExpiracao.HasValue)
                    {
                        query = query.Where(f => f.DtExpiracao >= filtro.DtInicioExpiracao.Value);
                    }

                    if (filtro.DtFimExpiracao.HasValue)
                    {
                        query = query.Where(f => f.DtExpiracao <= filtro.DtFimExpiracao.Value);
                    }

                    if (filtro.DtInicio.HasValue)
                    {
                        query = query.Where(f => f.DtCadastro >= filtro.DtInicio.Value);
                    }

                    if (filtro.DtFim.HasValue)
                    {
                        query = query.Where(f => f.DtCadastro <= filtro.DtFim.Value);
                    }

                    #endregion

                    #region Ordenação

                    if (!string.IsNullOrWhiteSpace(filtro.NomeColunaOrdenacao))
                    {
                        switch (filtro.NomeColunaOrdenacao.ToUpper())
                        {
                            case "NUMEROORCAMENTO":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => Convert.ToInt32(o.NumeroOrcamento));
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => Convert.ToInt32(o.NumeroOrcamento));
                                }
                                break;
                            case "CLIENTE_OBRA":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => 
                                    !string.IsNullOrWhiteSpace(o.NomeCliente) ? o.NomeCliente 
                                    : !string.IsNullOrWhiteSpace(o.NomeObra) ? o.NomeObra : o.NumeroOrcamento);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o =>
                                    !string.IsNullOrWhiteSpace(o.NomeCliente) ? o.NomeCliente
                                    : !string.IsNullOrWhiteSpace(o.NomeObra) ? o.NomeObra : o.NumeroOrcamento);
                                }
                                break;

                            case "VENDEDOR":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => o.Vendedor);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => o.Vendedor);
                                }
                                break;

                            case "PARCEIRO":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => o.Parceiro);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => o.Parceiro);
                                }
                                break;

                            case "VENDEDORPARCEIRO":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => o.VendedorParceiro);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => o.VendedorParceiro);
                                }
                                break;

                            case "STATUS":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => o.Status);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => o.Status);
                                }
                                break;

                            case "MENSAGEM":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => o.Mensagem);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => o.Mensagem);
                                }
                                break;

                            case "DTEXPIRACAO":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => o.DtExpiracao);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => o.DtExpiracao);
                                }
                                break;

                            case "DTCADASTRO":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => o.DtCadastro);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => o.DtCadastro);
                                }
                                break;
                        }
                    }
                    else
                    {
                        query = query.OrderByDescending(o => o.DataHoraCadastro);
                    }

                    #endregion

                    #region Paginação

                    int qtdeRegistros = query.Count();

                    if (filtro.Exportar)
                    {
                        filtro.QtdeItensPagina = qtdeRegistros;
                    }

                    query = query
                            .Skip((filtro.Pagina) * filtro.QtdeItensPagina)
                            .Take(filtro.QtdeItensPagina);

                    #endregion

                    var result = query.ToList();

                    return new OrcamentoCotacaoConsultaDto()
                    {
                        Sucesso = true,
                        OrcamentoCotacaoLista = result,
                        QtdeRegistros = qtdeRegistros
                    };
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}