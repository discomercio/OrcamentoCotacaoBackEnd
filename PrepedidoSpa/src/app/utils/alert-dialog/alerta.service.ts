import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material';
import { AlertDialogComponent } from './alert-dialog.component';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AlertaService {

  constructor(public readonly dialog: MatDialog,
    public readonly router: Router) {
  }

  public mostrarMensagemComLargura(msg: string, largura: string, aposOk: () => void): void {
    const dialogRef = this.dialog.open(AlertDialogComponent, {
      width: largura,
      data: msg
    });

    if (aposOk)
      dialogRef.afterClosed().subscribe((result) => { aposOk() });
  }

  public mostrarMensagem(msg: string): void {
    this.mostrarMensagemComLargura(
      msg, "350px", null);
  }

  public mostrarMensagemBrowser(): void {
    let msg: string = "Este navegador não é suportado!\nPor favor, utilize o Google Chrome, FireFox ou Edge!";
    this.mostrarMensagemComLargura(msg, "350px", null);
  }

  public static mostrandoErroNaoAutorizado: boolean = false;
  //esta é um pouco mais estreita...
  public mostrarErroInternet(r: any): void {

    if (r != null) {
      let error: HttpErrorResponse = r;
      if (error.status == 403 || error.status == 401) {
        //colocamos um id no botão para poder simular o click
        let sair = document.getElementById('btnSair');

        if (AlertaService.mostrandoErroNaoAutorizado) {
          sair.click();
          return;
        }

        AlertaService.mostrandoErroNaoAutorizado = true;
        this.mostrarMensagemComLargura("Erro: Acesso não autorizado!", "250px", () => {
          AlertaService.mostrandoErroNaoAutorizado = false;
          sair.click();
        });

        return;
      }
      //no caso de satus code "0" mandar a msg de verifique sua conexão com a internet
      //API fora ou usuário sem internet
      if (error.status == 0) {
        //erro 500
        this.mostrarMensagemComLargura(
          "Favor verificar sua conexão com a internet!",
          "250px", null);

        return;
      }

      if (error.status == 500) {
        //erro 500
        this.mostrarMensagemComLargura(
          "Erro inesperado! Favor entrar em contato com o suporte técnico.",
          "250px", null);

        return;
      }

      //412, erro de versão
      if (this.mostrarErro412(error))
        return;

      //erro inesperado Favor entrar em contato com o suporte técnico + status.
      this.mostrarMensagemComLargura(
        "Erro inesperado! Favor entrar em contato com o suporte técnico (Código: " + error.status + ").",
        "250px", null);

      return;
    }
    else {
      //erro inesperado Favor entrar em contato com o suporte técnico + status == null.
      this.mostrarMensagemComLargura(
        "Erro inesperado! Favor entrar em contato com o suporte técnico (null).",
        "250px", null);

      return;
    }


  }


  public mostrarErro412(error: HttpErrorResponse): boolean {
    if (error.status == 412) {
      let versao = error.headers.get("X-API-Version");
      if (versao == null) {
        versao = "";
      }
      if (versao.trim() != "") {
        versao = " (" + versao + ")";
      }
      this.mostrarMensagemComLargura("Uma nova versão do sistema está disponível" + versao + ". Clique em OK para carregar a nova versão.", "250px", () => {
        window.location.reload();
      });
      return true;
    }
    return false;
  }
}



