import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ErroRoutingModule } from './erro-routing.module';
import { ErroComponent } from './erro.component';

import { MatButtonModule, MatInputModule, MatIconModule, MatSnackBarModule } from '@angular/material';

@NgModule({
  declarations: [ErroComponent],
  imports: [
    CommonModule,
    MatIconModule,
    MatButtonModule,
    ErroRoutingModule
  ]
})
export class ErroModule { }
