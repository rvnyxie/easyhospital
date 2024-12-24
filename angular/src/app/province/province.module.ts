import { NgModule } from '@angular/core';

import { ProvinceRoutingModule } from './province-routing.module';
import { ProvinceComponent } from './province.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    ProvinceComponent
  ],
  imports: [
    SharedModule,
    ProvinceRoutingModule
  ]
})
export class ProvinceModule { }
