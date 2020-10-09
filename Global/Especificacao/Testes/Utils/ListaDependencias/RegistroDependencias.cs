using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace Especificacao.Testes.Utils.ListaDependencias
{
    public class RegistroDependencias
    {
        //esta existe porque lista TODAS as especificacoes que tenham sido criadas
        //o primeiro níel é a implementação; o segundo é a especificação
        private static readonly Dictionary<string, List<string>> ambientesRegistrados = new Dictionary<string, List<string>>();

        //as que já foram verificadas
        private static readonly Dictionary<string, List<string>> ambientesImplementados = new Dictionary<string, List<string>>();
        private static readonly Dictionary<string, List<string>> ambientesEspecificados = new Dictionary<string, List<string>>();

        public static void AdicionarDependencia(string ambiente, string especificacao) => AdicionarDependenciaInterno(ambiente, especificacao, ambientesRegistrados);

        private static void AdicionarDependenciaInterno(string ambiente, string especificacao, Dictionary<string, List<string>> ambientes)
        {
            if (!ambientes.ContainsKey(ambiente))
                ambientes.Add(ambiente, new List<string>());
            if (!ambientes[ambiente].Contains(especificacao))
                ambientes[ambiente].Add(especificacao);
        }

        public static void GivenEspecificadoEm(string ambiente, string especificacao)
        {
            VerificarQueUsou(ambiente, especificacao);

            //registra que verificou
            AdicionarDependenciaInterno(ambiente, especificacao, ambientesEspecificados);
        }
        public static void GivenImplementadoEm(string ambiente, string especificacao)
        {
            VerificarQueUsou(ambiente, especificacao);
            AdicionarDependenciaInterno(ambiente, especificacao, ambientesImplementados);
        }

        public static void VerificarQueUsou(string ambiente, string especificacao)
        {
            var ambientes = ambientesRegistrados;
            if (!ambientes.ContainsKey(ambiente))
                Assert.Equal("", $"{ambiente}: implementacao nunca foi definida");

            //este teste somente passa se executar todos os testes
            Assert.Contains(especificacao, ambientes[ambiente]);
        }

        public static void TodosVerificados()
        {
            DumpMapa(ambientesRegistrados, "ambientesRegistrados");
            //estes são uteis para debug
            DumpMapa(ambientesEspecificados, "ambientesEspecificados");
            DumpMapa(ambientesImplementados, "ambientesImplementados");


            DumpMapaInvertido(ambientesRegistrados, "ambientesRegistrados invertido");

            VerificarUmaLista(ambientesEspecificados);
            VerificarUmaLista(ambientesImplementados);
        }

        private static void DumpMapaInvertido(Dictionary<string, List<string>> ambientes, string msg)
        {
            //invertemos o mapa: ao invés de primeiro a implementação e depois a especificação, fazemos ao contrário
            Dictionary<string, List<string>> ambientesInvertidos = new Dictionary<string, List<string>>();
            foreach (var ambiente in ambientes.Keys.ToList().OrderBy(r => r))
            {
                foreach (var especificacao in ambientes[ambiente].OrderBy(r => r))
                {
                    if (!ambientesInvertidos.ContainsKey(especificacao))
                        ambientesInvertidos.Add(especificacao, new List<string>());

                    if (!ambientesInvertidos[especificacao].Contains(ambiente))
                        ambientesInvertidos[especificacao].Add(ambiente);
                }
            }

            DumpMapa(ambientesInvertidos, msg);
        }

        private static void DumpMapa(Dictionary<string, List<string>> ambientes, string msg)
        {
            //registrar todo o mapa no log
            msg += "\r\n";
            foreach (var ambiente in ambientes.Keys.ToList().OrderBy(r => r))
            {
                msg += "\r\n";
                msg += "\r\n" + ambiente;
                foreach (var especificacao in ambientes[ambiente].OrderBy(r => r))
                    msg += "\r\n\t" + especificacao;
            }
            msg += "\r\n";
            msg += "\r\n";
            LogTestes.Log(msg);
        }

        private static void VerificarUmaLista(Dictionary<string, List<string>> ambientesVerificados)
        {
            var listaregistrados = ambientesRegistrados.Keys.ToList();
            var listaverificados = ambientesVerificados.Keys.ToList();
            listaregistrados.Sort();
            listaverificados.Sort();
            //só apra facilitar o debug
            if (listaregistrados.Count != listaverificados.Count)
                Assert.Equal(listaregistrados, listaverificados);

            //agora a verificaçaõ de verdade
            Assert.Equal(listaregistrados, listaverificados);

            foreach (var ambiente in listaverificados)
            {
                var registrados = ambientesRegistrados[ambiente];
                var verificados = ambientesVerificados[ambiente];
                registrados.Sort();
                verificados.Sort();

                //só apra facilitar o debug
                if (registrados.Count != verificados.Count)
                    Assert.Equal(registrados, verificados);

                //agora a verificaçaõ de verdade
                Assert.Equal(registrados, verificados);
            }
        }
    }
}
