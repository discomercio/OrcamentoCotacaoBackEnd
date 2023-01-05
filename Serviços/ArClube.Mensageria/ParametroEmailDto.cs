namespace ArClube.Mensageria
{
    public sealed class ParametroEmailDto
    {
        public string ServerSMTP { get; set; }
        public int Port { get; set; }
        public MailKit.Security.SecureSocketOptions Options { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}