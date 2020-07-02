using Newtonsoft.Json;
using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Testes.Automatizados.InicializarBanco
{
    public static class InicializarClienteDados
    {
        public static ClienteCadastroUnisDto ClienteNaoCadastrado()
        {
            var ret = JsonConvert.DeserializeObject<ClienteCadastroUnisDto>(ClienteNaoCadastradoJson);
            return ret;
        }

        private static string ClienteNaoCadastradoJson = @"
{
  ""TokenAcesso"": ""eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJzdXBvcnRlIDEiLCJ1bmlxdWVfbmFtZSI6IkdVSUxIRVJNRSBHT01FUyBQSUVEQURFIC0gUyIsInJvbGUiOiJBcGlVbmlzIiwibmJmIjoxNTkzMDI5NzE5LCJleHAiOjE3NTA3MDk3MTksImlhdCI6MTU5MzAyOTcxOX0.Ofc7DSEKIDPSErYyaIGe-3VEsh9gE8nfuJwY8lCjN28"",
  ""DadosCliente"": {
    ""Indicador_Orcamentista"": ""Konar"",
    ""Cnpj_Cpf"": ""25.326.265/0001-05"",
    ""Rg"": null,
    ""Ie"": ""037784242"",
    ""Contribuinte_Icms_Status"": 2,
    ""Tipo"": ""PJ"",
    ""Observacao_Filiacao"": """",
    ""Nascimento"": null,
    ""Sexo"": null,
    ""Nome"": ""Teste Cadastro Empresa Unis 4"",
    ""ProdutorRural"": 0,
    ""Endereco"": ""teste"",
    ""Numero"": ""97"",
    ""Complemento"": """",
    ""Bairro"": ""teste"",
    ""Cidade"": ""Abacate da Pedreira"",
    ""Uf"": ""AP"",
    ""Cep"": ""68912350"",
    ""DddResidencial"": null,
    ""TelefoneResidencial"": null,
    ""DddComercial"": ""19"",
    ""TelComercial"": ""22859635"",
    ""Ramal"": """",
    ""DddCelular"": """",
    ""Celular"": """",
    ""TelComercial2"": """",
    ""DddComercial2"": """",
    ""Ramal2"": """",
    ""Email"": ""testecadastro@unis.com"",
    ""EmailXml"": """",
    ""Contato"": ""Gabriel""
  },
  ""RefBancaria"": [
    {
      ""Banco"": ""001"",
      ""Agencia"": ""003"",
      ""Conta"": ""12345-6"",
      ""Ddd"": ""19"",
      ""Telefone"": ""55668899"",
      ""Contato"": ""Teste"",
      ""Ordem"": 1
    }
  ],
  ""RefComercial"": [
    {
      ""Nome_Empresa"": ""Teste"",
      ""Contato"": ""Teste"",
      ""Ddd"": """",
      ""Telefone"": """",
      ""Ordem"": 1
    }
  ]
}";


    }
}
