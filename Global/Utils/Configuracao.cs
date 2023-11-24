namespace UtilsGlobais
{
    public class Configuracao
    {
        public string SegredoToken { get; set; }
        public int ValidadeTokenMinutos { get; set; }
        public bool TRATAMENTO_ACESSO_CONCORRENTE_LOCK_EXCLUSIVO_MANUAL_HABILITADO { get; set; } = true;
        public bool VerificarPrepedidoRepetido { get; set; } = true;
        public string BloqueioUsuarioLoginAmbiente { get; set; }
        public bool GerarLogProcessoAutomatizado { get; set; } = true;
        public bool CadastrarEControlarTempoRespostaEndpoint { get; set; }
    }
}