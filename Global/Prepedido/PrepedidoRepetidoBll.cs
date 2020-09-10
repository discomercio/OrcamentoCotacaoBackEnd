using InfraBanco;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UtilsGlobais;
using Prepedido.Dados.DetalhesPrepedido;

namespace Prepedido
{
    public class PrepedidoRepetidoBll
    {
        private readonly ContextoBdProvider contextoProvider;

        public PrepedidoRepetidoBll(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<string> PrepedidoJaCadastradoCriterioSiteColors(PrePedidoDados prePedido)
        {
            var ret = await PrepedidoJaCadastradoDesdeData(prePedido, DateTime.Now.AddMinutes(-10)); //no máxio há 10 minutos
            if (ret.Count == 0)
                return null;
            return ret[0];
        }

        public async Task<List<string>> PrepedidoJaCadastradoDesdeData(PrePedidoDados prePedido, DateTime dataLimite)
        {
            List<string> ret = new List<string>();

            /*
             * critério:
             * - se já existe algum prepedido do mesmo cliente, criado com a mesma data e há menos de 10 minutos
             * - com a mesma loja e o mesmo orcamentista
             * - onde todos os produtos sejam iguais (fabricante, produto, qtde e preco_venda)
             * - a ordem dos produtos no pedido não precisa ser igual (no sistema em ASP exigia a mesma ordem para considerar igual)
             * - consequencia: se a forma de pagamento for diferente, o valor será diferente e o prepedido será considerado diferente
             * */
            /*

            '	VERIFICA SE ESTE ORÇAMENTO JÁ FOI GRAVADO!
                dim orcamento_a, vjg
                s = "SELECT t_ORCAMENTO.orcamento, fabricante, produto, qtde, preco_venda FROM t_ORCAMENTO INNER JOIN t_ORCAMENTO_ITEM ON (t_ORCAMENTO.orcamento=t_ORCAMENTO_ITEM.orcamento)" & _
                    " WHERE (id_cliente='" & cliente_selecionado & "') AND (data=" & bd_formata_data(Date) & ")" & _
                    " AND (loja='" & loja & "') AND (orcamentista='" & usuario & "')" & _
                    " AND (hora>='" & formata_hora_hhnnss(Now-converte_min_to_dec(10))& "')" & _
                    " ORDER BY t_ORCAMENTO_ITEM.orcamento, sequencia"
                    */

            var banco = contextoProvider.GetContextoLeitura();
            var hora = Util.HoraParaBanco(dataLimite);
            var prepedidosExistentes = await (from prepedidoBanco in banco.Torcamentos
                                              join item in banco.TorcamentoItems on prepedidoBanco.Orcamento equals item.Orcamento
                                              where prepedidoBanco.Id_Cliente == prePedido.DadosCliente.Id
                                                 && (prepedidoBanco.Data.HasValue && prepedidoBanco.Data.Value.Date == dataLimite.Date)
                                                 && hora.CompareTo(prepedidoBanco.Hora) <= 0
                                                 && prepedidoBanco.Loja == prePedido.DadosCliente.Loja
                                                 && prepedidoBanco.Orcamentista == prePedido.DadosCliente.Indicador_Orcamentista
                                              select new { prepedidoBanco.Orcamento, item.Fabricante, item.Produto, item.Qtde, item.Preco_Venda, item.Sequencia }).ToListAsync();

            //agora já está na memória, as operações são rápidas
            var orcamentosExistentes = (from prepedido in prepedidosExistentes select prepedido.Orcamento).Distinct();

            foreach (var orcamentoExistente in orcamentosExistentes)
            {
                var itensDestePrepedido = (from item in prepedidosExistentes
                                           where item.Orcamento == orcamentoExistente
                                           orderby item.Fabricante, item.Produto, item.Qtde
                                           select item).ToList();
                //todos estes itens devem ser iguais aos do prepedido sendo criado para que o prepedido já exista
                var itensParaCriar = (from item in prePedido.ListaProdutos
                                      orderby item.Fabricante, item.NormalizacaoCampos_Produto, item.Qtde
                                      select new { item.Fabricante, Produto = item.NormalizacaoCampos_Produto, item.Qtde, Preco_Venda = Math.Round(item.NormalizacaoCampos_Preco_Venda, 2) }).ToList();

                if (itensDestePrepedido.Count() == itensParaCriar.Count() && itensDestePrepedido.Count() > 0)
                {
                    bool algumDiferente = false;
                    for (int i = 0; i < itensDestePrepedido.Count(); i++)
                    {
                        var um = itensDestePrepedido[i];
                        var dois = itensParaCriar[i];
                        //nao comparamos a sequencia
                        if (!(um.Fabricante == dois.Fabricante && um.Produto == dois.Produto && um.Qtde == dois.Qtde && um.Preco_Venda == dois.Preco_Venda))
                        {
                            algumDiferente = true;
                            break;
                        }
                    }

                    if (!algumDiferente)
                        ret.Add(itensDestePrepedido[0].Orcamento);
                }

            }

            return ret;
        }

        public async Task<List<string>> PrepedidoPorIdCLiente(string idCliente, DateTime dataLimite)
        {
            List<string> ret = new List<string>();

            var banco = contextoProvider.GetContextoLeitura();
            var hora = Util.HoraParaBanco(dataLimite);
            var prepedidosExistentes = await (from prepedidoBanco in banco.Torcamentos
                                              where prepedidoBanco.Id_Cliente == idCliente
                                                 && (prepedidoBanco.Data.HasValue && prepedidoBanco.Data.Value.Date == dataLimite.Date)
                                                 && hora.CompareTo(prepedidoBanco.Hora) <= 0
                                              select new { prepedidoBanco.Orcamento }).ToListAsync();

            //agora já está na memória, as operações são rápidas
            var orcamentosExistentes = (from prepedido in prepedidosExistentes select prepedido.Orcamento).Distinct();
            return orcamentosExistentes.ToList();
        }

    }
}
