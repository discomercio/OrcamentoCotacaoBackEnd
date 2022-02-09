using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ProdutoCatalogo
{
    public class ProdutoCatalogoBll : BaseBLL<TprodutoCatalogo, TprodutoCatalogoFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;
        private readonly ProdutoCatalogoData _data;

        public ProdutoCatalogoBll(ContextoBdProvider contextoBdProvider) : base(new ProdutoCatalogoData(contextoBdProvider))
        {
            this.contextoProvider = contextoBdProvider;
            _data = (ProdutoCatalogoData)base.data;
        }

        //public async Task<List<TprodutoCatalogo>> Listar(int page, int pageItens, int idCliente, string tipoUsuario, string usuario)
        //{
        //    try
        //    {
        //        using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
        //        {
        //            return await db.TprodutoCatalogo.ToListAsync();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public async Task<TprodutoCatalogo> ObterPorId(string id)
        //{
        //    try
        //    {
        //        using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
        //        {
        //            return await db.TprodutoCatalogo
        //                .AsNoTracking()
        //                .FirstOrDefaultAsync(x => x.Id == id);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public TprodutoCatalogo Detalhes(string id)
        {
            try
            {
                return _data.Detalhes(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public bool Excluir(string id)
        //{
        //    var saida = false;

        //    try
        //    {
        //        using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
        //        {

        //            var obj = db.TprodutoCatalogo.FirstOrDefault(x => x.Id == id);

        //            if (obj != null)
        //            {
        //                obj.Ativo = false;
        //                await db.SaveChangesAsync();
        //                saida = true;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }

        //    return saida;
        //}

        public bool ExcluirImagem(string idProduto, string idImagem)
        {
            return _data.ExcluirImagem(idProduto, idImagem);
            //var saida = false;

            //try
            //{
            //    using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
            //    {

            //        var obj = db.TprodutoCatalogoImagem.FirstOrDefault(x => x.Id == int.Parse(idImagem) && x.IdProdutoCatalogo == idProduto);

            //        if (obj != null)
            //        {
            //            db.TprodutoCatalogoImagem.Remove(obj);
            //            await db.SaveChangesAsync();
            //            saida = true;
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}

            //return saida;
        }

        public List<TprodutoCatalogoItens> ObterListaItens(string id)
        {
            return _data.ObterListaItens(id);
            //try
            //{
            //    using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
            //    {
            //        return await (from itens in db.TprodutoCatalogoItens
            //                      from item in db.TprodutoCatalogoItem
            //                      .Where(x => x.IdProdutoCatalogoItens == itens.Id && x.IdProdutoCatalogo == id).DefaultIfEmpty()
            //                      select new TprodutoCatalogoItens
            //                      {
            //                          Id = itens.Id,
            //                          Codigo = item.IdProdutoCatalogo,
            //                          Ordem = itens.Ordem,
            //                          Chave = itens.Valor,
            //                          Valor = item.Valor
            //                      }).ToListAsync();
            //    }
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
        }

        public List<TprodutoCatalogoImagem> ObterListaImagens(string id)
        {
            return _data.ObterListaImagens(id);
            //try
            //{
            //    using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
            //    {
            //        return await db.TprodutoCatalogoImagem
            //            .AsNoTracking()
            //            .Where(x => x.IdProdutoCatalogo == id)
            //            .OrderBy(x => x.Ordem)
            //            .ToListAsync();
            //    }
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
        }

        //public async Task<bool> Atualizar(TprodutoCatalogo produto, string usuario)
        //{
        //    var saida = false;

        //    try
        //    {
        //        using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
        //        {
        //            if (AtualizarItens(produto, usuario, db))
        //            {
        //                var obj = db.TprodutoCatalogo.FirstOrDefault(x => x.Id == produto.Id);

        //                if (obj != null)
        //                {
        //                    obj.Nome = produto.Nome == null ? "" : produto.Nome;
        //                    obj.Descricao = produto.Descricao;
        //                    obj.UsuarioEdicao = usuario;
        //                    obj.DtEdicao = DateTime.Now;
        //                    obj.Ativo = produto.Ativo;
        //                    await db.SaveChangesAsync();
        //                    db.transacao.Commit();
        //                    saida = true;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }

        //    return saida;
        //}

        //private bool AtualizarItens(TprodutoCatalogo produto, string usuario, ContextoBdGravacao db)
        //{
        //    var saida = false;

        //    try
        //    {
        //        var itens = db.TprodutoCatalogoItem.Where(x => x.IdProdutoCatalogo == produto.Id);
        //        if (itens != null)
        //        {
        //            db.TprodutoCatalogoItem.RemoveRange(itens);
        //            //db.TprodutoCatalogoItem.FromSql($"DELETE t_PRODUTO_CATALOGO_ITEM WHERE id_produto_catalogo = {produto.Id}");
        //            db.SaveChanges();
        //        }

        //        foreach (var campo in produto.campos)
        //        {
        //            db.TprodutoCatalogoItem.Add(
        //                new TprodutoCatalogoItem
        //                {
        //                    IdProdutoCatalogo = produto.Id,
        //                    IdProdutoCatalogoItens = campo.Id,
        //                    Valor = campo.Valor
        //                });
        //        }

        //        db.SaveChanges();
        //        saida = true;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }

        //    return saida;
        //}

        public bool SalvarArquivo(string nomeArquivo, string idProdutoCatalogo, string idTipo, string ordem)
        {
            return _data.SalvarArquivo(nomeArquivo, idProdutoCatalogo, idTipo, ordem);
            //var saida = false;

            //try
            //{
            //    using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
            //    {
            //        db.TprodutoCatalogoImagem.Add(
            //            new TprodutoCatalogoImagem
            //            {
            //                IdProdutoCatalogo = idProdutoCatalogo,
            //                IdTipoImagem = int.Parse(idTipo),
            //                Caminho = nomeArquivo,
            //                Ordem = int.Parse(ordem),
            //            });

            //        await db.SaveChangesAsync();
            //        saida = true;
            //    }
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}

            //return saida;
        }

        public bool Criar(TprodutoCatalogo produtoCatalogo, string usuario_cadastro)
        {
            return _data.Criar(produtoCatalogo, usuario_cadastro);
            //var saida = false;

            //try
            //{
            //    using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
            //    {
            //        db.TprodutoCatalogo.Add(
            //            new TprodutoCatalogo
            //            {
            //                Id = produtoCatalogo.Id,
            //                Nome = produtoCatalogo.Nome,
            //                Descricao = produtoCatalogo.Descricao,
            //                UsuarioCadastro = usuario_cadastro,
            //                DtCadastro = DateTime.Now
            //            }); ;

            //        await db.SaveChangesAsync();
            //        db.transacao.Commit();
            //        saida = true;
            //    }
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}

            //return saida;
        }

        //public async Task<bool> ExisteProduto(TprodutoCatalogo produtoCatalogo)
        //{
        //    var saida = false;

        //    try
        //    {
        //        using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
        //        {

        //            var obj = await db.TprodutoCatalogo.AnyAsync(x => x.Id == produtoCatalogo.Id);

        //            if (obj != null)
        //            {
        //                saida = true;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }

        //    return saida;
        //}
    }
}
