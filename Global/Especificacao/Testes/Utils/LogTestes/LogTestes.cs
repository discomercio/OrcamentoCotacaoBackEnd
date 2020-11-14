using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Especificacao.Testes.Utils.LogTestes
{
    public class LogTestes : IDisposable
    {
        public static LogTestes GetInstance()
        {
            if (_singleton == null)
                _singleton = new LogTestes();
            return _singleton;
        }
        public static LogTestes? _singleton = null;

        private LogTestes()
        {
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .AddJsonFile("appsettings.testes.json").Build();
            var configuracaoTestes = config.Get<ConfiguracaoTestes>();

            var DiretorioLogs = configuracaoTestes.DiretorioLogs;
            var ArquivoLog = configuracaoTestes.ArquivoLog;
            if (!Directory.Exists(DiretorioLogs))
                Directory.CreateDirectory(DiretorioLogs);
            if (configuracaoTestes.SubstituirArquivos)
            {
                Writer = new StreamWriter(new FileStream(DiretorioLogs + @"\" + ArquivoLog, FileMode.Create));
                WriterMapa = new StreamWriter(new FileStream(DiretorioLogs + @"\" + configuracaoTestes.ArquivoMapa, FileMode.Create));
            }
            else
            {
                Writer = new StreamWriter(new FileStream(DiretorioLogs + @"\" + ArquivoLog, FileMode.Append));
                WriterMapa = new StreamWriter(new FileStream(DiretorioLogs + @"\" + configuracaoTestes.ArquivoMapa, FileMode.Append));
            }
        }
        private readonly StreamWriter Writer;
        private readonly StreamWriter WriterMapa;

        public void Mapa(string? msg)
        {
            msg ??= "vazio";
            WriterMapa.WriteLine(msg);
            WriterMapa.Flush();
        }

        public static void Log(string? msg)
        {
            msg ??= "vazio";
            GetInstance().LogMensagem(msg);
        }

        public void LogMensagem(string msg)
        {
            string msglog = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff") + " - " + msg;
            Writer.WriteLine(msglog);
            Writer.Flush();
        }

        public static void Stack()
        {
            var este = GetInstance();
            string msglog = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff") + " - " + $"StackTrace: '{Environment.StackTrace}'";
            este.Writer.WriteLine(msglog);
            este.Writer.Flush();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Writer.Dispose();
                    WriterMapa.Dispose();
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion

    }
}
