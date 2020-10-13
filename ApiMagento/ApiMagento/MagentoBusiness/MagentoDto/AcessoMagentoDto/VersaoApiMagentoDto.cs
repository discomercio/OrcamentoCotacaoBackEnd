using System;
using System.Collections.Generic;
using System.Text;

namespace MagentoBusiness.MagentoDto
{
    public class VersaoApiMagentoDto
    {
        public VersaoApiMagentoDto(string versaoApi, string build, string ambiente, string mensagem)
        {
            VersaoApi = versaoApi;
            Build = build;
            Ambiente = ambiente;
            Mensagem = mensagem;
        }

        public string VersaoApi { get; set; }
        public string Build { get; set; }
        public string Ambiente { get; set; }
        public string Mensagem { get; set; }
    }
}
