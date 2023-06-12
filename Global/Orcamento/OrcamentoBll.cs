using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.EntityFrameworkCore;
using Orcamento.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public OrcamentoConsultaDto OrcamentoPorFiltro(TorcamentoFiltro filtro)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var paraterQuery = (from p in db.TcfgParametro
                                        join unp in db.TcfgUnidadeNegocioParametro on p.Id equals unp.IdCfgParametro
                                        where p.Id == 20
                                        select unp.Valor).FirstOrDefault();

                    if (filtro.DtInicio.HasValue && filtro.DtFim.HasValue)
                    {
                        if (filtro.DtFim.Value.Date < filtro.DtInicio.Value.Date)
                        {
                            return new OrcamentoConsultaDto()
                            {
                                Sucesso = false,
                                Mensagem = "Data 'Início da criação' não deve ser menor que data 'Fim da criação'!",
                                OrcamentoCotacaoLista = new List<OrcamentoCotacaoListaDto>(),
                                QtdeRegistros = 0
                            };
                        }

                        TimeSpan difference = filtro.DtFim.Value.Date - filtro.DtInicio.Value.Date;

                        if (difference.Days > Convert.ToInt32(paraterQuery))
                        {
                            return new OrcamentoConsultaDto()
                            {
                                Sucesso = false,
                                Mensagem = $"Número máximo de dias para o intervalo da consulta deve ser menor ou igual {paraterQuery}",
                                OrcamentoCotacaoLista = new List<OrcamentoCotacaoListaDto>(),
                                QtdeRegistros = 0
                            };
                        }
                    }

                    var query = from c in db.Torcamento

                                join v in db.Tusuario on c.Vendedor equals v.Usuario

                                join vp in db.TorcamentistaEIndicadorVendedor on c.IdIndicadorVendedor equals vp.Id into gj
                                from loj in gj.DefaultIfEmpty()

                                where
                                    c.Loja == filtro.Loja

                                select new OrcamentoCotacaoListaDto
                                {
                                    NumPedidoOrdenacao = Convert.ToInt32(c.Orcamento.Replace("Z", "")),
                                    NumeroOrcamento = !c.IdOrcamentoCotacao.HasValue || c.IdOrcamentoCotacao == 0 ? "-" : c.IdOrcamentoCotacao.Value.ToString(),
                                    NumPedido = c.Orcamento,
                                    Cliente_Obra = c.Endereco_nome,
                                    IdVendedor = v.Id,
                                    Vendedor = c.Vendedor == null ? "-" : c.Vendedor,
                                    Parceiro = c.Orcamentista == null ? "-" : c.Orcamentista,
                                    VendedorParceiro = loj.Nome,
                                    Valor = c.Permite_RA_Status == 1 ? c.Vl_Total_NF.Value.ToString() : c.Vl_Total.ToString(),
                                    Orcamentista = c.Orcamentista == null ? "" : c.Orcamentista,
                                    IdStatus = c.St_Orc_Virou_Pedido == 1 ? 1 : c.St_Orcamento != "CAN" ? 2 : 3,
                                    Status = c.St_Orc_Virou_Pedido == 1 ? "Pedido em andamento" : c.St_Orcamento != "CAN" ? "Pedido em processamento" : "Cancelado",
                                    VistoEm = "",
                                    IdIndicadorVendedor = c.IdIndicadorVendedor == null ? null : c.IdIndicadorVendedor,
                                    St_Orc_Virou_Pedido = c.St_Orc_Virou_Pedido,
                                    Mensagem = "",
                                    DtCadastro = c.Data,
                                    DtExpiracao = null,
                                    DtInicio = filtro.DtInicio,
                                    DtFim = filtro.DtFim,
                                    IdOrcamentoCotacao = !c.IdOrcamentoCotacao.HasValue || c.IdOrcamentoCotacao == 0 ? null : c.IdOrcamentoCotacao
                                };

                    #region Where

                    if (!string.IsNullOrEmpty(filtro.IdBaseBusca))
                    {
                        query = query.Where(x => Convert.ToInt32(x.NumPedidoOrdenacao) <= Convert.ToInt32(filtro.IdBaseBusca.Replace("Z", "")));
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
                            query = query.Where(f => f.IdOrcamentoCotacao == aux);
                        }
                        else
                        {
                            query = query.Where(f =>
                            f.Cliente_Obra.Contains(filtro.Nome_numero)
                            || f.NumPedido.Contains(filtro.Nome_numero));
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

                    if (!string.IsNullOrEmpty(filtro.VendedorParceiro))
                    {
                        query = query.Where(x => x.VendedorParceiro == filtro.VendedorParceiro);
                    }

                    if (filtro.DtInicio.HasValue)
                    {
                        query = query.Where(f => f.DtCadastro.Value.Date >= filtro.DtInicio.Value.Date);
                    }

                    if (filtro.DtFim.HasValue)
                    {
                        query = query.Where(f => f.DtCadastro.Value.Date <= filtro.DtFim.Value.Date);
                    }

                    #endregion

                    #region Ordenação

                    if (!string.IsNullOrWhiteSpace(filtro.NomeColunaOrdenacao))
                    {
                        switch (filtro.NomeColunaOrdenacao.ToUpper())
                        {
                            case "NUMPEDIDO":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => o.NumPedidoOrdenacao);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => o.NumPedidoOrdenacao);
                                }
                                break;
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
                                    query = query.OrderBy(o => o.Cliente_Obra);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => o.Cliente_Obra);
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
                            case "VALOR":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => Convert.ToDecimal(o.Valor));
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => Convert.ToDecimal(o.Valor));
                                }
                                break;
                        }
                    }
                    else
                    {
                        query = query.OrderByDescending(o => o.DtCadastro);
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

                    return new OrcamentoConsultaDto()
                    {
                        Sucesso = true,
                        OrcamentoCotacaoLista = result,
                        QtdeRegistros = qtdeRegistros
                    };
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
                        .Select(x => new TcfgSelectItem
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