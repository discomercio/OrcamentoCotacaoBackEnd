﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using PrepedidoUnisBusiness.UnisBll.AcessoBll;
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
        private readonly IServicoValidarTokenApiUnis servicoValidarTokenApiUnis;

        public PrepedidoUnisController(PrePedidoUnisBll prepedidoUnisBll, FormaPagtoUnisBll formaPagtoUnisBll,
            CoeficienteUnisBll coeficienteUnisBll, IServicoValidarTokenApiUnis servicoValidarTokenApiUnis)
        {
            this.prepedidoUnisBll = prepedidoUnisBll;
            this.formaPagtoUnisBll = formaPagtoUnisBll;
            this.coeficienteUnisBll = coeficienteUnisBll;
            this.servicoValidarTokenApiUnis = servicoValidarTokenApiUnis;
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
            if (!servicoValidarTokenApiUnis.ValidarToken(prePedido.TokenAcesso, out _))
                return Unauthorized();

            PrePedidoResultadoUnisDto ret = await prepedidoUnisBll.CadastrarPrepedidoUnis(prePedido);

            return Ok(ret);
        }

        /// <summary>
        /// Rotina para cancelar Pré-Pedido
        /// </summary>
        /// <param name="cancelarPrepedido"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("cancelarPrepedido")]
        public async Task<IActionResult> CancelarPrePedido(CancelarPrepedidoUnisDto cancelarPrepedido)
        {
            if (!servicoValidarTokenApiUnis.ValidarToken(cancelarPrepedido.TokenAcesso, out string usuario))
                return Unauthorized();

            //chamar bll para deletar
            bool ret = await prepedidoUnisBll.CancelarPrePedido(cancelarPrepedido.Indicador_Orcamentista.ToUpper(),
            cancelarPrepedido.NumeroPrepedido);

            if (ret == true)
                return Ok();
            else
                return NotFound();
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
            if (!servicoValidarTokenApiUnis.ValidarToken(tokenAcesso, out _))
                return Unauthorized();

            FormaPagtoUnisDto retorno = await formaPagtoUnisBll.ObterFormaPagto(orcamentista.Trim().ToUpper(), tipo_pessoa.Trim().ToUpper());

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
        public async Task<ActionResult<QtdeParcCartaoVisaResultadoUnisDto>> BuscarQtdeParcCartaoVisa(string tokenAcesso)
        {
            if (!servicoValidarTokenApiUnis.ValidarToken(tokenAcesso, out _))
                return Unauthorized();

            QtdeParcCartaoVisaResultadoUnisDto retorno = await formaPagtoUnisBll.BuscarQtdeParcCartaoVisa();

            return Ok(retorno);
        }

        /// <summary>
        /// Rotina para buscar lista de coeficiente para calcular produtos
        /// </summary>
        /// <param name="tokenAcesso"></param>
        /// <param name="lstFornecedores">Lista de fabricantes. Exemplo: "001", "002".</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("buscarCoeficienteFornecedores")]
        public async Task<ActionResult<List<List<CoeficienteUnisDto>>>> BuscarCoeficienteFornecedores(string tokenAcesso,
            List<string> lstFornecedores)
        {
            if (!servicoValidarTokenApiUnis.ValidarToken(tokenAcesso, out string usuario))
                return Unauthorized();

            IEnumerable<IEnumerable<CoeficienteUnisDto>> ret = await coeficienteUnisBll.BuscarListaCoeficientesFornecedores(lstFornecedores);

            return Ok(ret);
        }

        /// <summary>
        /// Rotina que busca se orçamentista permite RA
        /// </summary>
        /// <param name="tokenAcesso"></param>
        /// <param name="orcamentista"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("obterPermiteRaStatus")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<PermiteRaStatusResultadoUnisDto>> Obter_Permite_RA_Status(string tokenAcesso, string orcamentista)
        {
            if (!servicoValidarTokenApiUnis.ValidarToken(tokenAcesso, out string usuario))
                return Unauthorized();

            PermiteRaStatusResultadoUnisDto ret = await prepedidoUnisBll.Obter_Permite_RA_Status(orcamentista.ToUpper());
            if (ret == null)
                return NotFound("Orcamentista não localizado");

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
        public async Task<ActionResult<PercentualVlPedidoRAResultadoUnisDto>> ObtemPercentualVlPedidoRA(string tokenAcesso)
        {
            if (!servicoValidarTokenApiUnis.ValidarToken(tokenAcesso, out _))
                return Unauthorized();

            PercentualVlPedidoRAResultadoUnisDto ret = await prepedidoUnisBll.ObtemPercentualVlPedidoRA();

            return Ok(ret);
        }
    }
}