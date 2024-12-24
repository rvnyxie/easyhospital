import type { ProvinceCreateDto, ProvinceDto, ProvinceUpdateDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { LocalityPagedAndSortedResultRequestDto } from '../base/models';

@Injectable({
  providedIn: 'root',
})
export class ProvinceService {
  apiName = 'Default';
  

  create = (input: ProvinceCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProvinceDto>({
      method: 'POST',
      url: '/api/app/province',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/province/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProvinceDto>({
      method: 'GET',
      url: `/api/app/province/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: LocalityPagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ProvinceDto>>({
      method: 'GET',
      url: '/api/app/province',
      params: { search: input.search, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  importByInput = (input: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string[]>({
      method: 'POST',
      url: '/api/app/province/import',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: ProvinceUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProvinceDto>({
      method: 'PUT',
      url: `/api/app/province/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
