import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PrepedidoRoutingModule } from './prepedido-routing.module';
import { ConsultaPrepedidoComponent } from './consulta-prepedido/consulta-prepedido.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule, MatInputModule, MatButtonModule, MatCardModule, MatRadioModule, MatAutocompleteModule, MatCheckbox, MatCheckboxModule, MatTableModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ListaPrepedidoComponent } from './lista-prepedido/lista-prepedido.component';
import { ConsultaBaseComponent } from './consulta-base/consulta-base.component';
import { ListaBaseComponent } from './lista-base/lista-base.component';


@NgModule({
  declarations: [ConsultaPrepedidoComponent, ListaPrepedidoComponent, ConsultaBaseComponent, ListaBaseComponent],
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatInputModule, 
    MatAutocompleteModule,
    MatIconModule,
    FlexLayoutModule,
    MatCardModule,
    MatRadioModule,
    MatCheckboxModule,
    MatTableModule,
    ReactiveFormsModule,
    PrepedidoRoutingModule
  ]
})
export class PrepedidoModule { }
