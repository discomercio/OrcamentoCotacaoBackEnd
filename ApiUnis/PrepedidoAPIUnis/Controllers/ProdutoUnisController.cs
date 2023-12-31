﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepedidoUnisBusiness.UnisBll.AcessoBll;
using PrepedidoUnisBusiness.UnisBll.ProdutoUnisBll;
using PrepedidoUnisBusiness.UnisDto.ProdutoUnisDto;

namespace PrepedidoAPIUnis.Controllers
{
    [Route("api/prepedidoUnis")]
    [ApiController]
    public class ProdutoUnisController : Controller
    {
        private readonly ProdutoUnisBll produtoUnisBll;
        private readonly IServicoValidarTokenApiUnis servicoValidarTokenApiUnis;

        public ProdutoUnisController(ProdutoUnisBll produtoUnisBll, IServicoValidarTokenApiUnis servicoValidarTokenApiUnis)
        {
            this.produtoUnisBll = produtoUnisBll;
            this.servicoValidarTokenApiUnis = servicoValidarTokenApiUnis;
        }

        /// <summary>
        /// Rotina para buscar listagem de todos os produtos
        /// </summary>
        /// <param name="tokenAcesso"></param>
        /// <param name="loja"></param>
        /// <param name="cnpj_cpf_cliente">CPF ou CNPJ, somente dígitos</param>
        /// <returns></returns>
        /// <response code="204">Loja não existe ou cliente não cadastrado</response>
        [AllowAnonymous]
        [HttpGet("buscarProduto")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<ProdutoComboUnisDto>> BuscarProduto(string tokenAcesso, string loja,
            string cnpj_cpf_cliente)
        {
            if (!servicoValidarTokenApiUnis.ValidarToken(tokenAcesso, out _))
                return Unauthorized();

            ProdutoComboUnisDto retorno = await produtoUnisBll.ListaProdutosCombo(loja, cnpj_cpf_cliente);

            return Ok(retorno);
        }

        [HttpGet("listarProdutos")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<ProdutoComboUnisDto>> BuscarProduto(string tokenAcesso, string loja)
        {
            List<ProdutoUnisDto> retorno = await produtoUnisBll.ListarProdutos(loja);
            return Ok(retorno);
        }
    }
}