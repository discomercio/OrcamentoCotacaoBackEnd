using PrepedidoUnisBusiness.UnisDto.PedidoUnisDto;
using System.Linq;
using System.Threading.Tasks;

namespace PrepedidoUnisBusiness.UnisBll.PedidoUnisBll
{
    public class PedidoUnisBll
    {
        private readonly Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll pedidoVisualizacaoBll;
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public PedidoUnisBll(Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll  pedidoVisualizacaoBll, InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.pedidoVisualizacaoBll = pedidoVisualizacaoBll;
            this.contextoProvider = contextoProvider;
        }

        public async Task<PedidoUnisDto> BuscarPedido(string pedido)
        {
            PedidoUnisDto pedidoUnis = new PedidoUnisDto();

            //vamos buscar o orcamentista do pedido informado para passar para a busca de montagem do pedido no global
            var db = contextoProvider.GetContextoLeitura();

            if (!string.IsNullOrEmpty(pedido))
            {
                string indicador = (from c in db.Tpedido
                                       where c.Pedido == pedido
                                       select c.Indicador).FirstOrDefault();


                Prepedido.PedidoVisualizacao.Dados.DetalhesPedido.PedidoDados pedidoDados = await pedidoVisualizacaoBll.BuscarPedido(indicador?.Trim(), pedido);

                if (pedidoDados != null)
                {
                    PrepedidoBusiness.Dto.ClienteCadastro.DadosClienteCadastroDto dadosCliente =
                    PrepedidoBusiness.Dto.ClienteCadastro.DadosClienteCadastroDto
                    .DadosClienteCadastroDto_De_DadosClienteCadastroDados(pedidoDados.DadosCliente);

                    PrepedidoBusiness.Dto.ClienteCadastro.EnderecoEntregaDtoClienteCadastro enderecoEntrega =
                        PrepedidoBusiness.Dto.ClienteCadastro.EnderecoEntregaDtoClienteCadastro
                        .EnderecoEntregaDtoClienteCadastro_De_EnderecoEntregaClienteCadastroDados(pedidoDados.EnderecoEntrega);

                    pedidoUnis = PedidoUnisDto.PedidoUnisDto_De_PedidoDados(pedidoDados, dadosCliente, enderecoEntrega);
                }
            }
            
            return await Task.FromResult(pedidoUnis);
        }
    }
}
