﻿using Newtonsoft.Json;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido
{
    static class CadastrarPrepedidoDados
    {
        public static PrePedidoUnisDto PrepedidoBase() => PrepedidoParceladoCartao1vez();

        public static PrePedidoUnisDto PrepedidoParceladoCartao1vez()
        {
            var ret = JsonConvert.DeserializeObject<PrePedidoUnisDto>(PrepedidoBaseParceladoCartao1vez);
            ret.TokenAcesso = Ambiente.ApiUnis.InjecaoDependencias.TokenAcessoApiUnis();
            return ret;
        }
        public static PrePedidoUnisDto PrepedidoParceladoAvista()
        {
            var ret = PrepedidoParceladoCartao1vez();
            ret.FormaPagtoCriacao.CustoFinancFornecTipoParcelamento = "AV";
            ret.FormaPagtoCriacao.Tipo_Parcelamento = 1;
            ret.FormaPagtoCriacao.Op_av_forma_pagto = "1";
            ret.FormaPagtoCriacao.CustoFinancFornecQtdeParcelas = 0;

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

        public static PrePedidoUnisDto PrepedidoBaseComEnderecoDeEntrega()
        {
            var ret = PrepedidoParceladoCartao1vez();
            ret.OutroEndereco = true;

            return ret;
        }

        public static PrePedidoUnisDto PrepedidoBaseClientePF()
        {
            return PrepedidoParceladoCartao1vez();
        }

        public static PrePedidoUnisDto PrepedidoBaseClientePJ()
        {
            var ret = PrepedidoParceladoCartao1vez();
            MudarParaClientePj(ret);
            return ret;
        }

        public static PrePedidoUnisDto PrepedidoBaseClientePJComEnderecoDeEntrega()
        {
            var ret = PrepedidoBaseComEnderecoDeEntrega();
            MudarParaClientePj(ret);
            return ret;
        }

        private static void MudarParaClientePj(PrePedidoUnisDto ret)
        {
            ret.EnderecoCadastralCliente.Endereco_tipo_pessoa = "PJ";
            ret.EnderecoCadastralCliente.Endereco_cnpj_cpf = "76297703000195";
            ret.EnderecoCadastralCliente.Endereco_contribuinte_icms_status = (byte)InfraBanco.Constantes.Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO;
            ret.EnderecoCadastralCliente.Endereco_produtor_rural_status = (byte)InfraBanco.Constantes.Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL;
            ret.EnderecoCadastralCliente.Endereco_contato = "Endereco_contato";

            ret.EnderecoCadastralCliente.Endereco_ddd_res = "";
            ret.EnderecoCadastralCliente.Endereco_tel_res = "";
            ret.EnderecoCadastralCliente.Endereco_ddd_cel = "";
            ret.EnderecoCadastralCliente.Endereco_tel_cel = "";
            ret.EnderecoCadastralCliente.Endereco_ddd_com = "11";
            ret.EnderecoCadastralCliente.Endereco_tel_com = "12345678";

            ret.Cnpj_Cpf = ret.EnderecoCadastralCliente.Endereco_cnpj_cpf;
        }

        private static readonly string PrepedidoBaseParceladoCartao1vez = @"
{
  ""TokenAcesso"": ""vai ser calculado dinamicamente"",
  ""Cnpj_Cpf"": ""35270445824"",
  ""Indicador_Orcamentista"": ""Konar"",
  ""EnderecoCadastralCliente"": {
    ""Endereco_logradouro"": ""Rua Professor Fábio Fanucchi"",
    ""Endereco_numero"": ""420"",
    ""Endereco_complemento"": """",
    ""Endereco_bairro"": ""Jardim São Paulo(Zona Norte)"",
    ""Endereco_cidade"": ""São Paulo"",
    ""Endereco_uf"": ""SP"",
    ""Endereco_cep"": ""02045080"",
    ""Endereco_email"": ""gabrie@gmail.com"",
    ""Endereco_email_xml"": """",
    ""Endereco_nome"": ""Gabriel Prada Teodoro"",
    ""Endereco_ddd_res"": ""11"",
    ""Endereco_tel_res"": ""12213333"",
    ""Endereco_ddd_com"": """",
    ""Endereco_tel_com"": """",
    ""Endereco_ramal_com"": """",
    ""Endereco_ddd_cel"": ""11"",
    ""Endereco_tel_cel"": ""981603313"",
    ""Endereco_ddd_com_2"": """",
    ""Endereco_tel_com_2"": """",
    ""Endereco_ramal_com_2"": """",
    ""Endereco_tipo_pessoa"": ""PF"",
    ""Endereco_cnpj_cpf"": ""35270445824"",
    ""Endereco_contribuinte_icms_status"": 0,
    ""Endereco_produtor_rural_status"": 1,
    ""Endereco_ie"": """",
    ""Endereco_rg"": """",
    ""Endereco_contato"": """"
  },
  ""OutroEndereco"": false,
""EnderecoEntrega"": {
    ""EndEtg_endereco"": ""Rua Francisco Pecoraro"",
    ""EndEtg_endereco_numero"": ""97"",
    ""EndEtg_endereco_complemento"": """",
    ""EndEtg_bairro"": ""Água Fria"",
    ""EndEtg_cidade"": ""São Paulo"",
    ""EndEtg_uf"": ""SP"",
    ""EndEtg_cep"": ""02408150"",
    ""EndEtg_cod_justificativa"": ""003"",
    ""EndEtg_email"": ""testeCad@Gabriel.com"",
    ""EndEtg_email_xml"": """",
    ""EndEtg_nome"": ""Gabriel"",
    ""EndEtg_ddd_res"": ""11"",
    ""EndEtg_tel_res"": ""25321634"",
    ""EndEtg_ddd_com"": """",
    ""EndEtg_tel_com"": """",
    ""EndEtg_ramal_com"": """",
    ""EndEtg_ddd_cel"": ""11"",
    ""EndEtg_tel_cel"": ""981603313"",
    ""EndEtg_ddd_com_2"": """",
    ""EndEtg_tel_com_2"": """",
    ""EndEtg_ramal_com_2"": """",
    ""EndEtg_tipo_pessoa"": ""PF"",
    ""EndEtg_cnpj_cpf"": ""35270445824"",
    ""EndEtg_contribuinte_icms_status"": 2,
    ""EndEtg_produtor_rural_status"": 2,
    ""EndEtg_ie"": ""244.355.757.113"",
    ""EndEtg_rg"": ""30448048-4""
  },
  ""ListaProdutos"": [
    {
      ""Fabricante"": ""003"",
      ""Produto"": ""003220"",
      ""Qtde"": 2,
      ""Desc_Dado"": 0,
      ""Preco_Venda"": 659.60,
      ""Preco_Fabricante"": 659.3,
      ""Preco_Lista"": 659.60,
      ""Preco_NF"": 694.05,
      ""CustoFinancFornecCoeficiente"": 1.0527,
      ""CustoFinancFornecPrecoListaBase"": 626.58
    },
    {
      ""Fabricante"": ""003"",
      ""Produto"": ""003221"",
      ""Qtde"": 2,
      ""Desc_Dado"": 0,
      ""Preco_Venda"": 989.40,
      ""Preco_Fabricante"": 988.95,
      ""Preco_Lista"": 989.40,
      ""Preco_NF"": 1041.07,
      ""CustoFinancFornecCoeficiente"": 1.0527,
      ""CustoFinancFornecPrecoListaBase"": 939.87
    }
  ],
  ""PermiteRAStatus"": true,
  ""ValorTotalDestePedidoComRA"": 3470.24,
  ""VlTotalDestePedido"": 3298,
  ""DetalhesPrepedido"": {
    ""St_Entrega_Imediata"": 2,
    ""PrevisaoEntregaData"": null,
    ""BemDeUso_Consumo"": 1,
    ""InstaladorInstala"": 2,
    ""Obs_1"": """"
  },
  ""FormaPagtoCriacao"": {
    ""Tipo_Parcelamento"": 2,
    ""C_pc_qtde"": 1,
    ""C_pc_valor"": 3470.24,   
    ""CustoFinancFornecTipoParcelamento"": ""SE"",
    ""CustoFinancFornecQtdeParcelas"": 1
  },
  ""Perc_Desagio_RA_Liquida"": 25
}"
;

    }
}
