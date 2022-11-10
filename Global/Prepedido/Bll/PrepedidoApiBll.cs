using InfraBanco.Constantes;
using Prepedido.Dados;
using Prepedido.Dados.DetalhesPrepedido;
using Prepedido.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prepedido.Bll
{
    public class PrepedidoApiBll
    {
        private readonly Prepedido.Bll.PrepedidoBll prepedidoBll;
        private readonly InfraBanco.ContextoBdProvider _contextoBdProvider;


        public PrepedidoApiBll(Prepedido.Bll.PrepedidoBll prepedidoBll, InfraBanco.ContextoBdProvider _contextoBdProvider)
        {
            this.prepedidoBll = prepedidoBll;
            this._contextoBdProvider = _contextoBdProvider;
        }

        public async Task DeletarOrcamentoExisteComTransacao(PrePedidoDto prePedido, string apelido)
        {
            PrePedidoDados prePedidoDados = PrePedidoDto.PrePedidoDados_De_PrePedidoDto(prePedido);
            await prepedidoBll.DeletarOrcamentoExisteComTransacao(prePedidoDados, apelido.Trim());
        }

        public async Task<IEnumerable<string>> CadastrarPrepedido(
            PrePedidoDto prePedido, 
            string apelido,
            decimal limiteArredondamento, 
            bool verificarPrepedidoRepetido,
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavelCadastro,
            int limite_de_itens)
        {
            
            if(!string.IsNullOrEmpty(prePedido.DetalhesPrepedido.EntregaImediata) && 
                short.Parse(prePedido.DetalhesPrepedido.EntregaImediata) == (short)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO)
            {
                prePedido.DetalhesPrepedido.PrevisaoEntregaData = prePedido.DetalhesPrepedido.EntregaImediataData;

                if (prePedido.DetalhesPrepedido.EntregaImediataData.HasValue)
                {
                    prePedido.DetalhesPrepedido.PrevisaoEntregaData = prePedido.DetalhesPrepedido.EntregaImediataData.Value.Date;
                }
                
                prePedido.DetalhesPrepedido.EntregaImediataData = DateTime.Now;
            }

            PrePedidoDados prePedidoDados = PrePedidoDto.PrePedidoDados_De_PrePedidoDto(prePedido);

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.XLOCK_SYNC_ORCAMENTO))
            {
                var ret = (await prepedidoBll.CadastrarPrepedido(prePedidoDados, apelido,
                limiteArredondamento, verificarPrepedidoRepetido, sistemaResponsavelCadastro, limite_de_itens, dbGravacao)).ToList();

                if (ret.Count == 1)
                {
                    if (ret[0].Contains(Constantes.SUFIXO_ID_ORCAMENTO))
                    {
                        dbGravacao.transacao.Commit();
                    }
                        
                }
                return ret;
            }

        }

        public async Task<PrePedidoDto> BuscarPrePedido(string apelido, string numPrePedido)
        {
            var ret = await prepedidoBll.BuscarPrePedido(apelido, numPrePedido);
            return PrePedidoDto.PrePedidoDto_De_PrePedidoDados(ret);
        }

        public async Task<IEnumerable<PrepedidosCadastradosDtoPrepedido>> ListarPrePedidos(string apelido,
        Prepedido.Bll.PrepedidoBll.TipoBuscaPrepedido tipoBusca,
        string clienteBusca, string numeroPrePedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            IEnumerable<PrepedidosCadastradosPrepedidoDados> lista = await prepedidoBll.ListarPrePedidos(apelido,
                tipoBusca, clienteBusca,
                numeroPrePedido, dataInicial, dataFinal);

            return PrepedidosCadastradosDtoPrepedido.ListaPrepedidosCadastradosDtoPrepedido_De_PrepedidosCadastradosPrepedidoDados(lista);
        }
    }
}
