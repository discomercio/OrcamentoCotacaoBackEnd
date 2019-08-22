import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home/home.component';
import { ConsultaPrepedidoComponent } from './prepedido/consulta-prepedido/consulta-prepedido.component';
import { ErroComponent } from './utils/erro/erro.component';
import { ListaPrepedidoComponent } from './prepedido/lista-prepedido/lista-prepedido.component';
import { ConsultaPedidoComponent } from './pedido/consulta-pedido/consulta-pedido.component';
import { ListaPedidoComponent } from './pedido/lista-pedido/lista-pedido.component';
import { LoginformularioComponent } from './login/loginformulario/loginformulario.component';


const routes: Routes = [
  /*
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  }
  */
  {
    path: 'login',
    component: LoginformularioComponent
  },
  {
    path: 'prepedido/consulta',
    component: ConsultaPrepedidoComponent
  },
  {
    path: 'prepedido/lista',
    component: ListaPrepedidoComponent
  },
  {
    path: 'pedido/consulta',
    component: ConsultaPedidoComponent
  },
  {
    path: 'pedido/lista',
    component: ListaPedidoComponent
  },
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'erro',
    component: ErroComponent
  },
  {
    path: '**',
    component: HomeComponent
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
