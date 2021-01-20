using InfraBanco.Modelos;
using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo50
{
    class Passo50
    {
        private readonly PedidoCriacaoDados Pedido;
        private readonly PedidoCriacaoRetornoDados Retorno;
        private readonly Pedido.Criacao.PedidoCriacao Criacao;
        public Passo50(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
        {
            this.Pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));
            this.Retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            this.Criacao = pedidoCriacao ?? throw new ArgumentNullException(nameof(pedidoCriacao));
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Executar()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            EntregaImediata();
            ValidarRA();
        }
        private void EntregaImediata()
        {
            //valida somente EntregaImediataData e EntregaImediata
            Criacao.ValidacoesPrepedidoBll.ValidarDetalhesPrepedido(Pedido.DetalhesPedido, Retorno.ListaErros);
        }
        private void ValidarRA()
        {
            if (string.IsNullOrEmpty(Pedido.Ambiente.Indicador) && Pedido.Valor.PedidoPossuiRa())
                Retorno.ListaErros.Add("Necessário indicador para usar RA");

            if (Pedido.Valor.PedidoPossuiRa() && !Pedido.Valor.PermiteRAStatus)
                Retorno.ListaErros.Add("Pedido está usando RA mas está inconsistente com PermiteRAStatus.");

            //o resto só validamos se tiver RA
            if (!Pedido.Valor.PedidoPossuiRa())
                return;


            //repetido, né? melhor...
            if (String.IsNullOrEmpty(Pedido.Ambiente.Indicador))
            {
                Retorno.ListaErros.Add("Necessário indicador para usar RA");
                return;
            }

            if (Criacao.Execucao.TabelasBanco.Indicador == null)
            {
                Retorno.ListaErros.Add($"Falha ao recuperar os dados do indicador! Indicador: {Pedido.Ambiente.Indicador}");
                return;
            }
            bool Indicador_Permite_RA_Status;
            Indicador_Permite_RA_Status = Criacao.Execucao.TabelasBanco.Indicador.Permite_RA_Status == 1;


            if (Pedido.Valor.PedidoPossuiRa() && !Indicador_Permite_RA_Status)
                Retorno.ListaErros.Add("Indicador não tem permissão para usar RA");
        }
    }
}
