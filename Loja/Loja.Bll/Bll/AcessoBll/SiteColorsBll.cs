using InfraBanco;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;
using System.Threading.Tasks;

namespace Loja.Bll.Bll.AcessoBll
{
    public class SiteColorsBll
    {
        private readonly ContextoBdProvider contextoBdProvider;

        public SiteColorsBll(InfraBanco.ContextoBdProvider contextoBdProvider)
        {
            this.contextoBdProvider = contextoBdProvider;
        }
        public async Task<string> MontaSessionCtrlInfo(UsuarioLogado usuario)
        {
            return UtilsGlobais.SenhaBll.MontaSessionCtrlInfo(
                strUsuario: usuario.Usuario_atual.Trim(),
                strModulo: Constantes.Constantes.SESSION_CTRL_MODULO_LOJA,
                strLoja: usuario.Loja_atual_id.Trim(),
                strTicket: (await usuario.SessionCtrlTicket(contextoBdProvider)) ?? "",
                dtLogon: await usuario.SessionCtrlDtHrLogon(contextoBdProvider),
                dtUltAtividade: DateTime.Now,
                fatorCriptografia: Constantes.Constantes.FATOR_CRIPTO_SESSION_CTRL);
        }
    }
}
