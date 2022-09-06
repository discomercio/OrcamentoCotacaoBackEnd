using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.EntityFrameworkCore;
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

            contextoBdGravacao.SaveChanges();

            produtoCatalogo.campos = model.campos;
            produtoCatalogo.imagens = model.imagens;
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

            tProdutoCatalogoItem.IdProdutoCatalogo = model.IdProdutoCatalogo;
            tProdutoCatalogoItem.IdProdutoCatalogoPropriedade = model.IdProdutoCatalogoPropriedade;
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
                                       IdProdutoCatalogoImagem = images.Id,
                                       Ativo = pc.Ativo
                                   };


                    if (!String.IsNullOrEmpty(obj.Id))
                        produtos = produtos.Where(x => x.Id == int.Parse(obj.Id));

                    if (!String.IsNullOrEmpty(obj.Produto))
                        produtos = produtos.Where(x => x.Produto.PadLeft(6, '0') == obj.Produto.PadLeft(6, '0'));

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
                                     Fabricante = $"{f.Fabricante} - {f.Nome}",
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
                        saida.imagens = ObterListaImagensPorId(id);

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
                            imagens = new List<TprodutoCatalogoImagem>()
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

                        if (produtoCatalogo?.imagens != null)
                        {
                            foreach (var img in produtoCatalogo?.imagens)
                            {
                                prodCatalogo.imagens.Add(
                                    new TprodutoCatalogoImagem
                                    {
                                        IdProdutoCatalogo = prodCatalogo.Id,
                                        IdTipoImagem = 1,
                                        Caminho = img.Caminho,
                                        Ordem = img.Ordem
                                    });
                            }
                        }
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
    }
}
