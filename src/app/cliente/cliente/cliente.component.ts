import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { TelaDesktopBaseComponent } from '../../../../src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from '../../../../src/app/servicos/telaDesktop/telaDesktop.service';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { BuscarClienteService } from 'src/app/servicos/cliente/buscar-cliente.service';
import { AlertDialogComponent } from 'src/app/utils/alert-dialog/alert-dialog.component';
import { MatDialog } from '@angular/material';
import { StringUtils } from 'src/app/utils/stringUtils';

@Component({
  selector: 'app-cliente',
  templateUrl: './cliente.component.html',
  styleUrls: ['./cliente.component.scss']
})
export class ClienteComponent extends TelaDesktopBaseComponent implements OnInit {

  constructor(private readonly router: Router,
    private readonly location: Location,
    telaDesktopService: TelaDesktopService,
    public readonly buscarClienteService: BuscarClienteService,
    public readonly dialog: MatDialog,
    public readonly activatedRoute: ActivatedRoute) {
    super(telaDesktopService);
  }


  ngOnInit() {
    const cpfcnpj = StringUtils.retorna_so_digitos(this.activatedRoute.snapshot.params.cpfcnpj);
    this.buscarClienteService.buscar(cpfcnpj).toPromise()
      .then((r) => {
        if (r === null) {
          //erro...
          const dialogRef = this.dialog.open(AlertDialogComponent, {
            width: '350px',
            data: `Ocorreu um erro ao acessar os dados. Verifique a conexão com a Internet.`
          });
          return;
        }
        //cliente já existe
        this.dadosClienteCadastroDto = r;
      }).catch((r) => {
        //erro...
        const dialogRef = this.dialog.open(AlertDialogComponent, {
          width: '350px',
          data: `Ocorreu um erro ao acessar os dados. Verifique a conexão com a Internet.`
        });
      });
  }

  voltar() {
    this.location.back();
  }

  dadosClienteCadastroDto = new DadosClienteCadastroDto();

}
