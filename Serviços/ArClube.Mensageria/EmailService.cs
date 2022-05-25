using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit.Text;
using Microsoft.Extensions.Logging;
using InfraBanco.Modelos;

namespace ArClube.Mensageria
{
    public class EmailService : IDisposable
    {
        private readonly string serverSMTP;
        private readonly int port;
        private readonly SecureSocketOptions options;
        private readonly string username;
        private readonly string password;
        private readonly SmtpClient smtp;

        public EmailService(string serverSMTP, int port, SecureSocketOptions options, string username, string password)
        {
            this.serverSMTP = serverSMTP;
            this.port = port;
            this.options = options;
            this.username = username;
            this.password = password;

            smtp = new SmtpClient();
            smtp.Connect(serverSMTP, port, options);
            smtp.Authenticate(username, password);
        }

        public void Dispose()
        {
            smtp.Disconnect(true);
        }

        public bool Send(ILogger<Worker> _logger, TorcamentoCotacaoEmailQueue _email, out string msgRet)
        {
            msgRet = null;
            try
            {
                // create message 
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_email.From));
                if (!string.IsNullOrEmpty(_email.FromDisplayName))
                {
                    email.From[0].Name = _email.FromDisplayName;
                }
                email.To.Add(MailboxAddress.Parse(_email.To));
                if (_email.Cc != null)
                {
                    foreach (var item in _email.Cc.Split(';'))
                    {
                        if (item.Length > 2)
                            email.Cc.Add(MailboxAddress.Parse(item));
                    }
                }

                if (_email.Bcc != null)
                {
                    foreach (var item in _email.Bcc.Split(';'))
                    {
                        if (item.Length > 2)
                            email.Bcc.Add(MailboxAddress.Parse(item));
                    }
                }
                email.Subject = _email.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = _email.Body };
                _logger.LogInformation(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - Trying to send Email #" + _email.Id + "From: [" + _email.From + "] [TO] [" + _email.To + "] [CC]: [" + _email.Cc + "]");
                // send email
                smtp.Send(email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - Error: " + ex.Message);
                _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - Inner: " + ex.InnerException);
                msgRet = ex.Message;
                return false;
            }
        }
    }
}
