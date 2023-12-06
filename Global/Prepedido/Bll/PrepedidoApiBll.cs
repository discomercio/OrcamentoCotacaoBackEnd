using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Prepedido.Dados;
using Prepedido.Dados.DetalhesPrepedido;
using Prepedido.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsGlobais;

namespace Prepedido.Bll
{
    public class PrepedidoApiBll
    {
        private readonly Prepedido.Bll.PrepedidoBll prepedidoBll;
        private readonly InfraBanco.ContextoBdProvider _contextoBdProvider;


        public PrepedidoApiBll(Prepedido.Bll.PrepedidoBll prepedidoBll, InfraBanco.ContextoBdProvider _contextoBdProvider)
        {
            this.prepedidoBll = prepedidoBll;
            this._contextoBdProvider = _contextoBdProvider;
        }

        public async Task DeletarOrcamentoExisteComTransacao(PrePedidoDto prePedido, string apelido)
        {
            PrePedidoDados prePedidoDados = PrePedidoDto.PrePedidoDados_De_PrePedidoDto(prePedido);
            await prepedidoBll.DeletarOrcamentoExisteComTransacao(prePedidoDados, apelido.Trim());
        }

        private bool VerificarCadastroPrepedidoPagto(PrePedidoDto prePedido, string paramIdPagtoMonitorado)
        {
            if(prePedido.FormaPagtoCriacao.Tipo_parcelamento.ToString() == Constantes.COD_FORMA_PAGTO_A_VISTA)
            {
                if(paramIdPagtoMonitorado.IndexOf($"|{prePedido.FormaPagtoCriacao.Op_av_forma_pagto}|") > -1)
                {
                    return true;
                }
            }

            if (prePedido.FormaPagtoCriacao.Tipo_parcelamento.ToString() == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
            {
                if (paramIdPagtoMonitorado.IndexOf($"|{prePedido.FormaPagtoCriacao.Op_pce_entrada_forma_pagto}|") > -1 ||
                    paramIdPagtoMonitorado.IndexOf($"|{prePedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto}|") > -1)
                {
                    return true;
                }
            }

            if (prePedido.FormaPagtoCriacao.Tipo_parcelamento.ToString() == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
            {
                if (paramIdPagtoMonitorado.IndexOf($"|{prePedido.FormaPagtoCriacao.Op_pu_forma_pagto}|") > -1)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<IEnumerable<string>> CadastrarPrepedido(
            PrePedidoDto prePedido, 
            string apelido,
            decimal limiteArredondamento, 
            bool verificarPrepedidoRepetido,
            Constantes.CodSistemaResponsavel sistemaResponsavelCadastro,
            int limite_de_itens, 
            Constantes.TipoUsuarioContexto tipoUsuarioContexto,
            int usuarioId, 
            string ip)
        {

            var parametro = await BuscarRegistroParametro(Constantes.CLIENTE_EMAILBOLETO_REQUIREDFIELD_FLAGHABILITACAO);
            if(parametro == null)
            {
                return new List<string>() { "Falha ao buscar parâmetro para validação de e-mail boleto" };
            }

            var paramSplit = parametro.Campo_texto.Split('=');
            var paramIdPagtoMonitorado = paramSplit[1];

            if(VerificarCadastroPrepedidoPagto(prePedido, paramIdPagtoMonitorado))
            {
                if(string.IsNullOrEmpty(prePedido.EnderecoCadastroClientePrepedido.Endereco_Email_Boleto))
                {
                    return new List<string>() { "O campo 'E-mail boleto' é obrigatório!" };
                }
            }

            if (!string.IsNullOrEmpty(prePedido.DetalhesPrepedido.EntregaImediata) && 
                short.Parse(prePedido.DetalhesPrepedido.EntregaImediata) == (short)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO)
            {
                prePedido.DetalhesPrepedido.PrevisaoEntregaData = prePedido.DetalhesPrepedido.EntregaImediataData;

                if (prePedido.DetalhesPrepedido.EntregaImediataData.HasValue)
                {
                    prePedido.DetalhesPrepedido.PrevisaoEntregaData = prePedido.DetalhesPrepedido.EntregaImediataData.Value.Date;
                }

                prePedido.DetalhesPrepedido.PrevisaoEntregaDtHrUltAtualiz = DateTime.Now;
            }

            prePedido.DetalhesPrepedido.EntregaImediataData = DateTime.Now;

            var prePedidoDados = PrePedidoDto.PrePedidoDados_De_PrePedidoDto(prePedido);

            //Prepearar campos de auditoria
            var orcamentista = await prepedidoBll.BuscarTorcamentista(apelido);

            if(orcamentista == null)
            {
                return new List<string>() { "Ops! Usuário não encontrado." };
            }

            //pegar o contexto do usuário que esta cadastrando
            prePedidoDados.UsuarioCadastroId = usuarioId;
            prePedidoDados.UsuarioCadastroIdTipoUsuarioContexto = (short)tipoUsuarioContexto;
            prePedidoDados.Usuario_cadastro = $"[{prePedidoDados.UsuarioCadastroIdTipoUsuarioContexto}] {usuarioId}";

            prePedidoDados.DetalhesPrepedido.InstaladorInstalaIdTipoUsuarioContexto = (short)tipoUsuarioContexto;
            prePedidoDados.DetalhesPrepedido.InstaladorInstalaIdUsuarioUltAtualiz = usuarioId;
            prePedidoDados.DetalhesPrepedido.InstaladorInstalaUsuarioUltAtualiz = prePedidoDados.Usuario_cadastro;
            prePedidoDados.DetalhesPrepedido.InstaladorInstalaDtHrUltAtualiz = DateTime.Now;
            prePedidoDados.DetalhesPrepedido.GarantiaIndicadorIdTipoUsuarioContexto = (short)tipoUsuarioContexto;
            prePedidoDados.DetalhesPrepedido.GarantiaIndicadorIdUsuarioUltAtualiz = usuarioId;
            prePedidoDados.DetalhesPrepedido.GarantiaIndicadorUsuarioUltAtualiz = prePedidoDados.Usuario_cadastro;
            prePedidoDados.DetalhesPrepedido.GarantiaIndicadorDtHrUltAtualiz = DateTime.Now;
            prePedidoDados.DetalhesPrepedido.PrevisaoEntregaIdTipoUsuarioContexto = (short)tipoUsuarioContexto;
            prePedidoDados.DetalhesPrepedido.PrevisaoEntregaIdUsuarioUltAtualiz = usuarioId;
            prePedidoDados.DetalhesPrepedido.PrevisaoEntregaDtHrUltAtualiz = DateTime.Now;
            prePedidoDados.DetalhesPrepedido.PrevisaoEntregaUsuarioUltAtualiz = prePedidoDados.Usuario_cadastro;

            prePedidoDados.EnderecoEntrega.EtgImediataIdTipoUsuarioContexto = (short)tipoUsuarioContexto;
            prePedidoDados.EnderecoEntrega.EtgImediataIdUsuarioUltAtualiz = usuarioId;
            prePedidoDados.EnderecoEntrega.Etg_Imediata_Usuario = prePedidoDados.Usuario_cadastro;
            prePedidoDados.EnderecoEntrega.Etg_imediata_data = DateTime.Now;

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.XLOCK_SYNC_ORCAMENTO))
            {
                var ret = (await prepedidoBll
                    .CadastrarPrepedido(
                    prePedidoDados, 
                    apelido,
                    limiteArredondamento, 
                    verificarPrepedidoRepetido, 
                    sistemaResponsavelCadastro, 
                    limite_de_itens, 
                    dbGravacao, 
                    ip, 
                    false)).ToList();

                if (ret.Count == 1)
                {
                    if (ret[0].Contains(Constantes.SUFIXO_ID_ORCAMENTO))
                    {
                        dbGravacao.transacao.Commit();
                    }
                }
                return ret;
            }
        }

        public async Task<PrePedidoDto> BuscarPrePedido(string apelido, string numPrePedido)
        {
            var ret = await prepedidoBll.BuscarPrePedido(apelido, numPrePedido);
            return PrePedidoDto.PrePedidoDto_De_PrePedidoDados(ret);
        }

        public async Task<IEnumerable<PrepedidosCadastradosDtoPrepedido>> ListarPrePedidos(string apelido,
        Prepedido.Bll.PrepedidoBll.TipoBuscaPrepedido tipoBusca,
        string clienteBusca, string numeroPrePedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            IEnumerable<PrepedidosCadastradosPrepedidoDados> lista = await prepedidoBll.ListarPrePedidos(apelido,
                tipoBusca, clienteBusca,
                numeroPrePedido, dataInicial, dataFinal);

            return PrepedidosCadastradosDtoPrepedido.ListaPrepedidosCadastradosDtoPrepedido_De_PrepedidosCadastradosPrepedidoDados(lista);
        }

        public async Task<Tparametro> BuscarRegistroParametro(string id)
        {
            Tparametro parametroRegra = await Util.BuscarRegistroParametro(id, _contextoBdProvider);

            return parametroRegra;
        }
    }
}
