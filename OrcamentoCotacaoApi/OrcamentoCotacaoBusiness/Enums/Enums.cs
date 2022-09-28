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
            VisualizarOrcamentoConsultar = 103000,
            AcessoUniversalOrcamentoEditar = 103100,
            AprovarOrcamento = 100550,

            VisualizarPrePedidoConsultar = 102000,
            CancelarPrePedido = 102200,
            ConsultarPedido = 102300
        }

        public enum StatusOrcamento
        {
            Enviado = 1,
            Cancelado = 2,
            Aprovado = 3
        }
    }
}