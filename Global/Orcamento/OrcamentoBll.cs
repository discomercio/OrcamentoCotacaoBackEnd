using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.EntityFrameworkCore;
using Orcamento.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orcamento
{
    public class OrcamentoBll //: BaseData<Torcamento, TorcamentoFiltro>
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public OrcamentoBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public List<OrcamentoCotacaoListaDto> OrcamentoCotacaoPorFiltro(TorcamentoFiltro filtro)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    List<OrcamentoCotacaoListaDto> saida = (from c in db.TorcamentoCotacao
                                                            where c.DataCadastro > DateTime.Now.AddDays(-60)
                                                                    && c.Status != 7 //CANCELADOS
                                                                    && c.Loja == filtro.Loja
                                                            orderby c.DataCadastro descending
                                                            select new OrcamentoCotacaoListaDto
                                                            {
                                                                NumeroOrcamento = c.IdOrcamento,
                                                                NumPedido = c.IdPedido,
                                                                Cliente_Obra = $"{c.NomeCliente} - {c.NomeObra}",
                                                                Vendedor = c.IdVendedor,
                                                                Parceiro = "Parceiro",
                                                                VendedorParceiro = "VendedorParceiro",
                                                                Valor = "0",
                                                                Status = c.Status.ToString(),
                                                                VistoEm = "",
                                                                Mensagem = c.Status == 7 ? "Sim" : "Não",
                                                                DtCadastro = c.DataCadastro,
                                                                DtExpiracao = c.Validade,
                                                                DtInicio = filtro.DtInicio,
                                                                DtFim = filtro.DtFim
                                                            }).ToList();

                    if (filtro.Status?.Length > 0)
                        saida = saida.Where(x => filtro.Status.Contains(x.Status)).ToList();

                    if (!String.IsNullOrEmpty(filtro.NumeroOrcamento))
                        saida = saida.Where(x => x.NumeroOrcamento == filtro.NumeroOrcamento).ToList();

                    if (!String.IsNullOrEmpty(filtro.Parceiro))
                        saida = saida.Where(x => x.Parceiro == filtro.Parceiro).ToList();

                    if (!String.IsNullOrEmpty(filtro.Vendedor))
                        saida = saida.Where(x => x.Vendedor == filtro.Vendedor).ToList();

                    if (!String.IsNullOrEmpty(filtro.VendedorParceiro))
                        saida = saida.Where(x => x.VendedorParceiro == filtro.VendedorParceiro).ToList();

                    if (filtro.DtInicio.HasValue && filtro.DtFim.HasValue)
                        saida = saida.Where(x => filtro.DtInicio.Value >= x.DtInicio.Value && filtro.DtFim.Value <= x.DtFim.Value).ToList();

                    return saida;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<OrcamentoCotacaoListaDto> OrcamentoPorFiltro(TorcamentoFiltro filtro)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    List<OrcamentoCotacaoListaDto> saida = (from c in db.Torcamentos
                                                            where c.Data > DateTime.Now.AddDays(-60)
                                                                    && c.St_Orcamento != "CAN" //CANCELADOS
                                                                    && c.Loja == filtro.Loja
                                                            orderby c.Data descending
                                                            select new OrcamentoCotacaoListaDto
                                                            {
                                                                NumeroOrcamento = c.Orcamento.ToString(),
                                                                NumPedido = c.Pedido,
                                                                Cliente_Obra = $"{c.Tcliente.Nome}",
                                                                Vendedor = c.Vendedor,
                                                                Parceiro = c.Orcamentista,
                                                                VendedorParceiro = "VendedorParceiro",
                                                                Valor = c.Vl_Total_NF.ToString(),
                                                                Orcamentista = c.Orcamentista,
                                                                Status = c.St_Orcamento,
                                                                VistoEm = "",
                                                                IdIndicadorVendedor = c.IdIndicadorVendedor,
                                                                Mensagem = c.St_Orcamento == "7" ? "Sim" : "Não",
                                                                DtCadastro = c.Data,
                                                                DtExpiracao = null,
                                                                DtInicio = filtro.DtInicio,
                                                                DtFim = filtro.DtFim
                                                            }).ToList();

                    if (filtro.Status?.Length > 0)
                        saida = saida.Where(x => filtro.Status.Contains(x.Status)).ToList();

                    if (!String.IsNullOrEmpty(filtro.NumeroOrcamento))
                        saida = saida.Where(x => x.NumeroOrcamento == filtro.NumeroOrcamento).ToList();

                    if (!String.IsNullOrEmpty(filtro.Parceiro))
                        saida = saida.Where(x => x.Parceiro == filtro.Parceiro).ToList();

                    if (!String.IsNullOrEmpty(filtro.Vendedor))
                        saida = saida.Where(x => x.Vendedor == filtro.Vendedor).ToList();

                    if (!String.IsNullOrEmpty(filtro.VendedorParceiro))
                        saida = saida.Where(x => x.VendedorParceiro == filtro.VendedorParceiro).ToList();

                    if (filtro.DtInicio.HasValue && filtro.DtFim.HasValue)
                        saida = saida.Where(x => filtro.DtInicio.Value >= x.DtInicio.Value && filtro.DtFim.Value <= x.DtFim.Value).ToList();

                    return saida;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<TcfgSelectItem>> ObterListaStatus(TorcamentoFiltro obj)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                if (obj.Origem == "ORCAMENTOS")
                {
                    return await db.TcfgOrcamentoCotacaoStatus
                        .OrderBy(x => x.Id)
                        .Select(x=> new TcfgSelectItem
                        {
                            Id = x.Id.ToString(),
                            Value = x.Descricao
                        })
                        .ToListAsync();
                }
                else if (obj.Origem == "PENDENTES") //ORCAMENTOS
                {
                    return TcfgOrcamentoStatus.ObterLista();
                }
                else //if (obj.Origem == "PEDIDOS")
                {
                    return TcfgPedidoStatus.ObterLista();
                }
            }
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
                                           .Where(x => x.IdOrcamentoCotacao == IdOrcamentoCotacao && x.Lida == false && x.IdUsuarioDestinatario == IdUsuarioDestinatario)
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
