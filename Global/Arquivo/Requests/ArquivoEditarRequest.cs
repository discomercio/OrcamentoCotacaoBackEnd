namespace Arquivo.Requests
{
    public sealed class ArquivoEditarRequest
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}