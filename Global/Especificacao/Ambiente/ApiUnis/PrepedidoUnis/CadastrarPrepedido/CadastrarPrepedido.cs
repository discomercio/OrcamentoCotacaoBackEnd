using InfraBanco.Constantes;
using Microsoft.Extensions.DependencyInjection;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    class CadastrarPrepedido : Testes.Pedido.IPedidoPassosComuns
    {
        private readonly PrepedidoAPIUnis.Controllers.PrepedidoUnisController prepedidoUnisController;

        public CadastrarPrepedido()
        {
            prepedidoUnisController = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<PrepedidoAPIUnis.Controllers.PrepedidoUnisController>();
        }

        private PrePedidoUnisDto prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBase();
        public void GivenPrepedidoBase()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBase(this);
            prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBase();
        }
        public void GivenPedidoBaseComEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseComEnderecoDeEntrega(this);
            prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBaseComEnderecoDeEntrega();
        }

        public void GivenPedidoBaseClientePF()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseComEnderecoDeEntrega(this);
            prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBaseClientePF();
        }

        public void GivenPedidoBaseClientePJ()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseComEnderecoDeEntrega(this);
            prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBaseClientePJ();
        }

        public void GivenPedidoBaseClientePJComEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseClientePJComEnderecoDeEntrega(this);
            prePedidoUnisDto = CadastrarPrepedidoDados.PrepedidoBaseClientePJComEnderecoDeEntrega();
        }

        public void GivenPedidoBase()
        {
            GivenPrepedidoBase();
        }

        public void RecalcularTotaisDoPedido()
        {
            decimal totalCompare = 0;
            decimal totalRaCompare = 0;
            foreach (var x in prePedidoUnisDto.ListaProdutos)
            {
                totalCompare += Math.Round((decimal)(x.Preco_Venda * x.Qtde), 2);
                totalRaCompare += Math.Round((decimal)(x.Preco_NF * x.Qtde), 2);
            }
            prePedidoUnisDto.VlTotalDestePedido = totalCompare;
            prePedidoUnisDto.ValorTotalDestePedidoComRA = totalRaCompare;
        }
        public void DeixarFormaDePagamentoConsistente()
        {
            EstaticoDeixarFormaDePagamentoConsistente(prePedidoUnisDto);
        }

        //3 versoes da mesma rotina...
        public static void EstaticoDeixarFormaDePagamentoConsistente(PrePedidoUnisDto prePedidoUnisDto)
        {
            var total = prePedidoUnisDto.ValorTotalDestePedidoComRA;
            var fp = prePedidoUnisDto.FormaPagtoCriacao;
            switch (fp.Tipo_Parcelamento.ToString())
            {
                case Constantes.COD_FORMA_PAGTO_A_VISTA:
                    fp.Op_av_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
                    fp.C_pc_qtde = fp.CustoFinancFornecQtdeParcelas;
                    fp.C_pc_valor = total / fp.C_pc_qtde;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
                    fp.C_pce_entrada_valor = 3;
                    fp.C_pce_prestacao_qtde = fp.CustoFinancFornecQtdeParcelas;
                    fp.C_pce_prestacao_valor = (total - fp.C_pce_entrada_valor) / fp.C_pce_prestacao_qtde;
                    fp.C_pce_prestacao_periodo = 5;
                    fp.Op_pce_entrada_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    fp.Op_pce_prestacao_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
                    fp.C_pse_demais_prest_periodo = 10;
                    fp.C_pse_demais_prest_qtde = fp.CustoFinancFornecQtdeParcelas;
                    fp.C_pse_prim_prest_apos = 10;
                    fp.C_pse_prim_prest_valor = 5;
                    fp.C_pse_demais_prest_valor = (total - fp.C_pse_prim_prest_valor) / fp.C_pse_demais_prest_qtde;
                    fp.Op_pse_demais_prest_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    fp.Op_pse_prim_prest_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                    fp.C_pu_valor = total;
                    fp.C_pu_vencto_apos = 5;
                    fp.Op_pu_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
                    fp.C_pc_maquineta_qtde = fp.CustoFinancFornecQtdeParcelas;
                    fp.C_pc_maquineta_valor = total / fp.C_pc_maquineta_qtde;
                    break;
            }
        }
        //3 versoes da mesma rotina...
        public static void EstaticoDeixarFormaDePagamentoConsistente(MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto prePedidoUnisDto)
        {
            //var total = prePedidoUnisDto.VlTotalDestePedido;
            var total = prePedidoUnisDto.ListaProdutos.Select(x => Math.Round(x.Preco_Venda * x.Qtde, 2)).Sum();
            var fp = prePedidoUnisDto.FormaPagtoCriacao;
            switch (fp.Tipo_Parcelamento.ToString())
            {
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
                    fp.C_pc_qtde = 2;
                    fp.C_pc_valor = total / fp.C_pc_qtde;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                    fp.C_pu_valor = total;
                    break;
            }
        }
        //3 versoes da mesma rotina...
        public static void EstaticoDeixarFormaDePagamentoConsistente(global::Loja.Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto prePedidoUnisDto)
        {
            var total = prePedidoUnisDto.ValorTotalDestePedidoComRA;
            var fp = prePedidoUnisDto.FormaPagtoCriacao;
            switch (fp.Tipo_parcelamento.ToString())
            {
                case Constantes.COD_FORMA_PAGTO_A_VISTA:
                    fp.Op_av_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
                    fp.C_pc_qtde = fp.CustoFinancFornecQtdeParcelas;
                    fp.C_pc_valor = total / fp.C_pc_qtde;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
                    fp.C_pce_entrada_valor = 3;
                    fp.C_pce_prestacao_qtde = fp.CustoFinancFornecQtdeParcelas;
                    fp.C_pce_prestacao_valor = (total - fp.C_pce_entrada_valor) / fp.C_pce_prestacao_qtde;
                    fp.C_pce_prestacao_periodo = 5;
                    fp.Op_pce_entrada_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    fp.Op_pce_prestacao_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
                    fp.C_pse_demais_prest_periodo = 10;
                    fp.C_pse_demais_prest_qtde = fp.CustoFinancFornecQtdeParcelas;
                    fp.C_pse_prim_prest_apos = 10;
                    fp.C_pse_prim_prest_valor = 5;
                    fp.C_pse_demais_prest_valor = (total - fp.C_pse_prim_prest_valor) / fp.C_pse_demais_prest_qtde;
                    fp.Op_pse_demais_prest_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    fp.Op_pse_prim_prest_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                    fp.C_pu_valor = total;
                    fp.C_pu_vencto_apos = 5;
                    fp.Op_pu_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
                    fp.C_pc_maquineta_qtde = 2;
                    fp.C_pc_maquineta_valor = total / fp.C_pc_maquineta_qtde;
                    break;
            }
        }
        //3 versoes da mesma rotina...
        public static void EstaticoDeixarFormaDePagamentoConsistente(global::PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido.PrePedidoDto prePedidoUnisDto)
        {
            var total = prePedidoUnisDto.ValorTotalDestePedidoComRA;
            var fp = prePedidoUnisDto.FormaPagtoCriacao;
            switch (fp.Tipo_parcelamento.ToString())
            {
                case Constantes.COD_FORMA_PAGTO_A_VISTA:
                    fp.Op_av_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
                    fp.C_pc_qtde = fp.Qtde_Parcelas;
                    fp.C_pc_valor = total / fp.C_pc_qtde;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
                    fp.C_pce_entrada_valor = 3;
                    fp.C_pce_prestacao_qtde = fp.Qtde_Parcelas;
                    fp.C_pce_prestacao_valor = (total - fp.C_pce_entrada_valor) / fp.C_pce_prestacao_qtde;
                    fp.C_pce_prestacao_periodo = 5;
                    fp.Op_pce_entrada_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    fp.Op_pce_prestacao_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
                    fp.C_pse_demais_prest_periodo = 10;
                    fp.C_pse_demais_prest_qtde = fp.Qtde_Parcelas;
                    fp.C_pse_prim_prest_apos = 10;
                    fp.C_pse_prim_prest_valor = 5;
                    fp.C_pse_demais_prest_valor = (total - fp.C_pse_prim_prest_valor) / fp.C_pse_demais_prest_qtde;
                    fp.Op_pse_demais_prest_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    fp.Op_pse_prim_prest_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                    fp.C_pu_valor = total;
                    fp.C_pu_vencto_apos = 5;
                    fp.Op_pu_forma_pagto = Constantes.ID_FORMA_PAGTO_DEPOSITO;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
                    fp.C_pc_maquineta_qtde = fp.Qtde_Parcelas;
                    fp.C_pc_maquineta_valor = total / fp.C_pc_maquineta_qtde;
                    break;
            }
        }

        public void WhenInformo(string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.Informo(campo, valor, this);
            switch (campo)
            {
                case "TokenAcesso":
                    prePedidoUnisDto.TokenAcesso = valor;
                    return;
                case "CPF/CNPJ":
                    prePedidoUnisDto.Cnpj_Cpf = valor;
                    return;
            }

            //acertos em campos
            if (campo == "vl_total_NF")
                campo = "ValorTotalDestePedidoComRA";

            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, prePedidoUnisDto))
                return;
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, prePedidoUnisDto.FormaPagtoCriacao))
                return;
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, prePedidoUnisDto.EnderecoEntrega))
                return;
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, prePedidoUnisDto.EnderecoCadastralCliente))
                return;
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, prePedidoUnisDto.DetalhesPrepedido))
                return;

            switch (campo)
            {
                case "EndEtg_obs":
                    prePedidoUnisDto.EnderecoEntrega.EndEtg_cod_justificativa = valor;
                    return;
                case "Endereco_EmailXml":
                    prePedidoUnisDto.EnderecoCadastralCliente.Endereco_email_xml = valor;
                    return;
                case "FormaPagtoCriacao.Rb_forma_pagto":
                    prePedidoUnisDto.FormaPagtoCriacao.Tipo_Parcelamento = Convert.ToInt16(valor);
                    return;
                default:
                    Assert.Equal("", $"{campo} desconhecido na rotina Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.WhenInformo");
                    break;
            }
        }

        public void ListaDeItensComXitens(int numeroItens)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensComXitens(numeroItens, this);
            numeroItens = numeroItens < 0 ? 0 : numeroItens;
            numeroItens = numeroItens > 100 ? 100 : numeroItens;
            var lp = prePedidoUnisDto.ListaProdutos;
            while (lp.Count < numeroItens)
                lp.Add(new PrePedidoProdutoPrePedidoUnisDto());
            while (lp.Count > numeroItens)
                lp.RemoveAt(lp.Count - 1);
        }

        public void ListaDeItensInformo(int numeroItem, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensInformo(numeroItem, campo, valor, this);
            var item = prePedidoUnisDto.ListaProdutos[numeroItem];
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, item))
                return;
            Assert.Equal("", $"{campo} desconhecido na rotina Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.ListaDeItensInformo");
        }

        public void ThenErroStatusCode(int statusCode)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.ErroStatusCode(statusCode, this);
            Testes.Utils.LogTestes.LogOperacoes2.ChamadaController(prepedidoUnisController.GetType(), "CadastrarPrepedido", this);
            Microsoft.AspNetCore.Mvc.ActionResult<PrePedidoResultadoUnisDto> ret = prepedidoUnisController.CadastrarPrepedido(prePedidoUnisDto).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;
            Testes.Utils.StatusCodes.TestarStatusCode(statusCode, res);
        }

        public void ThenErro(string p0)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.Erro(p0, this);
            ThenErro(p0, true);
        }
        public void ThenSemErro(string p0)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.SemErro(p0, this);
            ThenErro(p0, false);
        }
        public void ThenSemNenhumErro()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.SemNenhumErro(this);
            ThenErro(null, false);
        }


        //para quando querem aessar o prepedido criado
        public PrePedidoResultadoUnisDto? UltimoPrePedidoResultadoUnisDto { get; private set; }

        private void ThenErro(string? erro, bool erroDeveExistir)
        {
            if (ignorarFeature) return;

            Testes.Utils.LogTestes.LogOperacoes2.ChamadaController(prepedidoUnisController.GetType(), "CadastrarPrepedido", this);

            Microsoft.AspNetCore.Mvc.ActionResult<PrePedidoResultadoUnisDto> ret = prepedidoUnisController.CadastrarPrepedido(prePedidoUnisDto).Result;
            Microsoft.AspNetCore.Mvc.ActionResult res = ret.Result;

            //deve ter retornado 200
            if (res.GetType() != typeof(Microsoft.AspNetCore.Mvc.OkObjectResult))
                Assert.Equal("", "Tipo não é OkObjectResult");

            UltimoPrePedidoResultadoUnisDto = (PrePedidoResultadoUnisDto)((Microsoft.AspNetCore.Mvc.OkObjectResult)res).Value;

            Testes.Pedido.HelperImplementacaoPedido.CompararMensagemErro(erro, erroDeveExistir, UltimoPrePedidoResultadoUnisDto.ListaErros, this);
        }

        private bool ignorarFeature = false;
        public void GivenIgnorarCenarioNoAmbiente(string p0)
        {
            Testes.Pedido.PedidoPassosComuns.IgnorarCenarioNoAmbiente(p0, ref ignorarFeature, this.GetType());
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public void TabelaT_PEDIDORegistroPaiCriadoVerificarCampo(string campo, string valor)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            //não é implementado aqui porque nunca deve ser executado dentro de um prepedido
            throw new NotImplementedException();
        }
#pragma warning disable IDE0060 // Remove unused parameter
        public void TabelaT_PEDIDO_ITEMRegistroCriadoVerificarCampo(int item, string campo, string valor)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            //não é implementado aqui porque nunca deve ser executado dentro de um prepedido
            throw new NotImplementedException();
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public void TabelaT_PEDIDORegistrosFilhotesCriadosVerificarCampo(string campo, string valor)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            //não é implementado aqui porque nunca deve ser executado dentro de um prepedido
            throw new NotImplementedException();
        }

        public void ThenTabelaVerificarCampo(string tabela, string campo, string valor)
        {
            throw new NotImplementedException();
        }

        public void TabelaT_ESTOQUE_MOVIMENTORegistroPaiEProdutoVerificarCampo(string produto, string tipo_estoque, string campo, string valor)
        {
            throw new NotImplementedException();
        }

        public void TabelaT_ESTOQUE_ITEMRegistroPaiEProdutoVerificarCampo(string produto, string campo, string valor)
        {
            throw new NotImplementedException();
        }

        public void TabelaT_ESTOQUERegistroPaiVerificarCampo(string campo, string valor)
        {
            throw new NotImplementedException();
        }

        public void TabelaT_ESTOQUE_LOGPedidoGeradoVerificarCampo(string produto, string operacao, string campo, string valor)
        {
            throw new NotImplementedException();
        }

        public void TabelaT_LOGPedidoGeradoEOperacaoVerificarCampo(string operacao, string campo, string valor)
        {
            throw new NotImplementedException();
        }

        public void TabelaT_PRODUTO_X_WMS_REGRA_CDFabricanteEProdutoVerificarCampo(string fabricante, string produto, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PRODUTO_X_WMS_REGRA_CD", "fabricante e produto", "verificar campos", campo, valor, this);

            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            gerenciamentoBanco.TabelaT_PRODUTO_X_WMS_REGRA_CDFabricanteEProdutoVerificarCampo(fabricante, produto, campo, valor);

        }
    }
}
