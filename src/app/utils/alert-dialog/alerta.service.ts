import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material';
import { AlertDialogComponent } from './alert-dialog.component';
import { HttpErrorResponse } from '@angular/common/http';
import { AppComponent } from 'src/app/app.component';
import { AutenticacaoService } from 'src/app/servicos/autenticacao/autenticacao.service';
import { Router } from '@angular/router';
import { $ } from 'protractor';

@Injectable({
  providedIn: 'root'
})
export class AlertaService {

  constructor(public readonly dialog: MatDialog,
    public readonly router: Router,
    private readonly autenticacaoService: AutenticacaoService) {
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

  public static mostrandoErroNaoAutorizado: boolean = false;
  //esta é um pouco mais estreita...
  public mostrarErroInternet(r: any): void {
    debugger;
    //afazer: passar o status do retorno e verificar se o status é 403 e redirecionar o usuário para o login e passar o router
    if (r != null) {
      let error: HttpErrorResponse = r;
      if (error.status == 403) {
        //colocamos um id no botão para poder simular o click
        let sair = document.getElementById('btnSair');
        
        if (AlertaService.mostrandoErroNaoAutorizado){
          sair.click();
          return;
        }

        AlertaService.mostrandoErroNaoAutorizado = true;

        this.mostrarMensagemComLargura("Erro: Acesso não autorizado!", "250px", () => {
          sair.click();
        });

        return;
      }

    }


    this.mostrarMensagemComLargura(
      "Erro no acesso ao sistema. Verifique a conexão com a internet.",
      "250px", null);
  }

}
