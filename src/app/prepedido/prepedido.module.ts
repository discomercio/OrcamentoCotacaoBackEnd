import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PrepedidoRoutingModule } from './prepedido-routing.module';
import { ConsultaPrepedidoComponent } from './consulta-prepedido/consulta-prepedido.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule, MatInputModule, MatButtonModule, MatCardModule, MatRadioModule, MatAutocompleteModule, MatCheckbox, MatCheckboxModule, MatTableModule, MatSnackBarModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ListaPrepedidoComponent } from './lista-prepedido/lista-prepedido.component';
import { ConsultaBaseComponent } from './consulta-base/consulta-base.component';
import { ListaBaseComponent } from './lista-base/lista-base.component';
import { DetalhesPrepedidoComponent } from './detalhes-prepedido/detalhes-prepedido.component';


@NgModule({
  declarations: [ConsultaPrepedidoComponent, ListaPrepedidoComponent, ConsultaBaseComponent, ListaBaseComponent, DetalhesPrepedidoComponent],
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
    MatSnackBarModule,
    ReactiveFormsModule,
    PrepedidoRoutingModule
  ],
  exports: [ConsultaBaseComponent, ListaBaseComponent]
})
export class PrepedidoModule { }
