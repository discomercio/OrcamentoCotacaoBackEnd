using System;
using System.Collections.Generic;
using System.Text;

namespace TestesApiMagentoCadastrarPedido
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
            ret.UrlApiMagentoCadastrarPedido = "https://localhost:44389/api/pedidoMagento/cadastrarPedido";

            return ret;
        }
        public string UrlApiMagentoCadastrarPedido = "http://its-appdev:9003/api/pedidoMagento/cadastrarPedido";
        public int NumeroExecucoes = 10;
    }
}
