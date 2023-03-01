using InfraBanco.Constantes;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfraBanco.Modelos;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Produto.RegrasCrtlEstoque;
using Produto.Dados;
using System;
using InfraBanco;
using Produto.Dto;
using System.Data;

namespace Produto
{
    public class ProdutoGeralBll
    {
        private readonly ContextoBdProvider contextoProvider;
        private readonly Contexto contexto;

        public ProdutoGeralBll(InfraBanco.ContextoBdProvider contextoProvider, Contexto contexto)
        {
            this.contextoProvider = contextoProvider;
            this.contexto = contexto;
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

        private async Task ExisteMensagensAlertaProdutos(List<ProdutoDados> lst_produtos)
        {
            using var db = contextoProvider.GetContextoLeitura();

            var alertasTask = from c in db.TprodutoXAlerta
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

        private void MontaListaRegras(List<ProdutoDados> lst_produtos, List<RegrasBll> lst_cliente_regra)
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
            using var db = contextoProvider.GetContextoLeitura();

            //buscar todos os produtos compostos com seus filhos
            //não acessar t_produto
            //não verificar se o produto é vendavel => o que vai definir são os filhotes
            var produtosCompostosSemGrupo = await (from tpl in db.TprodutoLoja
                                                   join tepc in db.TecProdutoComposto on new { a = tpl.Produto, b = tpl.Fabricante } equals new { a = tepc.Produto_Composto, b = tepc.Fabricante_Composto }
                                                   join tepci in db.TecProdutoCompostoItem on new { a = tepc.Produto_Composto, b = tepc.Fabricante_Composto } equals new { a = tepci.Produto_composto, b = tepci.Fabricante_composto }
                                                   join tf in db.Tfabricante on tpl.Fabricante equals tf.Fabricante
                                                   where tpl.Loja == loja &&
                                                         tepc.Descricao != "." &&
                                                         !string.IsNullOrEmpty(tepc.Descricao)
                                                   select new
                                                   {
                                                       fabricante_pai = tf.Fabricante,
                                                       fabricante_pai_nome = tf.Nome,
                                                       produto_pai = tepc.Produto_Composto,
                                                       pai_descricao = tepc.Descricao,
                                                       valor = 0,
                                                       qtde = 1,
                                                       produtosFilhos = new ProdutoFilhoDados
                                                       {
                                                           Fabricante = tf.Fabricante,
                                                           Fabricante_Nome = tf.Nome,
                                                           Produto = tepci.Produto_item,
                                                           Qtde = tepci.Qtde,
                                                       }
                                                   }).ToListAsync();

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
            using var db = contextoProvider.GetContextoLeitura();

            var todosProdutosTask = from c in db.Tproduto
                                    join pl in db.TprodutoLoja on c.Produto equals pl.Produto
                                    join fab in db.Tfabricante on c.Fabricante equals fab.Fabricante
                                    join grp in db.TprodutoGrupo on c.Grupo equals grp.Codigo into lfgrp
                                    from leftgrp in lfgrp.DefaultIfEmpty()
                                    join sbgrp in db.TprodutoSubgrupo on c.Subgrupo equals sbgrp.Codigo into lfsbgrp
                                    from leftsbgrp in lfsbgrp.DefaultIfEmpty()
                                    where pl.Vendavel == "S" &&
                                          c.Descontinuado != "S" &&
                                          pl.Loja == loja &&
                                          c.Excluido_status == 0 &&
                                          pl.Excluido_status == 0 &&
                                          c.Descricao_Html != "." &&
                                          pl.Preco_Lista > 0 
                                    select new Produto.Dados.ProdutoDados
                                    {
                                        Fabricante = c.Fabricante,
                                        Fabricante_Nome = fab.Nome,
                                        Produto = pl.Produto,
                                        Descricao_html = c.Descricao_Html,
                                        Descricao = c.Descricao,
                                        Preco_lista = pl.Preco_Lista,
                                        Qtde_Max_Venda = pl.Qtde_Max_Venda,
                                        Desc_Max = pl.Desc_Max,
                                        Grupo = c.Grupo,
                                        GrupoDescricao = leftgrp.Descricao,
                                        SubGrupo = c.Subgrupo,
                                        SubGrupoDescricao = leftsbgrp.Descricao
                                    };

            List<Produto.Dados.ProdutoDados> lstTodosProdutos = await todosProdutosTask.ToListAsync();

            return lstTodosProdutos;
        }

        public async Task<IEnumerable<ProdutoDados>> BuscarProdutosEspecificos(string loja, List<string> lstProdutos)
        {
            using var db = contextoProvider.GetContextoLeitura();

            var todosProdutosTask = from c in db.Tproduto
                                    join pl in db.TprodutoLoja on c.Produto equals pl.Produto
                                    join fab in db.Tfabricante on c.Fabricante equals fab.Fabricante
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

        public async Task<Tproduto> BuscarProdutoSimplesPorFabricanteECodigoComTransacao(string fabricante, string produto, string loja,
            ContextoBdGravacao contextoBdGravacao)
        {
            var produtoSimples = await contextoBdGravacao.Tproduto
                .Where(x => x.Fabricante == fabricante && x.Produto == produto).FirstOrDefaultAsync();

            return produtoSimples;
        }

        public async Task<TecProdutoComposto> BuscarProdutoCompostoPorFabricanteECodigoComTransacao(string fabricante, string produto, string loja,
            ContextoBdGravacao contextoBdGravacao)
        {

            var produtoComposto = await (contextoBdGravacao.TecProdutoComposto
                .Where(x => x.Fabricante_Composto == fabricante && x.Produto_Composto == produto)
                .Include(x => x.TecProdutoCompostoItems)
                .Select(x => x)).FirstOrDefaultAsync();

            return produtoComposto;
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
            using (var db = contextoProvider.GetContextoLeitura())
            {
                return await (from p in db.TProdutoCatalogoPropriedade
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

                              }).ToListAsync();
            }
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoPropriedadeDados>> ObterListaPropriedadesPorRange()
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {

                return await (from p in db.TProdutoCatalogoPropriedade
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

                              })
                                .ToListAsync();
            }
        }


        public async Task<List<Produto.Dados.ProdutoCatalogoItemDados>> ObterListaPropriedadesProdutosById(int idProdutoCatalogo)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {

                return await (from pc in db.TprodutoCatalogo
                              join pci in db.TprodutoCatalogoItem on pc.Id equals pci.IdProdutoCatalogo
                              join pcp in db.TProdutoCatalogoPropriedade on pci.IdProdutoCatalogoPropriedade equals pcp.id
                              join pco in db.TProdutoCatalogoPropriedadeOpcao on pci.IdProdutoCatalogoPropriedade equals pco.id_produto_catalogo_propriedade into gj
                              from x in gj.DefaultIfEmpty()
                              where pc.Id == idProdutoCatalogo
                              select new Produto.Dados.ProdutoCatalogoItemDados
                              {
                                  idProdutoCatalogoPropriedade = pcp.id,
                                  nome = pcp.descricao,
                                  valor_item = pci.Valor,
                                  valor_opcao = x.valor
                              })
                                   .ToListAsync();
            }
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoItemProdutosAtivosDados>> ObterProdutoPropriedadesAtivosTexto(int idProduto,
            bool? propriedadeOculta, bool? propriedadeOcultaItem)
        {
            try
            {
                //para buscar produto com as propriedades ativas de texto
                using (var db = contextoProvider.GetContextoLeitura())
                {

                    var produtosAtivos = from tpc in db.TprodutoCatalogo
                                         join tpci in db.TprodutoCatalogoItem on tpc.Id equals tpci.IdProdutoCatalogo
                                         join tf in db.Tfabricante on tpc.Fabricante equals tf.Fabricante
                                         join tpcp in db.TProdutoCatalogoPropriedade on tpci.IdProdutoCatalogoPropriedade equals tpcp.id
                                         where tpcp.IdCfgTipoPropriedade == 0 &&
                                               tpc.Id == idProduto
                                         select new ProdutoCatalogoItemProdutosAtivosDados
                                         {
                                             Id = tpc.Id,
                                             Produto = tpc.Produto,
                                             Fabricante = tf.Fabricante,
                                             FabricanteNome = tf.Nome,
                                             Descricao = tpc.Nome,
                                             DescricaoCompleta = tpc.Descricao,
                                             IdPropriedade = tpcp.id,
                                             NomePropriedade = tpcp.descricao,
                                             IdValorPropriedadeOpcao = tpci.IdProdutoCatalogoPropriedadeOpcao,
                                             ValorPropriedade = tpci.Valor,
                                             Ordem = tpcp.ordem,
                                             PropriedadeOcultaItem = tpci.Oculto,
                                             PropriedadeOculta = tpcp.oculto
                                         };

                    if (propriedadeOculta != null)
                    {
                        produtosAtivos = produtosAtivos.Where(x => x.PropriedadeOculta == propriedadeOculta);
                    }
                    if (propriedadeOcultaItem != null)
                    {
                        produtosAtivos = produtosAtivos.Where(x => x.PropriedadeOcultaItem == propriedadeOcultaItem);
                    }

                    return await produtosAtivos.OrderBy(x => x.Ordem).ToListAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoItemProdutosAtivosDados>> ListarProdutoPropriedadesAtivosTexto(bool? propriedadeOculta, bool? propriedadeOcultaItem)
        {
            try
            {
                //para buscar produto com as propriedades ativas de texto
                using (var db = contextoProvider.GetContextoLeitura())
                {

                    var produtosAtivos = from tpc in db.TprodutoCatalogo
                                         join tpci in db.TprodutoCatalogoItem on tpc.Id equals tpci.IdProdutoCatalogo
                                         join tf in db.Tfabricante on tpc.Fabricante equals tf.Fabricante
                                         join tpcp in db.TProdutoCatalogoPropriedade on tpci.IdProdutoCatalogoPropriedade equals tpcp.id
                                         where tpc.Ativo == true &&
                                               tpcp.IdCfgTipoPropriedade == 0
                                         select new ProdutoCatalogoItemProdutosAtivosDados
                                         {
                                             Id = tpc.Id,
                                             Produto = tpc.Produto,
                                             Fabricante = tf.Fabricante,
                                             FabricanteNome = tf.Nome,
                                             Descricao = tpc.Nome,
                                             DescricaoCompleta = tpc.Descricao,
                                             IdPropriedade = tpcp.id,
                                             NomePropriedade = tpcp.descricao,
                                             IdValorPropriedadeOpcao = tpci.IdProdutoCatalogoPropriedadeOpcao,
                                             ValorPropriedade = tpci.Valor,
                                             Ordem = tpcp.ordem,
                                             PropriedadeOcultaItem = tpci.Oculto,
                                             PropriedadeOculta = tpcp.oculto
                                         };

                    if (propriedadeOculta != null)
                    {
                        produtosAtivos = produtosAtivos.Where(x => x.PropriedadeOculta == propriedadeOculta);
                    }
                    if (propriedadeOcultaItem != null)
                    {
                        produtosAtivos = produtosAtivos.Where(x => x.PropriedadeOcultaItem == propriedadeOcultaItem);
                    }

                    return await produtosAtivos.ToListAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoItemProdutosAtivosDados>> ObterProdutoPropriedadesAtivosLista(int idProduto,
            bool? propriedadeOculta, bool? propriedadeOcultaItem)
        {
            try
            {
                //para buscar produto com as propriedades ativas de listas
                using (var db = contextoProvider.GetContextoLeitura())
                {
                    var produtosAtivos = from tpci in db.TprodutoCatalogoItem
                                         join tpc in db.TprodutoCatalogo on tpci.IdProdutoCatalogo equals tpc.Id
                                         join tf in db.Tfabricante on tpc.Fabricante equals tf.Fabricante
                                         join tpcpo in db.TProdutoCatalogoPropriedadeOpcao on tpci.IdProdutoCatalogoPropriedadeOpcao equals tpcpo.id
                                         join tpcp in db.TProdutoCatalogoPropriedade on tpci.IdProdutoCatalogoPropriedade equals tpcp.id
                                         where tpc.Id == idProduto
                                         select new ProdutoCatalogoItemProdutosAtivosDados
                                         {
                                             Id = tpc.Id,
                                             Produto = tpc.Produto,
                                             Fabricante = tf.Fabricante,
                                             FabricanteNome = tf.Nome,
                                             Descricao = tpc.Nome,
                                             DescricaoCompleta = tpc.Descricao,
                                             IdPropriedade = tpcp.id,
                                             NomePropriedade = tpcp.descricao,
                                             IdValorPropriedadeOpcao = tpci.IdProdutoCatalogoPropriedadeOpcao,
                                             ValorPropriedade = tpcpo.valor,
                                             Ordem = tpcp.ordem,
                                             PropriedadeOcultaItem = tpci.Oculto,
                                             PropriedadeOculta = tpcp.oculto,
                                             TProdutoCatalogoPropriedadeOpcaoOculto = tpcpo.oculto
                                         };

                    if (propriedadeOculta != null)
                    {
                        produtosAtivos = produtosAtivos.Where(x => x.PropriedadeOculta == propriedadeOculta);
                    }
                    if (propriedadeOcultaItem != null)
                    {
                        produtosAtivos = produtosAtivos.Where(x => x.PropriedadeOcultaItem == propriedadeOcultaItem);
                    }

                    return await produtosAtivos.OrderBy(x => x.Ordem).ToListAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoItemProdutosAtivosDados>> ListarProdutoPropriedadesAtivosLista(bool? propriedadeOculta, bool? propriedadeOcultaItem)
        {
            try
            {
                //para buscar produto com as propriedades ativas de listas
                using (var db = contextoProvider.GetContextoLeitura())
                {
                    var produtosAtivos = from tpci in db.TprodutoCatalogoItem
                                         join tpc in db.TprodutoCatalogo on tpci.IdProdutoCatalogo equals tpc.Id
                                         join tf in db.Tfabricante on tpc.Fabricante equals tf.Fabricante
                                         join tpcpo in db.TProdutoCatalogoPropriedadeOpcao on tpci.IdProdutoCatalogoPropriedadeOpcao equals tpcpo.id
                                         join tpcp in db.TProdutoCatalogoPropriedade on tpci.IdProdutoCatalogoPropriedade equals tpcp.id
                                         where tpc.Ativo == true
                                         select new ProdutoCatalogoItemProdutosAtivosDados
                                         {
                                             Id = tpc.Id,
                                             Produto = tpc.Produto,
                                             Fabricante = tf.Fabricante,
                                             FabricanteNome = tf.Nome,
                                             Descricao = tpc.Nome,
                                             DescricaoCompleta = tpc.Descricao,
                                             IdPropriedade = tpcp.id,
                                             NomePropriedade = tpcp.descricao,
                                             IdValorPropriedadeOpcao = tpci.IdProdutoCatalogoPropriedadeOpcao,
                                             ValorPropriedade = tpcpo.valor,
                                             Ordem = tpcp.ordem,
                                             PropriedadeOcultaItem = tpci.Oculto,
                                             PropriedadeOculta = tpcp.oculto
                                         };

                    if (propriedadeOculta != null)
                    {
                        produtosAtivos = produtosAtivos.Where(x => x.PropriedadeOculta == propriedadeOculta);
                    }
                    if (propriedadeOcultaItem != null)
                    {
                        produtosAtivos = produtosAtivos.Where(x => x.PropriedadeOcultaItem == propriedadeOcultaItem);
                    }

                    return await produtosAtivos.ToListAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoItemProdutosAtivosDados>> ObterListaProdutosPropriedadesProdutosAtivos()
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                return await (from tpci in db.TprodutoCatalogoItem
                              join tpc in db.TprodutoCatalogo on tpci.IdProdutoCatalogo equals tpc.Id
                              join tf in db.Tfabricante on tpc.Fabricante equals tf.Fabricante
                              join tpcpo in db.TProdutoCatalogoPropriedadeOpcao on tpci.IdProdutoCatalogoPropriedadeOpcao equals tpcpo.id
                              join tpcp in db.TProdutoCatalogoPropriedade on tpci.IdProdutoCatalogoPropriedade equals tpcp.id
                              where tpc.Ativo == true &&
                                    tpcp.oculto == true
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
                              })
                .ToListAsync();
            }
        }

        public async Task<List<ProdutoAtivoDto>> ObterProdutosAtivos()
        {
            var listaOpcoes = new List<int> {
                1,  // Incluir na calculadora VRF
                2,  // Tipo da Unidade
                3,  // Descarga Condensadora
                4,  // Voltagem
                5,  // Capacidade (BTU/h)
                6,  // Ciclo
                8   // Linha de Produto
            };

            using (var db = contextoProvider.GetContextoLeitura())
            {
                var cte = await (
                    from pc in db.TprodutoCatalogo
                    join pci in db.TprodutoCatalogoItem on pc.Id equals pci.IdProdutoCatalogo
                    join pcp in db.TProdutoCatalogoPropriedade on pci.IdProdutoCatalogoPropriedade equals pcp.id
                    join pco in db.TProdutoCatalogoPropriedadeOpcao on pcp.id equals pco.id_produto_catalogo_propriedade
                    join f in db.Tfabricante on pc.Fabricante equals f.Fabricante
                    where
                        pc.Ativo == true &&
                        listaOpcoes.Contains(pci.IdProdutoCatalogoPropriedade) &&
                        pco.id == pci.IdProdutoCatalogoPropriedadeOpcao.Value
                    select new ProdutoAtivoDto
                    {

                        id = pc.Id.ToString(),
                        produto = pc.Produto,
                        fabricante = f.Nome,
                        descricao = pc.Nome,
                        propId = pcp.id,
                        propValor = pco.valor
                    }).ToListAsync();

                return (
                    from c in cte
                    group c by new { c.id, c.produto, c.fabricante, c.descricao } into g
                    select new ProdutoAtivoDto
                    {
                        id = g.Key.id.ToString(),
                        produto = g.Key.produto,
                        fabricante = g.Key.fabricante,
                        descricao = g.Key.descricao,
                        calculadoraVRF = cte.FirstOrDefault(c => c.id == g.Key.id && c.propId == 1) == null ? null : cte.FirstOrDefault(c => c.id == g.Key.id && c.propId == 1).propValor,
                        tipoUnidade = cte.FirstOrDefault(c => c.id == g.Key.id && c.propId == 2) == null ? null : cte.FirstOrDefault(c => c.id == g.Key.id && c.propId == 2).propValor,
                        descargaCondensadora = cte.FirstOrDefault(c => c.id == g.Key.id && c.propId == 3) == null ? null : cte.FirstOrDefault(c => c.id == g.Key.id && c.propId == 3).propValor,
                        voltagem = cte.FirstOrDefault(c => c.id == g.Key.id && c.propId == 4) == null ? null : cte.FirstOrDefault(c => c.id == g.Key.id && c.propId == 4).propValor,
                        capacidadeBTU = cte.FirstOrDefault(c => c.id == g.Key.id && c.propId == 5) == null ? null : cte.FirstOrDefault(c => c.id == g.Key.id && c.propId == 5).propValor,
                        ciclo = cte.FirstOrDefault(c => c.id == g.Key.id && c.propId == 6) == null ? null : cte.FirstOrDefault(c => c.id == g.Key.id && c.propId == 6).propValor,
                        linhaProduto = cte.FirstOrDefault(c => c.id == g.Key.id && c.propId == 8) == null ? null : cte.FirstOrDefault(c => c.id == g.Key.id && c.propId == 8).propValor
                    }).ToList();
            }
        }

        public async Task<List<CatalogoPropriedadesOpcaoDados>> ObterPropriedadesEOpcoesProdutosAtivos()
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                return await (from tf in db.Tfabricante
                              join tpc in db.TprodutoCatalogo on tf.Fabricante equals tpc.Fabricante
                              join tpci in db.TprodutoCatalogoItem on tpc.Id equals tpci.IdProdutoCatalogo
                              join tpcp in db.TProdutoCatalogoPropriedade on tpci.IdProdutoCatalogoPropriedade equals tpcp.id
                              join tpcpo in db.TProdutoCatalogoPropriedadeOpcao on tpcp.id equals tpcpo.id_produto_catalogo_propriedade
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
                              })
                            .ToListAsync();
            }
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoItemDados>> ObterListaPropriedadesOpcoesProdutosById(int IdProdutoCatalogoPropriedade)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                return await (from pc in db.TprodutoCatalogo
                              join pci in db.TprodutoCatalogoItem on pc.Id equals pci.IdProdutoCatalogo
                              join pcp in db.TProdutoCatalogoPropriedade on pci.IdProdutoCatalogoPropriedade equals pcp.id
                              join pco in db.TProdutoCatalogoPropriedadeOpcao on pci.IdProdutoCatalogoPropriedade equals pco.id_produto_catalogo_propriedade into gj
                              from x in gj.DefaultIfEmpty()
                              where pcp.id == IdProdutoCatalogoPropriedade
                              select new Produto.Dados.ProdutoCatalogoItemDados
                              {
                                  idProdutoCatalogoPropriedade = pcp.id,
                                  nome = pcp.descricao,
                                  valor_item = pci.Valor,
                              }).ToListAsync();

            }
        }

        public async Task<List<Produto.Dados.FabricanteDados>> ObterListaFabricante()
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                return await (from f in db.Tfabricante
                              select new Produto.Dados.FabricanteDados
                              {
                                  Fabricante = f.Fabricante,
                                  Nome = f.Nome,
                                  Descricao = $"{f.Fabricante} - {f.Nome}"
                              }).ToListAsync();
            }
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoPropriedadeDados>> ObterListaPropriedadesProdutos(int id)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                return await (from p in db.TProdutoCatalogoPropriedade
                              where p.id == id
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
                              }).ToListAsync();
            }
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoPropriedadeOpcoesDados>> ObterListaPropriedadesOpcoes()
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                return await (from p in db.TProdutoCatalogoPropriedadeOpcao
                              select new Produto.Dados.ProdutoCatalogoPropriedadeOpcoesDados
                              {
                                  id = p.id,
                                  id_produto_catalogo_propriedade = p.id_produto_catalogo_propriedade,
                                  valor = p.valor,
                                  oculto = p.oculto,
                                  ordem = p.ordem,
                                  dt_cadastro = p.dt_cadastro,
                                  usuario_cadastro = p.usuario_cadastro
                              })
                                .ToListAsync();
            }
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoPropriedadeOpcoesDados>> ObterListaPropriedadesOpcoes(int idProdutoCatalogoPropriedade)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                return await (from p in db.TProdutoCatalogoPropriedadeOpcao
                              where p.id_produto_catalogo_propriedade == idProdutoCatalogoPropriedade
                              select new Produto.Dados.ProdutoCatalogoPropriedadeOpcoesDados
                              {
                                  id = p.id,
                                  id_produto_catalogo_propriedade = p.id_produto_catalogo_propriedade,
                                  valor = p.valor,
                                  oculto = p.oculto,
                                  ordem = p.ordem,
                                  dt_cadastro = p.dt_cadastro,
                                  usuario_cadastro = p.usuario_cadastro
                              })
                                .ToListAsync();
            }
        }

        public async Task<TProdutoCatalogoPropriedade> GravarPropriedadeComTransacao(ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade, ContextoBdGravacao dbGravacao)
        {
            TProdutoCatalogoPropriedade tProdutoCatalogoPropriedade = new TProdutoCatalogoPropriedade()
            {
                IdCfgDataType = produtoCatalogoPropriedade.IdCfgDataType,
                IdCfgTipoPropriedade = produtoCatalogoPropriedade.IdCfgTipoPropriedade,
                IdCfgTipoPermissaoEdicaoCadastro = produtoCatalogoPropriedade.IdCfgTipoPermissaoEdicaoCadastro,
                descricao = produtoCatalogoPropriedade.descricao,
                oculto = produtoCatalogoPropriedade.oculto,
                ordem = produtoCatalogoPropriedade.ordem,
                dt_cadastro = DateTime.Now,
                usuario_cadastro = produtoCatalogoPropriedade.usuario_cadastro
            };

            dbGravacao.Add(tProdutoCatalogoPropriedade);
            await dbGravacao.SaveChangesAsync();

            return tProdutoCatalogoPropriedade;
        }

        public async Task<TProdutoCatalogoPropriedade> AtualizarPropriedadeComTransacao(ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade, ContextoBdGravacao dbGravacao)
        {
            TProdutoCatalogoPropriedade tProdutoCatalogoPropriedade = new TProdutoCatalogoPropriedade()
            {
                id = produtoCatalogoPropriedade.id,
                IdCfgDataType = produtoCatalogoPropriedade.IdCfgDataType,
                IdCfgTipoPropriedade = produtoCatalogoPropriedade.IdCfgTipoPropriedade,
                IdCfgTipoPermissaoEdicaoCadastro = produtoCatalogoPropriedade.IdCfgTipoPermissaoEdicaoCadastro,
                descricao = produtoCatalogoPropriedade.descricao,
                oculto = produtoCatalogoPropriedade.oculto,
                ordem = produtoCatalogoPropriedade.ordem,
                usuario_cadastro = produtoCatalogoPropriedade.usuario_cadastro,
                dt_cadastro = produtoCatalogoPropriedade.dt_cadastro
            };

            dbGravacao.Update(tProdutoCatalogoPropriedade);
            await dbGravacao.SaveChangesAsync();

            return tProdutoCatalogoPropriedade;
        }




        public async Task<TProdutoCatalogoPropriedadeOpcao> GravarPropriedadeOpcaoComTransacao(ProdutoCatalogoPropriedadeOpcoesDados produtoCatalogoPropriedadeOpcao, ContextoBdGravacao dbGravacao)
        {
            TProdutoCatalogoPropriedadeOpcao tProdutoCatalogoPropriedadeOpcao = new TProdutoCatalogoPropriedadeOpcao()
            {
                id_produto_catalogo_propriedade = produtoCatalogoPropriedadeOpcao.id_produto_catalogo_propriedade,
                valor = produtoCatalogoPropriedadeOpcao.valor,
                oculto = produtoCatalogoPropriedadeOpcao.oculto,
                ordem = produtoCatalogoPropriedadeOpcao.ordem,
                dt_cadastro = DateTime.Now,
                usuario_cadastro = produtoCatalogoPropriedadeOpcao.usuario_cadastro
            };

            dbGravacao.Add(tProdutoCatalogoPropriedadeOpcao);
            await dbGravacao.SaveChangesAsync();

            return tProdutoCatalogoPropriedadeOpcao;
        }

        public async Task RemoverPropriedadeOpcaoComTransacao(ProdutoCatalogoPropriedadeOpcoesDados produtoCatalogoPropriedadeOpcao, ContextoBdGravacao dbGravacao)
        {
            TProdutoCatalogoPropriedadeOpcao tProdutoCatalogoPropriedadeOpcao = new TProdutoCatalogoPropriedadeOpcao()
            {
                id = produtoCatalogoPropriedadeOpcao.id,
                id_produto_catalogo_propriedade = produtoCatalogoPropriedadeOpcao.id_produto_catalogo_propriedade,
                valor = produtoCatalogoPropriedadeOpcao.valor,
                oculto = produtoCatalogoPropriedadeOpcao.oculto,
                ordem = produtoCatalogoPropriedadeOpcao.ordem,
                dt_cadastro = DateTime.Now,
                usuario_cadastro = produtoCatalogoPropriedadeOpcao.usuario_cadastro
            };

            dbGravacao.Remove(tProdutoCatalogoPropriedadeOpcao);
            await dbGravacao.SaveChangesAsync();
        }

        public async Task<List<TprodutoCatalogo>> BuscarProdutosCatalogoPorPropriedadeOpcao(int idPropriedade,
            int idOpcao)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                return await (from c in db.TProdutoCatalogoPropriedade
                              join d in db.TProdutoCatalogoPropriedadeOpcao on c.id equals d.id_produto_catalogo_propriedade
                              join e in db.TprodutoCatalogoItem on d.id equals e.IdProdutoCatalogoPropriedadeOpcao
                              join f in db.TprodutoCatalogo on e.IdProdutoCatalogo equals f.Id
                              where c.id == idPropriedade &&
                                    d.id == idOpcao
                              select new TprodutoCatalogo
                              {
                                  Id = f.Id,
                                  Produto = f.Produto,
                                  Descricao = f.Descricao
                              }).ToListAsync();
            }
        }

        public async Task<TProdutoCatalogoPropriedadeOpcao> AtualizarPropriedadeOpcaoComTransacao(ProdutoCatalogoPropriedadeOpcoesDados produtoCatalogoPropriedadeOpcao, ContextoBdGravacao dbGravacao)
        {
            TProdutoCatalogoPropriedadeOpcao tProdutoCatalogoPropriedadeOpcao = new TProdutoCatalogoPropriedadeOpcao()
            {
                id = produtoCatalogoPropriedadeOpcao.id,
                id_produto_catalogo_propriedade = produtoCatalogoPropriedadeOpcao.id_produto_catalogo_propriedade,
                valor = produtoCatalogoPropriedadeOpcao.valor,
                oculto = produtoCatalogoPropriedadeOpcao.oculto,
                ordem = produtoCatalogoPropriedadeOpcao.ordem,
                dt_cadastro = DateTime.Now,
                usuario_cadastro = produtoCatalogoPropriedadeOpcao.usuario_cadastro
            };

            dbGravacao.Update(tProdutoCatalogoPropriedadeOpcao);
            await dbGravacao.SaveChangesAsync();

            return tProdutoCatalogoPropriedadeOpcao;
        }

        public async Task<bool> ObterPropriedadesUtilizadosPorProdutos(int idPropriedade)
        {
            var count = 0;

            using (var db = contextoProvider.GetContextoLeitura())
            {
                count = await (from p in db.TprodutoCatalogoItem
                               where p.IdProdutoCatalogoPropriedade == idPropriedade
                               select p).CountAsync();
            }

            return count > 0;
        }

        public async Task<string> ExcluirPropriedades(int idPropriedade, ContextoBdGravacao dbGravacao)
        {
            TProdutoCatalogoPropriedade tProdutoCatalogoPropriedade;
            List<TProdutoCatalogoPropriedadeOpcao> tProdutoCatalogoPropriedadeOpcaos;

            using (var db = contextoProvider.GetContextoLeitura())
            {
                tProdutoCatalogoPropriedade = await (from p in db.TProdutoCatalogoPropriedade
                                                     where p.id == idPropriedade
                                                     select p).SingleOrDefaultAsync();

                tProdutoCatalogoPropriedadeOpcaos = await (from p in db.TProdutoCatalogoPropriedadeOpcao
                                                           where p.id_produto_catalogo_propriedade == idPropriedade
                                                           select p).ToListAsync();
            }

            string logOpcoes = "";
            string omitirCampos = "|dt_cadastro|usuario_cadastro|";
            if (tProdutoCatalogoPropriedadeOpcaos != null)
            {
                foreach (var propriedadeOpcao in tProdutoCatalogoPropriedadeOpcaos)
                {
                    dbGravacao.Remove(propriedadeOpcao);
                    await dbGravacao.SaveChangesAsync();

                    if (!string.IsNullOrEmpty(logOpcoes)) logOpcoes += "\r      ";
                    logOpcoes = UtilsGlobais.Util.MontaLog(propriedadeOpcao, logOpcoes, omitirCampos);
                }
            }
            string log = "";
            if (tProdutoCatalogoPropriedade != null)
            {
                dbGravacao.Remove(tProdutoCatalogoPropriedade);
                var result = await dbGravacao.SaveChangesAsync();

                log = UtilsGlobais.Util.MontaLog(tProdutoCatalogoPropriedade, log, omitirCampos);

                if (!string.IsNullOrEmpty(logOpcoes)) log = $"{log}\r   Lista de valores válidos: {logOpcoes}";

                return log;
            }

            return null;
        }
    }
}
