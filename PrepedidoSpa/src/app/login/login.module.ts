import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LoginRoutingModule } from './login-routing.module';
import { MatButtonModule, MatInputModule, MatIconModule, MatSnackBarModule, MatCheckboxModule } from '@angular/material';
import { RouterModule } from '@angular/router';
import { LoginformularioComponent } from './loginformulario/loginformulario.component';
import { FormsModule } from '@angular/forms';
import { AlterarsenhaComponent } from './alterarsenha/alterarsenha.component';


@NgModule({
  declarations: [LoginformularioComponent, AlterarsenhaComponent], 
  imports: [
    FormsModule,
    MatButtonModule,
    MatInputModule, 
    MatIconModule,
    RouterModule,
    CommonModule,
    LoginRoutingModule,
    MatSnackBarModule,
    MatCheckboxModule
  ]
})
export class LoginModule { }
