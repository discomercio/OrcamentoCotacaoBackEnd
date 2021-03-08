using System;
using TechTalk.SpecFlow;

namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao.Passo20
{
    [Binding, Scope(Tag = "Especificacao.Pedido.Passo60.Gravacao.Passo20.multi_cd_regra_determina_tipo_pessoa")]
    public class Multi_Cd_Regra_Determina_Tipo_PessoaSteps
    {

        [Then(@"Chamar rotina MULTI_CD_REGRA_DETERMINA_TIPO_PESSOA tipo cliente = ""(.*)"", contribuinte = """"(.*)""COD_ST_CLIENTE_PRODUTOR_RURAL_SIM""(.*)""COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL""")]
        public void ThenChamarRotinaMULTI_CD_REGRA_DETERMINA_TIPO_PESSOATipoClienteContribuinteCOD_ST_CLIENTE_PRODUTOR_RURAL_SIMCOD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL(string p0, string p1, string p2)
        {

        }
    }
}
