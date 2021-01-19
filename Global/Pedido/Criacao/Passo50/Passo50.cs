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
        private readonly PedidoCriacaoDados pedido;
        private readonly PedidoCriacaoRetornoDados retorno;
        private readonly Pedido.Criacao.PedidoCriacao pedidoCriacao;
        public Passo50(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
        {
            this.pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));
            this.retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            this.pedidoCriacao = pedidoCriacao ?? throw new ArgumentNullException(nameof(pedidoCriacao));
        }

        public async Task Executar()
        {
            EntregaImediata();
            await ValidarRA();
        }
        private void EntregaImediata()
        {
            //valida somente EntregaImediataData e EntregaImediata
            pedidoCriacao.validacoesPrepedidoBll.ValidarDetalhesPrepedido(pedido.DetalhesPedido, retorno.ListaErros);
        }
        private async Task ValidarRA()
        {
            if (string.IsNullOrEmpty(pedido.Ambiente.Indicador) && pedido.Valor.PedidoPossuiRa())
                retorno.ListaErros.Add("Necessário indicador para usar RA");

            if (pedido.Valor.PedidoPossuiRa() && !pedido.Valor.PermiteRAStatus)
                retorno.ListaErros.Add("Pedido está usando RA mas está inconsistente com PermiteRAStatus.");

            //o resto só validamos se tiver RA
            if (!pedido.Valor.PedidoPossuiRa())
                return;


            //repetido, né? melhor...
            if (String.IsNullOrEmpty(pedido.Ambiente.Indicador))
            {
                retorno.ListaErros.Add("Necessário indicador para usar RA");
                return;
            }

            if (pedidoCriacao.Indicador == null)
            {
                retorno.ListaErros.Add($"Falha ao recuperar os dados do indicador! Indicador: {pedido.Ambiente.Indicador}");
                return;
            }
            bool Indicador_Permite_RA_Status;
            Indicador_Permite_RA_Status = pedidoCriacao.Indicador.Permite_RA_Status == 1;


            if (pedido.Valor.PedidoPossuiRa() && !Indicador_Permite_RA_Status)
                retorno.ListaErros.Add("Indicador não tem permissão para usar RA");
        }
    }
}
