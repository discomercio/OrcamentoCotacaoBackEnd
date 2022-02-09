using InfraBanco.Constantes;
using Produto;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao.Passo20
{
    [Binding, Scope(Tag = "Especificacao.Pedido.Passo60.Gravacao.Passo20.multi_cd_regra_determina_tipo_pessoa")]
    public class Multi_Cd_Regra_Determina_Tipo_PessoaSteps
    {
        [Then(@"Chamar rotina MULTI_CD_REGRA_DETERMINA_TIPO_PESSOA tipo cliente = ""(.*)"", contribuinte = ""(.*)"", produtor rural = ""(.*)"" e resultado = ""(.*)""")]
        public void ThenChamarRotinaMULTI_CD_REGRA_DETERMINA_TIPO_PESSOATipoClienteContribuinteCOD_ST_CLIENTE_PRODUTOR_RURAL_SIMCOD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL(string tipo_cliente, string contribuinte, string? produtor, string resultado)
        {
            tipo_cliente = tipo_cliente switch
            {
                "ID_PF" => "PF",
                "ID_PJ" => "PJ",
                _ => ""
            };
            byte contribuinteStatus = contribuinte switch
            {
                "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL" => 0,
                "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO" => 1,
                "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM" => 2,
                "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO" => 3,
                _ => 0
            };
            byte produtorStatus = produtor switch
            {
                "COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL" => 0,
                "COD_ST_CLIENTE_PRODUTOR_RURAL_NAO" => 1,
                "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM" => 2,
                _ => 0
            };

            var sigla = UtilsProduto.MultiCdRegraDeterminaPessoa(tipo_cliente, (Constantes.ContribuinteICMS)contribuinteStatus,
                (Constantes.ProdutorRural)produtorStatus);

            switch (resultado)
            {
                case "COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA":
                    resultado = "PF";
                    break;
                case "COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL":
                    resultado = "PR";
                    break;
                case "COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE":
                    resultado = "PJC";
                    break;
                case "COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE":
                    resultado = "PJNC";
                    break;
                case "COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO":
                    resultado = "PJI";
                    break;
                default:
                    Assert.Equal("Resultado", $"{resultado} desconhecido");
                    break;
            }

            Assert.Equal(sigla, resultado);
        }

    }
}
