import { NgModule } from '@angular/core';

import { HospitalRoutingModule } from './hospital-routing.module';
import { HospitalComponent } from './hospital.component';
import { SharedModule } from '../shared/shared.module';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputDirective } from 'ng-zorro-antd/input';
import { NzModalContentDirective, NzModalModule } from 'ng-zorro-antd/modal';
import { NzPaginationComponent } from 'ng-zorro-antd/pagination';
import { NzTableComponent, NzTableModule, NzThAddOnComponent, NzThMeasureDirective } from 'ng-zorro-antd/table';


@NgModule({
  declarations: [
    HospitalComponent
  ],
  imports: [
    SharedModule,
    HospitalRoutingModule,
    NzTableComponent,
    NzTableModule,
    NzThMeasureDirective,
    NzThAddOnComponent,
    NzButtonModule,
    NzModalModule,
    NzModalContentDirective,
    NzFormModule,
    NzInputDirective,
    NzPaginationComponent
  ]
})
export class HospitalModule { }
