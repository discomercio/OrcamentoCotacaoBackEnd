using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            }
            else
            {
                Writer = new StreamWriter(new FileStream(DiretorioLogs + @"\" + ArquivoLog, FileMode.Append));
            }
        }
        private readonly StreamWriter Writer;

        public static void LogMensagemOperacao(string msg, Type getType)
        {
            string typeFullName = NomeTipo(getType);

            string msglog = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff") + " - " + typeFullName + " " + msg;
            lock (GetInstance().Writer)
            {
                GetInstance().Writer.WriteLine(msglog);
                GetInstance().Writer.Flush();
            }
        }

        public static string NomeTipo(Type getType)
        {
            var typeFullName = getType.FullName ?? "sem tipo";
            //mal resolvido: temos um Especificacao na frente.... bom, tiramos!
            typeFullName = typeFullName.Replace("Especificacao.Ambiente.", "Ambiente.");
            typeFullName = typeFullName.Replace("Especificacao.Especificacao.", "Especificacao.");
            return typeFullName;
        }

        public void LogMensagem(string msg)
        {
            string msglog = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff") + " - " + msg;
            lock (Writer)
            {
                Writer.WriteLine(msglog);
                Writer.Flush();
            }
        }

        public static void ErroNosTestes(string msg)
        {
            GetInstance().LogMensagem("ERRO: " + msg);
        }

        public void LogMemoria(string msg)
        {
            var memory = 0.0;
            using (Process proc = Process.GetCurrentProcess())
            {
                // The proc.PrivateMemorySize64 will returns the private memory usage in byte.
                // Would like to Convert it to Megabyte? divide it by 2^20
                memory = proc.PrivateMemorySize64 / (1024 * 1024);
            }
            msg += $" - Memória: {memory} megas";
            LogMensagem(msg);
        }

        public static void Stack()
        {
            var este = GetInstance();
            string msglog = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff") + " - " + $"StackTrace: '{Environment.StackTrace}'";
            lock (este.Writer)
            {
                este.Writer.WriteLine(msglog);
                este.Writer.Flush();
            }
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
