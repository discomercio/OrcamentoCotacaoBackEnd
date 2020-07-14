using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Loja.Bll;
using Loja.Bll.Dto.ClienteDto;
using Loja.UI.Models.Cliente;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Loja.UI.Controllers
{
    public class ClienteController : Controller
    {
        private readonly Bll.ClienteBll.ClienteBll clienteBll;

        public ClienteController(Bll.ClienteBll.ClienteBll clienteBll)
        {
            this.clienteBll = clienteBll;
        }
        public IActionResult Index()
        {

            //passar o DadosCienteDto para essa view
            //verificar se o modelo/cpf esta vazio
            //se o modelo/cpf estiver vazio, mostrar a modal com a opção de cadastrar um novo cliente
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ValidarCliente(string cpf_cnpj)
        {
            //só um teste
            HttpContext.Session.SetString("usuario_atual", "PRAGMATICA");
            HttpContext.Session.SetString("loja_atual", "202");

            //afazer:remover pontos do cpf
            cpf_cnpj = Loja.Bll.Util.Util.SoDigitosCpf_Cnpj(cpf_cnpj);

            //afazer:validar usuario antes
            if (!await clienteBll.ValidarCliente(cpf_cnpj))
                return RedirectToAction("Index");
            else
                return RedirectToAction("BuscarCliente", new { cpf_cnpj = cpf_cnpj }); //editar cliente
        }

        public async Task<IActionResult> BuscarCliente(string cpf_cnpj)
        {
            /*Passo a passo da entrada 
              pegar a session do usuario
              pegar a session da loja
              criar session de operações permitidas
              verificar a operação permitida
              buscar os dados do cliente
              buscar indicadores 
            */

            string usuario = HttpContext.Session.GetString("usuario_atual");

            ClienteCadastroViewModel cliente = new ClienteCadastroViewModel();

            string lstOperacoesPermitidas = await clienteBll.BuscaListaOperacoesPermitidas(usuario);
            HttpContext.Session.SetString("lista_operacoes_permitidas", lstOperacoesPermitidas);

            cliente.PermiteEdicao = Loja.Bll.Util.Util.OpercaoPermitida(Loja.Bll.Constantes.Constantes.OP_LJA_EDITA_CLIENTE_DADOS_CADASTRAIS
                , lstOperacoesPermitidas) ? true : false;

            //somente para teste remover após concluir
            //cliente.PermiteEdicao = false;
            HttpContext.Session.SetString("cpf_cnpj", cpf_cnpj);
            ClienteCadastroDto clienteCadastroDto = await clienteBll.BuscarCliente(cpf_cnpj, usuario);

            HttpContext.Session.SetString("cliente_selecionado", clienteCadastroDto.DadosCliente.Id);

            cliente.DadosCliente = clienteCadastroDto.DadosCliente;
            cliente.RefBancaria = clienteCadastroDto.RefBancaria;
            cliente.RefComercial = clienteCadastroDto.RefComercial;

            //Lista para carregar no select de Indicadores
            var lstInd = (await clienteBll.BuscarListaIndicadores(cliente.DadosCliente.Indicador_Orcamentista, usuario)).ToList();
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem { Value = "0", Text = "Selecione" });
            for (int i = 0; i < lstInd.Count; i++)
            {
                lst.Add(new SelectListItem { Value = lstInd[i], Text = lstInd[i] });
            }
            cliente.LstIndicadores = new SelectList(lst, "Value", "Text");

            //lista para carregar no select de Produtor Rural            
            var lstProdR = new[]
            {
                new SelectListItem{Value = "", Text = "Selecione"},
                new SelectListItem{Value = Bll.Constantes.Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO, Text = "Não"},
                new SelectListItem{Value = Bll.Constantes.Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM, Text = "Sim"}
            };
            cliente.LstProdutoRural = new SelectList(lstProdR, "Value", "Text");

            //lista para carregar o Contribuinte ICMS
            var lstContrICMS = new[]
            {
                new SelectListItem{Value = Bll.Constantes.Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL, Text="Selecione"},
                new SelectListItem{Value = Bll.Constantes.Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO, Text = "Isento"},
                new SelectListItem{Value = Bll.Constantes.Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO, Text = "Não"},
                new SelectListItem{Value = Bll.Constantes.Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM, Text = "Sim"}
            };
            cliente.LstContribuinte = new SelectList(lstContrICMS, "Value", "Text");

            cliente.EndJustificativa = (await clienteBll.ListarComboJustificaEndereco(usuario)).ToList();

            var lstBancos = (await clienteBll.ListarBancosCombo()).ToList();
            List<SelectListItem> lstbancos = new List<SelectListItem>();
            lstbancos.Add(new SelectListItem { Value = "", Text = "Selecione" });
            for (int i = 0; i < lstBancos.Count; i++)
            {
                lstbancos.Add(new SelectListItem { Value = lstBancos[i].Codigo, Text = lstBancos[i].Descricao });
            }
            cliente.LstComboBanco = new SelectList(lstbancos, "Value", "Text");


            return View("DadosCliente", cliente);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarCliente(bool permiteEditar,
            Loja.Bll.Dto.ClienteDto.DadosClienteCadastroDto dados,
            List<Loja.Bll.Dto.ClienteDto.RefComercialDtoCliente> lstRefCom,
            List<Loja.Bll.Dto.ClienteDto.RefBancariaDtoCliente> lstRefBancaria,
            Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro EndEntrega2)
        {
            /*afazer: NECESSÁRIO FAZER O TRATAMENTO PARA ERROS 
             * CRIAR A TELA DE ERRO
             * Edu sugeriu usar o erro já existente no MVC
             */

            /*Passo
             * Aqui poderá receber todos os campos, pois pode ser que os campos sejam editaveis
             * verificar se permite edição para não ter que verificar se os dados do cliente foram alterados
             * ou fazer um update independente do que veio
             * fazer a validação dos campos para poder criar um novo pedido
             * montar ModelView para listagem de produtos, forma de pagamento
             * 
             */

            //afazer: preciso de uma flag para diferenciar se é cadastro ou alteração do cliente

            string listaOperacoesPermitidas = HttpContext.Session.GetString("lista_operacoes_permitidas");
            string usuario = HttpContext.Session.GetString("usuario_atual");
            string loja = HttpContext.Session.GetString("loja_atual");
            string id_cliente = HttpContext.Session.GetString("cliente_selecionado");
            dados.Id = id_cliente;

            Bll.Dto.ClienteDto.ClienteCadastroDto clienteCadastro = new Bll.Dto.ClienteDto.ClienteCadastroDto();

            clienteCadastro.DadosCliente = dados;
            clienteCadastro.RefBancaria = lstRefBancaria;
            clienteCadastro.RefComercial = lstRefCom;

            //validar os dados do cliente             
            var retorno = await clienteBll.CadastrarCliente(clienteCadastro, usuario, loja);

            if (EndEntrega2 != null)
            {
                //Armazenando objeto na Session
                Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto dtoPedido = new Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto();
                dtoPedido.EnderecoEntrega = new EnderecoEntregaDtoClienteCadastro();
                dtoPedido.EnderecoEntrega = EndEntrega2;
                
                HttpContext.Session.SetString("pedidoDto", JsonConvert.SerializeObject(dtoPedido));
            }

            return RedirectToAction("IniciarNovoPedido", "Pedido");
        }
    }
}