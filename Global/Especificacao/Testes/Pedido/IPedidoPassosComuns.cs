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
        void TabelaT_PEDIDORegistroPaiCriadoVerificarCampo(string campo, string valor);
        void TabelaT_PEDIDO_ITEMRegistroCriadoVerificarCampo(int item, string campo, string valor);
        void TabelaT_PEDIDORegistrosFilhotesCriadosVerificarCampo(string campo, string valor);
        void RecalcularTotaisDoPedido();
        void DeixarFormaDePagamentoConsistente();
        void ListaDeItensComXitens(int numeroItens);
        void TabelaT_ESTOQUE_MOVIMENTORegistroPaiEProdutoVerificarCampo(string produto, string tipo_estoque, string campo, string valor);
        void TabelaT_ESTOQUE_ITEMRegistroPaiEProdutoVerificarCampo(string produto, string campo, string valor);
        void TabelaT_ESTOQUERegistroPaiVerificarCampo(string campo, string valor);
        void TabelaT_ESTOQUE_LOGPedidoGeradoVerificarCampo(string produto, string operacao, string campo, string valor);
        void TabelaT_LOGPedidoGeradoEOperacaoVerificarCampo(string operacao, string campo, string valor);
        void TabelaT_PRODUTO_X_WMS_REGRA_CDFabricanteEProdutoVerificarCampo(string fabricante, string produto, string campo, string valor);
    }
}