import { NgModule } from '@angular/core';

import { ProvinceRoutingModule } from './province-routing.module';
import { ProvinceComponent } from './province.component';
import { SharedModule } from '../shared/shared.module';
import { NzTableModule, NzThAddOnComponent, NzThMeasureDirective } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzModalContentDirective, NzModalModule, NzModalService } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputDirective } from 'ng-zorro-antd/input';

@NgModule({
  declarations: [
    ProvinceComponent
  ],
  imports: [
    SharedModule,
    ProvinceRoutingModule,
    NzTableModule,
    NzThMeasureDirective,
    NzThAddOnComponent,
    NzButtonModule,
    NzModalModule,
    NzModalContentDirective,
    NzFormModule,
    NzInputDirective
  ],
  providers: [
    NzModalService
  ]
})
export class ProvinceModule { }
