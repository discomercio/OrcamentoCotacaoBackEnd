import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { EnderecoEntregaDtoClienteCadastro } from 'src/app/dto/ClienteCadastro/EnderecoEntregaDTOClienteCadastro';
import { ClienteCadastroUtils } from 'src/app/dto/AngularClienteCadastroUtils/ClienteCadastroUtils';
import { BuscarClienteService, JustificativaEndEntregaComboDto } from 'src/app/servicos/cliente/buscar-cliente.service';
import { EnderecoEntregaJustificativaDto } from 'src/app/dto/ClienteCadastro/EnderecoEntregaJustificativaDto';
import { CepComponent } from 'src/app/cliente/cep/cep/cep.component';

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
  }

  //dados
  @Input() dadosClienteCadastroDto = new DadosClienteCadastroDto();
  @Input() enderecoEntregaDtoClienteCadastro = new EnderecoEntregaDtoClienteCadastro();

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

  public voltarFoco():void{
    let article = document.getElementById("article-confirmar-cliente");
    article.scrollTop = 0;
  }

}
