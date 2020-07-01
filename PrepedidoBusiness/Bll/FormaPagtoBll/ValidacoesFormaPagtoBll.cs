using InfraBanco.Constantes;
using PrepedidoBusiness.Bll.PrepedidoBll;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Bll.FormaPagtoBll
{
    public class ValidacoesFormaPagtoBll
    {
        private readonly ValidacoesPrepedidoBll validacoesPrepedidoBll;

        public ValidacoesFormaPagtoBll(ValidacoesPrepedidoBll validacoesPrepedidoBll)
        {
            this.validacoesPrepedidoBll = validacoesPrepedidoBll;
        }

        public bool ValidarFormaPagto(PrePedidoDto prepedido, List<string> lstErros, decimal limiteArredondamento)
        {
            bool retorno = false;

            if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
            {
                ValidarFormaPagtoAVista(prepedido, lstErros, limiteArredondamento);
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
            {
                ValidarFormaPagtoParcelaUnica(prepedido, lstErros);
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
            {
                ValidarFormaPagtoParceladoCartao(prepedido, lstErros);
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
            {
                ValidarFormaPagtoParceladoCartaoMaquineta(prepedido, lstErros);
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
            {
                ValidarFormaPagtoComEntreda(prepedido, lstErros);
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
            {
                ValidarFormaPagtoSemEntrada(prepedido, lstErros);
            }
            else
            {
                lstErros.Add("É obrigatório especificar a forma de pagamento");
            }

            if (lstErros.Count == 0)
                retorno = true;

            return retorno;
        }

        private void ValidarFormaPagtoAVista(PrePedidoDto prepedido, List<string> lstErros, decimal limiteArredondamento)
        {
            decimal vlTotalFormaPagto = 0M;

            if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.Op_av_forma_pagto))
                lstErros.Add("Indique a forma de pagamento (à vista).");
            if (!validacoesPrepedidoBll.CalculaItens(prepedido, out vlTotalFormaPagto, limiteArredondamento))
                lstErros.Add("Há divergência entre o valor total do pré-pedido (" + Constantes.SIMBOLO_MONETARIO + " " +
                    prepedido.VlTotalDestePedido + ") e o valor total descrito através da forma de pagamento (" + Constantes.SIMBOLO_MONETARIO + " " +
                    Math.Abs((decimal)prepedido.VlTotalDestePedido - vlTotalFormaPagto) + ")!");
        }

        private void ValidarFormaPagtoParcelaUnica(PrePedidoDto prepedido, List<string> lstErros)
        {
            if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.Op_pu_forma_pagto))
                lstErros.Add("Indique a forma de pagamento da parcela única.");
            else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pu_valor.ToString()))
                lstErros.Add("Indique o valor da parcela única.");
            else if (prepedido.FormaPagtoCriacao.C_pu_valor <= 0)
                lstErros.Add("Valor da parcela única é inválido.");
            else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pu_vencto_apos.ToString()))
                lstErros.Add("Indique o intervalo de vencimento da parcela única.");
            else if (prepedido.FormaPagtoCriacao.C_pu_vencto_apos <= 0)
                lstErros.Add("Intervalo de vencimento da parcela única é inválido.");
        }

        private void ValidarFormaPagtoParceladoCartao(PrePedidoDto prepedido, List<string> lstErros)
        {
            if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pc_qtde.ToString()))
                lstErros.Add("Indique a quantidade de parcelas (parcelado no cartão [internet]).");
            else if (prepedido.FormaPagtoCriacao.C_pc_qtde < 1)
                lstErros.Add("Quantidade de parcelas inválida (parcelado no cartão [internet]).");
            else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pc_valor.ToString()))
                lstErros.Add("Indique o valor da parcela (parcelado no cartão [internet]).");
            else if (prepedido.FormaPagtoCriacao.C_pc_valor <= 0)
                lstErros.Add("Valor de parcela inválido (parcelado no cartão [internet]).");
        }

        private void ValidarFormaPagtoParceladoCartaoMaquineta(PrePedidoDto prepedido, List<string> lstErros)
        {
            if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pc_maquineta_qtde.ToString()))
                lstErros.Add("Indique a quantidade de parcelas (parcelado no cartão [maquineta]).");
            else if (prepedido.FormaPagtoCriacao.C_pc_maquineta_qtde < 1)
                lstErros.Add("Quantidade de parcelas inválida (parcelado no cartão [maquineta]).");
            else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pc_maquineta_valor.ToString()))
                lstErros.Add("Indique o valor da parcela (parcelado no cartão [maquineta]).");
            else if (prepedido.FormaPagtoCriacao.C_pc_maquineta_valor <= 0)
                lstErros.Add("Valor de parcela inválido (parcelado no cartão [maquineta]).");
        }

        private void ValidarFormaPagtoComEntreda(PrePedidoDto prepedido, List<string> lstErros)
        {
            if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.Op_pce_entrada_forma_pagto.ToString()))
                lstErros.Add("Indique a forma de pagamento da entrada (parcelado com entrada).");
            else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pce_entrada_valor.ToString()))
                lstErros.Add("Indique o valor da entrada (parcelado com entrada).");
            else if (prepedido.FormaPagtoCriacao.C_pce_entrada_valor <= 0)
                lstErros.Add("Valor da entrada inválido (parcelado com entrada).");
            else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto))
                lstErros.Add("Indique a forma de pagamento das prestações (parcelado com entrada).");
            else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pce_prestacao_qtde.ToString()))
                lstErros.Add("Indique a quantidade de prestações (parcelado com entrada).");
            else if (prepedido.FormaPagtoCriacao.C_pce_prestacao_qtde <= 0)
                lstErros.Add("Quantidade de prestações inválida (parcelado com entrada).");
            else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pce_prestacao_valor.ToString()))
                lstErros.Add("Indique o valor da prestação (parcelado com entrada).");
            else if (prepedido.FormaPagtoCriacao.C_pce_prestacao_valor <= 0)
                lstErros.Add("Valor de prestação inválido (parcelado com entrada).");
            else if (prepedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto != "7" &&
                prepedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto != "5")
            {
                if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pce_prestacao_periodo.ToString()))
                    lstErros.Add("Indique o intervalo de vencimento entre as parcelas (parcelado com entrada).");
            }
            else if (prepedido.FormaPagtoCriacao.C_pce_prestacao_periodo <= 0)
                lstErros.Add("Intervalo de vencimento inválido (parcelado com entrada).");
        }

        private void ValidarFormaPagtoSemEntrada(PrePedidoDto prepedido, List<string> lstErros)
        {
            if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.Op_pse_prim_prest_forma_pagto))
                lstErros.Add("Indique a forma de pagamento da 1ª prestação (parcelado sem entrada).");
            else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pse_prim_prest_valor.ToString()))
                lstErros.Add("Indique o valor da 1ª prestação (parcelado sem entrada).");
            else if (prepedido.FormaPagtoCriacao.C_pse_prim_prest_valor <= 0)
                lstErros.Add("Valor da 1ª prestação inválido (parcelado sem entrada).");
            else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pse_prim_prest_apos.ToString()))
                lstErros.Add("Indique o intervalo de vencimento da 1ª parcela (parcelado sem entrada).");
            else if (prepedido.FormaPagtoCriacao.C_pse_prim_prest_apos <= 0)
                lstErros.Add("Intervalo de vencimento da 1ª parcela é inválido (parcelado sem entrada).");
            else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.Op_pse_demais_prest_forma_pagto))
                lstErros.Add("Indique a forma de pagamento das demais prestações (parcelado sem entrada).");
            else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pse_demais_prest_qtde.ToString()))
                lstErros.Add("Indique a quantidade das demais prestações (parcelado sem entrada).");
            else if (prepedido.FormaPagtoCriacao.C_pse_demais_prest_qtde <= 0)
                lstErros.Add("Quantidade de prestações inválida (parcelado sem entrada).");
            else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pse_demais_prest_valor.ToString()))
                lstErros.Add("Indique o valor das demais prestações (parcelado sem entrada).");
            else if (prepedido.FormaPagtoCriacao.C_pse_demais_prest_valor <= 0)
                lstErros.Add("Valor de prestação inválido (parcelado sem entrada).");
            else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pse_demais_prest_periodo.ToString()))
                lstErros.Add("Indique o intervalo de vencimento entre as parcelas (parcelado sem entrada).");
            else if (prepedido.FormaPagtoCriacao.C_pse_demais_prest_periodo < 0)
                lstErros.Add("Intervalo de vencimento inválido (parcelado sem entrada).");
        }
    }
}
