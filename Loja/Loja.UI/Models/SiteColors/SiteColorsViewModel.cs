using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace Loja.UI.Models.SiteColors
{
    public class SiteColorsViewModel
    {
        public SiteColorsViewModel(string sessionCtrlInfo, string pagina)
        {
            SessionCtrlInfo = sessionCtrlInfo;
            Pagina = pagina;
        }

        public string Url()
        {
            BaseSite = @"http://localhost:9010/loja/";
            BaseSite = @"http://its-appdev:9010/loja/";
            //return BaseSite + Pagina + "?SessionCtrlInfo=0x" + SessionCtrlInfo;
            return $@"{BaseSite}resumo.asp?SessionCtrlInfo=0x" + SessionCtrlInfo;
        }

        public string BaseSite { get; private set; } = @"http://its-appdev:9010/loja/";
        public string SessionCtrlInfo { get; }
        public string Pagina { get; }
    }
}
