import { Component, OnInit } from '@angular/core';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { CepService } from 'src/app/servicos/cep/cep.service';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { MatDialog } from '@angular/material';
import { CepDialogComponent } from '../cep-dialog/cep-dialog.component';
import { CepDto } from 'src/app/dto/Cep/CepDto';

@Component({
  selector: 'app-cep',
  templateUrl: './cep.component.html',
  styleUrls: ['./cep.component.scss', '../../../estilos/endereco.scss']
})
export class CepComponent extends TelaDesktopBaseComponent implements OnInit {

  constructor(private readonly alertaService: AlertaService,
    private readonly cepService: CepService,
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

    this.temCidade = true;
    this.temBairro = true;
    this.temEndereco = true;
    this.temUf = true;
  }

  public temEndereco: boolean = true;
  public temBairro: boolean = true;
  public temCidade: boolean = true;
  public temUf: boolean = true;
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

    this.zerarCamposEndEntrega();

    //vamos fazer a busca
    this.carregando = true;
    this.cepService.buscarCep(this.Cep, null, null, null).toPromise()
      .then((r) => {
        this.carregando = false;

        if (!r || r.length !== 1) {
          this.mostrarCepNaoEncontrado();
          return;
        }

        //recebemos um endereço
        const end = r[0];
        if (!!end.Bairro) {
          this.Bairro = end.Bairro;
          this.temBairro = true;
        }
        if (!!end.Cidade) {
          this.Cidade = end.Cidade;
          this.temCidade = true;
        }
        if (!!end.Endereco) {
          this.Endereco = end.Endereco;
          this.temEndereco = true;
        }
        if (!!end.Uf) {
          this.Uf = end.Uf;
          this.temUf = true;
        }

      }).catch((r) => {
        //deu erro na busca
        //ou não achou nada...
        this.carregando = false;
        this.alertaService.mostrarErroInternet();
      });
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

        if (!!end.Endereco.trim()) {
          this.Endereco = end.Endereco;
          this.temEndereco = true;
        }
        else{
          this.temEndereco = false;
        }
        if (!!end.Uf) {
          this.Uf = end.Uf;
          this.temUf = true;
        }
        if (!!end.Bairro.trim()) {
          this.Bairro = end.Bairro;
          this.temBairro = true;
        }
        else{
          this.temBairro = false;
        }
        if (!!end.Cidade.trim()) {
          this.Cidade = end.Cidade;
          this.temCidade = true;
        }
        if (!!end.LogradouroComplemento) {
          this.Complemento = end.LogradouroComplemento;
        }

        this.Cep = end.Cep;
      }
    });
  }


}
