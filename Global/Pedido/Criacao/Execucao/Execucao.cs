using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Pedido.Dados.Criacao;
using System;
using System.Linq;
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
            await Transportadora_Inicializar(pedido);
            await BlnMagentoPedidoComIndicador_Inicializar(pedido);
            await ParametroPercDesagioRALiquida_Inicializar();
        }

        public short TOrcamentista_Permite_RA_Status { get; private set; } = 0;
        public float Perc_desagio_RA { get; private set; } = 0;
        public float Perc_limite_RA_sem_desagio { get; private set; } = 0;
        public decimal Vl_limite_mensal { get; private set; } = 0;
        public decimal Vl_limite_mensal_consumido { get; private set; } = 0;
        public decimal Vl_limite_mensal_disponivel { get; private set; } = 0;
        public float Perc_comissao_e_desconto_a_utilizar { get; private set; } = 0;
        public string C_custoFinancFornecTipoParcelamento { get; private set; } = "";
        public short C_custoFinancFornecQtdeParcelas { get; private set; } = 0;
        public bool IsLojaGarantia { get; private set; } = false;

        private async Task ConfigurarVariaveisAdicionais(PedidoCriacaoDados pedido)
        {
            if (TabelasBanco.Indicador != null)
                TOrcamentista_Permite_RA_Status = TabelasBanco.Indicador.Permite_RA_Status;

            /* 10- valida se o pedido é com ou sem indicação
             * 11- valida percentual máximo de comissão */
            if (pedido.Cliente.Tipo.PessoaJuridica())
                Perc_comissao_e_desconto_a_utilizar = PercentualMaxDescEComissao.PercMaxComissaoEDescPJ;
            else
                Perc_comissao_e_desconto_a_utilizar = PercentualMaxDescEComissao.PercMaxComissaoEDesc;

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

            Perc_comissao_e_desconto_a_utilizar = Criacao.PedidoBll.VerificarPagtoPreferencial(tParametro, pedido, Perc_comissao_e_desconto_a_utilizar,
                    PercentualMaxDescEComissao, pedido.Valor.Vl_total);



            Tloja tloja = await (from l in Criacao.ContextoProvider.GetContextoLeitura().Tlojas
                                 where l.Loja == pedido.Ambiente.Loja
                                 select l).FirstOrDefaultAsync();
            IsLojaGarantia = false;
            if (tloja != null && tloja.Unidade_Negocio == Constantes.COD_UNIDADE_NEGOCIO_LOJA__GARANTIA)
                IsLojaGarantia = true;
        }


        #region blnPedidoECommerceCreditoOkAutomatico
        public bool BlnPedidoECommerceCreditoOkAutomatico
        {
            get
            {
                if (_blnPedidoECommerceCreditoOkAutomatico == null)
                    throw new ApplicationException($"_blnPedidoECommerceCreditoOkAutomatico acessado antes de ser calculado.");
                return _blnPedidoECommerceCreditoOkAutomatico.Value;
            }
            set => _blnPedidoECommerceCreditoOkAutomatico = value;
        }
        private bool? _blnPedidoECommerceCreditoOkAutomatico = null;
        #endregion

        #region Vl_aprov_auto_analise_credito
        public decimal Vl_aprov_auto_analise_credito
        {
            get
            {
                if (_vl_aprov_auto_analise_credito == null)
                    throw new ApplicationException($"Vl_aprov_auto_analise_credito acessado antes de ser calculado.");
                return _vl_aprov_auto_analise_credito.Value;
            }
            set => _vl_aprov_auto_analise_credito = value;
        }
        private decimal? _vl_aprov_auto_analise_credito = null;
        #endregion

        #region Comissao_loja_indicou
        //este pode ser null
        public float? Comissao_loja_indicou
        {
            get
            {
                if (!_comissao_loja_indicou_calculado)
                    throw new ApplicationException($"_comissao_loja_indicou  acessado antes de ser calculado.");
                return _comissao_loja_indicou;
            }
            set
            {
                _comissao_loja_indicou = value;
                _comissao_loja_indicou_calculado = true;
            }
        }
        private float? _comissao_loja_indicou = null;
        private bool _comissao_loja_indicou_calculado = false;
        #endregion


        #region Transportadora
        public Transportadora Transportadora
        {
            get
            {
                if (_transportadora == null)
                    throw new ApplicationException($"_transportadora  acessado antes de ser calculado.");
                return _transportadora;
            }
            private set => _transportadora = value;
        }
        private Transportadora? _transportadora = null;
        private async Task Transportadora_Inicializar(PedidoCriacaoDados pedido)
        {
            Transportadora = await Transportadora.CriarInstancia(pedido, Criacao.ContextoProvider);
        }
        #endregion

        #region Id_cliente
        public string Id_cliente
        {
            get
            {
                if (_id_cliente == null)
                    throw new ApplicationException($"_id_cliente  acessado antes de ser calculado.");
                return _id_cliente;
            }
            set => _id_cliente = value;
        }
        private string? _id_cliente = null;
        #endregion

        #region BlnMagentoPedidoComIndicador
        public bool BlnMagentoPedidoComIndicador
        {
            get
            {
                if (_blnMagentoPedidoComIndicador == null)
                    throw new ApplicationException($"_blnMagentoPedidoComIndicador acessado antes de ser calculado.");
                return _blnMagentoPedidoComIndicador.Value;
            }
            private set => _blnMagentoPedidoComIndicador = value;
        }
        private bool? _blnMagentoPedidoComIndicador = null;
        private async Task BlnMagentoPedidoComIndicador_Inicializar(PedidoCriacaoDados pedido)
        {
            /*
            'TRATAMENTO PARA CADASTRAMENTO DE PEDIDOS DO SITE MAGENTO DA BONSHOP
            dim blnMagentoPedidoComIndicador, sListaLojaMagentoPedidoComIndicador, vLoja, rParametro
            blnMagentoPedidoComIndicador = False
            sListaLojaMagentoPedidoComIndicador = ""
            if operacao_origem = OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO then
                If Not cria_recordset_otimista(tMAP_XML, msg_erro) then Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_CRIAR_ADO)

                set rParametro = get_registro_t_parametro(ID_PARAMETRO_MagentoPedidoComIndicadorListaLojaErp)
                sListaLojaMagentoPedidoComIndicador = Trim("" & rParametro.campo_texto)
                if sListaLojaMagentoPedidoComIndicador <> "" then
                    vLoja = Split(sListaLojaMagentoPedidoComIndicador, ",")
                    for i=LBound(vLoja) to UBound(vLoja)
                        if Trim("" & vLoja(i)) = loja then
                            blnMagentoPedidoComIndicador = True
                            exit for
                            end if
                        next
                    end if
                end if
            */
            _blnMagentoPedidoComIndicador = false;
            if (pedido.Ambiente.Operacao_origem != Constantes.Op_origem__pedido_novo.OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO)
                return;

            Tparametro rParametro = await UtilsGlobais.Util.BuscarRegistroParametro(
                Constantes.ID_PARAMETRO_MagentoPedidoComIndicadorListaLojaErp, Criacao.ContextoProvider);
            if (rParametro != null)
            {
                string sListaLojaMagentoPedidoComIndicador = (rParametro.Campo_texto ?? "").Trim();
                if (!String.IsNullOrWhiteSpace(sListaLojaMagentoPedidoComIndicador))
                {
                    var vLoja = sListaLojaMagentoPedidoComIndicador.Split(',');
                    foreach (var vLojai in vLoja)
                    {
                        if (vLojai.Trim() == pedido.Ambiente.Loja)
                        {
                            _blnMagentoPedidoComIndicador = true;
                            break;
                        }
                    }
                }
            }
        }
        #endregion


        #region ParametroPercDesagioRALiquida
        public float ParametroPercDesagioRALiquida
        {
            get
            {
                if (_parametroPercDesagioRALiquida == null)
                    throw new ApplicationException($"_parametroPercDesagioRALiquida  acessado antes de ser calculado.");
                return _parametroPercDesagioRALiquida.Value;
            }
            private set => _parametroPercDesagioRALiquida = value;
        }
        private float? _parametroPercDesagioRALiquida = null;
        private async Task ParametroPercDesagioRALiquida_Inicializar()
        {
            //rotina getParametroPercDesagioRALiquida do ASP
            ParametroPercDesagioRALiquida = 0;
            Tparametro tParametro = await UtilsGlobais.Util.BuscarRegistroParametro(
                Constantes.ID_PARAMETRO_PERC_DESAGIO_RA_LIQUIDA, Criacao.ContextoProvider);

            if (tParametro != null)
                ParametroPercDesagioRALiquida = tParametro.Campo_Real;
        }

        #endregion
    }
}
