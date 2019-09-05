import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material';
import { AlertDialogComponent } from './alert-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class AlertaService {

  constructor(public readonly dialog: MatDialog) { }

  public mostrarMensagemComLargura(msg: string, largura: string): void {
    const dialogRef = this.dialog.open(AlertDialogComponent, {
      width: largura,
      data: msg
    });
  }

  public mostrarMensagem(msg: string): void {
    this.mostrarMensagemComLargura(
      msg, "350px");
  }

  //esta é um pouco mais estreita...
  public mostrarErroInternet(): void {
    this.mostrarMensagemComLargura(
      "Erro no acesso ao sistema. Verifique a conexão com a internet.",
      "250px");
  }

}
