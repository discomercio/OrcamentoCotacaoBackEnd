import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { ConsultaClienteComponent } from './consulta-cliente/consulta-cliente.component';
import { ConsultaPrepedidoComponent } from './consulta-prepedido/consulta-prepedido.component';
import { ConsultaPedidoComponent } from './consulta-pedido/consulta-pedido.component';
import { HomeComponent } from './home.component';


@NgModule({
  declarations: [ConsultaClienteComponent, ConsultaPrepedidoComponent, ConsultaPedidoComponent, HomeComponent],
  imports: [
    CommonModule,
    HomeRoutingModule
  ]
})
export class HomeModule { }
