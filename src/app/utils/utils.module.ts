import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ErroModule } from './erro/erro.module';
import { UtilsRoutingModule } from './utils-routing.module';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    UtilsRoutingModule,
    ErroModule
  ]
})
export class UtilsModule { }
