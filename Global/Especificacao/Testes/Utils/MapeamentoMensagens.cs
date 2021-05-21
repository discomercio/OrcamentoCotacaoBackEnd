using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Testes.Utils
{
    public static class MapeamentoMensagens
    {
        //fazemos como static para facilitar MUITO vida
        private static readonly Dictionary<string, Dictionary<string, string>> dicionarioMensagens = new Dictionary<string, Dictionary<string, string>>();
        public static void GivenNoAmbienteErroE(string ambiente, string msgOriginal, string msgSubstituta)
        {
            if (!dicionarioMensagens.ContainsKey(ambiente))
                dicionarioMensagens.Add(ambiente, new Dictionary<string, string>());
            var mensagens = dicionarioMensagens[ambiente];
            if (!mensagens.ContainsKey(msgOriginal))
                mensagens.Add(msgOriginal, msgSubstituta);

            //se tentarem colocar mensagens diferentes damos erro
            Assert.Equal(mensagens[msgOriginal], msgSubstituta);
        }
        public static string MapearMensagem(string? typeFullName, string msgOriginal)
        {
            if (typeFullName == null)
            {
                Assert.Equal("", "sem this.GetType().FullName");
                return msgOriginal;
            }

            //mal resolvido: temos um Especificacao na frente.... bom, tiramos!
            typeFullName = typeFullName.Replace("Especificacao.Ambiente.", "Ambiente.");
            typeFullName = typeFullName.Replace("Especificacao.Especificacao.", "Especificacao.");

            if (!dicionarioMensagens.ContainsKey(typeFullName))
                return msgOriginal;
            var mensagens = dicionarioMensagens[typeFullName];
            if (!mensagens.ContainsKey(msgOriginal))
                return msgOriginal;
            return mensagens[msgOriginal];
        }
    }
}
