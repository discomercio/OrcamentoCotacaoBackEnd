import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ErroModule } from './erro/erro.module';
import { UtilsRoutingModule } from './utils-routing.module';
import { MatDialog, MatDialogModule } from '@angular/material';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    MatDialogModule,
    UtilsRoutingModule,
    ErroModule
  ]
})
export class UtilsModule { }
