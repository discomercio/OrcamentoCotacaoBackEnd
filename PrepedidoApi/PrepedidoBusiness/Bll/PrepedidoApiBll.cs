using Prepedido.Dados;
using Prepedido.Dados.DetalhesPrepedido;
using PrepedidoBusiness.Dto.Prepedido;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoBusiness.Bll
{
    public class PrepedidoApiBll
    {
        private readonly Prepedido.PrepedidoBll prepedidoBll;
        

        public PrepedidoApiBll(Prepedido.PrepedidoBll prepedidoBll)
        {
            this.prepedidoBll = prepedidoBll;
        }

        public async Task DeletarOrcamentoExisteComTransacao(PrePedidoDto prePedido, string apelido)
        {
            PrePedidoDados prePedidoDados = PrePedidoDto.PrePedidoDados_De_PrePedidoDto(prePedido);
            await prepedidoBll.DeletarOrcamentoExisteComTransacao(prePedidoDados, apelido.Trim());
        }

        public async Task<IEnumerable<string>> CadastrarPrepedido(PrePedidoDto prePedido, string apelido, 
            decimal limiteArredondamento, bool verificarPrepedidoRepetido, 
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavelCadastro,
            int limite_de_itens)
        {
            PrePedidoDados prePedidoDados = PrePedidoDto.PrePedidoDados_De_PrePedidoDto(prePedido);
            var ret = await prepedidoBll.CadastrarPrepedido(prePedidoDados, apelido, 
                limiteArredondamento, verificarPrepedidoRepetido, sistemaResponsavelCadastro, limite_de_itens);
            return ret;
        }

        public async Task<PrePedidoDto> BuscarPrePedido(string apelido, string numPrePedido)
        {
            var ret = await prepedidoBll.BuscarPrePedido(apelido, numPrePedido);
            return PrePedidoDto.PrePedidoDto_De_PrePedidoDados(ret);
        }

        public async Task<IEnumerable<PrepedidosCadastradosDtoPrepedido>> ListarPrePedidos(string apelido,
        Prepedido.PrepedidoBll.TipoBuscaPrepedido tipoBusca,
        string clienteBusca, string numeroPrePedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            IEnumerable<PrepedidosCadastradosPrepedidoDados> lista = await prepedidoBll.ListarPrePedidos(apelido,
                tipoBusca, clienteBusca,
                numeroPrePedido, dataInicial, dataFinal);

            return PrepedidosCadastradosDtoPrepedido.ListaPrepedidosCadastradosDtoPrepedido_De_PrepedidosCadastradosPrepedidoDados(lista);
        }
    }
}
