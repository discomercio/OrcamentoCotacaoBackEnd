using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;

namespace Vendedor
{
    public class VendedorBll
    {
        private readonly ContextoBdProvider contextoProvider;

        public VendedorBll(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        //public List<Tvendedor> PorFiltro(TvendedorFiltro obj)
        //{
        //    try
        //    {
        //        using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
        //        {
        //            return (from c in db.Tvendedor
        //                    where c.DataCadastro > DateTime.Now.AddDays(-60)
        //                            && obj.Loja.Contains(c.IdLoja)
        //                    orderby c.DataCadastro descending
        //                    select new OrcamentoCotacaoListaDto
        //                    {
        //                        NumOrcamento = c.Id.ToString("0000"),
        //                        NumPedido = null,
        //                        Cliente_Obra = $"{c.NomeCliente} - {c.NomeObra}",
        //                        Vendedor = c.Vendedor,
        //                        Parceiro = c.Parceiro,
        //                        ParceiroVendedor = c.VendedorParceiro,
        //                        Valor = "0",
        //                        Status = c.TorcamentoCotacaoStatus.Descricao,
        //                        VistoEm = "",
        //                        Pendente = c.IdStatus == 7 ? "Sim" : "Não",
        //                    }).ToList();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
    }
}

