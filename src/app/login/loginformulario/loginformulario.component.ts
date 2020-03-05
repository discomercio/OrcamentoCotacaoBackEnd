import { Component, OnInit } from '@angular/core';
import { TelaDesktopBaseComponent } from '../../../../src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from '../../../../src/app/servicos/telaDesktop/telaDesktop.service';
import { AutenticacaoService } from '../../../../src/app/servicos/autenticacao/autenticacao.service';
import { Router } from '@angular/router';
import { environment } from '../../../../src/environments/environment';
import { MatSnackBar } from '@angular/material';
import { AppComponent } from 'src/app/app.component';
import { Constantes } from 'src/app/dto/Constantes';
import { IfStmt } from '@angular/compiler';

@Component({
  selector: 'app-login',
  templateUrl: './loginformulario.component.html',
  styleUrls: ['./loginformulario.component.scss']
})
export class LoginformularioComponent extends TelaDesktopBaseComponent implements OnInit {


  constructor(telaDesktopService: TelaDesktopService,
    private readonly autenticacaoService: AutenticacaoService,
    private readonly _snackBar: MatSnackBar,
    private readonly router: Router,
    private readonly appComponent: AppComponent) {
    super(telaDesktopService);
  }

  fazendoLogin: boolean = false;


  usuario = "";
  senha = "";
  lembrar = !environment.autenticaStorageSession;

  ngOnInit() {
    this.appComponent.logo = null;
    if (this.autenticacaoService.authEstaLogado()) {
      this.router.navigateByUrl('/');
    }
  }

  constantes = new Constantes();
  login() {
    let msg = "";
    //document.getElementById("estilos").setAttribute('href', "assets/Unis.css");
    this.fazendoLogin = true;
    this.autenticacaoService.authLogin(this.usuario.trim(), this.senha.trim(), this.lembrar,
      () => { this.desligarFazendoLoginFOrmulario(); },
      this._snackBar,
      this.router, this.appComponent);
  }

  desligarFazendoLoginFOrmulario(): void {
    this.fazendoLogin = false;
  }

}
