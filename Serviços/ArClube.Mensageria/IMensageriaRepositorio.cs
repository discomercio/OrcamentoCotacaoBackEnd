namespace ArClube.Mensageria
{
    public interface IMensageriaRepositorio
    {
        Task<List<UnidadeNegocioParametroDto>> ObterUnidadeNeogocioParametrosAsync();
        Task<List<OrcamentoCotacaoEmailQueueDto>> ObterOrcamentoCotacaoEmailsQueueAsync(int? recordsPerPage);
        Task<bool> AtualizarOrcamentoCotacaoEmailsQueue(OrcamentoCotacaoEmailQueueDto orcamentoCotacaoEmailQueue);
        Task<int> ObterParametroPorId(string id);
    }
}