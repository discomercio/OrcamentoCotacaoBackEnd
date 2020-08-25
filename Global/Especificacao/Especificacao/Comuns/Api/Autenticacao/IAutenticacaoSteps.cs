namespace Especificacao.Comuns.Api.Autenticacao
{
    public interface IAutenticacaoSteps
    {
        void GivenDadoBase();
        void ThenErroStatusCode(int p0);
        void WhenInformo(string p0, string p1);
    }
}