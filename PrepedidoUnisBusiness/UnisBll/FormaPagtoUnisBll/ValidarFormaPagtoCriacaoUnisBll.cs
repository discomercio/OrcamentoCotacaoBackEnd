using InfraBanco;
using InfraBanco.Constantes;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using PrepedidoBusiness.Bll;
using PrepedidoBusiness.Bll.FormaPagtoBll;
using PrepedidoBusiness.Dto.FormaPagto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoUnisBusiness.UnisBll.FormaPagtoUnisBll
{
    public class ValidarFormaPagtoCriacaoUnisBll
    {
        public static async Task ValidarFormaPagto(FormaPagtoCriacaoUnisDto formaPagto, string apelido,
            string tipo_pessoa, List<string> lstErros, ContextoBdProvider contextoProvider)
        {
            //i)	Validar se tipo da opção de pagamento esta correta
            await ValidarTipoOpcaoPagto(formaPagto, apelido, tipo_pessoa, lstErros, contextoProvider);
            //h)	Validar a quantidade de parcelas

        }

        public static async Task ValidarTipoOpcaoPagto(FormaPagtoCriacaoUnisDto formaPagto, string apelido,
            string tipo_pessoa, List<string> lstErros, ContextoBdProvider contextoProvider)
        {
            /* buscar as opções de forma de pagto de cada Tipo de pagto para verificar se é permitido pelo 
             * Orcamentista e cliente
             * Ex: boleto, dinheiro etc...
             */
            FormaPagtoBll formaPagtoArclube = new FormaPagtoBll(contextoProvider);
            FormaPagtoDto tiposFormasPagto = await formaPagtoArclube.ObterFormaPagto(apelido, tipo_pessoa);

            //vamos verificar as opções de pagto conforme o tipo de pagto escolhido
            //if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
            //    ValidarOpcoesPagtoAvista(short.Parse(formaPagto.Op_av_forma_pagto), tiposFormasPagto.ListaAvista,
            //        lstErros);
            //else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
            //    ValidarOpcoesPagtoParcUnica(short.Parse(formaPagto.Op_av_forma_pagto),
            //        tiposFormasPagto.ListaParcUnica, lstErros);
            //else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
            //    ValidarOpcoesPagtoParcCartao(tiposFormasPagto.ParcCartaoInternet, lstErros);
            //else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
            //    ValidarOpcoesPagtoParcCartaoMaquineta(tiposFormasPagto.ParcCartaoMaquineta, lstErros);
            //else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
            //    ValidarOpcoesPagtoComEntrada(short.Parse(formaPagto.Op_pce_entrada_forma_pagto),
            //        short.Parse(formaPagto.Op_pce_prestacao_forma_pagto), tiposFormasPagto.ListaParcComEntrada, 
            //        tiposFormasPagto.ListaParcComEntPrestacao, lstErros);
        }

        private static void ValidarOpcoesPagtoAvista(short opcaoAVista, List<AvistaDto> lstOpcoes,
            List<string> lstErros)
        {
            bool existe = false;

            lstOpcoes.ForEach(x =>
            {
                if (opcaoAVista == x.Id) { }
                existe = true;
            });

            if (!existe)
                lstErros.Add("Opção para pagamento á vista inválido.");
        }

        private static void ValidarOpcoesPagtoParcUnica(short opcaoParcUnica, List<ParcUnicaDto> lstOpcoes,
            List<string> lstErros)
        {
            bool existe = false;

            lstOpcoes.ForEach(x =>
            {
                if (opcaoParcUnica == x.Id) { }
                existe = true;
            });

            if (!existe)
                lstErros.Add("Opção para pagamento Parcela Única inválido.");
        }

        private static void ValidarOpcoesPagtoParcCartao(bool permiteParcCartaoInternet, List<string> lstErros)
        {
            if (!permiteParcCartaoInternet)
                lstErros.Add("Opção para pagamento Parcelado no Cartão não permitido.");
        }

        private static void ValidarOpcoesPagtoParcCartaoMaquineta(bool permiteParcCartaoMaquieta, List<string> lstErros)
        {
            if (!permiteParcCartaoMaquieta)
                lstErros.Add("Opção para pagamento Parcelado no Cartão Maquineta não permitido.");
        }

        private static void ValidarOpcoesPagtoComEntrada(short opcaoPagtoComEntrada, short opcaoPrestacao,
            List<ParcComEntradaDto> lstParcComEntrada, List<ParcComEntPrestacaoDto> lstParcComEntradaPrest,
            List<string> lstErros)
        {
            bool existeComEntrada = false;
            bool existePrestacao = false;

            lstParcComEntrada.ForEach(x =>
            {
                if (opcaoPagtoComEntrada == x.Id)
                    existeComEntrada = true;
            });


        }

        public static void ValidarQtdeParcelas(int qtdParcelas, List<string> lstErros)
        {

        }
    }
}
