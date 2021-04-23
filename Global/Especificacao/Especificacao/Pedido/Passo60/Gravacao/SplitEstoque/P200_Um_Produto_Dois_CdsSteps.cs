using InfraBanco;
using InfraBanco.Modelos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao.SplitEstoque
{
    [Binding, Scope(Tag = "Especificacao.Pedido.Passo60.Gravacao.SplitEstoque")]
    public class P200_Um_Produto_Dois_CdsSteps
    {
        #region construtor
        private readonly ContextoBdProvider contextoBdProvider;
        private readonly Especificacao.Pedido.Passo60.Gravacao.SplitEstoque.EstoqueSaida.SplitEstoqueRotinas SplitEstoqueRotinas = new Especificacao.Pedido.Passo60.Gravacao.SplitEstoque.EstoqueSaida.SplitEstoqueRotinas();
        private readonly Testes.Utils.BancoTestes.GerenciamentoBancoSteps GerenciamentoBancoSteps = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
        public P200_Um_Produto_Dois_CdsSteps()
        {
            var servicos = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos();
            this.contextoBdProvider = servicos.GetRequiredService<InfraBanco.ContextoBdProvider>();

            //incializamos o produto com o pedido do magento
            var dados = Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoDados.PedidoBase();
            //todo: acerto DTO magento
            //fabricante = dados.ListaProdutos[0].Fabricante;
            //produto = dados.ListaProdutos[0].Produto;

            listaTipo_Pessoa = new List<string>();
            listaTipo_Pessoa.AddRange("PF PR PJC PJNC PJI".Split(' '));

            listaEstados = new List<string>();
            listaEstados.AddRange("AC AL AM AP BA CE DF ES GO MA MG MS MT PA PB PE PI PR RJ RN RO RR RS SC SE SP TO".Split(' '));

            SplitEstoqueRotinas.UsarProdutoComoFabricanteProduto(fabricante + produto, fabricante, produto);
        }
        #endregion

        #region produto
        //somente temos 1 produto
        private readonly string fabricante;
        private readonly string produto;
        #endregion

        #region CDs
        private class CdsDefinicao
        {
            public int Cd1 = 4001;
            public int Cd2 = 4003;
            public int Cd3 = 4903;
        }
        private CdsDefinicao cdsDefinicao = new CdsDefinicao();
        private int CdTextoParaNumero(string cdDesejado)
        {
            return cdDesejado.ToLower().Trim() switch
            {
                "cd1" => cdsDefinicao.Cd1,
                "cd2" => cdsDefinicao.Cd2,
                "cd3" => cdsDefinicao.Cd3,
                _ => throw new ArgumentException($"cdDesejado: {cdDesejado} não tratado"),
            };
        }
        private void CdGarantirCadastrado(int cd)
        {
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing();
            if ((from c in db.TnfEmitentes where c.Id == cd select c).Any())
                return;

            //criar
            db.Add(new TnfEmitente()
            {
                Apelido = "teste",
                Id = (short)cd,
                NFe_st_emitente_padrao = 1,
                St_Ativo = 1,
                St_Habilitado_Ctrl_Estoque = 1
            });
            db.SaveChanges();
            db.transacao.Commit();
        }

        private void CalcularCds(string primeiroCd, string segundoCd, string esperarCd, int cdnaoInformado, out int primeiroCdNumero, out int segundoCdNumero, out int esperarCdNumero)
        {
            #region Definir CDs
            cdnaoInformado = -1;
            primeiroCdNumero = cdnaoInformado;
            if (!string.IsNullOrWhiteSpace(primeiroCd))
            {
                primeiroCdNumero = CdTextoParaNumero(primeiroCd);
                CdGarantirCadastrado(primeiroCdNumero);
            }

            segundoCdNumero = cdnaoInformado;
            if (!string.IsNullOrWhiteSpace(segundoCd))
            {
                segundoCdNumero = CdTextoParaNumero(segundoCd);
                CdGarantirCadastrado(segundoCdNumero);
            }

            esperarCdNumero = cdnaoInformado;
            if (!string.IsNullOrWhiteSpace(esperarCd))
            {
                esperarCdNumero = CdTextoParaNumero(esperarCd);
                CdGarantirCadastrado(esperarCdNumero);
            }
            #endregion
        }
        private int CalcularCd(string primeiroCd)
        {
            var cdnaoInformado = -1;
            if (string.IsNullOrWhiteSpace(primeiroCd))
                return cdnaoInformado;

            return CdTextoParaNumero(primeiroCd);
        }
        #endregion

        #region Preparação do estoque
        //para conrolar a gravacao da regra
        private int? id_wms_regra_cd_usado = null;

        [When(@"Regra de consumo para ""(.*)"" para estado ""(.*)"" para ""(.*)"" depois ""(.*)"" e esperar mercadoria em ""(.*)""")]
        public void WhenRegraDeConsumoParaParaEstadoParaDepoisEEsperarMercadoriaEm(string tipo_pessoa, string uf,
            string primeiroCd, string segundoCd, string esperarCd)
        {
            int cdnaoInformado = -1;
            CalcularCds(primeiroCd, segundoCd, esperarCd, cdnaoInformado, out int primeiroCdNumero, out int segundoCdNumero, out int esperarCdNumero);

            using var db = contextoBdProvider.GetContextoGravacaoParaUsing();

            int id_wms_regra_cd;
            if (id_wms_regra_cd_usado.HasValue)
            {
                id_wms_regra_cd = id_wms_regra_cd_usado.Value;
            }
            else
            {
                id_wms_regra_cd = (from c in db.TwmsRegraCds select c.Id).Max() + 1;
            }

            int idwmsRegraCdXUfPessoa = (from c in db.TwmsRegraCdXUfPessoas select c.Id).Max() + 1;
            //produto e fabricante é chave única, somente inserimos se não existir
            var apagar = (from r in db.TprodutoXwmsRegraCds where r.Fabricante == fabricante && r.Produto == produto select r).ToList();
            foreach (var registro in apagar)
                db.Remove(registro);
            db.Add(new TprodutoXwmsRegraCd() { Fabricante = fabricante, Produto = produto, Id_wms_regra_cd = id_wms_regra_cd });
            if (!id_wms_regra_cd_usado.HasValue)
                db.Add(new TwmsRegraCd() { Id = id_wms_regra_cd, Apelido = "testes", St_inativo = 0 });

            int idRegraCdXUf = (from c in db.TwmsRegraCdXUfs select c.Id).Max() + 1;
            var existeIdRegraCdXUf = (from c in db.TwmsRegraCdXUfs where c.Id_wms_regra_cd == id_wms_regra_cd && c.Uf == uf select c).ToList();
            if (existeIdRegraCdXUf.Any())
            {
                idRegraCdXUf = existeIdRegraCdXUf.First().Id;
                existeIdRegraCdXUf.First().St_inativo = 0;
                db.Update(existeIdRegraCdXUf.First());
            }
            else
            {
                db.Add(new TwmsRegraCdXUf() { Id_wms_regra_cd = id_wms_regra_cd, St_inativo = 0, Uf = uf, Id = idRegraCdXUf });
            }
            db.Add(new TwmsRegraCdXUfPessoa() { Id_wms_regra_cd_x_uf = idRegraCdXUf, Spe_id_nfe_emitente = esperarCdNumero, St_inativo = 0, Tipo_pessoa = tipo_pessoa, Id = idwmsRegraCdXUfPessoa });

            int ordem_prioridade = 1;
            int idwmsRegraCdXUfXPessoaXCd = (from c in db.TwmsRegraCdXUfXPessoaXCds select c.Id).Max() + 1;
            if (primeiroCdNumero != cdnaoInformado)
            {
                db.Add(new TwmsRegraCdXUfXPessoaXCd() { Id = idwmsRegraCdXUfXPessoaXCd, Id_nfe_emitente = primeiroCdNumero, Ordem_prioridade = ordem_prioridade, St_inativo = 0, Id_wms_regra_cd_x_uf_x_pessoa = idwmsRegraCdXUfPessoa });
            }
            if (segundoCdNumero != cdnaoInformado)
            {
                ordem_prioridade++;
                idwmsRegraCdXUfXPessoaXCd++;
                db.Add(new TwmsRegraCdXUfXPessoaXCd() { Id = idwmsRegraCdXUfXPessoaXCd, Id_nfe_emitente = segundoCdNumero, Ordem_prioridade = ordem_prioridade, St_inativo = 0, Id_wms_regra_cd_x_uf_x_pessoa = idwmsRegraCdXUfPessoa });
            }

            id_wms_regra_cd_usado = id_wms_regra_cd;
            db.SaveChanges();
            db.transacao.Commit();
        }

        private readonly List<string> listaTipo_Pessoa;
        private readonly List<string> listaEstados;
        [When(@"Regra de consumo para todos os outros ""(.*)"" e para todos os outros estados ""(.*)"" para ""(.*)""")]
        public void WhenRegraDeConsumoParaTodosOsOutrosEParaTodosOsOutrosEstadosPara(string tipo_pessoa_ignorar, string uf_ignorar, string cd)
        {
            foreach (var tipo_pessoa in listaTipo_Pessoa)
            {
                if (tipo_pessoa.ToLower().Trim() == tipo_pessoa_ignorar.ToLower().Trim())
                    continue;
                foreach (var uf in listaEstados)
                {
                    if (uf.ToLower().Trim() == uf_ignorar.ToLower().Trim())
                        continue;
                    WhenRegraDeConsumoParaParaEstadoParaDepoisEEsperarMercadoriaEm(tipo_pessoa, uf, cd, "", cd);
                }
            }
        }

        [When(@"Desabilitar todas as regras por ""(.*)"" para PROD1")]
        public void WhenDesabilitarTodasAsRegrasPorParaPROD(string tabela)
        {
            tabela = tabela.ToLower().Trim();
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing();

            var lista_id_wms_regra_cd = (from p in db.TprodutoXwmsRegraCds
                                         where p.Produto == produto && p.Fabricante == fabricante
                                         select p.Id_wms_regra_cd).ToList();

            //local_desabilitar_regras: t_WMS_REGRA_CD
            if (tabela == "t_WMS_REGRA_CD".ToLower().Trim())
            {
                var atualizar = (from p in db.TwmsRegraCds
                                 where lista_id_wms_regra_cd.Contains(p.Id)
                                 select p).ToList();
                foreach (var registro in atualizar)
                {
                    registro.St_inativo = 1;
                    db.Update(registro);
                }
                db.SaveChanges();
                db.transacao.Commit();
                return;
            }

            //local_desabilitar_regras: t_WMS_REGRA_CD_X_UF
            if (tabela == "t_WMS_REGRA_CD_X_UF".ToLower().Trim())
            {
                var atualizar = (from p in db.TwmsRegraCdXUfs
                                 where lista_id_wms_regra_cd.Contains(p.Id_wms_regra_cd)
                                 select p).ToList();
                foreach (var registro in atualizar)
                {
                    registro.St_inativo = 1;
                    db.Update(registro);
                }
                db.SaveChanges();
                db.transacao.Commit();
                return;
            }

            //local_desabilitar_regras: t_WMS_REGRA_CD_X_UF_X_PESSOA
            var lista_id_wms_regra_cd_x_uf = (from p in db.TwmsRegraCdXUfs
                                              where lista_id_wms_regra_cd.Contains(p.Id_wms_regra_cd)
                                              select p.Id).ToList();
            if (tabela == "t_WMS_REGRA_CD_X_UF_X_PESSOA".ToLower().Trim())
            {
                var atualizar = (from p in db.TwmsRegraCdXUfPessoas
                                 where lista_id_wms_regra_cd_x_uf.Contains(p.Id_wms_regra_cd_x_uf)
                                 select p).ToList();
                foreach (var registro in atualizar)
                {
                    registro.St_inativo = 1;
                    db.Update(registro);
                }
                db.SaveChanges();
                db.transacao.Commit();
                return;
            }

            //local_desabilitar_regras: t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD
            var lista_id_wms_regra_cd_x_uf_x_pessoa = (from p in db.TwmsRegraCdXUfPessoas
                                                       where lista_id_wms_regra_cd_x_uf.Contains(p.Id_wms_regra_cd_x_uf)
                                                       select p.Id).ToList();
            if (tabela == "t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD".ToLower().Trim())
            {
                var atualizar = (from p in db.TwmsRegraCdXUfXPessoaXCds
                                 where lista_id_wms_regra_cd_x_uf_x_pessoa.Contains(p.Id_wms_regra_cd_x_uf_x_pessoa)
                                 select p).ToList();
                foreach (var registro in atualizar)
                {
                    registro.St_inativo = 1;
                    db.Update(registro);
                }
                db.SaveChanges();
                db.transacao.Commit();
                return;
            }

            throw new ArgumentException($"tabela {tabela} não reconhecida");
        }

        [When(@"Reiniciar banco imediatamente")]
        public void WhenReiniciarBancoImediatamente()
        {
            //temos que zerar o cadastramento de regras
            id_wms_regra_cd_usado = null;
            GerenciamentoBancoSteps.GivenReiniciarBancoImediatamente();
        }

        [When(@"Zerar o estoque do produto e colocar estoque de outros produtos")]
        public void WhenZerarOEstoqueDoProdutoEColocarEstoqueDeOutrosProdutos()
        {
            SplitEstoqueRotinas.ZerarTodoOEstoque();

            var listaProdutos = new List<Especificacao.Pedido.Passo60.Gravacao.SplitEstoque.EstoqueSaida.SplitEstoqueRotinas.FabricanteProdutoDados.FabricanteProdutoItem>
            {
                //temos os produtos 003220 e 003221 no nosso pedido base
                new EstoqueSaida.SplitEstoqueRotinas.FabricanteProdutoDados.FabricanteProdutoItem(fabricante: "003", produto: "003222"),
                new EstoqueSaida.SplitEstoqueRotinas.FabricanteProdutoDados.FabricanteProdutoItem(fabricante: "003", produto: "003223"),
                new EstoqueSaida.SplitEstoqueRotinas.FabricanteProdutoDados.FabricanteProdutoItem(fabricante: "003", produto: "003224"),
                new EstoqueSaida.SplitEstoqueRotinas.FabricanteProdutoDados.FabricanteProdutoItem(fabricante: "003", produto: "003225"),
                new EstoqueSaida.SplitEstoqueRotinas.FabricanteProdutoDados.FabricanteProdutoItem(fabricante: "003", produto: "003219"),
                new EstoqueSaida.SplitEstoqueRotinas.FabricanteProdutoDados.FabricanteProdutoItem(fabricante: "003", produto: "003218"),
                new EstoqueSaida.SplitEstoqueRotinas.FabricanteProdutoDados.FabricanteProdutoItem(fabricante: "001", produto: "003220"),
                new EstoqueSaida.SplitEstoqueRotinas.FabricanteProdutoDados.FabricanteProdutoItem(fabricante: "001", produto: "003221"),
                new EstoqueSaida.SplitEstoqueRotinas.FabricanteProdutoDados.FabricanteProdutoItem(fabricante: "004", produto: "003220"),
                new EstoqueSaida.SplitEstoqueRotinas.FabricanteProdutoDados.FabricanteProdutoItem(fabricante: "004", produto: "003221")
            };

            //CDs onde vamos inserir o estoque
            var listaCds = new List<int>() {
                cdsDefinicao.Cd1,
                cdsDefinicao.Cd2,
                cdsDefinicao.Cd3
            };


            foreach (var cd in listaCds)
            {
                foreach (var prod in listaProdutos)
                {
                    int qtde = 100;
                    int valor = 666;
                    string nomeProduto = prod.Fabricante + prod.Produto;
                    SplitEstoqueRotinas.UsarProdutoComoFabricanteProduto(nomeProduto, prod.Fabricante, prod.Produto);
                    SplitEstoqueRotinas.DefinirSaldoDeEstoqueParaProdutoComValorEIdNfeEmitente(qtde, nomeProduto, valor, (short)cd);
                }
            }
        }

        [When(@"Estoque ""(.*)"" de ""(.*)""")]
        public void WhenEstoqueDe(string cd, int qtde)
        {
            var cdNumero = CalcularCd(cd);
            int valor = 777;
            string nomeProduto = fabricante + produto;
            SplitEstoqueRotinas.DefinirSaldoDeEstoqueParaProdutoComValorEIdNfeEmitente(qtde, nomeProduto, valor, (short)cdNumero);
        }
        #endregion

        #region Execucao do teste
        //somente podemos fazer o teste em um ambiente por vez pq cvada um mexe no estoque
        private Testes.Pedido.HelperImplementacaoPedido? cadastrarPedidoVariavel = null;
        private Testes.Pedido.HelperImplementacaoPedido cadastrarPedido { get { return cadastrarPedidoVariavel ?? throw new ArgumentNullException($"local_teste ainda não foi definido"); } }
        [When(@"Local_teste: ""(.*)""")]
        public void WhenLocal_Teste(string local)
        {
            local = local.ToLower().Trim();
            cadastrarPedidoVariavel = local switch
            {
                "magento" => new Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido(),
                "loja" => new Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido(),
                _ => throw new ArgumentException($"local_teste: {local} não tratado"),
            };
        }

        [When(@"Criar pedido com (.*) itens para ""(.*)"" no estado ""(.*)""")]
        public void WhenCriarPedidoComItensPara(int quantidade, string tipo_pessoa, string uf)
        {
            switch (tipo_pessoa.ToUpper().Trim())
            {
                case "PF":
                    cadastrarPedido.GivenPedidoBaseClientePF();
                    break;
                case "PR":
                    cadastrarPedido.GivenPedidoBaseClientePF();
                    cadastrarPedido.WhenInformo("EndEtg_produtor_rural_status", "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM");
                    cadastrarPedido.WhenInformo("EndEtg_contribuinte_icms_status", "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM");
                    break;
                //o que faltam: "PR PJC PJNC PJI".Split(' '));
                default:
                    throw new ArgumentException($"tipo_pessoa {tipo_pessoa} não tratado ");
            }
            //somente par ao magento, ao executar na loja vamos ter qu emolhorar isto para pegar um endereço válido em cada estado
            switch (uf.ToUpper().Trim())
            {
                case "BA":
                    cadastrarPedido.WhenInformo("EndEtg_UF", uf);
                    cadastrarPedido.WhenInformo("EndEtg_cep", "40290050");
                    break;
                case "SP":
                    //o default
                    break;
                default:
                    throw new ArgumentException($"uf {uf} não tratado ");
            }
            cadastrarPedido.ListaDeItensComXitens(1);
            cadastrarPedido.ListaDeItensInformo(0, "Qtde", quantidade.ToString());
        }

        [When(@"Gerar 1 pedido pai sem filhotes")]
        public void WhenGerarPedidoPaiSemFilhotes()
        {
            GerarPedido(0);
        }
        [When(@"Gerar 1 pedido pai e 1 filhote")]
        public void WhenGerarPedidoPaiEFilhote()
        {
            GerarPedido(1);
        }
        private void GerarPedido(int numeroFilhotes)
        {
            cadastrarPedido.RecalcularTotaisDoPedido();
            cadastrarPedido.ThenSemNenhumErro();
            cadastrarPedido.GeradoPedidos(numeroFilhotes + 1);
        }

        #endregion

        #region Resultado do pedido
        [When(@"Pedido pai status = ""(.*)""")]
        public void WhenPedidoPaiStatus(string st_entrega)
        {
            cadastrarPedido.TabelaT_PEDIDORegistroPaiCriadoVerificarCampo("st_entrega", st_entrega);
        }
        [When(@"Pedido filhote status = ""(.*)""")]
        public void WhenPedidoFilhoteStatus(string st_entrega)
        {
            cadastrarPedido.TabelaT_PEDIDORegistrosFilhotesCriadosVerificarCampo("st_entrega", st_entrega);
        }
        [When(@"Pedido pai id_nfe_emitente = ""(.*)""")]
        public void WhenPedidoPaiId_Nfe_Emitente(string id_nfe_emitente)
        {
            cadastrarPedido.TabelaT_PEDIDORegistroPaiCriadoVerificarCampo("id_nfe_emitente", CdTextoParaNumero(id_nfe_emitente).ToString());
        }
        [When(@"Pedido filhote id_nfe_emitente = ""(.*)""")]
        public void WhenPedidoFilhoteId_Nfe_EmitenteCD(string id_nfe_emitente)
        {
            cadastrarPedido.TabelaT_PEDIDORegistrosFilhotesCriadosVerificarCampo("id_nfe_emitente", CdTextoParaNumero(id_nfe_emitente).ToString());
        }
        [When(@"Saldo de estoque na t_ESTOQUE_ITEM no ""(.*)"" = ""(\d*)""")]
        public void WhenSaldoDeEstoqueNaT_ESTOQUE_ITEMNo(string id_nfe_emitente, int saldo)
        {
            cadastrarPedido.TabelaT_ESTOQUE_ITEMVerificarSaldo(CdTextoParaNumero(id_nfe_emitente).ToString(), saldo);
        }
        [When(@"Saldo de estoque na t_ESTOQUE_ITEM no ""(.*)"" = \(45 - 20\)")]
        public void WhenSaldoDeEstoqueNaT_ESTOQUE_ITEMNo_45_20(string id_nfe_emitente)
        {
            WhenSaldoDeEstoqueNaT_ESTOQUE_ITEMNo(id_nfe_emitente, 45 - 20);
        }
        [When(@"Pedido pai t_pedido_item quantidade = ""(.*)""")]
        public void WhenPedidoPaiT_Pedido_ItemQuantidade(int qtde)
        {
            cadastrarPedido.TabelaT_PEDIDO_ITEMRegistroCriadoVerificarCampo(1, "qtde", qtde.ToString());
        }
        [When(@"Pedido filhote t_pedido_item quantidade = ""(.*)""")]
        public void WhenPedidoFilhoteT_Pedido_ItemQuantidade_(int qtde)
        {
            cadastrarPedido.TabelaT_PEDIDO_ITEMFilhoteRegistroCriadoVerificarCampo(1, "qtde", qtde.ToString());
        }
        [When(@"Pedido filhote t_pedido_item quantidade = \(50 - 45\)")]
        public void WhenPedidoFilhoteT_Pedido_ItemQuantidade_50_45()
        {
            WhenPedidoFilhoteT_Pedido_ItemQuantidade_(50 - 45);
        }
        [When(@"Saldo de estoque na t_ESTOQUE_ITEM no ""(.*)"" = 33 - \(50 - 45\) = 28")]
        public void WhenSaldoDeEstoqueNaT_ESTOQUE_ITEMNo__28(string id_nfe_emitente)
        {
            WhenSaldoDeEstoqueNaT_ESTOQUE_ITEMNo(id_nfe_emitente, 28);
        }
        [When(@"Pedido pai t_pedido_item quantidade = 45 \+ 22")]
        public void WhenPedidoPaiT_Pedido_ItemQuantidade()
        {
            WhenPedidoPaiT_Pedido_ItemQuantidade(45 + 22);
        }
        [When(@"Pedido pai t_pedido_item quantidade = 100 - 31")]
        public void WhenPedidoPaiT_Pedido_ItemQuantidade_100_31()
        {
            WhenPedidoPaiT_Pedido_ItemQuantidade(100 - 31);
        }
        [When(@"Pedido filhote t_pedido_item quantidade = 33 \+ 22")]
        public void WhenPedidoFilhoteT_Pedido_ItemQuantidade33_22()
        {
            WhenPedidoFilhoteT_Pedido_ItemQuantidade_(33 + 22);
        }
        [When(@"Pedido pai t_pedido_item quantidade = 31 \+ 41")]
        public void WhenPedidoPaiT_Pedido_ItemQuantidade31_41()
        {
            WhenPedidoPaiT_Pedido_ItemQuantidade(31 + 41);
        }


        [When(@"t_ESTOQUE_MOVIMENTO pedido = pedido pai, com ""(.*)"" registros")]
        public void WhenT_ESTOQUE_MOVIMENTOPedidoPedidoPaiComRegistros(int registros)
        {
            cadastrarPedido.TabelaT_ESTOQUE_MOVIMENTOPedidoPedidoPaiComRegistros(registros);
        }
        [When(@"t_ESTOQUE_MOVIMENTO pedido = pedido filhote, com ""(.*)"" registros")]
        public void WhenT_ESTOQUE_MOVIMENTOPedidoPedidoFilhoteComRegistros(int registros)
        {
            cadastrarPedido.TabelaT_ESTOQUE_MOVIMENTOPedidoPedidoFilhoteComRegistros(registros);
        }

        [When(@"t_ESTOQUE_MOVIMENTO pedido = pedido pai, qtde = ""(.*)"", estoque = ""(.*)"", operacao = ""(.*)""")]
        public void WhenT_ESTOQUE_MOVIMENTOPedidoPedidoPaiQtdeEstoqueOperacao(int qtde, string estoque, string operacao)
        {
            cadastrarPedido.TabelaT_ESTOQUE_MOVIMENTORegistroPaiEProdutoVerificarCampo(produto, estoque, "operacao", operacao);
            cadastrarPedido.TabelaT_ESTOQUE_MOVIMENTORegistroPaiEProdutoVerificarCampo(produto, estoque, "qtde", qtde.ToString());
        }
        [When(@"t_ESTOQUE_MOVIMENTO pedido = pedido pai, qtde = (.*) - (.*), estoque = ""(.*)"", operacao = ""(.*)""")]
        public void WhenT_ESTOQUE_MOVIMENTOPedidoPedidoPaiQtde_EstoqueOperacao(int qtde1, int qtde2, string estoque, string operacao)
        {
            WhenT_ESTOQUE_MOVIMENTOPedidoPedidoPaiQtdeEstoqueOperacao(qtde1 - qtde2, estoque, operacao);
        }

        [When(@"t_ESTOQUE_MOVIMENTO pedido = pedido filhote, qtde = ""(.*)"", estoque = ""(.*)"", operacao = ""(.*)""")]
        public void WhenT_ESTOQUE_MOVIMENTOPedidoPedidoFilhoteQtdeEstoqueOperacao(int qtde, string estoque, string operacao)
        {
            cadastrarPedido.TabelaT_ESTOQUE_MOVIMENTORegistroFilhotesEProdutoVerificarCampo(produto, estoque, "operacao", operacao);
            cadastrarPedido.TabelaT_ESTOQUE_MOVIMENTORegistroFilhotesEProdutoVerificarCampo(produto, estoque, "qtde", qtde.ToString());
        }

        // And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "003220" e estoque = "SPE", verificar campo "estoque" = "SPE"
        //public void ThenTabelaRegistroPaiEProdutoVerificarCampo(string produto, string tipo_estoque, string campo, string valor)
        //{
        //    Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_ESTOQUE_MOVIMENTO", $"produto {produto} e estoque{tipo_estoque}", "verificar campos", campo, valor, this);
        //    base.TabelaT_ESTOQUE_MOVIMENTORegistroPaiEProdutoVerificarCampo(produto, tipo_estoque, campo, valor);
        //}
        #endregion
    }
}
