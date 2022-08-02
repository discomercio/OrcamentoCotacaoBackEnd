namespace UtilsGlobais.Parametros
{
    public class ParametroOrcamentoCotacaoBll
    {
        private ParametroOrcamentoCotacaoData _data { get; set; }

        public ParametroOrcamentoCotacaoBll(ParametroOrcamentoCotacaoData data)
        {
            _data = data;
        }

        public ParametroOrcamentoCotacaoDto ObterParametros(string lojaLogada)
        {
            return _data.ObterParametros(lojaLogada);
        }
    }
}
