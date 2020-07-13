import { Component, OnInit, Input, Output, EventEmitter, ViewChild, Inject } from '@angular/core';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { EnderecoEntregaDtoClienteCadastro } from 'src/app/dto/ClienteCadastro/EnderecoEntregaDTOClienteCadastro';
import { ClienteCadastroUtils } from 'src/app/utils/ClienteCadastroUtils';
import { BuscarClienteService, JustificativaEndEntregaComboDto } from 'src/app/servicos/cliente/buscar-cliente.service';
import { EnderecoEntregaJustificativaDto } from 'src/app/dto/ClienteCadastro/EnderecoEntregaJustificativaDto';
import { CepComponent } from 'src/app/cliente/cep/cep/cep.component';
import { ClienteCorpoComponent } from 'src/app/cliente/cliente-corpo/cliente-corpo.component';
import { Constantes } from 'src/app/dto/Constantes';
import { FormatarTelefone } from 'src/app/utils/formatarTelefone';
import { CpfCnpjUtils } from 'src/app/utils/cpfCnpjUtils';

@Component({
  selector: 'app-confirmar-endereco',
  templateUrl: './confirmar-endereco.component.html',
  styleUrls: [
    './confirmar-endereco.component.scss',
    '../../../estilos/endereco.scss'
  ]
})
export class ConfirmarEnderecoComponent implements OnInit {

  constructor(private readonly buscarClienteService: BuscarClienteService) { }

  buscarClienteServiceJustificativaEndEntregaComboTemporario: EnderecoEntregaJustificativaDto[];
  ngOnInit() {
    this.inicializarCamposEndereco(this.enderecoEntregaDtoClienteCadastro);

    this.enderecoEntregaDtoClienteCadastro.OutroEndereco = false;

    this.buscarClienteServiceJustificativaEndEntregaComboTemporario = this.buscarClienteService.JustificativaEndEntregaComboTemporario();

  }

  atualizarDadosEnderecoTela(enderecoEntregaDtoClienteCadastro: EnderecoEntregaDtoClienteCadastro) {

    this.enderecoEntregaDtoClienteCadastro = enderecoEntregaDtoClienteCadastro;
    const src = this.componenteCep;
    src.Endereco = this.enderecoEntregaDtoClienteCadastro.EndEtg_endereco;
    src.Numero = this.enderecoEntregaDtoClienteCadastro.EndEtg_endereco_numero;
    src.Complemento = this.enderecoEntregaDtoClienteCadastro.EndEtg_endereco_complemento;
    src.Bairro = this.enderecoEntregaDtoClienteCadastro.EndEtg_bairro;
    src.Cidade = this.enderecoEntregaDtoClienteCadastro.EndEtg_cidade;
    src.Uf = this.enderecoEntregaDtoClienteCadastro.EndEtg_uf;
    src.Cep = this.enderecoEntregaDtoClienteCadastro.EndEtg_cep;
    enderecoEntregaDtoClienteCadastro.EndEtg_cod_justificativa = this.enderecoEntregaDtoClienteCadastro.EndEtg_cod_justificativa;
    this.tipoPessoaEntrega = this.enderecoEntregaDtoClienteCadastro.EndEtg_tipo_pessoa == this.constantes.ID_PJ ? true : false;
  }

  

  //dados
  @Input() enderecoEntregaDtoClienteCadastro = new EnderecoEntregaDtoClienteCadastro();
  @Input() tipoPf: boolean;

  //utilitários
  public clienteCadastroUtils = new ClienteCadastroUtils();

  //precisa do static: false porque está dentro de um ngif
  @ViewChild("componenteCep", { static: false }) componenteCep: CepComponent;
  public podeAvancar(): boolean {

    return !this.componenteCep.carregando;
  }
  public prepararAvancar(): void {
    //transferimos os dados do CEP para cá
    const src = this.componenteCep;

    this.enderecoEntregaDtoClienteCadastro.OutroEndereco;

    this.enderecoEntregaDtoClienteCadastro.EndEtg_endereco = src.Endereco ? src.Endereco : "";
    this.enderecoEntregaDtoClienteCadastro.EndEtg_endereco_numero = src.Numero ? src.Numero : "";
    this.enderecoEntregaDtoClienteCadastro.EndEtg_endereco_complemento = src.Complemento ? src.Complemento : "";
    this.enderecoEntregaDtoClienteCadastro.EndEtg_bairro = src.Bairro ? src.Bairro : "";
    this.enderecoEntregaDtoClienteCadastro.EndEtg_cidade = src.Cidade ? src.Cidade : "";
    this.enderecoEntregaDtoClienteCadastro.EndEtg_uf = src.Uf ? src.Uf : "";
    this.enderecoEntregaDtoClienteCadastro.EndEtg_cep = src.Cep ? src.Cep : "";
  }

  public mudarFoco(): void {
    let article = document.getElementById("article-confirmar-cliente");
    article.scrollTop = article.scrollHeight;
  }

  public voltarFoco(): void {
    let article = document.getElementById("article-confirmar-cliente");
    article.scrollTop = 0;
  }

  public mascaraTelefone = FormatarTelefone.mascaraTelefone;
  public mascaraCpf = CpfCnpjUtils.mascaraCpf;
  public mascaraCnpj = CpfCnpjUtils.mascaraCnpj;
  constantes: Constantes = new Constantes();
  tipoPessoaEntrega: boolean;

  inicializarCamposEndereco(enderecoEntrega: EnderecoEntregaDtoClienteCadastro) {
    if (!enderecoEntrega) return;

    enderecoEntrega.EndEtg_cnpj_cpf = "";
    enderecoEntrega.EndEtg_nome = "";
    enderecoEntrega.EndEtg_cep = "";
    enderecoEntrega.EndEtg_endereco = "";
    enderecoEntrega.EndEtg_endereco_numero = "";
    enderecoEntrega.EndEtg_bairro = "";
    enderecoEntrega.EndEtg_cidade = "";
    enderecoEntrega.EndEtg_uf = "";
    enderecoEntrega.EndEtg_endereco_complemento = "";
    enderecoEntrega.EndEtg_ddd_cel = "";
    enderecoEntrega.EndEtg_tel_cel = "";
    enderecoEntrega.EndEtg_ddd_res = "";
    enderecoEntrega.EndEtg_tel_res = "";
    enderecoEntrega.EndEtg_ddd_com = "";
    enderecoEntrega.EndEtg_tel_com = "";
    enderecoEntrega.EndEtg_ddd_com_2 = "";
    enderecoEntrega.EndEtg_tel_com_2 = "";
    enderecoEntrega.EndEtg_produtor_rural_status = 0;
    enderecoEntrega.EndEtg_contribuinte_icms_status = 0;
    enderecoEntrega.EndEtg_ie = "";
    enderecoEntrega.EndEtg_tipo_pessoa = "";
  }

  PF() {
    this.enderecoEntregaDtoClienteCadastro.EndEtg_cnpj_cpf = "";
    this.tipoPessoaEntrega = false;
    this.enderecoEntregaDtoClienteCadastro.EndEtg_tipo_pessoa = this.constantes.ID_PF;
  }

  PJ() {
    this.enderecoEntregaDtoClienteCadastro.EndEtg_cnpj_cpf = "";
    this.tipoPessoaEntrega = true;
    this.enderecoEntregaDtoClienteCadastro.EndEtg_tipo_pessoa = this.constantes.ID_PJ;
  }
}
