using System;
using System.Collections.Generic;
using System.Text;

namespace Especificacao.Testes.Utils.LogTestes
{
    public static class LogOperacoes
    {
        public static void VerificacaoFinalListaDependencias(Type getType)
        {
            LogTestes.LogMensagemOperacao("VerificacaoFinalListaDependencias", getType);
        }
        public static void DadoBase(Type getType)
        {
            LogTestes.LogMensagemOperacao("DadoBase", getType);
        }
        public static void DadoBaseClientePF(Type getType)
        {
            LogTestes.LogMensagemOperacao("DadoBaseClientePF", getType);
        }
        public static void DadoBaseClientePJ(Type getType)
        {
            LogTestes.LogMensagemOperacao("DadoBaseClientePJ", getType);
        }
        public static void DadoBaseComEnderecoDeEntrega(Type getType)
        {
            LogTestes.LogMensagemOperacao("DadoBaseComEnderecoDeEntrega", getType);
        }
        public static void Informo(string p0, string p1, Type getType)
        {
            LogTestes.LogMensagemOperacao($@"Informo(""{p0}"", ""{p1}"")", getType);
        }
        public static void Erro(string p0, Type getType)
        {
            LogTestes.LogMensagemOperacao($@"Erro(""{p0}"")", getType);
        }
        public static void ErroStatusCode(int p0, Type getType)
        {
            LogTestes.LogMensagemOperacao($@"Erro(""{p0}"")", getType);
        }
        public static void SemErro(string p0, Type getType)
        {
            LogTestes.LogMensagemOperacao($@"SemErro(""{p0}"")", getType);
        }
        public static void SemNenhumErro(Type getType)
        {
            LogTestes.LogMensagemOperacao("SemNenhumErro", getType);
        }
        public static void Resposta(string p0, Type getType)
        {
            LogTestes.LogMensagemOperacao($@"Resposta(""{p0}"")", getType);
        }
        public static void Resposta(int p0, Type getType)
        {
            LogTestes.LogMensagemOperacao($@"Resposta(""{p0}"")", getType);
        }
        public static void IgnorarFeatureNoAmbiente(string p0, Type getType)
        {
            LogTestes.LogMensagemOperacao($@"IgnorarFeatureNoAmbiente(""{p0}"")", getType);
        }
        public static void ChamadaController(Type controllerType, string msg, Type getType)
        {
            string controllerFullName = LogTestes.NomeTipo(controllerType);
            LogTestes.LogMensagemOperacao(controllerFullName + ": " + msg, getType);
        }
        public static void MensagemEspecial(string msg, Type getType)
        {
            LogTestes.LogMensagemOperacao(msg, getType);
        }
        public static class BancoDados
        {
            public static void Verificacao(string msg, Type getType)
            {
                LogTestes.LogMensagemOperacao(msg, getType);
            }
            public static void LimparTabela(string msg, Type getType)
            {
                LogTestes.LogMensagemOperacao("LimparTabela: " + msg, getType);
            }
            public static void NovoRegistroEm(string msg, Type getType)
            {
                LogTestes.LogMensagemOperacao("NovoRegistroEm: " + msg, getType);
            }
            public static void NovoRegistro(string p0, string p1, Type getType)
            {
                LogTestes.LogMensagemOperacao($@"NovoRegistro(""{p0}"", ""{p1}"")", getType);
            }
            public static void GravarRegistro(Type getType)
            {
                LogTestes.LogMensagemOperacao("GravarRegistro", getType);
            }
        }
    }
}
