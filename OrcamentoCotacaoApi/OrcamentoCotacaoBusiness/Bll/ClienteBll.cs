using Cep;
using Cliente.Dados;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class ClienteBll
    {
        private readonly ILogger<ClienteBll> _logger;
        private readonly Cliente.ClienteBll clienteBll;
        private readonly ContextoBdProvider _contextoBdProvider;
        private readonly CepBll _cepBll;
        private readonly IBancoNFeMunicipio _bancoNFeMunicipio;
        public ClienteBll(ILogger<ClienteBll> logger, Cliente.ClienteBll clienteBll, ContextoBdProvider contextoBdProvider, CepBll cepBll,
            IBancoNFeMunicipio bancoNFeMunicipio)
        {
            _logger = logger;
            this.clienteBll = clienteBll;
            _contextoBdProvider = contextoBdProvider;
            _cepBll = cepBll;
            _bancoNFeMunicipio = bancoNFeMunicipio;
        }

        public async Task<Tcliente> BuscarTcliente(string cpf_cnpj)
        {
            return await clienteBll.BuscarTcliente(cpf_cnpj).FirstOrDefaultAsync();
        }

        public async Task<List<string>> CadastrarClienteOrcamentoCotacao(DadosClienteCadastroDados dadosClienteCadastroDados,
            ContextoBdGravacao dbGravacao, string loja)
        {
            //vamos verificar se existe
            _logger.LogInformation($"Cadastrando cliente");

            //precisa receber esses dados
            string indicador = dadosClienteCadastroDados.Indicador_Orcamentista;
            string usuarioCadastro = dadosClienteCadastroDados.UsuarioCadastro;

            Tcliente clienteCadastrado = new Tcliente();
            var idCliente = await clienteBll.CadastrarDadosClienteDados(dbGravacao, dadosClienteCadastroDados, indicador, clienteCadastrado,
                Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, usuarioCadastro);
            if (string.IsNullOrEmpty(idCliente)) return new List<string>() { "Falha ao gerar o id!" };

            if (idCliente.Length == 12)
            {
                string campos_a_omitir = "dt_cadastro|usuario_cadastro|dt_ult_atualizacao|usuario_ult_atualizacao";
                string log = "";
                log = UtilsGlobais.Util.MontaLog(clienteCadastrado, log, campos_a_omitir);

                bool gravouLog = UtilsGlobais.Util.GravaLog(dbGravacao, usuarioCadastro, loja, "", idCliente,
                        Constantes.OP_LOG_CLIENTE_INCLUSAO, log);
            }

            return null;
        }

        public async Task<List<string>> ValidarClienteOrcamentoCotacao(ClienteCadastroDados clienteCadastroDados)
        {
            List<string> lstErros = new List<string>();

            List<Cliente.Dados.ListaBancoDados> lstBanco = (await clienteBll.ListarBancosCombo()).ToList();

            await Cliente.ValidacoesClienteBll.ValidarDadosCliente(clienteCadastroDados.DadosCliente, false,
               clienteCadastroDados.RefBancaria, clienteCadastroDados.RefComercial, lstErros, _contextoBdProvider,
               _cepBll, _bancoNFeMunicipio, lstBanco, true,
               Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, true);

            if (lstErros.Count() > 0) return lstErros;


            return null;
        }
    }
}
