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

        public List<OrcamentoCotacaoListaDto> PorFiltro(TorcamentoFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    if (obj.Origem == "ORCAMENTOS")
                    {
                        return (from c in db.TorcamentoCotacao
                                where c.DataCadastro > DateTime.Now.AddDays(-60)
                                        && obj.Loja == c.Loja
                                orderby c.DataCadastro descending
                                select new OrcamentoCotacaoListaDto
                                {
                                    NumOrcamento = c.Id.ToString("0000"),
                                    NumPedido = null,
                                    Cliente_Obra = $"{c.NomeCliente} - {c.NomeObra}",
                                    Vendedor = "Vendedor",
                                    Parceiro = "Parceiro",
                                    VendedorParceiro = "VendedorParceiro",
                                    Valor = "0",
                                    Status = c.TorcamentoCotacaoStatus.Descricao,
                                    VistoEm = "",
                                    Mensagem = c.Status == 7 ? "Sim" : "Não",
                                    DtCadastro = c.DataCadastro
                                }).ToList();
                    }
                    else if (obj.Origem == "PENDENTES") //ORCAMENTOS
                    {
                        return (from c in db.TorcamentoCotacao
                                where c.DataCadastro > DateTime.Now.AddDays(-60)
                                        && obj.Loja == c.Loja
                                        && c.Status == 7 //PENDENTE
                                orderby c.DataCadastro descending
                                select new OrcamentoCotacaoListaDto
                                {
                                    NumOrcamento = c.Id.ToString("0000"),
                                    NumPedido = null,
                                    Cliente_Obra = $"{c.NomeCliente} - {c.NomeObra}",
                                    Vendedor = "Vendedor",
                                    Parceiro = "Parceiro",
                                    VendedorParceiro = "VendedorParceiro",
                                    Valor = "0",
                                    Status = c.TorcamentoCotacaoStatus.Descricao,
                                    VistoEm = "",
                                    Mensagem = c.Status == 7 ? "Sim" : "Não",
                                }).ToList();
                    }
                    else //if (obj.Origem == "PEDIDOS")
                    {
                        return (from c in db.Tpedidos
                                where c.Data > DateTime.Now.AddDays(-60) //&& c.Loja == obj.Loja
                                orderby c.Data descending
                                select new OrcamentoCotacaoListaDto
                                {
                                    NumOrcamento = c.Orcamento,
                                    NumPedido = c.Pedido,
                                    Cliente_Obra = $"{c.Tcliente.Nome}",
                                    Vendedor = c.Vendedor,
                                    Parceiro = "",
                                    VendedorParceiro = "",
                                    Valor = c.Vl_Total_Familia.ToString(),
                                    Status = c.St_Pagto,
                                    VistoEm = "",
                                    Mensagem = "Sim",
                                }).ToList();
                    }
                }
            } 
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<TorcamentoCotacaoStatus>> ObterListaStatus(TorcamentoFiltro obj)
        {
            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                if (obj.Origem == "ORCAMENTOS")
                {
                    return await db.TorcamentoCotacaoStatus
                            .OrderBy(x => x.Descricao)
                            .ToListAsync();
                }
                else //if (obj.Origem == "PENDENTES") //ORCAMENTOS
                {
                    return await db.TorcamentoCotacaoStatus
                            .Where(x => x.Descricao == "Pendente")
                            .ToListAsync();
                }
                //else //if (obj.Origem == "PEDIDOS")
                //{
                //    return (from c in db.Tpedidos
                //            select new TorcamentoCotacaoStatus
                //            {
                //                Id = c.st
                //            }).ToList();
                //}
            }
        }

        //public Torcamento Atualizar(Torcamento obj)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool Excluir(Torcamento obj)
        //{
        //    throw new NotImplementedException();
        //}

        //public Torcamento Inserir(Torcamento obj)
        //{
        //    throw new NotImplementedException();
        //}

        //public Torcamento GetById(string id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
