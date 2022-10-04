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
                                                            join d in db.Tusuario on c.IdVendedor equals d.Id
                                                            where c.DataCadastro > DateTime.Now.AddDays(-60)
                                                                    && c.Status != 7 //CANCELADOS
                                                                    && c.Loja == filtro.Loja
                                                            orderby c.DataCadastro descending
                                                            select new OrcamentoCotacaoListaDto
                                                            {
                                                                NumeroOrcamento = c.IdOrcamento,
                                                                NumPedido = c.IdPedido,
                                                                Cliente_Obra = $"{c.NomeCliente} - {c.NomeObra}",
                                                                Vendedor = d.Usuario,
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

                    /*
                    if (filtro.Origem == "ORCAMENTOS")
                    {
                        foreach (var item in saida)
                        {
                            if(db.TorcamentoCotacaoMensagem.Any(x=> 
                                x.IdUsuarioDestinatario == filtro.IdUsuario &&
                                x.Lida == false))
                            {
                                item.Mensagem = "Sim";
                            } else
                            {
                                item.Mensagem = "Não";
                            }
                        }
                    }*/

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
                    List<OrcamentoCotacaoListaDto> saida = (from c in db.Torcamento
                                                            join vp in db.TorcamentistaEIndicadorVendedor on c.IdIndicadorVendedor equals vp.Id into gj
                                                            from loj in gj.DefaultIfEmpty()
                                                            where c.Data > DateTime.Now.AddDays(-60)
                                                                    && c.St_Orcamento != "CAN" //CANCELADOS
                                                                    && c.Loja == filtro.Loja
                                                            orderby c.Data descending
                                                            select new OrcamentoCotacaoListaDto
                                                            {
                                                                NumeroOrcamento = !c.IdOrcamentoCotacao.HasValue || c.IdOrcamentoCotacao == 0 ? "-" : c.IdOrcamentoCotacao.Value.ToString(),
                                                                NumPedido = c.Orcamento,
                                                                Cliente_Obra = $"{c.Endereco_nome}",
                                                                Vendedor = c.Vendedor == null ? "-" : c.Vendedor,
                                                                Parceiro = c.Orcamentista == null ? "-" : c.Orcamentista,
                                                                VendedorParceiro = loj.Nome,
                                                                Valor = c.Permite_RA_Status == 1 ? c.Vl_Total_NF.Value.ToString() : c.Vl_Total.ToString(),
                                                                Orcamentista = c.Orcamentista == null ? "" : c.Orcamentista,
                                                                Status = c.St_Orc_Virou_Pedido == 1 ? "Pedido em andamento" : "Pedido em processamento",
                                                                VistoEm = "",
                                                                IdIndicadorVendedor = c.IdIndicadorVendedor == null ? null : c.IdIndicadorVendedor,
                                                                St_Orc_Virou_Pedido = c.St_Orc_Virou_Pedido,
                                                                Mensagem = "",
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
                //else if (obj.Origem == "PENDENTES") //ORCAMENTOS
                //{
                //    return TcfgOrcamentoStatus.ObterLista();
                //}
                else //if (obj.Origem == "PEDIDOS")
                {
                    return TcfgPedidoStatus.ObterLista();
                }
            }
        }

    }
}
