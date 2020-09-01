import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { CepService } from 'src/app/servicos/cep/cep.service';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { MatDialog } from '@angular/material';
import { CepDialogComponent } from '../cep-dialog/cep-dialog.component';
import { CepDto } from 'src/app/dto/Cep/CepDto';
import { EnderecoEntregaDtoClienteCadastro } from 'src/app/dto/ClienteCadastro/EnderecoEntregaDTOClienteCadastro';
import { ConfirmarEnderecoComponent } from 'src/app/prepedido/novo-prepedido/confirmar-endereco/confirmar-endereco.component';
import { $ } from 'protractor';
import { StringUtils } from 'src/app/utils/stringUtils';
import { debugOutputAstAsTypeScript } from '@angular/compiler';


@Component({
  selector: 'app-cep',
  templateUrl: './cep.component.html',
  styleUrls: ['./cep.component.scss', '../../../estilos/endereco.scss']
})
export class CepComponent extends TelaDesktopBaseComponent implements OnInit {

  constructor(private readonly alertaService: AlertaService,
    public readonly cepService: CepService,
    public readonly dialog: MatDialog,
    telaDesktopService: TelaDesktopService
  ) {
    super(telaDesktopService);
  }

  ngOnInit() {

  }


  //sempre usamos no modo auto
  public floatLabel(): string {
    return "auto";

  }

  required: boolean;
  //indicador que estamos esperando dados
  public carregando = false;

  //campos sendo editados
  public Endereco: string;
  public Numero: string;
  public Complemento: string;
  public Bairro: string;
  public Cidade: string;
  public Uf: string;
  public Cep: string;

  mostrarCepNaoEncontrado() {
    this.Endereco = "";
    this.Numero = "";
    this.Complemento = "";
    this.Bairro = "";
    this.Cidade = "";
    this.Uf = "";
    this.Cep = "";
    this.alertaService.mostrarMensagem("CEP inválido ou não encontrado.");

  }

  mascaraCep() {
    return [/\d/, /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/];
  }

  public zerarCamposEndEntrega(): void {
    this.Cidade = "";
    this.Endereco = "";
    this.Bairro = "";
    this.Uf = "";
    this.Complemento = "";
    this.Numero = "";

    this.temCidade = false;
    this.temUf = false;
  }

  public lstCidadeIBGE: string[];
  public temCidade: boolean;
  public temUf: boolean;
  public cep_retorno: string;

  //saiu do campo de CEP, vamos carregar o endereco
  saiuCep() {

    //se vazio, não damos nenhuma mensagem
    if (this.Cep == "" || this.Cep == 'undefined') {
      //nao avisamos
      this.Endereco = "";
      this.Numero = "";
      this.Complemento = "";
      this.Bairro = "";
      this.Cidade = "";
      this.Uf = "";
      return false;
    }
    
    if (this.cep_retorno != undefined) {
      if (StringUtils.retorna_so_digitos(this.cep_retorno) == StringUtils.retorna_so_digitos(this.Cep)) {
        return;
      }
    }
    if(this.Cep != undefined && this.Cep != ""){
      this.zerarCamposEndEntrega();
      //vamos fazer a busca
      this.carregando = true;
  
      this.cepService.buscarCep(this.Cep, null, null, null).toPromise()
        .then((r) => {
          this.carregando = false;
  
          if (!r || r.length !== 1) {
            this.cep_retorno = "";
            this.mostrarCepNaoEncontrado();
            return;
          }
          //recebemos um endereço
          const end = r[0];
  
          this.cep_retorno = this.Cep;
          if (!!end.Bairro) {
            this.Bairro = end.Bairro;
          }
          if (!!end.Cidade) {
            if (!!end.ListaCidadeIBGE && end.ListaCidadeIBGE.length > 0) {
              this.temCidade = false;
              this.lstCidadeIBGE = end.ListaCidadeIBGE;
            }
            else {
              this.Cidade = end.Cidade;
              this.temCidade = true;
            }
  
          }
          if (!!end.Endereco) {
            this.Endereco = end.Endereco;
          }
          if (!!end.Uf) {
            this.Uf = end.Uf;
            this.temUf = true;
          }
  
        }).catch((r) => {
          //deu erro na busca
          //ou não achou nada...
          
          this.carregando = false;
          this.alertaService.mostrarErroInternet(r);
        });
    }
    




  }

  enterCep(event: Event) {
    document.getElementById("numero").focus();
    event.cancelBubble = true;
    event.preventDefault();
    // event.srcElement

  }

  //para acessar a caixa de diálogo
  buscarCep() {

    this.zerarCamposEndEntrega();

    let options: any = {
      autoFocus: false,
      width: "60em"
      //não colocamos aqui para que ele ajuste melhor: height:"85vh",
    };
    if (!this.telaDesktop) {
      //opções para celular
      options = {
        autoFocus: false,
        width: "100vw", //para celular, precisamos da largura toda
        maxWidth: "100vw"
        //não colocamos aqui para que ele ajuste melhor: height:"85vh",
      };
    }
    const dialogRef = this.dialog.open(CepDialogComponent, options);

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        let end: CepDto = dialogRef.componentInstance.lstEnderecos[dialogRef.componentInstance.endereco_selecionado];
        
        if (!!end.Uf) {
          this.Uf = end.Uf;
          this.temUf = true;
        }
        if (!!end.Cidade) {
          if (!!end.ListaCidadeIBGE && end.ListaCidadeIBGE.length > 0) {
            this.temCidade = false;
            this.lstCidadeIBGE = end.ListaCidadeIBGE;
          }
          else {
            this.Cidade = end.Cidade;
            this.temCidade = true;
          }

        }
        if (!!end.Bairro) {
          this.Bairro = end.Bairro;
        }
        if (!!end.Endereco) {
          this.Endereco = end.Endereco;
        }

        this.Cep = end.Cep;
        this.cep_retorno = end.Cep;
      }
    });
  }


}
