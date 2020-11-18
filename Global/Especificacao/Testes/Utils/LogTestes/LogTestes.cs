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

        public static void LogMensagemOperacao(string msg, Type getType)
        {
            string typeFullName = NomeTipo(getType);

            string msglog = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff") + " - " + typeFullName + " " + msg;
            GetInstance().Writer.WriteLine(msglog);
            GetInstance().Writer.Flush();
        }

        public static string NomeTipo(Type getType)
        {
            var typeFullName = getType.FullName ?? "sem tipo";
            //mal resolvido: temos um Especificacao na frente.... bom, tiramos!
            typeFullName = typeFullName.Replace("Especificacao.Ambiente.", "Ambiente.");
            typeFullName = typeFullName.Replace("Especificacao.Especificacao.", "Especificacao.");
            return typeFullName;
        }

        //todo: afazer: apaga resta rotina
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
