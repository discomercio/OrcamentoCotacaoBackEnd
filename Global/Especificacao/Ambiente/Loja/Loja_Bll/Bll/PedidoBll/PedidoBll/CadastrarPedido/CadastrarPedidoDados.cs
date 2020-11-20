using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido
{
    static class CadastrarPedidoDados
    {
        public static global::Loja.Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto PedidoBase(out string lojaUsuario, out string usuario, out bool vendedorExterno)
        {
            var ret = PedidoBaseComEnderecoDeEntrega(out lojaUsuario, out usuario, out vendedorExterno);
            ret.EnderecoEntrega = new global::Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro();
            ret.EnderecoEntrega.OutroEndereco = false;
            return ret;
        }
        public static global::Loja.Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto PedidoBaseComEnderecoDeEntrega(out string lojaUsuario, out string usuario, out bool vendedorExterno)
        {
            var ret = PedidoBaseaVista(out lojaUsuario, out usuario, out vendedorExterno);
            return ret;
        }
        public static global::Loja.Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto PedidoBaseClientePF(out string lojaUsuario, out string usuario, out bool vendedorExterno)
        {
            var ret = PedidoBaseaVista(out lojaUsuario, out usuario, out vendedorExterno);
            return ret;
        }
        public static global::Loja.Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto PedidoBaseClientePJ(out string lojaUsuario, out string usuario, out bool vendedorExterno)
        {
            var ret = PedidoBaseaVista(out lojaUsuario, out usuario, out vendedorExterno);
            ret.DadosCliente.Tipo = "PJ";
            ret.DadosCliente.Cnpj_Cpf = "76297703000195";
            ret.DadosCliente.Sexo = "";
            ret.DadosCliente.Rg = "";
            ret.DadosCliente.Nascimento = null;
            ret.DadosCliente.DddResidencial = "";
            ret.DadosCliente.TelefoneResidencial = "";
            ret.DadosCliente.DddCelular = "";
            ret.DadosCliente.Celular = "";
            ret.DadosCliente.DddComercial = "11";
            ret.DadosCliente.TelComercial = "12345678";
            ret.DadosCliente.Contato = "José da Silva";
            ret.DadosCliente.Contribuinte_Icms_Status = (byte)InfraBanco.Constantes.Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO;
            ret.DadosCliente.ProdutorRural = (byte)InfraBanco.Constantes.Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL;
            return ret;
        }

        public static global::Loja.Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto PedidoBaseaVista(out string lojaUsuario, out string usuario, out bool vendedorExterno)
        {
            var ret = PedidoBaseParceladoCartao1vez(out lojaUsuario, out usuario, out vendedorExterno);
            ret.FormaPagtoCriacao.CustoFinancFornecTipoParcelamento = "AV";
            ret.FormaPagtoCriacao.Tipo_parcelamento = 1;
            ret.FormaPagtoCriacao.Op_av_forma_pagto = "1";

            ret.ListaProdutos[0].CustoFinancFornecCoeficiente = 1;
            ret.ListaProdutos[0].Preco_Lista = 626.58m;
            ret.ListaProdutos[0].Preco_Venda = 626.58m;
            ret.ListaProdutos[0].Preco_NF = 626.58m;
            ret.ListaProdutos[0].CustoFinancFornecPrecoListaBase = 626.58m;

            ret.ListaProdutos[1].CustoFinancFornecCoeficiente = 1;
            ret.ListaProdutos[1].Preco_Lista = 939.87m;
            ret.ListaProdutos[1].Preco_Venda = 939.87m;
            ret.ListaProdutos[1].Preco_NF = 939.87m;
            ret.ListaProdutos[1].CustoFinancFornecPrecoListaBase = 939.87m;

            ret.VlTotalDestePedido = ret.ListaProdutos[0].Preco_Venda * ret.ListaProdutos[0].Qtde;
            ret.VlTotalDestePedido += ret.ListaProdutos[1].Preco_Venda * ret.ListaProdutos[1].Qtde;
            return ret;
        }
        public static global::Loja.Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto PedidoBaseParceladoCartao1vez(out string lojaUsuario, out string usuario, out bool vendedorExterno)
        {
            var ret = Testes.Utils.LerJson.LerArquivoEmbutido<global::Loja.Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto>(
                "Especificacao.Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedidoDados.json");
            lojaUsuario = "202";
            usuario = "USUARIOLOJA";
            vendedorExterno = true;
            return ret;
        }

    }
}
