﻿using InfraBanco.Constantes;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfraBanco.Modelos;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Produto.RegrasCrtlEstoque;
using Produto.Dados;
using System;
using InfraBanco;

namespace Produto
{
    public class ProdutoGeralBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public ProdutoGeralBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<ProdutoComboDados> ListaProdutosComboDados(string loja, string uf, string tipo,
            Constantes.ContribuinteICMS contribuinte, Constantes.ProdutorRural produtorRural)
        {
            ProdutoComboDados retorno = new ProdutoComboDados();

            //var db = contextoProvider.GetContextoLeitura();

            //obtém  a sigla para regra
            string cliente_regra = UtilsProduto.MultiCdRegraDeterminaPessoa(tipo, contribuinte, produtorRural);

            var lstProdutosCompostos = BuscarProdutosCompostos(loja);
            //List<Produto.Dados.ProdutoDados> lstTodosProdutos = qtdeParcelas == 0 ? await BuscarTodosProdutos(loja)) : (await BuscarTodosProdutos(loja, tipoParcela, qtdeParcelas, dataRefCoeficiente.GetValueOrDefault(new DateTime())));
            List<Produto.Dados.ProdutoDados> lstTodosProdutos = await BuscarTodosProdutos(loja);


            List<RegrasBll> lst_cliente_regra = new List<RegrasBll>();
            MontaListaRegras(lstTodosProdutos, lst_cliente_regra);

            List<string> lstErros = new List<string>();
            await UtilsProduto.ObterCtrlEstoqueProdutoRegra_Teste(lstErros, lst_cliente_regra, uf, cliente_regra, contextoProvider);

            UtilsProduto.ObterDisponibilidadeEstoque(lst_cliente_regra, lstTodosProdutos, lstErros, contextoProvider);

            //retorna as qtdes disponiveis
            //Estoque.asp => estoque_verifica_disponibilidade_integral_v2
            //método que executa essa parte no asp
            await UtilsProduto.VerificarEstoque(lst_cliente_regra, contextoProvider);
            await UtilsProduto.VerificarEstoqueGlobal(lst_cliente_regra, contextoProvider);

            //buscar o parametro produto 001020 indice 12
            Tparametro tparametro = await UtilsProduto.BuscarRegistroParametro(Constantes.ID_PARAMETRO_Flag_Orcamento_ConsisteDisponibilidadeEstoqueGlobal, contextoProvider);
            //atribui a qtde de estoque para o produto
            IncluirEstoqueProduto(lst_cliente_regra, lstTodosProdutos, tparametro);

            await ExisteMensagensAlertaProdutos(lstTodosProdutos);

            retorno.ProdutoCompostoDados = (await lstProdutosCompostos).ToList();
            retorno.ProdutoDados = lstTodosProdutos;
            return retorno;
        }

        private async Task ExisteMensagensAlertaProdutos(List<Produto.Dados.ProdutoDados> lst_produtos)
        {
            var db = contextoProvider.GetContextoLeitura();

            var alertasTask = from c in db.TprodutoXAlertas
                              where c.TalertaProduto.Ativo == "S"
                              orderby c.Dt_Cadastro, c.Id_Alerta
                              select new
                              {
                                  alerta_fabricante = c.Fabricante,
                                  alerta_produto = c.Produto,
                                  alerta_mensagem = c.TalertaProduto.Mensagem,
                                  alerta_descricao = c.Tproduto.Descricao
                              };

            var alertas = await alertasTask.ToListAsync();

            foreach (var p in lst_produtos)
            {
                foreach (var m in alertas)
                {
                    if (string.IsNullOrEmpty(m.alerta_fabricante))
                    {
                        if (m.alerta_fabricante == p.Fabricante && m.alerta_produto == p.Produto)
                        {
                            p.Alertas = m.alerta_mensagem;
                        }
                    }
                }
            }
        }

        private void MontaListaRegras(List<Produto.Dados.ProdutoDados> lst_produtos, List<RegrasBll> lst_cliente_regra)
        {
            foreach (var p in lst_produtos)
            {
                lst_cliente_regra.Add(new RegrasBll
                {
                    Fabricante = p.Fabricante,
                    Produto = p.Produto
                });
            }
        }

        private async Task<IEnumerable<ProdutoCompostoDados>> BuscarProdutosCompostos(string loja)
        {
            var db = contextoProvider.GetContextoLeitura();

            var produtosCompostosSemGrupo = await (from d in (from c in db.Tprodutos
                                                              join pc in db.TecProdutoCompostos on c.Produto equals pc.Produto_Composto
                                                              join pci in db.TecProdutoCompostoItems on pc.Fabricante_Composto equals pci.Fabricante_composto
                                                              join pl in db.TprodutoLojas on pci.Produto_item equals pl.Produto
                                                              join fab in db.Tfabricantes on c.Fabricante equals fab.Fabricante
                                                              where pl.Loja == loja &&
                                                                    pl.Vendavel == "S" &&
                                                                    c.Fabricante == pc.Fabricante_Composto &&
                                                                    pc.Produto_Composto == pci.Produto_composto &&
                                                                    c.Excluido_status == 0 &&
                                                                    pl.Excluido_status == 0
                                                              select new
                                                              {
                                                                  fabricante_pai = c.Fabricante,
                                                                  fabricante_pai_nome = fab.Nome,
                                                                  produto_pai = c.Produto,
                                                                  pai_descricao = c.Descricao_Html,
                                                                  valor = (decimal)pl.Preco_Lista,
                                                                  qtde = (int)pci.Qtde,
                                                                  produtosFilhos = new ProdutoFilhoDados
                                                                  {
                                                                      Fabricante = c.Fabricante,
                                                                      Fabricante_Nome = fab.Nome,
                                                                      Produto = pci.Produto_item,
                                                                      Qtde = pci.Qtde
                                                                  }
                                                              })
                                                   select d).ToListAsync();

            var produtosCompostosGrupo = from d in produtosCompostosSemGrupo
                                         group d by d.produto_pai into g
                                         select g;
            var produtosCompostosLista = from g in produtosCompostosGrupo
                                         select new ProdutoCompostoDados
                                         {
                                             PaiFabricante = g.OrderBy(r => r.fabricante_pai).Select(r => r.fabricante_pai).FirstOrDefault(),
                                             PaiFabricanteNome = g.OrderBy(r => r.fabricante_pai_nome).Select(r => r.fabricante_pai_nome).FirstOrDefault(),
                                             PaiProduto = g.OrderBy(r => r.produto_pai).Select(r => r.produto_pai).FirstOrDefault(),
                                             PaiDescricao = g.OrderBy(r => r.pai_descricao).Select(r => r.pai_descricao).FirstOrDefault(),
                                             PaiPrecoTotal = g.Sum(r => r.qtde * r.valor),
                                             Filhos = g.Select(r => r.produtosFilhos).ToList()
                                         };

            List<ProdutoCompostoDados> produto = produtosCompostosLista.ToList();

            return produto;
        }

        public async Task<List<Produto.Dados.ProdutoDados>> BuscarTodosProdutos(string loja)
        {
            var db = contextoProvider.GetContextoLeitura();

            var todosProdutosTask = from c in db.Tprodutos
                                    join pl in db.TprodutoLojas on c.Produto equals pl.Produto
                                    join fab in db.Tfabricantes on c.Fabricante equals fab.Fabricante
                                    where pl.Vendavel == "S" &&
                                          pl.Loja == loja &&
                                          c.Excluido_status == 0 &&
                                          pl.Excluido_status == 0
                                    select new Produto.Dados.ProdutoDados
                                    {
                                        Fabricante = c.Fabricante,
                                        Fabricante_Nome = fab.Nome,
                                        Produto = pl.Produto,
                                        Descricao_html = c.Descricao_Html,
                                        Descricao = c.Descricao,
                                        Preco_lista = pl.Preco_Lista,
                                        Qtde_Max_Venda = pl.Qtde_Max_Venda,
                                        Desc_Max = pl.Desc_Max
                                    };

            List<Produto.Dados.ProdutoDados> lstTodosProdutos = await todosProdutosTask.ToListAsync();

            return lstTodosProdutos;
        }

        public async Task<IEnumerable<ProdutoDados>> BuscarProdutosEspecificos(string loja, List<string> lstProdutos)
        {
            var db = contextoProvider.GetContextoLeitura();

            var todosProdutosTask = from c in db.Tprodutos
                                    join pl in db.TprodutoLojas on c.Produto equals pl.Produto
                                    join fab in db.Tfabricantes on c.Fabricante equals fab.Fabricante
                                    where pl.Vendavel == "S" &&
                                          pl.Loja == loja &&
                                          c.Excluido_status == 0 &&
                                          pl.Excluido_status == 0 &&
                                          lstProdutos.Contains(c.Produto)
                                    select new Produto.Dados.ProdutoDados
                                    {
                                        Fabricante = c.Fabricante,
                                        Fabricante_Nome = fab.Nome,
                                        Produto = pl.Produto,
                                        Descricao_html = c.Descricao_Html,
                                        Descricao = c.Descricao,
                                        Preco_lista = pl.Preco_Lista,
                                        Qtde_Max_Venda = pl.Qtde_Max_Venda,
                                        Desc_Max = pl.Desc_Max
                                    };

            List<Produto.Dados.ProdutoDados> lstTodosProdutos = await todosProdutosTask.ToListAsync();

            return lstTodosProdutos;
        }

        public async Task<Tproduto> BuscarProdutoPorFabricanteECodigoComTransacao(string fabricante, string produto, string loja,
            ContextoBdGravacao contextoBdGravacao)
        {

            var produtoComposto = await contextoBdGravacao.Tprodutos
                .Where(x => x.Fabricante == fabricante && x.Produto == produto)
                .Include(t => t.TecProdutoComposto.TecProdutoCompostoItems)
                .Select(x => x).FirstOrDefaultAsync();

            if (produtoComposto != null) return produtoComposto;

            var produtoSimples = contextoBdGravacao.Tprodutos
                .Where(x => x.Fabricante == fabricante && x.Produto == produto).FirstOrDefault();

            if (produtoSimples == null) return null;


            return produtoSimples;
        }

        public async Task<TecProdutoComposto> BuscarProdutoCompostoPorFabricanteECodigoComTransacao(string fabricante,
            string produto, ContextoBdGravacao contextoBdGravacao)
        {
            var produtoCompostoTask = from c in contextoBdGravacao.TecProdutoCompostos
                                      where c.Fabricante_Composto == fabricante && c.Produto_Composto == produto
                                      select c;

            var t = await produtoCompostoTask.FirstOrDefaultAsync();

            return t;
        }

        private void IncluirEstoqueProduto(List<RegrasBll> lstRegras, List<Produto.Dados.ProdutoDados> lst_produtos, Tparametro parametro)
        {
            int qtde_estoque_total_disponivel = 0;
            int qtde_estoque_total_disponivel_global = 0;

            foreach (var p in lst_produtos)
            {
                if (!string.IsNullOrEmpty(p.Produto) && !string.IsNullOrEmpty(p.Fabricante))
                {
                    foreach (var regra in lstRegras)
                    {

                        if (regra.TwmsRegraCd != null)
                        {
                            foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                            {
                                if (r.Id_nfe_emitente > 0)
                                {
                                    if (r.St_inativo == 0)
                                    {
                                        if (regra.Fabricante == p.Fabricante && regra.Produto == p.Produto)
                                        {
                                            qtde_estoque_total_disponivel += r.Estoque_Qtde;
                                            if (qtde_estoque_total_disponivel_global == 0)
                                            {
                                                if (r.Estoque_Qtde_Estoque_Global != null)
                                                    qtde_estoque_total_disponivel_global = (int)r.Estoque_Qtde_Estoque_Global;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (parametro.Campo_inteiro == 1)
                    {
                        if (qtde_estoque_total_disponivel_global == 0)
                        {
                            p.Estoque = 0;
                        }
                        else
                        {
                            p.Estoque = qtde_estoque_total_disponivel_global;
                        }
                    }
                    else
                    {
                        p.Estoque = qtde_estoque_total_disponivel;
                    }
                }
                qtde_estoque_total_disponivel_global = 0;
                qtde_estoque_total_disponivel = 0;
            }

        }

        public static void VerificarRegrasAssociadasAosProdutos(List<RegrasBll> lstRegras, List<string> lstErros, string clienteUf, string clienteTipo,
            int id_nfe_emitente_selecao_manual)
        {
            foreach (var r in lstRegras)
            {
                if (!string.IsNullOrEmpty(r.Produto))
                {
                    if (r.TwmsRegraCd.Id == 0)
                        lstErros.Add("Produto (" + r.Fabricante + ")" + r.Produto + " não possui regra de consumo do estoque associada");
                    else if (r.TwmsRegraCd.St_inativo == 1)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + r.TwmsRegraCd.Apelido + "' associada ao produto(" +
                            r.Fabricante + ")" + r.Produto + " está desativada");
                    }
                    else if (r.TwmsRegraCdXUf.St_inativo == 1)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + r.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            r.Fabricante + ")" + r.Produto + " está bloqueada para a UF '" + clienteUf + "'");
                    }
                    else if (r.TwmsRegraCdXUfXPessoa != null && r.TwmsRegraCdXUfXPessoa.St_inativo == 1)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + r.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            r.Fabricante + ")" + r.Produto + " está bloqueada para clientes '" + clienteTipo + "' da UF '" + clienteUf + "'");
                    }
                    else if (r.TwmsRegraCdXUfXPessoa != null && r.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + r.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            r.Fabricante + ")" + r.Produto + " não especifica nenhum CD para aguardar produtos sem presença no estoque para clientes '" +
                            clienteTipo + "' da UF '" + clienteUf + "'");
                    }
                    int verificaErros = 0;
                    if (r.TwmsCdXUfXPessoaXCd != null)
                    {
                        foreach (var re in r.TwmsCdXUfXPessoaXCd)
                        {
                            if (re.Id_nfe_emitente > 0)
                            {
                                if (re.St_inativo == 0)
                                    verificaErros++;
                            }
                        }
                    }
                    //'A SELEÇÃO MANUAL DE CD PERMITE O USO DE CD DESATIVADO
                    if (verificaErros == 0 && id_nfe_emitente_selecao_manual == 0)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + r.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            r.Fabricante + ")" + r.Produto + " não especifica nenhum CD ativo para clientes '" +
                            clienteTipo + "' da UF '" + clienteUf + "'");
                    }
                }
            }
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoPropriedadeDados>> ObterListaPropriedadesProdutos()
        {
            var db = contextoProvider.GetContextoLeitura();

            var produtoPropriedades = from p in db.TProdutoCatalogoPropriedades
                                      select new Produto.Dados.ProdutoCatalogoPropriedadeDados
                                      {
                                          id = p.id,
                                          IdCfgTipoPropriedade = p.IdCfgTipoPropriedade,
                                          IdCfgTipoPermissaoEdicaoCadastro = p.IdCfgTipoPermissaoEdicaoCadastro,
                                          IdCfgDataType = p.IdCfgDataType,
                                          descricao = p.descricao,
                                          oculto = p.oculto,
                                          ordem = p.ordem,
                                          dt_cadastro = p.dt_cadastro,
                                          usuario_cadastro = p.usuario_cadastro

                                      };

            List<Produto.Dados.ProdutoCatalogoPropriedadeDados> lprodutosPropriedades = await produtoPropriedades.ToListAsync();

            return lprodutosPropriedades;
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoPropriedadeDados>> ObterListaPropriedadesPorRange()
        {
            var db = contextoProvider.GetContextoLeitura();

            var produtoPropriedades = from p in db.TProdutoCatalogoPropriedades
                                      where p.IdCfgDataType == 0 && p.ordem <= 800
                                      select new Produto.Dados.ProdutoCatalogoPropriedadeDados
                                      {
                                          id = p.id,
                                          IdCfgTipoPropriedade = p.IdCfgTipoPropriedade,
                                          IdCfgTipoPermissaoEdicaoCadastro = p.IdCfgTipoPermissaoEdicaoCadastro,
                                          IdCfgDataType = p.IdCfgDataType,
                                          descricao = p.descricao,
                                          oculto = p.oculto,
                                          ordem = p.ordem,
                                          dt_cadastro = p.dt_cadastro,
                                          usuario_cadastro = p.usuario_cadastro

                                      };

            List<Produto.Dados.ProdutoCatalogoPropriedadeDados> lprodutosPropriedades = await produtoPropriedades.ToListAsync();

            return lprodutosPropriedades;
        }


        public async Task<List<Produto.Dados.ProdutoCatalogoItemDados>> ObterListaPropriedadesProdutosById(int idProdutoCatalogo)
        {
            var db = contextoProvider.GetContextoLeitura();

            var produtoItens = from pc in db.TprodutoCatalogos
                               join pci in db.TprodutoCatalogoItems on pc.Id equals pci.IdProdutoCatalogo
                               join pcp in db.TProdutoCatalogoPropriedades on pci.IdProdutoCatalogoPropriedade equals pcp.id
                               join pco in db.TProdutoCatalogoPropriedadeOpcoes on pci.IdProdutoCatalogoPropriedade equals pco.id_produto_catalogo_propriedade into gj
                               from x in gj.DefaultIfEmpty()
                               where pc.Id == idProdutoCatalogo
                               select new Produto.Dados.ProdutoCatalogoItemDados
                               {
                                   idProdutoCatalogoPropriedade = pcp.id,
                                   nome = pcp.descricao,
                                   valor_item = pci.Valor,
                                   valor_opcao = x.valor
                               };

            List<Produto.Dados.ProdutoCatalogoItemDados> lprodutosItens = await produtoItens.ToListAsync();

            return lprodutosItens;
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoItemProdutosAtivosDados>> ObterProdutoPropriedadesAtivosTexto(int idProduto)
        {
            try
            {
                //para buscar produto com as propriedades ativas de texto
                var db = contextoProvider.GetContextoLeitura();

                var produtosAtivos = from tpc in db.TprodutoCatalogos
                                     join tpci in db.TprodutoCatalogoItems on tpc.Id equals tpci.IdProdutoCatalogo
                                     join tf in db.Tfabricantes on tpc.Fabricante equals tf.Fabricante
                                     join tpcp in db.TProdutoCatalogoPropriedades on tpci.IdProdutoCatalogoPropriedade equals tpcp.id
                                     where tpc.Ativo == true &&
                                           tpcp.IdCfgTipoPropriedade == 0 &&
                                           tpcp.oculto == false &&
                                           tpc.Id == idProduto
                                     select new ProdutoCatalogoItemProdutosAtivosDados
                                     {
                                         Id = tpc.Id,
                                         Produto = tpc.Produto,
                                         Fabricante = tf.Fabricante,
                                         FabricanteNome = tpc.Nome,
                                         Descricao = tpc.Descricao,
                                         IdPropriedade = tpcp.id,
                                         NomePropriedade = tpcp.descricao,
                                         ValorPropriedade = tpci.Valor,
                                         Ordem = tpcp.ordem
                                     };

                return await produtosAtivos.ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<List<Produto.Dados.ProdutoCatalogoItemProdutosAtivosDados>> ObterProdutoPropriedadesAtivosLista(int idProduto)
        {
            try
            {
                //para buscar produto com as propriedades ativas de listas
                var db = contextoProvider.GetContextoLeitura();

                var produtosAtivos = from tpci in db.TprodutoCatalogoItems
                                     join tpc in db.TprodutoCatalogos on tpci.IdProdutoCatalogo equals tpc.Id
                                     join tf in db.Tfabricantes on tpc.Fabricante equals tf.Fabricante
                                     join tpcpo in db.TProdutoCatalogoPropriedadeOpcoes on tpci.IdProdutoCatalogoPropriedadeOpcao equals tpcpo.id
                                     join tpcp in db.TProdutoCatalogoPropriedades on tpci.IdProdutoCatalogoPropriedade equals tpcp.id
                                     where tpc.Ativo == true &&
                                           tpcp.oculto == false &&
                                           tpc.Id == idProduto
                                     select new ProdutoCatalogoItemProdutosAtivosDados
                                     {
                                         Id = tpc.Id,
                                         Produto = tpc.Produto,
                                         Fabricante = tf.Fabricante,
                                         FabricanteNome = tpc.Nome,
                                         Descricao = tpc.Descricao,
                                         IdPropriedade = tpcp.id,
                                         NomePropriedade = tpcp.descricao,
                                         ValorPropriedade = tpcpo.valor,
                                         Ordem = tpcp.ordem
                                     };


                return await produtosAtivos.ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<List<Produto.Dados.ProdutoCatalogoItemProdutosAtivosDados>> ObterListaProdutosPropriedadesProdutosAtivos()
        {
            var db = contextoProvider.GetContextoLeitura();

            var produtosAtivos = from tpci in db.TprodutoCatalogoItems
                                 join tpc in db.TprodutoCatalogos on tpci.IdProdutoCatalogo equals tpc.Id
                                 join tf in db.Tfabricantes on tpc.Fabricante equals tf.Fabricante
                                 join tpcpo in db.TProdutoCatalogoPropriedadeOpcoes on tpci.IdProdutoCatalogoPropriedadeOpcao equals tpcpo.id
                                 join tpcp in db.TProdutoCatalogoPropriedades on tpci.IdProdutoCatalogoPropriedade equals tpcp.id
                                 where tpc.Ativo == true &&
                                       tpcp.oculto == false
                                 group new { tpc, tf, tpcpo } by new
                                 {
                                     tpc.Id,
                                     tpc.Produto,
                                     tf.Fabricante,
                                     tpc.Nome,
                                     FabricanteNome = tf.Nome,
                                     tpcp.id,
                                     tpcp.descricao,
                                     tpcpo.valor
                                 } into gr
                                 select new ProdutoCatalogoItemProdutosAtivosDados
                                 {
                                     Id = gr.Key.Id,
                                     Produto = gr.Key.Produto,
                                     Fabricante = gr.Key.Fabricante,
                                     FabricanteNome = gr.Key.FabricanteNome,
                                     Descricao = gr.Key.Nome,
                                     IdPropriedade = gr.Key.id,
                                     NomePropriedade = gr.Key.descricao,
                                     ValorPropriedade = gr.Key.valor
                                 };


            return await produtosAtivos.ToListAsync();
        }

        public async Task<List<CatalogoPropriedadesOpcaoDados>> ObterPropriedadesEOpcoesProdutosAtivos()
        {
            var db = contextoProvider.GetContextoLeitura();

            var saida = from tf in db.Tfabricantes
                        join tpc in db.TprodutoCatalogos on tf.Fabricante equals tpc.Fabricante
                        join tpci in db.TprodutoCatalogoItems on tpc.Id equals tpci.IdProdutoCatalogo
                        join tpcp in db.TProdutoCatalogoPropriedades on tpci.IdProdutoCatalogoPropriedade equals tpcp.id
                        join tpcpo in db.TProdutoCatalogoPropriedadeOpcoes on tpcp.id equals tpcpo.id_produto_catalogo_propriedade
                        where tpc.Id == tpci.IdProdutoCatalogo &&
                              tpc.Ativo == true &&
                              tpci.Oculto == false &&
                              tpcpo.oculto == false &&
                              tpcp.ordem <= 700 &&
                              tpcp.IdCfgTipoPropriedade == 1 &&
                              tpcp.oculto == false
                        group new { tpcp, tpcpo } by new
                        {
                            tpcpo.id_produto_catalogo_propriedade,
                            tpcp.descricao,
                            tpcpo.id,
                            tpcpo.valor
                        } into gr
                        select new CatalogoPropriedadesOpcaoDados
                        {
                            IdProdpriedade = gr.Key.id_produto_catalogo_propriedade,
                            NomePropriedade = gr.Key.descricao,
                            IdPropriedadeOpcao = gr.Key.id,
                            ValorPropriedadeOpcao = gr.Key.valor
                        };



            return await saida.ToListAsync();
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoItemDados>> ObterListaPropriedadesOpcoesProdutosById(int IdProdutoCatalogoPropriedade)
        {
            var db = contextoProvider.GetContextoLeitura();

            var produtoItens = from pc in db.TprodutoCatalogos
                               join pci in db.TprodutoCatalogoItems on pc.Id equals pci.IdProdutoCatalogo
                               join pcp in db.TProdutoCatalogoPropriedades on pci.IdProdutoCatalogoPropriedade equals pcp.id
                               join pco in db.TProdutoCatalogoPropriedadeOpcoes on pci.IdProdutoCatalogoPropriedade equals pco.id_produto_catalogo_propriedade into gj
                               from x in gj.DefaultIfEmpty()
                               where pcp.id == IdProdutoCatalogoPropriedade
                               select new Produto.Dados.ProdutoCatalogoItemDados
                               {
                                   idProdutoCatalogoPropriedade = pcp.id,
                                   nome = pcp.descricao,
                                   valor_item = pci.Valor,
                                   //valor_opcao = x.valor
                               };

            List<Produto.Dados.ProdutoCatalogoItemDados> lprodutosItens = await produtoItens.ToListAsync();

            return lprodutosItens;
        }

        // l1ng refatorar
        public async Task<List<Produto.Dados.FabricanteDados>> ObterListaFabricante()
        {
            var db = contextoProvider.GetContextoLeitura();

            var fabricantes = from f in db.Tfabricantes

                              select new Produto.Dados.FabricanteDados
                              {
                                  Fabricante = f.Fabricante,
                                  Nome = f.Nome
                              };

            List<Produto.Dados.FabricanteDados> lfabricantes = await fabricantes.ToListAsync();

            return lfabricantes;
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoPropriedadeDados>> ObterListaPropriedadesProdutos(int id)
        {
            var db = contextoProvider.GetContextoLeitura();

            var produtoPropriedades = from p in db.TProdutoCatalogoPropriedades
                                      where p.IdCfgDataType == 0 && p.id == id
                                      select new Produto.Dados.ProdutoCatalogoPropriedadeDados
                                      {
                                          id = p.id,
                                          IdCfgTipoPropriedade = p.IdCfgTipoPropriedade,
                                          IdCfgTipoPermissaoEdicaoCadastro = p.IdCfgTipoPermissaoEdicaoCadastro,
                                          IdCfgDataType = p.IdCfgDataType,
                                          descricao = p.descricao,
                                          oculto = p.oculto,
                                          ordem = p.ordem,
                                          dt_cadastro = p.dt_cadastro,
                                          usuario_cadastro = p.usuario_cadastro

                                      };

            List<Produto.Dados.ProdutoCatalogoPropriedadeDados> lprodutosPropriedades = await produtoPropriedades.ToListAsync();

            return lprodutosPropriedades;
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoPropriedadeOpcoesDados>> ObterListaPropriedadesOpcoes()
        {
            var db = contextoProvider.GetContextoLeitura();

            var produtoPropriedades = from p in db.TProdutoCatalogoPropriedadeOpcoes
                                      select new Produto.Dados.ProdutoCatalogoPropriedadeOpcoesDados
                                      {
                                          id = p.id,
                                          id_produto_catalogo_propriedade = p.id_produto_catalogo_propriedade,
                                          valor = p.valor,
                                          oculto = p.oculto,
                                          ordem = p.ordem,
                                          dt_cadastro = p.dt_cadastro,
                                          usuario_cadastro = p.usuario_cadastro
                                      };

            List<Produto.Dados.ProdutoCatalogoPropriedadeOpcoesDados> lprodutosPropriedades = await produtoPropriedades.ToListAsync();

            return lprodutosPropriedades;
        }

        public bool GravarPropriedadesProdutos(Produto.Dados.ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade)
        {
            var saida = false;

            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    int maxId = db.TProdutoCatalogoPropriedades.Max(p => p.id) + 1;
                    int maxOrdem = db.TProdutoCatalogoPropriedades.Max(p => p.ordem) + 1;

                    db.TProdutoCatalogoPropriedades.Add(
                        new TProdutoCatalogoPropriedade
                        {
                            id = maxId,
                            IdCfgTipoPropriedade = produtoCatalogoPropriedade.IdCfgTipoPropriedade,
                            IdCfgTipoPermissaoEdicaoCadastro = produtoCatalogoPropriedade.IdCfgTipoPermissaoEdicaoCadastro,
                            IdCfgDataType = produtoCatalogoPropriedade.IdCfgDataType,
                            descricao = produtoCatalogoPropriedade.descricao,
                            oculto = produtoCatalogoPropriedade.oculto,
                            ordem = maxOrdem,
                            dt_cadastro = DateTime.Now,
                            usuario_cadastro = produtoCatalogoPropriedade.usuario_cadastro
                        });

                    db.SaveChanges();
                    db.transacao.Commit();
                    saida = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return saida;
        }

        public bool AtualizarPropriedadesProdutos(Produto.Dados.ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade)
        {
            var saida = false;

            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var produtoCatalogoPropriedades = db.TProdutoCatalogoPropriedades.Where(item => item.id == produtoCatalogoPropriedade.id);

                    if (produtoCatalogoPropriedades != null)
                    {
                        foreach (var item in produtoCatalogoPropriedades)
                        {
                            item.descricao = produtoCatalogoPropriedade.descricao;
                            item.oculto = produtoCatalogoPropriedade.oculto;
                        }
                    }

                    db.SaveChanges();
                    db.transacao.Commit();
                    saida = true;

                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return saida;
        }



    }
}
