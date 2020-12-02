using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace Loja.UI.Models.SiteColors
{
    public class SiteColorsViewModel
    {
        public SiteColorsViewModel(string sessionCtrlInfo, string pagina, Bll.Util.Configuracao configuracao)
        {
            SessionCtrlInfo = sessionCtrlInfo;
            Pagina = pagina;
            BaseSite = configuracao.Diretorios.RaizSiteColorsLoja;
        }

        public string Url()
        {
            var ret = $@"{BaseSite}/{Pagina}";
            if (ret.Contains("?"))
                ret += "&";
            else
                ret += "?";
            ret += "SessionCtrlInfo=0x" + SessionCtrlInfo + "&OrigemSolicitacao=LojaMvc";
            return ret;
        }

        public string BaseSite { get; private set; }
        public string SessionCtrlInfo { get; }
        public string Pagina { get; }
    }
}
