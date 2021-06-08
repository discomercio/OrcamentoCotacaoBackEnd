using System;
using System.Collections.Generic;
using System.Text;

namespace TestesPrepedidoApiCadastrarPrepedido
{
    class ConfiguracaoTestes
    {
        private ConfiguracaoTestes()
        {

        }
        public static ConfiguracaoTestes LerConfiguracao()
        {
            //confirmamos a configuracao pelo console
            var ret = new ConfiguracaoTestes();

            //para testar contra o localhost
            ret.UrlPrePedidoApiCadastrarPrePedido = "http://localhost:60877/api/prepedido/cadastrarPrepedido";

            //ret.UrlPrePedidoApiCadastrarPrePedido = "http://its-appdev:9000/api/prepedido/cadastrarPrepedido";

            return ret;
        }
        public string UrlPrePedidoApiCadastrarPrePedido = "http://its-appdev:9000/api/prepedido/cadastrarPrepedido";
        public int NumeroExecucoes = 100;
        //para fazer o login
        public string apelido = "POLITÉCNIC";
        public string senha = "0x0000000000000053fd8335db6d2d97";

        public static void AlterarConfiguracaoAppSettingsPrepedidoApi()
        {

        }
    }
}
