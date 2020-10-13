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
using Loja.Bll.Bll.AcessoBll;
using Loja.Bll.Util;
using Microsoft.Extensions.Logging;

namespace Loja.UI.Controllers
{
    public class ClienteController : Controller
    {
        private readonly Bll.ClienteBll.ClienteBll clienteBll;
        private readonly UsuarioAcessoBll usuarioAcessoBll;
        private readonly Configuracao configuracao;
        private readonly ILogger<UsuarioLogado> loggerUsuarioLogado;

        public ClienteController(Bll.ClienteBll.ClienteBll clienteBll, UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao,
            ILogger<UsuarioLogado> loggerUsuarioLogado)
        {
            this.clienteBll = clienteBll;
            this.usuarioAcessoBll = usuarioAcessoBll;
            this.configuracao = configuracao;
            this.loggerUsuarioLogado = loggerUsuarioLogado;
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
            cpf_cnpj = Loja.Bll.Util.Util.SoDigitosCpf_Cnpj(cpf_cnpj);

            bool novoCliente = false;
            if (!await clienteBll.ValidarCliente(cpf_cnpj))
            {
                //colocar uma msg de erro para retornar para a tela
                novoCliente = true;
                return RedirectToAction("BuscarCliente", new { cpf_cnpj = cpf_cnpj, novoCliente = novoCliente });
            }
            else
                return RedirectToAction("BuscarCliente", new { cpf_cnpj = cpf_cnpj, novoCliente = novoCliente }); //editar cliente
        }

        public async Task<IActionResult> BuscarCliente(string cpf_cnpj, bool novoCliente)
        {
            /*Passo a passo da entrada 
              pegar a session do usuario
              pegar a session da loja
              criar session de operações permitidas
              verificar a operação permitida
              buscar os dados do cliente
              buscar indicadores 
            */

            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);

            ClienteCadastroViewModel cliente = new ClienteCadastroViewModel();

            cliente.PermiteEdicao = Loja.Bll.Util.Util.OpercaoPermitida(Loja.Bll.Constantes.Constantes.OP_LJA_EDITA_CLIENTE_DADOS_CADASTRAIS
                , usuarioLogado.S_lista_operacoes_permitidas) ? true : false;


            //somente para teste remover após concluir
            //cliente.PermiteEdicao = false;
            //afazer: alterar essa session para utilizar no usuarioLogado
            //vou passar o cadastro do cliente para a session
            //HttpContext.Session.SetString("cpf_cnpj", cpf_cnpj);

            ClienteCadastroDto clienteCadastroDto = new ClienteCadastroDto();

            if (!novoCliente)
            {
                clienteCadastroDto = await clienteBll.BuscarCliente(UtilsGlobais.Util.SoDigitosCpf_Cnpj(cpf_cnpj), usuarioLogado.Usuario_atual);

                usuarioLogado.Cliente_Selecionado = clienteCadastroDto;

                cliente.RefBancaria = clienteCadastroDto.RefBancaria;
                cliente.RefComercial = clienteCadastroDto.RefComercial;

            }
            else
            {
                clienteCadastroDto.DadosCliente = new DadosClienteCadastroDto();
                clienteCadastroDto.DadosCliente.Cnpj_Cpf = cpf_cnpj;
                if (cpf_cnpj.Length == 11)
                    clienteCadastroDto.DadosCliente.Tipo = "PF";
                if (cpf_cnpj.Length == 14)
                    clienteCadastroDto.DadosCliente.Tipo = "PJ";
            }

            cliente.DadosCliente = clienteCadastroDto.DadosCliente;



            //Lista para carregar no select de Indicadores
            var lstInd = (await clienteBll.BuscarListaIndicadores(cliente.DadosCliente?.Indicador_Orcamentista,
                usuarioLogado.Usuario_atual, usuarioLogado.Loja_atual_id)).ToList();

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

            var lstJustificativas = (await clienteBll.ListarComboJustificaEndereco(usuarioLogado.Usuario_atual)).ToList();
            List<SelectListItem> lstSelect = new List<SelectListItem>();
            lstSelect.Add(new SelectListItem { Value = "", Text = "Selecione" });
            foreach (var i in lstJustificativas)
            {
                lstSelect.Add(new SelectListItem { Value = i.EndEtg_cod_justificativa, Text = i.EndEtg_descricao_justificativa });
            }

            cliente.EndJustificativa = new SelectList(lstSelect, "Value", "Text");

            var lstBancos = (await clienteBll.ListarBancosCombo()).ToList();
            List<SelectListItem> lstbancos = new List<SelectListItem>();
            lstbancos.Add(new SelectListItem { Value = "", Text = "Selecione" });
            for (int i = 0; i < lstBancos.Count; i++)
            {
                lstbancos.Add(new SelectListItem
                {
                    Value = lstBancos[i].Codigo,
                    Text = lstBancos[i].Descricao
                });
            }
            cliente.LstComboBanco = new SelectList(lstbancos, "Value", "Text");


            return View("DadosCliente", cliente);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarCliente(bool permiteEditar,
            Loja.Bll.Dto.ClienteDto.DadosClienteCadastroDto dados,
            List<Loja.Bll.Dto.ClienteDto.RefComercialDtoCliente> lstRefCom,
            List<Loja.Bll.Dto.ClienteDto.RefBancariaDtoCliente> lstRefBancaria,
            Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro EndEntrega2,
            bool novoCliente)
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
            //para teste: novoCliente 
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);


            //afazer: alterar essa session para utilizar no usuarioLogado
            //string id_cliente = HttpContext.Session.GetString("cliente_selecionado");
            dados.Id = usuarioLogado.Cliente_Selecionado.DadosCliente.Id;

            Bll.Dto.ClienteDto.ClienteCadastroDto clienteCadastro = new Bll.Dto.ClienteDto.ClienteCadastroDto();

            clienteCadastro.DadosCliente = dados;
            clienteCadastro.RefBancaria = lstRefBancaria;
            clienteCadastro.RefComercial = lstRefCom;

            //validar os dados do cliente             
            var retorno = await clienteBll.CadastrarCliente(clienteCadastro, usuarioLogado.Usuario_atual,
                usuarioLogado.Loja_atual_id);

            //Armazenando objeto na Session
            Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto dtoPedido = new Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto();
            dtoPedido.DadosCliente = new DadosClienteCadastroDto();
            dtoPedido.DadosCliente = dados;

            if (EndEntrega2 != null)
            {
                if (EndEntrega2.EndEtg_cep != null)
                {
                    dtoPedido.EnderecoEntrega = new EnderecoEntregaDtoClienteCadastro();
                    //vamos normalizar o cep enviado antes de armazenar na session
                    EndEntrega2.EndEtg_cep = EndEntrega2.EndEtg_cep.Replace("-", "");
                }
                dtoPedido.EnderecoEntrega = EndEntrega2;
            }

            usuarioLogado.PedidoDto = dtoPedido;

            return RedirectToAction("Indicador_SelecaoCD", "Pedido");
        }
    }
}