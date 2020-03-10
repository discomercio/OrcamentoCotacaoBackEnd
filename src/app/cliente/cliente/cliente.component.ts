import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { TelaDesktopBaseComponent } from '../../../../src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from '../../../../src/app/servicos/telaDesktop/telaDesktop.service';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { BuscarClienteService } from 'src/app/servicos/cliente/buscar-cliente.service';
import { MatDialog } from '@angular/material';
import { StringUtils } from 'src/app/utils/stringUtils';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { ClienteCadastroDto } from 'src/app/dto/ClienteCadastro/ClienteCadastroDto';

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
    public readonly alertaService: AlertaService,
    public readonly activatedRoute: ActivatedRoute) {
    super(telaDesktopService);
  }


  ngOnInit() {
    const cpfcnpj = StringUtils.retorna_so_digitos(this.activatedRoute.snapshot.params.cpfcnpj);
    this.buscarClienteService.buscar(cpfcnpj).toPromise()
      .then((r) => {
        if (r === null) {
          //erro...
          this.alertaService.mostrarErroInternet(r);
          return;
        }
        //cliente já existe
        this.dadosClienteCadastroDto = r.DadosCliente;
        this.clienteCadastroDto = r;
      }).catch((r) => {
        debugger;
        //erro...
        this.alertaService.mostrarErroInternet(r);
      });
  }

  voltar() {
    this.location.back();

  }

  dadosClienteCadastroDto = new DadosClienteCadastroDto();
  clienteCadastroDto = new ClienteCadastroDto();
}
