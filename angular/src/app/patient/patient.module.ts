import { NgModule } from '@angular/core';

import { PatientRoutingModule } from './patient-routing.module';
import { PatientComponent } from './patient.component';
import { SharedModule } from '../shared/shared.module';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputDirective } from 'ng-zorro-antd/input';
import { NzModalContentDirective, NzModalModule } from 'ng-zorro-antd/modal';
import { NzTableComponent, NzTableModule, NzThAddOnComponent, NzThMeasureDirective } from 'ng-zorro-antd/table';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzPaginationComponent } from 'ng-zorro-antd/pagination';


@NgModule({
  declarations: [
    PatientComponent
  ],
  imports: [
    SharedModule,
    PatientRoutingModule,
    NzTableComponent,
    NzTableModule,
    NzThMeasureDirective,
    NzThAddOnComponent,
    NzButtonModule,
    NzModalModule,
    NzModalContentDirective,
    NzFormModule,
    NzInputDirective,
    NzPaginationComponent,
    NzSelectModule,
  ]
})
export class PatientModule { }
