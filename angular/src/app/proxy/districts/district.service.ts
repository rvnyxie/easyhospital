import type { DistrictCreateDto, DistrictDto, DistrictUpdateDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { LocalityPagedAndSortedResultRequestDto } from '../base/models';

@Injectable({
  providedIn: 'root',
})
export class DistrictService {
  apiName = 'Default';
  

  create = (input: DistrictCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DistrictDto>({
      method: 'POST',
      url: '/api/app/district',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/district/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DistrictDto>({
      method: 'GET',
      url: `/api/app/district/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: LocalityPagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<DistrictDto>>({
      method: 'GET',
      url: '/api/app/district',
      params: { provinceCode: input.provinceCode, districtCode: input.districtCode, communeCode: input.communeCode, search: input.search, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  importByInput = (input: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string[]>({
      method: 'POST',
      url: '/api/app/district/import',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: DistrictUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DistrictDto>({
      method: 'PUT',
      url: `/api/app/district/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
