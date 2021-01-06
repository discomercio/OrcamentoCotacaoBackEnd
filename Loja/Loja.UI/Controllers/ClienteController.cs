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

        [HttpGet]
        public async Task<IActionResult> ValidarCliente(string cpf_cnpj)
        {
            cpf_cnpj = Loja.Bll.Util.Util.SoDigitosCpf_Cnpj(cpf_cnpj);

            bool novoCliente = false;
            if (!await clienteBll.ValidarCliente(cpf_cnpj))
            {
                //colocar uma msg de erro para retornar para a tela
                novoCliente = true;
                return Json(novoCliente);
                //return RedirectToAction("Index", new { cpf_cnpj = cpf_cnpj, novoCliente = novoCliente });
            }
            else
                return Json(novoCliente = false);
            //return RedirectToAction("BuscarCliente", new { cpf_cnpj = cpf_cnpj, novoCliente = novoCliente }); //editar cliente
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
            //vamos verificar se o cliente é novo

            //somente para teste remover após concluir
            //cliente.PermiteEdicao = false;

            ClienteCadastroDto clienteCadastroDto = new ClienteCadastroDto();

            if (!novoCliente)
            {
                clienteCadastroDto = await clienteBll.BuscarCliente(UtilsGlobais.Util.SoDigitosCpf_Cnpj(cpf_cnpj), usuarioLogado.Usuario_atual);

                usuarioLogado.Cliente_Selecionado = clienteCadastroDto;

                cliente.RefBancaria = clienteCadastroDto.RefBancaria;
                cliente.RefComercial = clienteCadastroDto.RefComercial;
                cliente.Cadastrando = false;
            }
            else
            {
                cliente.PermiteEdicao = true;
                clienteCadastroDto.DadosCliente = new DadosClienteCadastroDto();
                clienteCadastroDto.DadosCliente.Cnpj_Cpf = cpf_cnpj;

                if (UtilsGlobais.Util.SoDigitosCpf_Cnpj(cpf_cnpj).Length == 11)
                    clienteCadastroDto.DadosCliente.Tipo = "PF";
                if (UtilsGlobais.Util.SoDigitosCpf_Cnpj(cpf_cnpj).Length == 14)
                    clienteCadastroDto.DadosCliente.Tipo = "PJ";

                cliente.Cadastrando = true;
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

            var lstSexo = new[]
            {
                new SelectListItem{Value = "", Text = "Selecione"},
                new SelectListItem{Value = "M", Text = "Masculino"},
                new SelectListItem{Value = "F", Text = "Feminino"}
            };
            cliente.LstSexo = new SelectList(lstSexo, "Value", "Text");

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

            //vamos buscar a lista de IBGE para confrontar na validação de tela

            //vamos carregar os select's de cep
            cliente.Cep = new Models.Cep.CepViewModel();
            cliente.Cep.ClienteTipo = cliente.DadosCliente.Tipo;
            //lista para carregar no select de Produtor Rural              
            cliente.Cep.LstProdutoRural = new SelectList(lstProdR, "Value", "Text");
            //lista para carregar o Contribuinte ICMS           
            cliente.Cep.LstContribuinte = new SelectList(lstContrICMS, "Value", "Text");

            return View("DadosCliente", cliente);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarCliente(bool permiteEditar,
            Loja.Bll.Dto.ClienteDto.DadosClienteCadastroDto dados,
            List<Loja.Bll.Dto.ClienteDto.RefComercialDtoCliente> lstRefCom,
            List<Loja.Bll.Dto.ClienteDto.RefBancariaDtoCliente> lstRefBancaria,
            Loja.Bll.Dto.ClienteDto.EnderecoEntregaDtoClienteCadastro EndEntrega2,
            bool cadastrando)
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
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);

            if (cadastrando)
            {
                Bll.Dto.ClienteDto.ClienteCadastroDto novo_clienteCadastro = new Bll.Dto.ClienteDto.ClienteCadastroDto();

                novo_clienteCadastro.DadosCliente = dados;
                novo_clienteCadastro.RefBancaria = lstRefBancaria;
                novo_clienteCadastro.RefComercial = lstRefCom;
                //vamos cadastrar o cliente
                List<string> lstRetorno = (await clienteBll.Novo_CadastrarCliente(novo_clienteCadastro, usuarioLogado.Usuario_atual)).ToList();
                                
                if (lstRetorno.Count > 1)
                {
                    //afazer: tratar erro para retornar para usuário
                    //deu erro vamos redirecionar 
                    //return RedirectToAction("Error", "Home", new { lstErros = lstRetorno });
                }
                else if (lstRetorno.Count == 1)
                {
                    //vamos verificar se é o id do cliente cadastrado
                    if (lstRetorno[0].Length != 12)
                    {
                        //é o id
                        string idClienteNovo = lstRetorno[0];
                        //depois de cadastrar, vamos redirecionar o usuário para a tela de cadastro do cliente cadastrado
                        //para caso queira realizar um novo pedido
                        return RedirectToAction("BuscarCliente", new { cpf_cnpj = idClienteNovo, novoCliente = false });
                    }
                }
            }

            /* OBS: será necessário criar uma nova rotina em Global/Cliente para fazer a 
             * alteração completa de cadastro do cliente.
             * Verificar se conseguimos utilizar a criação de log de AtualizaParcial
             * Será necessário realizar a confrontação de dados do cliente para criação de log
             * Será necessário realizar a confrontação de referências em caso de PJ
             * vai dar um trabalho!!!             
             */


            /* OBS 2: Não iremos utilizar "Session" para a criação de novo Pedido
             * vamos validar os dados de cada tela mas, iremos devolver para a View e assim por diante.
             */

            //REFAZER DAQUI PARA BAIXO
            /*==============================================================*/


            //afazer: alterar essa session para utilizar no usuarioLogado
            //string id_cliente = HttpContext.Session.GetString("cliente_selecionado");
            dados.Id = usuarioLogado.Cliente_Selecionado.DadosCliente.Id;

            Bll.Dto.ClienteDto.ClienteCadastroDto clienteCadastro = new Bll.Dto.ClienteDto.ClienteCadastroDto();

            clienteCadastro.DadosCliente = dados;
            clienteCadastro.RefBancaria = lstRefBancaria;
            clienteCadastro.RefComercial = lstRefCom;

            //validar os dados do cliente 
            //alterar para fazer o cadastro pelo Global/Cliente
            var retorno = await clienteBll.CadastrarCliente(clienteCadastro, usuarioLogado.Usuario_atual,
                usuarioLogado.Loja_atual_id);

            //Não iremos mais armazenar em session. Iremos enviar o dto de pedido e retornar o 
            //dto do pedido em cada página.
            //Armazenando objeto na Session
            Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto dtoPedido = new Bll.Dto.PedidoDto.DetalhesPedido.PedidoDto();
            dtoPedido.DadosCliente = new DadosClienteCadastroDto();
            dtoPedido.DadosCliente = dados;

            if (EndEntrega2 != null)
            {
                if (EndEntrega2.EndEtg_cep != null)
                {
                    //vamos validar o endereço de entrega no Global/Prepedido
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