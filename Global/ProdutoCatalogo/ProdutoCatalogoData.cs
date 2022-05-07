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

        private bool AtualizarItens(TprodutoCatalogo produto)
        {
            var saida = false;

            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    var itens = db.TprodutoCatalogoItem.Where(x => x.IdProdutoCatalogo == produto.Id);
                    if (itens != null)
                    {
                        db.TprodutoCatalogoItem.RemoveRange(itens);
                    }
                    db.SaveChanges();

                    foreach (var campo in produto.campos)
                    {
                        db.TprodutoCatalogoItem.Add(
                            new TprodutoCatalogoItem
                            {
                                IdProdutoCatalogo = produto.Id,
                                //IdProdutoCatalogoItens = campo.IdProdutoCatalogoItens,
                                Valor = campo.Valor
                            });
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
            List<TprodutoCatalogo> lista = null;

            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    var produtos = from pc in db.TprodutoCatalogo
                                   join f in db.Tfabricantes on pc.Fabricante equals f.Fabricante
                                   select new TprodutoCatalogo
                                   {
                                       Produto = pc.Produto,
                                       Id = pc.Id,
                                       Fabricante = f.Nome,
                                       Nome = pc.Nome,
                                       Descricao = pc.Descricao,
                                       Ativo = pc.Ativo
                                   };

                    if (produtos != null)
                    {
                        if (obj.IncluirImagem)
                        {
                            produtos = produtos.Include(x => x.imagens);
                        }
                    }

                    lista = produtos.ToList();

                    if (!String.IsNullOrEmpty(obj.Id))
                        lista = lista.Where(x => x.Id == int.Parse(obj.Id)).ToList();

                    if (!String.IsNullOrEmpty(obj.Produto))
                        lista = lista.Where(x => x.Produto.PadLeft(6, '0') == obj.Produto.PadLeft(6, '0')).ToList();

                    if (obj.Ativo)
                        lista = lista.Where(x => x.Ativo == obj.Ativo).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return lista;
        }

        public TprodutoCatalogo Detalhes(int id)
        {
            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var saida = (from pc in db.TprodutoCatalogo
                                 join f in db.Tfabricantes on pc.Fabricante equals f.Fabricante
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

                    db.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return prodCatalogo;
        }

        public bool CriarItens(TprodutoCatalogoItem produtoCatalogoItem)
        {
            var saida = false;

            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    db.TprodutoCatalogoItem.Add(produtoCatalogoItem);

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

        internal bool ExcluirItens(TprodutoCatalogo obj)
        {
            var saida = false;

            try
            {
                using (var db = _contexto)
                {
                    _contexto.Database.ExecuteSqlCommand($"delete t_PRODUTO_CATALOGO_ITEM where id_produto_catalogo = {obj.Id}");
                    db.SaveChanges();

                    saida = true;
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
    }
}
