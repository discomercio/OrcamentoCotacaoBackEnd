import { Component, OnInit } from '@angular/core';
import { TelaDesktopBaseComponent } from '../../../../src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from '../../../../src/app/servicos/telaDesktop/telaDesktop.service';
import { AutenticacaoService } from '../../../../src/app/servicos/autenticacao/autenticacao.service';
import { Router } from '@angular/router';
import { environment } from '../../../../src/environments/environment';
import { MatSnackBar } from '@angular/material';
import { AppComponent } from 'src/app/app.component';

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
  login() {
    //document.getElementById("estilos").setAttribute('href', "assets/Unis.css");
    this.fazendoLogin = true;
    this.autenticacaoService.authLogin(this.usuario.trim(), this.senha.trim(), this.lembrar).subscribe(
      {
        next: (e) => {
          //nunca desligamos o fazendo login porque, quando fizer, já vai para outra página, então nao fazemos this.fazendoLogin = false;
          this.autenticacaoService.setarToken(e);

          //o carregarEstilo já navega para a home
          this.appComponent.carregarEstilo(true);
        }
        ,
        error: (e) => {
          this.fazendoLogin = false;
          let msg = "" + ((e && e.message) ? e.message : e.toString());
          if (e && e.status === 400)
            msg = "usuário e senha inválidos."
          if (e && e.status === 0)
            msg = "servidor de autenticação não disponível."
          if (e && e.status === 500)
            msg = "erro interno no servidor de autenticação."


          this._snackBar.open("Erro no login: " + msg, undefined, {
            duration: environment.esperaErros
          });
        },

      }
    );
  }

}
