using System;

namespace InfraBanco.Constantes
{
    public static partial class Constantes
    {
        public static int FATOR_CRIPTO = 1209;
        public static int TAMANHO_SENHA_FORMATADA = 32;  // Procurar usar sempre potência de 2
        public static String PREFIXO_SENHA_FORMATADA = "0x";
        public static int TAMANHO_CAMPO_COMPRIMENTO_SENHA = 2;

        public static string TEL_BONSHOP_1 = "1139344400";
        public static string TEL_BONSHOP_2 = "1139344420";
        public static string TEL_BONSHOP_3 = "1139344411";

        public static string SEM_INDICADOR = "*_SEM_INDICADOR_*";

        public enum eTipoUsuarioPerfil
        {
            USUÁRIO_DA_CENTRAL = 1,
            USUARIO_LOJA = 2,
            INDICADOR_PARCEIRO = 3,
            CLIENTE = 4,
        }
        public enum TipoUsuario
        {
            NAO_IDENTIFICADO = -1,
            GESTOR = 0,
            VENDEDOR = 1,
            PARCEIRO = 2,
            VENDEDOR_DO_PARCEIRO = 3,
            CLIENTE = 4,
        }
        public enum TipoUsuarioContexto
        {
            UsuarioInterno = 1,
            Parceiro = 2,
            VendedorParceiro = 3,
            Cliente = 4
        }

        public static class TipoUsuarioPerfil
        {
            public static eTipoUsuarioPerfil getUsuarioPerfil(TipoUsuario tipoUsuario)
            {
                var retorno = eTipoUsuarioPerfil.USUÁRIO_DA_CENTRAL;

                switch (tipoUsuario)
                {
                    case TipoUsuario.GESTOR:
                        retorno = eTipoUsuarioPerfil.USUÁRIO_DA_CENTRAL;
                        break;
                    case TipoUsuario.VENDEDOR:
                        retorno = eTipoUsuarioPerfil.USUARIO_LOJA;
                        break;
                    case TipoUsuario.CLIENTE:
                    case TipoUsuario.PARCEIRO:
                    case TipoUsuario.VENDEDOR_DO_PARCEIRO:
                        retorno = eTipoUsuarioPerfil.INDICADOR_PARCEIRO;
                        break;
                    default:
                        break;
                }

                return retorno;
            }
        }

        public enum CodSistemaResponsavel
        {
            COD_SISTEMA_RESPONSAVEL_CADASTRO__ERP = 1,
            COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS = 2,
            COD_SISTEMA_RESPONSAVEL_CADASTRO__UNIS = 3,
            COD_SISTEMA_RESPONSAVEL_CADASTRO__ERP_WEBAPI = 4,   //Web api do ERP
            COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO = 5   //API magento
        };

        public enum Modulos
        {
            COD_MODULO_CENTRAL = 1,
            COD_MODULO_LOJA = 2,
            COD_MODULO_PREPEDIDO = 3,
            COD_MODULO_ORCAMENTOCOTACAO = 4,
        }

        public enum TipoParcela
        {
            A_VISTA = 1,
            PARCELA_UNICA = 2,
            PARCELA_DE_ENTRADA = 3,
            PRIMEIRA_PRESTACAO = 4,
            DEMAIS_PARCELAS_PRESTACAO = 5,
            PARCELAMENTO_CARTAO = 6
        }

        public const string T_PEDIDO_ANALISE_ENDERECO = "T_PEDIDO_ANALISE_ENDERECO";
        public const string T_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO = "T_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO";

        public const string ID_FORMA_PAGTO_DINHEIRO = "1";
        public const string ID_FORMA_PAGTO_DEPOSITO = "2";
        public const string ID_FORMA_PAGTO_CHEQUE = "3";
        public const string ID_FORMA_PAGTO_BOLETO = "4";
        public const string ID_FORMA_PAGTO_CARTAO = "5";
        public const string ID_FORMA_PAGTO_BOLETO_AV = "6";
        public const string ID_FORMA_PAGTO_CARTAO_MAQUINETA = "7";

        public const string AGUARDANDO_EMISSAO_BOLETO = "006";
        public const string ANALISE_CREDITO_USUARIO_AUTOMATICO = "AUTOMÁTICO";

        //'	Criptografia em SessionCtrl (tratamento da sessão expirada)
        public const int FATOR_CRIPTO_SESSION_CTRL = 1329;

        public const string COD_MARKETPLACE_ARCLUBE = "001";

        public const int MAX_TAM_OBS1 = 500;

        public const int FATOR_BD = 1209;

        public const string ID_USUARIO_SISTEMA = "SISTEMA";

        public const string TITULO_JANELA_MODULO_ORCAMENTO = "Pré-Pedido";

        public const int SWITCH_QUADRO_AVISO_POPUP = 1;
        public const int TIMER_CARREGA_AVISO_NOVO_MILISSEGUNDOS = 300000;
        public const int TIMER_CARREGA_ORCAMENTO_NOVO_MILISSEGUNDOS = 60000;
        public const int NUM_MAXIMO_TELEFONES_REPETIDOS_CAD_CLIENTES = 5;

        //'	SIGLAS DE UF

        public const string SIGLA_UF__SP = "SP";

        public const string SIGLA_UF__ES = "ES";

        public const string SIGLA_UF__TODAS = "**";


        public const string MODO_SELECAO_CD__AUTOMATICO = "AUTOMATICO";

        public const string MODO_SELECAO_CD__MANUAL = "MANUAL";


        public const string TIPO_SPLIT__MANUAL = "M";

        public const string TIPO_SPLIT__AUTOMATICO = "A";


        //'	Tamanho dos campos que identificam o usuário (t_USUARIO.usuario) e o orçamentista/indicador (t_ORCAMENTISTA_E_INDICADOR.apelido)

        public const int MAX_TAMANHO_ID_USUARIO = 10;

        public const int MAX_TAMANHO_ID_ORCAMENTISTA_E_INDICADOR = 20;

        public const int MAX_TAMANHO_SENHA = 16;

        public const int MAX_TAMANHO_LOJA = 3;

#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG
        public const int MAX_TAMANHO_ID_PEDIDO_MAGENTO = 9;
        public const int MAX_TAMANHO_ID_PEDIDO_MARKETPLACE = 20;
        public const int MIN_TAMANHO_ID_PEDIDO_MARKETPLACE = 12;
#endif


        //'	Percentual de deságio para RA Líquida

        public const int PERC_DESAGIO_RA_LIQUIDA = 25;
        public const string ID_PARAMETRO_PERC_DESAGIO_RA_LIQUIDA = "PERC_DESAGIO_RA_LIQUIDA";


        //'	PARÂMETRO DE FUNCIONAMENTO DO SITE (ARTVEN3 = BONSHOP; ARTVEN = FABRICANTE)

        public const string COD_SITE_ARTVEN_BONSHOP = "ArtBS";

        public const string COD_SITE_ARTVEN_FABRICANTE = "ArtFab";

        public const string COD_SITE_ASSISTENCIA_TECNICA = "AssTec";

        //'  TAMANHO DA QUANTIDADE DE DIAS DE INATIVIDADE DE UM INDICADOR NA LOJA
        public const int QTDE_DIAS_INDICADORES = 45;


        public const string CIELO_USUARIO_CLIENTE = "Cliente";

        public const string BRASPAG_USUARIO_CLIENTE = "Cliente";


        public const string CIELO_TRANSACAO_STATUS__CRIADA = "0";

        public const string CIELO_TRANSACAO_STATUS__EM_ANDAMENTO = "1";

        public const string CIELO_TRANSACAO_STATUS__AUTENTICADA = "2";

        public const string CIELO_TRANSACAO_STATUS__NAO_AUTENTICADA = "3";

        public const string CIELO_TRANSACAO_STATUS__AUTORIZADA = "4";

        public const string CIELO_TRANSACAO_STATUS__NAO_AUTORIZADA = "5";

        public const string CIELO_TRANSACAO_STATUS__CAPTURADA = "6";

        public const string CIELO_TRANSACAO_STATUS__NAO_CAPTURADA = "8";

        public const string CIELO_TRANSACAO_STATUS__CANCELADA = "9";

        public const string CIELO_TRANSACAO_STATUS__EM_AUTENTICACAO = "10";


        public const string OP_CIELO_OPERACAO__PAGAMENTO = "PAGTO";

        public const string OP_CIELO_OPERACAO__CANCELAMENTO = "CANCEL";


        public const string CIELO_TIPO_TRANSACAO__REQUISICAO_TRANSACAO = "REQ_TRANSACAO";

        public const string CIELO_TIPO_TRANSACAO__REQUISICAO_CONSULTA = "REQ_CONSULTA";


        public const string CIELO_FLUXO_XML__TX = "TX";

        public const string CIELO_FLUXO_XML__RX = "RX";


        public const string CIELO_BANDEIRA__VISA = "visa";

        public const string CIELO_BANDEIRA__MASTERCARD = "mastercard";

        public const string CIELO_BANDEIRA__AMEX = "amex";

        public const string CIELO_BANDEIRA__ELO = "elo";

        public const string CIELO_BANDEIRA__DINERS = "diners";

        public const string CIELO_BANDEIRA__DISCOVER = "discover";

        public const string CIELO_BANDEIRA__AURA = "aura";

        public const string CIELO_BANDEIRA__JCB = "jcb";

        public const string CIELO_BANDEIRA__CELULAR = "celular";



        public const string CLEARSALE_ANTIFRAUDE_STATUS__APROVACAO_AUTOMATICA = "APA";

        public const string CLEARSALE_ANTIFRAUDE_STATUS__APROVACAO_MANUAL = "APM";

        public const string CLEARSALE_ANTIFRAUDE_STATUS__REPROVADO_SEM_SUSPEITA = "RPM";

        public const string CLEARSALE_ANTIFRAUDE_STATUS__ANALISE_MANUAL = "AMA";

        public const string CLEARSALE_ANTIFRAUDE_STATUS__ERRO = "ERR";

        public const string CLEARSALE_ANTIFRAUDE_STATUS__NOVO = "NVO";

        public const string CLEARSALE_ANTIFRAUDE_STATUS__SUSPENSAO_MANUAL = "SUS";

        public const string CLEARSALE_ANTIFRAUDE_STATUS__CANCELADO_PELO_CLIENTE = "CAN";

        public const string CLEARSALE_ANTIFRAUDE_STATUS__FRAUDE_CONFIRMADA = "FRD";

        public const string CLEARSALE_ANTIFRAUDE_STATUS__REPROVACAO_AUTOMATICA = "RPA";

        public const string CLEARSALE_ANTIFRAUDE_STATUS__REPROVACAO_POR_POLITICA = "RPP";


        public const string BRASPAG_ANTIFRAUDE_CARTAO_FRAUDANALYSISRESPONSE_TRANSACTIONSTATUSCODE__STARTED = "500";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_FRAUDANALYSISRESPONSE_TRANSACTIONSTATUSCODE__ACCEPT = "501";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_FRAUDANALYSISRESPONSE_TRANSACTIONSTATUSCODE__REVIEW = "502";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_FRAUDANALYSISRESPONSE_TRANSACTIONSTATUSCODE__REJECT = "503";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_FRAUDANALYSISRESPONSE_TRANSACTIONSTATUSCODE__PENDENT = "504";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_FRAUDANALYSISRESPONSE_TRANSACTIONSTATUSCODE__UNFINISHED = "505";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_FRAUDANALYSISRESPONSE_TRANSACTIONSTATUSCODE__ABORTED = "506";


        public const string BRASPAG_ANTIFRAUDE_CARTAO_FRAUDANALYSISTRANSACTIONDETAILSRESPONSE_ANTIFRAUDTRANSACTIONSTATUSCODE__STARTED = "500";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_FRAUDANALYSISTRANSACTIONDETAILSRESPONSE_ANTIFRAUDTRANSACTIONSTATUSCODE__ACCEPT = "501";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_FRAUDANALYSISTRANSACTIONDETAILSRESPONSE_ANTIFRAUDTRANSACTIONSTATUSCODE__REVIEW = "502";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_FRAUDANALYSISTRANSACTIONDETAILSRESPONSE_ANTIFRAUDTRANSACTIONSTATUSCODE__REJECT = "503";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_FRAUDANALYSISTRANSACTIONDETAILSRESPONSE_ANTIFRAUDTRANSACTIONSTATUSCODE__PENDENT = "504";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_FRAUDANALYSISTRANSACTIONDETAILSRESPONSE_ANTIFRAUDTRANSACTIONSTATUSCODE__UNFINISHED = "505";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_FRAUDANALYSISTRANSACTIONDETAILSRESPONSE_ANTIFRAUDTRANSACTIONSTATUSCODE__ABORTED = "506";


        public const string BRASPAG_ANTIFRAUDE_CARTAO_GLOBAL_STATUS__STARTED = "G500";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_GLOBAL_STATUS__ACCEPT = "G501";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_GLOBAL_STATUS__REVIEW = "G502";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_GLOBAL_STATUS__REJECT = "G503";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_GLOBAL_STATUS__PENDENT = "G504";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_GLOBAL_STATUS__UNFINISHED = "G505";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_GLOBAL_STATUS__ABORTED = "G506";


        public const string BRASPAG_ANTIFRAUDE_CARTAO_UPDATESTATUSRESPONSE_REQUESTSTATUSCODE__FAIL = "0";

        public const string BRASPAG_ANTIFRAUDE_CARTAO_UPDATESTATUSRESPONSE_REQUESTSTATUSCODE__SUCCESS = "1";


        public const string BRASPAG_PAGADOR_CARTAO_PAYMENTDATARESPONSE_STATUS__CAPTURADA = "0";

        public const string BRASPAG_PAGADOR_CARTAO_PAYMENTDATARESPONSE_STATUS__AUTORIZADA = "1";

        public const string BRASPAG_PAGADOR_CARTAO_PAYMENTDATARESPONSE_STATUS__NAO_AUTORIZADA = "2";

        public const string BRASPAG_PAGADOR_CARTAO_PAYMENTDATARESPONSE_STATUS__ERRO_DESQUALIFICANTE = "3";

        public const string BRASPAG_PAGADOR_CARTAO_PAYMENTDATARESPONSE_STATUS__AGUARDANDO_RESPOSTA = "4";


        public const string BRASPAG_PAGADOR_CARTAO_CAPTURECREDITCARDTRANSACTIONRESPONSE_STATUS__CAPTURE_CONFIRMED = "0";

        public const string BRASPAG_PAGADOR_CARTAO_CAPTURECREDITCARDTRANSACTIONRESPONSE_STATUS__CAPTURE_DENIED = "2";


        public const string BRASPAG_PAGADOR_CARTAO_VOIDCREDITCARDTRANSACTIONRESPONSE_STATUS__VOID_CONFIRMED = "0";

        public const string BRASPAG_PAGADOR_CARTAO_VOIDCREDITCARDTRANSACTIONRESPONSE_STATUS__VOID_DENIED = "1";

        public const string BRASPAG_PAGADOR_CARTAO_VOIDCREDITCARDTRANSACTIONRESPONSE_STATUS__INVALID_TRANSACTION = "2";


        public const string BRASPAG_PAGADOR_CARTAO_REFUNDCREDITCARDTRANSACTIONRESPONSE_STATUS__REFUND_CONFIRMED = "0";

        public const string BRASPAG_PAGADOR_CARTAO_REFUNDCREDITCARDTRANSACTIONRESPONSE_STATUS__REFUND_DENIED = "1";

        public const string BRASPAG_PAGADOR_CARTAO_REFUNDCREDITCARDTRANSACTIONRESPONSE_STATUS__INVALID_TRANSACTION = "2";

        public const string BRASPAG_PAGADOR_CARTAO_REFUNDCREDITCARDTRANSACTIONRESPONSE_STATUS__REFUND_ACCEPTED = "3";


        public const string BRASPAG_PAGADOR_CARTAO_GETTRANSACTIONDATARESPONSE_STATUS__INDEFINIDA = "0";

        public const string BRASPAG_PAGADOR_CARTAO_GETTRANSACTIONDATARESPONSE_STATUS__CAPTURADA = "1";

        public const string BRASPAG_PAGADOR_CARTAO_GETTRANSACTIONDATARESPONSE_STATUS__AUTORIZADA = "2";

        public const string BRASPAG_PAGADOR_CARTAO_GETTRANSACTIONDATARESPONSE_STATUS__NAO_AUTORIZADA = "3";

        public const string BRASPAG_PAGADOR_CARTAO_GETTRANSACTIONDATARESPONSE_STATUS__CAPTURA_CANCELADA = "4";

        public const string BRASPAG_PAGADOR_CARTAO_GETTRANSACTIONDATARESPONSE_STATUS__ESTORNADA = "5";

        public const string BRASPAG_PAGADOR_CARTAO_GETTRANSACTIONDATARESPONSE_STATUS__AGUARDANDO_RESPOSTA = "6";

        public const string BRASPAG_PAGADOR_CARTAO_GETTRANSACTIONDATARESPONSE_STATUS__ERRO_DESQUALIFICANTE = "7";


        public const string BRASPAG_PAGADOR_CARTAO_GLOBAL_STATUS__INDEFINIDA = "G00";

        public const string BRASPAG_PAGADOR_CARTAO_GLOBAL_STATUS__CAPTURADA = "G01";

        public const string BRASPAG_PAGADOR_CARTAO_GLOBAL_STATUS__AUTORIZADA = "G02";

        public const string BRASPAG_PAGADOR_CARTAO_GLOBAL_STATUS__NAO_AUTORIZADA = "G03";

        public const string BRASPAG_PAGADOR_CARTAO_GLOBAL_STATUS__CAPTURA_CANCELADA = "G04";

        public const string BRASPAG_PAGADOR_CARTAO_GLOBAL_STATUS__ESTORNADA = "G05";

        public const string BRASPAG_PAGADOR_CARTAO_GLOBAL_STATUS__AGUARDANDO_RESPOSTA = "G06";

        public const string BRASPAG_PAGADOR_CARTAO_GLOBAL_STATUS__ERRO_DESQUALIFICANTE = "G07";

        public const string BRASPAG_PAGADOR_CARTAO_GLOBAL_STATUS__ESTORNO_PENDENTE = "G13";


        public const string BRASPAG_AF_DECISION__ACCEPT = "ACCEPT";

        public const string BRASPAG_AF_DECISION__REJECT = "REJECT";

        public const string BRASPAG_AF_DECISION__REVIEW = "REVIEW";

        public const string BRASPAG_AF_DECISION__ERROR = "ERROR";


        public const string OP_BRASPAG_OPERACAO__AF_PAG = "AF_PAG";//' ANTI-FRAUDE E PAGAMENTO (AnalyseAndAuthorizeOnSuccess)

        public const string OP_BRASPAG_OPERACAO__AUTHORIZE = "AUTHOR";//' AUTHORIZE (PAGADOR)


        public const string BRASPAG_TIPO_TRANSACAO__REQ_AUTHORIZE = "REQ_AUTHORIZE";


        public const string BRASPAG_TIPO_TRANSACAO__REQ_ANALYSE_AUTHORIZE = "REQ_ANALYSE_AUTH";

        public const string BRASPAG_TIPO_TRANSACAO__PAG_GET_TRANSACTION_DATA = "Get_TD";

        public const string BRASPAG_TIPO_TRANSACAO__PAG_CAPTURECREDITCARDTRANSACTION = "Capt_CCT";

        public const string BRASPAG_TIPO_TRANSACAO__PAG_VOIDCREDITCARDTRANSACTION = "Void_CCT";

        public const string BRASPAG_TIPO_TRANSACAO__PAG_REFUNDCREDITCARDTRANSACTION = "Refund_CCT";

        public const string BRASPAG_TIPO_TRANSACAO__PAG_GET_ORDERID_DATA = "Get_OID";

        public const string BRASPAG_TIPO_TRANSACAO__AF_FRAUD_ANALYSIS_TRANSACTION_DETAILS = "AF_TR_DET";

        public const string BRASPAG_TIPO_TRANSACAO__AF_UPDATE_STATUS = "AF_UpdSt";


        public const string BRASPAG_REGISTRA_PAGTO__OP_CAPTURA = "CAP";

        public const string BRASPAG_REGISTRA_PAGTO__OP_AUTORIZACAO = "AUT";

        public const string BRASPAG_REGISTRA_PAGTO__OP_CANCELAMENTO = "CAN";

        public const string BRASPAG_REGISTRA_PAGTO__OP_ESTORNO = "EST";


        public const string BRASPAG_FLUXO_XML__TX = "TX";

        public const string BRASPAG_FLUXO_XML__RX = "RX";


        public const string BRASPAG_BANDEIRA__VISA = "visa";

        public const string BRASPAG_BANDEIRA__MASTERCARD = "mastercard";

        public const string BRASPAG_BANDEIRA__AMEX = "amex";

        public const string BRASPAG_BANDEIRA__ELO = "elo";

        public const string BRASPAG_BANDEIRA__DINERS = "diners";

        public const string BRASPAG_BANDEIRA__DISCOVER = "discover";

        public const string BRASPAG_BANDEIRA__AURA = "aura";

        public const string BRASPAG_BANDEIRA__JCB = "jcb";

        public const string BRASPAG_BANDEIRA__CELULAR = "celular";


        public const string COD_REL_TRANSACOES_CIELO__TRANSACAO_AUTORIZADA = "TR_OK";

        public const string COD_REL_TRANSACOES_CIELO__TRANSACAO_NAO_AUTORIZADA = "TR_NOK";

        public const string COD_REL_TRANSACOES_CIELO__TRANSACAO_EM_SITUACAO_DESCONHECIDA = "TR_UNK";

        public const string COD_REL_TRANSACOES_CIELO__TRANSACAO_CANCELADA_PELO_USUARIO = "TR_CANC_USER";

        public const string COD_REL_TRANSACOES_CIELO__TRANSACAO_EM_ANDAMENTO = "TR_ONGOING";

        public const string COD_REL_TRANSACOES_CIELO__TRANSACAO_EM_AUTENTICACAO = "TR_ON_AUT";


        public const string COD_REL_TRANSACOES_BRASPAG__TRANSACAO_AUTORIZADA = "TR_AUTORIZADA";

        public const string COD_REL_TRANSACOES_BRASPAG__TRANSACAO_NAO_AUTORIZADA = "TR_NAO_AUTORIZADA";

        public const string COD_REL_TRANSACOES_BRASPAG__TRANSACAO_CAPTURADA = "TR_CAPTURADA";

        public const string COD_REL_TRANSACOES_BRASPAG__TRANSACAO_CAPTURA_CANCELADA = "TR_CAPTURA_CANCELADA";

        public const string COD_REL_TRANSACOES_BRASPAG__TRANSACAO_ESTORNADA = "TR_ESTORNADA";

        public const string COD_REL_TRANSACOES_BRASPAG__TRANSACAO_ESTORNO_PENDENTE = "TR_ESTORNO_PENDENTE";

        public const string COD_REL_TRANSACOES_BRASPAG__TRANSACAO_COM_ERRO_DESQUALIFICANTE = "TR_ERRO_DESQUALIFICANTE";

        public const string COD_REL_TRANSACOES_BRASPAG__TRANSACAO_AGUARDANDO_RESPOSTA = "TR_AGUARDANDO_RESPOSTA";


        public const string COD_REL_BRASPAG_AF_REVIEW__REVISAO_MANUAL_PENDENTE = "REV_MANUAL_PENDENTE";

        public const string COD_REL_BRASPAG_AF_REVIEW__REVISAO_MANUAL_TRATADA_ACCEPT = "REV_MANUAL_TRATADA_ACCEPT";

        public const string COD_REL_BRASPAG_AF_REVIEW__REVISAO_MANUAL_TRATADA_REJECT = "REV_MANUAL_TRATADA_REJECT";

        public const string COD_REL_BRASPAG_AF_REVIEW__APROVADO_AUTOMATICAMENTE = "APROVADO_AUTOMATICO";

        public const string COD_REL_BRASPAG_AF_REVIEW__REJEITADO_AUTOMATICAMENTE = "REJEITADO_AUTOMATICO";



        //'	VISANET

        public const string VISANET_AUTHENTTYPE = "1";     /*'	AUTHENTTYPE = 1: AUTENTICAÇÃO FEITA ATRAVÉS DA DIGITAÇÃO DA SENHA PELO CLIENTE (DEPENDE DE INTEGRAÇÃO ENTRE VISANET E EMISSOR DO CARTÃO)*/
        //'	AUTHENTTYPE = 0: A TRANSAÇÃO NÃO FAZ AUTENTICAÇÃO

        public const string VISANET_TIPO_CARTAO_CREDITO = "1";

        public const string VISANET_TIPO_CARTAO_DEBITO = "2";

        public const string MASTERCARD_TIPO_CARTAO_CREDITO = "3";


        public const string OP_VISANET_PAGAMENTO = "PAGTO";

        public const string OP_VISANET_CANCELAMENTO = "CANCEL";


        public const string COD_VISANET_PRAZO_PAGTO_LOJA = "PRAZO_LOJA";

        public const string COD_VISANET_PRAZO_PAGTO_EMISSOR = "PRAZO_CARTAO";


        public const string COD_MASTERCARD_PRAZO_PAGTO_LOJA = "MastPrazoLj";

        public const string COD_MASTERCARD_PRAZO_PAGTO_EMISSOR = "MastPrazoCar";


        public const string COD_AMEX_PRAZO_PAGTO_LOJA = "AmexPrazoLj";

        public const string COD_AMEX_PRAZO_PAGTO_EMISSOR = "AmexPrazoCar";


        public const string COD_ELO_PRAZO_PAGTO_LOJA = "EloPrazoLj";

        public const string COD_ELO_PRAZO_PAGTO_EMISSOR = "EloPrazoCar";


        public const string COD_DINERS_PRAZO_PAGTO_LOJA = "DnrsPrazoLj";

        public const string COD_DINERS_PRAZO_PAGTO_EMISSOR = "DnrsPrazoCar";


        public const string COD_DISCOVER_PRAZO_PAGTO_LOJA = "DiscPrazoLj";

        public const string COD_DISCOVER_PRAZO_PAGTO_EMISSOR = "DiscPrazoCar";


        public const string COD_AURA_PRAZO_PAGTO_LOJA = "AuraPrazoLj";

        public const string COD_AURA_PRAZO_PAGTO_EMISSOR = "AuraPrazoCar";


        public const string COD_JCB_PRAZO_PAGTO_LOJA = "JcbPrazoLj";

        public const string COD_JCB_PRAZO_PAGTO_EMISSOR = "JcbPrazoCar";


        public const string COD_CELULAR_PRAZO_PAGTO_LOJA = "CelPrazoLj";

        public const string COD_CELULAR_PRAZO_PAGTO_EMISSOR = "CelPrazoCar";


        //'	TIPOS DE ESTOQUE

        public const string ID_ESTOQUE_VENDA = "VDA";

        public const string ID_ESTOQUE_VENDIDO = "VDO";

        public const string ID_ESTOQUE_SEM_PRESENCA = "SPE";

        public const string ID_ESTOQUE_KIT = "KIT";

        public const string ID_ESTOQUE_SHOW_ROOM = "SHR";

        public const string ID_ESTOQUE_DANIFICADOS = "DAN";

        public const string ID_ESTOQUE_DEVOLUCAO = "DEV";

        public const string ID_ESTOQUE_ROUBO = "ROU";

        public const string ID_ESTOQUE_ENTREGUE = "ETG";

        //'	OPERAÇÕES (MOVIMENTOS) DO ESTOQUE

        public const string OP_ESTOQUE_ENTRADA = "CAD";

        public const string OP_ESTOQUE_VENDA = "VDA";

        public const string OP_ESTOQUE_CONVERSAO_KIT = "KIT";

        public const string OP_ESTOQUE_TRANSFERENCIA = "TRF";

        public const string OP_ESTOQUE_ENTREGA = "ETG";

        public const string OP_ESTOQUE_DEVOLUCAO = "DEV";

        //'	OPERAÇÕES NO LOG DE MOVIMENTAÇÃO DO ESTOQUE (T_ESTOQUE_LOG)

        public const string OP_ESTOQUE_LOG_ENTRADA = "CAD";

        public const string OP_ESTOQUE_LOG_ENTRADA_VIA_KIT = "CKT";

        public const string OP_ESTOQUE_LOG_VENDA = "VDA";

        public const string OP_ESTOQUE_LOG_CONVERSAO_KIT = "KIT";

        public const string OP_ESTOQUE_LOG_TRANSFERENCIA = "TRF";

        public const string OP_ESTOQUE_LOG_ENTREGA = "ETG";

        public const string OP_ESTOQUE_LOG_DEVOLUCAO = "DEV";

        public const string OP_ESTOQUE_LOG_ESTORNO = "EST";

        public const string OP_ESTOQUE_LOG_CANCELA_SEM_PRESENCA = "XSP";

        public const string OP_ESTOQUE_LOG_SPLIT = "SPL";

        public const string OP_ESTOQUE_LOG_VENDA_SEM_PRESENCA = "VSP";

        public const string OP_ESTOQUE_LOG_REMOVE_ENTRADA_ESTOQUE = "XEE";

        public const string OP_ESTOQUE_LOG_ENTRADA_ESTOQUE_ALTERA_NOVO_ITEM = "EEN";

        public const string OP_ESTOQUE_LOG_ENTRADA_ESTOQUE_ALTERA_INCREMENTA = "EEI";

        public const string OP_ESTOQUE_LOG_ENTRADA_ESTOQUE_ALTERA_DECREMENTA = "EED";

        public const string OP_ESTOQUE_LOG_PRODUTO_VENDIDO_SEM_PRESENCA_SAIDA = "SPS";

        public const string OP_ESTOQUE_LOG_TRANSF_PRODUTO_VENDIDO_ENTRE_PEDIDOS = "TVP";

        //'   STATUS DE ENTREGA DO PEDIDO

        public const string ST_ENTREGA_ESPERAR = "ESP";//  ' NENHUMA MERCADORIA SOLICITADA ESTÁ DISPONÍVEL

        public const string ST_ENTREGA_SPLIT_POSSIVEL = "SPL";//' PARTE DA MERCADORIA ESTÁ DISPONÍVEL PARA ENTREGA

        public const string ST_ENTREGA_SEPARAR = "SEP";//' TODA A MERCADORIA ESTÁ DISPONÍVEL E JÁ PODE SER SEPARADA PARA ENTREGA

        public const string ST_ENTREGA_A_ENTREGAR = "AET";//' A TRANSPORTADORA JÁ SEPAROU A MERCADORIA PARA ENTREGA

        public const string ST_ENTREGA_ENTREGUE = "ETG";//' MERCADORIA FOI ENTREGUE

        public const string ST_ENTREGA_CANCELADO = "CAN";//' VENDA FOI CANCELADA

        //'	CÓDIGOS DE TIPO DE PESSOA USADOS NA REGRA DE CONSUMO DO ESTOQUE (MULTI CD)
        //'	OBSERVAÇÕES: AO DEFINIR ESTES CÓDIGOS, NÃO UTILIZAR NENHUM TIPO DE SEPARADOR, POIS NA PÁGINA DE EDIÇÃO DA REGRA DE CONSUMO DO ESTOQUE,
        //'				O CONTROLE DA EDIÇÃO DOS CAMPOS É REALIZADO ATRAVÉS DE UMA COMBINAÇÃO DE INFORMAÇÕES QUE FORMAM OS ATRIBUTOS 'NAME' E 'ID'
        //'				DE ELEMENTOS HTML.
        //'				EX: <input type="checkbox" name="ckb_cd_RJ_PJC_1" id="ckb_cd_RJ_PJC_1" />
        //'				ONDE: ckb_cd_<UF>_<TipoPessoa>_<IdNfeEmitente>
        public const string COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA = "PF";

        public const string COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL = "PR";

        public const string COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE = "PJC";

        public const string COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE = "PJNC";

        public const string COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO = "PJI";

        //'	CÓDIGO P/ INDICAR QUE A NF FOI EMITIDA PELO PRÓPRIO CLIENTE NOS DADOS DE NF ARMAZENADOS NA DEVOLUÇÃO DE MERCADORIAS

        public const int COD_NFE_EMITENTE__CLIENTE = -1;

        //'	CÓDIGOS P/ ENDEREÇO DE DESTINO

        public const string COD_WMS_ENDERECO_DESTINO__CAD_CLIENTE = "C";

        public const string COD_WMS_ENDERECO_DESTINO__END_ENTREGA = "E";

        //'	CÓDIGOS P/ ANÁLISE DO ENDEREÇO

        public const string COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE = "C";

        public const string COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE_MEMORIZADO = "M";

        public const string COD_PEDIDO_AN_ENDERECO__END_ENTREGA = "E";

        public const string COD_PEDIDO_AN_ENDERECO__END_PARCEIRO = "P";

        //'	CÓDIGOS PARA O CAMPO "INSTALADOR INSTALA";

        public enum Instalador_Instala
        {
            COD_INSTALADOR_INSTALA_NAO_DEFINIDO = 0,
            COD_INSTALADOR_INSTALA_NAO = 1,
            COD_INSTALADOR_INSTALA_SIM = 2
        }

        public const string ORCAMENTISTA_INDICADOR_STATUS_ATIVO = "A";
        public const string ORCAMENTISTA_INDICADOR_STATUS_INATIVO = "I";

        //'   STATUS DE RECEBIMENTO DO PEDIDO POR PARTE DO CLIENTE

        public const string COD_ST_PEDIDO_RECEBIDO_NAO = "0";

        public const string COD_ST_PEDIDO_RECEBIDO_SIM = "1";

        public const string COD_ST_PEDIDO_RECEBIDO_NAO_DEFINIDO = "10";

        //'	STATUS DE PAGAMENTO "RECEBIDO" DO PEDIDO (EXISTE APENAS PARA SATISFAZER AO CLIENTE QUANDO O PEDIDO É IMPRESSO)

        public const string ST_RECEBIDO_SIM = "S";

        public const string ST_RECEBIDO_NAO = "N";

        public const string ST_RECEBIDO_PARCIAL = "P";

        //'	STATUS DE PAGAMENTO DO PEDIDO (CONTROLA DE FATO O ANDAMENTO DOS PAGAMENTOS)

        public const string ST_PAGTO_PAGO = "S";

        public const string ST_PAGTO_NAO_PAGO = "N";

        public const string ST_PAGTO_PARCIAL = "P";

        //'	STATUS DE FECHAMENTO DO PEDIDO (INDICA SE O ORÇAMENTO GEROU UM PEDIDO OU NÃO)

        public const string ST_FECHAMENTO_PEDIDO_FECHOU = "S";

        public const string ST_FECHAMENTO_PEDIDO_NAO_FECHOU = "N";

        public const string ST_FECHAMENTO_PEDIDO_CONCORRENTE = "C";

        //'	STATUS DO ORÇAMENTO

        public const string ST_ORCAMENTO_CANCELADO = "CAN";//' ORÇAMENTO FOI CANCELADO

        //'	STATUS DA ORDEM DE SERVIÇO

        public const string ST_OS_EM_ANDAMENTO = "AND";

        public const string ST_OS_ENCERRADA = "ENC";

        public const string ST_OS_CANCELADA = "CAN";

        //' 	SUFIXO QUE IDENTIFICA O ORÇAMENTO

        public const string SUFIXO_ID_ORCAMENTO = "Z";

        //' CÓDIGOS PARA OPERAÇÕES

        public const string OP_CONSULTA = "C";

        public const string OP_INCLUI = "I";

        public const string OP_EXCLUI = "E";

        public const string OP_ALTERA = "A";


        public const string OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO = "PED_NOVO_EC_SEMI_AUTO";


        public enum Cod_plataforma_origem
        {
            COD_PLATAFORMA_ORIGEM_PEDIDO__ERP = 0,
            COD_PLATAFORMA_ORIGEM_PEDIDO__MAGENTO = 1
        };


        //' CÓDIGOS PARA NÍVEL DOS USUÁRIOS

        public const string ID_VENDEDOR = "V";

        public const string ID_SEPARADOR = "S";

        public const string ID_ADMINISTRADOR = "A";

        public const string ID_GERENCIAL = "G";

        //' CÓDIGOS PARA TIPOS DE PAGAMENTO

        public const string COD_PAGTO_QUITACAO = "Q";

        public const string COD_PAGTO_PARCIAL = "P";

        public const string COD_PAGTO_VISANET = "V";

        public const string COD_PAGTO_CIELO = "C";

        public const string COD_PAGTO_BRASPAG = "B";

        public const string COD_PAGTO_GW_BRASPAG_CLEARSALE = "G";

        //' CÓDIGOS P/ ANÁLISE DE CRÉDITO DO PEDIDO

        public const string COD_AN_CREDITO_ST_INICIAL = "0";

        public const string COD_AN_CREDITO_PENDENTE = "1";

        public const string COD_AN_CREDITO_OK = "2";

        public const string COD_AN_CREDITO_PENDENTE_ENDERECO = "6";

        public const string COD_AN_CREDITO_OK_DEPOSITO_AGUARDANDO_DESBLOQUEIO = "7";

        public const string COD_AN_CREDITO_PENDENTE_VENDAS = "8";

        public const string COD_AN_CREDITO_OK_AGUARDANDO_DEPOSITO = "9";

        public const string COD_AN_CREDITO_NAO_ANALISADO = "10";//' PEDIDOS ANTIGOS QUE JÁ ESTAVAM NA BASE

        public const string COD_AN_CREDITO_PENDENTE_CARTAO = "PEND_CARTAO";//' CONSTANTE USADA P/ UNIFORMIZAR O TRATAMENTO DO STATUS NA TELA (PRINCIPALMENTE A DECODIFICAÇÃO DA DESCRIÇÃO), JÁ QUE ESSE STATUS NÃO EXISTE NO BD, POIS ESSE STATUS LÓGICO É DEFINIDO NO PEDIDO PELA COMBINAÇÃO DO STATUS DA ANÁLISE DE CRÉDITO 'COD_AN_CREDITO_ST_INICIAL' + FORMA DE PAGAMENTO USANDO SOMENTE PAGAMENTO POR CARTÃO
        // Novos códigos p/ análise de crédito do pedido - 18/03/2021
        public const string COD_AN_CREDITO_OK_AGUARDANDO_PAGTO_BOLETO_AV = "11";
        public const string COD_AN_CREDITO_PENDENTE_PAGTO_ANTECIPADO_BOLETO = "12";

        //' CÓDIGOS PARA PAGAMENTO ANTECIPADO
        public const string COD_PAGTO_ANTECIPADO_STATUS_NORMAL = "0";
        public const string COD_PAGTO_ANTECIPADO_STATUS_ANTECIPADO = "1";

        public const string COD_PAGTO_ANTECIPADO_QUITADO_STATUS_PENDENTE = "0";
        public const string COD_PAGTO_ANTECIPADO_QUITADO_STATUS_QUITADO = "1";

        //' CÓDIGOS P/ STATUS DA ANÁLISE DE ENDEREÇO
        public const string COD_ANALISE_ENDERECO_TRATADO_STATUS_INICIAL = "0";

        public const string COD_ANALISE_ENDERECO_TRATADO_STATUS_OK = "3";

        public const string COD_ANALISE_ENDERECO_TRATADO_STATUS_NOK = "9";

        //' CÓDIGOS P/ ENTREGA IMEDIATA

        public enum EntregaImediata
        {
            COD_ETG_IMEDIATA_ST_INICIAL = 0,
            COD_ETG_IMEDIATA_NAO = 1,
            COD_ETG_IMEDIATA_SIM = 2,
            COD_ETG_IMEDIATA_NAO_DEFINIDO = 10 //' PEDIDOS ANTIGOS QUE JÁ ESTAVAM NA BASE
        }


        //' CÓDIGOS P/ FLAG "BEM DE USO/CONSUMO";

        public enum Bem_DeUsoComum
        {
            COD_ST_BEM_USO_CONSUMO_NAO = 0,
            COD_ST_BEM_USO_CONSUMO_SIM = 1,
            COD_ST_BEM_USO_CONSUMO_NAO_DEFINIDO = 10 //' PEDIDOS ANTIGOS QUE JÁ ESTAVAM NA BASE
        }


        //' CÓDIGOS P/ INDICAR SE A COMISSÃO FOI PAGA OU NÃO

        public const string COD_COMISSAO_NAO_PAGA = "0";

        public const string COD_COMISSAO_PAGA = "1";

        public const string COD_COMISSAO_PAGA_NAO_DEFINIDO = "10";//' PEDIDOS ANTIGOS QUE JÁ ESTAVAM NA BASE

        public const double COMISSAO_INDICADOR_PERC_DESCONTO_SEM_NF = 16;
        public const double COMISSAO_INDICADOR_VL_NFS_MARGEM_ERRO = 5.00;

        //' CÓDIGOS P/ INDICAR SE O ITEM DEVOLVIDO OU VALOR DE PERDA JÁ FOI DESCONTADO OU NÃO DA COMISSÃO

        public const string COD_COMISSAO_NAO_DESCONTADA = "0";

        public const string COD_COMISSAO_DESCONTADA = "1";

        public const string COD_COMISSAO_DESCONTADA_NAO_DEFINIDO = "10";//' PEDIDOS ANTIGOS QUE JÁ ESTAVAM NA BASE

        //' CÓDIGOS P/ FLAG "GarantiaIndicadorStatus";

        public const string COD_GARANTIA_INDICADOR_STATUS__NAO = "0";

        public const string COD_GARANTIA_INDICADOR_STATUS__SIM = "1";

        public const string COD_GARANTIA_INDICADOR_STATUS__NAO_DEFINIDO = "10";//' PEDIDOS ANTIGOS QUE JÁ ESTAVAM NA BASE

        //' CÓDIGOS P/ SOLICITAÇÃO DE EMISSÃO DE NFe

        public const string COD_NFE_EMISSAO_SOLICITADA__PENDENTE = "0";

        public const string COD_NFE_EMISSAO_SOLICITADA__ATENDIDA = "1";

        public const string COD_NFE_EMISSAO_SOLICITADA__CANCELADA = "2";

        //' CÓDIGOS P/ STATUS DE GERAÇÃO DO ROMANEIO

        public const string COD_ROMANEIO_STATUS__INICIAL = "0";

        public const string COD_ROMANEIO_STATUS__OK = "1";

        public const string COD_ROMANEIO_STATUS__NAO_DEFINIDO = "10";

        //' CÓDIGOS P/ STATUS QUE INDICA SE A DANFE FOI IMPRESSA

        public const string COD_DANFE_IMPRESSA_STATUS__INICIAL = "0";

        public const string COD_DANFE_IMPRESSA_STATUS__OK = "1";

        public const string COD_DANFE_IMPRESSA_STATUS__NAO_DEFINIDO = "10";

        //' CÓDIGOS P/ STATUS QUE INDICA SE A DANFE ESTÁ MARCADA PARA IMPRIMIR

        public const string COD_DANFE_A_IMPRIMIR_STATUS__INICIAL = "0";

        public const string COD_DANFE_A_IMPRIMIR_STATUS__MARCADA = "1";

        public const string COD_DANFE_A_IMPRIMIR_STATUS__IMPRESSA = "2";

        public const string COD_DANFE_A_IMPRIMIR_STATUS__NAO_DEFINIDO = "10";

        //' CÓDIGOS P/ STATUS QUE INDICA SE A OS IMPOSTOS DA NOTA FISCAL ESTÃO OK

        public const string COD_CONTROLE_IMPOSTOS_STATUS__INICIAL = "0";

        public const string COD_CONTROLE_IMPOSTOS_STATUS__OK = "1";

        public const string COD_CONTROLE_IMPOSTOS_STATUS__NAO_DEFINIDO = "10";

        //' FORMA DE PAGAMENTO

        public const string COD_FORMA_PAGTO_A_VISTA = "1";

        public const string COD_FORMA_PAGTO_PARCELADO_CARTAO = "2";

        public const string COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA = "3";

        public const string COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA = "4";

        public const string COD_FORMA_PAGTO_PARCELA_UNICA = "5";

        public const string COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA = "6";


        public enum FormaPagto
        {
            ID_FORMA_PAGTO_DINHEIRO = 1,
            ID_FORMA_PAGTO_DEPOSITO = 2,
            ID_FORMA_PAGTO_CHEQUE = 3,
            ID_FORMA_PAGTO_BOLETO = 4,
            ID_FORMA_PAGTO_CARTAO = 5,
            ID_FORMA_PAGTO_BOLETO_AV = 6,
            ID_FORMA_PAGTO_CARTAO_MAQUINETA = 7
        }



        public const string CTRL_PAGTO_MODULO__BOLETO = "1";

        public const string CTRL_PAGTO_MODULO__CHEQUE = "2";

        public const string CTRL_PAGTO_MODULO__VISA = "3";

        public const string CTRL_PAGTO_MODULO__BRASPAG_CARTAO = "4";

        public const string CTRL_PAGTO_MODULO__BRASPAG_CLEARSALE = "5";

        public const string CTRL_PAGTO_MODULO__BRASPAG_WEBHOOK = "6";


        public const string CTRL_PAGTO_STATUS__CONTROLE_MANUAL = "0";

        public const string CTRL_PAGTO_STATUS__CADASTRADO_INICIAL = "1";

        public const string CTRL_PAGTO_STATUS__BOLETO_BAIXADO = "3";

        public const string CTRL_PAGTO_STATUS__CHEQUE_DEVOLVIDO = "4";

        public const string CTRL_PAGTO_STATUS__VISA_CANCELADO = "5";

        public const string CTRL_PAGTO_STATUS__BOLETO_PAGO_CHEQUE_VINCULADO = "6";

        public const string CTRL_PAGTO_STATUS__BOLETO_COM_PAGAMENTO_CANCELADO = "7";

        public const string CTRL_PAGTO_STATUS__PAGO = "10";


        public const string COD_BOLETO_ARQ_RETORNO_ST_PROCESSAMENTO__EM_PROCESSAMENTO = "1";

        public const string COD_BOLETO_ARQ_RETORNO_ST_PROCESSAMENTO__SUCESSO = "2";

        public const string COD_BOLETO_ARQ_RETORNO_ST_PROCESSAMENTO__FALHA = "3";


        public const string ST_T_FIN_PEDIDO_HIST_PAGTO__PREVISAO = "1";

        public const string ST_T_FIN_PEDIDO_HIST_PAGTO__QUITADO = "2";

        public const string ST_T_FIN_PEDIDO_HIST_PAGTO__CANCELADO = "3";

        //' CÓDIGOS P/ STATUS QUE INDICA SE CLIENTE É OU NÃO CONTRIBUINTE DO ICMS

        public enum ContribuinteICMS
        {
            COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL = 0,
            COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO = 1,
            COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM = 2,
            COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO = 3
        }


        //' CÓDIGOS P/ STATUS QUE INDICA SE CLIENTE É OU NÃO PRODUTOR RURAL
        public enum ProdutorRural
        {
            COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL = 0,
            COD_ST_CLIENTE_PRODUTOR_RURAL_NAO = 1,
            COD_ST_CLIENTE_PRODUTOR_RURAL_SIM = 2
        }


        //'   CÓDIGOS DE STATUS DE DEVOLUÇÃO EM PEDIDO
        public const string COD_ST_PEDIDO_DEVOLUCAO__CADASTRADA = "1";
        public const string COD_ST_PEDIDO_DEVOLUCAO__EM_ANDAMENTO = "2";
        public const string COD_ST_PEDIDO_DEVOLUCAO__MERCADORIA_RECEBIDA = "3";
        public const string COD_ST_PEDIDO_DEVOLUCAO__FINALIZADA = "4";
        public const string COD_ST_PEDIDO_DEVOLUCAO__REPROVADA = "5";
        public const string COD_ST_PEDIDO_DEVOLUCAO__CANCELADA = "6";
        public const string COD_ST_PEDIDO_DEVOLUCAO__INDEFINIDO = "0";

        //'   CÓDIGOS T_CODIGO_DESCRICAO PRÉ-DEVOLUÇÃO
        public const string CREDITO_TRANSACAO__TRANSFERENCIA = "001";
        public const string CREDITO_TRANSACAO__ESTORNO = "002";
        public const string CREDITO_TRANSACAO__REEMBOLSO = "003";
        public const string COD_PEDIDO_DEVOLUCAO_TAXA_FORMA_PAGTO__DESCONTO_COMISSAO = "001";
        public const string COD_PEDIDO_DEVOLUCAO_TAXA_FORMA_PAGTO__DEPOSITO_BANCARIO = "002";
        public const string COD_PEDIDO_DEVOLUCAO_TAXA_FORMA_PAGTO__ABATIMENTO_CREDITO = "003";
        public const string COD_PEDIDO_DEVOLUCAO_TAXA_RESPONSAVEL__VENDEDOR = "001";
        public const string COD_PEDIDO_DEVOLUCAO_TAXA_RESPONSAVEL__PARCEIRO = "002";
        public const string COD_PEDIDO_DEVOLUCAO_TAXA_RESPONSAVEL__CLIENTE = "003";
        public const string COD_PEDIDO_DEVOLUCAO_TAXA_RESPONSAVEL__VENDEDOR_PARCEIRO = "004";

        //'   PRÉ-DEVOLUÇÃO: CONSTANTES AUXILIARES
        public const string TAXA_ADMINISTRATIVA__NAO = "0";
        public const string TAXA_ADMINISTRATIVA__SIM = "1";
        public const int PEDIDO_DEVOLUCAO_QTDE_FOTO = 6;

        //'	TIPO DE MENSAGEM DO BLOCO DE NOTAS DO PEDIDO
        public const int COD_TIPO_MSG_BLOCO_NOTAS_PEDIDO__MANUAL = 0;
        public const int COD_TIPO_MSG_BLOCO_NOTAS_PEDIDO__AUTOMATICA_EDICAO_ENDERECO = 1100;
        public const int COD_TIPO_MSG_BLOCO_NOTAS_PEDIDO__AUTOMATICA_EDICAO_FORMA_PAGTO = 1200;
        public const int COD_TIPO_MSG_BLOCO_NOTAS_PEDIDO__AUTOMATICA_EDICAO_INDICADOR = 1300;
        public const int COD_TIPO_MSG_BLOCO_NOTAS_PEDIDO__AUTOMATICA_SPLIT_MANUAL = 1400;
        public const int COD_TIPO_MSG_BLOCO_NOTAS_PEDIDO__AUTOMATICA_SPLIT_AUTOMATICO = 1500;

        //'	NÍVEL DE ACESSO DO BLOCO DE NOTAS DO PEDIDO
        public const int COD_NIVEL_ACESSO_BLOCO_NOTAS_PEDIDO__NAO_DEFINIDO = 0;
        public const int COD_NIVEL_ACESSO_BLOCO_NOTAS_PEDIDO__ILIMITADO = -1;
        public const int COD_NIVEL_ACESSO_BLOCO_NOTAS_PEDIDO__PUBLICO = 10;
        public const int COD_NIVEL_ACESSO_BLOCO_NOTAS_PEDIDO__RESTRITO = 20;
        public const int COD_NIVEL_ACESSO_BLOCO_NOTAS_PEDIDO__SIGILOSO = 30;

        //'	NÍVEL DE ACESSO DO CHAMADO DO PEDIDO
        public const int COD_NIVEL_ACESSO_CHAMADO_PEDIDO__NAO_DEFINIDO = 0;
        public const int COD_NIVEL_ACESSO_CHAMADO_PEDIDO__ILIMITADO = -1;
        public const int COD_NIVEL_ACESSO_CHAMADO_PEDIDO__PUBLICO = 10;
        public const int COD_NIVEL_ACESSO_CHAMADO_PEDIDO__PUBLICO_INTERNO = 20;
        public const int COD_NIVEL_ACESSO_CHAMADO_PEDIDO__RESTRITO = 30;
        public const int COD_NIVEL_ACESSO_CHAMADO_PEDIDO__SIGILOSO = 40;

        //'	FLUXO DAS MENSAGENS GRAVADAS NAS OCORRÊNCIAS EM PEDIDOS
        public const string COD_FLUXO_MENSAGEM_OCORRENCIAS_EM_PEDIDOS__LOJA_PARA_CENTRAL = "LJ->CE";
        public const string COD_FLUXO_MENSAGEM_OCORRENCIAS_EM_PEDIDOS__CENTRAL_PARA_LOJA = "CE->LJ";

        //'   FLUXO DAS MENSAGENS GRAVADAS NOS CHAMADOS EM PEDIDOS
        public const string COD_FLUXO_MENSAGEM_CHAMADOS_EM_PEDIDOS__TX = "TX";
        public const string COD_FLUXO_MENSAGEM_CHAMADOS_EM_PEDIDOS__RX = "RX";

        //'   EMAIL REMETENTE DE ENVIO DE NOTIFICAÇÃO
        public const string EMAILSNDSVC_REMETENTE__CHAMADOS_EM_PEDIDOS = "modulo_chamados@tropikal.com.br";
        public const string EMAILSNDSVC_REMETENTE__PEDIDO_DEVOLUCAO = "modulo_devolucao@tropikal.com.br";

        public const string EMAILSNDSVC_REMETENTE__SENTINELA_SISTEMA = "sentinela_sistema@tropikal.com.br";


        //'	GRUPOS DE CÓDIGOS ARMAZENADOS EM T_CODIGO_DESCRICAO

        public const string GRUPO_T_CODIGO_DESCRICAO__OCORRENCIAS_EM_PEDIDOS__TIPO_OCORRENCIA = "OcorrenciasEmPedidos_TipoOcorrencia";
        public const string GRUPO_T_CODIGO_DESCRICAO__OCORRENCIAS_EM_PEDIDOS__MOTIVO_ABERTURA = "OcorrenciaPedido_MotivoAbertura";

        public const string GRUPO_T_CODIGO_DESCRICAO__CAD_ORCAMENTISTA_E_INDICADOR__FORMA_COMO_CONHECEU = "CadOrcamentistaEIndicador_FormaComoConheceu";

        public const string GRUPO_T_CODIGO_DESCRICAO__CANCELAMENTOPEDIDO_MOTIVO = "CancelamentoPedido_Motivo";

        public const string GRUPO_T_CODIGO_DESCRICAO__CANCELAMENTOPEDIDO_MOTIVO_SUB = "CancelamentoPedido_Motivo_Sub";
        public const string GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA = "EndEtg_justificativa";
        public const string GRUPO_T_CODIGO_DESCRICAO__PEDIDO_TIPO_FRETE = "Pedido_TipoFrete";
        public const string GRUPO_T_CODIGO_DESCRICAO__PEDIDOECOMMERCE_ORIGEM = "PedidoECommerce_Origem";
        public const string GRUPO_T_CODIGO_DESCRICAO__PEDIDOECOMMERCE_ORIGEM_GRUPO = "PedidoECommerce_Origem_Grupo";
        public const string GRUPO_T_CODIGO_DESCRICAO__CHAMADOS_EM_PEDIDOS__MOTIVO_ABERTURA = "PedidoChamado_MotivoAbertura";
        public const string GRUPO_T_CODIGO_DESCRICAO__CHAMADOS_EM_PEDIDOS__MOTIVO_FINALIZACAO = "PedidoChamado_MotivoFinalizacao";
        public const string GRUPO_T_CODIGO_DESCRICAO__CHAMADOS_EM_PEDIDOS__DEPARTAMENTO = "PedidoChamado_Depto";
        public const string GRUPO_T_CODIGO_DESCRICAO__AC_PENDENTE_VENDAS_MOTIVO = "AnaliseCredito_PendenteVendas_Motivo";
        public const string GRUPO_T_CODIGO_DESCRICAO__PEDIDO_DEVOLUCAO__CREDITO_TRANSACAO = "DevolucaoPedido_CreditoTransacao";
        public const string GRUPO_T_CODIGO_DESCRICAO__PEDIDO_DEVOLUCAO__LOCAL_COLETA = "DevolucaoPedido_LocalColeta";
        public const string GRUPO_T_CODIGO_DESCRICAO__PEDIDO_DEVOLUCAO__MOTIVO = "DevolucaoPedido_Motivo";
        public const string GRUPO_T_CODIGO_DESCRICAO__PEDIDO_DEVOLUCAO__PROCEDIMENTO = "DevolucaoPedido_Procedimento";
        public const string GRUPO_T_CODIGO_DESCRICAO__PEDIDO_DEVOLUCAO__TAXA_FORMA_PAGAMENTO = "DevolucaoPedido_TaxaFormaDePagamento";
        public const string GRUPO_T_CODIGO_DESCRICAO__PEDIDO_DEVOLUCAO__TAXA_RESPONSAVEL = "DevolucaoPedido_TaxaResponsavel";

        //'	TIPO DE ESTABELECIMENTO DO PARCEIRO

        public const string COD_PARCEIRO_TIPO_ESTABELECIMENTO__NAO_INFORMADO = "0";

        public const string COD_PARCEIRO_TIPO_ESTABELECIMENTO__CASA = "1";

        public const string COD_PARCEIRO_TIPO_ESTABELECIMENTO__ESCRITORIO = "2";

        public const string COD_PARCEIRO_TIPO_ESTABELECIMENTO__LOJA = "3";

        public const string COD_PARCEIRO_TIPO_ESTABELECIMENTO__OFICINA = "4";


        //'	TIPO DE FRETE ADICIONAL

        public const string COD_FRETE_ADICIONAL__DEVOLUCAO = "D";

        public const string COD_FRETE_ADICIONAL__REENTREGA = "R";

        //'	INDICA QUEM PAGA O FRETE ADICIONAL

        public const string COD_FRETE_ADICIONAL__PAGO_PELO_CLIENTE = "C";

        public const string COD_FRETE_ADICIONAL__PAGO_PELA_EMPRESA = "E";

        //' CÓDIGOS QUE IDENTIFICAM SE É PESSOA FÍSICA OU JURÍDICA

        public const string ID_PF = "PF";

        public const string ID_PJ = "PJ";


        public class TipoPessoa
        {
            private readonly string tipo = ID_PJ;
            public TipoPessoa(string tipoString)
            {
                if (tipoString.ToUpper() == ID_PF)
                {
                    tipo = ID_PF;
                    return;
                }
                if (tipoString.ToUpper() == ID_PJ)
                {
                    tipo = ID_PJ;
                    return;
                }
                throw new ArgumentException($"InfraBanco.Constantes.Constantes.TipoPessoa: tipo desconhecido: {tipoString}");
            }
            public static bool TipoValido(string tipoString)
            {
                if (tipoString.ToUpper() == ID_PF)
                    return true;
                if (tipoString.ToUpper() == ID_PJ)
                    return true;

                return false;
            }
            public bool PessoaFisica() { return tipo == ID_PF; }
            public bool PessoaJuridica() { return !PessoaFisica(); }
            public string ParaString() { return tipo; }
        }

        //' CÓDIGO USADO NA TABELA t_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FORMA_PAGTO PARA INDICAR UMA RESTRIÇÃO QUE SE APLICA A TODOS OS PARCEIROS

        public const string ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS = "*_A_L_L_*";


        //'	PERÍODO MÁXIMO EM QUE O DANFE FICA ACESSÍVEL NO PEDIDO

        public const int MAX_PERIODO_LINK_DANFE_DISPONIVEL_NO_PEDIDO_EM_DIAS = 60;

        //' TAMANHO MÁXIMO DO CAMPO NO BD

        public const int MAX_OBS_2 = 10;

        //' QUANTIDADE MÍNIMA DE LINHAS DO QUADRO DE ITENS QUANDO O PEDIDO É IMPRESSO NA IMPRESSORA

        public const int MIN_LINHAS_ITENS_IMPRESSAO_PEDIDO = 4;

        public const int MIN_LINHAS_ITENS_IMPRESSAO_ORCAMENTO = 4;

        //' QUANTIDADE MÁXIMA DE ITENS EM UM PEDIDO

        public const int MAX_ITENS = 12;

        //' QUANTIDADE MÁXIMA DE PRODUTOS QUE COMPÕEM UM PRODUTO COMPOSTO

        public const int MAX_PRODUTO_COMPOSTO_ITENS = 12;

        //' QUANTIDADE MÁXIMA DE PEDIDOS P/ OS QUAIS SE PODE CADASTRAR O VALOR DO FRETE POR TELA	

        public const int MAX_ITENS_ANOTA_FRETE_PEDIDO = 10;

        //' QUANTIDADE MÁXIMA DE ITENS P/ OS QUAIS SE PODE CADASTRAR SENHA DE DESCONTO SUPERIOR POR TELA

        public const int MAX_ITENS_SENHA_DESCONTO = 12;

        //' QUANTIDADE MÁXIMA DE ITENS QUE PODEM SER TRANSFERIDOS DE UM PEDIDO P/ OUTRO	

        public const int MAX_ITENS_TRANSF_PRODUTOS_ENTRE_PEDIDOS = 12;

        //' QUANTIDADE MÁXIMA DE VOLUMAS EM UMA ORDEM DE SERVIÇO

        public const int MAX_VOLUMES_OS = 6;

        //' QUANTIDADE MÁXIMA DE PRODUTOS EM UMA OPERAÇÃO DE ENTRADA NO ESTOQUE

        public const int MAX_PRODUTOS_ENTRADA_ESTOQUE = 30;

        //' QUANTIDADE MÁXIMA DE PRODUTOS QUE PODEM COMPOR 1 KIT

        public const int MAX_PRODUTOS_CONVERSOR_KIT = 30;

        //' QUANTIDADE MÁXIMA DE PERCENTUAIS DE DESCONTO P/ A TABELA DE COMISSÃO DOS VENDEDORES

        public const int MAX_LINHAS_TABELA_COMISSAO_VENDEDOR = 30;

        //' QUANTIDADE MÁXIMA DE REGISTROS COM OPÇÕES DE PARCELAMENTO P/ CADA FORNECEDOR

        public const int MAX_LINHAS_TABELA_CUSTO_FINANCEIRO_FORNECEDOR = 24;

        public const int MAX_DECIMAIS_COEFICIENTE_CUSTO_FINANCEIRO_FORNECEDOR = 6;

        public const string COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA = "CE";

        public const string COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA = "SE";

        public const string COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA = "AV";

        //' QUANTIDADE MÁXIMA DE PRODUTOS NA OPERAÇÃO DE TRANSFERÊNCIA ENTRE ESTOQUES

        public const int MAX_PRODUTOS_TRANSFERENCIA_ESTOQUE = 30;

        //' VALOR ACEITO COMO MARGEM DE ERRO PARA ARREDONDAMENTOS

        public const double MAX_VALOR_MARGEM_ERRO_PAGAMENTO = 0.5;

        //' REF BANCÁRIA NO CADASTRO DE CLIENTES

        public const int MAX_REF_BANCARIA_CLIENTE_PF = 1;

        public const int MAX_REF_BANCARIA_CLIENTE_PJ = 1;

        //' REF COMERCIAL NO CADASTRO DE CLIENTES

        public const int MAX_REF_COMERCIAL_CLIENTE_PJ = 3;

        //' REF PROFISSIONAL NO CADASTRO DE CLIENTES

        public const int MAX_REF_PROFISSIONAL_CLIENTE_PF = 1;


        public const int MAX_SERVER_SCRIPT_TIMEOUT_EM_SEG = 300;

        public const int MAX_SERVER_SCRIPT_EXTENDED_TIMEOUT_EM_SEG = 600;

        public const int MAX_SERVER_SCRIPT_TIMEOUT_CIELO_EM_SEG = 300;

        public const int MAX_SERVER_SCRIPT_TIMEOUT_BRASPAG_EM_SEG = 300;

        //' TAMANHO MÁXIMO DO CAMPO ENDEREÇO DEVIDO À RESTRIÇÃO EXISTENTE NA NOTA FISCAL ELETRÔNICA
        public const int MAX_TAMANHO_CAMPO_ENDERECO = 60;

        public const int MAX_TAMANHO_CAMPO_ENDERECO_NUMERO = 60;

        public const int MAX_TAMANHO_CAMPO_ENDERECO_COMPLEMENTO = 60;

        public const int MAX_TAMANHO_CAMPO_ENDERECO_BAIRRO = 60;

        public const int MAX_TAMANHO_CAMPO_ENDERECO_CIDADE = 60;

        //'	ANÁLISE DE ENDEREÇO

        public const int MAX_AN_ENDERECO_QTDE_PEDIDOS_EXIBICAO = 40;

        public const int MAX_AN_ENDERECO_QTDE_PEDIDOS_CADASTRAMENTO = 50;

        //'	FAIXAS DE CEP

        public const int TRANSPORTADORA_SELECAO_AUTO_STATUS_FLAG_N = 0;

        public const int TRANSPORTADORA_SELECAO_AUTO_STATUS_FLAG_S = 1;

        public const int TRANSPORTADORA_SELECAO_AUTO_TIPO_ENDERECO_NENHUM = 0;

        public const int TRANSPORTADORA_SELECAO_AUTO_TIPO_ENDERECO_ENTREGA = 1;

        public const int TRANSPORTADORA_SELECAO_AUTO_TIPO_ENDERECO_CLIENTE = 2;

        //'	MÁXIMA QUANTIDADE DE ITENS EM UM PRODUTO COMPOSTO (EXPORTAÇÃO DE DADOS P/ O E-COMMERCE)

        public const int MAX_EC_PRODUTO_COMPOSTO_ITENS = 10;

        //'	CONSTANTES QUE IDENTIFICAM O NSU GERADO DE REGISTRO NA T_FIN_CONTROLE

        public const string NSU_REL_SEPARACAO_ZONA = "NSU_REL_SEPARACAO_ZONA";//' DEIXOU DE SER USADO A PARTIR DA IMPLANTAÇÃO DA IMPRESSÃO DE ETIQUETAS (SET/2013)

        //' CONSTANTES QUE IDENTIFICAM O NSU NA T_CONTROLE (MÁXIMO 80 CARACTERES)

        public const string NSU_QUADRO_AVISO = "QUADRO_DE_AVISOS";

        public const string NSU_CADASTRO_CLIENTES = "CADASTRO_CLIENTES";

        public const string NSU_PEDIDO = "PEDIDO";

        public const string NSU_PEDIDO_TEMPORARIO = "PEDIDO_TEMPORARIO";

        public const string NSU_ID_ESTOQUE_MOVTO = "ESTOQUE_MOVTO";

        public const string NSU_ID_ESTOQUE = "ESTOQUE";

        public const string NSU_ID_ESTOQUE_TEMP = "ESTOQUE_TEMPORARIO";

        public const string NSU_PEDIDO_PAGAMENTO = "PEDIDO_PAGAMENTO";

        public const string NSU_DESC_SUP_AUTORIZACAO = "DESC_SUP_AUTORIZACAO";

        public const string NSU_PEDIDO_ITEM_DEVOLVIDO = "PEDIDO_ITEM_DEVOLVID";

        public const string NSU_ULTIMA_LIMPEZA_BD = "ULTIMA_LIMPEZA_BD";

        public const string NSU_ORCAMENTO = "ORCAMENTO";

        public const string NSU_ORCAMENTO_TEMPORARIO = "ORCAMENTO_TEMPORARIO";

        public const string NSU_PEDIDO_PAGTO_VISANET = "PEDIDO_PAGTO_VISANET";

        public const string NSU_PEDIDO_PERDA = "PEDIDO_VALOR_PERDA";

        public const string NSU_CADASTRO_PERFIL = "T_PERFIL";

        public const string NSU_CADASTRO_PERFIL_ITEM = "T_PERFIL_ITEM";

        public const string NSU_ORDEM_SERVICO = "T_ORDEM_SERVICO";

        public const string NSU_WMS_REGRA_CD = "T_WMS_REGRA_CD";

        public const string NSU_WMS_REGRA_CD_X_UF = "T_WMS_REGRA_CD_X_UF";

        public const string NSU_WMS_REGRA_CD_X_UF_X_PESSOA = "T_WMS_REGRA_CD_X_UF_X_PESSOA";

        public const string NSU_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD = "T_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD";

        public const string ID_PARAM_CAD_VL_APROV_AUTO_ANALISE_CREDITO = "VL_APR_AUTO_AN_CRED";//' NESTE CASO, O REGISTRO É USADO P/ ARMAZENAR UM PARÂMETRO E NÃO P/ GERAR UM NSU!

        public const string ID_PARAM_PERC_LIMITE_RA_SEM_DESAGIO = "PercLimRASemDesagio";//' NESTE CASO, O REGISTRO É USADO P/ ARMAZENAR UM PARÂMETRO E NÃO P/ GERAR UM NSU!

        public const string ID_PARAM_PERC_MAX_RT = "PercMaxRT";//' NESTE CASO, O REGISTRO É USADO P/ ARMAZENAR UM PARÂMETRO E NÃO P/ GERAR UM NSU!

        public const string ID_PARAM_MAX_DIAS_DT_INICIAL_FILTRO_PERIODO = "MaxDiasDtInicFiltro";//' NESTE CASO, O REGISTRO É USADO P/ ARMAZENAR UM PARÂMETRO E NÃO P/ GERAR UM NSU!

        public const string ID_PARAM_VlMaxSemAplicarBloqDescMaxSemZerarRT = "VlMaxSemAplicarBloqDescMaxSemZerarRT";//' NESTE CASO, O REGISTRO É USADO P/ ARMAZENAR UM PARÂMETRO E NÃO P/ GERAR UM NSU!

        public const string ID_PARAM_VlLimiteMensalIndicadorParaCadastroFeitoNaLoja = "VlLimiteMensalIndicadorParaCadastroFeitoNaLoja";//' NESTE CASO, O REGISTRO É USADO P/ ARMAZENAR UM PARÂMETRO E NÃO P/ GERAR UM NSU!

        public const string ID_PARAM_PercDesagioRAIndicadorParaCadastroFeitoNaLoja = "PercDesagioRAIndicadorParaCadastroFeitoNaLoja";//' NESTE CASO, O REGISTRO É USADO P/ ARMAZENAR UM PARÂMETRO E NÃO P/ GERAR UM NSU!

        public const string ID_PARAM_PercVlPedidoLimiteRA = "PercVlPedidoLimiteRA";//' NESTE CASO, O REGISTRO É USADO P/ ARMAZENAR UM PARÂMETRO E NÃO P/ GERAR UM NSU!

        public const string ID_XLOCK_SYNC_PEDIDO = "XLOCK_SYNC_PEDIDO"; //   ' NESTE CASO, O REGISTRO É USADO PARA SINCRONIZAR (SERIALIZAR) A OPERAÇÃO E EVITAR ACESSO CONCORRENTE

        public const string ID_XLOCK_SYNC_ORCAMENTO = "XLOCK_SYNC_ORCAMENTO"; //   ' NESTE CASO, O REGISTRO É USADO PARA SINCRONIZAR (SERIALIZAR) A OPERAÇÃO E EVITAR ACESSO CONCORRENTE

        public const string ID_XLOCK_SYNC_CLIENTE = "XLOCK_SYNC_CLIENTE"; //   ' NESTE CASO, O REGISTRO É USADO PARA SINCRONIZAR (SERIALIZAR) A OPERAÇÃO E EVITAR ACESSO CONCORRENTE

        public const string ID_XLOCK_SYNC_ORCAMENTISTA_E_INDICADOR = "XLOCK_SYNC_ORCAMENTISTA_E_INDICADOR"; //   ' NESTE CASO, O REGISTRO É USADO PARA SINCRONIZAR (SERIALIZAR) A OPERAÇÃO E EVITAR ACESSO CONCORRENTE


        //'	CONSTANTES QUE IDENTIFICAM PARÂMETROS ARMAZENADOS NA TABELA "t_PARAMETRO";

        public const string ID_PARAMETRO_PercMaxComissaoEDesconto_Nivel2_MeiosPagto = "PercMaxComissaoEDesconto_Nivel2_MeiosPagto";

        public const string ID_PARAMETRO_NF_FlagOperacaoTriangular = "NF_FlagOperacaoTriangular";

        public const string ID_PARAMETRO_Flag_Orcamento_ConsisteDisponibilidadeEstoqueGlobal = "Flag_Orcamento_ConsisteDisponibilidadeEstoqueGlobal";

        public const string ID_PARAMETRO_Flag_Pedido_MemorizacaoCompletaEnderecos = "Flag_Pedido_MemorizacaoCompletaEnderecos";

        public const string ID_PARAMETRO_MagentoPedidoComIndicadorListaLojaErp = "MagentoPedidoComIndicadorListaLojaErp";

        public const string ID_PARAMETRO_EmailDestinatarioAlertaEdicaoCadastroClienteComPedidoCreditoOkEntregaPendente = "EmailDestinatarioAlertaEdicaoCadastroClienteComPedidoCreditoOkEntregaPendente";


        //' CONSTANTES PARA USAR COM O BANCO DE DADOS

        public const string BD_DATA_NULA = "DEC 30 1899";

        public const string BD_CURINGA_TODOS = "%";

        public const string SQL_COLLATE_CASE_ACCENT = " COLLATE Latin1_General_CI_AI";


        public const int TAM_MIN_FABRICANTE = 3;

        public const int TAM_MIN_LOJA = 2;

        public const int TAM_MIN_GRUPO_LOJAS = 2;

        public const int TAM_MIN_MIDIA = 3;

        public const int TAM_MIN_NUM_PEDIDO = 6;//' SOMENTE PARTE NUMÉRICA DO NÚMERO DO PEDIDO

        public const int TAM_MIN_ID_PEDIDO = 7;//' PARTE NUMÉRICA DO NÚMERO DO PEDIDO + LETRA REFERENTE AO ANO

        public const int TAM_MAX_ID_PEDIDO = 9;//' PARTE NUMÉRICA DO NÚMERO DO PEDIDO + LETRA REFERENTE AO ANO + SEPARADOR PEDIDO-FILHOTE + SUFIXO PEDIDO-FILHOTE

        public const int TAM_MIN_PRODUTO = 6;

        public const int TAM_MAX_NSU = 12;

        public const int TAM_MIN_NUM_ORCAMENTO = 6;//' SOMENTE PARTE NUMÉRICA DO NÚMERO DO ORÇAMENTO

        public const int TAM_MIN_ID_ORCAMENTO = 7;//' PARTE NUMÉRICA DO NÚMERO DO ORÇAMENTO + LETRA (SUFIXO) QUE IDENTIFICA COMO ORÇAMENTO

        public const int TAM_MAX_ID_ORCAMENTO = 7;//' PARTE NUMÉRICA DO NÚMERO DO ORÇAMENTO + LETRA (SUFIXO) QUE IDENTIFICA COMO ORÇAMENTO

        public const int TIMEOUT_DESCONTO_EM_MIN = 30;

        //'   CONSTANTES PRAZO CANCELAMENTO AUTOMATICO DE PEDIDO
        public const int PRAZO_CANCEL_AUTO_PEDIDO_PENDENTE_CARTAO_CREDITO = 7;
        public const int PRAZO_CANCEL_AUTO_PEDIDO_CREDITO_OK_AGUARDANDO_DEPOSITO = 7;
        public const int PRAZO_CANCEL_AUTO_PEDIDO_PENDENTE_VENDAS = 10;

        //'	MÓDULO FINANCEIRO

        public const int TAM_PLANO_CONTAS__EMPRESA = 2;

        public const int TAM_PLANO_CONTAS__GRUPO = 2;

        public const int TAM_PLANO_CONTAS__CONTA = 4;


        public const string COD_FIN_NATUREZA__CREDITO = "C";

        public const string COD_FIN_NATUREZA__DEBITO = "D";


        public const int COD_FIN_ST_ATIVO__INATIVO = 0;

        public const int COD_FIN_ST_ATIVO__ATIVO = 1;


        public const int COD_FIN_ST_SISTEMA__NAO = 0;

        public const int COD_FIN_ST_SISTEMA__SIM = 1;


        //'   SEPARA SUFIXO DO PEDIDO FILHOTE

        public const string COD_SEPARADOR_FILHOTE = "-";


        public const int COD_NEGATIVO_UM = -1;

        public const string SIMBOLO_MONETARIO = "R$";


        public const string SESSION_CLIPBOARD = "ClipBoard";

        //'	NÚMERO MÁXIMO DE VENDEDORES DA EMPRESA DO PARCEIRO DENTRO DO CADASTRO

        public const int CADASTRO_INDICADOR_QTDE_MAX_VENDEDORES = 5;

        //'	NÚMERO DE LINHAS DO CAMPO "OBS I" DO PEDIDO

        public const int MAX_LINHAS_OBS1 = 5;

        //'   NÚMERO DE LINHAS DO CAMPO "TEXTO CONSTAR NF" DO PEDIDO
        public const int MAX_LINHAS_NF_TEXTO_CONSTAR = 2;
        public const int MAX_TAM_NF_TEXTO = 800;

        //'	NÚMERO DE LINHAS DO CAMPO "MENSAGEM" AO CADASTRAR NOVA MENSAGEM NO BLOCO DE NOTAS DO PEDIDO

        public const int MAX_LINHAS_MENSAGEM_BLOCO_NOTAS = 5;

        public const int MAX_LINHAS_MENSAGEM_BLOCO_NOTAS_EM_ITEM_DEVOLVIDO = 5;

        //'	NÚMERO DE LINHAS DO CAMPO "DESCRIÇÃO DA OCORRÊNCIA" AO CADASTRAR NOVA OCORRÊNCIA NO PEDIDO

        public const int MAX_LINHAS_DESCRICAO_OCORRENCIAS_EM_PEDIDOS = 5;

        //'	NÚMERO DE LINHAS DO CAMPO "DESCRIÇÃO DO CHAMADO" AO CADASTRAR NOVO CHAMADO NO PEDIDO

        public const int MAX_LINHAS_DESCRICAO_CHAMADOS_EM_PEDIDOS = 7;

        //'	NÚMERO DE LINHAS DO CAMPO MENSAGEM AO CADASTRAR UMA NOVA MENSAGEM REFERENTE A UMA OCORRÊNCIA NO PEDIDO

        public const int MAX_LINHAS_MENSAGEM_OCORRENCIAS_EM_PEDIDOS = 5;

        //'	NÚMERO DE LINHAS DO CAMPO MENSAGEM AO CADASTRAR UMA NOVA MENSAGEM REFERENTE A UMA OCORRÊNCIA NO PEDIDO

        public const int MAX_LINHAS_MENSAGEM_CHAMADOS_EM_PEDIDOS = 5;

        //'	NÚMERO DE LINHAS DO CAMPO "FORMA DE PAGAMENTO" DO PEDIDO
        public const int MAX_LINHAS_FORMA_PAGTO = 5;
        public const int MAX_TAM_FORMA_PAGTO = 250;

        //'	NÚMERO DE LINHAS DOS CAMPOS MULTI-LINHAS DA ORDEM DE SERVIÇO	

        public const int MAX_LINHAS_OS_OBS_PECAS_NECESSARIAS = 8;

        public const int MAX_LINHAS_OS_OBS_PROBLEMA = 2;

        public const int MAX_LINHAS_OS_ENDERECO = 2;

        //'   NÚMERO DE LINHAS DOS CAMPOS DE DESCRIÇÃO/OBSERVAÇÕES DA DEVOLUÇÃO
        public const int MAX_LINHAS_DESCRICAO_OBSERVACAO_DEVOLUCAO = 5;

        //'	TAMANHO MÁXIMO DE CAMPOS DA ORDEM DE SERVIÇO

        public const int MAX_TAM_OS_OBS_PROBLEMA = 80;

        public const int MAX_TAM_OS_OBS_PECAS_NECESSARIAS = 400;

        //'	TAMANHO MÁXIMO DE CAMPOS NO CADASTRO DE ORÇAMENTISTA/INDICADOR

        public const int MAX_LINHAS_OBS_ORCAMENTISTA_INDICADOR = 6;

        public const int MAX_TAM_OBS_ORCAMENTISTA_INDICADOR = 500;

        //'	TAMANHO MÁXIMO DE CAMPOS DA TABELA T_ESTOQUE

        public const int MAX_TAM_T_ESTOQUE_CAMPO_OBS = 500;

        public const int MAX_LINHAS_ESTOQUE_OBS = 5;

        //'	TAMANHO MÁXIMO DE CADA MENSAGEM DO BLOCO DE NOTAS

        public const int MAX_TAM_MENSAGEM_BLOCO_NOTAS = 400;

        //'	TAMANHO MÁXIMO DE CADA MENSAGEM DO BLOCO DE NOTAS DE RELACIONAMENTO COM PARCEIRO

        public const int MAX_TAM_MENSAGEM_BLOCO_NOTAS_RELACIONAMENTO = 1600;

        //'	TAMANHO MÁXIMO DE CADA MENSAGEM DO BLOCO DE NOTAS EM ITENS DEVOLVIDOS

        public const int MAX_TAM_MENSAGEM_BLOCO_NOTAS_EM_ITEM_DEVOLVIDO = 400;

        //'	TAMANHO MÁXIMO P/ TEXTO DE DESCRIÇÃO E FINALIZAÇÃO DA OCORRÊNCIA

        public const int MAX_TAM_DESCRICAO_OCORRENCIAS_EM_PEDIDOS = 240;
        //'	TAMANHO MÁXIMO P/ TEXTO DE RÉPLICA DA OCORRÊNCIA

        public const int MAX_TAM_MENSAGEM_OCORRENCIAS_EM_PEDIDOS = 1200;

        //'   TAMANHO MÁXIMO P/ TEXTO DE DESCRIÇÃO DO CHAMADO EM PEDIDOS
        public const int MAX_TAM_DESCRICAO_CHAMADO_EM_PEDIDOS = 4000;
        //'	TAMANHO MÁXIMO P/ TEXTO DE RÉPLICA Do CHAMADO

        public const int MAX_TAM_MENSAGEM_CHAMADOS_EM_PEDIDOS = 2000;

        //'	TAMANHO MÁXIMO DO CAMPO DE OBSERVAÇÕES

        public const int MAX_TAM_T_PEDIDO_PAGTO_CIELO__TRATADO_MANUAL_OBS = 400;

        public const int MAX_TAM_T_PEDIDO_PAGTO_BRASPAG__TRATADO_MANUAL_OBS = 400;

        public const int MAX_TAM_T_PEDIDO_PAGTO_BRASPAG__AF_REVIEW_COMENTARIO = 400;

        //'	CÓDIGOS REFERENTES A NOTAS FISCAIS

        public const string COD_NF_IDDEST_OPERACAO_INTERNA = "1";

        public const string COD_NF_IDDEST_OPERACAO_INTERESTADUAL = "2";

        public const string COD_NF_IDDEST_OPERACAO_EXTERIOR = "3";

        //' CÓDIGOS PARA REGISTRO DE OPERAÇÕES NO LOG (MÁXIMO 20 CARACTERES)

        public const string OP_LOG_SENHA_ALTERACAO = "SENHA ALTERAÇÃO";


        public const string OP_LOG_DESC_SUP_AUTORIZACAO = "DESC SUP AUTORIZACAO";

        public const string OP_LOG_DESC_SUP_CANCELA = "DESC SUP CANCELA";


        public const string OP_LOG_DESC_SUP_AUTORIZACAO_NA_LOJA = "LojaDescSupAutoriza";

        public const string OP_LOG_DESC_SUP_CANCELA_NA_LOJA = "LojaDescSupCancela";


        public const string OP_LOG_USUARIO_EXCLUSAO = "USUÁRIO EXCLUSÃO";

        public const string OP_LOG_USUARIO_ALTERACAO = "USUÁRIO EDIÇÃO";

        public const string OP_LOG_USUARIO_INCLUSAO = "USUÁRIO INCLUSÃO";


        public const string OP_LOG_FABRICANTE_EXCLUSAO = "FABRICANTE EXCLUSÃO";

        public const string OP_LOG_FABRICANTE_ALTERACAO = "FABRICANTE EDIÇÃO";

        public const string OP_LOG_FABRICANTE_INCLUSAO = "FABRICANTE INCLUSÃO";


        public const string OP_LOG_PRODUTO_COMPOSTO_EXCLUSAO = "PROD COMPOSTO EXCL";

        public const string OP_LOG_PRODUTO_COMPOSTO_ALTERACAO = "PROD COMPOSTO EDIÇÃO";

        public const string OP_LOG_PRODUTO_COMPOSTO_INCLUSAO = "PROD COMPOSTO INCL";


        public const string OP_LOG_TRANSPORTADORA_EXCLUSAO = "TRANSPORT EXCLUSÃO";

        public const string OP_LOG_TRANSPORTADORA_ALTERACAO = "TRANSPORT EDIÇÃO";

        public const string OP_LOG_TRANSPORTADORA_INCLUSAO = "TRANSPORT INCLUSÃO";


        public const string OP_LOG_LOJA_EXCLUSAO = "LOJA EXCLUSÃO";

        public const string OP_LOG_LOJA_ALTERACAO = "LOJA EDIÇÃO";

        public const string OP_LOG_LOJA_INCLUSAO = "LOJA INCLUSÃO";


        public const string OP_LOG_GRUPO_LOJAS_EXCLUSAO = "GRUPO LOJAS EXCLUSÃO";

        public const string OP_LOG_GRUPO_LOJAS_ALTERACAO = "GRUPO LOJAS EDIÇÃO";

        public const string OP_LOG_GRUPO_LOJAS_INCLUSAO = "GRUPO LOJAS INCLUSÃO";


        public const string OP_LOG_MIDIA_EXCLUSAO = "MÍDIA EXCLUSÃO";

        public const string OP_LOG_MIDIA_ALTERACAO = "MÍDIA EDIÇÃO";

        public const string OP_LOG_MIDIA_INCLUSAO = "MÍDIA INCLUSÃO";


        public const string OP_LOG_AVISO_EXCLUSAO = "AVISO EXCLUSÃO";

        public const string OP_LOG_AVISO_ALTERACAO = "AVISO EDIÇÃO";

        public const string OP_LOG_AVISO_INCLUSAO = "AVISO INCLUSÃO";

        public const string OP_LOG_AVISO_LIDO = "AVISO LEITURA";

        public const string OP_LOG_AVISO_EXCLUSAO_ANTIGOS = "AVISO EXCL ANTIGOS";


        public const string OP_LOG_CLIENTE_EXCLUSAO = "CLIENTE EXCLUSÃO";

        public const string OP_LOG_CLIENTE_ALTERACAO = "CLIENTE EDIÇÃO";

        public const string OP_LOG_CLIENTE_INCLUSAO = "CLIENTE INCLUSÃO";


        public const string OP_LOG_PEDIDO_NOVO = "PEDIDO INCLUSÃO";

        public const string OP_LOG_PEDIDO_CANCELA = "PEDIDO EXCLUSÃO";

        public const string OP_LOG_PEDIDO_ALTERACAO = "PEDIDO EDIÇÃO";

        public const string OP_LOG_PEDIDO_SPLIT = "PEDIDO SPLIT";

        public const string OP_LOG_PEDIDO_PAGTO_PARCIAL = "PEDIDO PAGTO PARCIAL";

        public const string OP_LOG_PEDIDO_PAGTO_QUITACAO = "PEDIDO PAGTO QUITADO";

        public const string OP_LOG_PEDIDO_SEPARACAO = "PEDIDO SEPARAÇÃO";

        public const string OP_LOG_PEDIDO_ENTREGUE = "PEDIDO ENTREGUE";

        public const string OP_LOG_PEDIDO_DEVOLUCAO = "PEDIDO DEVOLUÇÃO";

        public const string OP_LOG_PEDIDO_PAGTO_VISANET = "PEDIDO PAGTO VISANET";

        public const string OP_LOG_PEDIDO_CANCEL_VISANET = "PEDIDO CANCL VISANET";

        public const string OP_LOG_PEDIDO_PERDA = "PEDIDO VL PERDA";

        public const string OP_LOG_ORCAMENTO_VIROU_PEDIDO = "ORÇAM VIROU PEDIDO";

        public const string OP_LOG_PEDIDO_RECEBIDO = "PEDIDO RECEBIDO";


        public const string OP_LOG_PEDIDO_NFE_EMISSAO_SOLICITADA = "PedNFeEmissaoSolic";


        public const string OP_LOG_PEDIDO_BLOCO_NOTAS_INCLUSAO = "PED BLOCO NOTAS INCL";

        public const string OP_LOG_PEDIDO_OCORRENCIA_INCLUSAO = "PED OCORRENCIA INCL";

        public const string OP_LOG_PEDIDO_OCORRENCIA_MENSAGEM_INCLUSAO = "PED OCORR MSG INCL";

        public const string OP_LOG_PEDIDO_OCORRENCIA_FINALIZACAO = "PED OCORRENCIA FINAL";

        public const string OP_LOG_PEDIDO_ITEM_DEVOLVIDO_BLOCO_NOTAS_INCLUSAO = "ITMDEV BL NOTAS INCL";
        public const string OP_LOG_PEDIDO_CHAMADO_INCLUSAO = "PED CHAMADO INCL";

        public const string OP_LOG_PEDIDO_CHAMADO_MENSAGEM_INCLUSAO = "PED CHAMD MSG INCL";
        public const string OP_LOG_PEDIDO_CHAMADO_FINALIZACAO = "PED CHAMADO FINAL";
        public const string OP_LOG_PEDIDO_DEVOLUCAO_MENSAGEM_INCLUSAO = "PED DEVOL MSG INCL";


        public const string OP_LOG_ESTOQUE_ENTRADA = "ESTOQUE INCLUSÃO";

        public const string OP_LOG_ESTOQUE_REMOVE = "ESTOQUE EXCLUSÃO";

        public const string OP_LOG_ESTOQUE_ALTERACAO = "ESTOQUE EDIÇÃO";

        public const string OP_LOG_ESTOQUE_CONVERSAO_KIT = "ESTOQUE CONVERTE KIT";

        public const string OP_LOG_ESTOQUE_PROCESSA_SP = "ESTOQUE PROCESSA SP";

        public const string OP_LOG_ESTOQUE_TRANSFERENCIA = "ESTOQUE TRANSFERE";

        public const string OP_LOG_ESTOQUE_TRANSF_PEDIDO = "ESTOQUE TRANSF PED";


        public const string OP_LOG_ORCAMENTO_NOVO = "ORÇAMENTO INCLUSÃO";

        public const string OP_LOG_ORCAMENTO_ALTERACAO = "ORÇAMENTO EDIÇÃO";

        public const string OP_LOG_ORCAMENTO_CANCELA = "ORÇAMENTO EXCLUSÃO";


        public const string OP_LOG_VISANET_OPCOES_PAGTO_ALTERACAO = "VISANET PRAZO EDIÇÃO";

        public const string OP_LOG_VISANET_OPCOES_PAGTO_INCLUSAO = "VISANET PRAZO INCL";


        public const string OP_LOG_PERFIL_EXCLUSAO = "PERFIL EXCLUSÃO";

        public const string OP_LOG_PERFIL_ALTERACAO = "PERFIL EDIÇÃO";

        public const string OP_LOG_PERFIL_INCLUSAO = "PERFIL INCLUSÃO";


        public const string OP_LOG_MULTI_CD_REGRA_EXCLUSAO = "MULTI CD REGRA EXCL";

        public const string OP_LOG_MULTI_CD_REGRA_ALTERACAO = "MULTI CD REGRA EDIC";

        public const string OP_LOG_MULTI_CD_REGRA_INCLUSAO = "MULTI CD REGRA INCL";

        public const string OP_LOG_MULTI_CD_REGRA_EXCLUSAO_PRODUTOS_ASSOCIADOS = "MultiCdRegraExcProd";


        public const string OP_LOG_ANALISE_CREDITO = "ANALISE CREDITO";

        public const string OP_LOG_ANALISE_CREDITO_OK_AGUARDANDO_DEPOSITO = "AN CRED OK: DEPOSITO";

        public const string OP_LOG_ANALISE_CREDITO_OK_DEPOSITO_AGUARDANDO_DESBLOQUEIO = "AN CRED OK: DEP DESB";

        public const string OP_LOG_ANALISE_CREDITO_OK_PENDENTE_VENDAS = "AN CRED OK: PEND VEN";

        public const string OP_LOG_CAD_VL_APROV_AUTO_ANALISE_CREDITO = "CAD VL AN CRED AUTO";

        public const string OP_LOG_CAD_PERC_LIMITE_RA_SEM_DESAGIO = "PERC LIM RA S/ DESAG";


        public const string OP_LOG_ORCAMENTISTA_E_INDICADOR_EXCLUSAO = "ORÇAM INDIC EXCLUSÃO";

        public const string OP_LOG_ORCAMENTISTA_E_INDICADOR_ALTERACAO = "ORÇAM INDIC EDIÇÃO";

        public const string OP_LOG_ORCAMENTISTA_E_INDICADOR_INCLUSAO = "ORÇAM INDIC INCLUSÃO";


        public const string OP_LOG_ORCAMENTISTA_E_INDICADOR_TABELA_DESCONTO_INCLUSAO = "INDIC DESCONTO INCL";
        public const string OP_LOG_ORCAMENTISTA_E_INDICADOR_TABELA_DESCONTO_EXCL = "INDIC DESCONTO EXCL";
        public const string OP_LOG_ORCAMENTISTA_E_INDICADOR_TABELA_DESCONTO_EDICAO = "INDIC DESCONTO EDIÇ";

        public const string OP_LOG_ORCAMENTISTA_E_INDICADOR_CONTATOS__INCLUSAO = "INDIC CONTATO INCL";
        public const string OP_LOG_ORCAMENTISTA_E_INDICADOR_CONTATOS__EXCLUSAO = "INDIC CONTATO EXCL";
        public const string OP_LOG_ORCAMENTISTA_E_INDICADOR_CONTATOS__EDICAO = "INDIC CONTATO EDIÇ";


        public const string OP_LOG_CEP_EXCLUSAO = "CEP EXCLUSÃO";

        public const string OP_LOG_CEP_ALTERACAO = "CEP EDIÇÃO";

        public const string OP_LOG_CEP_INCLUSAO = "CEP INCLUSÃO";


        public const string OP_LOG_ORDEM_SERVICO_INCLUSAO = "O.S. INCLUSÃO";

        public const string OP_LOG_ORDEM_SERVICO_ALTERACAO = "O.S. ALTERAÇÃO";

        public const string OP_LOG_ORDEM_SERVICO_ENCERRA = "O.S. ENCERRA";

        public const string OP_LOG_ORDEM_SERVICO_CANCELA = "O.S. EXCLUSÃO";


        public const string OP_LOG_MENSAGEM_ALERTA_EXCLUSAO = "MSG ALERTA EXCLUSÃO";

        public const string OP_LOG_MENSAGEM_ALERTA_ALTERACAO = "MSG ALERTA EDIÇÃO";

        public const string OP_LOG_MENSAGEM_ALERTA_INCLUSAO = "MSG ALERTA INCLUSÃO";


        public const string OP_LOG_ANOTA_FRETE_PEDIDO = "ANOTA FRETE PEDIDO";


        public const string OP_LOG_TABELA_COMISSAO_VENDEDOR = "TabComissaoVendedor";

        public const string OP_LOG_TABELA_CUSTO_FINANCEIRO_FORNECEDOR = "TabCustoFinancFornec";


        public const string OP_LOG_PLANO_CONTAS_EMPRESA_EXCLUSAO = "FinPlContasEmprExcl";

        public const string OP_LOG_PLANO_CONTAS_EMPRESA_INCLUSAO = "FinPlContasEmprIncl";

        public const string OP_LOG_PLANO_CONTAS_EMPRESA_ALTERACAO = "FinPlContasEmprEdit";

        public const string OP_LOG_PLANO_CONTAS_GRUPO_EXCLUSAO = "FinPlContasGrupoExcl";

        public const string OP_LOG_PLANO_CONTAS_GRUPO_INCLUSAO = "FinPlContasGrupoIncl";

        public const string OP_LOG_PLANO_CONTAS_GRUPO_ALTERACAO = "FinPlContasGrupoEdit";

        public const string OP_LOG_PLANO_CONTAS_CONTA_EXCLUSAO = "FinPlContasContaExcl";

        public const string OP_LOG_PLANO_CONTAS_CONTA_INCLUSAO = "FinPlContasContaIncl";

        public const string OP_LOG_PLANO_CONTAS_CONTA_ALTERACAO = "FinPlContasContaEdit";

        public const string OP_LOG_CONTA_CORRENTE_EXCLUSAO = "FinCtaCorrenteExcl";

        public const string OP_LOG_CONTA_CORRENTE_INCLUSAO = "FinCtaCorrenteIncl";

        public const string OP_LOG_CONTA_CORRENTE_ALTERACAO = "FinCtaCorrenteEdit";

        public const string OP_LOG_UNIDADE_NEGOCIO_EXCLUSAO = "FinUnNegocioExcl";

        public const string OP_LOG_UNIDADE_NEGOCIO_INCLUSAO = "FinUnNegocioIncl";

        public const string OP_LOG_UNIDADE_NEGOCIO_ALTERACAO = "FinUnNegocioEdit";

        public const string OP_LOG_UNIDADE_NEGOCIO_RATEIO_EXCLUSAO = "FinUnNegocRateioExcl";

        public const string OP_LOG_UNIDADE_NEGOCIO_RATEIO_INCLUSAO = "FinUnNegocRateioIncl";

        public const string OP_LOG_UNIDADE_NEGOCIO_RATEIO_ALTERACAO = "FinUnNegocRateioEdit";


        public const string OP_LOG_BOLETO_CEDENTE_PARAMETROS_EXCLUSAO = "FinBolCedParamExcl";

        public const string OP_LOG_BOLETO_CEDENTE_PARAMETROS_INCLUSAO = "FinBolCedParamIncl";

        public const string OP_LOG_BOLETO_CEDENTE_PARAMETROS_ALTERACAO = "FinBolCedParamEdit";


        public const string OP_LOG_EQUIPE_VENDAS_EXCLUSAO = "EquipeVendasExcl";

        public const string OP_LOG_EQUIPE_VENDAS_INCLUSAO = "EquipeVendasIncl";

        public const string OP_LOG_EQUIPE_VENDAS_ALTERACAO = "EquipeVendasEdit";


        public const string OP_LOG_ROMANEIO_ENTREGA = "Romaneio Entrega";

        public const string OP_LOG_FAROL_RESUMIDO = "Farol Resumido";

        public const string OP_LOG_FAROL_PRODUTO_COMPRADO_GRAVA_DADOS = "FarolProdComprGrava";


        public const string OP_LOG_PERC_MAX_DESC_SEM_ZERAR_RT = "MaxDescSemZerarRT";

        public const string OP_LOG_PERC_MAX_SENHA_DESC_CADASTRADO_NA_LOJA = "MaxSenhaDescCadLoja";

        public const string OP_LOG_PERC_MAX_COMISSAO_E_DESC_POR_LOJA = "MaxComisEDescPorLoja";


        public const string OP_LOG_REL_PRODUTO_DEPOSITO_ZONA_GRAVA_DADOS = "RelProdDepZonaGrava";

        public const string OP_LOG_REL_SEPARACAO_ZONA = "RelSeparacaoZona";


        public const string OP_LOG_PEDIDO_PAGTO_CIELO = "PEDIDO PAGTO CIELO";

        public const string OP_LOG_PEDIDO_PAGTO_CONTABILIZADO_CIELO = "PedPagtoContabCielo";


        public const string OP_LOG_PEDIDO_PAGTO_BRASPAG = "PEDIDO PAGTO BRASPAG";

        public const string OP_LOG_PEDIDO_PAGTO_CONTABILIZADO_BRASPAG = "PedPagtoContabBraspg";

        public const string OP_LOG_BRASPAG_PEDIDO_NSU_GERADO = "BraspagPedNsuGerado";


        public const string OP_LOG_PEDIDO_TRX_BRASPAG = "TRX BRASPAG";

        public const string OP_LOG_PEDIDO_TRX_BRASPAG_FALHA = "TRX BRASPAG ERR";

        public const string OP_LOG_PEDIDO_PAGTO_CONTABILIZADO_BRASPAG_CLEARSALE = "PedPagtoContabBpCs";


        public const string OP_LOG_CAD_INDICADOR_OPCAO_FORMA_COMO_CONHECEU_EXCLUSAO = "IndOpcFormaConhExcl";

        public const string OP_LOG_CAD_INDICADOR_OPCAO_FORMA_COMO_CONHECEU_ALTERACAO = "IndOpcFormaConhEdit";

        public const string OP_LOG_CAD_INDICADOR_OPCAO_FORMA_COMO_CONHECEU_INCLUSAO = "IndOpcFormaConhIncl";


        public const string OP_LOG_PAGTO_CIELO_EM_ANDAMENTO_MARCAR_COMO_TRATADO = "CieloEmAndamMarcTrat";


        public const string OP_LOG_TRANSACOES_BRASPAG_MARCAR_COMO_JA_TRATADO = "BraspagTrMarcTrat";

        public const string OP_LOG_TRANSACOES_BRASPAG_OBS_EDITADO = "BraspagTrObsEdit";


        public const string OP_LOG_TRANSACOES_BRASPAG_CLEARSALE_MARCAR_COMO_JA_TRATADO = "BraspagCSTrMarcTrat";

        public const string OP_LOG_TRANSACOES_BRASPAG_CLEARSALE_OBS_EDITADO = "BraspagCSTrObsEdit";


        public const string OP_LOG_ETQWMS_ETIQUETA_ALTERACAO = "EtqWmsEtiquetaEdit";


        public const string OP_LOG_EC_PRODUTO_COMPOSTO_INCLUSAO = "EC-ProdCompostoInc";

        public const string OP_LOG_EC_PRODUTO_COMPOSTO_ALTERACAO = "EC-ProdCompostoAlt";

        public const string OP_LOG_EC_PRODUTO_COMPOSTO_EXCLUSAO = "EC-ProdCompostoExc";


        public const string OP_LOG_EC_EXPORTACAO_PRE_LISTA_INCLUSAO = "EC-ExpPreListaInc";
        public const string OP_LOG_EC_EXPORTACAO_PRE_LISTA_ALTERACAO = "EC-ExpPreListaAlt";
        public const string OP_LOG_EC_EXPORTACAO_PRE_LISTA_EXCLUSAO = "EC-ExpPreListaExc";


        public const string OP_LOG_SPC_CLIENTE_NEGATIVADO_ALTERACAO = "SPCNegativacao";


        public const string OP_LOG_INDICADOR_BLOCO_NOTAS_INCLUSAO = "IND BLOCO NOTAS INCL";


        public const string OP_LOG_REL_COMISSAO_INDICADORES_PAGAMENTO = "GRAVA REL COM IND";
        public const string OP_LOG_REL_COMISSAO_INDICADORES_GRAVADADOS = "GRAVA PROC AUTO";


        public const string OP_LOG_NFE_CTRL_IMPOSTOS = "NFE CTRL IMPOSTOS";

        public const string OP_LOG_PRODUTO_REGRA_CD_INCLUSAO = "PROD INCL REGRA CD";
        public const string OP_LOG_PRODUTO_REGRA_CD_ALTERACAO = "PROD ALT REGRA CD";
        public const string OP_LOG_PRODUTO_REGRA_CD_EXCLUSAO = "PROD EXCL REGRA CD";

        //' CÓDIGOS DE OPERAÇÕES (CONTROLE DE ACESSO)
        //' =========================================
        //'	AGRUPAMENTO POR MÓDULO

        public const string COD_OP_MODULO_CENTRAL = "CENTR";

        public const string COD_OP_MODULO_LOJA = "LOJA";

        //' CENTRAL

        public const int OP_CEN_ACESSO_TODAS_LOJAS = 10100;

        public const int OP_CEN_CONSULTA_PEDIDOS_ANTERIORES_CLIENTE = 10200;

        public const int OP_CEN_CONSULTA_PEDIDO = 10300;

        public const int OP_CEN_CONSULTA_ORCAMENTO = 10400;

        public const int OP_CEN_EDITA_PEDIDO = 10500;

        public const int OP_CEN_EDITA_ITEM_DO_PEDIDO = 10600;

        public const int OP_CEN_EDITA_ORCAMENTO = 10700;

        public const int OP_CEN_EDITA_ITEM_DO_ORCAMENTO = 10800;

        public const int OP_CEN_PAGTO_PARCIAL = 10900;

        public const int OP_CEN_PAGTO_QUITACAO = 11000;

        public const int OP_CEN_CADASTRA_PERDA = 11100;

        public const int OP_CEN_CADASTRA_SENHA_DESCONTO = 11200;

        public const int OP_CEN_SEPARACAO_MERCADORIAS = 11300;

        public const int OP_CEN_ENTREGA_MERCADORIAS = 11400;

        public const int OP_CEN_AGENDAMENTO_ENTREGA = 11500;

        public const int OP_CEN_CADASTRO_PERFIL = 11600;

        public const int OP_CEN_CADASTRO_USUARIOS = 11700;

        public const int OP_CEN_CADASTRO_LOJAS = 11800;

        public const int OP_CEN_CADASTRO_GRUPO_LOJAS = 11900;

        public const int OP_CEN_CADASTRO_FABRICANTES = 12000;

        public const int OP_CEN_CADASTRO_TRANSPORTADORAS = 12100;

        public const int OP_CEN_CADASTRO_VEICULOS_MIDIA = 12200;

        public const int OP_CEN_CADASTRO_AVISOS = 12300;

        public const int OP_CEN_OPCOES_PAGTO_VISANET = 12400;

        public const int OP_CEN_ENTRADA_MERCADORIAS_ESTOQUE = 12500;

        public const int OP_CEN_ENTRADA_ESPECIAL_ESTOQUE = 12600;

        public const int OP_CEN_EDITA_ENTRADA_ESTOQUE = 12700;

        public const int OP_CEN_CONVERSOR_KITS = 12800;

        public const int OP_CEN_TRANSF_MOV_ESTOQUE_PERFIL_BASICO = 12900;

        public const int OP_CEN_TRANSF_MOV_ESTOQUE_PERFIL_AVANCADO = 12950;

        public const int OP_CEN_TRANSF_ENTRE_PED_PROD_ESTOQUE_VENDIDO = 13000;

        public const int OP_CEN_LER_AVISOS_NAO_LIDOS = 13100;

        public const int OP_CEN_LER_AVISOS_TODOS = 13200;

        public const int OP_CEN_REL_MULTICRITERIO_PEDIDOS_ANALITICO = 13300;

        public const int OP_CEN_REL_MULTICRITERIO_ORCAMENTOS = 13400;

        public const int OP_CEN_REL_PRODUTOS_VENDIDOS = 13500;

        public const int OP_CEN_REL_PEDIDOS_COLOCADOS_NO_MES = 13600;

        public const int OP_CEN_REL_VENDAS_COM_DESCONTO_SUPERIOR = 13700;

        public const int OP_CEN_REL_COMISSAO_VENDEDORES = 13800;

        public const int OP_CEN_REL_FATURAMENTO_VENDEDORES_EXT = 13900;

        public const int OP_CEN_REL_COMISSAO_LOJA_POR_INDICACAO = 14000;

        public const int OP_CEN_REL_ANALISE_PEDIDOS = 14100;

        public const int OP_CEN_REL_SEPARACAO = 14200;

        public const int OP_CEN_REL_ESTOQUE_VISAO_BASICA = 14300;

        public const int OP_CEN_REL_ESTOQUE_VISAO_COMPLETA = 14400;

        public const int OP_CEN_REL_PRODUTOS_PENDENTES = 14500;

        public const int OP_CEN_REL_DEVOLUCAO_PRODUTOS = 14600;

        public const int OP_CEN_REL_DEVOLUCAO_PRODUTOS2 = 23300;

        public const int OP_CEN_REL_PRODUTOS_SPLIT_POSSIVEL = 14700;

        public const int OP_CEN_REL_COMPRAS = 14800;

        public const int OP_CEN_REL_ESTOQUE_VENDA_CRITICO = 14900;

        public const int OP_CEN_REL_MEIO_DIVULGACAO = 15000;

        public const int OP_CEN_REL_LOG_ESTOQUE = 15100;

        public const int OP_CEN_REL_RESUMO_OPERACOES_ENTRE_ESTOQUES = 15150;

        public const int OP_CEN_REL_FATURAMENTO = 15200;

        public const int OP_CEN_REL_VENDAS = 15300;

        public const int OP_CEN_REL_REGISTROS_ENTRADA_ESTOQUE = 15400;

        public const int OP_CEN_REL_ROMANEIO_ENTREGA = 15500;

        public const int OP_CEN_REL_CONTAGEM_ESTOQUE = 15600;

        public const int OP_CEN_EDITA_ANALISE_CREDITO = 15700;

        public const int OP_CEN_CAD_VL_APROV_AUTO_ANALISE_CREDITO = 15800;

        public const int OP_CEN_REL_ANALISE_CREDITO = 15900;

        public const int OP_CEN_CADASTRO_ORCAMENTISTAS_E_INDICADORES = 16000;

        public const int OP_CEN_FLAG_COMISSAO_PAGA = 16100;

        public const int OP_CEN_REL_COMISSAO_INDICADORES = 16200;

        public const int OP_CEN_EDITA_RT_E_RA = 16300;

        public const int OP_CEN_EDITA_PEDIDO_OBS1 = 16400;

        public const int OP_CEN_EDITA_PEDIDO_OBS2 = 16500;

        public const int OP_CEN_EDITA_PEDIDO_FORMA_PAGTO = 16600;

        public const int OP_CEN_CAD_PERC_LIMITE_RA_SEM_DESAGIO = 16700;

        public const int OP_CEN_CAD_PERC_MAX_RT = 16800;

        public const int OP_CEN_CAD_PERC_MAX_DESC_SEM_ZERAR_RT = 16900;

        public const int OP_CEN_EDITA_ORDEM_SERVICO = 17000;

        public const int OP_CEN_CONSULTA_ORDEM_SERVICO = 17100;

        public const int OP_CEN_AUTORIZA_SENHA_DESCONTO = 17200;

        public const int OP_CEN_MODULO_ADM = 17300;

        public const int OP_CEN_FILTRO_MCRIT_PEDIDOS_ENTREGUES_ENTRE = 17400;

        public const int OP_CEN_FILTRO_MCRIT_PEDIDOS_COLOCADOS_ENTRE = 17500;

        public const int OP_CEN_FILTRO_MCRIT_PEDIDOS_ENTREGA_MARC_ENTRE = 17600;

        public const int OP_CEN_RESTRINGE_DT_INICIAL_FILTRO_PERIODO = 17700;

        public const int OP_CEN_CAD_PARAMETROS_GLOBAIS = 17800;

        public const int OP_CEN_REL_GERENCIAL_DE_VENDAS = 17900;

        public const int OP_CEN_REL_SOLICITACAO_COLETAS = 18000;

        public const int OP_CEN_REL_MULTICRITERIO_PEDIDOS_SINTETICO = 18100;

        public const int OP_CEN_CAD_CEP = 18200;

        public const int OP_CEN_PESQUISA_INDICADORES = 18300;

        public const int OP_CEN_ANOTA_TRANSPORTADORA_NO_PEDIDO = 18400;

        public const int OP_CEN_EDITA_PEDIDO_TRANSPORTADORA = 18500;

        public const int OP_CEN_REL_INDICADOR_SEM_AVALIACAO_DESEMPENHO = 18600;

        public const int OP_CEN_REL_CHECAGEM_NOVOS_PARCEIROS = 18700;

        public const int OP_CEN_EDITA_PEDIDO_VALOR_FRETE = 18800;

        public const int OP_CEN_ANOTA_VALOR_FRETE_NO_PEDIDO = 18900;

        public const int OP_CEN_REL_FRETE_ANALITICO = 19000;

        public const int OP_CEN_REL_FRETE_SINTETICO = 19100;

        public const int OP_CEN_REL_FATURAMENTO2 = 19200;

        public const int OP_CEN_REL_ESTOQUE2 = 19300;

        public const int OP_CEN_REL_COMPRAS2 = 19400;

        public const int OP_CEN_REL_ESTOQUE_VISAO_BASICA_CMVPV = 19600;

        public const int OP_CEN_REL_ESTOQUE_VISAO_COMPLETA_CMVPV = 19700;

        public const int OP_CEN_REL_FATURAMENTO_CMVPV = 19800;

        public const int OP_CEN_EDITA_ANALISE_CREDITO_PENDENTE_VENDAS = 19900;

        public const int OP_CEN_REL_PRODUTOS_ESTOQUE_DEVOLUCAO = 20000;

        public const int OP_CEN_CANCELAR_PEDIDO = 20100;

        public const int OP_CEN_EDITA_CLIENTE_CAMPO_INDICADOR = 20200;

        public const int OP_CEN_REL_DIVERGENCIA_CLIENTE_INDICADOR = 20300;

        public const int OP_CEN_ANOTA_PEDIDO_RECEBIDO = 20400;

        public const int OP_CEN_REL_PEDIDO_NAO_RECEBIDO = 20500;

        public const int OP_CEN_EDITA_PEDIDO_STATUS_PEDIDO_RECEBIDO = 20600;

        public const int OP_CEN_CAD_MENSAGEM_ALERTA_PRODUTOS = 20700;

        public const int OP_CEN_REL_METAS_INDICADOR = 20800;

        public const int OP_CEN_CAD_TABELA_COMISSAO_VENDEDOR = 20900;

        public const int OP_CEN_REL_COMISSAO_VENDEDORES_TABELA_PROGRESSIVA_SINTETICO = 21000;

        public const int OP_CEN_REL_COMISSAO_VENDEDORES_TABELA_PROGRESSIVA_ANALITICO = 21100;

        public const int OP_CEN_CAD_TABELA_CUSTO_FINANCEIRO_FORNECEDOR = 21200;

        public const int OP_CEN_FIN_CADASTRO_PLANO_CONTAS = 21300;

        public const int OP_CEN_FIN_APP_FINANCEIRO_ACESSO_AO_MODULO = 21400;

        public const int OP_CEN_FIN_APP_FINANCEIRO_FLUXO_CAIXA_CONSULTAR = 21500;

        public const int OP_CEN_FIN_APP_FINANCEIRO_FLUXO_CAIXA_LANCTO_DEBITO = 21600;

        public const int OP_CEN_FIN_APP_FINANCEIRO_FLUXO_CAIXA_LANCTO_CREDITO = 21700;

        public const int OP_CEN_FIN_APP_FINANCEIRO_FLUXO_CAIXA_EDITAR_LANCTO = 21800;

        public const int OP_CEN_MODULO_NF_ACESSO_AO_MODULO = 21900;

        public const int OP_CEN_FIN_APP_FINANCEIRO_BOLETO_CADASTRAR = 22000;

        public const int OP_CEN_CADASTRO_EQUIPE_VENDAS = 22100;

        public const int OP_CEN_FIN_APP_COBRANCA_ACESSO_AO_MODULO = 22200;

        public const int OP_CEN_FIN_APP_COBRANCA_ADMINISTRACAO_CARTEIRA_EM_ATRASO = 22300;

        public const int OP_CEN_FIN_APP_COBRANCA_COBRANCA_CARTEIRA_EM_ATRASO = 22400;

        public const int OP_CEN_REL_PERFORMANCE_INDICADOR = 22500;

        public const int OP_CEN_REL_AUDITORIA_ESTOQUE = 22600;

        public const int OP_CEN_BLOCO_NOTAS_PEDIDO_LEITURA = 22700;

        public const int OP_CEN_BLOCO_NOTAS_PEDIDO_CADASTRAMENTO = 22800;

        public const int OP_CEN_EDITA_PEDIDO_GARANTIA_INDICADOR = 22900;

        public const int OP_CEN_PEDIDO_EXIBIR_LINK_DANFE = 23000;

        public const int OP_CEN_CANCELAR_PEDIDO_COM_NFE_EMITIDA = 23100;

        public const int OP_CEN_EDITA_PEDIDO_DADOS_NFE_MERCADORIAS_DEVOLVIDAS = 23200;

        public const int OP_CEN_OCORRENCIAS_EM_PEDIDOS_LEITURA = 23400;

        public const int OP_CEN_REL_OCORRENCIAS_EM_PEDIDOS = 23500;

        public const int OP_CEN_REL_ESTATISTICAS_OCORRENCIAS_EM_PEDIDOS = 23600;

        public const int OP_CEN_EDITA_CLIENTE_DADOS_CADASTRAIS = 23700;

        public const int OP_CEN_OCORRENCIAS_EM_PEDIDOS_CADASTRAMENTO = 23800;

        public const int OP_CEN_BLOCO_NOTAS_ITEM_DEVOLVIDO_LEITURA = 23900;

        public const int OP_CEN_BLOCO_NOTAS_ITEM_DEVOLVIDO_CADASTRAMENTO = 24000;

        public const int OP_CEN_CADASTRA_DEVOLUCAO_PRODUTO = 24100;

        public const int OP_CEN_REL_FATURAMENTO3 = 24200;

        public const int OP_CEN_REL_PRODUTO_DEPOSITO_ZONA = 24300;

        public const int OP_CEN_REL_SEPARACAO_ZONA = 24400;

        public const int OP_CEN_REL_TRANSACOES_CIELO = 24500;

        public const int OP_CEN_CAD_PERC_MAX_COMISSAO_E_DESC_POR_LOJA = 24600;

        public const int OP_CEN_CAD_INDICADOR_OPCOES_FORMA_COMO_CONHECEU = 24700;

        public const int OP_CEN_REL_PESQUISA_ORDEM_SERVICO = 24800;

        public const int OP_CEN_PEDIDO_EXIBIR_DADOS_LOGISTICA = 24900;

        public const int OP_CEN_REL_FAROL_RESUMIDO = 25000;

        public const int OP_CEN_REL_FAROL_CADASTRO_PRODUTO_COMPRADO = 25100;

        public const int OP_CEN_REL_SINTETICO_CUBAGEM_VOLUME_PESO = 25300;

        public const int OP_CEN_REL_TRANSACOES_CIELO_ANDAMENTO = 25400;

        public const int OP_CEN_FRETE_ADICIONAL_LEITURA = 00000;//' TODO

        public const int OP_CEN_FRETE_ADICIONAL_CADASTRAMENTO = 00000; //' TODO

        public const int OP_CEN_ETQWMS_EDITA_DADOS_ETIQUETA = 25700;

        public const int OP_CEN_VENDAS_POR_BOLETO = 25800;

        public const int OP_CEN_EDITA_FORMA_PAGTO_SEM_APLICAR_RESTRICOES = 25900;

        public const int OP_CEN_PEDIDO_HISTORICO_PAGAMENTO_EXIBE = 26100;

        public const int OP_CEN_CAD_EC_PRODUTO_COMPOSTO = 26200;

        public const int OP_CEN_REL_ECOMMERCE_EXPORTACAO = 26300;

        public const int OP_CEN_REL_BRASPAG_TRANSACOES = 26400;

        public const int OP_CEN_REL_BRASPAG_AF_REVIEW = 26500;

        public const int OP_CEN_REL_CLIENTE_SPC = 26600;

        public const int OP_CEN_REL_DADOS_TABELA_DINAMICA = 26700;

        public const int OP_CEN_CAD_PRODUTO_COMPOSTO = 26800;

        public const int OP_CEN_CAD_TAB_DESCONTOS_ORCAMENTISTAS_E_INDICADORES = 26900;

        public const int OP_CEN_REL_PEDIDOS_INDICADORES_PAGAMENTO = 27000;

        public const int OP_CEN_REL_PEDIDOS_CANCELADOS = 27100;

        public const int OP_CEN_GER_LIST_CAD_INDICADORES = 27200;

        public const int OP_CEN_REL_PERFIL_PAGAMENTO_BOLETOS = 27300;

        public const int OP_CEN_EDITA_PEDIDO_INDICADOR = 27400;

        public const int OP_CEN_EDITA_PEDIDO_NUM_PEDIDO_ECOMMERCE = 27500;

        public const int OP_CEN_REL_IMPOSTOS_PAGOS = 27550;

        public const int OP_CEN_REL_CONTROLE_IMPOSTOS = 27600;

        public const int OP_CEN_APP_CONSOLIDADOR_XLS_EC__ACESSO = 27700;

        public const int OP_CEN_REL_PEDIDO_MARKETPLACE_NAO_RECEBIDO_CLIENTE = 27800;

        public const int OP_CEN_REL_REGISTRO_PEDIDO_MARKETPLACE_RECEBIDO_CLIENTE = 27900;

        public const int OP_CEN_PEDIDO_CHAMADO_LEITURA_QUALQUER_CHAMADO = 28000;

        public const int OP_CEN_PEDIDO_CHAMADO_CADASTRAMENTO = 28100;

        public const int OP_CEN_PEDIDO_CHAMADO_ESCREVER_MSG_QUALQUER_CHAMADO = 28200;

        public const int OP_CEN_REL_PEDIDO_CHAMADO = 28300;

        public const int OP_CEN_REL_ESTATISTICAS_PEDIDO_CHAMADO = 28400;

        public const int OP_CEN_PEDIDO_CHAMADO_CONSULTA_CHAMADOS_TODOS_DEPTOS = 28500;

        public const int OP_CEN_PEDIDO_EXIBE_DETALHES_HISTORICO_PAGTO_CARTAO = 28600;

        public const int OP_CEN_MULTI_CD_CADASTRO_REGRAS_CONSUMO_ESTOQUE = 28700;

        public const int OP_CEN_MULTI_CD_ASSOCIACAO_PRODUTO_REGRA = 28800;

        public const int OP_CEN_FIN_APP_FINANCEIRO_ETIQUETAS_ACESSO_AO_MODULO = 28900;

        public const int OP_CEN_APP_CONSOLIDADOR_XLS_EC__ADM_PRECOS = 29000;

        public const int OP_CEN_APP_CONSOLIDADOR_XLS_EC__ADM_PEDIDOS = 29100;

        public const int OP_CEN_PRE_DEVOLUCAO_CADASTRAMENTO = 29200;

        public const int OP_CEN_PRE_DEVOLUCAO_RECEBIMENTO_MERCADORIA = 29300;

        public const int OP_CEN_PRE_DEVOLUCAO_ADMINISTRACAO = 29400;

        public const int OP_CEN_PRE_DEVOLUCAO_LEITURA = 29500;

        public const int OP_CEN_PRE_DEVOLUCAO_ESCREVER_MSG = 29600;

        public const int OP_CEN_EDITA_PEDIDO_CD = 29800;


        //' LOJA
        //'	public const  int   PRAZO_ACESSO_REL_PEDIDOS_INDICADORES_LOJA     = 5

        public const int PRAZO_ACESSO_REL_PEDIDOS_INDICADORES_LOJA = 8;

        public const int OP_LJA_CADASTRA_NOVO_PEDIDO = 50100;

        public const int OP_LJA_CADASTRA_NOVO_ORCAMENTO = 50200;

        public const int OP_LJA_CONSULTA_PEDIDO = 50300;

        public const int OP_LJA_CONSULTA_ORCAMENTO = 50400;

        public const int OP_LJA_CONSULTA_DISPONIBILIDADE_ESTOQUE = 50500;

        public const int OP_LJA_CONSULTA_PROD_BLOQ_ESTOQUE_ENTREGA = 50600;

        public const int OP_LJA_CONSULTA_LISTA_PRECOS = 50700;

        public const int OP_LJA_LER_AVISOS_NAO_LIDOS = 50800;

        public const int OP_LJA_LER_AVISOS_TODOS = 50900;

        public const int OP_LJA_CONSULTA_UNIVERSAL_PEDIDO_ORCAMENTO = 51000;

        public const int OP_LJA_REL_PEDIDOS_CREDITO_PENDENTE = 51100;

        public const int OP_LJA_REL_COMISSAO_INDICADORES = 51200;

        public const int OP_LJA_REL_FATURAMENTO = 51300;

        public const int OP_LJA_REL_VENDAS_COM_DESCONTO_SUPERIOR = 51400;

        public const int OP_LJA_REL_MULTICRITERIO_PEDIDOS_ANALITICO = 51500;

        public const int OP_LJA_CADASTRA_SENHA_DESCONTO = 51600;

        public const int OP_LJA_RESTRINGE_DT_INICIAL_FILTRO_PERIODO = 51700;

        public const int OP_LJA_EDITA_PEDIDO = 51800;

        public const int OP_LJA_EDITA_RA = 51900;

        public const int OP_LJA_EDITA_RT = 51950;

        public const int OP_LJA_EDITA_ORCAMENTO = 52000;

        public const int OP_LJA_CONS_CAD_ORCAMENTISTAS_E_INDICADORES = 52100;

        public const int OP_LJA_REL_PEDIDOS_COLOCADOS_NO_MES = 52200;

        public const int OP_LJA_REL_COMISSAO_VENDEDORES = 52300;

        public const int OP_LJA_REL_ESTOQUE_VENDA_SINTETICO = 52400;

        public const int OP_LJA_REL_DEVOLUCAO_PRODUTOS = 52500;

        public const int OP_LJA_REL_MEIO_DIVULGACAO = 52600;

        public const int OP_LJA_REL_VENDAS = 52700;

        public const int OP_LJA_EDITA_ITEM_DO_PEDIDO = 52800;

        public const int OP_LJA_EDITA_ITEM_DO_ORCAMENTO = 52900;

        public const int OP_LJA_PESQUISA_PEDIDO_POR_OBS2 = 53000;

        public const int OP_LJA_REL_ESTOQUE_VENDA_INTERMEDIARIO = 53100;

        public const int OP_LJA_EDITA_PEDIDO_FORMA_PAGTO = 53200;

        public const int OP_LJA_REL_MULTICRITERIO_PEDIDOS_SINTETICO = 53300;

        public const int OP_LJA_REL_GERENCIAL_DE_VENDAS = 53400;

        public const int OP_LJA_PESQUISA_INDICADORES = 53500;

        public const int OP_LJA_CADASTRAMENTO_INDICADOR = 53600;

        public const int OP_LJA_REL_CHECAGEM_NOVOS_PARCEIROS = 53700;

        public const int OP_LJA_REL_FATURAMENTO_CMVPV = 53800;

        public const int OP_LJA_REL_ESTOQUE_VENDA_SINTETICO_CMVPV = 53900;

        public const int OP_LJA_REL_ESTOQUE_VENDA_INTERMEDIARIO_CMVPV = 54000;

        public const int OP_LJA_LOGIN_TROCA_RAPIDA_LOJA = 54100;

        public const int OP_LJA_EDITA_ANALISE_CREDITO_PENDENTE_VENDAS = 54200;

        public const int OP_LJA_EDITA_CLIENTE_CAMPO_INDICADOR = 54300;

        public const int OP_LJA_REL_DIVERGENCIA_CLIENTE_INDICADOR = 54400;

        public const int OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO = 54500;

        public const int OP_LJA_REL_PEDIDOS_CREDITO_PENDENTE_VENDAS = 54600;

        public const int OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO = 54700;

        public const int OP_LJA_EXIBIR_CAMPOS_COM_SEM_INDICACAO_AO_CADASTRAR_NOVO_PEDIDO = 54800;

        public const int OP_LJA_REL_METAS_INDICADOR = 54900;

        public const int OP_LJA_REL_COMISSAO_VENDEDORES_TABELA_PROGRESSIVA_SINTETICO = 55000;

        public const int OP_LJA_REL_COMISSAO_VENDEDORES_TABELA_PROGRESSIVA_ANALITICO = 55100;

        public const int OP_LJA_REL_PRODUTOS_ESTOQUE_DEVOLUCAO = 55200;

        public const int OP_LJA_CONSULTA_TABELA_CUSTO_FINANCEIRO_FORNECEDOR = 55300;

        public const int OP_LJA_CADASTRA_DEVOLUCAO_PRODUTO = 55400;

        public const int OP_LJA_REL_PERFORMANCE_INDICADOR = 55500;

        public const int OP_LJA_BLOCO_NOTAS_PEDIDO_LEITURA = 55600;

        public const int OP_LJA_BLOCO_NOTAS_PEDIDO_CADASTRAMENTO = 55700;

        public const int OP_LJA_EDITA_PEDIDO_GARANTIA_INDICADOR = 55800;

        public const int OP_LJA_PEDIDO_EXIBIR_LINK_DANFE = 55900;

        public const int OP_LJA_EDITA_PEDIDO_OBS1 = 56000;

        public const int OP_LJA_CANCELAR_PEDIDO_COM_NFE_EMITIDA = 56100;

        public const int OP_LJA_OCORRENCIAS_EM_PEDIDOS_LEITURA = 56200;

        public const int OP_LJA_OCORRENCIAS_EM_PEDIDOS_CADASTRAMENTO = 56300;

        public const int OP_LJA_EDITA_CLIENTE_DADOS_CADASTRAIS = 56400;

        public const int OP_LJA_BLOCO_NOTAS_ITEM_DEVOLVIDO_LEITURA = 56500;

        public const int OP_LJA_BLOCO_NOTAS_ITEM_DEVOLVIDO_CADASTRAMENTO = 56600;

        public const int OP_LJA_PREENCHER_INDICADOR_EM_PEDIDO_CADASTRADO = 56700;

        public const int OP_LJA_REL_PEDIDOS_PENDENTES_CARTAO = 56800;

        public const int OP_LJA_FRETE_ADICIONAL_LEITURA = 00000; //' TODO

        public const int OP_LJA_VENDAS_POR_BOLETO = 57000;

        public const int OP_LJA_EDITA_FORMA_PAGTO_SEM_APLICAR_RESTRICOES = 57100;

        public const int OP_LJA_EDITA_CAD_ORCAMENTISTAS_E_INDICADORES = 57200;

        public const int OP_LJA_EDITA_CAMPO_ENTREGA_IMEDIATA = 57300;

        public const int OP_LJA_OCORRENCIAS_EM_PEDIDOS_ESCREVER_MSG = 57400;

        public const int OP_LJA_REL_ESTATISTICAS_OCORRENCIAS_EM_PEDIDOS = 57500;

        public const int OP_LJA_REL_INDICADORES_SEM_ATIVIDADE_RECENTE = 57600;

        public const int OP_LJA_REL_PEDIDOS_CANCELADOS = 57700;

        public const int OP_LJA_EDITA_PEDIDO_INDICADOR = 57800;

        public const int OP_LJA_EDITA_PEDIDO_NUM_PEDIDO_ECOMMERCE = 57900;

        public const int OP_LJA_SELECIONAR_QUALQUER_INDICADOR_EM_PEDIDO_NOVO = 58000;

        public const int OP_LJA_PEDIDO_HISTORICO_PAGAMENTO_EXIBE = 58100;

        public const int OP_LJA_REL_REGISTRO_PEDIDO_MARKETPLACE_RECEBIDO_CLIENTE = 58200;

        public const int OP_LJA_PEDIDO_CHAMADO_LEITURA_QUALQUER_CHAMADO = 58300;

        public const int OP_LJA_PEDIDO_CHAMADO_CADASTRAMENTO = 58400;

        public const int OP_LJA_PEDIDO_CHAMADO_ESCREVER_MSG_QUALQUER_CHAMADO = 58500;

        public const int OP_LJA_REL_PEDIDO_CHAMADO = 58600;

        public const int OP_LJA_REL_ESTATISTICAS_PEDIDO_CHAMADO = 58700;

        public const int OP_LJA_PEDIDO_CHAMADO_CONSULTA_CHAMADOS_TODOS_DEPTOS = 58800;

        public const int OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD = 58900;

        public const int OP_LJA_REL_PEDIDO_MARKETPLACE_NAO_RECEBIDO_CLIENTE = 59000;

        public const int OP_LJA_PEDIDO_EXIBE_DETALHES_HISTORICO_PAGTO_CARTAO = 59100;

        public const int OP_LJA_CADASTRA_NOVO_PEDIDO_EC_SEMI_AUTOMATICO = 59200;

        public const int OP_LJA_PRE_DEVOLUCAO_CADASTRAMENTO = 59300;

        public const int OP_LJA_PRE_DEVOLUCAO_LEITURA = 59400;

        public const int OP_LJA_PRE_DEVOLUCAO_ESCREVER_MSG = 59500;

        public const int OP_LJA_CADASTRA_NOVO_PEDIDO_EC_INDICADOR_SEMI_AUTOMATICO = 59600;

        public const int OP_LJA_PRE_DEVOLUCAO_ADMINISTRACAO = 59700;


        //' CÓDIGOS DE ERRO
        //' ===============
        //' ERROS QUE ENCERRAM A SESSÃO DO USUÁRIO

        public const string ERR_PADRAO = "0";

        public const string ERR_CONEXAO = "1";

        public const string ERR_IDENTIFICACAO = "2";

        public const string ERR_IDENTIFICACAO_LOJA = "3";

        public const string ERR_SESSAO = "4";

        public static string ERR_SENHA_EXPIRADA = "4";

        public const string ERR_SENHA_INVALIDA = "5";

        public const string ERR_ACESSO_INSUFICIENTE = "6";

        public const string ERR_USUARIO_BLOQUEADO = "7";

        public const string ERR_FALHA_OPERACAO_BD = "8";

        public const string ERR_FALHA_OPERACAO_GERAR_NSU = "9";

        public const string ERR_NSU_JA_EM_USO = "10";

        public const string ERR_NSU_NAO_LOCALIZADO = "11";

        public const string ERR_ID_INVALIDO = "12";

        public const string ERR_FALHA_OPERACAO_MOVIMENTO_ESTOQUE = "13";

        public const string ERR_FALHA_GERAR_ID_PEDIDO_FILHOTE = "14";

        public const string ERR_FALHA_OPERACAO_CRIAR_ADO = "15";

        public const string ERR_PAG_DEST_INDEFINIDA = "16";

        public const string ERR_TIT_REL_INDEFINIDO = "17";

        public const string ERR_TIPO_CARTAO_INVALIDO = "18";

        public const string ERR_FALHA_OPERACAO_GRAVAR_LOG_ESTOQUE = "19";

        public const string ERR_SENHA_NAO_INFORMADA = "20";

        public const string ERR_HORARIO_MANUTENCAO_SISTEMA = "21";

        public const string ERR_HORARIO_REBOOT_SERVIDOR = "22";

        //' ERROS QUE NÃO ENCERRAM A SESSÃO DO USUÁRIO

        public const string ERR_USUARIO_NAO_ESPECIFICADO = "5001";

        public const string ERR_OPERACAO_NAO_ESPECIFICADA = "5002";

        public const string ERR_USUARIO_JA_CADASTRADO = "5003";

        public const string ERR_USUARIO_NAO_CADASTRADO = "5004";

        public const string ERR_LOJA_NAO_ESPECIFICADA = "5005";

        public const string ERR_LOJA_JA_CADASTRADA = "5006";

        public const string ERR_LOJA_NAO_CADASTRADA = "5007";

        public const string ERR_FABRICANTE_NAO_ESPECIFICADO = "5008";

        public const string ERR_FABRICANTE_JA_CADASTRADO = "5009";

        public const string ERR_FABRICANTE_NAO_CADASTRADO = "5010";

        public const string ERR_MIDIA_NAO_ESPECIFICADA = "5011";

        public const string ERR_MIDIA_JA_CADASTRADA = "5012";

        public const string ERR_MIDIA_NAO_CADASTRADA = "5013";

        public const string ERR_CLIENTE_NAO_ESPECIFICADO = "5014";

        public const string ERR_CLIENTE_JA_CADASTRADO = "5015";

        public const string ERR_CLIENTE_NAO_CADASTRADO = "5016";

        public const string ERR_CONSULTAR_ESTOQUE = "5017";

        public const string ERR_PEDIDO_NAO_ESPECIFICADO = "5018";

        public const string ERR_PEDIDO_INVALIDO = "5019";

        public const string ERR_ESTOQUE_NAO_ESPECIFICADO = "5020";

        public const string ERR_GRUPO_LOJAS_NAO_ESPECIFICADO = "5021";

        public const string ERR_GRUPO_LOJAS_JA_CADASTRADO = "5022";

        public const string ERR_GRUPO_LOJAS_NAO_CADASTRADO = "5023";

        public const string ERR_ORCAMENTO_NAO_ESPECIFICADO = "5024";

        public const string ERR_ORCAMENTO_INVALIDO = "5025";

        public const string ERR_CNPJ_CPF_INVALIDO = "5026";

        public const string ERR_OPCAO_PAGTO_INVALIDA = "5027";

        public const string ERR_VISANET_TID_NAO_ESPECIFICADO = "5028";

        public const string ERR_TRANSPORTADORA_NAO_ESPECIFICADA = "5029";

        public const string ERR_TRANSPORTADORA_JA_CADASTRADA = "5030";

        public const string ERR_TRANSPORTADORA_NAO_CADASTRADA = "5031";

        public const string ERR_PERFIL_NAO_ESPECIFICADO = "5032";

        public const string ERR_PERFIL_JA_CADASTRADO = "5033";

        public const string ERR_PERFIL_NAO_CADASTRADO = "5034";

        public const string ERR_ORCAMENTISTA_INDICADOR_NAO_ESPECIFICADO = "5035";

        public const string ERR_ORCAMENTISTA_INDICADOR_JA_CADASTRADO = "5036";

        public const string ERR_ORCAMENTISTA_INDICADOR_NAO_CADASTRADO = "5037";

        public const string ERR_ID_JA_EM_USO_POR_USUARIO = "5038";

        public const string ERR_ID_JA_EM_USO_POR_ORCAMENTISTA = "5039";

        public const string ERR_CLIENTE_FALHA_RECUPERAR_DADOS = "5040";

        public const string ERR_ORDEM_SERVICO_NAO_CADASTRADA = "5041";

        public const string ERR_PEDIDO_ACESSO_NEGADO = "5042";

        public const string ERR_CEP_NAO_ESPECIFICADO = "5043";

        public const string ERR_CEP_INVALIDO = "5044";

        public const string ERR_IDENTIFICADOR_NAO_FORNECIDO = "5045";

        public const string ERR_REGISTRO_NAO_CADASTRADO = "5046";

        public const string ERR_IDENTIFICADOR_JA_CADASTRADO = "5047";

        public const string ERR_ID_NAO_INFORMADO = "5048";

        public const string ERR_ID_JA_CADASTRADO = "5049";

        public const string ERR_ID_NAO_CADASTRADO = "5050";

        public const string ERR_FIN_NATUREZA_OPERACAO_NAO_ESPECIFICADO = "5051";

        public const string ERR_IDENTIFICADOR_NAO_CADASTRADO = "5052";

        public const string ERR_CAD_CLIENTE_ENDERECO_NUMERO_NAO_PREENCHIDO = "5053";

        public const string ERR_CAD_CLIENTE_ENDERECO_EXCEDE_TAMANHO_MAXIMO = "5054";

        public const string ERR_VALOR_MINIMO_NAO_CADASTRADO = "5055";

        public const string ERR_NIVEL_ACESSO_INSUFICIENTE = "5056";

        public const string ERR_PARAMETRO_OBRIGATORIO_NAO_ESPECIFICADO = "5057";

        public const string ERR_CIELO_VALOR_PAGTO_NAO_INFORMADO = "5058";

        public const string ERR_CIELO_FORMA_PAGTO_NAO_INFORMADO = "5059";

        public const string ERR_CIELO_FORMA_PAGTO_INVALIDO = "5060";

        public const string ERR_CIELO_QTDE_PARCELAS_INVALIDA = "5061";

        public const string ERR_BRASPAG_VALOR_PAGTO_NAO_INFORMADO = "5062";

        public const string ERR_BRASPAG_FORMA_PAGTO_NAO_INFORMADO = "5063";

        public const string ERR_BRASPAG_FORMA_PAGTO_INVALIDO = "5064";

        public const string ERR_BRASPAG_QTDE_PARCELAS_INVALIDA = "5065";

        public const string ERR_BRASPAG_NOME_TITULAR_NAO_INFORMADO = "5066";

        public const string ERR_BRASPAG_NUMERO_CARTAO_NAO_INFORMADO = "5067";

        public const string ERR_BRASPAG_NUMERO_CARTAO_COM_TAMANHO_INVALIDO = "5068";

        public const string ERR_BRASPAG_VALIDADE_MES_NAO_INFORMADO = "5069";

        public const string ERR_BRASPAG_VALIDADE_ANO_NAO_INFORMADO = "5070";

        public const string ERR_BRASPAG_CODIGO_SEGURANCA_NAO_INFORMADO = "5071";

        public const string ERR_BRASPAG_FATURA_ENDERECO_LOGRADOURO_NAO_INFORMADO = "5072";

        public const string ERR_BRASPAG_FATURA_ENDERECO_NUMERO_NAO_INFORMADO = "5073";

        public const string ERR_BRASPAG_FATURA_ENDERECO_UF_NAO_INFORMADA = "5074";

        public const string ERR_BRASPAG_FATURA_CEP_NAO_INFORMADO = "5075";

        public const string ERR_BRASPAG_FATURA_CEP_COM_TAMANHO_INVALIDO = "5076";

        public const string ERR_BRASPAG_FATURA_ENDERECO_CIDADE_NAO_INFORMADO = "5077";

        public const string ERR_BRASPAG_NOME_TITULAR_INVALIDO = "5078";

        public const string ERR_EC_PRODUTO_COMPOSTO_NAO_ESPECIFICADO = "5079";

        public const string ERR_EC_PRODUTO_COMPOSTO_JA_CADASTRADO = "5080";

        public const string ERR_EC_PRODUTO_COMPOSTO_NAO_CADASTRADO = "5081";

        public const string ERR_BRASPAG_FATURA_TEL_PAIS_NAO_INFORMADO = "5082";

        public const string ERR_BRASPAG_FATURA_TEL_PAIS_INVALIDO = "5083";

        public const string ERR_BRASPAG_FATURA_TEL_DDD_NAO_INFORMADO = "5084";

        public const string ERR_BRASPAG_FATURA_TEL_DDD_INVALIDO = "5085";

        public const string ERR_BRASPAG_FATURA_TEL_NUMERO_NAO_INFORMADO = "5086";

        public const string ERR_BRASPAG_FATURA_TEL_NUMERO_INVALIDO = "5087";

        public const string ERR_BRASPAG_PERGUNTA_CARTAO_PROPRIO_NAO_RESPONDIDA = "5088";

        public const string ERR_PRODUTO_NAO_ESPECIFICADO = "5089";

        public const string ERR_PRODUTO_COMPOSTO_JA_CADASTRADO = "5090";

        public const string ERR_PRODUTO_COMPOSTO_NAO_ESPECIFICADO = "5091";

        public const string ERR_PRODUTO_COMPOSTO_NAO_CADASTRADO = "5092";

        public const string ERR_NENHUM_CD_HABILITADO_PARA_USUARIO = "5093";
        public const string ERR_INDICADORES_VENDEDOR_INFORMADO_JA_PROCESSADO = "5094";
        public const string ERR_INESPERADO = "5095";

        public const string ERR_QTDE_CARTOES_INVALIDA = "5096";
        public const string ERR_EC_PRODUTO_COMPOSTO_ITEM_JA_CADASTRADO = "5097";
        public const string ERR_EC_PRODUTO_PRE_LISTA_CADASTRADO = "5098";

        public const string ERR_MULTI_CD_REGRA_NAO_ESPECIFICADA = "5099";

        public const string ERR_MULTI_CD_REGRA_APELIDO_NAO_INFORMADO = "5100";

        public const string ERR_MULTI_CD_REGRA_JA_CADASTRADA = "5101";

        public const string ERR_MULTI_CD_REGRA_NAO_CADASTRADA = "5102";

        public const string NUMERO_LOJA_ECOMMERCE_AR_CLUBE = "201";

        public const string NUMERO_LOJA_BONSHOP = "202";
        public const string NUMERO_LOJA_BONSHOP_LAB = "204";
        public const string NUMERO_LOJA_VRF = "205";
        public const string NUMERO_LOJA_VRF2 = "206";
        public const string NUMERO_LOJA_VRF3 = "207";
        public const string NUMERO_LOJA_VRF4 = "208";
        public const string NUMERO_LOJA_OLD03 = "300";
        public const string NUMERO_LOJA_OLD03_BONIFICACAO = "301";
        public const string NUMERO_LOJA_OLD03_ASSISTENCIA = "302";
        public const string NUMERO_LOJA_MARCELO_ARTVEN = "305";
        public const string NUMERO_LOJA_TRANSFERENCIA = "01";
        public const string NUMERO_LOJA_KITS = "02";

        //CÓDIGOS QUE IDENTIFICAM A UNIDADE DE NEGÓCIO A QUE A LOJA PERTENCE
        public const string COD_UNIDADE_NEGOCIO_LOJA__VRF = "VRF";
        public const string COD_UNIDADE_NEGOCIO_LOJA__BS = "BS";
        public const string COD_UNIDADE_NEGOCIO_LOJA__AC = "AC";
        public const string COD_UNIDADE_NEGOCIO_LOJA__GARANTIA = "GAR";

        public const string SESSION_CTRL_MODULO_CENTRAL = "CENTR";
        public const string SESSION_CTRL_MODULO_LOJA = "LOJA";
        public const string SESSION_CTRL_MODULO_ORCAMENTO = "ORCTO";
        public const string SESSION_CTRL_MODULO_APIUNIS = "APIUN";
        public const string SESSION_CTRL_MODULO_APIMAGENTO = "APIMA";


        //Para o t_LOJA.magento_api_versao
        /*
        -- 0 = Default, API usada inicialmente na integração com o Magento v1.8 (SOAP/XML)
        -- 2 = API do Magento 2 em REST/JSON
        */

        public const int VERSAO_API_MAGENTO_V1_SOAP_XML = 0;
        public const int VERSAO_API_MAGENTO_V2_REST_JSON = 2;

        public enum MagentoPedidoStatusEnum
        {
            //0 = pedido não é do Magento ou é pedido cadastrado antes da implantação da API para o Magento
            //0 também é usado para filhotes
            // 1 = aprovação pendente  (pedido cadastrado e pagamento não confirmado)
            // 2 = aprovado (pagamento confirmado)
            // 3 = rejeitado (pedido cancelado)
            MAGENTO_PEDIDO_STATUS_NAO_MAGENTO,
            MAGENTO_PEDIDO_STATUS_APROVACAO_PENDENTE,
            MAGENTO_PEDIDO_STATUS_APROVADO,
            MAGENTO_PEDIDO_STATUS_REJEITADO
        };

        public enum ePermissoes
        {
            ACESSO_AO_MODULO_100100 = 100100,
            ADMIN_DO_MODULO_100200 = 100200,
            PARCEIRO_INDICADOR_100300 = 100300,
            SELECIONAR_QUALQUER_LOJA_100400 = 100400,
            PRORRROGAR_VENCIMENTO_ORCAMENTO_100500 = 100500
        };

        public enum eCfgOrcamentoCotacaoEmailStatus
        {
            EnviarEmail = 0,
            EnvioCancelado = 1,
            EnvioComSucesso = 2,
            FalhaNoEnvioTemporario = 3,
            FalhaNoEnvioDefinitivo = 4
        }

        public enum eCfgParametro
        {
            ModuloOrcamentoCotacao_Login_BackgroundImage = 1,
            ModuloOrcamentoCotacao_UrlAcesso = 2,
            ModuloOrcamentoCotacao_LogoHeader = 3,
            ModuloOrcamentoCotacao_CorPrincipal = 4,
            ModuloOrcamentoCotacao_EmailTransacional_Remetente = 5,
            ModuloOrcamentoCotacao_EmailTransacional_RemetenteDisplayName = 6,
            ModuloOrcamentoCotacao_EmailTransacional_SmtpHost = 7,
            ModuloOrcamentoCotacao_EmailTransacional_SmtpPort = 8,
            ModuloOrcamentoCotacao_EmailTransacional_SmtpUsername = 9,
            ModuloOrcamentoCotacao_EmailTransacional_SmtpPassword = 10
        }
    }
}
