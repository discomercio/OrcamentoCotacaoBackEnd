using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using System.IO;
using Microsoft.Extensions.Configuration;

#nullable enable

namespace Especificacao.Testes.Utils.ListaDependencias
{
    public class RegistroDependencias
    {
        private class TextoInstancia
        {
            public string? Texto;
            public object? Instancia;
        }

        //esta existe porque lista TODAS as especificacoes que tenham sido criadas
        //o primeiro níel é a implementação; o segundo é a especificação
        private static readonly Dictionary<string, List<TextoInstancia>> ambientesRegistrados = new Dictionary<string, List<TextoInstancia>>();

        //as que já foram verificadas
        private static readonly Dictionary<string, List<TextoInstancia>> ambientesImplementados = new Dictionary<string, List<TextoInstancia>>();
        private static readonly Dictionary<string, List<TextoInstancia>> ambientesEspecificados = new Dictionary<string, List<TextoInstancia>>();

        //aqui todas as mensagens de log
        private static readonly List<TextoInstancia> mensagensLog = new List<TextoInstancia>();
        public static void AdicionarMensagemLog(string msg, object instancia) => mensagensLog.Add(new TextoInstancia() { Texto = msg, Instancia = instancia });

        public static void AdicionarDependencia(string ambiente, object? instancia, string especificacao) => AdicionarDependenciaInterno(ambiente, instancia, especificacao, ambientesRegistrados);

        private static void AdicionarDependenciaInterno(string ambiente, object? instancia, string especificacao, Dictionary<string, List<TextoInstancia>> ambientes)
        {
            if (!ambientes.ContainsKey(ambiente))
                ambientes.Add(ambiente, new List<TextoInstancia>());
            if (!ambientes[ambiente].Where(r => r.Texto == especificacao && r.Instancia == instancia).Any())
                ambientes[ambiente].Add(new TextoInstancia() { Texto = especificacao, Instancia = instancia });
        }

        public static void GivenEspecificadoEm(string ambiente, string especificacao)
        {
            VerificarQueUsou(ambiente, especificacao);

            //registra que verificou
            AdicionarDependenciaInterno(ambiente, null, especificacao, ambientesEspecificados);
        }
        public static void GivenImplementadoEm(string ambiente, string especificacao)
        {
            VerificarQueUsou(ambiente, especificacao);
            AdicionarDependenciaInterno(ambiente, null, especificacao, ambientesImplementados);
        }

        public static void VerificarQueUsou(string ambiente, string especificacao)
        {
            var ambientes = ambientesRegistrados;
            if (!ambientes.ContainsKey(ambiente))
            {
                LogTestes.LogTestes.GetInstance().LogMensagem($"{ambiente}: implementacao nunca foi definida");
                Assert.Equal("", $"{ambiente}: implementacao nunca foi definida");
            }

            //para debug
            if (!ambientes[ambiente].Select(r => r.Texto).ToList().Contains(especificacao))
                LogTestes.LogTestes.GetInstance().LogMensagem($"Erro: VerificarQueUsou {especificacao} em {String.Join(",", ambientes[ambiente].Select(r => r.Texto).Distinct().ToList())} ");

            //este teste somente passa se executar todos os testes
            Assert.Contains(especificacao, ambientes[ambiente].Select(r => r.Texto).ToList());
        }

        public static void TodosVerificados()
        {
            LogTestes.LogTestes.GetInstance().LogMemoria("TodosVerificados inicio");

            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .AddJsonFile("appsettings.testes.json").Build();
            var configuracaoTestes = config.Get<ConfiguracaoTestes>();

            DumpMapa(ambientesRegistrados, false, configuracaoTestes.DiretorioLogs + @"\Mapa.txt");
            //estes são uteis para debug
            //DumpMapa(ambientesEspecificados, "ambientesEspecificados");
            //DumpMapa(ambientesImplementados, "ambientesImplementados");
            DumpMapaInvertido(ambientesRegistrados, configuracaoTestes.DiretorioLogs + @"\InvertidoMapa.txt");

            VerificarUmaLista(ambientesEspecificados);
            VerificarUmaLista(ambientesImplementados);

            LogTestes.LogTestes.GetInstance().LogMemoria("TodosVerificados fim");
        }

        public static void SalvarMapaComChamadas_Txt()
        {
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .AddJsonFile("appsettings.testes.json").Build();
            var configuracaoTestes = config.Get<ConfiguracaoTestes>();

            DumpMapa(ambientesRegistrados, true, configuracaoTestes.DiretorioLogs + @"\MapaComChamadas.txt");
        }
        public static void ApagarMapaComChamadas_Txt()
        {
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .AddJsonFile("appsettings.testes.json").Build();
            var configuracaoTestes = config.Get<ConfiguracaoTestes>();
            System.IO.File.Delete(configuracaoTestes.DiretorioLogs + @"\MapaComChamadas.txt");
        }

        private static void DumpMapaInvertido(Dictionary<string, List<TextoInstancia>> ambientes, string arquivo)
        {
            //invertemos o mapa: ao invés de primeiro a implementação e depois a especificação, fazemos ao contrário
            //nao fazemos o dump das mensagens em si
            Dictionary<string, List<TextoInstancia>> ambientesInvertidos = new Dictionary<string, List<TextoInstancia>>();
            foreach (var ambiente in ambientes.Keys.ToList().OrderBy(r => r))
            {
                var lista = (from r in ambientes[ambiente] group r by r.Texto into g select new TextoInstancia() { Texto = g.Key, Instancia = null }).Distinct().OrderBy(r => r.Texto);
                foreach (var especificacao in lista)
                {
                    if (!ambientesInvertidos.ContainsKey(especificacao.Texto ?? ""))
                        ambientesInvertidos.Add(especificacao.Texto ?? "", new List<TextoInstancia>());

                    if (!ambientesInvertidos[especificacao.Texto ?? ""].Select(r => r.Texto).ToList().Contains(ambiente))
                        ambientesInvertidos[especificacao.Texto ?? ""].Add(new TextoInstancia() { Texto = ambiente, Instancia = null });
                }
            }

            DumpMapa(ambientesInvertidos, false, arquivo);
        }

        private static void DumpMapa(Dictionary<string, List<TextoInstancia>> ambientes, bool detalhesChamadas, string arquivo)
        {
            using StreamWriter writerMapa = new StreamWriter(new FileStream(arquivo, FileMode.Create))
            {
                AutoFlush = false
            };

            writerMapa.Write("\r\n");
            DumpMapaItem(writerMapa, ambientes, ambientes, detalhesChamadas);
            writerMapa.Write("\r\n");
            writerMapa.Write("\r\n");
        }
        private static void DumpMapaItem(StreamWriter writerMapa, Dictionary<string, List<TextoInstancia>> ambientesFiltrados,
            Dictionary<string, List<TextoInstancia>> ambientesTodos,
            bool detalhesChamadas, int identacao = 0)
        {
            if (identacao > 20)
            {
                writerMapa.Write("LIMITE DE RECURSÃO ATINGIDO!");
                return;
            }

            //registrar todo o mapa no log
            foreach (var ambiente in ambientesFiltrados.Keys.ToList().Distinct().OrderBy(r => r))
            {
                if (identacao == 0)
                    writerMapa.Write("\r\n");

                writerMapa.Write("\r\n" + new string('\t', identacao) + ambiente);

                var lista = ambientesFiltrados[ambiente].OrderBy(r => r.Texto);
                if (!detalhesChamadas)
                    lista = (from r in lista group r by r.Texto into g select new TextoInstancia() { Texto = g.Key, Instancia = null }).Distinct().OrderBy(r => r.Texto);
                foreach (var especificacao in lista)
                {
                    if (!ambientesTodos.ContainsKey(especificacao.Texto ?? ""))
                        writerMapa.Write("\r\n\t" + new string('\t', identacao) + especificacao.Texto);

                    if (detalhesChamadas)
                    {
                        foreach (var i in mensagensLog.Where(r => r.Instancia == especificacao.Instancia))
                            writerMapa.Write("\r\n\t\t" + new string('\t', identacao) + "--" + i.Texto);
                    }

                    if (ambientesTodos.ContainsKey(especificacao.Texto ?? ""))
                    {
                        var filtrado = new Dictionary<string, List<TextoInstancia>>
                        {
                            { especificacao.Texto ?? "", ambientesTodos[especificacao.Texto ?? ""] }
                        };
                        DumpMapaItem(writerMapa, filtrado, ambientesTodos, detalhesChamadas, identacao + 1);
                    }
                }
            }
        }

        private static void VerificarUmaLista(Dictionary<string, List<TextoInstancia>> ambientesVerificados)
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
                var registrados = ambientesRegistrados[ambiente].Select(r => r.Texto).Distinct().ToList();
                var verificados = ambientesVerificados[ambiente].Select(r => r.Texto).Distinct().ToList();
                registrados.Sort();
                verificados.Sort();

                //só apra facilitar o debug
                if (registrados.Count != verificados.Count)
                {
                    LogTestes.LogTestes.GetInstance().LogMensagem($"Erro: VerificarUmaLista {String.Join(", ", registrados)} diferente de {String.Join(",", verificados)} ");

                    foreach (var registrado in registrados)
                    {
                        if (!verificados.Contains(registrado))
                        {
                            LogTestes.LogTestes.GetInstance().LogMensagem($"Erro: !verificados.Contains(registrado) {registrado} em {String.Join(",", verificados)} ");
                            Assert.Equal("", $"{registrado} Erro: !verificados.Contains(registrado) ");
                        }
                    }
                    foreach (var verificado in verificados)
                    {
                        if (!registrados.Contains(verificado))
                        {
                            LogTestes.LogTestes.GetInstance().LogMensagem($"Erro: !registrados.Contains(verificado) {verificado} em {String.Join(",", registrados)} ");
                            Assert.Equal("", $"{verificado} Erro: !verificados.Contains(registrado) ");
                        }
                    }
                }


                //agora a verificaçaõ de verdade
                Assert.Equal(registrados, verificados);
            }
        }
    }
}
