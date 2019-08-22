import { Component, OnInit } from '@angular/core';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { AutenticacaoService } from 'src/app/servicos/autenticacao/autenticacao.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-login',
  templateUrl: './loginformulario.component.html',
  styleUrls: ['./loginformulario.component.scss']
})
export class LoginformularioComponent extends TelaDesktopBaseComponent implements OnInit {

  constructor(telaDesktopService: TelaDesktopService,
    private autenticacaoService: AutenticacaoService,
    private _snackBar: MatSnackBar,
    private router: Router) {
    super(telaDesktopService);
  }

  usuario = "";
  senha = "";
  lembrar = !environment.autenticaStorageSession;

  ngOnInit() {
  }
  login() {
    const __this = this;
    this.autenticacaoService.authLogin(this.usuario, this.senha, this.lembrar).subscribe(
      {
        next(e) {
          __this.autenticacaoService.setarToken(e);
          __this.router.navigate(['/']);
        }
        ,
        error(e) {
          let msg = "" + ((e && e.message) ? e.message : e.toString());
          if ((e && e.status) == 400)
            msg = "usuário e senha inválidos."
          if ((e && e.status) == 0)
            msg = "servidor de autenticação não disponível."
          if ((e && e.status) == 500)
            msg = "erro interno no servidor de autenticação."


          __this._snackBar.open("Erro no login: " + msg, undefined, {
            duration: environment.esperaErros
          });
        },

      }
    );
  }

}
