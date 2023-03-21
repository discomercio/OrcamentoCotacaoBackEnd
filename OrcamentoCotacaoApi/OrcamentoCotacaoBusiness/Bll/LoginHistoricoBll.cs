using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Models.Request.LoginHistorico;
using OrcamentoCotacaoBusiness.Models.Response.LoginHistorico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class LoginHistoricoBll
    {
        private readonly LoginHistorico.LoginHistoricoBll loginHistoricoBll;
        private readonly ILogger<LoginHistoricoBll> _logger;

        public LoginHistoricoBll(LoginHistorico.LoginHistoricoBll loginHistoricoBll, ILogger<LoginHistoricoBll> _logger)
        {
            this.loginHistoricoBll = loginHistoricoBll;
            this._logger = _logger;
        }

        public ListaLoginHistoricoResponse PorFiltro(LoginHistoricoRequest loginHistoricoRequest)
        {
            var response = new ListaLoginHistoricoResponse();

            response.Sucesso = false;

            var saida = loginHistoricoBll.PorFiltro(new InfraBanco.Modelos.Filtros.TloginHistoricoFiltro()
            {
                IdUsuario = loginHistoricoRequest.IdUsuario,
                SistemaResponsavel = loginHistoricoRequest.SistemaResponsavel
            });

            if (saida == null)
            {
                response.Mensagem = "Nenhum histórico encontrado!";
                return response;
            }

            response.LstLoginHistoricoResponse = new List<LoginHistoricoResponse>();

            foreach (var item in saida)
            {
                var retorno = new LoginHistoricoResponse();
                retorno.Id = item.Id;
                retorno.DataHora = item.DataHora;
                retorno.IdTipoUsuarioContexto = item.IdTipoUsuarioContexto;
                retorno.IdUsuario = item.IdUsuario;
                retorno.StSucesso = item.StSucesso;
                retorno.Ip = item.Ip;
                retorno.sistema_responsavel = item.sistema_responsavel;
                retorno.Login = item.Login;
                retorno.Motivo = item.Motivo;
                retorno.IdCfgModulo = item.IdCfgModulo;
                retorno.Loja = item.Loja;

                response.LstLoginHistoricoResponse.Add(retorno);
            }

            response.Sucesso = true;
            return response;
        }

    }
}
