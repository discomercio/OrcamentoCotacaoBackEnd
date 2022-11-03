using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto.DetalhesPedido
{
    public class DetalhesNFPedidoDtoPedido
    {
        public string Observacoes { get; set; }
        public string ConstaNaNF { get; set; }
        public string XPed { get; set; }
        public string NumeroNF { get; set; }
        public string NFSimples { get; set; }//acho que esse campo não é usado
        public string EntregaImediata { get; set; }
        public short StBemUsoConsumo { get; set; }
        public short InstaladorInstala { get; set; }
        public string GarantiaIndicadorStatus { get; set; }
        //Campos para incluir
        //NumeroMagento = nº do pedido Magento
        //NumeroPedidoMarketplace
        //OrigemPedido = de qual loja esta vindo

        //todo: incluir na interface de usuário
        public DateTime? EntregaImediataData { get; set; }
        public string FormaDePagamento { get; set; }
        public string DescricaoFormaPagamento { get; set; }
        public string PrevisaoEntrega { get; set; }


        public static Prepedido.Dados.DetalhesPrepedido.DetalhesPrepedidoDados DetalhesPrepedidoDados_De_DetalhesNFPedidoDtoPedido(DetalhesNFPedidoDtoPedido origem)
        {
            if (origem == null) return null;
            return new Prepedido.Dados.DetalhesPrepedido.DetalhesPrepedidoDados()
            {
                Observacoes = origem.Observacoes,
                NumeroNF = origem.NumeroNF,
                EntregaImediata = origem.EntregaImediata,
                EntregaImediataData = origem.EntregaImediataData,
                BemDeUso_Consumo = origem.StBemUsoConsumo,
                InstaladorInstala = origem.InstaladorInstala,
                GarantiaIndicador = Byte.Parse(origem.GarantiaIndicadorStatus),
                FormaDePagamento = origem.FormaDePagamento,
                DescricaoFormaPagamento = origem.DescricaoFormaPagamento,
                PrevisaoEntrega = DateTime.Parse(origem.PrevisaoEntrega)
            };
        }
        public static DetalhesNFPedidoDtoPedido DetalhesNFPedidoDtoPedido_De_DetalhesNFPedidoPedidoDados(DetalhesNFPedidoPedidoDados origem)
        {
            if (origem == null) return null;
            return new DetalhesNFPedidoDtoPedido()
            {
                Observacoes = origem.Observacoes,
                ConstaNaNF = origem.ConstaNaNF,
                XPed = origem.XPed,
                NumeroNF = origem.NumeroNF,
                NFSimples = origem.NFSimples,
                EntregaImediata = origem.EntregaImediata,
                StBemUsoConsumo = origem.StBemUsoConsumo,
                InstaladorInstala = origem.InstaladorInstala,
                GarantiaIndicadorStatus = origem.GarantiaIndicadorStatus,
                PrevisaoEntrega = origem.PrevisaoEntrega
            };
        }
    }
}
