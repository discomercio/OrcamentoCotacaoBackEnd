using Arquivo.Dto;
using Arquivo.Requests;
using Arquivo.Responses;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UtilsGlobais.Exceptions;

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

            // TESTES
            //throw new DomainException("mensagem de erro");
            //throw new DomainException("mensagem de erro", new Exception("inner mensagem de erro"));
            //throw new Exception("Teste" );
            //throw new Exception("Teste",new Exception("inner mensagem de erro"));

            try
            {
                _logger.LogInformation($"ArquivoObterEstrutura: Obter estrutura da tela de download. CorrelationId => [{request.CorrelationId}].");
                var estruturas = this.ObterEstrutura();

                _logger.LogInformation($"ArquivoObterEstrutura: Retorno estrutura de tela de download. Retorno => [{estruturas.Count}]. CorrelationId => [{request.CorrelationId}].");
                if (estruturas.Count <= 0)
                {
                    response.Sucesso = false;
                    response.Mensagem = "Não existem pastas cadastrada. Favor criar nova pasta.";
                    return response;
                }

                var child = new List<Child>();

                foreach (TorcamentoCotacaoArquivos itemPai in estruturas)
                {
                    if (itemPai.Pai == null)
                    {
                        child.Add(new Child()
                        {
                            data = new Data()
                            {
                                key = itemPai.Id.ToString(),
                                name = itemPai.Nome,
                                size = itemPai.Tamanho,
                                type = itemPai.Tipo,
                                descricao = itemPai.Descricao
                            },
                            children = ObterEstruturaFilho(estruturas, itemPai.Id)
                        });
                    }
                }

                _logger.LogInformation($"ArquivoObterEstrutura: Retorno da estrutura com sucesso. CorrelationId => [{request.CorrelationId}].");
                response.CorrelationId = request.CorrelationId;
                response.Sucesso = true;
                response.Mensagem = "Retorno da estrutura com sucesso.";
                response.Childs = child;
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

            _logger.LogInformation($"ArquivoDownload: Verificando campo preenchido. Id => [{request.Id}]. CorrelationId => [{request.CorrelationId}].");
            if (string.IsNullOrEmpty(request.Id))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo Id.";
                return response;
            }

            _logger.LogInformation($"ArquivoDownload: Verificando campo preenchido. CaminhoArquivo => [{request.CaminhoArquivo}]. CorrelationId => [{request.CorrelationId}].");
            if (string.IsNullOrEmpty(request.CaminhoArquivo))
            {
                response.Sucesso = false;
                response.Mensagem = "Erro inesperado! Favor entrar em contato com o suporte técnico.";
                return response;
            }

            try
            {
                _logger.LogInformation($"ArquivoDownload: Salvando arquivo no caminho. Arquivo => [{request.Id}]. CorrelationId => [{request.CorrelationId}].");
                var caminho = Path.Combine(request.CaminhoArquivo, $"{request.Id}.pdf");
                var fileinfo = new FileInfo(caminho);
                byte[] byteArray = File.ReadAllBytes(caminho);
                var arquivo = this.ObterArquivoPorID(Guid.Parse(request.Id));

                _logger.LogInformation($"ArquivoDownload: Download efetuado com sucesso. CorrelationId => [{request.CorrelationId}].");
                response.CorrelationId = request.CorrelationId;
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

        public async Task<ArquivoExcluirResponse> ArquivoExcluir(ArquivoExcluirRequest request)
        {
            var response = new ArquivoExcluirResponse();

            _logger.LogInformation($"ArquivoExcluir: Verificando campo preenchido. Id => [{request.Id}]. CorrelationId => [{request.CorrelationId}].");
            if (string.IsNullOrEmpty(request.Id))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo Id.";
                return response;
            }

            _logger.LogInformation($"ArquivoExcluir: Verificando campo preenchido. CaminhoArquivo => [{request.CaminhoArquivo}]. CorrelationId => [{request.CorrelationId}].");
            if (string.IsNullOrEmpty(request.CaminhoArquivo))
            {
                response.Sucesso = false;
                response.Mensagem = "Erro inesperado! Favor entrar em contato com o suporte técnico.";
                return response;
            }

            try
            {
                _logger.LogInformation($"ArquivoExcluir: Excluindo arquivo do banco de dados. Arquivo => [{request.Id}]. CorrelationId => [{request.CorrelationId}].");
                var retorno = this.Excluir(new TorcamentoCotacaoArquivos
                {
                    Id = Guid.Parse(request.Id)
                });
                
                var file = Path.Combine(request.CaminhoArquivo, $"{request.Id}.pdf");

                _logger.LogInformation($"ArquivoExcluir: Excluindo arquivo da pasta na reder banco de dados. CaminhoArquivo => [{request.CaminhoArquivo}]. Arquivo => [{request.Id}]. CorrelationId => [{request.CorrelationId}].");
                if (retorno)
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }

                _logger.LogInformation($"ArquivoExcluir: Exclusão concluida com sucesso. CorrelationId => [{request.CorrelationId}].");
                response.CorrelationId = request.CorrelationId;
                response.Sucesso = true;
                response.Mensagem = "Exclusão concluida com sucesso.";
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

            _logger.LogInformation($"ArquivoEditar: Verificando campo preenchido. Id => [{request.Id}]. CorrelationId => [{request.CorrelationId}].");
            if (string.IsNullOrEmpty(request.Id))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo Id.";
                return response;
            }

            _logger.LogInformation($"ArquivoEditar: Verificando campo preenchido. Nome => [{request.Nome}]. CorrelationId => [{request.CorrelationId}].");
            if (string.IsNullOrEmpty(request.Nome))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo Nome.";
                return response;
            }

            _logger.LogInformation($"ArquivoEditar: Verificando campo Nome excedeu máximo de 255 caracteres. CorrelationId => [{request.CorrelationId}].");
            if (request.Nome.Length > 255)
            {
                response.Sucesso = false;
                response.Mensagem = "Nome excedeu máximo de 255 caracteres.";
                return response;
            }

            _logger.LogInformation($"ArquivoEditar: Verificando campo Descricao excedeu máximo de 500 caracteres. CorrelationId => [{request.CorrelationId}].");
            if (!string.IsNullOrEmpty(request.Descricao) 
                && request.Descricao.Length > 500)
            {
                response.Sucesso = false;
                response.Mensagem = "Descrição excedeu máximo de 500 caracteres.";
                return response;
            }

            try
            {
                _logger.LogInformation($"ArquivoEditar: Editando Pasta/Arquivo do banco de dados. Arquivo => [{request.Id}]. Nome => [{request.Nome}]. Descrição => [{request.Descricao}]. CorrelationId => [{request.CorrelationId}].");
                var retorno = this.Editar(new TorcamentoCotacaoArquivos
                {
                    Id = Guid.Parse(request.Id),
                    Nome = request.Nome.Trim(),
                    Descricao = !string.IsNullOrEmpty(request.Descricao) ? request.Descricao.Trim() : string.Empty,
                });

                _logger.LogInformation($"ArquivoEditar: Salvo com sucesso. CorrelationId => [{request.CorrelationId}].");
                response.CorrelationId = request.CorrelationId;
                response.Sucesso = true;
                response.Mensagem = "Salvo com sucesso.";
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

            _logger.LogInformation($"ArquivoCriarPasta: Verificando campo preenchido. Nome => [{request.Nome}]. CorrelationId => [{request.CorrelationId}].");
            if (string.IsNullOrEmpty(request.Nome))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo Nome.";
                return response;
            }
            
            _logger.LogInformation($"ArquivoCriarPasta: Verificando campo Nome excedeu máximo de 255 caracteres. CorrelationId => [{request.CorrelationId}].");
            if (request.Nome.Length > 255)
            {
                response.Sucesso = false;
                response.Mensagem = "Nome excedeu máximo de 255 caracteres.";
                return response;
            }

            _logger.LogInformation($"ArquivoCriarPasta: Verificando campo Descrição excedeu máximo de 500 caracteres. CorrelationId => [{request.CorrelationId}].");
            if (!string.IsNullOrEmpty(request.Descricao) 
                && request.Descricao.Length > 500)
            {
                response.Sucesso = false;
                response.Mensagem = "Descrição excedeu máximo de 500 caracteres.";
                return response;
            }

            try
            {
                _logger.LogInformation($"ArquivoCriarPasta: Criando pasta no banco de dados. Nome => [{request.Nome}]. CorrelationId => [{request.CorrelationId}].");
                var tOrcamentoCotacaoArquivos = this.Inserir(new TorcamentoCotacaoArquivos()
                {
                    Id = Guid.NewGuid(),
                    Nome = request.Nome.Trim(),
                    Pai = !string.IsNullOrEmpty(request.IdPai) ? Guid.Parse(request.IdPai) : (Guid?)null,
                    Descricao = !string.IsNullOrEmpty(request.Descricao) ? request.Descricao.Trim() : string.Empty,
                    Tamanho = string.Empty,
                    Tipo = "Folder"
                });

                _logger.LogInformation($"ArquivoCriarPasta: Pasta [{request.Nome}] criada com sucesso. CorrelationId => [{request.CorrelationId}].");
                response.CorrelationId = request.CorrelationId;
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

        public async Task<ArquivoUploadResponse> ArquivoUpload(ArquivoUploadRequest request)
        {
            var response = new ArquivoUploadResponse();

            _logger.LogInformation($"ArquivoUpload: Verificando campo preenchido. CaminhoArquivo => [{request.CaminhoArquivo}]. CorrelationId => [{request.CorrelationId}].");
            if (string.IsNullOrEmpty(request.CaminhoArquivo))
            {
                response.Sucesso = false;
                response.Mensagem = "Erro inesperado! Favor entrar em contato com o suporte técnico.";
                return response;
            }

            _logger.LogInformation($"ArquivoUpload: Verificando se o upload esta foi enviado. CorrelationId => [{request.CorrelationId}].");
            if (request.Arquivo == null || string.IsNullOrEmpty(request.Arquivo.FileName))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo nome arquivo.";
                return response;
            }

            _logger.LogInformation($"ArquivoUpload: Verificando campo Nome do aquivo excedeu máximo de 255 caracteres. CorrelationId => [{request.CorrelationId}].");
            if (request.Arquivo.FileName.Length > 255)
            {
                response.Sucesso = false;
                response.Mensagem = "Nome excedeu máximo de 255 caracteres.";
                return response;
            }

            _logger.LogInformation($"ArquivoUpload: Verificando se o upload é um PDF. CorrelationId => [{request.CorrelationId}].");
            if (!request.Arquivo.ContentType.Equals("application/pdf") && !request.Arquivo.ContentType.Equals("pdf"))
            {
                response.Sucesso = false;
                response.Mensagem = "Formato inválido. O arquivo deve ser no formato PDF.";
                return response;
            }

            try
            {
                var idArquivo = Guid.NewGuid();

                _logger.LogInformation($"ArquivoUpload: Fazendo o upload do PDF. CaminhoArquivo => [{request.CaminhoArquivo}]. Arquivo => [{idArquivo}]. CorrelationId => [{request.CorrelationId}].");
                var file = Path.Combine(request.CaminhoArquivo, $"{idArquivo}.pdf");

                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await request.Arquivo.CopyToAsync(fileStream);
                }

                var tamanho = new FileInfo(file).Length;

                _logger.LogInformation($"ArquivoUpload: Salvando arquivo no banco de dados. Arquivo => [{idArquivo}]. CorrelationId => [{request.CorrelationId}].");
                this.Inserir(new TorcamentoCotacaoArquivos()
                {
                    Id = idArquivo,
                    Nome = request.Arquivo.FileName,
                    Pai = !string.IsNullOrEmpty(request.IdPai) ? Guid.Parse(request.IdPai) : (Guid?)null,
                    Descricao = string.Empty,
                    Tamanho = calculaTamanho(tamanho),
                    Tipo = "File"
                });

                _logger.LogInformation($"ArquivoUpload: Upload efetuado com sucesso. CorrelationId => [{request.CorrelationId}].");
                response.CorrelationId = request.CorrelationId;
                response.Sucesso = true;
                response.Mensagem = "Upload efetuado com sucesso.";
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
                        orderby orcamentoCotacaoArquivos.Tipo descending, orcamentoCotacaoArquivos.Nome
                        select orcamentoCotacaoArquivos)
                    .ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private List<Child> ObterEstruturaFilho(List<TorcamentoCotacaoArquivos> estruturas, Guid IdPai)
        {
            return estruturas.Where(x => x.Pai == IdPai)
                            .Select(c => new Child
                            {
                                data = new Data()
                                {
                                    key = c.Id.ToString(),
                                    name = c.Nome,
                                    type = c.Tipo,
                                    size = c.Tamanho,
                                    descricao = c.Descricao
                                },
                                children = ObterEstruturaFilho(estruturas, c.Id)
                            }).ToList();
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