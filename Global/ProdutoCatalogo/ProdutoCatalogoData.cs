using ClassesBase;
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
    public class ProdutoCatalogoData : BaseData<TprodutoCatalogo, TprodutoCatalogoFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public ProdutoCatalogoData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TprodutoCatalogo Atualizar(TprodutoCatalogo obj)
        {
            var saida = false;
            TprodutoCatalogo produtoCatalogo = new TprodutoCatalogo();
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    if (AtualizarItens(obj))
                    {
                        produtoCatalogo = db.TprodutoCatalogo.FirstOrDefault(x => x.Id == obj.Id);

                        if (obj != null)
                        {
                            produtoCatalogo.Nome = obj.Nome == null ? "" : obj.Nome;
                            produtoCatalogo.Descricao = obj.Descricao;
                            produtoCatalogo.UsuarioEdicao = obj.UsuarioEdicao;
                            produtoCatalogo.DtEdicao = DateTime.Now;
                            produtoCatalogo.Ativo = obj.Ativo;
                            db.SaveChanges();
                            db.transacao.Commit();
                            saida = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return saida ? produtoCatalogo : null;
        }

        public bool Excluir(TprodutoCatalogo obj)
        {
            var saida = false;

            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {

                    var query = db.TprodutoCatalogo.FirstOrDefault(x => x.Id == obj.Id);

                    if (obj != null)
                    {
                        query.Ativo = false;
                        db.SaveChanges();
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
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {

                    var obj = db.TprodutoCatalogoImagem.FirstOrDefault(x => x.Id == idImagem && x.IdProdutoCatalogo == idProduto);

                    if (obj != null)
                    {
                        db.TprodutoCatalogoImagem.Remove(obj);
                        db.SaveChanges();
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

        public TprodutoCatalogo Inserir(TprodutoCatalogo obj)
        {
            throw new NotImplementedException();
        }

        /*
        public List<TprodutoCatalogo> PorFiltro(TprodutoCatalogoFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    return db.TprodutoCatalogo
                        
                        .ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }*/

        public List<TprodutoCatalogo> PorFiltro(TprodutoCatalogoFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    return (
                            from pc in db.TprodutoCatalogo
                            join f in db.Tfabricantes on pc.Fabricante equals f.Fabricante

                            select new TprodutoCatalogo
                            {         
                                Produto = pc.Produto,
                                Id = pc.Id,
                                Fabricante = f.Nome,
                                Nome = pc.Nome,
                                Descricao = pc.Descricao,
                                Ativo = pc.Ativo
                            }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TprodutoCatalogo Detalhes(int id)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var saida = db.TprodutoCatalogo.FirstOrDefault(x => x.Id == id);

                    if (saida != null)
                    {
                        //saida.campos = ObterListaItens(id);
                        saida.imagens = ObterListaImagens(id);

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
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
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

        public List<TprodutoCatalogoImagem> ObterListaImagens(int id)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
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

        private bool AtualizarItens(TprodutoCatalogo produto)
        {
            var saida = false;

            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    var itens = db.TprodutoCatalogoItem.Where(x => x.IdProdutoCatalogo == produto.Id);
                    if (itens != null)
                    {
                        db.TprodutoCatalogoItem.RemoveRange(itens);
                        //db.TprodutoCatalogoItem.FromSql($"DELETE t_PRODUTO_CATALOGO_ITEM WHERE id_produto_catalogo = {produto.Id}");
                        db.SaveChanges();
                    }

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
                    saida = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return saida;
        }

        public bool SalvarArquivo(string nomeArquivo, int idProdutoCatalogo, int idTipo, string ordem)
        {
            var saida = false;

            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
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
                    saida = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return saida;
        }

        public bool Criar(TprodutoCatalogo produtoCatalogo, string usuario_cadastro)
        {
            var saida = false;

            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    db.TprodutoCatalogo.Add(
                        new TprodutoCatalogo
                        {
                            //Id = produtoCatalogo.Id,
                            Produto = produtoCatalogo.Produto,
                            Fabricante = produtoCatalogo.Fabricante,
                            Nome = produtoCatalogo.Nome,                                                        
                            Descricao = produtoCatalogo.Descricao,
                            UsuarioCadastro = usuario_cadastro,
                            DtCadastro = DateTime.Now,
                            Ativo = true
                            
                        }); ;

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

        public bool CriarItem(TprodutoCatalogoItem produtoCatalogoItem)
        {
            var saida = false;

            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {
                    db.TprodutoCatalogoItem.Add(
                        new TprodutoCatalogoItem
                        {
                            IdProdutoCatalogo = produtoCatalogoItem.IdProdutoCatalogo,
                            IdProdutoCatalogoPropriedade = produtoCatalogoItem.IdProdutoCatalogoPropriedade,
                            IdProdutoCatalogoPropriedadeOpcao = produtoCatalogoItem.IdProdutoCatalogoPropriedadeOpcao,
                            Valor = produtoCatalogoItem.Valor,
                            Oculto = produtoCatalogoItem.Oculto
                        }); ;

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

        public bool ExisteProduto(TprodutoCatalogo produtoCatalogo)
        {
            var saida = false;

            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
                {

                    bool? obj = db.TprodutoCatalogo.Any(x => x.Id == produtoCatalogo.Id);

                    if (obj != null)
                    {
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

        public TprodutoCatalogo InserirComTransacao(TprodutoCatalogo model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TprodutoCatalogo> PorFilroComTransacao(TprodutoCatalogoFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }
    }
}
