﻿namespace Especificacao.Testes.Pedido
{
    public interface IPedidoPassosComuns
    {
        void GivenPedidoBaseComEnderecoDeEntrega();
        void GivenPedidoBase();
        void GivenPedidoBaseClientePF();
        void GivenPedidoBaseClientePJ();
        void GivenPedidoBaseClientePJComEnderecoDeEntrega();
        void WhenInformo(string p0, string p1);
        void ThenSemErro(string p0);
        void ThenErro(string p0);
        void GivenIgnorarCenarioNoAmbiente(string p0);
        void ThenSemNenhumErro();
        void ListaDeItensInformo(int numeroItem, string campo, string valor);
        void RecalcularTotaisDoPedido();
        void DeixarFormaDePagamentoConsistente();
        void ListaDeItensComXitens(int numeroItens);
    }
}