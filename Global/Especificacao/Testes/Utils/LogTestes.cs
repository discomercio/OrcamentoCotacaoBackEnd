using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Especificacao.Testes.Utils
{
    public class LogTestes : IDisposable
    {
        public static LogTestes GetInstance()
        {
            if (_singleton == null)
                _singleton = new LogTestes();
            return _singleton;
        }
        public static LogTestes _singleton = null;

        private LogTestes()
        {
            if (!Directory.Exists(DiretorioLog))
                Directory.CreateDirectory(DiretorioLog);
            Writer = new StreamWriter(new FileStream(DiretorioLog + @"\" + CaminhoLog, FileMode.Append));
        }
        private static readonly string CaminhoLog = @"log.txt";
        private static readonly string DiretorioLog = @"c:\temp\arclube_testes_log";
        private StreamWriter Writer;

        public void Log(string msg)
        {
            string msglog = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff") + " - " + msg;
            Writer.WriteLine(msglog);
            Writer.Flush();
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
