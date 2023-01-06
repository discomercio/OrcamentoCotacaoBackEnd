using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;

namespace ArClube.Mensageria
{
    public sealed class EmailService : IDisposable
    {
        private readonly string _serverSMTP;
        private readonly int _port;
        private readonly SecureSocketOptions _options;
        private readonly string _username;
        private readonly string _password;
        private readonly SmtpClient _smtp;

        public EmailService(ParametroEmailDto parametroEmail)
        {
            this._serverSMTP = parametroEmail.ServerSMTP;
            this._port = parametroEmail.Port;
            this._options = parametroEmail.Options;
            this._username = parametroEmail.UserName;
            this._password = parametroEmail.Password;

            _smtp = new SmtpClient();
            _smtp.Connect(parametroEmail.ServerSMTP, parametroEmail.Port, parametroEmail.Options);
            _smtp.Authenticate(parametroEmail.UserName, parametroEmail.Password);
        }

        public bool Send(
            ILogger<Worker> logger,
            OrcamentoCotacaoEmailQueueDto torcamentoCotacaoEmailQueue,
            out string messageReturn)
        {
            messageReturn = string.Empty;

            try
            {
                // create message 
                using (var mimeMessageEmail = new MimeMessage())
                {
                    mimeMessageEmail.From.Add(MailboxAddress.Parse(torcamentoCotacaoEmailQueue.From));

                    if (!string.IsNullOrEmpty(torcamentoCotacaoEmailQueue.FromDisplayName))
                    {
                        mimeMessageEmail.From[0].Name = torcamentoCotacaoEmailQueue.FromDisplayName;
                    }

                    mimeMessageEmail.To.Add(MailboxAddress.Parse(torcamentoCotacaoEmailQueue.To));

                    if (torcamentoCotacaoEmailQueue.Cc != null)
                    {
                        foreach (var item in torcamentoCotacaoEmailQueue.Cc.Split(';'))
                        {
                            if (item.Length > 2)
                            {
                                mimeMessageEmail.Cc.Add(MailboxAddress.Parse(item));
                            }
                        }
                    }

                    if (torcamentoCotacaoEmailQueue.Bcc != null)
                    {
                        foreach (var item in torcamentoCotacaoEmailQueue.Bcc.Split(';'))
                        {
                            if (item.Length > 2)
                            {
                                mimeMessageEmail.Bcc.Add(MailboxAddress.Parse(item));
                            }
                        }
                    }

                    mimeMessageEmail.Subject = torcamentoCotacaoEmailQueue.Subject;
                    mimeMessageEmail.Body = new TextPart(TextFormat.Html) { Text = torcamentoCotacaoEmailQueue.Body };

                    logger.LogInformation(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - Trying to send Email #" + torcamentoCotacaoEmailQueue.Id + "From: [" + torcamentoCotacaoEmailQueue.From + "] [TO] [" + torcamentoCotacaoEmailQueue.To + "] [CC]: [" + torcamentoCotacaoEmailQueue.Cc + "]");

                    // send email
                    _smtp.Send(mimeMessageEmail);
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - Error: " + ex.Message);
                logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - Inner: " + ex.InnerException);
                messageReturn = ex.Message;

                return false;
            }
        }

        public void Dispose()
        {
            _smtp.Disconnect(true);
            _smtp.Dispose();
        }
    }
}