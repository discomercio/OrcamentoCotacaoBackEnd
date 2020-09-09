using Newtonsoft.Json;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesPrepedidoUnisBll
{
    static class DadosPrepedidoUnisBll
    {
        public static PrePedidoUnisDto PrepedidoParceladoCartao1vez()
        {
            var ret = JsonConvert.DeserializeObject<PrePedidoUnisDto>(PrepedidoBaseParceladoCartao1vez);
            ret.EnderecoCadastralCliente.Endereco_contato = "";
            return ret;
        }
        public static PrePedidoUnisDto PrepedidoParceladoAvista()
        {
            var ret = PrepedidoParceladoCartao1vez();
            ret.FormaPagtoCriacao.CustoFinancFornecTipoParcelamento = "AV";
            ret.FormaPagtoCriacao.Tipo_Parcelamento = 1;
            ret.FormaPagtoCriacao.Op_av_forma_pagto = "1";

            ret.ListaProdutos[0].CustoFinancFornecCoeficiente = 1;
            ret.ListaProdutos[0].NormalizacaoCampos_Preco_Lista = 659.30m;
            ret.ListaProdutos[0].Preco_Venda = 652.71m;

            ret.ListaProdutos[1].CustoFinancFornecCoeficiente = 1;
            ret.ListaProdutos[1].NormalizacaoCampos_Preco_Lista = 988.95m;
            ret.ListaProdutos[1].Preco_Venda = 979.06m;

            ret.NormalizacaoCampos_Vl_total = ret.ListaProdutos[0].Preco_Venda * ret.ListaProdutos[0].Qtde;
            ret.NormalizacaoCampos_Vl_total += ret.ListaProdutos[1].Preco_Venda * ret.ListaProdutos[1].Qtde;

            return ret;
        }

        public static PrePedidoUnisDto EnderecoCadastralPJ()
        {
            var ret = PrepedidoParceladoCartao1vez();
            ret.Cnpj_Cpf = "00371048000106";

            ret.EnderecoCadastralCliente.Endereco_email = "edsondasso@teste.com.br";
            ret.EnderecoCadastralCliente.Endereco_email_xml = "contato@teste.com.br";
            ret.EnderecoCadastralCliente.Endereco_ddd_res = "";
            ret.EnderecoCadastralCliente.Endereco_tel_res = "";
            ret.EnderecoCadastralCliente.Endereco_ddd_cel = "";
            ret.EnderecoCadastralCliente.Endereco_tel_cel = "";
            ret.EnderecoCadastralCliente.Endereco_ddd_com = "19";
            ret.EnderecoCadastralCliente.Endereco_tel_com = "983462361";
            ret.EnderecoCadastralCliente.Endereco_ramal_com = "12";
            ret.EnderecoCadastralCliente.Endereco_ddd_com_2 = "19";
            ret.EnderecoCadastralCliente.Endereco_tel_com_2 = "123456578";
            ret.EnderecoCadastralCliente.Endereco_ramal_com_2 = "12";
            ret.EnderecoCadastralCliente.Endereco_tipo_pessoa = "PJ";
            ret.EnderecoCadastralCliente.Endereco_cnpj_cpf = "00371048000106";
            ret.EnderecoCadastralCliente.Endereco_contribuinte_icms_status = 2;
            ret.EnderecoCadastralCliente.Endereco_produtor_rural_status = 0;
            ret.EnderecoCadastralCliente.Endereco_ie = "244.355.757.113";
            ret.EnderecoCadastralCliente.Endereco_contato = "EDSON";

            ret.FormaPagtoCriacao.C_pc_qtde = 0;
            ret.FormaPagtoCriacao.C_pc_valor = 0;

            return ret;
        }

        public static PrePedidoUnisDto PrepedidoParcelaUnica()
        {
            var ret = EnderecoCadastralPJ();

            ret.FormaPagtoCriacao.Op_pu_forma_pagto = "1";
            ret.FormaPagtoCriacao.CustoFinancFornecTipoParcelamento = "SE";
            ret.FormaPagtoCriacao.Tipo_Parcelamento = 5;
            ret.FormaPagtoCriacao.C_pu_valor = 3470.24m;
            ret.FormaPagtoCriacao.C_pu_vencto_apos = 20;

            return ret;
        }

        public static PrePedidoUnisDto PrepedidoParcelaCartao()
        {
            var ret = EnderecoCadastralPJ();

            ret.FormaPagtoCriacao.CustoFinancFornecTipoParcelamento = "SE";
            ret.FormaPagtoCriacao.Tipo_Parcelamento = 2;
            ret.FormaPagtoCriacao.C_pc_valor = 433.78m;
            ret.FormaPagtoCriacao.C_pc_qtde = 4;
            ret.FormaPagtoCriacao.CustoFinancFornecQtdeParcelas = 4;

            return ret;
        }

        public static PrePedidoUnisDto PrepedidoParcelaCartaoMaquineta()
        {
            var ret = EnderecoCadastralPJ();

            ret.FormaPagtoCriacao.CustoFinancFornecTipoParcelamento = "SE";
            ret.FormaPagtoCriacao.Tipo_Parcelamento = 6;
            ret.FormaPagtoCriacao.C_pc_maquineta_valor = 433.78m;
            ret.FormaPagtoCriacao.C_pc_maquineta_qtde = 4;
            ret.FormaPagtoCriacao.CustoFinancFornecQtdeParcelas = 4;

            return ret;
        }

        public static PrePedidoUnisDto PrepedidoPagtoComEntrada()
        {
            var ret = EnderecoCadastralPJ();

            ret.FormaPagtoCriacao.CustoFinancFornecTipoParcelamento = "CE";
            ret.FormaPagtoCriacao.CustoFinancFornecQtdeParcelas = 3;
            ret.FormaPagtoCriacao.Tipo_Parcelamento = 3;
            ret.FormaPagtoCriacao.Op_pce_entrada_forma_pagto = "2";
            ret.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto = "2";
            ret.FormaPagtoCriacao.C_pce_entrada_valor = 693.78m;
            ret.FormaPagtoCriacao.C_pce_prestacao_qtde = 3;
            ret.FormaPagtoCriacao.C_pce_prestacao_periodo = 15;
            ret.FormaPagtoCriacao.C_pce_prestacao_valor = 347.11m;

            return ret;
        }

        public static PrePedidoUnisDto PrepedidoEnderecoEntregaCompleto()
        {
            var ret = PrepedidoParceladoCartao1vez();
            ret.OutroEndereco = true;

            ret.EnderecoEntrega.EndEtg_endereco = "Rua Professor Fábio Fanucchi";
            ret.EnderecoEntrega.EndEtg_endereco_numero = "97";
            ret.EnderecoEntrega.EndEtg_endereco_complemento = "";
            ret.EnderecoEntrega.EndEtg_bairro = "Jardim São Paulo(Zona Norte)";
            ret.EnderecoEntrega.EndEtg_cidade = "São Paulo";
            ret.EnderecoEntrega.EndEtg_uf = "SP";
            ret.EnderecoEntrega.EndEtg_cep = "02045080";
            ret.EnderecoEntrega.EndEtg_cod_justificativa = "003";
            ret.EnderecoEntrega.EndEtg_email = "testeCad@Gabriel.com";
            ret.EnderecoEntrega.EndEtg_email_xml = "";
            ret.EnderecoEntrega.EndEtg_nome = "Vivian";
            ret.EnderecoEntrega.EndEtg_ddd_res = "11";
            ret.EnderecoEntrega.EndEtg_tel_res = "11111111";
            ret.EnderecoEntrega.EndEtg_ddd_com = "11";
            ret.EnderecoEntrega.EndEtg_tel_com = "25321634";
            ret.EnderecoEntrega.EndEtg_ramal_com = "32";
            ret.EnderecoEntrega.EndEtg_ddd_cel = "11";
            ret.EnderecoEntrega.EndEtg_tel_cel = "981603313";
            ret.EnderecoEntrega.EndEtg_ddd_com_2 = "11";
            ret.EnderecoEntrega.EndEtg_tel_com_2 = "85868586";
            ret.EnderecoEntrega.EndEtg_ramal_com_2 = "11";
            ret.EnderecoEntrega.EndEtg_tipo_pessoa = "PF";
            ret.EnderecoEntrega.EndEtg_cnpj_cpf = "29756194804";
            ret.EnderecoEntrega.EndEtg_contribuinte_icms_status = 2;
            ret.EnderecoEntrega.EndEtg_produtor_rural_status = 2;
            ret.EnderecoEntrega.EndEtg_ie = "244.355.757.113";
            ret.EnderecoEntrega.EndEtg_rg = "11997996-2";

            return ret;
        }

        private static string PrepedidoBaseParceladoCartao1vez = @"
{
  ""TokenAcesso"": ""eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJzdXBvcnRlIDEiLCJ1bmlxdWVfbmFtZSI6IkdVSUxIRVJNRSBHT01FUyBQSUVEQURFIC0gUyIsInJvbGUiOiJBcGlVbmlzIiwibmJmIjoxNTkzMDI5NzE5LCJleHAiOjE3NTA3MDk3MTksImlhdCI6MTU5MzAyOTcxOX0.Ofc7DSEKIDPSErYyaIGe-3VEsh9gE8nfuJwY8lCjN28"",
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
    ""Endereco_nome"": ""Teste Cadastro Empresa Unis 4"",
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
    ""Endereco_contato"": ""Gabriel""
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
    ""EndEtg_email"": """",
    ""EndEtg_email_xml"": """",
    ""EndEtg_nome"": """",
    ""EndEtg_ddd_res"": """",
    ""EndEtg_tel_res"": """",
    ""EndEtg_ddd_com"": """",
    ""EndEtg_tel_com"": """",
    ""EndEtg_ramal_com"": """",
    ""EndEtg_ddd_cel"": """",
    ""EndEtg_tel_cel"": """",
    ""EndEtg_ddd_com_2"": """",
    ""EndEtg_tel_com_2"": """",
    ""EndEtg_ramal_com_2"": """",
    ""EndEtg_tipo_pessoa"": """",
    ""EndEtg_cnpj_cpf"": """",
    ""EndEtg_contribuinte_icms_status"": 0,
    ""EndEtg_produtor_rural_status"": 0,
    ""EndEtg_ie"": """",
    ""EndEtg_rg"": """"
  },
  ""ListaProdutos"": [
    {
      ""Fabricante"": ""003"",
      ""Produto"": ""003220"",
      ""Qtde"": 2,
      ""Desc_Dado"": 1,
      ""Preco_Venda"": 687.11,
      ""NormalizacaoCampos_CustoFinancFornecPrecoListaBase"": 659.3,
      ""Preco_Lista"": 694.05,
      ""Preco_NF"": 694.05,
      ""CustoFinancFornecCoeficiente"": 1.0527,
      ""NormalizacaoCampos_Preco_Lista"": 694.05
    },
    {
      ""Fabricante"": ""003"",
      ""Produto"": ""003221"",
      ""Qtde"": 2,
      ""Desc_Dado"": 1,
      ""Preco_Venda"": 1030.66,
      ""NormalizacaoCampos_CustoFinancFornecPrecoListaBase"": 988.95,
      ""Preco_Lista"": 1041.07,
      ""Preco_NF"": 1041.07,
      ""CustoFinancFornecCoeficiente"": 1.0527,
      ""NormalizacaoCampos_Preco_Lista"": 1041.07
    }
  ],
  ""PermiteRAStatus"": true,
  ""NormalizacaoCampos_Vl_total_NF"": 3470.24,
  ""NormalizacaoCampos_Vl_total"": 3435.54,
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
}";

    }
}
