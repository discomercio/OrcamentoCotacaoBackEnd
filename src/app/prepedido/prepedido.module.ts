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
import { PedidoDesktopComponent } from './detalhes-prepedido/pedido-desktop/pedido-desktop.component';
import { PedidoCelularComponent } from './detalhes-prepedido/pedido-celular/pedido-celular.component';
import { PrepedidoDesktopComponent } from './detalhes-prepedido/prepedido-desktop/prepedido-desktop.component';
import { PrepedidoCelularComponent } from './detalhes-prepedido/prepedido-celular/prepedido-celular.component';
import { NovoPrepedidoModule } from './novo-prepedido/novo-prepedido.module';
import { ClienteCorpoModule } from '../cliente/cliente-corpo/cliente-corpo.module';

@NgModule({
  declarations: [ConsultaPrepedidoComponent, ListaPrepedidoComponent, ConsultaBaseComponent, ListaBaseComponent, DetalhesPrepedidoComponent,
    PedidoDesktopComponent, PedidoCelularComponent, PrepedidoDesktopComponent, PrepedidoCelularComponent],
  imports: [
    ClienteCorpoModule,
    NovoPrepedidoModule,
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
