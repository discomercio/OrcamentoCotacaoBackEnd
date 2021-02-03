using System;
using System.Collections.Generic;
using System.Text;

namespace Especificacao.Testes.Utils.LogTestes
{
    public static class LogOperacoes2
    {
        private static void GravarLog(string msg, object objeto)
        {
            ListaDependencias.RegistroDependencias.AdicionarMensagemLog(msg, objeto);
            LogTestes.LogMensagemOperacao(msg, objeto.GetType());
        }
        public static void VerificacaoFinalListaDependencias(object objeto)
        {
            GravarLog("VerificacaoFinalListaDependencias", objeto);
        }
        public static void DadoBase(object objeto)
        {
            GravarLog("DadoBase", objeto);
        }
        public static void DadoBaseClientePF(object objeto)
        {
            GravarLog("DadoBaseClientePF", objeto);
        }
        public static void DadoBaseClientePJ(object objeto)
        {
            GravarLog("DadoBaseClientePJ", objeto);
        }
        public static void LimparEnderecoDeEntrega(object objeto)
        {
            GravarLog("LimparEnderecoDeEntrega", objeto);
        }
        public static void LimparDadosCadastraisEEnderecoDeEntrega(object objeto)
        {
            GravarLog("LimparDadosCadastraisEEnderecoDeEntrega", objeto);
        }
        public static void DadoBaseClientePJComEnderecoDeEntrega(object objeto)
        {
            GravarLog("DadoBaseClientePJComEnderecoDeEntrega", objeto);
        }
        public static void EnderecoDeEntregaDoEstado(string p0, object objeto)
        {
            GravarLog($@"EnderecoDeEntregaDoEstado ""{p0}"" ", objeto);
        }
        public static void DadoBaseComEnderecoDeEntrega(object objeto)
        {
            GravarLog("DadoBaseComEnderecoDeEntrega", objeto);
        }
        public static void Informo(string p0, string p1, object objeto)
        {
            GravarLog($@"Informo ""{p0}"" = ""{p1}""", objeto);
        }
        public static void Erro(string p0, object objeto)
        {
            GravarLog($@"Erro ""{p0}""", objeto);
        }
        public static void ErroStatusCode(int p0, object objeto)
        {
            GravarLog($@"ErroStatusCode ""{p0}""", objeto);
        }
        public static void SemErro(string p0, object objeto)
        {
            GravarLog($@"SemErro ""{p0}""", objeto);
        }
        public static void SemNenhumErro(object objeto)
        {
            GravarLog("SemNenhumErro", objeto);
        }
        public static void Resposta(string p0, object objeto)
        {
            GravarLog($@"Resposta(""{p0}"")", objeto);
        }
        public static void Resposta(int p0, object objeto)
        {
            GravarLog($@"Resposta ""{p0}""", objeto);
        }
        public static void IgnorarCenarioNoAmbiente(string p0, object objeto)
        {
            GravarLog($@"IgnorarCenarioNoAmbiente ""{p0}""", objeto);
        }
        public static void ChamadaController(Type controllerType, string msg, object objeto)
        {
            string controllerFullName = LogTestes.NomeTipo(controllerType);
            GravarLog(controllerFullName + ": " + msg, objeto);
        }
        public static void ListaDeItensInformo(int numeroItem, string campo, string valor, object objeto)
        {
            GravarLog($@"ListaDeItensInformo ""{numeroItem}"", ""{campo}"" = ""{valor}""", objeto);
        }
        public static void RecalcularTotaisDoPedido(object objeto)
        {
            GravarLog($@"RecalcularTotaisDoPedido ", objeto);
        }
        public static void DeixarFormaDePagamentoConsistente(object objeto)
        {
            GravarLog($@"DeixarFormaDePagamentoConsistente ", objeto);
        }
        public static void ListaDeItensComXitens(int i, object objeto)
        {
            GravarLog($@"ListaDeItensComXitens {i}", objeto);
        }
        public static void MensagemEspecial(string msg, object objeto)
        {
            GravarLog(msg, objeto);
        }
        public static void Excecao(string msg, object objeto)
        {
            GravarLog("EXCEÇÃO: " + msg, objeto);
        }
        public static class BancoDados
        {
            public static void Verificacao(string msg, object objeto)
            {
                GravarLog(msg, objeto);
            }
            public static void LimparTabela(string msg, object objeto)
            {
                GravarLog("LimparTabela " + msg, objeto);
            }
            public static void NovoRegistroNaTabela(string msg, object objeto)
            {
                GravarLog("NovoRegistroEm " + msg, objeto);
            }
            public static void NovoRegistroEmCampo(string tabela, string p0, string p1, object objeto)
            {
                GravarLog($@"NovoRegistro tabela=""{tabela}"", ""{p0}"" = ""{p1}""", objeto);
            }
            public static void GravarRegistroEm(string p0, object objeto)
            {
                GravarLog($@"GravarRegistro {p0}", objeto);
            }
            public static void TabelaRegistroComCampoVerificarCampo(string tabela, string campoBusca, string valorBusca, string campoDesejado, string valorDesejado, object objeto)
            {
                GravarLog($"TabelaRegistroComCampoVerificarCampo tabela {tabela}, campoBusca {campoBusca}, " +
                    $"valorBusca {valorBusca}, campoDesejado {campoDesejado}, valorDesejado {valorDesejado}", objeto);
            }
            public static void TabelaApagarRegistroComCampo(string tabela, string campoBusca, string valorBusca, object objeto)
            {
                GravarLog($"TabelaApagarRegistroComCampo tabela {tabela}, campoBusca {campoBusca}, " +
                    $"valorBusca {valorBusca}", objeto);
            }
            public static void TabelaAlterarRegistroComCampo(string tabela, string campoBusca, string valorBusca, object objeto)
            {
                GravarLog($"TabelaAlterarRegistroComCampo tabela {tabela}, campoBusca {campoBusca}, " +
                    $"valorBusca {valorBusca}", objeto);
            }
        }
    }
}
