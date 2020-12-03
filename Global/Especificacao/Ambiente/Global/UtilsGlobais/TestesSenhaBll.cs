using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.Global.UtilsGlobais
{
    public class TestesSenhaBll
    {
        [Fact]
        public void CriptografaTexto_vs_DecriptografaTexto()
        {
            string texto;
            string chave;

            texto = "kjhk"; chave = "ho890u";
            Assert.Equal(texto, global::UtilsGlobais.SenhaBll.DecriptografaTexto(global::UtilsGlobais.SenhaBll.CriptografaTexto(texto, chave), chave));
            texto = "kjahsd890123jk"; chave = "90191";
            Assert.Equal(texto, global::UtilsGlobais.SenhaBll.DecriptografaTexto(global::UtilsGlobais.SenhaBll.CriptografaTexto(texto, chave), chave));
            texto = "9012jmn"; chave = "nbasd";
            Assert.Equal(texto, global::UtilsGlobais.SenhaBll.DecriptografaTexto(global::UtilsGlobais.SenhaBll.CriptografaTexto(texto, chave), chave));
            texto = "mn as023"; chave = "09ama";
            Assert.Equal(texto, global::UtilsGlobais.SenhaBll.DecriptografaTexto(global::UtilsGlobais.SenhaBll.CriptografaTexto(texto, chave), chave));
        }

        [Fact]
        public void DecriptografaTexto()
        {
            //esse aqui foi gerado pelo ASP
            int fatorCriptografia = InfraBanco.Constantes.Constantes.FATOR_CRIPTO_SESSION_CTRL;
            var texto = "0xa31bf9b9692dc3b5651da38f4d1bc9ffa71f5f0fd39fe981c19bd7a5573d67b5e7f73dd1b74587cd01d7bd5b3d610fe98933df055f8169054317f125610b438363d513ff83cd934f8bfd3f718be33977a70d4b9bd10d491fcb3779b3f51767b5f35b89c71f4d89c5";
            var chave = global::UtilsGlobais.SenhaBll.Gera_chave(fatorCriptografia);
            var alvo = "PRAGMATICA|LOJA|202|0xa31bf9b9692dc3b5651d3f77b7ef2967a10d599bdb054d81|44123,6982986111|44123,6982986111";
            Assert.Equal(alvo, global::UtilsGlobais.SenhaBll.DecriptografaTexto(texto, chave));
        }
    }
}
