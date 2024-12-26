import { NgModule } from '@angular/core';

import { DistrictRoutingModule } from './district-routing.module';
import { DistrictComponent } from './district.component';
import { SharedModule } from '../shared/shared.module';
import { NzTableComponent, NzTableModule, NzThAddOnComponent, NzThMeasureDirective } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzModalContentDirective, NzModalModule } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputDirective } from 'ng-zorro-antd/input';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzFlexDirective } from 'ng-zorro-antd/flex';


@NgModule({
  declarations: [
    DistrictComponent
  ],
  imports: [
    SharedModule,
    DistrictRoutingModule,
    NzTableComponent,
    NzTableModule,
    NzThMeasureDirective,
    NzThAddOnComponent,
    NzButtonModule,
    NzModalModule,
    NzModalContentDirective,
    NzFormModule,
    NzInputDirective,
    NzPaginationModule,
    NzFlexDirective
  ]
})
export class DistrictModule { }
