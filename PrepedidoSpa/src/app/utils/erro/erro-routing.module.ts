import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ErroComponent } from './erro.component';


const routes: Routes = [
  {
    path: 'erro',
    component: ErroComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ErroRoutingModule { }
