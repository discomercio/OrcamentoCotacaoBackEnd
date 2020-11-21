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
        public static void DadoBaseComEnderecoDeEntrega(object objeto)
        {
            GravarLog("DadoBaseComEnderecoDeEntrega", objeto);
        }
        public static void Informo(string p0, string p1, object objeto)
        {
            GravarLog($@"Informo(""{p0}"", ""{p1}"")", objeto);
        }
        public static void Erro(string p0, object objeto)
        {
            GravarLog($@"Erro(""{p0}"")", objeto);
        }
        public static void ErroStatusCode(int p0, object objeto)
        {
            GravarLog($@"Erro(""{p0}"")", objeto);
        }
        public static void SemErro(string p0, object objeto)
        {
            GravarLog($@"SemErro(""{p0}"")", objeto);
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
            GravarLog($@"Resposta(""{p0}"")", objeto);
        }
        public static void IgnorarFeatureNoAmbiente(string p0, object objeto)
        {
            GravarLog($@"IgnorarFeatureNoAmbiente(""{p0}"")", objeto);
        }
        public static void ChamadaController(Type controllerType, string msg, object objeto)
        {
            string controllerFullName = LogTestes.NomeTipo(controllerType);
            GravarLog(controllerFullName + ": " + msg, objeto);
        }
        public static void MensagemEspecial(string msg, object objeto)
        {
            GravarLog(msg, objeto);
        }
        public static class BancoDados
        {
            public static void Verificacao(string msg, object objeto)
            {
                GravarLog(msg, objeto);
            }
            public static void LimparTabela(string msg, object objeto)
            {
                GravarLog("LimparTabela: " + msg, objeto);
            }
            public static void NovoRegistroEm(string msg, object objeto)
            {
                GravarLog("NovoRegistroEm: " + msg, objeto);
            }
            public static void NovoRegistro(string p0, string p1, object objeto)
            {
                GravarLog($@"NovoRegistro(""{p0}"", ""{p1}"")", objeto);
            }
            public static void GravarRegistro(object objeto)
            {
                GravarLog("GravarRegistro", objeto);
            }
        }
    }
}
