using InfraBanco;
using Microsoft.EntityFrameworkCore;
using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Pedido.Dados.Criacao;

namespace Pedido
{
    public class PedidoRepetidoBll
    {
        private readonly ContextoBdProvider contextoProvider;

        public PedidoRepetidoBll(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task PedidoJaCadastrado(PedidoCriacaoDados pedido, List<string> listaErros)
        {
            DateTime dataLimite = DateTime.Now.AddSeconds(-1 * pedido.Configuracao.LimitePedidosExatamenteIguais_TempoSegundos);
            var repetidos = await PedidoJaCadastradoDesdeData(pedido, dataLimite);
            if (repetidos.Count < pedido.Configuracao.LimitePedidosExatamenteIguais_Numero)
                return;

            foreach (var pedidoJaCadastrado in repetidos)
                listaErros.Add($"Esta solicitação já foi gravada com o número {pedidoJaCadastrado}");
        }

        public async Task<List<string>> PedidoJaCadastradoDesdeData(PedidoCriacaoDados pedido, DateTime dataLimite)
        {
            List<string> ret = new List<string>();
            //"SELECT t_PEDIDO.pedido, fabricante, produto, qtde, preco_venda FROM t_PEDIDO INNER JOIN t_PEDIDO_ITEM ON (t_PEDIDO.pedido=t_PEDIDO_ITEM.pedido)" & _
            //" WHERE (id_cliente='" & cliente_selecionado & "') AND (data=" & bd_formata_data(Date) & ")" & _
            //" AND (loja='" & loja & "') AND (vendedor='" & usuario & "')" & _
            //" AND (data >= " & bd_monta_data(Date) & ")" & _
            //" AND (hora >= '" & formata_hora_hhnnss(Now - converte_min_to_dec(5)) & "')" & _
            //" AND (st_entrega<>'" & ST_ENTREGA_CANCELADO & "')" & _
            //" ORDER BY t_PEDIDO_ITEM.pedido, sequencia"

            var db = contextoProvider.GetContextoLeitura();
            var hora = UtilsGlobais.Util.HoraParaBanco(dataLimite);
            //com muitas combinações, o entitty não está conseguindo otimizar a 
            //passamos de 13 para 4 segundos na criação do pedido
            /*
            var pedidosExistentes = await (from pedidoBanco in db.Tpedidos
                                           join item in db.TpedidoItems on pedidoBanco.Pedido equals item.Pedido
                                           where pedidoBanco.Id_Cliente == pedido.Cliente.Id_cliente &&
                                                (pedidoBanco.Data.HasValue && pedidoBanco.Data.Value.Date == dataLimite.Date) &&
                                                pedidoBanco.Loja == pedido.Ambiente.Loja &&
                                                pedidoBanco.Usuario_Cadastro == pedido.Ambiente.Usuario &&
                                                dataLimite.CompareTo(pedidoBanco.Data) <= 0 &&
                                                hora.CompareTo(pedidoBanco.Hora) <= 0 &&
                                                pedidoBanco.St_Entrega != InfraBanco.Constantes.Constantes.ST_ENTREGA_CANCELADO
                                           orderby item.Pedido, item.Sequencia
                                           select new { pedidoBanco.Pedido, item.Fabricante, item.Produto, item.Qtde, item.Preco_Venda }).ToListAsync();
                                           */
            var pedidosFiltradosSemData = await (from pedidoBanco in db.Tpedidos
                                                 where pedidoBanco.Id_Cliente == pedido.Cliente.Id_cliente &&
                                                      pedidoBanco.Loja == pedido.Ambiente.Loja &&
                                                      pedidoBanco.Usuario_Cadastro == pedido.Ambiente.Usuario &&
                                                      pedidoBanco.St_Entrega != InfraBanco.Constantes.Constantes.ST_ENTREGA_CANCELADO
                                                 select new { pedidoBanco.Pedido, pedidoBanco.Data, pedidoBanco.Hora }).ToListAsync();
            var pedidosFiltrados = (from pedidoBanco in pedidosFiltradosSemData
                                    where
                                        (pedidoBanco.Data.HasValue && pedidoBanco.Data.Value.Date == dataLimite.Date) &&
                                        dataLimite.CompareTo(pedidoBanco.Data) <= 0 &&
                                        hora.CompareTo(pedidoBanco.Hora) <= 0
                                    select pedidoBanco).ToList();
            var pedidosFiltradsPedidos = (from p in pedidosFiltrados select p.Pedido).Distinct();
            var pedidosExistentes = (from item in db.TpedidoItems
                                     where pedidosFiltradsPedidos.Contains(item.Pedido)  //precisa ds=esta linha para aplicar o where
                                     orderby item.Pedido, item.Sequencia
                                     select new { item.Pedido, item.Fabricante, item.Produto, item.Qtde, item.Preco_Venda }).ToList();

            var pedExistentes = (from p in pedidosExistentes select p.Pedido).Distinct();

            foreach (var existente in pedExistentes)
            {
                //precisa ordenar pelo mesmo critério nos dois casos
                var itensDestePedido = (from item in pedidosExistentes
                                        where item.Pedido == existente
                                        orderby item.Fabricante, item.Produto, item.Qtde
                                        select item).ToList();

                var itensParaCriar = (from item in pedido.ListaProdutos
                                      orderby item.Fabricante, item.Produto, item.Qtde
                                      select new
                                      {
                                          item.Fabricante,
                                          item.Produto,
                                          item.Qtde,
                                          Preco_Venda = Math.Round(item.Preco_Venda, 2)
                                      }).ToList();

                if (itensDestePedido.Count() == itensParaCriar.Count())
                {
                    bool algumDiferente = false;
                    for (int i = 0; i < itensDestePedido.Count(); i++)
                    {
                        var um = itensDestePedido[i];
                        var dois = itensParaCriar[i];
                        //nao comparamos a sequencia
                        if (!(um.Fabricante == dois.Fabricante && um.Produto == dois.Produto && um.Qtde == dois.Qtde && um.Preco_Venda == dois.Preco_Venda))
                        {
                            algumDiferente = true;
                            break;
                        }
                    }

                    if (!algumDiferente)
                        ret.Add(existente);
                }
            }
            return ret;
        }

        public async Task<List<string>> PedidoPorIdCLiente(string idCliente, DateTime dataLimite)
        {
            List<string> ret = new List<string>();

            var banco = contextoProvider.GetContextoLeitura();
            var hora = UtilsGlobais.Util.HoraParaBanco(dataLimite);
            var pedidosExistentes = await (from pedidoBanco in banco.Tpedidos
                                           where pedidoBanco.Id_Cliente == idCliente
                                              && (pedidoBanco.Data.HasValue && pedidoBanco.Data.Value.Date == dataLimite.Date)
                                              && hora.CompareTo(pedidoBanco.Hora) <= 0
                                           select new { pedidoBanco.Pedido }).ToListAsync();

            //agora já está na memória, as operações são rápidas
            var pedExistentes = (from pedido in pedidosExistentes select pedido.Pedido).Distinct();
            return pedExistentes.ToList();
        }
    }
}
