using Pedido;
using PrepedidoUnisBusiness.UnisDto.PedidoUnisDto;
using System.Linq;
using System.Threading.Tasks;

namespace PrepedidoUnisBusiness.UnisBll.PedidoUnisBll
{
    public class PedidoUnisBll
    {
        private readonly PedidoBll pedidoBll;
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public PedidoUnisBll(PedidoBll pedidoBll, InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.pedidoBll = pedidoBll;
            this.contextoProvider = contextoProvider;
        }

        public async Task<PedidoUnisDto> BuscarPedido(string pedido)
        {
            //PedidoUnisDto pedidoUnis = new PedidoUnisDto();

            //vamos buscar o orcamentista do pedido informado para passar para a busca de montagem do pedido no global
            var db = contextoProvider.GetContextoLeitura();

            string orcamentista = (from c in db.Tpedidos
                                   where c.Pedido == pedido
                                   select c.Orcamentista).FirstOrDefault();

            Pedido.Dados.DetalhesPedido.PedidoDados pedidoDados = await pedidoBll.BuscarPedido(orcamentista?.Trim(), pedido);

            PrepedidoBusiness.Dto.ClienteCadastro.DadosClienteCadastroDto dadosCliente =
                PrepedidoBusiness.Dto.ClienteCadastro.DadosClienteCadastroDto
                .DadosClienteCadastroDto_De_DadosClienteCadastroDados(pedidoDados.DadosCliente);

            PrepedidoBusiness.Dto.ClienteCadastro.EnderecoEntregaDtoClienteCadastro enderecoEntrega =
                PrepedidoBusiness.Dto.ClienteCadastro.EnderecoEntregaDtoClienteCadastro
                .EnderecoEntregaDtoClienteCadastro_De_EnderecoEntregaClienteCadastroDados(pedidoDados.EnderecoEntrega);

            PedidoUnisDto pedidoUnis = PedidoUnisDto.PedidoUnisDto_De_PedidoDados(pedidoDados, dadosCliente, enderecoEntrega);
            
            return await Task.FromResult(pedidoUnis);
        }
    }
}
