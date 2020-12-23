using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xunit;

namespace Especificacao.Testes.Utils
{
    public static class LerJson
    {
        public static T LerArquivoEmbutido<T>(string nomeArquivo)
        {
            Stream? stream;
            StreamReader reader;
            string texto;
            stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(nomeArquivo);
            if (stream == null)
            {
                Testes.Utils.LogTestes.LogTestes.GetInstance().LogMensagem($"LerArquivoEmbutido: {nomeArquivo}" + $"StackTrace: '{Environment.StackTrace}'");
                Assert.Equal("", nomeArquivo + $"StackTrace: '{Environment.StackTrace}'");
                throw new NullReferenceException(nomeArquivo);
            }
            reader = new StreamReader(stream);
            texto = reader.ReadToEnd();
            var json = JsonConvert.DeserializeObject<T>(texto);
            return json;
        }
    }
}
