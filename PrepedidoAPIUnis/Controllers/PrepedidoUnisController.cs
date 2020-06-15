﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using PrepedidoUnisBusiness.UnisBll.CoeficienteUnisBll;
using PrepedidoUnisBusiness.UnisBll.FormaPagtoUnisBll;
using PrepedidoUnisBusiness.UnisDto.CoeficienteUnisDto;
using PrepedidoUnisBusiness.UnisDto.FormaPagtoUnisDto;
using PrepedidoUnisBusiness.UnisDto.PrepedidoUnisDto;

namespace PrepedidoAPIUnis.Controllers
{
    [Route("api/prepedidoUnis")]
    [ApiController]
    public class PrepedidoUnisController : Controller
    {
        private readonly PrePedidoUnisBll prepedidoUnisBll;
        private readonly FormaPagtoUnisBll formaPagtoUnisBll;
        private readonly CoeficienteUnisBll coeficienteUnisBll;
        public PrepedidoUnisController(PrePedidoUnisBll prepedidoUnisBll, FormaPagtoUnisBll formaPagtoUnisBll,
            CoeficienteUnisBll coeficienteUnisBll)
        {
            this.prepedidoUnisBll = prepedidoUnisBll;
            this.formaPagtoUnisBll = formaPagtoUnisBll;
            this.coeficienteUnisBll = coeficienteUnisBll;
        }


        /// <summary>
        /// Rotina para cadastrar Pré-Pedido
        /// </summary>
        /// <param name="prePedido">PrePedidoUnisDto</param>
        /// <returns>Retona classe PrePedidoResultadoUnisDto</returns>
        [AllowAnonymous]
        [HttpPost("cadastrarPrepedido")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<PrePedidoResultadoUnisDto>> CadastrarPrepedido(PrePedidoUnisDto prePedido)
        {
            //todo: validar o token
            PrePedidoResultadoUnisDto retorno;
            //todo: retornar a estrutura certa
            var ret = await prepedidoUnisBll.CadastrarPrepedidoUnis(prePedido);

            return Ok(ret);
        }

        /// <summary>
        /// Rotina para deletar Pré-Pedido
        /// </summary>
        /// <param name="deletarPrepedido"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("deletarPrepedido")]
        public async Task<IActionResult> DeletarPrePedido(DeletarPrepedidoUnisDto deletarPrepedido)
        {
            //todo: validar o token
            //deletarPrepedido.TokenAcesso

            //validar Orçamentista
            //validar numero de prepedido

            //chamar bll para deletar
            await prepedidoUnisBll.DeletarOrcamentoExisteComTransacao(deletarPrepedido.Indicador_Orcamentista,
            deletarPrepedido.NumeroPrepedido);

            return Ok();
        }

        /// <summary>
        /// Rotina para buscar listas para forma de pagto ex: Dinheiro, Boleto etc...
        /// </summary>
        /// <param name="tokenAcesso"></param>
        /// <param name="tipo_pessoa"></param>
        /// <param name="orcamentista"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("buscarFormasPagto")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<FormaPagtoUnisDto>> BuscarFormasPagto(string tokenAcesso,
            string tipo_pessoa, string orcamentista)
        {
            //todo: validar token

            FormaPagtoUnisDto retorno = await formaPagtoUnisBll.ObterFormaPagto(orcamentista.Trim(), tipo_pessoa);

            return Ok(retorno);
        }


        /// <summary>
        /// Rotina para buscar a quantidade máxima para parcelamento
        /// </summary>
        /// <param name="tokenAcesso"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("buscarQtdeParcCartaoVisa")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<int>> BuscarQtdeParcCartaoVisa(string tokenAcesso)
        {
            //todo: validar token

            int retorno = await formaPagtoUnisBll.BuscarQtdeParcCartaoVisa();

            return Ok(retorno);
        }

        /// <summary>
        /// Rotina para buscar lista de coeficiente para calcular produtos
        /// </summary>
        /// <param name="tokenAcesso"></param>
        /// <param name="lstFornecedores"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("buscarCoeficienteFornecedores")]
        public async Task<ActionResult<List<List<CoeficienteUnisDto>>>> BuscarCoeficienteFornecedores(string tokenAcesso,
            List<string> lstFornecedores)
        {
            //todo: validar token

            var ret = await coeficienteUnisBll.BuscarListaCoeficientesFornecedores(lstFornecedores);

            return Ok(ret);
        }

        /// <summary>
        /// Rotina que busca se orçamentista permite RA
        /// </summary>
        /// <param name="tokenAcesso"></param>
        /// <param name="orcamentista"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("obter_permite_ra_status")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<short>> Obter_Permite_RA_Status(string tokenAcesso, string orcamentista)
        {
            //todo: validar token

            var ret = await prepedidoUnisBll.Obter_Permite_RA_Status(orcamentista);

            return Ok(ret);
        }

        /// <summary>
        /// Rotina para buscar o percentual máximo para Pré-Pedido com RA
        /// </summary>
        /// <param name="tokenAcesso"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("obtemPercentualVlPedidoRA")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<decimal>> ObtemPercentualVlPedidoRA(string tokenAcesso)
        {
            //todo: validar token

            var ret = await prepedidoUnisBll.ObtemPercentualVlPedidoRA();

            return Ok(ret);
        }
    }
}