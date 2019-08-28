import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ClienteRoutingModule } from './cliente-routing.module';
import { ClienteComponent } from './cliente/cliente.component';
import { ClienteCorpoComponent } from './cliente-corpo/cliente-corpo.component';
import { MatButtonModule, MatIconModule } from '@angular/material';


@NgModule({
  declarations: [ClienteComponent, ClienteCorpoComponent],
  imports: [
    CommonModule,
    ClienteRoutingModule,
    MatButtonModule,
    MatIconModule
  ]
})
export class ClienteModule { }
