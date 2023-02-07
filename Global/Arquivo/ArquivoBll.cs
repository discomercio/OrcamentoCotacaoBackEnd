using Arquivo.Dto;
using Arquivo.Requests;
using Arquivo.Responses;
using Cfg.CfgOperacao;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
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
        private readonly CfgOperacaoBll _cfgOperacaoBll;

        public ArquivoBll(
            ILogger<ArquivoBll> logger,
            InfraBanco.ContextoBdProvider contextoProvider,
            CfgOperacaoBll cfgOperacaoBll)
        {
            _logger = logger;
            _contextoProvider = contextoProvider;
            _cfgOperacaoBll = cfgOperacaoBll;
        }

        public async Task<ArquivoObterEstruturaResponse> ArquivoObterEstrutura(ArquivoObterEstruturaRequest request)
        {
            var response = new ArquivoObterEstruturaResponse();
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();

            try
            {
                _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Obter estrutura da tela de download.");
                var estruturas = this.ObterEstrutura();

                _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Retorno estrutura de tela de download. Retorno => [{estruturas.Count}].");
                if (estruturas.Count <= 0)
                {
                    response.Sucesso = false;
                    response.Mensagem = "Não existem pastas cadastrada. Favor criar nova pasta.";
                    return response;
                }

                var childs = new List<Child>();

                foreach (TorcamentoCotacaoArquivos itemPai in estruturas)
                {
                    if (itemPai.Pai == null)
                    {
                        childs.Add(new Child()
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

                _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Retorno da estrutura com sucesso.");
                response.CorrelationId = request.CorrelationId;
                response.Sucesso = true;
                response.Mensagem = "Retorno da estrutura com sucesso.";
                response.Childs = childs;
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
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Verificando campo preenchido. Id => [{request.Id}].");
            if (string.IsNullOrEmpty(request.Id))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo Id.";
                return response;
            }

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Verificando campo preenchido. CaminhoArquivo => [{request.CaminhoArquivo}].");
            if (string.IsNullOrEmpty(request.CaminhoArquivo))
            {
                response.Sucesso = false;
                response.Mensagem = "Erro inesperado! Favor entrar em contato com o suporte técnico.";
                return response;
            }

            try
            {
                _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Salvando arquivo no caminho. Arquivo => [{request.Id}].");
                var caminho = Path.Combine(request.CaminhoArquivo, $"{request.Id}.pdf");
                var fileinfo = new FileInfo(caminho);
                byte[] byteArray = File.ReadAllBytes(caminho);
                var arquivo = this.ObterArquivoPorID(Guid.Parse(request.Id));

                _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Download efetuado com sucesso.");
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

        public async Task<ArquivoExcluirResponse> ArquivoExcluir(ArquivoExcluirRequest request, UsuarioLogin usuarioLogin)
        {
            var response = new ArquivoExcluirResponse();
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Verificando campo preenchido. Id => [{request.Id}].");
            if (string.IsNullOrEmpty(request.Id))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo Id.";
                return response;
            }

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Verificando campo preenchido. CaminhoArquivo => [{request.CaminhoArquivo}].");
            if (string.IsNullOrEmpty(request.CaminhoArquivo))
            {
                response.Sucesso = false;
                response.Mensagem = "Erro inesperado! Favor entrar em contato com o suporte técnico.";
                return response;
            }

            try
            {
                using (var dbGravacao = _contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Excluindo arquivo do banco de dados. Arquivo => [{request.Id}].");
                    var tOrcamentoArquivo = this.ExcluirComTransacao(new TorcamentoCotacaoArquivos() { Id = Guid.Parse(request.Id) }, dbGravacao);

                    var file = Path.Combine(request.CaminhoArquivo, $"{request.Id}.pdf");

                    _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Excluindo arquivo da pasta na reder banco de dados. CaminhoArquivo => [{request.CaminhoArquivo}]. Arquivo => [{request.Id}].");
                    if (tOrcamentoArquivo != null)
                    {
                        if (File.Exists(file))
                        {
                            File.Delete(file);
                        }
                    }

                    string log = "";
                    _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Montando log de operação.");
                    log = UtilsGlobais.Util.MontaLog(tOrcamentoArquivo, log, "");

                    _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Buscando operação de criação de pasta.");
                    var cfgOperacao = _cfgOperacaoBll.PorFiltroComTransacao(new TcfgOperacaoFiltro() { Id = tOrcamentoArquivo.Tipo == "File" ? 17 : 16 }, dbGravacao).FirstOrDefault();
                    if (cfgOperacao == null)
                    {
                        response.Sucesso = false;
                        response.Mensagem = "Ops! Falha ao excluir pasta.";
                        return response;
                    }
                    var tLogV2 = UtilsGlobais.Util.GravaLogV2ComTransacao(dbGravacao, log, (short)usuarioLogin.TipoUsuario, usuarioLogin.Id, request.Loja, null, null, null,
                        InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, cfgOperacao.Id, request.IP);

                    dbGravacao.SaveChanges();
                    dbGravacao.transacao.Commit();

                    _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Exclusão concluida com sucesso.");
                    response.CorrelationId = request.CorrelationId;
                    response.Sucesso = true;
                    response.Mensagem = "Exclusão concluida com sucesso.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ArquivoEditarResponse> ArquivoEditar(ArquivoEditarRequest request, UsuarioLogin usuarioLogin)
        {
            var response = new ArquivoEditarResponse();
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Verificando campo preenchido. Id => [{request.Id}].");
            if (string.IsNullOrEmpty(request.Id))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo Id.";
                return response;
            }

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Verificando campo preenchido. Nome => [{request.Nome}].");
            if (string.IsNullOrEmpty(request.Nome))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo Nome.";
                return response;
            }

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Verificando campo Nome excedeu máximo de 255 caracteres.");
            if (request.Nome.Length > 255)
            {
                response.Sucesso = false;
                response.Mensagem = "Nome excedeu máximo de 255 caracteres.";
                return response;
            }

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Verificando campo Descricao excedeu máximo de 500 caracteres.");
            if (!string.IsNullOrEmpty(request.Descricao)
                && request.Descricao.Length > 500)
            {
                response.Sucesso = false;
                response.Mensagem = "Descrição excedeu máximo de 500 caracteres.";
                return response;
            }

            try
            {
                using (var dbGravacao = _contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Editando Pasta/Arquivo do banco de dados. Arquivo => [{request.Id}]. Nome => [{request.Nome}]. Descrição => [{request.Descricao}].");
                   
                    var tOrcamentoCotacaoArquivosAntigo = this.ObterArquivoPorID(Guid.Parse(request.Id));

                    var tOrcamentoCotacaoArquivosNovo = new TorcamentoCotacaoArquivos
                    {
                        Id = Guid.Parse(request.Id),
                        Nome = request.Nome.Trim(),
                        Descricao = !string.IsNullOrEmpty(request.Descricao) ? request.Descricao.Trim() : string.Empty,
                    };

                    var retorno = EditarComTransacao(tOrcamentoCotacaoArquivosNovo, dbGravacao);

                    
                    string log = "";
                   
                    _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Montando log de operação.");
                    log = UtilsGlobais.Util.MontalogComparacao(retorno, tOrcamentoCotacaoArquivosAntigo, log, "");

                    _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Buscando operação de criação de pasta.");

                    var cfgOperacao = _cfgOperacaoBll.PorFiltroComTransacao(new TcfgOperacaoFiltro() { Id = retorno.Tipo == "Folder" ? 14 : 15 }, dbGravacao).FirstOrDefault();
                    if (cfgOperacao == null)
                    {
                        response.Sucesso = false;
                        response.Mensagem = "Ops! Falha ao criar pasta.";
                        return response;
                    }

                    if (!string.IsNullOrEmpty(log))
                    {
                        log = $"id={retorno.Id}; {log}";
                        var tLogV2 = UtilsGlobais.Util.GravaLogV2ComTransacao(dbGravacao, log, (short)usuarioLogin.TipoUsuario, usuarioLogin.Id, null, null, null, null,
                        InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, cfgOperacao.Id, request.IP);
                    }

                    dbGravacao.SaveChanges();
                    dbGravacao.transacao.Commit();

                    _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Salvo com sucesso.");
                    response.CorrelationId = request.CorrelationId;
                    response.Sucesso = true;
                    response.Mensagem = "Salvo com sucesso.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ArquivoCriarPastaResponse> ArquivoCriarPasta(ArquivoCriarPastaRequest request, UsuarioLogin usuarioLogin)
        {
            var response = new ArquivoCriarPastaResponse();
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Verificando campo preenchido. Nome => [{request.Nome}].");
            if (string.IsNullOrEmpty(request.Nome))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo Nome.";
                return response;
            }

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Verificando campo Nome excedeu máximo de 255 caracteres.");
            if (request.Nome.Length > 255)
            {
                response.Sucesso = false;
                response.Mensagem = "Nome excedeu máximo de 255 caracteres.";
                return response;
            }

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Verificando campo Descrição excedeu máximo de 500 caracteres.");
            if (!string.IsNullOrEmpty(request.Descricao)
                && request.Descricao.Length > 500)
            {
                response.Sucesso = false;
                response.Mensagem = "Descrição excedeu máximo de 500 caracteres.";
                return response;
            }

            try
            {
                _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Criando pasta no banco de dados. Nome => [{request.Nome}].");

                using (var dbGravacao = _contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var tOrcamentoCotacaoArquivos = new TorcamentoCotacaoArquivos()
                    {
                        Id = Guid.NewGuid(),
                        Nome = request.Nome.Trim(),
                        Pai = !string.IsNullOrEmpty(request.IdPai) ? Guid.Parse(request.IdPai) : (Guid?)null,
                        Descricao = !string.IsNullOrEmpty(request.Descricao) ? request.Descricao.Trim() : string.Empty,
                        Tamanho = string.Empty,
                        Tipo = "Folder"
                    };
                    tOrcamentoCotacaoArquivos = this.InserirComTransacao(tOrcamentoCotacaoArquivos, dbGravacao);

                    string log = "";

                    _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Montando log de operação.");
                    log = UtilsGlobais.Util.MontaLog(tOrcamentoCotacaoArquivos, log, "");

                    _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Buscando operação de criação de pasta.");
                    var cfgOperacao = _cfgOperacaoBll.PorFiltroComTransacao(new TcfgOperacaoFiltro() { Id = 12 }, dbGravacao).FirstOrDefault();
                    if (cfgOperacao == null)
                    {
                        response.Sucesso = false;
                        response.Mensagem = "Ops! Falha ao criar pasta.";
                        return response;
                    }
                    var tLogV2 = UtilsGlobais.Util.GravaLogV2ComTransacao(dbGravacao, log, (short)usuarioLogin.TipoUsuario, usuarioLogin.Id, request.Loja, null, null, null,
                        InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, cfgOperacao.Id, request.IP);

                    dbGravacao.SaveChanges();
                    dbGravacao.transacao.Commit();

                    _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Pasta [{request.Nome}] criada com sucesso.");
                    response.CorrelationId = request.CorrelationId;
                    response.Id = tOrcamentoCotacaoArquivos.Id;
                    response.Sucesso = true;
                    response.Mensagem = $"Pasta '{request.Nome}' criada com sucesso.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ArquivoUploadResponse> ArquivoUpload(ArquivoUploadRequest request, UsuarioLogin usuarioLogin)
        {
            var response = new ArquivoUploadResponse();
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();

            #region validações
            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Verificando campo preenchido. CaminhoArquivo => [{request.CaminhoArquivo}].");
            if (string.IsNullOrEmpty(request.CaminhoArquivo))
            {
                response.Sucesso = false;
                response.Mensagem = "Erro inesperado! Favor entrar em contato com o suporte técnico.";
                return response;
            }

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Verificando se o upload esta foi enviado.");
            if (request.Arquivo == null || string.IsNullOrEmpty(request.Arquivo.FileName))
            {
                response.Sucesso = false;
                response.Mensagem = "Favor preencher campo nome arquivo.";
                return response;
            }

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Verificando campo Nome do aquivo excedeu máximo de 255 caracteres.");
            if (request.Arquivo.FileName.Length > 255)
            {
                response.Sucesso = false;
                response.Mensagem = "Nome excedeu máximo de 255 caracteres.";
                return response;
            }

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Verificando se o upload é um PDF.");
            if (!request.Arquivo.ContentType.Equals("application/pdf") && !request.Arquivo.ContentType.Equals("pdf"))
            {
                response.Sucesso = false;
                response.Mensagem = "Formato inválido. O arquivo deve ser no formato PDF.";
                return response;
            }
            #endregion
            try
            {
                using (var dbGravacao = _contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var idArquivo = Guid.NewGuid();

                    _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Fazendo o upload do PDF. CaminhoArquivo => [{request.CaminhoArquivo}]. Arquivo => [{idArquivo}].");
                    var file = Path.Combine(request.CaminhoArquivo, $"{idArquivo}.pdf");

                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await request.Arquivo.CopyToAsync(fileStream);
                    }

                    var tamanho = new FileInfo(file).Length;

                    _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Salvando arquivo no banco de dados. Arquivo => [{idArquivo}].");
                    var tOrcamentoCotacaoArquivos = this.Inserir(new TorcamentoCotacaoArquivos()
                    {
                        Id = idArquivo,
                        Nome = request.Arquivo.FileName,
                        Pai = !string.IsNullOrEmpty(request.IdPai) ? Guid.Parse(request.IdPai) : (Guid?)null,
                        Descricao = string.Empty,
                        Tamanho = calculaTamanho(tamanho),
                        Tipo = "File"
                    });

                    string log = "";

                    _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Montando log de operação.");
                    log = UtilsGlobais.Util.MontaLog(tOrcamentoCotacaoArquivos, log, "");

                    _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Buscando operação de criação de pasta.");
                    var cfgOperacao = _cfgOperacaoBll.PorFiltroComTransacao(new TcfgOperacaoFiltro() { Id = 13 }, dbGravacao).FirstOrDefault();
                    if (cfgOperacao == null)
                    {
                        response.Sucesso = false;
                        response.Mensagem = "Ops! Falha ao criar pasta.";
                        return response;
                    }
                    var tLogV2 = UtilsGlobais.Util.GravaLogV2ComTransacao(dbGravacao, log, (short)usuarioLogin.TipoUsuario, usuarioLogin.Id, request.Loja, null, null, null,
                        InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, cfgOperacao.Id, request.IP);

                    dbGravacao.SaveChanges();
                    dbGravacao.transacao.Commit();

                    _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. {nomeMetodo}. Upload efetuado com sucesso.");
                    response.CorrelationId = request.CorrelationId;
                    response.Sucesso = true;
                    response.Mensagem = "Upload efetuado com sucesso.";
                    return response;
                }

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

        public TorcamentoCotacaoArquivos EditarComTransacao(TorcamentoCotacaoArquivos obj, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            var retorno = contextoBdGravacao.TorcamentoCotacaoArquivos.FirstOrDefault(x => x.Id == obj.Id);
            if (retorno == null) return null;

            retorno.Nome = obj.Nome;
            retorno.Descricao = obj.Descricao;

            contextoBdGravacao.SaveChanges();

            return retorno;
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
            catch (Exception ex)
            {
                return false;
            }
        }

        public TorcamentoCotacaoArquivos InserirComTransacao(TorcamentoCotacaoArquivos model, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.TorcamentoCotacaoArquivos.Add(model);
            contextoBdGravacao.SaveChanges();
            return model;
        }

        public List<TorcamentoCotacaoArquivos> PorFilroComTransacao(TorcamentoCotacaoArquivosFiltro obj, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoArquivos AtualizarComTransacao(TorcamentoCotacaoArquivos model, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoArquivos ExcluirComTransacao(TorcamentoCotacaoArquivos obj, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            var saida = contextoBdGravacao.TorcamentoCotacaoArquivos.FirstOrDefault(x => x.Id == obj.Id);

            if (saida == null) return null;

            contextoBdGravacao.TorcamentoCotacaoArquivos.Remove(saida);
            contextoBdGravacao.SaveChanges();
            return saida;
        }
    }
}