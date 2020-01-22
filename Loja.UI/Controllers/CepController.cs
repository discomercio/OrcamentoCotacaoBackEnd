using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Loja.Bll.Dto.CepDto;
using Loja.UI.Models.Cep;
using Microsoft.AspNetCore.Mvc;

namespace Loja.UI.Controllers
{
    public class CepController : Controller
    {
        public readonly Bll.CepBll.CepBll cepBll;

        public CepController(Bll.CepBll.CepBll cepBll)
        {
            this.cepBll = cepBll;
        }

        public IActionResult Index()
        {
            CepViewModel cep = new CepViewModel();
            cep.ListaCep = new List<CepDto>();
            return View(cep);
        }

        [HttpGet]
        public async Task<IActionResult> BuscarCep(string cep, string endereco, string uf, string cidade)
        {
            List<CepDto> lstCepDto = (await cepBll.BuscarCep(cep, endereco, uf, cidade)).ToList();

            CepViewModel modelCep = new CepViewModel();
            modelCep.ListaCep = lstCepDto;

            return Json(modelCep.ListaCep);
        }

        [HttpGet]
        public async Task<IEnumerable<string>> BuscarUfs()
        {
            List<string> lstUf = new List<string>();
            lstUf = (await cepBll.BuscarUfs()).ToList();
            
            return lstUf;
        }

        [HttpGet]
        public async Task<ActionResult> BuscarLocalidades(string uf)
        {
            var ret = await cepBll.BuscarLocalidades(uf);

            return Ok(ret);
        }

        [HttpGet]
        public async Task<IActionResult> BuscarCepPorEndereco(string nendereco, string localidade, string lstufs)
        {
            //esse metodo esta buscando apenas 300 itens
            CepViewModel cep = new CepViewModel();
            cep.ListaCep = (await cepBll.BuscarCepPorEndereco(nendereco, localidade, lstufs)).ToList();


            return Ok(cep);
        }
    }
}