using Arquivo.Dto;
using Arquivo.Requests;
using Arquivo.Responses;
using ClassesBase;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Arquivo
{
    public class ArquivoBll //: BaseData<TorcamentoCotacaoArquivos, TorcamentoCotacaoArquivosFiltro>
    {
        private readonly ILogger<ArquivoBll> _logger;
        private readonly InfraBanco.ContextoBdProvider _contextoProvider;

        public ArquivoBll(
            ILogger<ArquivoBll> logger,
            InfraBanco.ContextoBdProvider contextoProvider)
        {
            _logger = logger;
            _contextoProvider = contextoProvider;
        }

        public async Task<ArquivoObterEstruturaResponse> ArquivoObterEstrutura(ArquivoObterEstruturaRequest request)
        {
            var response = new ArquivoObterEstruturaResponse();

            try
            {
                var lista = this.ObterEstrutura();

                var root = lista.Where(x => x.Pai == null).FirstOrDefault();

                var data = new List<Child>{
                new Child
                {
                    data = new Data {
                        key = root.Id.ToString(),
                        name = root.Nome,
                        type = "Folder",
                        size = root.Tamanho,
                        descricao = root.Descricao
                    },
                    children = lista.Where(x => x.Pai == root.Id)
                                .Select(c => new Child
                                {
                                    data = new Data
                                    {
                                        key = c.Id.ToString(),
                                        name = c.Nome,
                                        type = c.Tipo,
                                        size = c.Tamanho,
                                        descricao = c.Descricao
                                    },
                                    children = lista.Where(x => x.Pai == c.Id)
                                                .Select(d => new Child
                                                {
                                                    data = new Data
                                                    {
                                                        key = d.Id.ToString(),
                                                        name = d.Nome,
                                                        type = d.Tipo,
                                                        size = d.Tamanho,
                                                        descricao = d.Descricao
                                                    },
                                                    children = lista.Where(x => x.Pai == d.Id)
                                                                .Select(e => new Child
                                                                {
                                                                    data = new Data
                                                                    {
                                                                        key = e.Id.ToString(),
                                                                        name = e.Nome,
                                                                        type = e.Tipo,
                                                                        size = e.Tamanho,
                                                                        descricao = e.Descricao
                                                                    },
                                        children = lista.Where(x => x.Pai == c.Id)
                                                .Select(d => new Child
                                                {
                                                    data = new Data
                                                    {
                                                        key = d.Id.ToString(),
                                                        name = d.Nome,
                                                        type = d.Tipo,
                                                        size = d.Tamanho,
                                                        descricao = d.Descricao
                                                    },
                                                    children = lista.Where(x => x.Pai == d.Id)
                                                                .Select(e => new Child
                                                                {
                                                                    data = new Data
                                                                    {
                                                                        key = e.Id.ToString(),
                                                                        name = e.Nome,
                                                                        type = e.Tipo,
                                                                        size = e.Tamanho,
                                                                        descricao = e.Descricao
                                                                    },
                                                                }).ToList()
                                                }).ToList()
                                                                }).ToList()
                                                }).ToList()
                                }).ToList()
                }};

                response.Sucesso = true;
                response.Mensagem = "ObterEstrutura sucesso.";
                response.Childs = data;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ArquivoDownloadResponse> ArquivoDownload(ArquivoDownloadRequest request)
        {
            var response = new ArquivoDownloadResponse();

            if (string.IsNullOrEmpty(request.Id))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo Id.";
                return response;
            }

            if (string.IsNullOrEmpty(request.Caminho))
            {
                response.Sucesso = false;
                response.Mensagem = "Erro inesperado! Favor entrar em contato com o suporte técnico.";
                return response;
            }

            try
            {
                var caminho = Path.Combine(request.Caminho, $"{request.Id}.pdf");
                var fileinfo = new FileInfo(caminho);
                byte[] byteArray = File.ReadAllBytes(caminho);
                var arquivo = this.ObterArquivoPorID(Guid.Parse(request.Id));

                response.Sucesso = true;
                response.Mensagem = "Download efetuado com sucesso.";
                response.Nome = arquivo.Nome;
                response.FileLength = fileinfo.Length.ToString();
                response.ByteArray = byteArray;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ArquivoUploadResponse> ArquivoUpload(ArquivoUploadRequest request)
        {
            var response = new ArquivoUploadResponse();

            if (string.IsNullOrEmpty(request.Caminho))
            {
                response.Sucesso = false;
                response.Mensagem = "Erro inesperado! Favor entrar em contato com o suporte técnico.";
                return response;
            }

            if (request.Arquivo == null || string.IsNullOrEmpty(request.Arquivo.FileName))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo nome arquivo.";
                return response;
            }

            if (request.Arquivo.FileName.Length > 255)
            {
                response.Sucesso = false;
                response.Mensagem = "Nome excedeu máximo de 255 caracteres.";
                return response;
            }

            if (!request.Arquivo.ContentType.Equals("application/pdf") && !request.Arquivo.ContentType.Equals("pdf"))
            {
                response.Sucesso = false;
                response.Mensagem = "Formato inválido. O arquivo deve ser no formato PDF.";
                return response;
            }

            try
            {
                var idArquivo = Guid.NewGuid();

                var file = Path.Combine(request.Caminho, $"{idArquivo}.pdf");

                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await request.Arquivo.CopyToAsync(fileStream);
                }

                var tamanho = new FileInfo(file).Length;
                
                this.Inserir(new TorcamentoCotacaoArquivos()
                {
                    Id = idArquivo,
                    Nome = request.Arquivo.FileName,
                    Pai = !string.IsNullOrEmpty(request.IdPai) ? Guid.Parse(request.IdPai) : (Guid?)null,
                    Descricao = string.Empty,
                    Tamanho = calculaTamanho(tamanho),
                    Tipo = "File"
                });

                response.Sucesso = true;
                response.Mensagem = "Arquivo salvo com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ArquivoCriarPastaResponse> ArquivoCriarPasta(ArquivoCriarPastaRequest request)
        {
            var response = new ArquivoCriarPastaResponse();

            if (string.IsNullOrEmpty(request.IdPai))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo IdPai.";
                return response;
            }

            if (string.IsNullOrEmpty(request.Nome))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo Nome.";
                return response;
            }

            try
            {
                var tOrcamentoCotacaoArquivos = this.Inserir(new TorcamentoCotacaoArquivos()
                {
                    Id = Guid.NewGuid(),
                    Nome = request.Nome,
                    Pai = !string.IsNullOrEmpty(request.IdPai) ? Guid.Parse(request.IdPai) : (Guid?)null,
                    Descricao = request.Nome,
                    Tamanho = "",
                    Tipo = "Folder"
                });


                response.Id = tOrcamentoCotacaoArquivos.Id;
                response.Sucesso = true;
                response.Mensagem = $"Pasta '{request.Nome}' criada com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ArquivoEditarResponse> ArquivoEditar(ArquivoEditarRequest request)
        {
            var response = new ArquivoEditarResponse();

            if (string.IsNullOrEmpty(request.Id))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo Id.";
                return response;
            }

            if (string.IsNullOrEmpty(request.Nome))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo Nome.";
                return response;
            }

            if (request.Descricao.Length > 500)
            {
                response.Sucesso = false;
                response.Mensagem = "Descrição excedeu máximo de 500 caracteres.";
                return response;
            }

            try
            {
                var retorno = this.Editar(new TorcamentoCotacaoArquivos
                {
                    Id = Guid.Parse(request.Id),
                    Nome = request.Nome,
                    Descricao = request.Descricao
                });

                response.Sucesso = true;
                response.Mensagem = "Salvo com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ArquivoExcluirResponse> ArquivoExcluir(ArquivoExcluirRequest request)
        {
            var response = new ArquivoExcluirResponse();

            if (string.IsNullOrEmpty(request.Id))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo Id.";
                return response;
            }

            if (string.IsNullOrEmpty(request.Caminho))
            {
                response.Sucesso = false;
                response.Mensagem = "Erro inesperado! Favor entrar em contato com o suporte técnico.";
                return response;
            }

            try
            {
                var retorno = this.Excluir(new TorcamentoCotacaoArquivos
                {
                    Id = Guid.Parse(request.Id)
                });

                var file = Path.Combine(request.Caminho, $"{request.Id}.pdf");

                if (retorno)
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }

                response.Sucesso = true;
                response.Mensagem = "Exclusão concluida com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<TorcamentoCotacaoArquivos> ObterEstrutura()
        {
            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    return (
                        from orcamentoCotacaoArquivos
                        in db.TorcamentoCotacaoArquivos
                        orderby orcamentoCotacaoArquivos.Nome
                        select orcamentoCotacaoArquivos)
                    .ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private TorcamentoCotacaoArquivos Inserir(TorcamentoCotacaoArquivos obj)
        {
            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    db.TorcamentoCotacaoArquivos.Add(obj);
                    db.SaveChanges();
                    db.transacao.Commit();
                    return obj;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private bool Editar(TorcamentoCotacaoArquivos obj)
        {
            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var arquivo = db.TorcamentoCotacaoArquivos.First(x => x.Id == obj.Id);
                    arquivo.Nome = obj.Nome;
                    arquivo.Descricao = obj.Descricao;
                    db.SaveChanges();
                    db.transacao.Commit();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string calculaTamanho(long tamanhoBytes)
        {
            string sOut = "";
            decimal saida = 0;

            if (tamanhoBytes / 1024 <= 1024)
            {
                saida = (decimal)tamanhoBytes / 1024;
                sOut = $"{saida.ToString("F0")}kb";
            }
            else
            {
                saida = (decimal)tamanhoBytes / 1024 / 1024;
                sOut = $"{saida.ToString("F")}mb";
            }

            return sOut;
        }



        public TorcamentoCotacaoArquivos Atualizar(TorcamentoCotacaoArquivos obj)
        {
            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    db.TorcamentoCotacaoArquivos.Update(obj);
                    db.SaveChanges();
                    return obj;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TorcamentoCotacaoArquivos> PorFiltro(TorcamentoCotacaoArquivosFiltro obj)
        {
            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var saida = (from orcamentoCotacaoArquivos in db.TorcamentoCotacaoArquivos
                                 select orcamentoCotacaoArquivos);

                    if (obj.id.HasValue)
                    {
                        saida = saida.Where(x => x.Id == obj.id);
                    }

                    return saida.ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        public TorcamentoCotacaoArquivos ObterArquivoPorID(Guid id)
        {
            return PorFiltro(new TorcamentoCotacaoArquivosFiltro() { id = id }).FirstOrDefault();
        }

        public bool Excluir(TorcamentoCotacaoArquivos obj)
        {
            try
            {
                using (var db = _contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var objeto = db.TorcamentoCotacaoArquivos.FirstOrDefault(x => x.Id == obj.Id);

                    if (objeto != null)
                    {
                        db.TorcamentoCotacaoArquivos.Remove(objeto);
                        db.SaveChanges();
                        db.transacao.Commit();
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public TorcamentoCotacaoArquivos InserirComTransacao(TorcamentoCotacaoArquivos model, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacaoArquivos> PorFilroComTransacao(TorcamentoCotacaoArquivosFiltro obj, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoArquivos AtualizarComTransacao(TorcamentoCotacaoArquivos model, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(TorcamentoCotacaoArquivos obj, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }
    }
}
