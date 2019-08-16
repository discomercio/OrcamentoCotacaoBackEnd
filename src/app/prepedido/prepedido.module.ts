import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PrepedidoRoutingModule } from './prepedido-routing.module';
import { ConsultaPrepedidoComponent } from './consulta-prepedido/consulta-prepedido.component';
import { FormsModule } from '@angular/forms';
import { MatIconModule, MatInputModule, MatButtonModule } from '@angular/material';


@NgModule({
  declarations: [ConsultaPrepedidoComponent],
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatInputModule, 
    MatIconModule,
    PrepedidoRoutingModule
  ]
})
export class PrepedidoModule { }
