using InfraBanco;
using InfraBanco.Constantes;
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

        public void TclientesApagar()
        {
            using (var db = contextoBdProvider.GetContextoGravacaoParaUsing())
            {
                foreach (var c in db.Tclientes)
                    db.Tclientes.Remove(c);

                db.SaveChanges();
            }
        }

        private void Inicalizar()
        {
            using (var db = contextoBdProvider.GetContextoGravacaoParaUsing())
            {
                db.TorcamentistaEindicadors.Add(new InfraBanco.Modelos.TorcamentistaEindicador()
                {
                    Apelido = Dados.Orcamentista.Apelido
                });

                db.Tcontroles.Add(new InfraBanco.Modelos.Tcontrole() { Id_Nsu = Constantes.NSU_CADASTRO_CLIENTES, Nsu = "000000645506" });

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
