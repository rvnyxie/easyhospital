import { NgModule } from '@angular/core';

import { UserHospitalRoutingModule } from './user-hospital-routing.module';
import { UserHospitalComponent } from './user-hospital.component';
import { SharedModule } from '../shared/shared.module';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputDirective } from 'ng-zorro-antd/input';
import { NzModalContentDirective, NzModalModule } from 'ng-zorro-antd/modal';
import { NzPaginationComponent } from 'ng-zorro-antd/pagination';
import { NzTableComponent, NzTableModule, NzThAddOnComponent, NzThMeasureDirective } from 'ng-zorro-antd/table';
import { NzSelectModule } from 'ng-zorro-antd/select';


@NgModule({
  declarations: [
    UserHospitalComponent
  ],
  imports: [
    SharedModule,
    UserHospitalRoutingModule,
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
    NzSelectModule
  ]
})
export class UserHospitalModule { }
