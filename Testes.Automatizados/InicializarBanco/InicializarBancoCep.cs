using InfraBanco;
using System;
using System.Collections.Generic;
using System.Text;

namespace Testes.Automatizados.InicializarBanco
{
    public class InicializarBancoCep
    {
        private static bool _inicialziado = false;
        private readonly ContextoCepProvider contextoCepProvider;

        public InicializarBancoCep(InfraBanco.ContextoCepProvider contextoCepProvider)
        {
            this.contextoCepProvider = contextoCepProvider;

            if (!_inicialziado)
            {
                _inicialziado = true;
                Inicalizar();
            }
        }

        public static class DadosCep
        {
            public static string Cep = "04321001";
            public static string CepNaoExiste = "14321001";

            public static string Ufe_sg = "SP";
            public static string Ufe_sgNaoExiste = "XX";
        }

        private void Inicalizar()
        {
            var db = contextoCepProvider.GetContextoLeitura();

            //veja o Testes.Automatizados.InicializarBanco.ClienteNaoCadastradoJson
            db.LogLocalidades.Add(new InfraBanco.Modelos.LogLocalidade() { Cep_dig = "68912350", Ufe_sg = "AP", Loc_nosub = "", Loc_nu_sequencial = 1 });

            db.LogLocalidades.Add(new InfraBanco.Modelos.LogLocalidade() { Cep_dig = DadosCep.Cep, Ufe_sg = DadosCep.Ufe_sg, Loc_nosub = "", Loc_nu_sequencial = 2 });
            db.SaveChanges();
        }

    }
}
