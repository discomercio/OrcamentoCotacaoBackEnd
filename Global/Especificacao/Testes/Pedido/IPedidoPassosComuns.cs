namespace Especificacao.Testes.Pedido
{
    public interface IPedidoPassosComuns
    {
        void GivenPedidoBaseComEnderecoDeEntrega();
        void WhenPedidoBase();
        void WhenInformo(string p0, string p1);
        void ThenSemErro(string p0);
        void ThenErro(string p0);
        void GivenIgnorarFeatureNoAmbiente2(string p0);
        void ThenSemNenhumErro();
    }
}