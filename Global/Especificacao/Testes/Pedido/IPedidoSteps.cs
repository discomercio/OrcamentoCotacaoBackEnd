namespace Especificacao.Testes.Pedido
{
    public interface IPedidoSteps
    {
        void ThenNoAmbienteErro(string ambiente, string erro);
        void ThenNoAmbienteSemErro(string ambiente, string erro);
        void WhenInformo(string p0, string p1);
        void WhenPedidoBase();
    }
}