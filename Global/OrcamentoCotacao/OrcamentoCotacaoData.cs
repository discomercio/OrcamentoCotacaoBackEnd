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
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacao> PorFiltro(TorcamentoCotacaoFiltro obj)
        {
            try
            {
                using(var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var saida = from c in db.TorcamentoCotacao
                                select c;

                    if (obj.LimitarData)
                    {
                        saida = saida.Where(x => x.DataCadastro > DateTime.Now.AddDays(-60));
                    }
                    if (!string.IsNullOrEmpty(obj.Loja))
                    {
                        saida = saida.Where(x => x.Loja == obj.Loja);
                    }
                    if (!string.IsNullOrEmpty(obj.Status))
                    {
                        saida = saida.Include(x => x.TcfgOrcamentoCotacaoStatus.Equals(obj.Status));
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
                                    StEtgImediata = c.StEtgImediata,
                                    Telefone = c.Telefone,
                                    TipoCliente = c.TipoCliente,
                                    UF = c.UF,
                                    Validade = c.Validade,
                                    ValidadeAnterior = c.ValidadeAnterior,
                                    Tusuario = u
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
