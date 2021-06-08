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
            //para rodar local
            //ret.UrlPrePedidoApiCadastrarPrePedido = "http://localhost:60877/api/prepedido/cadastrarPrepedido";
            //ret.UrlPrepedidoApiFazerLogin = "http://localhost:60877/api/acesso/fazerLogin";

            return ret;
        }
        public string UrlPrePedidoApiCadastrarPrePedido = "http://its-appdev:9000/api/prepedido/cadastrarPrepedido";
        public string UrlPrepedidoApiFazerLogin = "http://its-appdev:9000/api/acesso/fazerLogin";
        public int NumeroExecucoes = 100;
        //para fazer o login
        public string apelido = "POLITÉCNIC";
        public string senha = "0x0000000000000053fd8335db6d2d97";

    }
}
