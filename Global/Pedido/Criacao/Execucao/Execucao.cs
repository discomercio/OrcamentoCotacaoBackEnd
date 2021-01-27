using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Execucao
{
    class Execucao
    {
        private readonly Pedido.Criacao.PedidoCriacao Criacao;
        public Execucao(PedidoCriacao pedidoCriacao)
        {
            this.Criacao = pedidoCriacao ?? throw new ArgumentNullException(nameof(pedidoCriacao));
        }

        #region propriedades calculadas
        //prporiedades calculadas. Elas são calculadas no inicio do processo, mas não podem ser determiandas
        //pelo cosntrutor. Por isso temos os getters que testam se estão null e garantem a inicialização
        //e tamém permitem usar as validações de nullable do c#
        private UtilsGlobais.Usuario.UsuarioPermissao? _usuarioPermissao = null;
        public UtilsGlobais.Usuario.UsuarioPermissao UsuarioPermissao
        {
            get
            {
                if (_usuarioPermissao == null)
                    throw new Exception("Pedido.Criacao.PedidoCriacao: _usuarioPermissao == null. Erro na inicialização do objeto.");
                return _usuarioPermissao;
            }
        }
        private Pedido.Criacao.Execucao.UtilsLoja.PercentualMaxDescEComissao? _percentualMaxDescEComissao = null;
        public Pedido.Criacao.Execucao.UtilsLoja.PercentualMaxDescEComissao PercentualMaxDescEComissao
        {
            get
            {
                if (_percentualMaxDescEComissao == null)
                    throw new Exception("Pedido.Criacao.PedidoCriacao: _percentualMaxDescEComissao == null. Erro na inicialização do objeto.");
                return _percentualMaxDescEComissao;
            }
        }

        private TabelasBanco? _tabelasBanco = null;
        public TabelasBanco TabelasBanco
        {
            get
            {
                if (_tabelasBanco == null)
                    throw new Exception("Pedido.Criacao.PedidoCriacao: TabelasBanco == null. Erro na inicialização do objeto.");
                return _tabelasBanco;
            }
        }
        #endregion

        public async Task ConfigurarExecucaoInicial(PedidoCriacaoDados pedido)
        {
            //busca a lista de permissões
            _usuarioPermissao = await UtilsGlobais.Usuario.UsuarioPermissao.ConstruirUsuarioPermissao(pedido.Ambiente.Usuario.ToUpper(), Criacao.ContextoProvider);
        }

        public async Task ConfigurarExecucaoComPermissaoOk(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno)
        {
            /* busca o percentual máximo de comissão*/
            _percentualMaxDescEComissao =
                await Pedido.Criacao.Execucao.UtilsLoja.PercentualMaxDescEComissao.ObterPercentualMaxDescEComissao(pedido.Ambiente.Loja, Criacao.ContextoProvider);

            _tabelasBanco = new TabelasBanco(pedido, retorno, Criacao);
            await TabelasBanco.Inicializar();

            await ConfigurarVariaveisAdicionais(pedido);
        }

        public short TOrcamentista_Permite_RA_Status { get; private set; } = 0;
        public float Perc_desagio_RA { get; private set; } = 0;
        public float Perc_limite_RA_sem_desagio { get; private set; } = 0;
        public decimal Vl_limite_mensal { get; private set; } = 0;
        public decimal Vl_limite_mensal_consumido { get; private set; } = 0;
        public decimal Vl_limite_mensal_disponivel { get; private set; } = 0;
        public float PercDescComissaoUtilizar { get; private set; } = 0;
        public string C_custoFinancFornecTipoParcelamento { get; private set; } = "";
        public short C_custoFinancFornecQtdeParcelas { get; private set; } = 0;

        private async Task ConfigurarVariaveisAdicionais(PedidoCriacaoDados pedido)
        {
            if (TabelasBanco.Indicador != null)
                TOrcamentista_Permite_RA_Status = TabelasBanco.Indicador.Permite_RA_Status;

            /* 10- valida se o pedido é com ou sem indicação
             * 11- valida percentual máximo de comissão */
            if (pedido.Cliente.Tipo.PessoaJuridica())
                PercDescComissaoUtilizar = PercentualMaxDescEComissao.PercMaxComissaoEDescPJ;
            else
                PercDescComissaoUtilizar = PercentualMaxDescEComissao.PercMaxComissaoEDesc;

            if (pedido.Ambiente.ComIndicador)
            {
                //perc_desagio_RA
                Perc_desagio_RA = await UtilsGlobais.Util.ObterPercentualDesagioRAIndicador(pedido.Ambiente.Indicador, Criacao.ContextoProvider);
                Perc_limite_RA_sem_desagio = await UtilsGlobais.Util.VerificarSemDesagioRA(Criacao.ContextoProvider);
                Vl_limite_mensal = await UtilsGlobais.Util.ObterLimiteMensalComprasDoIndicador(pedido.Ambiente.Indicador, Criacao.ContextoProvider);
                Vl_limite_mensal_consumido = await UtilsGlobais.Util.CalcularLimiteMensalConsumidoDoIndicador(pedido.Ambiente.Indicador, DateTime.Now, Criacao.ContextoProvider);
                Vl_limite_mensal_disponivel = Vl_limite_mensal - Vl_limite_mensal_consumido;
            }
            C_custoFinancFornecTipoParcelamento = Criacao.PrepedidoBll.ObterSiglaFormaPagto(pedido.FormaPagtoCriacao);
            C_custoFinancFornecQtdeParcelas = (short)Prepedido.PrepedidoBll.ObterCustoFinancFornecQtdeParcelasDeFormaPagto(pedido.FormaPagtoCriacao);



            /* 4- busca get_registro_t_parametro(ID_PARAMETRO_PercMaxComissaoEDesconto_Nivel2_MeiosPagto) */
            Tparametro tParametro = await UtilsGlobais.Util.BuscarRegistroParametro(
                Constantes.ID_PARAMETRO_PercMaxComissaoEDesconto_Nivel2_MeiosPagto, Criacao.ContextoProvider);

            PercDescComissaoUtilizar = Criacao.PedidoBll.VerificarPagtoPreferencial(tParametro, pedido, PercDescComissaoUtilizar,
                    PercentualMaxDescEComissao, pedido.Valor.Vl_total);
        }

    }
}
