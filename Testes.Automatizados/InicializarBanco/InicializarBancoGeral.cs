using InfraBanco;
using System;
using System.Collections.Generic;
using System.Text;

namespace Testes.Automatizados.InicializarBanco
{
    public class InicializarBancoGeral
    {
        private static bool _inicialziado = false;
        private readonly ContextoBdProvider contextoBdProvider;
        private readonly Testes.Automatizados.InicializarBanco.InicializarBancoCep inicializarCep;

        public InicializarBancoGeral(InfraBanco.ContextoBdProvider contextoBdProvider, Testes.Automatizados.InicializarBanco.InicializarBancoCep inicializarCep)
        {
            this.contextoBdProvider = contextoBdProvider;
            this.inicializarCep = inicializarCep;
            if (!_inicialziado)
            {
                _inicialziado = true;
                Inicalizar();
            }
        }


        private void Inicalizar()
        {
            using (var db = contextoBdProvider.GetContextoGravacaoParaUsing())
            {
                db.TorcamentistaEindicadors.Add(new InfraBanco.Modelos.TorcamentistaEindicador() {
                    Apelido = Dados.Orcamentista.Apelido
                });
                db.SaveChanges();
            }
        }

        static public class Dados
        {
            static public class Orcamentista
            {
                public static string Apelido = "Konar";
                public static string ApelidoNaoExiste = "XXX";
            }
        }
    }
}
