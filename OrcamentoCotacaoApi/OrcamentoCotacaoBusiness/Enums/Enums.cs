namespace OrcamentoCotacaoBusiness.Enums
{
    public class Enums
    {
        public enum ePermissao
        {
            AcessoAoModulo = 100100,
            AdministradorDoModulo = 100200,
            ParceiroIndicadorUsuarioMaster = 100300,
            SelecionarQualquerIndicadorDaLoja = 100400,
            CadastroVendedorParceiroIncluirEditar = 100320,
            ProrrogarVencimentoOrcamento = 100500,
            ArquivosDownloadIncluirEditarPastasArquivos = 102800,

            DescontoSuperior1 = 100800,
            DescontoSuperior2 = 100900,
            DescontoSuperior3 = 101000,
            AcessoUniversalOrcamentoPedidoPrepedidoConsultar = 103000,
            AcessoUniversalOrcamentoEditar = 103100,
            AprovarOrcamento = 100550,

            VisualizarPrePedidoConsultar = 102000,
            CancelarPrePedido = 102200,
            IncluirPrePedido = 102100,
            ConsultarPedido = 102300,
            CatalogoConsultar = 102400,
            CatalogoCaradastrarIncluirEditar = 102500,
            CatalogoPropriedadeConsultar = 102600,
            CatalogoPropriedadeIncluirEditar = 102700,

            RelOrcamentosVigente = 103200,
            RelOrcamentosExpirados = 103300,
            RelOrcamentosMensagemPendente = 103400,
            RelOrcamentosCadastrados = 102900,

            ExcluirOrcamento = 103500,
            AnularOrcamentoAprovado = 103600
        }

        public enum StatusOrcamento
        {
            Enviado = 1,
            Cancelado = 2,
            Aprovado = 3
        }
    }
}