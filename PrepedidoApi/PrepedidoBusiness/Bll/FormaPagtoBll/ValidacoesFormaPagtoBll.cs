using InfraBanco.Constantes;
using PrepedidoBusiness.Bll.PrepedidoBll;
using PrepedidoBusiness.Dto.FormaPagto;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoBusiness.Bll.FormaPagtoBll
{
    public class ValidacoesFormaPagtoBll
    {
        

        public ValidacoesFormaPagtoBll()
        {
            
        }

        private void ValidarSiglaFormaPagto(PrePedidoDto prepedido, string c_custoFinancFornecTipoParcelamento,
            List<string> lstErros)
        {
            if (!string.IsNullOrEmpty(c_custoFinancFornecTipoParcelamento))
            {
                if (c_custoFinancFornecTipoParcelamento != prepedido.FormaPagtoCriacao.CustoFinancFornecTipoParcelamento)
                    lstErros.Add($"Tipo do parcelamento (CustoFinancFornecTipoParcelamento '" +
                        $"{prepedido.FormaPagtoCriacao.CustoFinancFornecTipoParcelamento}') está incorreto!");
            }
            else
            {
                lstErros.Add("Tipo do parcelamento inválido");
            }
        }

        public bool ValidarFormaPagto(PrePedidoDto prepedido, List<string> lstErros,
            decimal limiteArredondamento, decimal maxErroArredondamento, string c_custoFinancFornecTipoParcelamento, 
            FormaPagtoDto formaPagtoDto)
        {
            ////vamos verificar a forma de pagamento com base no orçamentista que esta sendo enviado
            //FormaPagtoDto formasPagto = await formaPagtoBll.ObterFormaPagto(orcamentista, prepedido.DadosCliente.Tipo);

            bool retorno = false;

            if (!string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.CustoFinancFornecTipoParcelamento))
            {
                ValidarSiglaFormaPagto(prepedido, c_custoFinancFornecTipoParcelamento, lstErros);
            }

            string msgNegarFormaPagto = "Forma de pagamento não aceita para esse indicador.";

            if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
            {
                if (formaPagtoDto.ListaAvista?.Count > 0)
                    ValidarFormaPagtoAVista(prepedido, lstErros, limiteArredondamento);
                else lstErros.Add(msgNegarFormaPagto);
            }

            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
            {
                if (formaPagtoDto.ListaParcUnica?.Count > 0)
                    ValidarFormaPagtoParcelaUnica(prepedido, lstErros, maxErroArredondamento);
                else lstErros.Add(msgNegarFormaPagto);
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
            {
                if (formaPagtoDto.ParcCartaoInternet)
                    ValidarFormaPagtoParceladoCartao(prepedido, lstErros, maxErroArredondamento);
                else lstErros.Add(msgNegarFormaPagto);
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
            {
                if (formaPagtoDto.ParcCartaoMaquineta)
                    ValidarFormaPagtoParceladoCartaoMaquineta(prepedido, lstErros, maxErroArredondamento);
                else lstErros.Add(msgNegarFormaPagto);
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
            {
                if (formaPagtoDto.ListaParcComEntrada?.Count > 0 && formaPagtoDto.ListaParcComEntPrestacao?.Count > 0)
                    ValidarFormaPagtoComEntreda(prepedido, lstErros, maxErroArredondamento);
                else lstErros.Add(msgNegarFormaPagto);
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
            {
                if (formaPagtoDto.ListaParcSemEntPrestacao?.Count > 0 && formaPagtoDto.ListaParcSemEntPrimPrest?.Count > 0)
                    ValidarFormaPagtoSemEntrada(prepedido, lstErros, maxErroArredondamento);
                else lstErros.Add(msgNegarFormaPagto);
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
            if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.Op_av_forma_pagto))
                lstErros.Add("Indique a forma de pagamento (à vista).");

            if (prepedido.FormaPagtoCriacao.Qtde_Parcelas != 1)
            {
                lstErros.Add("Quantidade da parcela esta divergente!");
            }
        }

        private void ValidarFormaPagtoParcelaUnica(PrePedidoDto prepedido, List<string> lstErros, decimal maxErroArredondamento)
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

            if (lstErros.Count == 0)
            {
                if (prepedido.FormaPagtoCriacao.Qtde_Parcelas != 1)
                    lstErros.Add("Quantidade da parcela esta divergente!");

                if (prepedido.PermiteRAStatus == 1)
                {
                    if (Math.Abs((decimal)(prepedido.ValorTotalDestePedidoComRA - prepedido.FormaPagtoCriacao.C_pu_valor)) > maxErroArredondamento)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
                else
                {
                    if (Math.Abs((decimal)(prepedido.VlTotalDestePedido - prepedido.FormaPagtoCriacao.C_pu_valor)) > maxErroArredondamento)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
            }
        }

        private void ValidarFormaPagtoParceladoCartao(PrePedidoDto prepedido, List<string> lstErros, decimal maxErroArredondamento)
        {
            if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pc_qtde.ToString()))
                lstErros.Add("Indique a quantidade de parcelas (parcelado no cartão [internet]).");
            else if (prepedido.FormaPagtoCriacao.C_pc_qtde < 1)
                lstErros.Add("Quantidade de parcelas inválida (parcelado no cartão [internet]).");
            else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pc_valor.ToString()))
                lstErros.Add("Indique o valor da parcela (parcelado no cartão [internet]).");
            else if (prepedido.FormaPagtoCriacao.C_pc_valor <= 0)
                lstErros.Add("Valor de parcela inválido (parcelado no cartão [internet]).");

            if (lstErros.Count == 0)
            {
                if (prepedido.FormaPagtoCriacao.C_pc_qtde != prepedido.FormaPagtoCriacao.Qtde_Parcelas)
                {
                    lstErros.Add("Quantidade de parcelas esta divergente!");
                }

                decimal vlTotal = (decimal)(prepedido.FormaPagtoCriacao.C_pc_valor * prepedido.FormaPagtoCriacao.C_pc_qtde);
                if (prepedido.PermiteRAStatus == 1)
                {
                    if (Math.Abs((decimal)(prepedido.ValorTotalDestePedidoComRA - vlTotal)) > maxErroArredondamento * prepedido.FormaPagtoCriacao.C_pc_qtde)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
                else
                {
                    if (Math.Abs((decimal)(prepedido.VlTotalDestePedido - vlTotal)) > maxErroArredondamento * prepedido.FormaPagtoCriacao.C_pc_qtde)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
            }
        }

        private void ValidarFormaPagtoParceladoCartaoMaquineta(PrePedidoDto prepedido, List<string> lstErros, decimal maxErroArredondamento)
        {
            if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pc_maquineta_qtde.ToString()))
                lstErros.Add("Indique a quantidade de parcelas (parcelado no cartão [maquineta]).");
            else if (prepedido.FormaPagtoCriacao.C_pc_maquineta_qtde < 1)
                lstErros.Add("Quantidade de parcelas inválida (parcelado no cartão [maquineta]).");
            else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pc_maquineta_valor.ToString()))
                lstErros.Add("Indique o valor da parcela (parcelado no cartão [maquineta]).");
            else if (prepedido.FormaPagtoCriacao.C_pc_maquineta_valor <= 0)
                lstErros.Add("Valor de parcela inválido (parcelado no cartão [maquineta]).");

            //afazer: validar o valor da forma de pagto com o valor total do pedido
            if (lstErros.Count == 0)
            {
                if (prepedido.FormaPagtoCriacao.C_pc_maquineta_qtde != prepedido.FormaPagtoCriacao.Qtde_Parcelas)
                {
                    lstErros.Add("Quantidade de parcelas esta divergente!");
                }

                decimal vlTotal = (decimal)(prepedido.FormaPagtoCriacao.C_pc_maquineta_valor *
                prepedido.FormaPagtoCriacao.C_pc_maquineta_qtde);
                if (prepedido.PermiteRAStatus == 1)
                {
                    if (Math.Abs((decimal)(prepedido.ValorTotalDestePedidoComRA - vlTotal)) > maxErroArredondamento * prepedido.FormaPagtoCriacao.C_pc_maquineta_qtde)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
                else
                {
                    if (Math.Abs((decimal)(prepedido.VlTotalDestePedido - vlTotal)) > maxErroArredondamento * prepedido.FormaPagtoCriacao.C_pc_maquineta_qtde)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
            }
        }

        private void ValidarFormaPagtoComEntreda(PrePedidoDto prepedido, List<string> lstErros, decimal maxErroArredondamento)
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

            //afazer: validar o valor da forma de pagto com o valor total do pedido
            if (lstErros.Count == 0)
            {
                if ((prepedido.FormaPagtoCriacao.C_pce_prestacao_qtde + 1) != prepedido.FormaPagtoCriacao.Qtde_Parcelas)
                {
                    lstErros.Add("Quantidade de parcelas esta divergente!");
                }

                decimal vlTotal = (decimal)(prepedido.FormaPagtoCriacao.C_pce_entrada_valor +
                    (prepedido.FormaPagtoCriacao.C_pce_prestacao_valor *
                    prepedido.FormaPagtoCriacao.C_pce_prestacao_qtde));

                if (prepedido.PermiteRAStatus == 1)
                {
                    if (Math.Abs((decimal)(prepedido.ValorTotalDestePedidoComRA - vlTotal)) > maxErroArredondamento * prepedido.FormaPagtoCriacao.C_pce_prestacao_qtde)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
                else
                {
                    if (Math.Abs((decimal)(prepedido.VlTotalDestePedido - vlTotal)) > maxErroArredondamento * prepedido.FormaPagtoCriacao.C_pce_prestacao_qtde)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
            }
        }

        private void ValidarFormaPagtoSemEntrada(PrePedidoDto prepedido, List<string> lstErros, decimal maxErroArredondamento)
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

            //afazer: validar o valor da forma de pagto com o valor total do pedido
            if (lstErros.Count == 0)
            {
                if ((prepedido.FormaPagtoCriacao.C_pse_demais_prest_qtde + 1) != prepedido.FormaPagtoCriacao.Qtde_Parcelas)
                {
                    lstErros.Add("Quantidade de parcelas esta divergente!");
                }

                decimal vlTotal = (decimal)(prepedido.FormaPagtoCriacao.C_pse_prim_prest_valor +
                    (prepedido.FormaPagtoCriacao.C_pse_demais_prest_valor *
                    prepedido.FormaPagtoCriacao.C_pse_demais_prest_qtde));

                if (prepedido.PermiteRAStatus == 1)
                {
                    if (Math.Abs((decimal)(prepedido.ValorTotalDestePedidoComRA - vlTotal)) > maxErroArredondamento * prepedido.FormaPagtoCriacao.C_pse_demais_prest_qtde)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
                else
                {
                    if (Math.Abs((decimal)(prepedido.VlTotalDestePedido - vlTotal)) > maxErroArredondamento * prepedido.FormaPagtoCriacao.C_pse_demais_prest_qtde)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
            }
        }
    }
}
