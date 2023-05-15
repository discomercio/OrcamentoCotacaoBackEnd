using Azure.Core;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.EntityFrameworkCore;
using ProdutoCatalogo.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using static InfraBanco.ContextoBdGravacao;

namespace ProdutoCatalogo
{
    public class ProdutoCatalogoData
    {
        private readonly ContextoBdProvider _contextoProvider;
        private readonly Contexto _contexto;

        public ProdutoCatalogoData(ContextoBdProvider contextoProvider, Contexto contexto)
        {
            _contextoProvider = contextoProvider;
            _contexto = contexto;
        }

        public TprodutoCatalogo Atualizar(TprodutoCatalogo obj)
        {
            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    var produtoCatalogo = db.TprodutoCatalogo.FirstOrDefault(x => x.Id == obj.Id);

                    if (produtoCatalogo != null)
                    {
                        produtoCatalogo.Nome = obj.Nome == null ? "" : obj.Nome;
                        produtoCatalogo.Descricao = obj.Descricao;
                        produtoCatalogo.UsuarioEdicao = obj.UsuarioEdicao;
                        produtoCatalogo.DtEdicao = DateTime.Now;
                        produtoCatalogo.Ativo = obj.Ativo;

                        db.SaveChanges();
                        db.transacao.Commit();

                        return produtoCatalogo;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;
        }

        public TprodutoCatalogo AtualizarComTransacao(TprodutoCatalogo model, ContextoBdGravacao contextoBdGravacao)
        {
            var produtoCatalogo = (from c in contextoBdGravacao.TprodutoCatalogo
                                   where c.Id == model.Id
                                   select c).FirstOrDefault();
            if (produtoCatalogo == null) return null;

            produtoCatalogo.Nome = model.Nome == null ? "" : model.Nome;
            produtoCatalogo.Descricao = model.Descricao;
            produtoCatalogo.UsuarioEdicao = model.UsuarioEdicao;
            produtoCatalogo.DtEdicao = DateTime.Now;
            produtoCatalogo.Ativo = model.Ativo;
            produtoCatalogo.Fabricante = model.Fabricante;

            contextoBdGravacao.SaveChanges();

            //produtoCatalogo.campos = model.campos;
            //produtoCatalogo.imagem = model.imagem;
            return produtoCatalogo;
        }

        public TprodutoCatalogoItem AtualizarItemComTransacao(TprodutoCatalogoItem model,
            ContextoBdGravacao contextoBdGravacao)
        {
            var tProdutoCatalogoItem = (from c in contextoBdGravacao.TprodutoCatalogoItem
                                        where c.IdProdutoCatalogo == model.IdProdutoCatalogo &&
                                              c.IdProdutoCatalogoPropriedade == model.IdProdutoCatalogoPropriedade
                                        select c).FirstOrDefault();
            if (tProdutoCatalogoItem == null)
            {
                return CriarItensComTransacao(model, contextoBdGravacao);
            }

            //tProdutoCatalogoItem.IdProdutoCatalogo = model.IdProdutoCatalogo;
            //tProdutoCatalogoItem.IdProdutoCatalogoPropriedade = model.IdProdutoCatalogoPropriedade;
            tProdutoCatalogoItem.IdProdutoCatalogoPropriedadeOpcao = model.IdProdutoCatalogoPropriedadeOpcao;
            tProdutoCatalogoItem.Valor = model.Valor;
            tProdutoCatalogoItem.Oculto = model.Oculto;

            contextoBdGravacao.SaveChanges();

            return tProdutoCatalogoItem;
        }


        public bool Excluir(int id)
        {
            var saida = false;

            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {

                    var query = db.TprodutoCatalogo.FirstOrDefault(x => x.Id == id);

                    if (query != null)
                    {
                        query.Ativo = false;
                        db.SaveChanges();
                        db.transacao.Commit();
                        saida = true;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return saida;
        }

        public bool ExcluirImagem(int idProduto, int idImagem)
        {
            var saida = false;

            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    var obj = db.TprodutoCatalogoImagem.FirstOrDefault(x => x.Id == idImagem && x.IdProdutoCatalogo == idProduto);

                    if (obj != null)
                    {
                        db.TprodutoCatalogoImagem.Remove(obj);
                        db.SaveChanges();
                        db.transacao.Commit();
                        saida = true;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return saida;
        }

        public bool ExcluirImagemComTransacao(int idProduto, int idImagem, ContextoBdGravacao contextoBdGravacao)
        {
            var tProdutoCatalogoImagem = (from c in contextoBdGravacao.TprodutoCatalogoImagem
                                          where c.Id == idImagem &&
                                                c.IdProdutoCatalogo == idProduto
                                          select c).FirstOrDefault();

            if (tProdutoCatalogoImagem == null) return false;

            contextoBdGravacao.TprodutoCatalogoImagem.Remove(tProdutoCatalogoImagem);
            contextoBdGravacao.SaveChanges();

            return true;
        }

        public bool ExcluirImagemTmp()
        {
            var saida = false;

            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    var imgTmp = db.TprodutoCatalogoImagem.AsNoTracking().Where(x => x.IdProdutoCatalogo == 0);

                    db.TprodutoCatalogoImagem.RemoveRange(imgTmp);
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

        public TprodutoCatalogo Inserir(TprodutoCatalogo obj)
        {
            throw new NotImplementedException();
        }

        public List<TprodutoCatalogo> BuscarTprodutoCatalogo(TprodutoCatalogoFiltro obj)
        {
            using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
            {
                var produtos = from c in db.TprodutoCatalogo
                               select c;


                if (!String.IsNullOrEmpty(obj.Id))
                    produtos = produtos.Where(x => x.Id == int.Parse(obj.Id));

                if (!String.IsNullOrEmpty(obj.Produto))
                    produtos = produtos.Where(x => x.Produto.PadLeft(6, '0') == obj.Produto.PadLeft(6, '0'));

                if (obj.Ativo)
                    produtos = produtos.Where(x => x.Ativo == obj.Ativo);

                if (obj.IncluirImagem) produtos = produtos.Include(x => x.imagem);

                if (obj.IncluirPropriedades) produtos = produtos.Include(x => x.campos);

                return produtos.ToList();
            }
        }

        public List<TprodutoCatalogo> PorFiltro(TprodutoCatalogoFiltro obj)
        {


            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    var produtos = from pc in db.TprodutoCatalogo
                                   join f in db.Tfabricante on pc.Fabricante equals f.Fabricante
                                   join pci in db.TprodutoCatalogoImagem on pc.Id equals pci.IdProdutoCatalogo into pci_l
                                   from images in pci_l.DefaultIfEmpty()
                                   select new TprodutoCatalogo
                                   {
                                       Produto = pc.Produto,
                                       Id = pc.Id,
                                       Fabricante = f.Nome,
                                       Nome = pc.Nome,
                                       Descricao = pc.Descricao,
                                       imagem = images,
                                       Ativo = pc.Ativo
                                   };


                    if (!String.IsNullOrEmpty(obj.Id))
                        produtos = produtos.Where(x => x.Id == int.Parse(obj.Id));

                    if (!String.IsNullOrEmpty(obj.Produto))
                        produtos = produtos.Where(x => x.Produto == obj.Produto.PadLeft(6, '0'));

                    if (obj.Ativo)
                        produtos = produtos.Where(x => x.Ativo == obj.Ativo);
                    return produtos.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<ProdutoCatalogoListarDto> ListarProdutoCatalogo(
            string[] fabricantes,
            string codAlfaNumFabricante,
            string descargaCondensadora,
            string[] voltagem,
            string[] capacidade,
            string ciclo,
            string[] tipoUnidade,
            bool? imagem,
            bool? ativo,
            int pagina,
            int qtdeItensPorPagina,
            string nomeColunaOrdenacao)
        {
            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    var produtos = (from pc in db.TprodutoCatalogo
                                    join f in db.Tfabricante on pc.Fabricante equals f.Fabricante
                                    join pci in db.TprodutoCatalogoImagem on pc.Id equals pci.IdProdutoCatalogo into img
                                    from Timg in img.DefaultIfEmpty()
                                    join p1 in
                                    (
                                        from pci in db.TprodutoCatalogoItem
                                        join pcp in db.TProdutoCatalogoPropriedade on pci.IdProdutoCatalogoPropriedade equals pcp.id
                                        where pcp.descricao.Trim().ToUpper() == "Código Alfanumérico do Fabricante".ToUpper()
                                        select new { p1IdProdutoCatalogo = pci.IdProdutoCatalogo, p1Id = 0, p1Valor = pci.Valor }
                                    ) on pc.Id equals p1.p1IdProdutoCatalogo into TempP1
                                    from TTempP1 in TempP1.DefaultIfEmpty()
                                    join p2 in
                                    (
                                         from pci in db.TprodutoCatalogoItem
                                         join pcp in db.TProdutoCatalogoPropriedade on pci.IdProdutoCatalogoPropriedade equals pcp.id
                                         join pcpo in db.TProdutoCatalogoPropriedadeOpcao
                                         on new { a = pci.IdProdutoCatalogoPropriedade, b = pci.IdProdutoCatalogoPropriedadeOpcao.Value } equals new { a = pcpo.id_produto_catalogo_propriedade, b = pcpo.id }
                                         where pcp.descricao.Trim().ToUpper() == "Capacidade (BTU/h)".ToUpper()
                                         select new { p2IdProdutoCatalogo = pci.IdProdutoCatalogo, p2Id = pcpo.id, p2Valor = pcpo.valor }
                                    ) on pc.Id equals p2.p2IdProdutoCatalogo into TempP2
                                    from TTempP2 in TempP2.DefaultIfEmpty()
                                    join p3 in
                                    (
                                         from pci in db.TprodutoCatalogoItem
                                         join pcp in db.TProdutoCatalogoPropriedade on pci.IdProdutoCatalogoPropriedade equals pcp.id
                                         join pcpo in db.TProdutoCatalogoPropriedadeOpcao
                                         on new { a = pci.IdProdutoCatalogoPropriedade, b = pci.IdProdutoCatalogoPropriedadeOpcao.Value } equals new { a = pcpo.id_produto_catalogo_propriedade, b = pcpo.id }
                                         where pcp.descricao.Trim().ToUpper() == "Ciclo".ToUpper()
                                         select new { p3IdProdutoCatalogo = pci.IdProdutoCatalogo, p3Id = pcpo.id, p3Valor = pcpo.valor }
                                    ) on pc.Id equals p3.p3IdProdutoCatalogo into TempP3
                                    from TTempP3 in TempP3.DefaultIfEmpty()
                                    join p4 in
                                    (
                                         from pci in db.TprodutoCatalogoItem
                                         join pcp in db.TProdutoCatalogoPropriedade on pci.IdProdutoCatalogoPropriedade equals pcp.id
                                         join pcpo in db.TProdutoCatalogoPropriedadeOpcao
                                         on new { a = pci.IdProdutoCatalogoPropriedade, b = pci.IdProdutoCatalogoPropriedadeOpcao.Value } equals new { a = pcpo.id_produto_catalogo_propriedade, b = pcpo.id }
                                         where pcp.descricao.Trim().ToUpper() == "Tipo da Unidade".ToUpper()
                                         select new { p4IdProdutoCatalogo = pci.IdProdutoCatalogo, p4Id = pcpo.id, p4Valor = pcpo.valor }
                                    ) on pc.Id equals p4.p4IdProdutoCatalogo into TempP4
                                    from TTempP4 in TempP4.DefaultIfEmpty()
                                    join p5 in
                                    (
                                         from pci in db.TprodutoCatalogoItem
                                         join pcp in db.TProdutoCatalogoPropriedade on pci.IdProdutoCatalogoPropriedade equals pcp.id
                                         join pcpo in db.TProdutoCatalogoPropriedadeOpcao
                                         on new { a = pci.IdProdutoCatalogoPropriedade, b = pci.IdProdutoCatalogoPropriedadeOpcao.Value } equals new { a = pcpo.id_produto_catalogo_propriedade, b = pcpo.id }
                                         where pcp.descricao.Trim().ToUpper() == "Descarga Condensadora".ToUpper()
                                         select new { p5IdProdutoCatalogo = pci.IdProdutoCatalogo, p5Id = pcpo.id, p5Valor = pcpo.valor }
                                    ) on pc.Id equals p5.p5IdProdutoCatalogo into TempP5
                                    from TTempP5 in TempP5.DefaultIfEmpty()
                                    join p6 in
                                    (
                                         from pci in db.TprodutoCatalogoItem
                                         join pcp in db.TProdutoCatalogoPropriedade on pci.IdProdutoCatalogoPropriedade equals pcp.id
                                         join pcpo in db.TProdutoCatalogoPropriedadeOpcao
                                         on new { a = pci.IdProdutoCatalogoPropriedade, b = pci.IdProdutoCatalogoPropriedadeOpcao.Value } equals new { a = pcpo.id_produto_catalogo_propriedade, b = pcpo.id }
                                         where pcp.descricao.Trim().ToUpper() == "Voltagem".ToUpper()
                                         select new { p6IdProdutoCatalogo = pci.IdProdutoCatalogo, p6Id = pcpo.id, p6Valor = pcpo.valor }
                                    ) on pc.Id equals p6.p6IdProdutoCatalogo into TempP6
                                    from TTempP6 in TempP6.DefaultIfEmpty()
                                    select new ProdutoCatalogoListarDto
                                    {
                                        Id = pc.Id,
                                        Codigo = pc.Produto,
                                        CodigoFabricante = f.Fabricante,
                                        Fabricante = f.Nome,
                                        CodAlfanumericoFabricante = TTempP1.p1Valor,
                                        DescricaoCompleta = pc.Descricao,
                                        IdCapacidade = (int?)TTempP2.p2Id,
                                        Capacidade = TTempP2.p2Valor,
                                        IdCiclo = (int?)TTempP3.p3Id,
                                        Ciclo = TTempP3.p3Valor,
                                        IdTipoUnidade = (int?)TTempP4.p4Id,
                                        TipoUnidade = TTempP4.p4Valor,
                                        IdDescargaCondensadora = (int?)TTempP5.p5Id,
                                        DescargaCondensadora = TTempP5.p5Valor,
                                        IdVoltagem = (int?)TTempP6.p6Id,
                                        Voltagem = TTempP6.p6Valor,
                                        Imagem = Timg != null ? true : false,
                                        Ativo = pc.Ativo,
                                    }).ToList();

                    if (fabricantes != null)
                    {
                        produtos = produtos.Where(x => fabricantes.Any(y => x.CodigoFabricante.Contains(y.ToString()))).ToList();
                    }

                    if (!string.IsNullOrEmpty(codAlfaNumFabricante) && codAlfaNumFabricante.Length >= 3)
                    {
                        produtos = produtos
                            .Where(f => !string.IsNullOrEmpty(f.CodAlfanumericoFabricante)
                            && f.CodAlfanumericoFabricante.Trim().ToUpper().Contains(codAlfaNumFabricante.Trim().ToUpper())).ToList();
                    }

                    if (!string.IsNullOrEmpty(descargaCondensadora))
                    {
                        produtos = produtos
                            .Where(f => f.IdDescargaCondensadora.HasValue
                            && f.IdDescargaCondensadora.Value == Convert.ToInt32(descargaCondensadora)).ToList();
                    }

                    if (voltagem != null)
                    {
                        produtos = produtos
                            .Where(x => voltagem.Any(y => x.IdVoltagem.HasValue && x.IdVoltagem.Value.ToString().Contains(y.ToString()))).ToList();
                    }

                    if (capacidade != null)
                    {
                        produtos = produtos
                            .Where(x => capacidade.Any(y => x.IdCapacidade.HasValue && x.IdCapacidade.Value.ToString().Contains(y.ToString()))).ToList();
                    }

                    if (!string.IsNullOrEmpty(ciclo))
                    {
                        produtos = produtos
                            .Where(f => f.IdCiclo.HasValue
                            && f.IdCiclo.Value == Convert.ToInt32(ciclo)).ToList();
                    }

                    if (tipoUnidade != null)
                    {
                        produtos = produtos
                            .Where(x => tipoUnidade.Any(y => x.IdTipoUnidade.HasValue && x.IdTipoUnidade.Value.ToString().Contains(y.ToString()))).ToList();
                    }

                    if (imagem.HasValue)
                    {
                        produtos = produtos.Where(x => x.Imagem == imagem.Value).ToList();
                    }

                    if (ativo.HasValue)
                    {
                        produtos = produtos.Where(x => x.Ativo == ativo.Value).ToList();
                    }

                    return produtos;
                }
            }
            catch
            {
                throw;
            }
        }

        public TprodutoCatalogo Detalhes(int id)
        {
            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var saida = (from pc in db.TprodutoCatalogo
                                 join f in db.Tfabricante on pc.Fabricante equals f.Fabricante
                                 select new TprodutoCatalogo
                                 {
                                     Id = pc.Id,
                                     Nome = pc.Nome,
                                     Fabricante = f.Fabricante,
                                     Descricao = pc.Descricao,
                                     Produto = pc.Produto,
                                     UsuarioCadastro = pc.UsuarioCadastro,
                                     UsuarioEdicao = pc.UsuarioEdicao,
                                     DtCadastro = pc.DtCadastro,
                                     DtEdicao = pc.DtEdicao,
                                     Ativo = pc.Ativo
                                 })
                        .FirstOrDefault(x => x.Id == id);

                    if (saida != null)
                    {
                        saida.campos = ObterListaItens(id);
                        saida.imagem = ObterListaImagensPorId(id).FirstOrDefault();

                        if (saida.imagem == null)
                        {
                            saida.imagem = new TprodutoCatalogoImagem() { Caminho = "sem-imagem.png" };
                        }

                        return saida;
                    }

                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public List<TprodutoCatalogoItem> ObterListaItens(int id)
        {
            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    var existeItem = (from item
                                      in db.TprodutoCatalogoItem
                                      where item.IdProdutoCatalogo == id
                                      select item).Any();

                    if (!existeItem)
                    {
                        return new List<TprodutoCatalogoItem>();
                    }

                    return (
                            from item in db.TprodutoCatalogoItem
                            .Where(x => x.IdProdutoCatalogo == id).DefaultIfEmpty()
                            select new TprodutoCatalogoItem
                            {
                                IdProdutoCatalogo = item.IdProdutoCatalogo,
                                IdProdutoCatalogoPropriedade = item.IdProdutoCatalogoPropriedade,
                                IdProdutoCatalogoPropriedadeOpcao = item.IdProdutoCatalogoPropriedadeOpcao,
                                Valor = item.Valor,
                                Oculto = item.Oculto
                            }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TprodutoCatalogoItem> ObterListaItensComTransacao(int idProduto, ContextoBdGravacao contextoBdGravacao)
        {
            var retorno = from item in contextoBdGravacao.TprodutoCatalogoItem
                          where item.IdProdutoCatalogo == idProduto
                          select item;

            return retorno.ToList();
        }

        public List<TprodutoCatalogoImagem> ObterListaImagensPorId(int id)
        {
            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    return db.TprodutoCatalogoImagem
                        .AsNoTracking()
                        .Where(x => x.IdProdutoCatalogo == id)
                        .OrderBy(x => x.Ordem)
                        .ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TprodutoCatalogoImagem> ObterListaImagens(List<int> idProdutos)
        {
            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {

                    return db.TprodutoCatalogoImagem
                        .Where(x => idProdutos.Contains(x.IdProdutoCatalogo))
                        .OrderBy(x => x.Ordem)
                        .ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool SalvarArquivo(string nomeArquivo, int idProdutoCatalogo, int idTipo, string ordem)
        {
            var saida = false;

            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    db.TprodutoCatalogoImagem.Add(
                        new TprodutoCatalogoImagem
                        {
                            IdProdutoCatalogo = idProdutoCatalogo,
                            IdTipoImagem = idTipo,
                            Caminho = nomeArquivo,
                            Ordem = int.Parse(ordem),
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

        public TprodutoCatalogo Criar(TprodutoCatalogo produtoCatalogo, string usuario_cadastro)
        {
            TprodutoCatalogo prodCatalogo = null;

            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    prodCatalogo = db.TprodutoCatalogo.Add(
                        new TprodutoCatalogo
                        {
                            Produto = produtoCatalogo.Produto?.PadLeft(6, '0'),
                            Fabricante = produtoCatalogo.Fabricante,
                            Nome = produtoCatalogo.Nome,
                            Descricao = produtoCatalogo.Descricao,
                            UsuarioCadastro = usuario_cadastro,
                            DtCadastro = DateTime.Now,
                            Ativo = produtoCatalogo.Ativo,
                            campos = new List<TprodutoCatalogoItem>(),
                            imagem = new TprodutoCatalogoImagem()
                        }).Entity;

                    db.SaveChanges();
                    db.transacao.Commit();

                    if (prodCatalogo != null && prodCatalogo.Id > 0)
                    {
                        foreach (var campo in produtoCatalogo?.campos)
                        {
                            prodCatalogo.campos.Add(
                                new TprodutoCatalogoItem
                                {
                                    IdProdutoCatalogo = prodCatalogo.Id,
                                    IdProdutoCatalogoPropriedade = campo.IdProdutoCatalogoPropriedade,
                                    IdProdutoCatalogoPropriedadeOpcao = campo.IdProdutoCatalogoPropriedadeOpcao == -1 ? null : campo.IdProdutoCatalogoPropriedadeOpcao,
                                    Valor = campo.Valor,
                                    Oculto = campo.Oculto
                                });
                        }

                        if (produtoCatalogo?.imagem != null)
                        {
                            prodCatalogo.imagem = new TprodutoCatalogoImagem()
                            {
                                IdProdutoCatalogo = prodCatalogo.Id,
                                IdTipoImagem = 1,
                                Caminho = prodCatalogo.imagem.Caminho,
                                Ordem = prodCatalogo.imagem.Ordem
                            };
                        }

                        //if (produtoCatalogo?.imagens != null)
                        //{
                        //    foreach (var img in produtoCatalogo?.imagens)
                        //    {
                        //        prodCatalogo.imagens.Add(
                        //            new TprodutoCatalogoImagem
                        //            {
                        //                IdProdutoCatalogo = prodCatalogo.Id,
                        //                IdTipoImagem = 1,
                        //                Caminho = img.Caminho,
                        //                Ordem = img.Ordem
                        //            });
                        //    }
                        //}
                    }

                    db.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return prodCatalogo;
        }

        public TprodutoCatalogo CriarComTransacao(TprodutoCatalogo produtoCatalogo, string usuario_cadastro,
            ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.Add(produtoCatalogo);
            contextoBdGravacao.SaveChanges();
            return produtoCatalogo;
        }

        public TprodutoCatalogoItem CriarItensComTransacao(TprodutoCatalogoItem produtoCatalogoItem,
            ContextoBdGravacao contextoBdGravacao)
        {
            var existe = (from c in contextoBdGravacao.TprodutoCatalogoItem
                          where c.IdProdutoCatalogo == produtoCatalogoItem.IdProdutoCatalogo &&
                                c.IdProdutoCatalogoPropriedade == produtoCatalogoItem.IdProdutoCatalogoPropriedade
                          select c).FirstOrDefault();
            if (existe == null)
            {
                contextoBdGravacao.Add(produtoCatalogoItem);
                contextoBdGravacao.SaveChanges();
                return produtoCatalogoItem;
            }

            return null;
        }
        public bool CriarItens(TprodutoCatalogoItem produtoCatalogoItem)
        {
            var saida = false;

            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    var existe = (from c in db.TprodutoCatalogoItem
                                  where c.IdProdutoCatalogo == produtoCatalogoItem.IdProdutoCatalogo &&
                                        c.IdProdutoCatalogoPropriedade == produtoCatalogoItem.IdProdutoCatalogoPropriedade
                                  select c).FirstOrDefault();

                    if (existe == null)
                    {
                        db.TprodutoCatalogoItem.Add(produtoCatalogoItem);

                        db.SaveChanges();
                        db.transacao.Commit();
                        db.Dispose();
                    }


                    saida = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return saida;
        }

        internal bool ExcluirItens(TprodutoCatalogo obj)
        {
            var saida = false;

            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    foreach (var item in obj.campos)
                    {
                        var itemResponse = (from c in db.TprodutoCatalogoItem
                                            where c.IdProdutoCatalogo == item.IdProdutoCatalogo
                                            select c).FirstOrDefault();
                        if (itemResponse != null)
                        {
                            db.Remove(itemResponse);
                            db.SaveChanges();
                        }
                    }

                    saida = true;
                    db.transacao.Commit();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return saida;
        }

        public bool ExcluirItensComTransacao(TprodutoCatalogoItem model, ContextoBdGravacao contextoBdGravacao)
        {

            var prop = (from c in contextoBdGravacao.TprodutoCatalogoItem
                        where model.IdProdutoCatalogo == c.IdProdutoCatalogo &&
                              model.IdProdutoCatalogoPropriedade == c.IdProdutoCatalogoPropriedade
                        select c).AsNoTracking().FirstOrDefault();

            if (prop != null)
            {

                contextoBdGravacao.TprodutoCatalogoItem.Remove(prop);
                contextoBdGravacao.SaveChanges();
                return true;
            }

            return false;
        }

        public bool ExcluirItensPorIdProdutoCatalogoComTransacao(int id, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.TprodutoCatalogoItem.Remove(new TprodutoCatalogoItem() { IdProdutoCatalogo = id });
            contextoBdGravacao.SaveChanges();

            return true;
        }

        public bool ExcluirPorIdProdutoCatalogoComTransacao(int id, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.TprodutoCatalogo.Remove(new TprodutoCatalogo() { Id = id });
            contextoBdGravacao.SaveChanges();

            return true;
        }

        public bool CriarImagens(TprodutoCatalogoImagem img)
        {
            var saida = false;

            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    db.TprodutoCatalogoImagem.Add(img);

                    db.SaveChanges();
                    db.transacao.Commit();
                    db.Dispose();

                    saida = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return saida;
        }

        public TprodutoCatalogoImagem CriarImagensComTransacao(TprodutoCatalogoImagem img, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.Add(img);
            contextoBdGravacao.SaveChanges();
            return img;
        }

        public List<TprodutoCatalogoImagemTipo> BuscarTipoImagemComTransacao(TprodutoCatalogoImagemTipoFiltro filtro, ContextoBdGravacao contextoBdGravacao)
        {
            var tipoImagem = from c in contextoBdGravacao.TprodutoCatalogoImagemTipo
                             select c;
            if (tipoImagem == null)
                return null;

            if (filtro.Id != 0)
            {
                tipoImagem = tipoImagem.Where(x => x.Id == filtro.Id);
            }

            return tipoImagem.ToList();
        }

        public List<TcfgDataType> ObterDataTypesPorFiltro(TcfgDataTypeFiltro filtro)
        {
            if (filtro == null) return null;

            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    var tCfgDataTypes = from c in db.TcfgDataType
                                        select c;

                    if (filtro.Id != 0)
                    {
                        tCfgDataTypes = tCfgDataTypes.Where(x => x.Id == filtro.Id);
                    }

                    return tCfgDataTypes.ToList();
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<TcfgTipoPropriedadeProdutoCatalogo> ObterTipoPropriedadesPorFiltro(TcfgTipoPropriedadeProdutoCatalogoFiltro filtro)
        {
            if (filtro == null) return null;

            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    var tcfgTipoPropriedadeProdutoCatalogo = from c in db.TcfgTipoPropriedadeProdutoCatalogo
                                                             select c;


                    return tcfgTipoPropriedadeProdutoCatalogo.ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TcfgTipoPermissaoEdicaoCadastro> ObterTipoPermissaoEdicaoCadastro()
        {
            using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
            {
                var retorno = from c in db.TcfgTipoPermissaoEdicaoCadastro
                              select c;

                return retorno.ToList();
            }
        }

        public List<TprodutoGrupo> BuscarGrupos(TprodutoGrupoFiltro obj)
        {
            using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
            {
                var saida = from c in db.TprodutoGrupo
                            select c;

                if (obj.IncluirTProduto)
                {
                    saida = from c in saida
                            join tp in db.Tproduto on c.Codigo equals tp.Grupo
                            select c;
                }

                return saida.ToList();
            }
        }

        public List<TprodutoSubgrupo> BuscarProdutosSubgrupos(TprodutoSubgrupoFiltro obj)
        {
            using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
            {
                var saida = from c in db.TprodutoSubgrupo
                            select c;

                return saida.ToList();
            }
        }

        public Object ConsultarProdutoCatalogoAtivo(int id)
        {
            using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
            {
                var saida = from c in db.TprodutoCatalogo
                            join f in db.Tfabricante on c.Fabricante equals f.Fabricante
                            where c.Id == id
                            select new
                            {
                                produto = c,
                                fabricante = f
                            };

                return saida.FirstOrDefault();
            }
        }

        public IQueryable<Object> BuscarPropriedadesProdutoCatalogoAtivo(int idProdutoCatalogo)
        {
            using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
            {
                var saida = from c in db.TprodutoCatalogoItem
                            join lf1 in db.TProdutoCatalogoPropriedadeOpcao on c.IdProdutoCatalogoPropriedadeOpcao equals lf1.id into lfs
                            from d in lfs.DefaultIfEmpty()
                            join lf2 in db.TProdutoCatalogoPropriedade on c.IdProdutoCatalogoPropriedade equals lf2.id into lfs2
                            from e in lfs2.DefaultIfEmpty()
                            where c.IdProdutoCatalogo == idProdutoCatalogo &&
                                  c.Oculto == false &&
                                  e.oculto == false
                            orderby e.ordem
                            select new
                            {
                                nome = e.descricao,
                                valorTexto = c.Valor,
                                valorLista = d.valor
                            };

                return saida;
            }
        }
    }
}