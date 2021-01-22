using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Especificacao.Ambiente.PrepedidoApi.PrepedidoBusiness.Bll.PrepedidoApiBll.CadastrarPrepedido
{
    static class CadastrarPrepedidoDados
    {
        public static global::PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido.PrePedidoDto PrepedidoBase() => PrepedidoParceladoCartao1vez();

        public static global::PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido.PrePedidoDto PrepedidoParceladoCartao1vez()
        {
            var ret = JsonConvert.DeserializeObject<global::PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido.PrePedidoDto>(
                JsonConvert.SerializeObject(PrepedidoBaseParceladoCartao1vez()));
            return ret;
        }

        public static readonly string Usuario = "USUARIOPREPEDIDOAPI";

        public static global::PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido.PrePedidoDto PrepedidoParceladoAvista()
        {
            var ret = PrepedidoParceladoCartao1vez();
            ret.FormaPagtoCriacao.CustoFinancFornecTipoParcelamento = "AV";
            ret.FormaPagtoCriacao.Tipo_parcelamento = 1;
            ret.FormaPagtoCriacao.Op_av_forma_pagto = "1";
            ret.FormaPagtoCriacao.Qtde_Parcelas = 0;

            ret.ListaProdutos[0].CustoFinancFornecCoeficiente = 1;
            ret.ListaProdutos[0].Preco_Lista = 659.30m;
            ret.ListaProdutos[0].Preco_Venda = 652.71m;

            ret.ListaProdutos[1].CustoFinancFornecCoeficiente = 1;
            ret.ListaProdutos[1].Preco_Lista = 988.95m;
            ret.ListaProdutos[1].Preco_Venda = 979.06m;

            ret.VlTotalDestePedido = ret.ListaProdutos[0].Preco_Venda * ret.ListaProdutos[0].Qtde;
            ret.VlTotalDestePedido += ret.ListaProdutos[1].Preco_Venda * ret.ListaProdutos[1].Qtde;

            return ret;
        }

        public static global::PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido.PrePedidoDto PrepedidoBaseComEnderecoDeEntrega()
        {
            var ret = PrepedidoParceladoCartao1vez();
            ret.EnderecoEntrega.OutroEndereco = true;

            return ret;
        }

        public static global::PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido.PrePedidoDto PrepedidoBaseClientePF()
        {
            return PrepedidoParceladoCartao1vez();
        }

        public static global::PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido.PrePedidoDto PrepedidoBaseClientePJ()
        {
            var ret = PrepedidoParceladoCartao1vez();
            MudarParaClientePj(ret);
            return ret;
        }

        public static global::PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido.PrePedidoDto PrepedidoBaseClientePJComEnderecoDeEntrega()
        {
            var ret = PrepedidoBaseComEnderecoDeEntrega();
            MudarParaClientePj(ret);
            return ret;
        }

        private static void MudarParaClientePj(global::PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido.PrePedidoDto ret)
        {
            ret.EnderecoCadastroClientePrepedido.Endereco_tipo_pessoa = "PJ";
            ret.EnderecoCadastroClientePrepedido.Endereco_cnpj_cpf = "76297703000195";
            ret.EnderecoCadastroClientePrepedido.Endereco_contribuinte_icms_status = (byte)InfraBanco.Constantes.Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO;
            ret.EnderecoCadastroClientePrepedido.Endereco_produtor_rural_status = (byte)InfraBanco.Constantes.Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL;
            ret.EnderecoCadastroClientePrepedido.Endereco_contato = "Endereco_contato";

            ret.EnderecoCadastroClientePrepedido.Endereco_ddd_res = "";
            ret.EnderecoCadastroClientePrepedido.Endereco_tel_res = "";
            ret.EnderecoCadastroClientePrepedido.Endereco_ddd_cel = "";
            ret.EnderecoCadastroClientePrepedido.Endereco_tel_cel = "";
            ret.EnderecoCadastroClientePrepedido.Endereco_ddd_com = "11";
            ret.EnderecoCadastroClientePrepedido.Endereco_tel_com = "12345678";

            ret.DadosCliente.Cnpj_Cpf = ret.EnderecoCadastroClientePrepedido.Endereco_cnpj_cpf;
        }

        private static global::PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido.PrePedidoDto PrepedidoBaseParceladoCartao1vez()
        {
            Cliente.ClienteBll clienteBll = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<Cliente.ClienteBll>();

            //pegamos da uni!!
            var prePedidoUnis = global::Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedidoDados.PrepedidoBase();
            global::PrepedidoBusiness.Dto.ClienteCadastro.EnderecoCadastralClientePrepedidoDto endCadastralArclube =
                PrepedidoUnisBusiness.UnisDto.ClienteUnisDto.EnderecoCadastralClientePrepedidoUnisDto.EnderecoCadastralClientePrepedidoDtoDeEnderecoCadastralClientePrepedidoUnisDto(
                    prePedidoUnis.EnderecoCadastralCliente);

            List<global::PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido.PrepedidoProdutoDtoPrepedido> lstProdutosArclube
                = new List<global::PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido.PrepedidoProdutoDtoPrepedido>();
            prePedidoUnis.ListaProdutos.ForEach(x =>
            {
                var ret = PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto.PrePedidoProdutoPrePedidoUnisDto.
                    PrepedidoProdutoDtoPrepedidoDePrePedidoProdutoPrePedidoUnisDto(x,
                    Convert.ToInt16(prePedidoUnis.PermiteRAStatus));
                lstProdutosArclube.Add(ret);
            });

            global::PrepedidoBusiness.Dto.ClienteCadastro.ClienteCadastroDto clienteArclube = global::PrepedidoBusiness.Dto.ClienteCadastro.ClienteCadastroDto.ClienteCadastroDto_De_ClienteCadastroDados(clienteBll.BuscarCliente(prePedidoUnis.Cnpj_Cpf,
                prePedidoUnis.Indicador_Orcamentista).Result);

            //precisa para forçar a conversao do endereço de engtrega
            prePedidoUnis.OutroEndereco = true;
            var prePedidoDto = global::PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto.PrePedidoUnisDto.PrePedidoDtoDePrePedidoUnisDto(
                prePedidoUnis, endCadastralArclube, lstProdutosArclube, clienteArclube.DadosCliente);
            prePedidoUnis.OutroEndereco = false;
            return prePedidoDto;
        }

    }
}
