import { Component, OnInit, Inject } from '@angular/core';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';

@Component({
  selector: 'app-cep-dialog',
  templateUrl: './cep-dialog.component.html',
  styleUrls: ['./cep-dialog.component.scss']
})
export class CepDialogComponent  extends TelaDesktopBaseComponent implements OnInit {

  constructor(
    public readonly dialogRef: MatDialogRef<CepDialogComponent>,
    telaDesktopService: TelaDesktopService,
    public readonly alertaService: AlertaService) {
    super(telaDesktopService);
  }

  ngOnInit() {
  }

  onNoClick(): void {
    //afazer: avisar que clicou em cancelar this.selecProdInfoPassado.ClicouOk = false;
    this.dialogRef.close(false);
  }

  onOkClick():void{
    //afazer: avisar que clicou em cancelar this.selecProdInfoPassado.ClicouOk = false;
    this.dialogRef.close(true);
  }

}
