using Microsoft.AspNetCore.Mvc;

namespace OrcamentoCotacaoApi.Controllers
{
    public class RelatoriosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
