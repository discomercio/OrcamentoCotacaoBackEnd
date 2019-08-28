import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { TelaDesktopBaseComponent } from '../../../../src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from '../../../../src/app/servicos/telaDesktop/telaDesktop.service';

@Component({
  selector: 'app-cliente',
  templateUrl: './cliente.component.html',
  styleUrls: ['./cliente.component.scss']
})
export class ClienteComponent extends TelaDesktopBaseComponent implements OnInit {

  cpfcnpj: string;
  constructor(private readonly router: Router,
    private readonly location: Location,
    telaDesktopService: TelaDesktopService,
    public readonly activatedRoute: ActivatedRoute) { 
      super(telaDesktopService);
    }

  
  ngOnInit() {
    const cpfcnpj = this.activatedRoute.snapshot.params.cpfcnpj;
    if (!!cpfcnpj) {
      this.cpfcnpj = cpfcnpj;
    }
  }

  voltar() {
    this.location.back();
  }

}
