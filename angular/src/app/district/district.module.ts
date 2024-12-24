import { NgModule } from '@angular/core';

import { DistrictRoutingModule } from './district-routing.module';
import { DistrictComponent } from './district.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    DistrictComponent
  ],
  imports: [
    SharedModule,
    DistrictRoutingModule
  ]
})
export class DistrictModule { }
