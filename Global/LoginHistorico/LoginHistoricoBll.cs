using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;

namespace LoginHistorico
{
    public class LoginHistoricoBll : BaseBLL<TloginHistorico, TloginHistoricoFiltro>
    {
        private LoginHistoricoData _data { get; set; }

        public LoginHistoricoBll(ContextoBdProvider contextoBdProvider) : base(new LoginHistoricoData(contextoBdProvider))
        {
            _data = new LoginHistoricoData(contextoBdProvider);
        }

    }
}