import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ErroRoutingModule } from './erro-routing.module';
import { ErroComponent } from './erro.component';

import { MatButtonModule } from '@angular/material';
import {MatIconModule} from '@angular/material/icon';

@NgModule({
  declarations: [ErroComponent],
  imports: [
    CommonModule,
    MatIconModule,
    MatButtonModule,
    ErroRoutingModule
  ]
})
export class ErroModule { }
