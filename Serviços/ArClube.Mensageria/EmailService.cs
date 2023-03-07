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
        private SmtpClient _smtp;
        private readonly bool _shouldGenerateLog;

        public EmailService(ParametroEmailDto parametroEmail, bool shouldGenerateLog)
        {
            this._serverSMTP = parametroEmail.ServerSMTP;
            this._port = parametroEmail.Port;
            this._options = parametroEmail.Options;
            this._username = parametroEmail.UserName;
            this._password = parametroEmail.Password;
            this._shouldGenerateLog = shouldGenerateLog;
        }

        public async Task<Tuple<bool, string>> Send(
            ILogger<Worker> logger,
            OrcamentoCotacaoEmailQueueDto torcamentoCotacaoEmailQueue)
        {
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

                    if (this._shouldGenerateLog)
                    {
                        logger.LogInformation(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - Trying to send Email #" + torcamentoCotacaoEmailQueue.Id + "From: [" + torcamentoCotacaoEmailQueue.From + "] [TO] [" + torcamentoCotacaoEmailQueue.To + "] [CC]: [" + torcamentoCotacaoEmailQueue.Cc + "]");
                    }

                    await this.SmtpAuthenticateAsync();
                    await _smtp.SendAsync(mimeMessageEmail);
                }
                
                return new Tuple<bool, string>(true, string.Empty);
            }
            catch (Exception ex)
            {
                
                if (this._shouldGenerateLog)
                {
                    logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - Error: " + ex.Message);
                    logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - Inner: " + ex.InnerException);
                }

                return new Tuple<bool, string>(false, ex.Message);
            }
        }

        private async Task SmtpAuthenticateAsync()
        {
            this._smtp = new SmtpClient();

            await this._smtp.ConnectAsync(this._serverSMTP, this._port, this._options);

            if (!this._smtp.IsAuthenticated)
            {
                await _smtp.AuthenticateAsync(this._username, this._password);
            }
        }

        public void Dispose()
        {
            _smtp.Disconnect(true);
            _smtp.Dispose();
        }
    }
}