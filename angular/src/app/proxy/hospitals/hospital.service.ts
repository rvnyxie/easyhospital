import type { HospitalCreateDto, HospitalDto, HospitalPagedAndSortedResultRequestDto, HospitalUpdateDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HospitalService {
  apiName = 'Default';
  

  create = (input: HospitalCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HospitalDto>({
      method: 'POST',
      url: '/api/app/hospital',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/hospital/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HospitalDto>({
      method: 'GET',
      url: `/api/app/hospital/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: HospitalPagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<HospitalDto>>({
      method: 'GET',
      url: '/api/app/hospital',
      params: { provinceCode: input.provinceCode, districtCode: input.districtCode, communeCode: input.communeCode, search: input.search, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: HospitalUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HospitalDto>({
      method: 'PUT',
      url: `/api/app/hospital/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
