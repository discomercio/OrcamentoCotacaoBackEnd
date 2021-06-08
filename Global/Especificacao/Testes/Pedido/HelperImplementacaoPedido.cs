using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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
        protected abstract void AbstractDadoBaseClientePJComEnderecoDeEntrega();
        protected abstract void AbstractInformo(string p0, string p1);
        protected abstract void AbstractListaDeItensInformo(int numeroItem, string campo, string valor);
        protected abstract void AbstractRecalcularTotaisDoPedido();
        protected abstract void AbstractDeixarFormaDePagamentoConsistente();
        protected abstract void AbstractListaDeItensComXitens(int numeroItens);
        protected abstract void AbstractLimparEnderecoDeEntrega();
        protected abstract void AbstractLimparDadosCadastraisEEnderecoDeEntrega();
        protected abstract List<string> AbstractListaErros();
        protected abstract string? AbstractPedidoPaiGerado();
        protected abstract List<string> AbstractPedidosFilhotesGerados();
        protected abstract List<string> AbstractPedidosGerados();
        #endregion

        protected bool ignorarFeature = false;
        public void GivenIgnorarCenarioNoAmbiente(string p0)
        {
            Testes.Pedido.PedidoPassosComuns.IgnorarCenarioNoAmbiente(p0, ref ignorarFeature, this.GetType());
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
                    if (System.Text.RegularExpressions.Regex.Match(m, erroOriginal, System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success)
                        contem = true;
                    if (System.Text.RegularExpressions.Regex.Match(MensagemLimpaParaComparacao(m), erroOriginal, System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success)
                        contem = true;
                    if (System.Text.RegularExpressions.Regex.Match(m, erroParaTeste, System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success)
                        contem = true;
                    if (System.Text.RegularExpressions.Regex.Match(MensagemLimpaParaComparacao(m), erroParaTeste, System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success)
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
        public void LimparEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.LimparEnderecoDeEntrega(this);
            AbstractLimparEnderecoDeEntrega();
        }
        public void LimparDadosCadastraisEEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.LimparDadosCadastraisEEnderecoDeEntrega(this);
            AbstractLimparDadosCadastraisEEnderecoDeEntrega();
        }

        public void RecalcularTotaisDoPedido()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.RecalcularTotaisDoPedido(this);
            AbstractRecalcularTotaisDoPedido();
        }
        public void DeixarFormaDePagamentoConsistente()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DeixarFormaDePagamentoConsistente(this);
            AbstractDeixarFormaDePagamentoConsistente();
        }

        public void GivenDadoBaseComEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseComEnderecoDeEntrega(this);
            AbstractDadoBaseComEnderecoDeEntrega();
        }
        public void GivenPedidoBaseClientePJComEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseClientePJComEnderecoDeEntrega(this);
            AbstractDadoBaseClientePJComEnderecoDeEntrega();
        }

        public void GivenPedidoBaseComEnderecoDeEntrega()
        {
            GivenDadoBaseComEnderecoDeEntrega();
        }

        public void GivenPedidoBase()
        {
            GivenDadoBase();
        }

        public void GivenPedidoBaseClientePF() => GivenDadoBaseClientePF();
        public void GivenPedidoBaseClientePJ() => GivenDadoBaseClientePJ();

        public void ListaDeItensInformo(int numeroItem, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensInformo(numeroItem, campo, valor, this);
            AbstractListaDeItensInformo(numeroItem, campo, valor);
        }

        public void ListaDeItensComXitens(int numeroItens)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensComXitens(numeroItens, this);
            AbstractListaDeItensComXitens(numeroItens);
        }

        public void TabelaT_PEDIDORegistroPaiCriadoVerificarCampo(string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            var pedidoPaiGerado = AbstractPedidoPaiGerado();
            if (string.IsNullOrEmpty(pedidoPaiGerado))
            {
                Assert.Equal("sem pedido gerado", pedidoPaiGerado ?? "");
                throw new ArgumentNullException();
            }
            List<string> somentePai = new List<string>()
                { pedidoPaiGerado };

            gerenciamentoBanco.TabelaT_PEDIDORegistroVerificarCampo(somentePai, campo, valor);
        }

        public void TabelaT_PEDIDO_ITEMRegistroCriadoVerificarCampo(int item, string campo, string valor)
        {
            if (ignorarFeature) return;
            var pedidoPaiGerado = AbstractPedidoPaiGerado();
            if (string.IsNullOrEmpty(pedidoPaiGerado))
            {
                Assert.Equal("sem pedido gerado", pedidoPaiGerado ?? "");
                throw new ArgumentNullException();
            }

            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            gerenciamentoBanco.TabelaT_PEDIDO_ITEMRegistroVerificarCampo(item, pedidoPaiGerado, campo, valor);

        }
        public void TabelaT_PEDIDO_ITEMFilhoteRegistroCriadoVerificarCampo(int item, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            var filhotes = AbstractPedidosFilhotesGerados();

            Assert.True(filhotes.Any());

            foreach (var filho in filhotes)
            {
                gerenciamentoBanco.TabelaT_PEDIDO_ITEMRegistroVerificarCampo(item, filho, campo, valor);
            }

        }

        public void TabelaT_PEDIDORegistrosFilhotesCriadosVerificarCampo(string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            var filhotes = AbstractPedidosFilhotesGerados();
            gerenciamentoBanco.TabelaT_PEDIDORegistroVerificarCampo(filhotes, campo, valor);
        }



        public void TabelaT_ESTOQUE_MOVIMENTORegistroPaiEProdutoVerificarCampo(string produto, string tipo_estoque, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_ESTOQUE_MOVIMENTO", "produto", "verificar campos", campo, valor, this);
            var pedidoPaiGerado = AbstractPedidoPaiGerado();
            if (string.IsNullOrEmpty(pedidoPaiGerado))
            {
                Assert.Equal("sem pedido gerado", pedidoPaiGerado ?? "");
                throw new ArgumentNullException();
            }

            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            var itemPedido = gerenciamentoBanco.BuscarItensPedido(pedidoPaiGerado).Where(x => x.Produto == produto).First();

            gerenciamentoBanco.TabelaT_ESTOQUE_MOVIMENTORegistroPaiEProdutoVerificarCampo(itemPedido, tipo_estoque, campo, valor, pedidoPaiGerado);
        }

        public void TabelaT_ESTOQUE_MOVIMENTORegistroFilhotesEProdutoVerificarCampo(string produto, string tipo_estoque, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_ESTOQUE_MOVIMENTO", "produto", "verificar campos filhotes", campo, valor, this);
            var pedidosFilhotesGerados = AbstractPedidosFilhotesGerados();
            if (!pedidosFilhotesGerados.Any())
            {
                Assert.Equal("sem pedido gerado", "");
                throw new ArgumentNullException();
            }

            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            foreach (var pedidoPaiGerado in pedidosFilhotesGerados)
            {
                var itemPedido = gerenciamentoBanco.BuscarItensPedido(pedidoPaiGerado).Where(x => x.Produto == produto).First();
                gerenciamentoBanco.TabelaT_ESTOQUE_MOVIMENTORegistroPaiEProdutoVerificarCampo(itemPedido, tipo_estoque, campo, valor, pedidoPaiGerado);
            }
        }


        public void TabelaT_ESTOQUE_MOVIMENTOPedidoPedidoPaiComRegistros(int registros)
        {
            if (ignorarFeature) return;
            var pedidoPaiGerado = AbstractPedidoPaiGerado();
            if (string.IsNullOrEmpty(pedidoPaiGerado))
            {
                Assert.Equal("sem pedido gerado", pedidoPaiGerado ?? "");
                throw new ArgumentNullException();
            }

            ContarRegistrosTEstoqueMovimentos(registros, pedidoPaiGerado);
        }

        public void TabelaT_ESTOQUE_MOVIMENTOPedidoPedidoFilhoteComRegistros(int registros)
        {
            if (ignorarFeature) return;
            var pedidosFilhotesGerados = AbstractPedidosFilhotesGerados();
            Assert.Single(pedidosFilhotesGerados);

            ContarRegistrosTEstoqueMovimentos(registros, pedidosFilhotesGerados.First());
        }

        private static void ContarRegistrosTEstoqueMovimentos(int registros, string pedidoPaiGerado)
        {
            var servicos = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos();
            var contextoBdProvider = servicos.GetRequiredService<InfraBanco.ContextoBdProvider>();
            var db = contextoBdProvider.GetContextoLeitura();
            var registrosLidos = (from registro in db.TestoqueMovimentos
                                  where registro.Pedido == pedidoPaiGerado
                                  select registro).ToList();
            Assert.Equal(registros, registrosLidos.Count());
        }

        public void VerificarPedidoGeradoSaldoDeID_ESTOQUE_SEM_PRESENCA(int indicePedido, int qtde)
        {
            if (ignorarFeature) return;
            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            if (indicePedido == 0)
            {
                //pai
                var pedidoPaiGerado = AbstractPedidoPaiGerado();
                if (string.IsNullOrEmpty(pedidoPaiGerado))
                {
                    Assert.Equal("sem pedido gerado", pedidoPaiGerado ?? "");
                    throw new ArgumentNullException();
                }

                var itensPedido = gerenciamentoBanco.BuscarItensPedido(pedidoPaiGerado);
                foreach (var item in itensPedido)
                {
                    gerenciamentoBanco.TabelaT_ESTOQUE_MOVIMENTORegistroDoPedidoVerificarCampo(item, "SPE", "qtde", qtde, pedidoPaiGerado);
                }
            }
            if (indicePedido == 1)
            {
                //filho
                var filhotes = AbstractPedidosFilhotesGerados();
                Assert.True(filhotes.Any());
                foreach (var filho in filhotes)
                {
                    var itensPedido = gerenciamentoBanco.BuscarItensPedido(filho);
                    foreach (var item in itensPedido)
                    {
                        gerenciamentoBanco.TabelaT_ESTOQUE_MOVIMENTORegistroDoPedidoVerificarCampo(item, "SPE", "qtde", qtde, filho);
                    }
                }
            }
        }

        public void TabelaT_ESTOQUE_ITEMRegistroPaiEProdutoVerificarCampo(string produto, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_ESTOQUE_ITEM", "produto", "verificar campos", campo, valor, this);
            var pedidoPaiGerado = AbstractPedidoPaiGerado();
            if (string.IsNullOrEmpty(pedidoPaiGerado))
            {
                Assert.Equal("sem pedido gerado", pedidoPaiGerado ?? "");
                throw new ArgumentNullException();
            }

            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            var itemPedido = gerenciamentoBanco.BuscarItensPedido(pedidoPaiGerado).Where(x => x.Produto == produto).FirstOrDefault();

            gerenciamentoBanco.TabelaT_ESTOQUE_ITEMRegistroPaiEProdutoVerificarCampo(itemPedido, campo, valor);
        }



        public void TabelaT_ESTOQUERegistroPaiVerificarCampo(string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_ESTOQUE", "pedido", "verificar campos", campo, valor, this);
            var pedidoPaiGerado = AbstractPedidoPaiGerado();
            if (string.IsNullOrEmpty(pedidoPaiGerado))
            {
                Assert.Equal("sem pedido gerado", pedidoPaiGerado ?? "");
                throw new ArgumentNullException();
            }

            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            var itemPedido = gerenciamentoBanco.BuscarItensPedido(pedidoPaiGerado).ToList();

            gerenciamentoBanco.TabelaT_ESTOQUERegistroPaiVerificarCampo(itemPedido, campo, valor);
        }

        public void TabelaT_ESTOQUE_LOGPedidoGeradoVerificarCampo(string produto, string operacao, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_ESTOQUE_LOG", "pedido", "verificar campos", campo, valor, this);
            var pedidoPaiGerado = AbstractPedidoPaiGerado();
            if (string.IsNullOrEmpty(pedidoPaiGerado))
            {
                Assert.Equal("sem pedido gerado", pedidoPaiGerado ?? "");
                throw new ArgumentNullException();
            }

            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            gerenciamentoBanco.TabelaT_ESTOQUE_LOGPedidoGeradoVerificarCampo(pedidoPaiGerado, operacao, produto, campo, valor);
        }

        public void TabelaT_LOGPedidoGeradoEOperacaoVerificarCampo(string operacao, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_LOG", "pedido", "verificar campos", campo, valor, this);
            var pedidoPaiGerado = AbstractPedidoPaiGerado();
            if (string.IsNullOrEmpty(pedidoPaiGerado))
            {
                Assert.Equal("sem pedido gerado", pedidoPaiGerado ?? "");
                throw new ArgumentNullException();
            }

            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            gerenciamentoBanco.TabelaT_LOGPedidoGeradoEOperacaoVerificarCampo(pedidoPaiGerado, operacao, campo, valor);

        }

        public void TabelaT_PRODUTO_X_WMS_REGRA_CDFabricanteEProdutoVerificarCampo(string fabricante, string produto, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PRODUTO_X_WMS_REGRA_CD", "fabricante e produto", "verificar campos", campo, valor, this);

            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            gerenciamentoBanco.TabelaT_PRODUTO_X_WMS_REGRA_CDFabricanteEProdutoVerificarCampo(fabricante, produto, campo, valor);

        }

        public void TabelaT_PEDIDO_ANALISE_ENDERECORegistroCriadoVerificarCampo(string campo, string valor)
        {
            if (ignorarFeature) return;
            var pedidoPaiGerado = AbstractPedidoPaiGerado();
            if (string.IsNullOrEmpty(pedidoPaiGerado))
            {
                Assert.Equal("sem pedido gerado", pedidoPaiGerado ?? "");
                throw new ArgumentNullException();
            }
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PEDIDO_ANALISE_ENDERECO", "campo", "valor", campo, valor, this);
            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            gerenciamentoBanco.TabelaT_PEDIDO_ANALISE_ENDERECORegistroCriadoVerificarCampo(pedidoPaiGerado, campo, valor);
        }

        public void TabelaT_PEDIDO_ANALISE_ENDERECO_CONFRONTACAORegistroCriadoVerificarCampo(string campo, string valor)
        {
            if (ignorarFeature) return;
            var pedidoPaiGerado = AbstractPedidoPaiGerado();
            if (string.IsNullOrEmpty(pedidoPaiGerado))
            {
                Assert.Equal("sem pedido gerado", pedidoPaiGerado ?? "");
                throw new ArgumentNullException();
            }
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO", "campo", "valor", campo, valor, this);
            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            gerenciamentoBanco.TabelaT_PEDIDO_ANALISE_ENDERECO_CONFRONTACAORegistroCriadoVerificarCampo(pedidoPaiGerado, campo, valor);

        }

        public void GeradoPedidos(int qtde_pedidos)
        {
            if (ignorarFeature) return;

            var ultimo_retorno = AbstractPedidosGerados();

            Assert.Equal(qtde_pedidos, ultimo_retorno.Count());
        }

        public void TabelaT_ESTOQUE_ITEMVerificarSaldo(string id_nfe_emitente, int saldo)
        {
            if (ignorarFeature) return;
            var pedidoPaiGerado = AbstractPedidoPaiGerado();
            if (string.IsNullOrEmpty(pedidoPaiGerado))
            {
                Assert.Equal("sem pedido gerado", pedidoPaiGerado ?? "");
                throw new ArgumentNullException();
            }

            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            var itemPedido = gerenciamentoBanco.BuscarItensPedido(pedidoPaiGerado);

            Assert.True(itemPedido.Any());

            foreach (var item in itemPedido)
            {
                gerenciamentoBanco.TabelaT_ESTOQUE_ITEMVerificarSaldo(int.Parse(id_nfe_emitente), saldo, item);
            }

        }

        public void VerificarQtdePedidosSalvos(int qtde)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.Verificacao("Verificar quantidade de pedidos salvos: ", qtde);
            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            gerenciamentoBanco.VerificarQtdePedidosSalvos(qtde);
        }
        public void TabelaT_PEDIDO_ANALISE_ENDERECOVerificarQtdeDeItensSalvos(int qtde)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.Verificacao("Verificar quantidade de itens salvos: ", qtde);
            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            gerenciamentoBanco.TabelaT_PEDIDO_ANALISE_ENDERECOVerificarQtdeDeItensSalvos(qtde);
        }

        public void TabelaT_PEDIDO_ANALISE_ENDERECO_CONFRONTACAOVerificarQtdeDeItensSalvos(int qtde)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.Verificacao("Verificar quantidade de itens salvos: ", qtde);
            Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
            gerenciamentoBanco.TabelaT_PEDIDO_ANALISE_ENDERECO_CONFRONTACAOVerificarQtdeDeItensSalvos(qtde);
        }

    }
}
