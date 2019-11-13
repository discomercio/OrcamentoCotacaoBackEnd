import { Component, OnInit, Inject } from '@angular/core';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { CepDto } from 'src/app/dto/Cep/CepDto';
import { CepService } from 'src/app/servicos/cep/cep.service';
import { FormatarEndereco } from 'src/app/utils/formatarEndereco';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-cep-dialog',
  templateUrl: './cep-dialog.component.html',
  styleUrls: ['./cep-dialog.component.scss']
})
export class CepDialogComponent extends TelaDesktopBaseComponent implements OnInit {

  formatarEndereco: FormatarEndereco = new FormatarEndereco();
  carregando: boolean = false;
  // optionsCepLocalidades$ : Observable<string[]>;

  constructor(
    public readonly cepService: CepService,
    public readonly dialogRef: MatDialogRef<CepDialogComponent>,
    telaDesktopService: TelaDesktopService,
    public readonly alertaService: AlertaService) {
    super(telaDesktopService);
  }

  ngOnInit() {
    this.buscarUfs();
  }

  onNoClick(): void {
    //afazer: avisar que clicou em cancelar this.selecProdInfoPassado.ClicouOk = false;
    this.dialogRef.close(false);
  }

  onOkClick(): void {
    //afazer: avisar que clicou em cancelar this.selecProdInfoPassado.ClicouOk = false;
    this.dialogRef.close(true);
  }

  //campos que o usuario escolhe
  public endereco: string = "";
  public localidade: string = "";
  public uf: string = "";
  public lstUf: string[] = [];
  public lstEnderecos: CepDto[] = [];
  public endereco_selecionado: string;

  //buscar lista de estados
  buscarUfs() {
    return this.cepService.BuscarUfs().subscribe({
      next: (r: string[]) => {
        if (!!r) {
          this.lstUf = r;
        }
        else {
          this.alertaService.mostrarMensagem("Erro ao carregar a lista de Estados!")
        }
      },
      error: (r: string) => this.alertaService.mostrarErroInternet()
    });
  }

  public lstCidades: string[] = [];

  buscarLocalidades(): void {
    this.lstCidades = new Array();
    if (!this.uf) {
      this.alertaService.mostrarMensagem("Favor selecionar um Estado!");
    }
    else {
      this.carregando = true;
      this.cepService.BuscarLocalidades(this.uf).subscribe({
        next: (r: string[]) => {
          if (!!r) {
            this.carregando = false;
            this.lstCidades = r;
          }
          else {
            this.alertaService.mostrarMensagem("Erro ao carregar a lista de Cidades!")
          }
        },
        error: (r: string) => {
          this.carregando = false;
          this.alertaService.mostrarErroInternet();
        }
      });
    }
  }

  buscarCepPorEndereco() {

    // if (!this.endereco && !this.localidade) {
    //   this.alertaService.mostrarMensagem("Favor digitar o endereco ou a localidade!");
    // }
    if (!this.uf) {
      this.alertaService.mostrarMensagem("Favor selecionar um Estado!");
    }
    else if (this.uf && !this.localidade) {
      this.alertaService.mostrarMensagem("Favor digitar uma localidade!");
    }
    else {
      
      this.carregando = true;
      for (let i = 0; i < this.lstCidades.length; i++) {
        if (this.localidade == this.lstCidades[i]) {
          //afazer: verificar a possibilidade de fazer a busca apenas por estado
          return this.cepService.buscarCepPorEndereco(this.endereco, this.localidade, this.uf).subscribe({
            next: (r: CepDto[]) => {
              this.carregando = false;
              if (!!r) {
                this.lstEnderecos = r;
              }
              else {
                this.alertaService.mostrarMensagem("Erro ao carregar a lista de Endereços!")
              }
            },
            error: (r: CepDto[]) => {
              this.carregando = false;
              this.alertaService.mostrarErroInternet();
            }
          });
        }
      }
      if(this.carregando){
        this.carregando = false;
        this.alertaService.mostrarMensagem("Favor selecionar uma Localidade na lista!");
      }
    }
  }

  filtrarPorUf(uf: string) {
    if (!uf) {
      this.alertaService.mostrarMensagem("Necessário selecionar um estado!")
    }
    else {
      let lst = this.lstEnderecos.filter((estado) => {
        return estado.Uf === uf;
      });
    }
  }


  mascaraCep() {
    return [/\d/, /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/];
  }
}
