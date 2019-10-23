import { Component, OnInit, Inject } from '@angular/core';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { CepDto } from 'src/app/dto/Cep/CepDto';
import { CepService } from 'src/app/servicos/cep/cep.service';
import { FormatarEndereco } from 'src/app/utils/formatarEndereco';

@Component({
  selector: 'app-cep-dialog',
  templateUrl: './cep-dialog.component.html',
  styleUrls: ['./cep-dialog.component.scss']
})
export class CepDialogComponent extends TelaDesktopBaseComponent implements OnInit {

  formatarEndereco: FormatarEndereco = new FormatarEndereco();
  carregando: boolean = false;

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
          this.alertaService.mostrarMensagem("Erro ao carregar a lista de Estados")
        }
      },
      error: (r: string) => this.alertaService.mostrarErroInternet()
    });
  }

  buscarCepPorEndereco() {

    if (!this.endereco && !this.localidade) {
      this.alertaService.mostrarMensagem("Favor digitar o endereco ou a localidade!");
    }
    else {
      this.carregando = true;
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

  filtrarPorUf(uf: string) {
    if (!uf) {
      this.alertaService.mostrarMensagem("Necessário selecionar um estado!")
    }
    else {
      let lst = this.lstEnderecos.filter((estado) => {
        return estado.Uf === uf;
      });
      console.log(lst);
    }
  }


  mascaraCep() {
    return [/\d/, /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/];
  }
}
