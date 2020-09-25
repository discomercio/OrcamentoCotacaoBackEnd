using InfraBanco.Constantes;
using Prepedido.Dados.DetalhesPrepedido;
using System;
using System.Collections.Generic;

namespace Prepedido.FormaPagto
{
    public class ValidacoesFormaPagtoBll
    {
        private void ValidarSiglaFormaPagto(FormaPagtoCriacaoDados formaPagtoPrepedido, string c_custoFinancFornecTipoParcelamento,
            List<string> lstErros)
        {
            if (!string.IsNullOrEmpty(c_custoFinancFornecTipoParcelamento))
            {
                if (c_custoFinancFornecTipoParcelamento != formaPagtoPrepedido.CustoFinancFornecTipoParcelamento)
                    lstErros.Add($"Tipo do parcelamento (CustoFinancFornecTipoParcelamento '" +
                        $"{formaPagtoPrepedido.CustoFinancFornecTipoParcelamento}') está incorreto!");
            }
            else
            {
                lstErros.Add("Tipo do parcelamento inválido");
            }
        }

        public bool ValidarFormaPagto(FormaPagtoCriacaoDados formaPagtoPrepedido, List<string> lstErros,
            decimal limiteArredondamento, decimal maxErroArredondamento, string c_custoFinancFornecTipoParcelamento,
            Prepedido.Dados.FormaPagto.FormaPagtoDados formaPagtoDados, short permiteRA, decimal vl_total_nf, decimal vl_total)
        {
            ////vamos verificar a forma de pagamento com base no orçamentista que esta sendo enviado
            //FormaPagtoDto formasPagto = await formaPagtoBll.ObterFormaPagto(orcamentista, prepedido.DadosCliente.Tipo);

            bool retorno = false;

            if (!string.IsNullOrEmpty(formaPagtoPrepedido.CustoFinancFornecTipoParcelamento))
            {
                ValidarSiglaFormaPagto(formaPagtoPrepedido, c_custoFinancFornecTipoParcelamento, lstErros);
            }

            string msgNegarFormaPagto = "Forma de pagamento não aceita para esse indicador.";

            if (formaPagtoPrepedido.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
            {
                if (formaPagtoDados.ListaAvista?.Count > 0)
                    ValidarFormaPagtoAVista(formaPagtoPrepedido, lstErros, limiteArredondamento);
                else lstErros.Add(msgNegarFormaPagto);
            }

            else if (formaPagtoPrepedido.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
            {
                if (formaPagtoDados.ListaParcUnica?.Count > 0)
                    ValidarFormaPagtoParcelaUnica(formaPagtoPrepedido, lstErros, maxErroArredondamento, 
                        permiteRA, vl_total_nf, vl_total );
                else lstErros.Add(msgNegarFormaPagto);
            }
            else if (formaPagtoPrepedido.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
            {
                if (formaPagtoDados.ParcCartaoInternet)
                    ValidarFormaPagtoParceladoCartao(formaPagtoPrepedido, lstErros, maxErroArredondamento, permiteRA, vl_total_nf, vl_total);
                else lstErros.Add(msgNegarFormaPagto);
            }
            else if (formaPagtoPrepedido.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
            {
                if (formaPagtoDados.ParcCartaoMaquineta)
                    ValidarFormaPagtoParceladoCartaoMaquineta(formaPagtoPrepedido, lstErros, maxErroArredondamento, permiteRA, vl_total_nf, vl_total);
                else lstErros.Add(msgNegarFormaPagto);
            }
            else if (formaPagtoPrepedido.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
            {
                if (formaPagtoDados.ListaParcComEntrada?.Count > 0 && formaPagtoDados.ListaParcComEntPrestacao?.Count > 0)
                    ValidarFormaPagtoComEntrada(formaPagtoPrepedido, lstErros, maxErroArredondamento, permiteRA, vl_total_nf, vl_total);
                else lstErros.Add(msgNegarFormaPagto);
            }
            else if (formaPagtoPrepedido.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
            {
                if (formaPagtoDados.ListaParcSemEntPrestacao?.Count > 0 && formaPagtoDados.ListaParcSemEntPrimPrest?.Count > 0)
                    ValidarFormaPagtoSemEntrada(formaPagtoPrepedido, lstErros, maxErroArredondamento, permiteRA, vl_total_nf, vl_total);
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

        private void ValidarFormaPagtoAVista(FormaPagtoCriacaoDados formaPagtoPrepedido, List<string> lstErros, decimal limiteArredondamento)
        {
            if (string.IsNullOrEmpty(formaPagtoPrepedido.Op_av_forma_pagto))
                lstErros.Add("Indique a forma de pagamento (à vista).");

            if (formaPagtoPrepedido.Qtde_Parcelas != 1)
            {
                lstErros.Add("Quantidade da parcela esta divergente!");
            }
        }

        private void ValidarFormaPagtoParcelaUnica(FormaPagtoCriacaoDados formaPagtoPrepedido, List<string> lstErros, 
            decimal maxErroArredondamento, short permiteRA, decimal vl_total_nf, decimal vl_total)
        {
            if (string.IsNullOrEmpty(formaPagtoPrepedido.Op_pu_forma_pagto))
                lstErros.Add("Indique a forma de pagamento da parcela única.");
            else if (string.IsNullOrEmpty(formaPagtoPrepedido.C_pu_valor.ToString()))
                lstErros.Add("Indique o valor da parcela única.");
            else if (formaPagtoPrepedido.C_pu_valor <= 0)
                lstErros.Add("Valor da parcela única é inválido.");
            else if (string.IsNullOrEmpty(formaPagtoPrepedido.C_pu_vencto_apos.ToString()))
                lstErros.Add("Indique o intervalo de vencimento da parcela única.");
            else if (formaPagtoPrepedido.C_pu_vencto_apos <= 0)
                lstErros.Add("Intervalo de vencimento da parcela única é inválido.");

            if (lstErros.Count == 0)
            {
                if (formaPagtoPrepedido.Qtde_Parcelas != 1)
                    lstErros.Add("Quantidade da parcela esta divergente!");

                if (permiteRA == 1)
                {
                    if (Math.Abs((decimal)(vl_total_nf - formaPagtoPrepedido.C_pu_valor)) > maxErroArredondamento)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
                else
                {
                    if (Math.Abs((decimal)(vl_total - formaPagtoPrepedido.C_pu_valor)) > maxErroArredondamento)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
            }
        }

        private void ValidarFormaPagtoParceladoCartao(FormaPagtoCriacaoDados formaPagtoPrepedido, List<string> lstErros, 
            decimal maxErroArredondamento, short permiteRA, decimal vl_total_nf, decimal vl_total)
        {
            if (string.IsNullOrEmpty(formaPagtoPrepedido.C_pc_qtde.ToString()))
                lstErros.Add("Indique a quantidade de parcelas (parcelado no cartão [internet]).");
            else if (formaPagtoPrepedido.C_pc_qtde < 1)
                lstErros.Add("Quantidade de parcelas inválida (parcelado no cartão [internet]).");
            else if (string.IsNullOrEmpty(formaPagtoPrepedido.C_pc_valor.ToString()))
                lstErros.Add("Indique o valor da parcela (parcelado no cartão [internet]).");
            else if (formaPagtoPrepedido.C_pc_valor <= 0)
                lstErros.Add("Valor de parcela inválido (parcelado no cartão [internet]).");

            if (lstErros.Count == 0)
            {
                if (formaPagtoPrepedido.C_pc_qtde != formaPagtoPrepedido.Qtde_Parcelas)
                {
                    lstErros.Add("Quantidade de parcelas esta divergente!");
                }

                decimal vlTotal = (decimal)(formaPagtoPrepedido.C_pc_valor * formaPagtoPrepedido.C_pc_qtde);
                if (permiteRA == 1)
                {
                    if (Math.Abs((decimal)(vl_total_nf - vlTotal)) > maxErroArredondamento * formaPagtoPrepedido.C_pc_qtde)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
                else
                {
                    if (Math.Abs((decimal)(vl_total - vlTotal)) > maxErroArredondamento * formaPagtoPrepedido.C_pc_qtde)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
            }
        }

        private void ValidarFormaPagtoParceladoCartaoMaquineta(FormaPagtoCriacaoDados formaPagtoPrepedido, List<string> lstErros, 
            decimal maxErroArredondamento, short permiteRA, decimal vl_total_nf, decimal vl_total)
        {
            if (string.IsNullOrEmpty(formaPagtoPrepedido.C_pc_maquineta_qtde.ToString()))
                lstErros.Add("Indique a quantidade de parcelas (parcelado no cartão [maquineta]).");
            else if (formaPagtoPrepedido.C_pc_maquineta_qtde < 1)
                lstErros.Add("Quantidade de parcelas inválida (parcelado no cartão [maquineta]).");
            else if (string.IsNullOrEmpty(formaPagtoPrepedido.C_pc_maquineta_valor.ToString()))
                lstErros.Add("Indique o valor da parcela (parcelado no cartão [maquineta]).");
            else if (formaPagtoPrepedido.C_pc_maquineta_valor <= 0)
                lstErros.Add("Valor de parcela inválido (parcelado no cartão [maquineta]).");


            if (lstErros.Count == 0)
            {
                if (formaPagtoPrepedido.C_pc_maquineta_qtde != formaPagtoPrepedido.Qtde_Parcelas)
                {
                    lstErros.Add("Quantidade de parcelas esta divergente!");
                }

                decimal vlTotal = (decimal)(formaPagtoPrepedido.C_pc_maquineta_valor * formaPagtoPrepedido.C_pc_maquineta_qtde);
                if (permiteRA == 1)
                {
                    if (Math.Abs((decimal)(vl_total_nf - vlTotal)) > maxErroArredondamento * formaPagtoPrepedido.C_pc_maquineta_qtde)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
                else
                {
                    if (Math.Abs((decimal)(vl_total - vlTotal)) > maxErroArredondamento * formaPagtoPrepedido.C_pc_maquineta_qtde)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
            }
        }

        private void ValidarFormaPagtoComEntrada(FormaPagtoCriacaoDados formaPagtoPrepedido, List<string> lstErros, 
            decimal maxErroArredondamento, short permiteRA, decimal vl_total_nf, decimal vl_total)
        {
            if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.Op_pce_entrada_forma_pagto))
                lstErros.Add("Indique a forma de pagamento da entrada (parcelado com entrada).");
            else if (prepedido.FormaPagtoCriacao.C_pce_entrada_valor <= 0)
                lstErros.Add("Indique o valor da entrada (parcelado com entrada).");
            else if (prepedido.FormaPagtoCriacao.C_pce_entrada_valor <= 0 || 
                prepedido.FormaPagtoCriacao.C_pce_entrada_valor == null)
                lstErros.Add("Valor da entrada inválido (parcelado com entrada).");
            else if (string.IsNullOrEmpty(formaPagtoPrepedido.Op_pce_prestacao_forma_pagto))
                lstErros.Add("Indique a forma de pagamento das prestações (parcelado com entrada).");
            else if (prepedido.FormaPagtoCriacao.C_pce_prestacao_qtde <= 0)
                lstErros.Add("Indique a quantidade de prestações (parcelado com entrada).");
            else if (prepedido.FormaPagtoCriacao.C_pce_prestacao_qtde <= 0 ||
                prepedido.FormaPagtoCriacao.C_pce_prestacao_qtde == null)
                lstErros.Add("Quantidade de prestações inválida (parcelado com entrada).");
            else if (prepedido.FormaPagtoCriacao.C_pce_prestacao_valor <= 0)
                lstErros.Add("Indique o valor da prestação (parcelado com entrada).");
            else if (prepedido.FormaPagtoCriacao.C_pce_prestacao_valor <= 0 ||
                prepedido.FormaPagtoCriacao.C_pce_prestacao_valor == null)
                lstErros.Add("Valor de prestação inválido (parcelado com entrada).");
            else if (formaPagtoPrepedido.Op_pce_prestacao_forma_pagto != "7" &&
                formaPagtoPrepedido.Op_pce_prestacao_forma_pagto != "5")
            {
                if (prepedido.FormaPagtoCriacao.C_pce_prestacao_periodo <= 0 ||
                    prepedido.FormaPagtoCriacao.C_pce_prestacao_periodo == null)
                    lstErros.Add("Indique o intervalo de vencimento entre as parcelas (parcelado com entrada).");
            }
            else if (formaPagtoPrepedido.C_pce_prestacao_periodo <= 0)
                lstErros.Add("Intervalo de vencimento inválido (parcelado com entrada).");


            if (lstErros.Count == 0)
            {
                if ((formaPagtoPrepedido.C_pce_prestacao_qtde + 1) != formaPagtoPrepedido.Qtde_Parcelas)
                {
                    lstErros.Add("Quantidade de parcelas esta divergente!");
                }

                decimal vlTotal = (decimal)(formaPagtoPrepedido.C_pce_entrada_valor +
                    (formaPagtoPrepedido.C_pce_prestacao_valor *
                    formaPagtoPrepedido.C_pce_prestacao_qtde));

                if (permiteRA == 1)
                {
                    if (Math.Abs((decimal)(vl_total_nf - vlTotal)) > maxErroArredondamento * formaPagtoPrepedido.C_pce_prestacao_qtde)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
                else
                {
                    if (Math.Abs((decimal)(vl_total - vlTotal)) > maxErroArredondamento * formaPagtoPrepedido.C_pce_prestacao_qtde)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
            }
        }

        private void ValidarFormaPagtoSemEntrada(FormaPagtoCriacaoDados formaPagtoPrepedido, List<string> lstErros, 
            decimal maxErroArredondamento, short permiteRA, decimal vl_total_nf, decimal vl_total)
        {
            if (string.IsNullOrEmpty(formaPagtoPrepedido.Op_pse_prim_prest_forma_pagto))
                lstErros.Add("Indique a forma de pagamento da 1ª prestação (parcelado sem entrada).");
            else if (string.IsNullOrEmpty(formaPagtoPrepedido.C_pse_prim_prest_valor.ToString()))
                lstErros.Add("Indique o valor da 1ª prestação (parcelado sem entrada).");
            else if (formaPagtoPrepedido.C_pse_prim_prest_valor <= 0)
                lstErros.Add("Valor da 1ª prestação inválido (parcelado sem entrada).");
            else if (string.IsNullOrEmpty(formaPagtoPrepedido.C_pse_prim_prest_apos.ToString()))
                lstErros.Add("Indique o intervalo de vencimento da 1ª parcela (parcelado sem entrada).");
            else if (formaPagtoPrepedido.C_pse_prim_prest_apos <= 0)
                lstErros.Add("Intervalo de vencimento da 1ª parcela é inválido (parcelado sem entrada).");
            else if (string.IsNullOrEmpty(formaPagtoPrepedido.Op_pse_demais_prest_forma_pagto))
                lstErros.Add("Indique a forma de pagamento das demais prestações (parcelado sem entrada).");
            else if (string.IsNullOrEmpty(formaPagtoPrepedido.C_pse_demais_prest_qtde.ToString()))
                lstErros.Add("Indique a quantidade das demais prestações (parcelado sem entrada).");
            else if (formaPagtoPrepedido.C_pse_demais_prest_qtde <= 0)
                lstErros.Add("Quantidade de prestações inválida (parcelado sem entrada).");
            else if (string.IsNullOrEmpty(formaPagtoPrepedido.C_pse_demais_prest_valor.ToString()))
                lstErros.Add("Indique o valor das demais prestações (parcelado sem entrada).");
            else if (formaPagtoPrepedido.C_pse_demais_prest_valor <= 0)
                lstErros.Add("Valor de prestação inválido (parcelado sem entrada).");
            else if (string.IsNullOrEmpty(formaPagtoPrepedido.C_pse_demais_prest_periodo.ToString()))
                lstErros.Add("Indique o intervalo de vencimento entre as parcelas (parcelado sem entrada).");
            else if (formaPagtoPrepedido.C_pse_demais_prest_periodo < 0)
                lstErros.Add("Intervalo de vencimento inválido (parcelado sem entrada).");


            if (lstErros.Count == 0)
            {
                if ((formaPagtoPrepedido.C_pse_demais_prest_qtde + 1) != formaPagtoPrepedido.Qtde_Parcelas)
                {
                    lstErros.Add("Quantidade de parcelas esta divergente!");
                }

                decimal vlTotal = (decimal)(formaPagtoPrepedido.C_pse_prim_prest_valor +
                    (formaPagtoPrepedido.C_pse_demais_prest_valor *
                    formaPagtoPrepedido.C_pse_demais_prest_qtde));

                if (permiteRA == 1)
                {
                    if (Math.Abs((decimal)(vl_total_nf - vlTotal)) > maxErroArredondamento * formaPagtoPrepedido.C_pse_demais_prest_qtde)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
                else
                {
                    if (Math.Abs((decimal)(vl_total - vlTotal)) > maxErroArredondamento * formaPagtoPrepedido.C_pse_demais_prest_qtde)
                        lstErros.Add("Valor total da forma de pagamento diferente do valor total!");
                }
            }
        }
    }
}
