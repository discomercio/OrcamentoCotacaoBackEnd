using Dapper;
using Microsoft.Data.SqlClient;

namespace ArClube.Mensageria
{
    public sealed class MensageriaRepositorio : IMensageriaRepositorio
    {
        private readonly string _connectionString;

        public MensageriaRepositorio(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<UnidadeNegocioParametroDto>> ObterUnidadeNeogocioParametrosAsync()
        {
            IEnumerable<UnidadeNegocioParametroDto> unidadeNegocioParametro;

            using (var con = new SqlConnection(this._connectionString))
            {
                var query = @"  
                            SELECT 
                                Id, 
	                            IdCfgUnidadeNegocio, 
	                            IdCfgParametro, 
	                            Valor, 
	                            DataCadastro, 
	                            DataHoraCadastro, 
	                            DataHoraUltAtualizacao 
                            FROM 
	                            t_CFG_UNIDADE_NEGOCIO_PARAMETRO UNP (NOLOCK) 
                            WHERE 
	                            UNP.IdCfgUnidadeNegocio in (SELECT Id FROM t_CFG_UNIDADE_NEGOCIO UN (NOLOCK)) 
	                            AND UNP.IdCfgParametro in (7,8,9,10,11) ";

                unidadeNegocioParametro = await con.QueryAsync<UnidadeNegocioParametroDto>(query);
            }

            return unidadeNegocioParametro.ToList();
        }

        public async Task<List<OrcamentoCotacaoEmailQueueDto>> ObterOrcamentoCotacaoEmailsQueueAsync(int? recordsPerPage)
        {
            if (!recordsPerPage.HasValue)
            {
                recordsPerPage = 10;
            }

            IEnumerable<OrcamentoCotacaoEmailQueueDto> orcamentoCotacaoEmailQueue;

            using (var con = new SqlConnection(this._connectionString))
            {
                var query = @"  
                            SELECT 
	                            [Id], 
	                            [Attachment], 
	                            [AttemptsQty], 
	                            [Bcc], 
	                            [Body], 
	                            [Cc], 
	                            [DateCreated], 
	                            [DateLastAttempt], 
	                            [DateScheduled], 
	                            [DateSent], 
	                            [ErrorMsgLastAttempt], 
	                            [From], 
	                            [FromDisplayName], 
	                            [IdCfgUnidadeNegocio], 
	                            [Sent], 
	                            [Status], 
	                            [Subject], 
	                            [To] 
                            FROM 
                                T_ORCAMENTO_COTACAO_EMAIL_QUEUE (NOLOCK) 
                            WHERE 
	                            [Id] IN 
	                            ( 
		                            SELECT [t].[Id] 
		                            FROM [T_ORCAMENTO_COTACAO_EMAIL_QUEUE] AS [t] (NOLOCK) 
		                            WHERE 
                                        (([t].[Sent] = 0) OR ([t].[Sent] IS NULL)) 
                                        AND (([t].[DateScheduled] < GETDATE()) OR ([t].[DateScheduled] IS NULL)) 
                                        AND (([t].[Status] IN (0, 3)) OR ([t].[Status] IS NULL)) 
		                            ORDER BY (SELECT 1) 
		                            OFFSET 0 ROWS FETCH NEXT @recordsPerPage ROWS ONLY 
	                            ) 
                            ORDER BY [DateScheduled] ";

                orcamentoCotacaoEmailQueue = await con.QueryAsync<OrcamentoCotacaoEmailQueueDto>(query, new
                {
                    recordsPerPage = recordsPerPage.Value
                });
            }

            return orcamentoCotacaoEmailQueue.ToList();
        }

        public async Task<bool> AtualizarOrcamentoCotacaoEmailsQueue(OrcamentoCotacaoEmailQueueDto orcamentoCotacaoEmailQueue)
        {
            var result = 0;

            using (var con = new SqlConnection(this._connectionString))
            {
                var command = @" 
                                UPDATE 
	                                T_ORCAMENTO_COTACAO_EMAIL_QUEUE 
                                SET 
	                                [DateSent]              = @DateSent, 
	                                [Status]                = @Status, 
	                                [Sent]                  = @Sent, 
	                                [AttemptsQty]           = @AttemptsQty, 
	                                [DateLastAttempt]       = @DateLastAttempt, 
	                                [ErrorMsgLastAttempt]   = @ErrorMsgLastAttempt, 
                                    [DateScheduled]         = @DateScheduled 
                                WHERE 
	                                Id = @Id ";


                result = await con.ExecuteAsync(command, new
                {
                    Id = orcamentoCotacaoEmailQueue.Id,
                    DateSent = orcamentoCotacaoEmailQueue.DateSent,
                    Status = orcamentoCotacaoEmailQueue.Status,
                    Sent = orcamentoCotacaoEmailQueue.Sent,
                    AttemptsQty = orcamentoCotacaoEmailQueue.AttemptsQty,
                    DateLastAttempt = orcamentoCotacaoEmailQueue.DateLastAttempt,
                    ErrorMsgLastAttempt = orcamentoCotacaoEmailQueue.ErrorMsgLastAttempt,
                    DateScheduled = orcamentoCotacaoEmailQueue.DateScheduled
                });
            }

            return result > 0 ? true : false;
        }

        public async Task<int> ObterParametroPorId(string id)
        {
            var result = 0;

            var command = @" 
                            SELECT 
                                campo_inteiro 
                            FROM 
                                t_PARAMETRO (NOLOCK) 
                            WHERE id = @id ";

            using (var con = new SqlConnection(this._connectionString))
            {
                result = await con.ExecuteScalarAsync<int>(command, new { id = id });
            }

            return result;
        }
    }
}