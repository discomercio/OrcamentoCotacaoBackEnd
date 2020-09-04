namespace Especificacao.Testes.Pedido
{
    public interface IPedidoPassosComuns
    {
        void WhenInformo(string p0, string p1);
        void WhenPedidoBase();
        void ThenSemErro(string p0);
        void ThenErro(string p0);
        void GivenIgnorarFeatureNoAmbiente(string p0);
    }
}