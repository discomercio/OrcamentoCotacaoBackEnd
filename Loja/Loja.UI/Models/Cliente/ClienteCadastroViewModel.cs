using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Loja.Bll.Dto.ClienteDto;
using Loja.Bll.Dto.CepDto;
using Loja.UI.Models.Cep;
using Microsoft.AspNetCore.Mvc.Rendering;
using Loja.Bll.Dto.PedidoDto.DetalhesPedido;

namespace Loja.UI.Models.Cliente
{
    public class ClienteCadastroViewModel
    {
        public bool Cadastrando { get; set; }
        public string TipoCliente { get; set; }
        public Boolean PermiteEdicao { get; set; }
        public DadosClienteCadastroDto DadosCliente { get; set; }
        public List<RefBancariaDtoCliente> RefBancaria { get; set; }
        public List<RefComercialDtoCliente> RefComercial { get; set; }
        public EnderecoEntregaDtoClienteCadastro EndEntrega { get; set; }
        public SelectList LstContribuinte { get; set; }
        public SelectList LstProdutoRural { get; set; }
        public SelectList LstIndicadores { get; set; }
        public SelectList EndJustificativa { get; set; }
        public SelectList LstComboBanco { get; set; }
        public CepViewModel Cep { get; set; }

        public PedidoDto PedidoDto { get; set; }


        public ClienteCadastroViewModel(bool permiteEditar, bool cadastrando, ClienteCadastroDto clienteCadastroDto,
            List<string> lstIndicadores, List<EnderecoEntregaJustificativaDto> lstJustificativas,
            List<ListaBancoDto> lstBancos)
        {
            Cadastrando = cadastrando;
            TipoCliente = clienteCadastroDto.DadosCliente.Tipo;
            PermiteEdicao = permiteEditar;
            DadosCliente = clienteCadastroDto.DadosCliente;
            RefBancaria = clienteCadastroDto.RefBancaria;
            RefComercial = clienteCadastroDto.RefComercial;
            MontarListaContribuinteICMS();
            MontarListaProdutorRural();
            MontarListaIndicadores(lstIndicadores);
            MontarListaJustificativaEnderecoEntrega(lstJustificativas);
            MontarListaBancos(lstBancos);
            MontarCepViewModel(clienteCadastroDto.DadosCliente.Tipo);
        }

        

        private void MontarListaJustificativaEnderecoEntrega(List<EnderecoEntregaJustificativaDto> lst)
        {
            List<SelectListItem> lstSelect = new List<SelectListItem>();
            lstSelect.Add(new SelectListItem { Value = "", Text = "Selecione" });
            foreach (var i in lst)
            {
                lstSelect.Add(new SelectListItem { Value = i.EndEtg_cod_justificativa, Text = i.EndEtg_descricao_justificativa });
            }
            EndJustificativa = new SelectList(lstSelect, "Value", "Text");
        }

        private void MontarListaBancos(List<ListaBancoDto> lstBancos)
        {
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
            LstComboBanco = new SelectList(lstbancos, "Value", "Text");
        }

        private void MontarListaContribuinteICMS()
        {
            var lstContrICMS = new[]
            {
                new SelectListItem{Value = Bll.Constantes.Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL, Text="Selecione"},
                new SelectListItem{Value = Bll.Constantes.Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO, Text = "Isento"},
                new SelectListItem{Value = Bll.Constantes.Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO, Text = "Não"},
                new SelectListItem{Value = Bll.Constantes.Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM, Text = "Sim"}
            };
            LstContribuinte = new SelectList(lstContrICMS, "Value", "Text");
        }

        private void MontarListaProdutorRural()
        {
            var lstProdR = new[]
            {
                new SelectListItem{Value = "", Text = "Selecione"},
                new SelectListItem{Value = Bll.Constantes.Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO, Text = "Não"},
                new SelectListItem{Value = Bll.Constantes.Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM, Text = "Sim"}
            };
            LstProdutoRural = new SelectList(lstProdR, "Value", "Text");
        }

        private void MontarListaIndicadores(List<string> lstIndicadores)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem { Value = "0", Text = "Selecione" });
            for (int i = 0; i < lstIndicadores.Count; i++)
            {
                lst.Add(new SelectListItem { Value = lstIndicadores[i], Text = lstIndicadores[i] });
            }
            LstIndicadores = new SelectList(lst, "Value", "Text");
        }

        private void MontarCepViewModel(string tipo)
        {
            Cep = new Models.Cep.CepViewModel();
            Cep.ClienteTipo = tipo;
        }
    }
}
