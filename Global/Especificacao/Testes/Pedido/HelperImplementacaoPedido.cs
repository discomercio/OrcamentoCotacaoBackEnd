using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Testes.Pedido
{
    public abstract class HelperImplementacaoPedido : Testes.Pedido.IPedidoPassosComuns
    {
        #region métodos abstratos
        protected abstract void AbstractDadoBaseComEnderecoDeEntrega();
        protected abstract void AbstractDadoBase();
        protected abstract void AbstractDadoBaseClientePF();
        protected abstract void AbstractDadoBaseClientePJ();
        protected abstract void AbstractInformo(string p0, string p1);
        protected abstract List<string> AbstractListaErros();
        #endregion

        protected bool ignorarFeature = false;
        public void GivenIgnorarFeatureNoAmbiente2(string p0)
        {
            Testes.Pedido.PedidoPassosComuns.IgnorarFeatureNoAmbiente(p0, ref ignorarFeature, this.GetType());
        }


        private void ThenErro(string? erroOriginal, bool erroDeveExistir)
        {
            if (ignorarFeature) return;

            List<string> listaErrosOriginal = AbstractListaErros();
            CompararMensagemErro(erroOriginal, erroDeveExistir, listaErrosOriginal, this);
        }

        public static void CompararMensagemErro(string? erroOriginal, bool erroDeveExistir, List<string> listaErrosOriginal, object objeto)
        {
            if (erroOriginal != null)
                erroOriginal = Testes.Utils.MapeamentoMensagens.MapearMensagem(objeto.GetType().FullName, erroOriginal);


            var erroParaTeste = MensagemLimpaParaComparacao(erroOriginal);
            var listaParaTeste = new List<string>();
            foreach (var m in listaErrosOriginal)
                listaParaTeste.Add(MensagemLimpaParaComparacao(m) ?? "mensagem vazia");

            bool regex = AcertosRegex(ref erroOriginal);

            if (erroDeveExistir)
            {
                bool contem = CompararListaMensagensRegex(erroOriginal, listaErrosOriginal, regex, erroParaTeste, listaParaTeste);
                if (!contem)
                {
                    Testes.Utils.LogTestes.LogOperacoes2.MensagemEspecial(
                        $"Erro: {erroOriginal} em {string.Join(" - ", listaErrosOriginal)}",
                        objeto);
                    Assert.Contains(erroOriginal, listaErrosOriginal);
                }
            }
            else
            {
                if (erroParaTeste == null)
                {
                    if (listaParaTeste.Count != 0)
                        Testes.Utils.LogTestes.LogOperacoes2.MensagemEspecial(
                            $"Erro: {erroOriginal} em {string.Join(" - ", listaErrosOriginal)}",
                            objeto);
                    Assert.Empty(listaErrosOriginal);
                }
                else
                {
                    bool contem = CompararListaMensagensRegex(erroOriginal, listaErrosOriginal, regex, erroParaTeste, listaParaTeste);
                    if (contem)
                        Assert.DoesNotContain(erroOriginal, listaErrosOriginal);
                }
            }
        }

        private static bool AcertosRegex(ref string? erroOriginal)
        {
            bool regex = false;
            if (erroOriginal != null)
            {
                string tentarRemover = "REGEX ";
                if (erroOriginal.ToUpper().StartsWith(tentarRemover))
                {
                    regex = true;
                    erroOriginal = erroOriginal.Substring(tentarRemover.Length);
                }
                tentarRemover = "REGEXP ";
                if (erroOriginal.ToUpper().StartsWith(tentarRemover))
                {
                    regex = true;
                    erroOriginal = erroOriginal.Substring(tentarRemover.Length);
                }
            }

            return regex;
        }

        private static bool CompararListaMensagensRegex(string? erroOriginal, List<string> listaErrosOriginal, bool regex, string? erroParaTeste, List<string> listaParaTeste)
        {
            bool contem = listaParaTeste.Contains(erroParaTeste ?? "");
            if (regex)
            {
                foreach (var m in listaErrosOriginal)
                {
                    //testamos todas as possibilidades
                    if (System.Text.RegularExpressions.Regex.Match(m, erroOriginal).Success)
                        contem = true;
                    if (System.Text.RegularExpressions.Regex.Match(MensagemLimpaParaComparacao(m), erroOriginal).Success)
                        contem = true;
                    if (System.Text.RegularExpressions.Regex.Match(m, erroParaTeste).Success)
                        contem = true;
                    if (System.Text.RegularExpressions.Regex.Match(MensagemLimpaParaComparacao(m), erroParaTeste).Success)
                        contem = true;
                    if (contem)
                        break;
                }
            }

            return contem;
        }

        private static string? MensagemLimpaParaComparacao(string? msg)
        {
            if (msg == null)
                return msg;

            msg = msg.ToUpper();
            //!! ou . no final, tudo é a mesma coisa
            msg = msg.Replace("!", ".").Replace("?", ".").Replace("-", ".");
            //tora todo so sodis pontos por um só
            while (msg.IndexOf("..") != -1)
                msg = msg.Replace("..", ".");
            return msg;
        }

        public void ThenErro(string p0)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.Erro(p0, this);
            ThenErro(p0, true);
        }
        public void ThenSemErro(string p0)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.SemErro(p0, this);
            ThenErro(p0, false);
        }
        public void ThenSemNenhumErro()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.SemNenhumErro(this);
            ThenErro(null, false);
        }

        public void WhenInformo(string p0, string p1)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.Informo(p0, p1, this);
            AbstractInformo(p0, p1);
        }

        public void GivenDadoBase()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBase(this);
            AbstractDadoBase();
        }
        public void GivenDadoBaseClientePF()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseClientePF(this);
            AbstractDadoBaseClientePF();
        }
        public void GivenDadoBaseClientePJ()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseClientePJ(this);
            AbstractDadoBaseClientePJ();
        }

        public void GivenDadoBaseComEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseComEnderecoDeEntrega(this);
            AbstractDadoBaseComEnderecoDeEntrega();
        }

        public void GivenPedidoBaseComEnderecoDeEntrega()
        {
            GivenDadoBaseComEnderecoDeEntrega();
        }

        public void WhenPedidoBase()
        {
            GivenDadoBase();
        }

        public void WhenPedidoBaseClientePF() => GivenDadoBaseClientePF();
        public void WhenPedidoBaseClientePJ() => GivenDadoBaseClientePJ();
    }
}
